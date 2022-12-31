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

namespace Projektverwaltung
{
    /// <summary>
    /// Interaktionslogik für Startseite.xaml
    /// </summary>
    public partial class Startseite
    {
        public Startseite()
        {
            InitializeComponent();
        }

        private void OpenMitarbeiterliste(object sender, RoutedEventArgs e)
         {
            /* 
               var mainWindow = (MainWindow)Application.Current.MainWindow;
               mainWindow?.ChangeView(new Mitarbeiterliste());
            */
            Mitarbeiterliste objMitarbeiterliste = new Mitarbeiterliste();
            //this.Visibility = Visibility.Hidden; // Versteckt aktuelles Fenster
            objMitarbeiterliste.Show();
            
        }


        private void OpenProjektliste(object sender, RoutedEventArgs e)
        {
            
            Projektliste objProjektliste = new Projektliste();
           // this.Visibility = Visibility.Hidden; // Versteckt aktuelles Fenster
            objProjektliste.Show();
            
        }
    }
}
