using Second_lesson_csh_ado.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Second_lesson_csh_ado
{
    public partial class InsertOrUpdateForm : Form
    {
        public Category _Category = new Category();
        public Product _Product = new Product();

        private TextBox _TextBox1 = new TextBox();
        private TextBox _TextBox2 = new TextBox();
        private TextBox _TextBox3 = new TextBox();

        public InsertOrUpdateForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InsertOrUpdateForm_Load(object sender, EventArgs e)
        {
            if (Extension.IsCategory)
            {
                Label labelTitle = new Label();
                labelTitle.Text = "Title:";
                labelTitle.Location = new Point(12, 37);
                labelTitle.Font = new Font("Calibri", 12);
                this.Controls.Add(labelTitle);

                TextBox textBoxTitle = new TextBox();
                textBoxTitle.TextChanged += TextBoxTitleCategory_TextChanged;
                textBoxTitle.Size = new Size(238, 32);
                textBoxTitle.Location = new Point(127, 29);
                textBoxTitle.Font = new Font("Calibri", 12);
                this.Controls.Add(textBoxTitle);

                Label labelDescription = new Label();
                labelDescription.Text = "Description:";
                labelDescription.Location = new Point(12, 92);
                labelDescription.Font = new Font("Calibri", 12);
                this.Controls.Add(labelDescription);

                TextBox textBoxDescription = new TextBox();
                textBoxDescription.TextChanged += TextBoxDescription_TextChanged;
                textBoxDescription.Size = new Size(238, 32);
                textBoxDescription.Location = new Point(127, 84);
                textBoxDescription.Font = new Font("Calibri", 12);
                this.Controls.Add(textBoxDescription);

                Button buttonOK = new Button();
                buttonOK.Text = "OK";
                buttonOK.Size = new Size(64, 40);
                buttonOK.Location = new Point(73, 145);
                buttonOK.Font = new Font("Calibri", 12, FontStyle.Bold);
                buttonOK.Click += ButtonOKCategory_Click;
                this.Controls.Add(buttonOK);

                _TextBox1 = textBoxTitle;
                _TextBox2 = textBoxDescription;

                if (!Extension.IsInsert)
                {
                    MainForm mainForm = new MainForm();
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Category " +
                                                                              $"WHERE ID = {Extension.SelectedItemID}", connection))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);

                            DataTable dataTable = dataSet.Tables[0];

                            if (dataTable.Rows.Count > 0)
                            {
                                textBoxTitle.Text = dataTable.Rows[0]["Title"].ToString();
                                textBoxDescription.Text = dataTable.Rows[0]["Description"].ToString();
                            }
                        }
                    }
                }
            }
            if (!Extension.IsCategory)
            {
                Label labelTitle = new Label();
                labelTitle.Text = "Title:";
                labelTitle.Location = new Point(12, 37);
                labelTitle.Font = new Font("Calibri", 12);
                this.Controls.Add(labelTitle);

                TextBox textBoxTitle = new TextBox();
                textBoxTitle.TextChanged += TextBoxTitleProduct_TextChanged;
                textBoxTitle.Size = new Size(238, 32);
                textBoxTitle.Location = new Point(127, 29);
                textBoxTitle.Font = new Font("Calibri", 12);
                this.Controls.Add(textBoxTitle);

                Label labelPrice = new Label();
                labelPrice.Text = "Price:";
                labelPrice.Location = new Point(12, 75);
                labelPrice.Font = new Font("Calibri", 12);
                this.Controls.Add(labelPrice);

                TextBox textBoxPrice = new TextBox();
                textBoxPrice.TextChanged += TextBoxPrice_TextChanged;
                textBoxPrice.Size = new Size(238, 32);
                textBoxPrice.Location = new Point(127, 67);
                textBoxPrice.Font = new Font("Calibri", 12);
                this.Controls.Add(textBoxPrice);

                Label labelCategory = new Label();
                labelCategory.Text = "Category:";
                labelCategory.Location = new Point(12, 113);
                labelCategory.Font = new Font("Calibri", 12);
                this.Controls.Add(labelCategory);

                TextBox textBoxCategory = new TextBox();
                textBoxCategory.Size = new Size(238, 32);
                textBoxCategory.Location = new Point(127, 105);
                textBoxCategory.Font = new Font("Calibri", 12);
                this.Controls.Add(textBoxCategory);

                Button buttonOK = new Button();
                buttonOK.Text = "OK";
                buttonOK.Size = new Size(64, 40);
                buttonOK.Location = new Point(73, 145);
                buttonOK.Font = new Font("Calibri", 12, FontStyle.Bold);
                buttonOK.Click += ButtonOKProduct_Click;
                this.Controls.Add(buttonOK);

                _TextBox1 = textBoxTitle;
                _TextBox2 = textBoxPrice;
                _TextBox3 = textBoxCategory;

                if (!Extension.IsInsert)
                {
                    string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Product " +
                                                                              $"WHERE ID = {Extension.SelectedItemID}", connection))
                        {
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);

                            DataTable dataTable = dataSet.Tables[0];

                            if (dataTable.Rows.Count > 0)
                            {
                                textBoxTitle.Text = dataTable.Rows[0]["Title"].ToString();
                                string tmpStr = dataTable.Rows[0]["Price"].ToString();

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

                                textBoxPrice.Text = tmpStr;

                                using (SqlDataAdapter dataAdapter2 = new SqlDataAdapter("SELECT Title FROM Category " +
                                                                                       $"WHERE ID = '{dataTable.Rows[0]["IDCategory"]}'", connection))
                                {
                                    DataSet dataSet2 = new DataSet();
                                    dataAdapter2.Fill(dataSet2);
                                    DataTable dataTable2 = dataSet2.Tables[0];

                                    if (dataTable2.Rows.Count > 0)
                                        textBoxCategory.Text = dataTable2.Rows[0]["Title"].ToString();
                                    else
                                        textBoxCategory.Text = null;
                                }
                            }
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        private void TextBoxTitleCategory_TextChanged(object sender, EventArgs e)
        {
            _Category.Title = _TextBox1.Text;
        }
        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            _Category.Description = _TextBox2.Text;
        }
        private void ButtonOKCategory_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_Category.Title) || String.IsNullOrEmpty(_Category.Description))
                MessageBox.Show("Not all fields filled correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        private void TextBoxTitleProduct_TextChanged(object sender, EventArgs e)
        {
            _Product.Title = _TextBox1.Text;
        }
        private void TextBoxPrice_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_TextBox2.Text))
            {
                string filter = Regex.Replace(_TextBox2.Text, "[^0-9,]", "");

                if (_TextBox2.Text != filter)
                {
                    MessageBox.Show("Wrong input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _TextBox2.Text = filter;
                }
                else
                {
                    if (!_TextBox2.Text.EndsWith(","))
                        _Product.Price = Convert.ToSingle(_TextBox2.Text);
                }
            }
            else
                _Product.Price = 0f;
        }
        private void ButtonOKProduct_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_Product.Title) || String.IsNullOrEmpty(_Product.Price.ToString()) || _Product.Price <= 0 || String.IsNullOrEmpty(_Product.IDCategory.ToString()))
                MessageBox.Show("Not all fields filled correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string conStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT ID FROM Category " +
                                                                          $"WHERE Title = '{_TextBox3.Text}'", connection))
                    {
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        DataTable dataTable = dataSet.Tables[0];

                        if (dataTable.Rows.Count > 0)
                        {
                            _Product.IDCategory = Convert.ToInt32(dataTable.Rows[0]["ID"]);
                            this.DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                            MessageBox.Show($"Category \"{_TextBox3.Text}\" was not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}