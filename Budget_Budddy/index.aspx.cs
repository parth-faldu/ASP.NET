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
                    ClientScript.RegisterStartupScript(this.GetType(), "consoleLog", "console.log('✅ Connected to Database!');", true);
                }
                catch (Exception ex)
                {
                    lblError.Text = "❌ Database Connection Error: " + ex.Message;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Fetch stored hashed password from DB
                    string query = "SELECT password_hash FROM users WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        object storedHash = cmd.ExecuteScalar();

                        if (storedHash != null)
                        {
                            string storedPasswordHash = storedHash.ToString();

                            // Hash the entered password
                            string enteredPasswordHash = HashPassword(password);

                            if (storedPasswordHash == enteredPasswordHash)
                            {
                                Session["username"] = username;
                                Response.Redirect("pages/dashboard.aspx");
                            }
                            else
                            {
                                lblError.Text = "❌ Invalid username or password.";
                            }
                        }
                        else
                        {
                            lblError.Text = "❌ Invalid username or password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "❌ Error: " + ex.Message;
                }
            }
        }

        // 🔹 Hashing Function (Must Match Your SignUp Hashing)
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
