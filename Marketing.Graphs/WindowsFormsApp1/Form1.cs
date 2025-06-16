using Marketing.Graphs;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            chart1 = ServicoGraph.GerarGrafico("C:\\PROJETO\\MARKETING\\GRAVICO.JPG");
        }
    }
}
