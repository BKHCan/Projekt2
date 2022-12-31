using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
    /// Interaktionslogik für Projektliste.xaml
    /// </summary>
    public partial class Projektliste : Window
    {
        Projektverwaltung.DatabaseDataSet databaseDataSet { get; set; }
        Projektverwaltung.DatabaseDataSetTableAdapters.ProjekteTableAdapter databaseDataSetProjekteTableAdapter { get; set; }


        

        public class TreeItem
        {
            public string iName
            {
                get;
                set;
            }

            public string iImage
            {
                get;
                set;
            }
        }

        OleDbCommand cmd = new OleDbCommand();
        OleDbTransaction transaction = null;
        OleDbConnection con;
        OleDbDataReader dre;
        DB_Help _db_help;
        public Projektliste()
        {
            InitializeComponent();
            _db_help = new DB_Help("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Database.accdb");
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Database.accdb");
            con.Open();
            con.Close();
            btn_delete.IsEnabled = false;
           // tre.DisplayMemberPath = "img";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            databaseDataSet = ((Projektverwaltung.DatabaseDataSet)(this.FindResource("databaseDataSet")));
            // Lädt Daten in Tabelle "Projekte". Sie können diesen Code nach Bedarf ändern.
            databaseDataSetProjekteTableAdapter = new Projektverwaltung.DatabaseDataSetTableAdapters.ProjekteTableAdapter();
            databaseDataSetProjekteTableAdapter.Fill(databaseDataSet.Projekte);
            System.Windows.Data.CollectionViewSource projekteViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("projekteViewSource")));
            projekteViewSource.View.MoveCurrentToFirst();


            

            con.Open();
            cmd = new OleDbCommand("Select * from Mitarbeiter", con);
            dre = cmd.ExecuteReader();
            while (dre.Read())
            {
                cmb_emp.Items.Add(new ListBoxItem
                {
                    Tag = dre["id"],
                    Content = dre["Nachname"] + " " + dre["Vorname"]
                });
            }
            con.Close();


            cmb_parent_id.Items.Add(new ListBoxItem
            {
                Tag = "0",
                Content = "Parent Project"
            });
            cmb_parent_id.SelectedIndex = 0;

            con.Open();
            cmd = new OleDbCommand("Select * from Projekte where [ist Projektmappe]=true;", con);
            dre = cmd.ExecuteReader();
            while (dre.Read())
            {
                cmb_parent_id.Items.Add(new ListBoxItem
                {
                    Tag = dre["id"],
                    Content = dre["titel"]
                });
            }
            con.Close();

            cmb_pr.SelectedIndex = 0;
            BindToTree();
        }
    
        private void BindToTree()
        {
            //tre.ItemsSource = databaseDataSet.Projekte;
            TreeViewItem main_tvi = new TreeViewItem
            {
                Header = new TreeItem()
                {
                    iName = "Project Solutions",
                    iImage = "Images/Icons/ofolder.png"
                },
                Uid = "0"
            };
            List<DB_Help.Projekte> list_pr = new List<DB_Help.Projekte>();
            //GEt All Parent Projects
            if (con.State != ConnectionState.Open)
                con.Open();
            
            cmd = new OleDbCommand("Select * from Projekte where [ist Projektmappe]=true;", con);
            dre=cmd.ExecuteReader();
            while (dre.Read()) 
            {
                DB_Help.Projekte _pr = new DB_Help.Projekte();
                _pr.Titel = dre["Titel"] + "";
                _pr.Erledigt = dre["Erledigt"] + "";
                _pr.ID = int.Parse( dre["id"] + "");
                list_pr.Add(_pr);
            }
            con.Close();


            //Get All Child Project
            for (int i = 0; i < list_pr.Count; i++)
            {
                TreeViewItem pr_tvi = new TreeViewItem
                {
                    Header = new TreeItem()
                    {
                        iName = list_pr[i].Titel,
                        iImage = "Images/Icons/ofolder.png"
                    },
                    Uid = ""+list_pr[i].ID
                };
                if (con.State != ConnectionState.Open)
                    con.Open();
                
                cmd = new OleDbCommand("Select * from Projekte where ParrentID=" + list_pr[i].ID, con);
                dre = cmd.ExecuteReader();
                while (dre.Read())
                {
                    string im_pat = "";
                    if ((dre["Erledigt"]+"").ToLower()=="true")
                    {
                        im_pat = "Images/Icons/ok2.png";
                    }
                    else
                    {
                        im_pat = "Images/Icons/not2.png";
                    }
                    TreeViewItem child_tvi = new TreeViewItem
                    {
                        Header = new TreeItem()
                        {
                            iName = dre["Titel"] + "",
                            iImage = im_pat
                        },
                        Uid = "" + dre["id"]
                    };
                    pr_tvi.Items.Add(child_tvi);
                    
                }
                con.Close();
                main_tvi.Items.Add(pr_tvi);
            }
            tre.Items.Clear();
            tre.Items.Add(main_tvi);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        string pr_id = "";
        private void tre_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue != null)
            {
                var selectedTreeViewItem = e.NewValue as TreeViewItem;
                if (selectedTreeViewItem.Uid != "0")
                {
                    pr_id = selectedTreeViewItem.Uid;
                    btn_delete.IsEnabled = true;
                }
                else
                {
                    pr_id = "0";
                    btn_delete.IsEnabled = false;
                }
                //MessageBox.Show(selectedTreeViewItem.Uid);
            }
        }

        private void btn_add_emp_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_emp.SelectedItem != null)
            {
                ListBoxItem lit = cmb_emp.SelectedItem as ListBoxItem;
                if (con.State != ConnectionState.Open)
                    con.Open();
                
                cmd = new OleDbCommand("Select * from Mitarbeiter where id=" + lit.Tag, con);
                dre = cmd.ExecuteReader();
                while (dre.Read())
                {
                    li_emp.Items.Add(new ListBoxItem
                    {
                        Tag = dre["id"],
                        Content = dre["Nachname"] + " " + dre["Vorname"]
                    });
                }
                con.Close();
            }
        }

        private void btn_new_pr_sol_Click(object sender, RoutedEventArgs e)
        {
            DB_Help.Projekte obj = new DB_Help.Projekte();
            obj.Titel = txt_title.Text;
            obj.Ressourcen = txt_res.Text;
            obj.Beschreibung = txt_bes.Text;
            obj.Startdatum = date_p_start.Text;
            obj.Enddatum = date_p_end.Text;
            obj.ist_Projektmappe = "True";
            obj.Erledigt = ""+chk_erledigt.IsChecked;
            obj.Priorität = int.Parse( cmb_pr.Text);
            
            obj.ParrentID = "0";
            _db_help.Insert_Projekte(obj);
            con.Open();
            cmd.CommandText = "SELECT top 1 id from projekte order by id desc";
            int newId = (int)cmd.ExecuteScalar();
            con.Close();
            DB_Help.Projektbeteiligung obj_2 = new DB_Help.Projektbeteiligung();
            obj_2.ProjektID = newId;

            for (int i = 0; i < li_emp.Items.Count; i++)
            {
                ListBoxItem _lit = li_emp.Items[i] as ListBoxItem;
                obj_2.MtarbeiterID = int.Parse("" + _lit.Tag);
                _db_help.Insert_Projektbeteiligung(obj_2);
            }

            MessageBox.Show("New Entery " + newId);
            BindToTree();
        }

        private void btn_new_pr_Click(object sender, RoutedEventArgs e)
        {
            DB_Help.Projekte obj = new DB_Help.Projekte();
            obj.Titel = txt_title.Text;
            obj.Ressourcen = txt_res.Text;
            obj.Beschreibung = txt_bes.Text;
            obj.Startdatum = date_p_start.Text;
            obj.Enddatum = date_p_end.Text;
            obj.ist_Projektmappe = "False";
            obj.Erledigt = "" + chk_erledigt.IsChecked;
            obj.Priorität = int.Parse(cmb_pr.Text);
            ListBoxItem lit = cmb_parent_id.SelectedItem as ListBoxItem;
            obj.ParrentID = (lit.Tag+"");
            _db_help.Insert_Projekte(obj);
            con.Open();
            cmd.CommandText = "SELECT top 1 id from projekte order by id desc";
            int newId = (int)cmd.ExecuteScalar();
            con.Close();
            DB_Help.Projektbeteiligung obj_2 = new DB_Help.Projektbeteiligung();
            obj_2.ProjektID = newId;

            for (int i = 0; i < li_emp.Items.Count; i++)
            {
                ListBoxItem _lit = li_emp.Items[i] as ListBoxItem;
                obj_2.MtarbeiterID = int.Parse(""+ _lit.Tag);
                _db_help.Insert_Projektbeteiligung(obj_2);
            }

            MessageBox.Show("New Entery "+newId);
            BindToTree();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbTransaction transaction = null;
            if (con.State != ConnectionState.Open)
                con.Open();
            transaction = con.BeginTransaction();
            cmd.Connection = con;
            cmd.Transaction = transaction;
            cmd.CommandText = "DELETE FROM Projekte WHERE ID=" + pr_id.ToString();
            cmd.ExecuteNonQuery();
            transaction.Commit();
            con.Close();
            BindToTree();
        }

        private void cmb_parent_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmb_parent_id.SelectedIndex == -1) { }
            else
            {
                if (cmb_parent_id.SelectedIndex == 0) {
                    btn_new_pr.IsEnabled = false;
                    btn_new_pr_sol.IsEnabled = true; }
                else
                {
                    btn_new_pr.IsEnabled = true;
                    btn_new_pr_sol.IsEnabled = false;
                }
            }
        }
    }
}
