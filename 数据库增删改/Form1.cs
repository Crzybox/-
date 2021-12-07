using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JSqlLib;
using System.Data.SqlClient;
namespace 数据库增删改
{
    public partial class Form1 : Form
    {
        #region 静态容器或对象
        /// <summary>
        /// 数据库对象
        /// </summary>
        public static JD_str_DataInfo di = new JD_str_DataInfo();
        /// <summary>
        /// 静态连接对象
        /// </summary>
        public static SqlConnection conn;
        /// <summary>
        /// 静态结果
        /// </summary>
        public static JD_str_SqlRunResu rs;
        /// <summary>
        /// 静态表
        /// </summary>
        public static DataTable dt1;
        /// <summary>
        /// 当前操作的行的ID
        /// </summary>
        public int ID { get; set; }
        #endregion
        public Form1()
        {
            InitializeComponent();
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            connection();
            
            
            


        }
        /// <summary>
        /// 当表格数据显示绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //（删除后）需要绑定的行数，大于数据库表的行数时直接结束返回
            if (e.RowIndex>=dt1.Rows.Count)
            {
                return;
            }
            else
            {
                switch (dataGridView1.Columns[e.ColumnIndex].Name)
                {

                    case "id":
                        e.Value = dt1.Rows[e.RowIndex]["id"];
                        break;
                    case "name":
                        e.Value = dt1.Rows[e.RowIndex]["name"];
                        break;
                    case "sex":
                        e.Value = dt1.Rows[e.RowIndex]["sex"];
                        break;
                    default:
                        break;
                }

            }

        }

        /// <summary>
        /// 建立连接
        /// </summary>
        private void connection()
        {
            //输入数据库信息
            di.DataName = "test";
            di.ServerName = "101.33.235.201";
            di.UserName = "sa";
            di.UserPw = "Crt?!Sqlserver";


            //建立连接
            /*conn = JSqlLib.JD_cla_SqlServer.GetConn(di, ref errms);*/

            //绑定数据源
            di.SQLSentence = "select * from ui order by id";
            rs = JSqlLib.JD_cla_SqlServer.GetSqlTable(di);
            dt1 = rs.msNcb;
            //dataGridView1.DataSource = rs.msNcb;

            dataGridView1.RowCount = rs.msNcb.Rows.Count;
            //for (int i = 0; i < rs.msNcb.Rows.Count; i++)
            //{
            //    for (int j = 0; j < rs.msNcb.Columns.Count; j++)
            //    {
            //        dataGridView1.Rows[i].Cells["id"].Value = rs.msNcb.Rows[i][j].ToString(); 
            //    }
            //}


        }
        /// <summary>
        /// 刷新表单
        /// </summary>
        public void reload()
        {
            
            di.SQLSentence = "select * from ui order by id";
            rs = JSqlLib.JD_cla_SqlServer.GetSqlTable(di);
            dt1 = rs.msNcb;
            dataGridView1.RowCount = rs.msNcb.Rows.Count;
            //刷新一遍表格
            dataGridView1.Visible = false;
            dataGridView1.Visible = true;

        }

        /// <summary>
        /// 打开添加窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ADD add = new ADD(this);
            add.ShowDialog();

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //获取选中行id
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].FormattedValue);
            di.SQLSentence = "delete from ui where id=" + id;
            rs = JD_cla_SqlServer.GetSqlRow(di);
            if (rs.iValue > 0)
            {
                MessageBox.Show("删除成功");
            }
            //删除后刷新表单
            reload();


        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //收集数据
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].FormattedValue);
            string name = dataGridView1.SelectedRows[0].Cells[1].FormattedValue.ToString();
            string sex = dataGridView1.SelectedRows[0].Cells[2].FormattedValue.ToString();
            Form3 f3 = new Form3(this);
            f3.setTxt(id, name, sex);
            f3.ShowDialog();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string name = txtName.Text;
            string sex = txtSex.Text;
            string sql = String.Format("select * from ui where name like '%{0}%' and sex like '%{1}%' ", name, sex);

            if (id != "")
            {
                sql += "and id=" + id;
            }
            sql += " order by id";
            di.SQLSentence = sql;
            rs = JD_cla_SqlServer.GetSqlTable(di);
            if (rs.msNcb.Rows.Count > 0)
            {
                dataGridView1.RowCount = rs.msNcb.Rows.Count;
            }

            else
                dataGridView1.RowCount = 1;
            dt1 = rs.msNcb;
        }
        /// <summary>
        /// 直接在表格中修改时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int i= e.RowIndex;
            
            //当修改的列为id时
            if(e.ColumnIndex == 0)
            {
                int id = int.Parse(dataGridView1.Rows[i].Cells[0].FormattedValue.ToString());
                di.SQLSentence = "update ui set id=" + id;
                Change();
            }
            else if(e.ColumnIndex == 1)
            {
                string name =dataGridView1.Rows[i].Cells[1].FormattedValue.ToString();
                di.SQLSentence = "update ui set name=" + name;
                Change();
            }
            var s = sender;
        }
        /// <summary>
        /// 直接修改
        /// </summary>
        public void Change()
        {
            try
            {
                rs = JD_cla_SqlServer.GetSqlRow(di);
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改失败" + ex.Message.ToString());
            }
        }
    }
}
