using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Budget_Budddy.pages
{
    public partial class updateprofile : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            if (Session["username"] != null)
            {
                string currentUsername = Session["username"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT username, email FROM users WHERE username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", currentUsername);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtUsername.Text = reader["username"].ToString();
                                txtEmail.Text = reader["email"].ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("../index.aspx");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] != null)
                {
                    string oldUsername = Session["username"].ToString();
                    string newUsername = txtUsername.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string newPassword = txtPassword.Text.Trim();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // **🔹 Step 1: Check if the new username already exists (excluding the current user)**
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @NewUsername AND username <> @OldUsername";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@NewUsername", newUsername);
                            checkCmd.Parameters.AddWithValue("@OldUsername", oldUsername);

                            int existingUserCount = (int)checkCmd.ExecuteScalar();
                            if (existingUserCount > 0)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Text = "User already exists. Please choose a different username.";
                                return;
                            }
                        }

                        // **🔹 Step 2: Proceed with the update if no duplicate username exists**
                        string updateQuery = "UPDATE users SET username = @NewUsername, email = @Email, updated_at = GETDATE()";
                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            updateQuery += ", password_hash = @PasswordHash";
                        }
                        updateQuery += " WHERE username = @OldUsername";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@NewUsername", newUsername);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@OldUsername", oldUsername);

                            if (!string.IsNullOrEmpty(newPassword))
                            {
                                cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));
                            }

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                                lblMessage.Text = "Profile updated successfully!";
                                Session["username"] = newUsername; // Update session if username changes
                            }
                            else
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Text = "Error updating profile. Please try again.";
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("../index.aspx");
                }
            }
            catch (SqlException ex)
            {
                // **🔹 Step 3: Catch SQL Unique Constraint Violation (Error Code: 2627 or 2601)**
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "User already exists. Please choose a different username.";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }


        // Hashing function using SHA256
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
