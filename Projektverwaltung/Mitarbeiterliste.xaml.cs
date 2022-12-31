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
using System.Data.OleDb;
using System.Data;
using Microsoft.Win32;
using System.IO;

namespace Projektverwaltung
{
    /// <summary>
    /// Interaktionslogik für Mitarbeiterliste.xaml
    /// </summary>
    public partial class Mitarbeiterliste : Window
    {
        OleDbConnection con;
        DataTable dt;
        DB_Help _db_Help;

        public Mitarbeiterliste()
        {
            InitializeComponent();
            _db_Help = new DB_Help("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Database.accdb");
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Database.accdb";
            BindGrid();

        }

        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Mitarbeiter";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.DataContext = dt;
            gvData.Visibility = System.Windows.Visibility.Visible;
            //gvData.Columns[gvData.Columns.Count - 1].Visibility = Visibility.Collapsed;
        }
        byte[] bits;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            

            DB_Help.Mitarbeiter obj = new DB_Help.Mitarbeiter();
            obj.Nachname = nachname.Text;
            obj.Vorname = vorname.Text;
            obj.Adresse = adresse.Text;
            obj.Email = mail.Text;
            obj.Telefon=telefon.Text;
            obj.Bild = bits;
            _db_Help.Insert_Mitarbeiter(obj);
            BindGrid();
          

           
           
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                DB_Help.Mitarbeiter obj = new DB_Help.Mitarbeiter();

                obj.Nachname = row["Nachname"].ToString();
                obj.Vorname = row["Vorname"].ToString();
                obj.Adresse = row["Adresse"].ToString();
                obj.Email = row["Email"].ToString();
                obj.Telefon = row["Telefon"].ToString();
                if(row["Bild"] is DBNull) { obj.Bild = null; }
                else { obj.Bild = (byte[])row["Bild"]; }
               
                obj.ID = (int.Parse(row["ID"].ToString()));
                
                _db_Help.Update_Mitarbeiter(obj,obj.ID);
                BindGrid();
            }
           
            
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                if (con.State != ConnectionState.Open)
                    con.Open();
                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandText = "DELETE FROM Mitarbeiter WHERE ID=" + row["ID"].ToString();
                cmd.ExecuteNonQuery();
                transaction.Commit();
                BindGrid();
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        { }


        private void GoBackToMain(object sender, RoutedEventArgs e)
        {
            MainWindow objMainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden; // Versteckt aktuelles Fenster
            //objProjektliste.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                profilbild.Source = new BitmapImage(new Uri(op.FileName));
                bits = _db_Help.GetPhoto(op.FileName);
            }
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
