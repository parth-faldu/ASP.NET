using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Budget_Budddy.pages
{
    public partial class budgetSuggestion : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected async void Page_Load(object sender, EventArgs e)
        {
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
                budgetSuggestionsLiteral.Visible = false;
            }
        }

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
                // Trigger a client-side update (for example, updating charts)
                ClientScript.RegisterStartupScript(this.GetType(), "updateChart", "updateChart();", true);
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

        protected void lnkGetBudgetSuggestion_Click(object sender, EventArgs e)
        {
            Page.RegisterAsyncTask(new PageAsyncTask(ProcessBudgetSuggestionAsync));
        }

        private async Task ProcessBudgetSuggestionAsync()
        {
            await Task.Delay(3000);
            int userID = GetUserID(Session["username"].ToString());
            decimal totalExpenses = GetTotalExpenses(userID);

            decimal targetBudget = 0;
            if (!decimal.TryParse(txtNextYearBudget.Text.Trim(), out targetBudget) || targetBudget <= 0)
            {
                budgetSuggestionsLiteral.Text = "Please enter a valid target budget for next year.";
                budgetSuggestionsLiteral.Visible = true;
                return;
            }

            if (totalExpenses == 0)
            {
                budgetSuggestionsLiteral.Text = "No expenses found. Please add some expenses to see a budget suggestion.";
            }
            else
            {
                Dictionary<string, decimal> categoryTotals = GetCategoryTotals(userID);
                string rawSuggestion = await GetBudgetSuggestionGeminiAsync(totalExpenses, categoryTotals, targetBudget);

                if (string.IsNullOrWhiteSpace(rawSuggestion))
                {
                    budgetSuggestionsLiteral.Text = "API returned an empty response.";
                }
                else
                {
                    string formattedSuggestion = FormatSuggestionUI(rawSuggestion);
                    budgetSuggestionsLiteral.Text = formattedSuggestion;
                }
            }
            budgetSuggestionsLiteral.Visible = true;
        }


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

        private async Task<string> GetBudgetSuggestionGeminiAsync(decimal totalExpenses, Dictionary<string, decimal> categoryTotals, decimal targetBudget)
        {
            // Build a summary string for category spending.
            var categorySummary = new StringBuilder();
            foreach (var entry in categoryTotals)
            {
                categorySummary.Append($"{entry.Key}: {entry.Value:C}, ");
            }
            if (categorySummary.Length > 2)
            {
                categorySummary.Length -= 2; // Remove trailing comma and space.
            }

            // Construct the prompt including the target budget.
            string prompt = $"I have been tracking my expenses meticulously. " +
                $"I've spent a total of {totalExpenses:C}. " +
                $"Here is the breakdown of my spending by category: {categorySummary}. " +
                $"I am planning for next year with a target budget of {targetBudget:C}. " +
                "Based solely on this data, please provide a concise (under 400 words) and actionable recommendation " +
                "on how to optimize my spending habits for next year. Highlight any areas of overspending and suggest steps " +
                "to better manage my expenses to meet the new budget.";

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

        private string FormatSuggestionUI(string rawSuggestion)
        {
            // Replace markdown markers with HTML headings.
            string formatted = rawSuggestion;
            formatted = formatted.Replace("**Area of Overspending:**", "</p><h3 style='color:#007ACC;'>Area of Overspending</h3><p>");
            formatted = formatted.Replace("**Actionable Recommendations:**", "</p><h3 style='color:#007ACC;'>Actionable Recommendations</h3>");
            // Convert markdown bold to <strong> tags.
            formatted = System.Text.RegularExpressions.Regex.Replace(formatted, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

            // Rebuild recommendations as an ordered list if applicable.
            string recHeading = "</h3>";
            int recIndex = formatted.IndexOf(recHeading);
            if (recIndex != -1)
            {
                int start = recIndex + recHeading.Length;
                string recText = formatted.Substring(start).Trim();
                var matches = System.Text.RegularExpressions.Regex.Matches(recText, @"\d+\.\s+(.*?)(?=\d+\.\s+|$)", System.Text.RegularExpressions.RegexOptions.Singleline);

                if (matches.Count > 0)
                {
                    StringBuilder listBuilder = new StringBuilder("<ol>");
                    foreach (System.Text.RegularExpressions.Match match in matches)
                    {
                        string itemText = match.Groups[1].Value.Trim();
                        listBuilder.Append("<li>" + itemText + "</li>");
                    }
                    listBuilder.Append("</ol>");
                    formatted = formatted.Substring(0, start) + listBuilder.ToString();
                }
            }
            formatted = "<div style='font-family:Arial, sans-serif; color:#cecece; line-height:1.5;'>" + formatted + "</div>";
            return formatted;
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
