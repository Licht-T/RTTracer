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
    public partial class MainWindow : Form
    {
        private MainProgram mainProgram;

        public MainWindow()
        {
            InitializeComponent();
            this.mainProgram = new MainProgram(this);
        }

        private void CancelClick(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.mainProgram.loginCheck();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mainProgram.startAuth();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void Go_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            btn.Enabled = false;
            GraphData data = await Task.Run( ()=>mainProgram.createGraphData(long.Parse(this.StatusId.Text)) );
            GraphCreator gc = new GraphCreator(data);
            btn.Enabled = true;
        }
    }
}
