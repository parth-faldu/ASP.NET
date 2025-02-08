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

                        // Build the update query dynamically.
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
                                // Hash the new password before storing it.
                                cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));
                            }

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                                lblMessage.Text = "Profile updated successfully!";
                                // Update session variable if username has changed.
                                Session["username"] = newUsername;
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
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message;
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
