using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace BudgetButikker
{
    class UnitAndCategoryHandler
    {
        StreamWriter sw;
        public string Path { get; set; }
        public string FileName { get; set; }
        public UnitAndCategoryHandler(string path)
        {
            Path = path;

        }
        public void AddUnit(string category, string unit)
        {
            FileName = string.Format("\\" + category + ".txt");
            sw = new StreamWriter(Path + FileName, true, Encoding.GetEncoding(1252));
            sw.WriteLine(string.Format(unit.TrimEnd('\n')));
            sw.Close();
        }

        public void AddCategory(string category)
        {
            try
            {
                FileName = "\\Category.txt";
                sw = new StreamWriter(Path + FileName, true, Encoding.GetEncoding(1252));
                sw.WriteLine(category);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void EditUnits(string[] units, string category)
        {
            FileName = string.Format("\\" + category + ".txt");
            sw = new StreamWriter(Path + FileName, false, Encoding.GetEncoding(1252));
            for (int i = 0; i < units.Length; i++)
            {
                sw.WriteLine(units[i]);
            }
            sw.Close();
        }

        public void EditCategorys(string[] Categorys)
        {
            FileName = "\\Category.txt";
            sw = new StreamWriter(Path + FileName, false, Encoding.GetEncoding(1252));
            for (int i = 0; i < Categorys.Length; i++)
            {
                sw.WriteLine(Categorys[i]);
            }
            sw.Close();
        }
    }
}
