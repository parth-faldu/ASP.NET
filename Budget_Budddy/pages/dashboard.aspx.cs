using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Budget_Budddy.pages
{
    public partial class dashboard : Page
    {
        // Get Connection String from Web.config for security
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect if user is not logged in
            if (Session["usrname"] == null)
            {
                Response.Redirect("../index.aspx");
            }
            else
            {
                // Display logged-in username
                litUsername.Text = Session["username"].ToString();
            }
        }

        // Logout function
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Redirect("../index.aspx", true);
        }

        // Add Expense function
        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            try
            {
                // Get input values
                string category = txtCategory.Text.Trim();
                decimal amount;
                string description = txtDescription.Text.Trim();
                DateTime date;

                bool isValidAmount = decimal.TryParse(txtAmount.Text, out amount);
                bool isValidDate = DateTime.TryParse(txtDate.Text, out date);

                if (string.IsNullOrEmpty(category) || !isValidAmount || !isValidDate)
                {
                    Response.Write("<script>alert('Invalid input. Please check your values.');</script>");
                    return;
                }

                int username = Convert.ToInt32(Session["username"]); // Get logged-in User ID

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Expenses (Use, Category, Amount, Description, ExpenseDate) VALUES (@username, @Category, @Amount, @Description, @ExpenseDate)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ExpenseDate", date);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Clear input fields
                txtCategory.Text = "";
                txtAmount.Text = "";
                txtDescription.Text = "";
                txtDate.Text = "";

                // Refresh the page to update UI
                Response.Redirect("dashboard.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
