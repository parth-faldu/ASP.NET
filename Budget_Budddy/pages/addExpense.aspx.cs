using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Budget_Budddy.pages
{
    public partial class addExpense : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("../index.aspx");
            }
            litUsername.Text = Session["username"].ToString();

        }

        protected void btnAddExpense_Click(object sender, EventArgs e)
        {
            // Get the logged-in user's ID
            int userID = GetUserID(Session["username"].ToString());
            if (userID == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error retrieving user information.');", true);
                return;
            }

            // Capture user input
            string category = txtCategory.Text.Trim();
            string description = txtDescription.Text.Trim();
            decimal amount;
            DateTime expenseDate;

            // Validate user input
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Category and Description cannot be empty.');", true);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out amount))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid amount. Please enter a valid number.');", true);
                return;
            }

            if (!DateTime.TryParse(txtDate.Text, out expenseDate))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid date. Please enter a valid date.');", true);
                return;
            }

            // Insert the expense into the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Expenses (UserID, Category, Amount, Description, ExpenseDate) VALUES (@UserID, @Category, @Amount, @Description, @ExpenseDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@ExpenseDate", expenseDate);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Alert and then redirect to avoid double posting
                        string script = "alert('Expense added successfully!'); window.location='" + Request.RawUrl + "';";
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to add expense. Please try again.');", true);
                    }
                }
                ClearFields();
            }
        }


        // Retrieves the UserID based on the logged-in username
        private int GetUserID(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID FROM Users WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        // Clears input fields after adding an expense
        private void ClearFields()
        {
            txtCategory.Text = "";
            txtAmount.Text = "";
            txtDescription.Text = "";
            txtDate.Text = "";
        }
    }
}
