using Second_lesson_csh_ado.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Second_lesson_csh_ado
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            comboBoxTables.SelectedIndex = 0;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedIndex == 0)
                InitializeDataGridViewCategory();
            else if (comboBoxTables.SelectedIndex == 1)
                InitializeDataGridViewProduct();
        }

        public void SelectedItem()
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            Extension.SelectedItemID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                SelectedItem();
                if (comboBoxTables.SelectedItem.ToString() == "Category")
                {
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("UPDATE Product SET IDCategory = null " +
                                                                   "WHERE IDCategory = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@ID", Extension.SelectedItemID);
                            command.ExecuteNonQuery();
                        }
                        using (SqlCommand command = new SqlCommand("DELETE FROM Category " +
                                                                   "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@ID", Extension.SelectedItemID);
                            command.ExecuteNonQuery();
                        }
                    }
                    InitializeDataGridViewCategory();
                }
                else if (comboBoxTables.SelectedItem.ToString() == "Product")
                {
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        using (SqlCommand command = new SqlCommand("DELETE FROM Product " +
                                                                   "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@ID", Extension.SelectedItemID);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    InitializeDataGridViewProduct();
                }
            }
            else
                MessageBox.Show("Please choose item (one) to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        private void InitializeDataGridViewCategory()
        {
            if (comboBoxTables.SelectedIndex != 0)
                comboBoxTables.SelectedIndex = 0;

            string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conStr))
            {

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Category", connection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "Category");

                    dataGridView1.DataSource = dataSet.Tables["Category"];
                }
            }
            dataGridView1.Columns["ID"].Visible = false;
        }
        private void buttonInsertCategory_Click(object sender, EventArgs e)
        {
            InsertOrUpdateForm insertForm = new InsertOrUpdateForm();
            Extension.IsCategory = true;
            Extension.IsInsert = true;
            DialogResult dialogResult = insertForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Category (Title, Description)" +
                                                               "VALUES (@Title, @Description)", connection))
                    {
                        command.Parameters.AddWithValue("@Title", insertForm._Category.Title);
                        command.Parameters.AddWithValue("@Description", insertForm._Category.Description);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            InitializeDataGridViewCategory();
        }
        private void buttonUpdateCategory_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                SelectedItem();

                InsertOrUpdateForm insertForm = new InsertOrUpdateForm();
                Extension.IsCategory = true;
                Extension.IsInsert = false;
                DialogResult dialogResult = insertForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Category SET Title = @Title, Description = @Description " +
                                                                   "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@Title", insertForm._Category.Title);
                            command.Parameters.AddWithValue("@Description", insertForm._Category.Description);
                            command.Parameters.AddWithValue("@ID", Extension.SelectedItemID);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
                MessageBox.Show("Please choose category (one) to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            InitializeDataGridViewCategory();
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        private void InitializeDataGridViewProduct()
        {
            if (comboBoxTables.SelectedIndex != 1)
                comboBoxTables.SelectedIndex = 1;

            string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                DataTable dataTable = new DataTable();

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Product", connection))
                    dataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    string tmpStr = row["Price"].ToString();
                    if (tmpStr.Contains(","))
                    {
                        char[] tmpChr = tmpStr.ToCharArray();
                        tmpStr = null;

                        for (int i = 0; i < tmpChr.Length; i++)
                        {
                            tmpStr += tmpChr[i].ToString();
                            if (tmpChr[i] == ',')
                            {
                                for (int j = i; j < i + 2; j++)
                                    tmpStr += tmpChr[j + 1];
                                break;
                            }
                        }
                    }
                    row["Price"] = tmpStr;
                }
                dataGridView1.DataSource = dataTable;
            }
            dataGridView1.Columns["ID"].Visible = false;
        }

        private void buttonInsertProduct_Click(object sender, EventArgs e)
        {
            InsertOrUpdateForm insertForm = new InsertOrUpdateForm();
            Extension.IsCategory = false;
            Extension.IsInsert = true;
            DialogResult dialogResult = insertForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Product (Title, Price, IDCategory)" +
                                                               "VALUES (@Title, @Price, @IDCategory)", connection))
                    {
                        command.Parameters.AddWithValue("@Title", insertForm._Product.Title);
                        command.Parameters.AddWithValue("@Price", insertForm._Product.Price);
                        command.Parameters.AddWithValue("@IDCategory", insertForm._Product.IDCategory);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            InitializeDataGridViewProduct();
        }
        private void buttonUpdateProduct_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                SelectedItem();

                InsertOrUpdateForm insertForm = new InsertOrUpdateForm();
                Extension.IsCategory = false;
                Extension.IsInsert = false;
                DialogResult dialogResult = insertForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Product SET Title = @Title, Price = @Price, IDCategory = @IDCategory " +
                                                                   "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@Title", insertForm._Product.Title);

                            command.Parameters.AddWithValue("@Price", insertForm._Product.Price);
                            command.Parameters.AddWithValue("@IDCategory", insertForm._Product.IDCategory);
                            command.Parameters.AddWithValue("@ID", Extension.SelectedItemID);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
                MessageBox.Show("Please choose category (one) to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            InitializeDataGridViewProduct();
        }
    }
}