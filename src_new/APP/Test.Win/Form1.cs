using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mini.Foundation.Basic.Utility;

namespace Test.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Configuration configer = MyDll.GetDllConfiguration(MyDll.GetCurrentDllConfigFilePath());
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(configer.AppSettings.Settings["ShowMsg"]?.Value);
        }

        AssemblyConfiger asmConfiger = new AssemblyConfiger();
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(asmConfiger.AppSettings("ShowMsg"));
        }
    }
}
