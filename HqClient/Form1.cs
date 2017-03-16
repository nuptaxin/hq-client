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
using System.Collections;

namespace HqClient
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        string constructorString = "server=localhost;User Id=root;password=;Database=hq";
        int myid;
        Hashtable ht = new Hashtable();


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
        public Form1(String name, int id)
        {
            myid = id;
            InitializeComponent();
            label1.Text = "欢迎你：" + name;
            // 获取当前用户的题目列表
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            

            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    OpenConn();
                }

                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from hq_ucrelation ucr ,hq_course c where ucr.courseid = c.id and ucr.userid=" + id;
                reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        ht.Add(reader.GetString("courseid"), reader.GetString("name"));
                    }
                }
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("查询用户课程关系异常：" + ee.Message);
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
            foreach(DictionaryEntry de in ht) //ht为一个Hashtable实例
            {
                comboBox1.Items.Add(de.Value);
            }
            refreshTask(id);
        }

        private void refreshTask(int id)
        {
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            Hashtable ht = new Hashtable();

            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    OpenConn();
                }

                cmd = conn.CreateCommand();
                cmd.CommandText = "select c.name,t.status from hq_task t, hq_course c where t.courseid = c.id and t.userid=" + id;
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        ht.Add(reader.GetString("name"), reader.GetString("status"));
                    }
                }
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("查询任务表失败：" + ee.Message);
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
 
            foreach (DictionaryEntry de in ht) //ht为一个Hashtable实例
            {
                
			
			String key = de.Key.ToString();
            String value = "";
            if ("0".Equals(de.Value.ToString()))
            {
                value = "未执行";
            }
            else if ("1".Equals(de.Value.ToString()))
            {
                value = "执行中";
            }
            else if ("2".Equals(de.Value.ToString()))
            {
                value = "执行成功";
            }
            else if ("3".Equals(de.Value.ToString()))
            {
                value = "执行失败";
            }
            System.Windows.Forms.MessageBox.Show(key+"-" + value);
            ListViewItem first = new ListViewItem(new string[] {key,value }); 
			this.listView1.Items.Add(first);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;

            int courseId =-1;
            foreach (DictionaryEntry de in ht) //ht为一个Hashtable实例
            {
                if (comboBox1.SelectedText.Equals(de.Value.ToString())) {
                    courseId = int.Parse(de.Key.ToString());
                    break;
                }
            }
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    OpenConn();
                }

                cmd = conn.CreateCommand();
                cmd.CommandText = "insert into hq_task(userid,courseid,createtime,updatetime,status) values (" + myid + "," + courseId + ",now(),now()," + 0 + ")";
                reader = cmd.ExecuteReader();

                
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("查询用户课程关系异常：" + ee.Message);
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

        }  
    }
}
