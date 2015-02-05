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

namespace BudgetButikker
{
    /// <summary>
    /// Interaction logic for WinAddCategoryDialog1.xaml
    /// </summary>
    public partial class WinCategoryDialog : Window
    {
        public WinCategoryDialog()
        {
            InitializeComponent();
        }

        string category = null;
        public string Category
        {
            get { return category; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch(btn.Name)
            {
                case "btnAddCategory":
                    category = txtCategory.Text;
                    this.Close();
                    break;

                case "btnCancel":
                    category = null;
                    this.Close();
                    break;
            }
        }
    }
}
