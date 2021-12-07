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
    public partial class Form3 : Form
    {
        public static JD_str_DataInfo di =new JD_str_DataInfo();
        public static Form1 f1;
        public static JD_str_SqlRunResu rs;
        public Form3(Form1 f)
        {
            f1 = f;
            InitializeComponent();
            di.DataName = "test";
            di.ServerName = "101.33.235.201";
            di.UserName = "sa";
            di.UserPw = "Crt?!Sqlserver";
        }
        /// <summary>
        /// 接收主窗体穿过来的值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        public void setTxt(int id, string name, string sex)
        {
            txtName.Text = name;
            txtSex.Text = sex;
            txtID.Text = id.ToString();

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string name=txtName.Text ;
            string sex=txtSex.Text ;
            string id=txtID.Text ;
            di.SQLSentence = String.Format("update ui set name='{0}',sex='{1}' where id={2}", name, sex, id);
            rs = JD_cla_SqlServer.GetSqlRow(di);
            if (rs.iValue > 0)
            {
                MessageBox.Show("修改成功"); 
                f1.reload();
                this.Close();
            }
        }
    }
}
