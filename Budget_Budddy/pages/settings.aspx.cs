using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Budget_Budddy.pages
{
    public partial class settings : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("../index.aspx");
                return;
            }
            litUsername.Text = Session["username"].ToString();


            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        // Loads the current user's profile information.
        private void LoadUserProfile()
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

        // Handles the profile update.
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string oldUsername = Session["username"].ToString();
                string newUsername = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string newPassword = txtPassword.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the new username is already in use by another user.
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @NewUsername AND username <> @OldUsername";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@NewUsername", newUsername);
                        checkCmd.Parameters.AddWithValue("@OldUsername", oldUsername);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Username already exists. Please choose a different username.";
                            return;
                        }
                    }

                    // Build the update query.
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
                            Session["username"] = newUsername; // Update session if username changes.
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Error updating profile. Please try again.";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Username already exists. Please choose a different username.";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        // Handles account deletion.
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                int userID = GetUserID(username);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Delete user expenses first.
                    string deleteExpensesQuery = "DELETE FROM Expenses WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteExpensesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the user record.
                    string deleteUserQuery = "DELETE FROM Users WHERE ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Clear session and redirect to login.
                Session.Clear();
                Session.Abandon();
                Response.Redirect("../index.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error deleting account: " + ex.Message;
            }
        }

        // Helper method to retrieve the user ID.
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
                    {
                        userID = Convert.ToInt32(result);
                    }
                }
            }
            return userID;
        }

        // Hashes a password using SHA256.
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("../index.aspx");
        }
    }
}
