using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProgram
{

    public delegate void GoButtonEventHandler(string code);

    public partial class LoginForm : Form
    {
        private GoButtonEventHandler goPushed;

        public LoginForm(GoButtonEventHandler goPushed)
        {
            InitializeComponent();
            this.goPushed = goPushed;
            //this.FormClosing +=
            //    new FormClosingEventHandler(formClosing);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Go_Click(object sender, EventArgs e)
        {
            this.goPushed(this.codeTextBox.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        //void formClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Close();
        //}
    }
}
