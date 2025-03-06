using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CRUD
{
    public partial class CRUD : System.Web.UI.Page
    {
        String dropDown,radio;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CRUDConnectionString1"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            Print();
            /*
            DeleteCommand = "DELETE FROM [emp] WHERE [Id] = @Id" 
            InsertCommand = "INSERT INTO [emp] ([Id], [Name], [Gmail], [Gender], [City]) VALUES (@Id, @Name, @Gmail, @Gender, @City)"  
            SelectCommand = "SELECT [Id], [Name], [Gmail], [Gender], [City] FROM [emp]" 
            UpdateCommand = "UPDATE [emp] SET [Name] = @Name, [Gmail] = @Gmail, [Gender] = @Gender, [City] = @City WHERE [Id] = @Id"
            */
        }
        public void Print() {
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT * FROM [emp]", con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SqlCommand cmd = new SqlCommand("DELETE FROM [emp] WHERE [Id] = "+btn.CommandArgument,con);
            con.Open();
            cmd.ExecuteNonQuery();
            Print();
            con.Close();
            Label5.Text = "Data Removed...";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            SqlDataAdapter adpt = new SqlDataAdapter("SELECT [Id] ,[Name], [Gmail], [Gender], [City] FROM [emp] WHERE [Id] = " + btn.CommandArgument, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);

            TextBox1.Text = dt.Rows[0][1].ToString();
            TextBox3.Text = dt.Rows[0][2].ToString();
            RadioButtonList1.Text = dt.Rows[0][3].ToString();
            DropDownList1.Text = dt.Rows[0][4].ToString();
            ViewState["Id"] = btn.CommandArgument;
            Button1.Text = "Update";
            Label5.Text = "Record Selected...";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Button1.Text == "Save")
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [emp] ([Name], [Gmail], [Gender], [City]) VALUES (@Name, @Gmail, @Gender, @City)", con);
                cmd.Parameters.AddWithValue("@Name", TextBox1.Text);
                cmd.Parameters.AddWithValue("@Gmail", TextBox3.Text);

                if (RadioButtonList1.SelectedItem != null)
                {
                    radio = RadioButtonList1.SelectedValue;
                }
                else
                {
                    radio = "";
                }
                cmd.Parameters.AddWithValue("@Gender", radio);


                if (DropDownList1.SelectedIndex != 0)
                {
                    dropDown = DropDownList1.SelectedValue;
                }
                else
                {
                    dropDown = "";
                }
                cmd.Parameters.AddWithValue("@City", dropDown);

                con.Open();
                cmd.ExecuteNonQuery();
                Print();
                con.Close();

                TextBox1.Text = "";
                TextBox3.Text = "";
                DropDownList1.ClearSelection();
                RadioButtonList1.ClearSelection();
                Label5.Text = "Data Inserted...";
            }
            else {
                SqlCommand cmd = new SqlCommand("UPDATE [emp] SET [Name] = @Name, [Gmail] = @Gmail, [Gender] = @Gender, [City] = @City WHERE [Id] = @Id", con);
                cmd.Parameters.AddWithValue("@Name", TextBox1.Text);
                cmd.Parameters.AddWithValue("@Gmail", TextBox3.Text);

                if (RadioButtonList1.SelectedItem != null)
                {
                    radio = RadioButtonList1.SelectedValue;
                }
                else
                {
                    radio = "";
                }
                cmd.Parameters.AddWithValue("@Gender", radio);


                if (DropDownList1.SelectedItem != null)
                {
                    dropDown = DropDownList1.SelectedValue;
                }
                else
                {
                    dropDown = "";
                }
                cmd.Parameters.AddWithValue("@City", dropDown);
                cmd.Parameters.AddWithValue("@Id",ViewState["Id"]);

                con.Open();
                cmd.ExecuteNonQuery();
                Print();
                con.Close();

                TextBox1.Text = "";
                TextBox3.Text = "";
                DropDownList1.ClearSelection();
                RadioButtonList1.ClearSelection();
                Label5.Text = "Data Updated...";
                Button1.Text = "Save";
            }
        }
    }
}