using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// ScriptRunner.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptRunner : Window
    {
        public static ScriptRunner Run(EventHandler closed) {
            var sr = new ScriptRunner();
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
