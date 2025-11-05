using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLitePCL;




namespace RecordKeeper
{
    public partial class Form1 : Form
    {
        string dbPath;
        string connectionString;
        int selectedId = -1;
        public Form1()
        {
            InitializeComponent();
            dbPath = Path.Combine(Application.StartupPath, "data.db");
            connectionString = "Data Source=data.db";
            CreateDatabaseAndTable();
            LoadData();
        }
        private void CreateDatabaseAndTable()
        {
            if (!File.Exists(dbPath))
                using (File.Create(dbPath)) { }

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Person (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                CodeMelli TEXT NOT NULL,
                                FullName TEXT NOT NULL,
                                ParvandehNo INTEGER NOT NULL
                            );";
                using (var cmd = new SqliteCommand(sql, conn))
                    cmd.ExecuteNonQuery();
            }
        }
        private void LoadData()
        {
            DataTable dt = new DataTable();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Person";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            dataShow.DataSource = dt;
            dataShow.ClearSelection();
            selectedId = -1;
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodemeli.Text) ||
               string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("لطفا همه فیلدها را پر کنید","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            long parvandehNo;
            if (string.IsNullOrWhiteSpace(txtParvandehNo.Text))
                parvandehNo = new Random().Next(1000, 999999999);
            else
                parvandehNo = Convert.ToInt64(txtParvandehNo.Text);

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Person (CodeMelli, FullName, ParvandehNo) VALUES (@cm, @fn, @p)";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cm", txtCodemeli.Text);
                    cmd.Parameters.AddWithValue("@fn", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@p", parvandehNo);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("✅ اطلاعات با موفقیت ذخیره شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadData();
        }

        private void dataShow_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataShow_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataShow.Rows[e.RowIndex];
                selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                txtCodemeli.Text = row.Cells["CodeMelli"].Value.ToString();
                txtFullName.Text = row.Cells["FullName"].Value.ToString();
                txtParvandehNo.Text = row.Cells["ParvandehNo"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("لطفاً یک ردیف برای ویرایش انتخاب کنید!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Person SET CodeMelli=@cm, FullName=@fn, ParvandehNo=@p WHERE Id=@id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cm", txtCodemeli.Text);
                    cmd.Parameters.AddWithValue("@fn", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@p", txtParvandehNo.Text);
                    cmd.Parameters.AddWithValue("@id", selectedId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("✏️ رکورد با موفقیت ویرایش شد");
            ClearFields();
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("لطفاً یک ردیف برای حذف انتخاب کنید!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("آیا از حذف این رکورد مطمئن هستید؟", "تأیید حذف",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Person WHERE Id=@id";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("🗑️ رکورد حذف شد");
                ClearFields();
                LoadData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("لطفا برای جست و جو کد ملی ,نام یا نام خوانوادگی مورد نظر خود را وارد کنید!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                DataTable dt = new DataTable();
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string search = txtSearch.Text.Trim();
                    string sql = @"SELECT * FROM Person
                               WHERE FullName LIKE @search 
                               OR CodeMelli LIKE @search 
                               OR ParvandehNo LIKE @search";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }

                dataShow.DataSource = dt;
            }
        }
        private void ClearFields()
        {
            txtCodemeli.Clear();
            txtFullName.Clear();
            txtParvandehNo.Clear();
            selectedId = -1;
        }
    }
}
