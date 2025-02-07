using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Budget_Budddy
{
    public partial class SignUp : System.Web.UI.Page
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

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (password != confirmPassword)
            {
                lblError.Text = "❌ Passwords do not match.";
                return;
            }

            string hashedPassword = HashPassword(password);
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Check if username or email already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username OR email = @email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                        checkCmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar).Value = email;
                        int userExists = (int)checkCmd.ExecuteScalar();

                        if (userExists > 0)
                        {
                            lblError.Text = "❌ Username or Email already exists!";
                            return;
                        }
                    }

                    // Insert user into database
                    string insertQuery = "INSERT INTO users (username, email, password_hash) VALUES (@username, @email, @password_hash)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar).Value = email;
                        cmd.Parameters.Add("@password_hash", System.Data.SqlDbType.NVarChar).Value = hashedPassword;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Session["username"] = username;
                            Response.Redirect("dashboard.aspx");
                        }
                        else
                        {
                            lblError.Text = "❌ Registration failed. Please try again.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "❌ Error: " + ex.Message;
                }
            }
        }

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
