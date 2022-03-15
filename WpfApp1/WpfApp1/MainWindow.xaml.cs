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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable categoryTable;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            categoryGrid.RowEditEnding += CategoryGrid_RowEditEnding;
        }

        private void Update_Window()
        {
            string sql = "SELECT * FROM Category";
            categoryTable = new DataTable();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                // install command on adding, for call contained procedure
                adapter.InsertCommand = new SqlCommand("sp_InsertCategory", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Designation", SqlDbType.NVarChar, 50, "Designation"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50, "Description"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                parameter.Direction = ParameterDirection.Output;

                connection.Open();
                adapter.Fill(categoryTable);
                categoryGrid.ItemsSource = categoryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Window();
        }

        private void updateDB()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(categoryTable);
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            updateDB();
            //Update_Window();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItems != null)
            {
                for(int i = 0; i < categoryGrid.SelectedItems.Count; i++)
                {
                    DataRowView dataRowView = categoryGrid.SelectedItems[i] as DataRowView;
                    if(dataRowView != null)
                    {
                        DataRow dataRow = (DataRow)dataRowView.Row;
                        dataRow.Delete();
                    }
                }
            }

            updateDB();
            //Update_Window();
        }

        private void CategoryGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            updateDB();
        }
    }
}
