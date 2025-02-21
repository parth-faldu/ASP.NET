using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Budget_Budddy.pages
{
    public partial class manageExpense : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BudgetBuddy"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
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
                BindExpensesGrid();
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
        // -------------------------
        // GridView Binding and Events
        // -------------------------
        private void BindExpensesGrid()
        {
            try
            {
                int userID = GetUserID(Session["username"].ToString());
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, Category, Amount, Description, ExpenseDate FROM Expenses WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                gvExpenses.DataSource = dt;
                gvExpenses.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error loading expenses grid: {ex.Message}');", true);
            }
        }

        protected void gvExpenses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvExpenses.EditIndex = e.NewEditIndex;
            BindExpensesGrid();
        }

        protected void gvExpenses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int expenseID = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value); // Get the expense ID being updated
                GridViewRow row = gvExpenses.Rows[e.RowIndex]; // Get the current editing row

                // Find the TextBoxes inside the row
                TextBox txtCategoryEdit = (TextBox)row.FindControl("txtCategoryEdit");
                TextBox txtAmountEdit = (TextBox)row.FindControl("txtAmountEdit");
                TextBox txtDescriptionEdit = (TextBox)row.FindControl("txtDescriptionEdit");
                TextBox txtDateEdit = (TextBox)row.FindControl("txtDateEdit");

                // Debugging check - Make sure controls exist
                if (txtAmountEdit == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: txtAmountEdit not found!');", true);
                    return;
                }

                // Extract new values
                string newCategory = txtCategoryEdit.Text.Trim();
                decimal newAmount = Convert.ToDecimal(txtAmountEdit.Text.Trim());
                string newDescription = txtDescriptionEdit.Text.Trim();
                DateTime newExpenseDate = Convert.ToDateTime(txtDateEdit.Text.Trim());

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Expenses SET Category = @Category, Amount = @Amount, Description = @Description, ExpenseDate = @ExpenseDate WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category", newCategory);
                        cmd.Parameters.AddWithValue("@Amount", newAmount);
                        cmd.Parameters.AddWithValue("@Description", newDescription);
                        cmd.Parameters.AddWithValue("@ExpenseDate", newExpenseDate);
                        cmd.Parameters.AddWithValue("@ID", expenseID);

                        cmd.ExecuteNonQuery();
                    }
                }

                // ✅ 1. Exit Edit Mode
                gvExpenses.EditIndex = -1;

                // ✅ 2. Reload Data to Reflect Changes
                BindExpensesGrid();

                // ✅ 3. Refresh the UI (Force Page Reload)
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error updating expense: {ex.Message}');", true);
            }
        }

        protected void gvExpenses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvExpenses.EditIndex = -1;
            BindExpensesGrid();
        }

        protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int expenseID = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM Expenses WHERE ID = @ExpenseID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExpenseID", expenseID);
                        cmd.ExecuteNonQuery();
                    }
                }
                BindExpensesGrid();
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error deleting expense: {ex.Message}');", true);
            }
        }

    }
}