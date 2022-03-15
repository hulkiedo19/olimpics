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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using WpfApp2.Models;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        CategoryContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new CategoryContext();
            db.Categories.Load();
            categoryGrid.ItemsSource = db.Categories.Local.ToBindingList();// устанавливаем привязку к кэшу

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItems.Count > 0)
            {
                for(int i = 0; i < categoryGrid.SelectedItems.Count; i++)
                {
                    Category category = categoryGrid.SelectedItems[i] as Category;
                    if(category != null)
                    {
                        db.Categories.Remove(category);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}
