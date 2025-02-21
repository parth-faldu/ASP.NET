using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Budget_Budddy.pages
{
    public partial class dashboard : Page
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
                //BindExpensesGrid();
                LoadExpenses(); // Update charts and grid as 
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

        // Calculate the total expenses for a given user.
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
    }
}