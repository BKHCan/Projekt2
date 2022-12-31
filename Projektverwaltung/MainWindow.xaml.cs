using System;
using System.Windows;
using System.Windows.Controls;

namespace Projektverwaltung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //Application.Current.MainWindow = this;
            //Loaded += OnMainWindowLoaded;
            //FillDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Startseite startseiteWindow = new Startseite();
            startseiteWindow.Show();
            this.Close();
        }

        /* private void FillDataGrid()
         {

             OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Database.accdb");

             con.Open();
             OleDbDataAdapter cmd = new OleDbDataAdapter("SELECT ID, Nachname, Vorname FROM Mitarbeiter", con);
             DataTable dt = new DataTable();
             cmd.Fill(dt);
             //grdEmployee.ItemsSource = dt.DefaultView;
             con.Close();
             }*/
        /*

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            //ChangeView(new Startseite());
            
        }

        public void ChangeView(Startseite view)
        {
            //MainFrame.NavigationService.Navigate(view);
           // throw new NotImplementedException();
        }

        public void ChangeView(Page view)
        {
            MainFrame.NavigationService.Navigate(view);
        }*/


    }

}


