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
    /// Interaction logic for WinEditCategory.xaml
    /// </summary>
    public partial class WinEditCategory : Window
    {
        public WinEditCategory()
        {
            InitializeComponent();
        }

        internal void WinEditCategorys(UnitAndCategoryHandler unitAndCategoryHandlerFromBase)
        {
            unitAndCategoryHandler = unitAndCategoryHandlerFromBase;
            ReadCategorysToList();
        }

        private UnitAndCategoryHandler unitAndCategoryHandler;
        public List<string> Categorys = new List<string>();

        private void ReadCategorysToList()
        {
            for (int i = 0; i < Categorys.Count; i++)
            {
                lstBoxCategorys.Items.Add(Categorys[i]);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] categorys;
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnRename":
                    //Gør dialog vinduet klar
                    WinCategoryDialog winEdit = new WinCategoryDialog();
                    winEdit.lblWhat.Content = "Omdøb Kategori:";
                    winEdit.Title = "Omdøb kategori";
                    winEdit.btnAddCategory.Content = "Omdøb";
                    winEdit.ShowDialog();
                    string category;
                    category = winEdit.Category;
                    if (category == null || category == "")
                    {
                        return;
                    }
                    lstBoxCategorys.Items[lstBoxCategorys.SelectedIndex] = category;

                    //Refresh listbox og fyld kategori arrayet med listboxens indhold
                    categorys = new string[lstBoxCategorys.Items.Count];
                    for (int i = 0; i < lstBoxCategorys.Items.Count; i++)
                    {
                        categorys[i] = lstBoxCategorys.Items[i].ToString();
                    }
                    lstBoxCategorys.Items.Clear();
                    for (int i = 0; i < categorys.Length; i++)
                    {
                        lstBoxCategorys.Items.Add(categorys[i]);
                    }

                    //Refresh category.txt fil 
                    unitAndCategoryHandler.EditCategorys(categorys);
                    break;

                case "btnDelete":
                    lstBoxCategorys.Items.Remove(lstBoxCategorys.SelectedItem);
                    categorys = new string[lstBoxCategorys.Items.Count];
                    for (int i = 0; i < lstBoxCategorys.Items.Count; i++)
                    {
                        categorys[i] = lstBoxCategorys.Items[i].ToString();
                    }
                    unitAndCategoryHandler.EditCategorys(categorys);
                    break;

                case "btnClose":
                    this.Close();
                    break;

                default:
                    break;
            }
        }
    }
}
