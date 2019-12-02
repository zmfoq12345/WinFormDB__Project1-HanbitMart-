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
    public partial class AddForm : Form
    {
        //상품번호, 상품종류, 상품명, 재고량, 단가
        private string initial = "";

        LoginForm lform = new LoginForm();
        MainForm main = new MainForm(); 

        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
        }
            
        #region RadioButton

        private void radioButton1_Click(object sender, EventArgs e)
        {
            initial = "A";
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            initial = "B";
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            initial = "C";
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            initial = "D";
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            initial = "E";
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            initial = "F";
        }
        #endregion

        private void btn_add_Click(object sender, EventArgs e)
        {
            string sql = "insert into 상품(상품번호, 상품종류, 상품명, 재고량, 단가) values(@num, @kind, @name, @amount, @price)";

            MySqlCommand cmd = new MySqlCommand(sql, lform.conn);
            cmd.Parameters.AddWithValue("@num", initial + getNum());
            cmd.Parameters.AddWithValue("@kind", tb_kind.Text);
            cmd.Parameters.AddWithValue("@name", tb_name.Text);
            cmd.Parameters.AddWithValue("@amount", tb_amount.Text);
            cmd.Parameters.AddWithValue("@price", tb_price.Text);
            try
            {
                lform.conn.Open();
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("물품이 등록되었습니다.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lform.conn.Close();
                changeForm();
            }
        }

        private string getNum()
        {
            string sql = "SELECT count(*) FROM 상품 where substr(상품번호, 1,1) = @a;";
            string tmp = "";
            MySqlCommand cmd = new MySqlCommand(sql, lform.conn);
            cmd.Parameters.AddWithValue("@a", initial);
            try
            {
                lform.conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) // select 결과가 있으면 True
                {
                    tmp = reader.GetString("count(*)");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lform.conn.Close();
            }

            int a = int.Parse(tmp)+1;
            return a.ToString();
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("물품등록이 취소되었습니다.");
            changeForm();
        }
        private void changeForm()
        {
            this.Close();
        }
    }
}
