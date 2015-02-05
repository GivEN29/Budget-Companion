using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetButikker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            readSettingsAndCategoryFiles();
        }

        UnitAndCategoryHandler unitAndCategoryHandler;
        string workDirectoryAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Budget Companion";
        string[] strCategorys;
        /// <summary>
        /// Read The settings file and set the path for config files
        /// Read the category file and set the combobox = every category
        /// </summary>
        private void readSettingsAndCategoryFiles()
        {
            //Settings File
            if (!Directory.Exists(workDirectoryAppData))
            {
                Directory.CreateDirectory(workDirectoryAppData);
            }
            try
            {
                string[] strSettings = File.ReadAllLines(string.Format(workDirectoryAppData + "\\settings.txt"), Encoding.GetEncoding(1252));
                Console.Write(strSettings[0]);
                txtConfigFilesPath.Text = strSettings[0];
                unitAndCategoryHandler = new UnitAndCategoryHandler(strSettings[0]);
                
                //Category file
                if (!File.Exists(strSettings[0] + "\\Category.txt"))
                {
                    return;
                }
                else
                {
                    cbCategory.Items.Clear();
                    strCategorys = File.ReadAllLines(string.Format(strSettings[0] + "\\Category.txt"), Encoding.GetEncoding(1252));
                    for (int i = 0; i < strCategorys.Length; i++)
                    {
                        cbCategory.Items.Add(strCategorys[i]);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Det ser ud som om at der ikke valgt nogen mappe til config filerne\nVælg den venligst nu");
                
                //Show settings menu
                MainMenu.Opacity = 0;
                MainMenu.Visibility = Visibility.Hidden;
                SettingsMenu.Opacity = 1;
                SettingsMenu.Visibility = Visibility.Visible;
                btnMainMenu.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw;
            Button btn = (Button)sender;
            switch(btn.Name)
            {
                case "btnSettingsMenu":
                    MainMenu.Opacity = 0;
                    MainMenu.Visibility = Visibility.Hidden;
                    SettingsMenu.Opacity = 1;
                    SettingsMenu.Visibility = Visibility.Visible;
                    break;

                case "btnMainMenu":
                    SettingsMenu.Opacity = 0;
                    SettingsMenu.Visibility = Visibility.Hidden;
                    MainMenu.Opacity = 1;
                    MainMenu.Visibility = Visibility.Visible;
                    break;

                case "btnUnit":
                    WinCategoryDialog winAddUnitDialog = new WinCategoryDialog();
                    winAddUnitDialog.lblWhat.Content = "Enhed:";
                    winAddUnitDialog.Title = "Tilføj Enhed";
                    winAddUnitDialog.ShowDialog();
                    string unit;
                    unit = winAddUnitDialog.Category;
                    if (unit == null || unit == "")
                    {
                        return;
                    }
                    unitAndCategoryHandler.AddUnit(cbCategory.SelectionBoxItem.ToString(), unit);
                    lstbxUnits.Items.Add(unit);
                    MessageBox.Show(unit + " tilføjet succesfuldt under: " + cbCategory.SelectionBoxItem.ToString());
                    break;

                case "btnAddCategory":
                    WinCategoryDialog winAddCategoryDialog = new WinCategoryDialog();
                    winAddCategoryDialog.ShowDialog();
                    string category;
                    category = winAddCategoryDialog.Category;
                    if (category == null || category == "")
                    {
                        return;
                    }
                    try
                    {
                        unitAndCategoryHandler.AddCategory(category);
                        readSettingsAndCategoryFiles();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;

                case "btnEditCategory":
                    WinEditCategory WinEdit = new WinEditCategory();
                    for (int i = 0; i < cbCategory.Items.Count; i++)
                    {
                        WinEdit.Categorys.Add(cbCategory.Items[i].ToString());
                    }
                    WinEdit.WinEditCategorys(unitAndCategoryHandler);
                    WinEdit.ShowDialog();
                    readSettingsAndCategoryFiles();
                    break;

                case "btnChangeConfigPath":
                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result.ToString() == "Cancel")
                    {
                        MessageBox.Show("Du skal vælge en mappe");
                        btnMainMenu.IsEnabled = false;
                        return;
                    }
                    sw = new StreamWriter(workDirectoryAppData + "\\settings.txt", false, Encoding.GetEncoding(1252));
                    txtConfigFilesPath.Text = dialog.SelectedPath;
                    sw.WriteLine(dialog.SelectedPath);
                    sw.Close();
                    btnMainMenu.IsEnabled = true;
                    readSettingsAndCategoryFiles();
                    break;

                case "btnResetProgram":
                    if (MessageBox.Show("Dette vil gendane programmet fuldstændigt\nDu mister alle dine katagorier og enheder\nVil du forsætte?",
                        "ADVARSEL", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }

                    try
                    {
                        cbCategory.Items.Clear();
                        txtConfigFilesPath.Text = "";
                        File.Delete(workDirectoryAppData + "\\settings.txt");
                        File.Delete(txtConfigFilesPath.Text + "\\Category.txt");
                        for (int i = 0; i < strCategorys.Length; i++)
                        {
                            File.Delete(txtConfigFilesPath.Text + "\\" + strCategorys[i] + ".txt");
                        }
                    }
                    catch (Exception)
                    {
                    }
                    break;
            }
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstbxUnits.Items.Clear();
            if (cbCategory.SelectedIndex == -1)
            {
                return;
            }
            if (!File.Exists(txtConfigFilesPath.Text + "\\" + cbCategory.SelectedItem.ToString() + ".txt"))
            {
                return;
            }
            string[] units = File.ReadAllLines(txtConfigFilesPath.Text + "\\" + cbCategory.SelectedItem.ToString() + ".txt", Encoding.GetEncoding(1252));
            for (int i = 0; i < units.Length; i++)
            {
                lstbxUnits.Items.Add(units[i]);
            }
        }

        private void lstbxUnits_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                lstbxUnits.Items.Remove(lstbxUnits.SelectedItem);
                string[] units = new string[lstbxUnits.Items.Count];
                for (int i = 0; i < lstbxUnits.Items.Count; i++)
                {
                    units[i] = lstbxUnits.Items[i].ToString();
                }
                unitAndCategoryHandler.EditUnits(units, cbCategory.SelectedItem.ToString());
            }
        }
    }
}
