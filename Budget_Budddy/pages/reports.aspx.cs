using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Budget_Budddy.pages
{
    public partial class reports : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect if the session has expired.
            if (Session["username"] == null)
            {
                Response.Redirect("../index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            litUsername.Text = Session["username"].ToString();

            if (!IsPostBack)
            {
                LoadExpenses();
            }
        }

        // Load expense data from the database, bind it to the GridView,
        // and serialize a subset of the data into the hidden field for chart rendering.
        private void LoadExpenses()
        {
            try
            {
                int userID = GetUserID(Session["username"].ToString());
                DataTable dt = new DataTable();
                List<dynamic> chartData = new List<dynamic>();

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

                // Bind the DataTable to the GridView.
                gvExpenses.DataSource = dt;
                gvExpenses.DataBind();

                // Prepare chart data using only the Category and Amount fields.
                foreach (DataRow row in dt.Rows)
                {
                    chartData.Add(new
                    {
                        category = row["Category"].ToString(),
                        amount = row["Amount"]
                    });
                }

                // Serialize the chart data to JSON.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                hiddenExpenseData.Value = serializer.Serialize(chartData);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error loading expenses: " + ex.Message + "');", true);
            }
        }

        // Export report as PDF.
        protected void btnExportPDF_Click(object sender, EventArgs e)
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

            // Retrieve chart image data from the hidden field.
            string chartImageData = hiddenChartImage.Value;

            using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(document, msOutput);
                document.Open();

                Paragraph title = new Paragraph("Expense Report",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                document.Add(title);

                // If chart image exists, add it.
                if (!string.IsNullOrEmpty(chartImageData))
                {
                    int commaIndex = chartImageData.IndexOf(",");
                    string base64Data = chartImageData.Substring(commaIndex + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                    using (System.IO.MemoryStream msImage = new System.IO.MemoryStream(imageBytes))
                    {
                        iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(msImage);
                        chartImage.ScaleToFit(400f, 400f);
                        chartImage.Alignment = Element.ALIGN_CENTER;
                        chartImage.SpacingAfter = 20f;
                        document.Add(chartImage);
                    }
                }

                // Create a table for expense data.
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 10f;
                table.SetWidths(new float[] { 1f, 3f, 2f, 4f, 2f });

                var headerFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
                PdfPCell cell = new PdfPCell(new Phrase("ID", headerFont));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Category", headerFont));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Amount", headerFont));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Description", headerFont));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expense Date", headerFont));
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                table.AddCell(cell);

                var cellFont = new Font(Font.FontFamily.HELVETICA, 10);
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
            using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage())
            {
                OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Category";
                worksheet.Cells["C1"].Value = "Amount";
                worksheet.Cells["D1"].Value = "Description";
                worksheet.Cells["E1"].Value = "Expense Date";

                using (var range = worksheet.Cells["A1:E1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

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

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Insert chart image if available.
                string chartImageData = hiddenChartImage.Value;
                if (!string.IsNullOrEmpty(chartImageData))
                {
                    int commaIndex = chartImageData.IndexOf(",");
                    string base64Data = chartImageData.Substring(commaIndex + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        var picture = worksheet.Drawings.AddPicture("ChartImage", img);
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Redirect("../index.aspx", true);
        }
    }
}
