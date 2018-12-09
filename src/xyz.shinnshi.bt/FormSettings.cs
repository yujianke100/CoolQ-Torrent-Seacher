using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Flexlive.CQP.CSharpPlugins.Demo
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            //加载标题。
            this.Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + " 参数设置";
            Master master = new Master();
            textBox1.Text = master.QQ + "";
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //参数保存处理代码。
            Master master = new Master();
            master.QQ = int.Parse(textBox1.Text);
            this.btnExit_Click(null, null);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar < '0' || e.KeyChar > '9';  //允许输入数字
            if (e.KeyChar == (char)8)  //允许输入回退键
            {
                e.Handled = false;
            }
        }
    }
}
