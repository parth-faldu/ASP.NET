using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;

namespace Budget_Budddy.pages
{
    public partial class dashboard : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        /*private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return text;
                return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }*/

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("../index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            else
            {
                litUsername.Text = Session["username"].ToString();
                if (!IsPostBack)
                {
                    BindExpensesGrid();
                    LoadExpenses();

                    int userID = GetUserID(Session["username"].ToString());
                    decimal totalExpenses = GetTotalExpenses(userID);

                    if (totalExpenses == 0)
                    {
                        budgetSuggestionsLiteral.Text = "No expenses found. Please add some expenses to see a budget suggestion.";
                    }
                    else
                    {
                        Dictionary<string, decimal> categoryTotals = GetCategoryTotals(userID);
                        string rawSuggestion = await GetBudgetSuggestionGeminiAsync(totalExpenses, categoryTotals);
                        //rawSuggestion = TruncateText(rawSuggestion, 400); // if needed
                        string formattedSuggestion = FormatSuggestionUI(rawSuggestion);
                        budgetSuggestionsLiteral.Text = formattedSuggestion;
                    }
                }
            }
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database Connection Successful!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Database Connection Failed: " + ex.Message);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Redirect("../index.aspx", true);
        }

        // Load expenses from the database and serialize as JSON for the chart.
        private void LoadExpenses()
        {
            try
            {
                int userID = GetUserID(Session["username"].ToString());
                List<dynamic> expenseData = new List<dynamic>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, Category, Amount, Description, ExpenseDate FROM Expenses WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                expenseData.Add(new
                                {
                                    id = Convert.ToInt32(reader["ID"]),
                                    category = reader["Category"].ToString(),
                                    amount = Convert.ToDecimal(reader["Amount"]),
                                    description = reader["Description"].ToString(),
                                    date = Convert.ToDateTime(reader["ExpenseDate"]).ToString("yyyy-MM-dd")
                                });
                            }
                        }
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                hiddenExpenseData.Value = serializer.Serialize(expenseData);
                // Trigger a chart update on the client side.
                ClientScript.RegisterStartupScript(this.GetType(), "updateChart", "setTimeout(updateChart, 1000);", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error loading expenses: {ex.Message}');", true);
            }
        }

        private int GetUserID(string username)
        {
            int userID = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id FROM users WHERE username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        userID = Convert.ToInt32(result);
                }
            }
            return userID;
        }

        // Calculate the total expenses for a given user.
        private decimal GetTotalExpenses(int userID)
        {
            decimal total = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(SUM(Amount), 0) FROM Expenses WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }
            return total;
        }

        // Add expense with duplicate-check and PRG pattern.
        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            try
            {
                // Server-side check: do not allow negative amount.
                decimal amountValue = Convert.ToDecimal(txtAmount.Text);
                if (amountValue < 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Negative amounts are not allowed.');", true);
                    return;
                }

                int userID = GetUserID(Session["username"].ToString());
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Check for duplicate expense.
                    string duplicateQuery = "SELECT COUNT(*) FROM Expenses WHERE UserID = @UserID AND Category = @Category AND Amount = @Amount AND Description = @Description AND ExpenseDate = @ExpenseDate";
                    using (SqlCommand duplicateCmd = new SqlCommand(duplicateQuery, conn))
                    {
                        duplicateCmd.Parameters.AddWithValue("@UserID", userID);
                        duplicateCmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                        duplicateCmd.Parameters.AddWithValue("@Amount", amountValue);
                        duplicateCmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        duplicateCmd.Parameters.AddWithValue("@ExpenseDate", Convert.ToDateTime(txtDate.Text));
                        int duplicateCount = (int)duplicateCmd.ExecuteScalar();
                        if (duplicateCount > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Duplicate expense exists.');", true);
                            return;
                        }
                    }
                    string query = "INSERT INTO Expenses (UserID, Category, Amount, Description, ExpenseDate) VALUES (@UserID, @Category, @Amount, @Description, @ExpenseDate)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                        cmd.Parameters.AddWithValue("@Amount", amountValue);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@ExpenseDate", Convert.ToDateTime(txtDate.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                ClearInputFields();
                BindExpensesGrid();
                LoadExpenses();
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error adding expense: {ex.Message}');", true);
            }
        }

        // -------------------------
        // GridView Binding and Events
        // -------------------------
        private void BindExpensesGrid()
        {
            try
            {
                int userID = GetUserID(Session["username"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, Category, Amount, Description, ExpenseDate FROM Expenses WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                gvExpenses.DataSource = dt;
                gvExpenses.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error loading expenses grid: {ex.Message}');", true);
            }
        }

        protected void gvExpenses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvExpenses.EditIndex = e.NewEditIndex;
            BindExpensesGrid();
        }

        protected void gvExpenses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int expenseID = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value);
            var row = gvExpenses.Rows[e.RowIndex];

            // Retrieve updated values from the GridView cells.
            string category = ((System.Web.UI.WebControls.TextBox)(row.Cells[1].Controls[0])).Text;
            string amountText = ((System.Web.UI.WebControls.TextBox)(row.Cells[2].Controls[0])).Text;
            string description = ((System.Web.UI.WebControls.TextBox)(row.Cells[3].Controls[0])).Text;
            string dateText = ((System.Web.UI.WebControls.TextBox)(row.Cells[4].Controls[0])).Text;

            if (!decimal.TryParse(amountText, out decimal amount) || !DateTime.TryParse(dateText, out DateTime expenseDate))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid input.');", true);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Expenses SET Category = @Category, Amount = @Amount, Description = @Description, ExpenseDate = @ExpenseDate WHERE ID = @ExpenseID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ExpenseDate", expenseDate);
                        cmd.Parameters.AddWithValue("@ExpenseID", expenseID);
                        cmd.ExecuteNonQuery();
                    }
                }
                gvExpenses.EditIndex = -1;
                BindExpensesGrid();
                LoadExpenses(); // Update chart data.
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Expense updated successfully.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error updating expense: {ex.Message}');", true);
            }
        }

        protected void gvExpenses_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvExpenses.EditIndex = -1;
            BindExpensesGrid();
        }

        protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int expenseID = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM Expenses WHERE ID = @ExpenseID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExpenseID", expenseID);
                        cmd.ExecuteNonQuery();
                    }
                }
                BindExpensesGrid();
                LoadExpenses(); // Update chart data.
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error deleting expense: {ex.Message}');", true);
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            // For example, redirect to a separate Update Profile page.
            Response.Redirect("updateprofile.aspx");
            // Alternatively, you could display a modal panel on this page.
        }

        protected void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] != null)
                {
                    string username = Session["username"].ToString();
                    int userID = GetUserID(username); // Reuse the existing method to get user ID

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Delete user's expenses first (assuming foreign key constraints exist)
                        string deleteExpensesQuery = "DELETE FROM Expenses WHERE UserID = @UserID";
                        using (SqlCommand deleteExpensesCmd = new SqlCommand(deleteExpensesQuery, conn))
                        {
                            deleteExpensesCmd.Parameters.AddWithValue("@UserID", userID);
                            deleteExpensesCmd.ExecuteNonQuery();
                        }

                        // Delete user account
                        string deleteUserQuery = "DELETE FROM Users WHERE ID = @UserID";
                        using (SqlCommand deleteUserCmd = new SqlCommand(deleteUserQuery, conn))
                        {
                            deleteUserCmd.Parameters.AddWithValue("@UserID", userID);
                            deleteUserCmd.ExecuteNonQuery();
                        }
                    }

                    // Clear session and redirect to login page
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("../index.aspx", true);
                }
                else
                {
                    // If session is null, redirect to login page
                    Response.Redirect("../index.aspx", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error deleting account: {ex.Message}');", true);
            }
        }

        private void ClearInputFields()
        {
            txtCategory.Text = "";
            txtAmount.Text = "";
            txtDescription.Text = "";
            txtDate.Text = "";
        }

        // -------------------------
        // Export Functionality
        // -------------------------

        // Export report as PDF.
        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            // Retrieve expense data for the table.
            int userID = GetUserID(Session["username"].ToString());
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Category, Amount, Description, ExpenseDate FROM Expenses WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            // Get the chart image from the hidden field.
            string chartImageData = hiddenChartImage.Value;

            using (MemoryStream msOutput = new MemoryStream())
            {
                // Create a document with margins (left, right, top, bottom).
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(document, msOutput);
                document.Open();

                // Add a title.
                Paragraph title = new Paragraph("Expense Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                document.Add(title);

                // If a chart image exists, add it.
                if (!string.IsNullOrEmpty(chartImageData))
                {
                    int commaIndex = chartImageData.IndexOf(",");
                    string base64Data = chartImageData.Substring(commaIndex + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    using (MemoryStream msImage = new MemoryStream(imageBytes))
                    {
                        iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(msImage);
                        chartImage.ScaleToFit(400f, 400f);
                        chartImage.Alignment = Element.ALIGN_CENTER;
                        chartImage.SpacingAfter = 20f;
                        document.Add(chartImage);
                    }
                }

                // Create a table with 5 columns.
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 10f;
                table.SetWidths(new float[] { 1f, 3f, 2f, 4f, 2f });

                // Define header font.
                var headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);

                // Add table headers.
                PdfPCell cell = new PdfPCell(new Phrase("ID", headerFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Category", headerFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount", headerFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Description", headerFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expense Date", headerFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(200, 200, 200);
                table.AddCell(cell);

                // Define a regular font.
                var cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);

                // Add data rows.
                foreach (DataRow dr in dt.Rows)
                {
                    table.AddCell(new Phrase(dr["ID"].ToString(), cellFont));
                    table.AddCell(new Phrase(dr["Category"].ToString(), cellFont));
                    table.AddCell(new Phrase(String.Format("{0:C}", dr["Amount"]), cellFont));
                    table.AddCell(new Phrase(dr["Description"].ToString(), cellFont));
                    DateTime expDate = Convert.ToDateTime(dr["ExpenseDate"]);
                    table.AddCell(new Phrase(expDate.ToString("yyyy-MM-dd"), cellFont));
                }

                document.Add(table);
                document.Close();
                writer.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
                Response.BinaryWrite(msOutput.ToArray());
                Response.End();
            }
        }

        // Export report as Excel.
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                // Define headers.
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Category";
                worksheet.Cells["C1"].Value = "Amount";
                worksheet.Cells["D1"].Value = "Description";
                worksheet.Cells["E1"].Value = "Expense Date";

                // Format header row.
                using (var range = worksheet.Cells["A1:E1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Retrieve expense data.
                int userID = GetUserID(Session["username"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, Category, Amount, Description, ExpenseDate FROM Expenses WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                // Populate the worksheet with data.
                int rowIndex = 2;
                foreach (DataRow row in dt.Rows)
                {
                    worksheet.Cells["A" + rowIndex].Value = row["ID"];
                    worksheet.Cells["B" + rowIndex].Value = row["Category"];
                    worksheet.Cells["C" + rowIndex].Value = row["Amount"];
                    worksheet.Cells["D" + rowIndex].Value = row["Description"];
                    worksheet.Cells["E" + rowIndex].Value = Convert.ToDateTime(row["ExpenseDate"]).ToString("yyyy-MM-dd");
                    rowIndex++;
                }

                // Auto-fit columns.
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Insert the chart image if available.
                string chartImageData = hiddenChartImage.Value;
                if (!string.IsNullOrEmpty(chartImageData))
                {
                    int commaIndex = chartImageData.IndexOf(",");
                    string base64Data = chartImageData.Substring(commaIndex + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        var picture = worksheet.Drawings.AddPicture("ChartImage", img);
                        // Position the chart image below the table.
                        picture.SetPosition(rowIndex + 1, 0, 0, 0);
                        picture.SetSize(400, 400);
                    }
                }

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=Report.xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
            }
        }
        private Dictionary<string, decimal> GetCategoryTotals(int userID)
        {
            var categoryTotals = new Dictionary<string, decimal>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Category, SUM(Amount) as Total FROM Expenses WHERE UserID = @UserID GROUP BY Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader["Category"].ToString();
                            decimal total = Convert.ToDecimal(reader["Total"]);
                            categoryTotals[category] = total;
                        }
                    }
                }
            }
            return categoryTotals;
        }
        private string FormatSuggestionUI(string rawSuggestion)
        {
            // Replace markers for headings with HTML headings.
            string formatted = rawSuggestion;
            formatted = formatted.Replace("**Area of Overspending:**", "</p><h3 style='color:#007ACC;'>Area of Overspending</h3><p>");
            formatted = formatted.Replace("**Actionable Recommendations:**", "</p><h3 style='color:#007ACC;'>Actionable Recommendations</h3>");

            // Convert markdown bold markers (**text**) into <strong> tags.
            formatted = System.Text.RegularExpressions.Regex.Replace(formatted, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

            // Now, extract and rebuild the recommendations as an ordered list.
            // We assume that the recommendations start immediately after the closing </h3> of the "Actionable Recommendations" heading.
            string recHeading = "</h3>";
            int recIndex = formatted.IndexOf(recHeading);
            if (recIndex != -1)
            {
                // The recommendations text starts after the heading.
                int start = recIndex + recHeading.Length;
                string recText = formatted.Substring(start).Trim();

                // Use regex to capture the recommendation items.
                // This pattern matches a number followed by a period and whitespace, then captures the text until the next numbered item or end-of-string.
                var matches = System.Text.RegularExpressions.Regex.Matches(recText, @"\d+\.\s+(.*?)(?=\d+\.\s+|$)", System.Text.RegularExpressions.RegexOptions.Singleline);

                if (matches.Count > 0)
                {
                    StringBuilder listBuilder = new StringBuilder("<ol>");
                    foreach (System.Text.RegularExpressions.Match match in matches)
                    {
                        // match.Groups[1] contains the text after the number and period.
                        string itemText = match.Groups[1].Value.Trim();
                        listBuilder.Append("<li>" + itemText + "</li>");
                    }
                    listBuilder.Append("</ol>");

                    // Replace the original recommendations text with the formatted ordered list.
                    formatted = formatted.Substring(0, start) + listBuilder.ToString();
                }
            }

            // Wrap the whole output in a div with basic styling.
            formatted = "<div style='font-family:Arial, sans-serif; color:#cecece; line-height:1.5;'>" + formatted + "</div>";

            return formatted;
        }

        // -------------------------
        // AI API Integration for Budget Suggestions
        // -------------------------
        /// <summary>
        /// Calls the AI API (OpenAI in this example) to generate a budget suggestion.
        /// </summary>
        /// <param name="availableBudget">The total available budget.</param>
        /// <param name="totalExpenses">The sum of all recorded expenses.</param>
        /// <returns>A string containing the suggestion.</returns>
        private async Task<string> GetBudgetSuggestionGeminiAsync(decimal totalExpenses, Dictionary<string, decimal> categoryTotals)
        {
            // Build a summary string for category spending.
            var categorySummary = new StringBuilder();
            foreach (var entry in categoryTotals)
            {
                // Format each category and its total (e.g., "Groceries: $150.00").
                categorySummary.Append($"{entry.Key}: {entry.Value:C}, ");
            }
            if (categorySummary.Length > 2)
            {
                categorySummary.Length -= 2; // Remove trailing comma and space.
            }

            // Construct the prompt solely based on the user's expense data.
            string prompt = $"I have been tracking my expenses meticulously. " +
                $"I've spent a total of {totalExpenses:C}. " +
                $"Here is the breakdown of my spending by category: {categorySummary}. " +
                "Based solely on this data, please provide a concise (only under 400 words) and actionable recommendation " +
                "to help optimize my spending habits. Highlight any areas of overspending and suggest steps to better manage my expenses.";

            // Prepare the request payload.
            var requestBody = new
            {
                contents = new[]
                {
            new { parts = new[] { new { text = prompt } } }
        },
            };

            // Retrieve your Gemini API key from configuration.
            string apiKey = ConfigurationManager.AppSettings["Gemini_API_Key"];
            string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Gemini API response: " + responseString);
                    dynamic result = JsonConvert.DeserializeObject(responseString);

                    if (result != null && result.candidates != null && result.candidates.Count > 0)
                    {
                        var candidate = result.candidates[0];
                        if (candidate.content != null &&
                            candidate.content.parts != null &&
                            candidate.content.parts.Count > 0 &&
                            candidate.content.parts[0].text != null)
                        {
                            return candidate.content.parts[0].text.ToString().Trim();
                        }
                        else
                        {
                            return $"Candidate content structure is not as expected. Raw response: {responseString}";
                        }
                    }
                    else
                    {
                        return $"No candidates found in API response. Raw response: {responseString}";
                    }
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    return $"Unable to generate a suggestion due to an error with the Gemini API. Status Code: {response.StatusCode}. Details: {errorDetails}";
                }
            }
        }




    }
}
