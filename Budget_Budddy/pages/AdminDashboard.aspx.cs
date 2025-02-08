using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Budget_Budddy
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
            }
        }

        // Binds the users from the database to the GridView.
        private void BindUsers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id, username, email FROM users";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvUsers.DataSource = dt;
                        gvUsers.DataBind();
                    }
                }
            }
        }

        // Handles delete commands from the GridView.
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                DeleteUser(id);
                BindUsers();
            }
        }

        // Deletes a user with the specified id.
        private void DeleteUser(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM users WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Handles the Add User button click event.
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string email = txtNewEmail.Text.Trim();
            string password = txtNewPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // Ideally, you would show an error message here.
                return;
            }

            if (UserExists(username, email))
            {
                // Show error message that user/email already exists.
                return;
            }

            AddUser(username, email, password);
            ClearAddUserForm();
            BindUsers();
        }

        // Checks if a user with the given username or email exists.
        private bool UserExists(string username, string email)
        {
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM users WHERE username = @username OR email = @email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Adds a new user to the database.
        private void AddUser(string username, string email, string password)
        {
            string hashedPassword = HashPassword(password);
            string connStr = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO users (username, email, password_hash) VALUES (@username, @email, @password_hash)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password_hash", hashedPassword);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
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

        // Clears the new user form fields.
        private void ClearAddUserForm()
        {
            txtNewUsername.Text = "";
            txtNewEmail.Text = "";
            txtNewPassword.Text = "";
        }

        // Optional: Handle Logout button click.
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session data, if necessary.
            Session.Abandon();

            // Clear session or perform logout logic here.
            Response.Redirect("../index.aspx");
        }
    }
}
