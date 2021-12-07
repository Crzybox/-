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
namespace 数据库增删改
{
    public partial class ADD : Form
    {
        public static Form1 f1;
        public static JD_str_DataInfo di;
        public ADD(Form1 f)
        {
            f1 = f;
            //输入数据库信息
            di.DataName = "test";
            di.ServerName = "101.33.235.201";
            di.UserName = "sa";
            di.UserPw = "Crt?!Sqlserver";
            InitializeComponent();
        }
       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //收集数据
            string name = txtName.Text;
            string sex = txtSex.Text;

           
            di.SQLSentence = string.Format("insert ui values('{0}','{1}')",name,sex);        
            JSqlLib.JD_str_SqlRunResu rs = new JSqlLib.JD_str_SqlRunResu();
            rs = JSqlLib.JD_cla_SqlServer.GetSqlRow(di);
            
            if (rs.iValue > 0)
            {
                MessageBox.Show("添加成功");
               f1.reload();
                this.Close();
                
               
            }
           
        }
    }
}
