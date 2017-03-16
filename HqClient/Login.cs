using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;
//建议使用mysqlClient模式，如果连接的数据库是mysql的话  
//在C#中，如果想连接数据库的话，需要使用Connection连接对象。同样，不同的连接模式下，所使用的连接对象也不同  
//还有三种连接方式  
//(1）System.Data.SqlClient模式，使用sqlServer数据库比较好  
//(2) System.Data.OleDb模式  
//(3) System.Data.Odbc模式  
//<1>如果使用MsqlClient模式的话，其基本连接字符串和连接对象如下：  
//连接字符串：string connectString = "server=localhost;User Id=root;password=;Database=testDB";  
//属性server是指数据库所在的机器（服务器）的IP地址，如果使用当前机器（本地机器）的话，也就是使用自己电脑上的数据库的话，可以使用"localhost"或者"127.0.0.1",如果使用其它机器上的数据库的话，使用那台机器的IP地址。  
//database指的数据库的名字。  
//Id代表连接数据库的用户名  
//password代表连接数据库的密码,如果密码为空的话不需要填写，这样写"password="即可。

namespace HqClient
{
    public partial class Login : Form
    {
        MySqlConnection conn;
        string constructorString = "server=localhost;User Id=root;password=;Database=hq";
        public Login()
        {
            InitializeComponent();
        }
        private void OpenConn()
        {
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("打开数据库异常：" + ee.Message);
            }
        }


       
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            String name = null;
            int status = 0;
            Boolean hasResult = false;
            int id = -1;

            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    OpenConn();
                }

                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from hq_user where uid='" + textBox1.Text + "' and password='" + textBox2.Text + "'";
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    hasResult = true;
                    if (reader.Read())
                    {
                        name = reader.GetString("name");
                        status = reader.GetInt32("status");
                        id = reader.GetInt32("id");
                    }
                }
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("服务器检查登录的用户合法性时异常：" + ee.Message);
            }
            finally
            {
                if (conn != null && (conn.State != ConnectionState.Closed))
                {
                    if (reader.IsClosed)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            if (hasResult&&name != null&&status ==2)
            {
                MessageBox.Show("用户："+name+" 登陆成功", "登陆成功跳转提示");
                this.Hide();
                Form1 form1 = new Form1(name,id);
                form1.Show();
            }
            else {
                if (!hasResult) {
                    MessageBox.Show("用户名或密码错误，请核对后重新输入", "登陆失败提示");
                    textBox2.Text = null;
                }else if (status < 2) {
                    MessageBox.Show("当前用户未开通登陆功能，请5分钟后再试", "登陆失败提示");
                }
                else if (status == 3)
                {
                    MessageBox.Show("当前用户注册失败，请联系管理员:nuptaxin@gmail.com", "登陆失败提示");
                }
            }
        }

        private void Login_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }
    }
}
