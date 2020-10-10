using System;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// ScriptRunner.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptRunner : Window
    {
        public static ScriptRunner Run(EventHandler closed)
        {
            ScriptRunner sr = new ScriptRunner();
            sr.Closed += closed;

            sr.Show();
            return sr;
        }

        private ScriptRunner()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
