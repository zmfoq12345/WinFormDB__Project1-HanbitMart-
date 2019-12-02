using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_DB_project01_
{
    public partial class MainForm : Form
    {
        int[] radioCount = new int[6]; // 음식A 침구B 의류C 가전제품D 도서E 생필품F
        public MainForm()
        {
            InitializeComponent();
        }
        LoginForm lform = new LoginForm();
        MySqlDataAdapter adapter;
        DataSet dataSet;
        string admin = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            adapter = new MySqlDataAdapter("SELECT * FROM 상품", lform.conn); // dataSet과 DB 연결
            dataSet = new DataSet(); // 메모리상의 가상 DataTable 관리
            search_list.Items.Add("상품번호");
            search_list.Items.Add("상품종류");
            search_list.Items.Add("상품명");

            adapter.Fill(dataSet, "상품");
            dataGridView1.DataSource = dataSet.Tables["상품"];
            
            LoginForm lformtmp = Owner as LoginForm;
            if (lformtmp != null)
            {
                admin = lformtmp.name;
                label3.Text = lformtmp.level;
                if (admin == "Admin")
                {
                    dataGridView1.ReadOnly = false;
                    this.title.Location = new System.Drawing.Point(4, 12);
                    this.btn_search.Location = new System.Drawing.Point(468, 147);
                    this.search_list.Location = new System.Drawing.Point(50, 149);
                    this.lb_search.Location = new System.Drawing.Point(11, 153);
                    this.tb_search.Location = new System.Drawing.Point(130, 149);
                    title.Text = "HANBIT MART\nMANAGEMENT";
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = true;
                    pictureBox3.Visible = true;
                    pictureBox4.Visible = true;
                }
                else
                {
                    dataGridView1.ReadOnly = true;
                    this.title.Location = new System.Drawing.Point(195, 9); 
                    this.tb_search.Location = new System.Drawing.Point(130, 79);
                    this.lb_search.Location = new System.Drawing.Point(11, 83);
                    this.btn_search.Location = new System.Drawing.Point(468, 77);
                    this.search_list.Location = new System.Drawing.Point(50, 79);
                    title.Text = "HANBIT MART";
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    pictureBox4.Visible = false;
                }
                label1.Text = lformtmp.name;
            }
        }

        public void search(string kind, string sql = "SELECT * FROM 상품 where substr(상품번호, 1,1) = @a")
        {
            adapter.SelectCommand = new MySqlCommand(sql, lform.conn);
            adapter.SelectCommand.Parameters.AddWithValue("@a", kind);
            try
            {
                lform.conn.Open();
                dataSet.Clear();
                if (adapter.Fill(dataSet, "상품") > 0) dataGridView1.DataSource = dataSet.Tables["상품"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lform.conn.Close();
            }
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (tb_search.Text == "")
            {
                search("", "SELECT * FROM 상품");
            }
            else
            {
                search(tb_search.Text, "SELECT * FROM 상품 WHERE " + search_list.Text + " = @a");
            }
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
        }
        private void btn_remove_Click(object sender, EventArgs e)
        {
            string sql = "delete from 상품 where 상품번호 = @num";
            adapter.DeleteCommand = new MySqlCommand(sql, lform.conn);
            string num = dataGridView1.SelectedRows[0].Cells["상품번호"].Value.ToString();
            adapter.DeleteCommand.Parameters.AddWithValue("@num", num);
            try
            {
                lform.conn.Open();
                if (adapter.DeleteCommand.ExecuteNonQuery() > 0)
                {
                    dataSet.Clear();
                    adapter.Fill(dataSet, "상품");
                    dataGridView1.DataSource = dataSet.Tables["상품"];
                    MessageBox.Show("물품이 삭제되었습니다..");
                }
                else
                {
                    MessageBox.Show("삭제된 데이터가 없습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lform.conn.Close();
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.Show();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string sql = $"update 상품 set 상품번호=@상품번호, 상품종류=@상품종류, 상품명=@상품명, 재고량=@재고량, 단가=@단가 where 상품번호 = \"{dataGridView1.SelectedRows[0].Cells["상품번호"].Value.ToString()}\"";
            adapter.UpdateCommand = new MySqlCommand(sql, lform.conn);
            adapter.UpdateCommand.Parameters.AddWithValue("@상품번호", dataGridView1.SelectedRows[0].Cells["상품번호"].Value.ToString());
            adapter.UpdateCommand.Parameters.AddWithValue("@상품종류", dataGridView1.SelectedRows[0].Cells["상품종류"].Value.ToString());
            adapter.UpdateCommand.Parameters.AddWithValue("@상품명", dataGridView1.SelectedRows[0].Cells["상품명"].Value.ToString());
            adapter.UpdateCommand.Parameters.AddWithValue("@재고량", dataGridView1.SelectedRows[0].Cells["재고량"].Value);
            adapter.UpdateCommand.Parameters.AddWithValue("@단가", dataGridView1.SelectedRows[0].Cells["단가"].Value);
            
            try
            {
                lform.conn.Open();
                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    dataSet.Clear();
                    adapter.Fill(dataSet, "상품"); // DB -> 메모리
                    dataGridView1.DataSource = dataSet.Tables["상품"];
                    MessageBox.Show("물품 정보가 수정되었습니다.");
                }
                else
                {
                    MessageBox.Show("수정된 데이터가 없습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lform.conn.Close();
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && admin == "Admin")
            {
                btn_update_Click(sender, e);
            }

        }

        #region radioButton
        private void radioButton1_Click(object sender, EventArgs e)
        {
            search("A");
        }
        private void radioButton2_Click(object sender, EventArgs e)
        {
            search("B");
        }
        private void radioButton3_Click(object sender, EventArgs e)
        {
            search("C");
        }
        private void radioButton4_Click(object sender, EventArgs e)
        {
            search("D");
        }
        private void radioButton5_Click(object sender, EventArgs e)
        {
            search("E");
        }
        private void radioButton6_Click(object sender, EventArgs e)
        {
            search("F");
        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tb_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_search.PerformClick();
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            lform.Show();
            this.Hide();
        }
    }
}
