using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Budget_Budddy
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TestDatabaseConnection();
            }
        }

        private void TestDatabaseConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    ClientScript.RegisterStartupScript(this.GetType(), "consoleLog", "console.log('Connected to Database!');", true);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Database Connection Error: " + ex.Message;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            // Determine if admin login is selected using rbAdmin.Checked
            bool isAdmin = rbAdmin.Checked;

            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    if (!isAdmin)
                    {
                        // Regular user login from the "users" table
                        string query = "SELECT password_hash FROM users WHERE username = @username";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            object storedHash = cmd.ExecuteScalar();

                            if (storedHash != null)
                            {
                                string storedPasswordHash = storedHash.ToString();
                                string enteredPasswordHash = HashPassword(password);

                                if (storedPasswordHash == enteredPasswordHash)
                                {
                                    Session["username"] = username;
                                    // Redirect regular user to the dashboard page
                                    Response.Redirect("pages/dashboard.aspx");
                                }
                                else
                                {
                                    lblError.Text = "Invalid username or password.";
                                }
                            }
                            else
                            {
                                lblError.Text = "Invalid username or password.";
                            }
                        }
                    }
                    else
                    {
                        // Admin login from the "admins" table
                        string query = "SELECT password_hash FROM admins WHERE username = @username";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            object storedHash = cmd.ExecuteScalar();

                            if (storedHash != null)
                            {
                                string storedPasswordHash = storedHash.ToString();
                                string enteredPasswordHash = HashPassword(password);

                                if (storedPasswordHash == enteredPasswordHash)
                                {
                                    Session["admin"] = username;
                                    // Redirect admin user to the admin dashboard page.
                                    // Adjust the path if your admin dashboard file is located elsewhere.
                                    Response.Redirect("pages/AdminDashboard.aspx");
                                }
                                else
                                {
                                    lblError.Text = "Invalid admin username or password.";
                                }
                            }
                            else
                            {
                                lblError.Text = "Invalid admin username or password.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                }
            }
        }

        // Hashing Function (must match your SignUp hashing method)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
