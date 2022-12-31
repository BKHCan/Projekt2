using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Projektverwaltung
{
    public class DB_Help
    {

        #region Objects
        OleDbCommand cmd = new OleDbCommand();
        OleDbTransaction transaction = null;
        OleDbConnection con;
       public DB_Help(string ConnectionString)
        {
            con = new OleDbConnection(ConnectionString);
            //Testing Connection
            con.Open();
            con.Close();
        }
        #endregion

        public class Mitarbeiter
        {
            public int ID { get; set; }
            public string Nachname { get; set; }
            public string Vorname { get; set; }
            public string Adresse { get; set; }
            public string Telefon { get; set; }
            public string Email { get; set; }
            public byte[] Bild { get; set; }



        }
        public class Projektbeteiligung
        {
            public int ID { get; set; }
            public int MtarbeiterID { get; set; }
            public int ProjektID { get; set; }

        }

        public class Projekte 
        {
            public int ID { get; set; }
            public string Titel { get; set; }
            public string Startdatum { get; set; }
            public string Enddatum { get; set; }
            public int Priorität { get; set; }
            public string Ressourcen { get; set; }
            public string Beschreibung { get; set; }
            public string Erledigt { get; set; }
            public string ist_Projektmappe { get; set; }
            public string ParrentID { get; set; }


        }
        public bool Insert_Mitarbeiter(Mitarbeiter obj)
        {
            bool op=true;
            try
            {
                cmd = new OleDbCommand();
                transaction = null;
                if (con.State != ConnectionState.Open)
                    con.Open();

                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO Mitarbeiter(Nachname,Vorname,Adresse,Telefon,Email,Bild) VALUES(@Nachname,@Vorname,@Adresse,@Telefon,@Email,@Bild)";
                // Fixing "" ' "" Error
                cmd.Parameters.AddWithValue("Nachname", obj.Nachname);
                cmd.Parameters.AddWithValue("Vorname", obj.Vorname);
                cmd.Parameters.AddWithValue("Adresse", obj.Adresse);
                cmd.Parameters.AddWithValue("Telefon", obj.Telefon);
                cmd.Parameters.AddWithValue("Email", obj.Email);
                cmd.Parameters.AddWithValue("Bild", obj.Bild);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                //Close Connection No need for continuous Connection
                con.Close();

            }
            catch (Exception ex)
            {
                op = false;
                throw ex;
                
                
            }
            return op;
        }
        public bool Update_Mitarbeiter(Mitarbeiter obj,int id)
        {
            bool op = true;
            try
            {
                cmd = new OleDbCommand();
                transaction = null;
                if (con.State != ConnectionState.Open)
                    con.Open();

                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;
                if (obj.Bild == null) {
                    cmd.CommandText = "Update Mitarbeiter set Nachname=@Nachname,Vorname=@Vorname,Adresse=@Adresse,Telefon=@Telefon,Email=@Email where id=@id ";
                }
                else { cmd.CommandText = "Update Mitarbeiter set Bild=@Bild , Nachname=@Nachname,Vorname=@Vorname,Adresse=@Adresse,Telefon=@Telefon,Email=@Email where id=@id ";
                 cmd.Parameters.AddWithValue("Bild", obj.Bild); }
                // Fixing "" ' "" Error
                cmd.Parameters.AddWithValue("Nachname", obj.Nachname);
                cmd.Parameters.AddWithValue("Vorname", obj.Vorname);
                cmd.Parameters.AddWithValue("Adresse", obj.Adresse);
                cmd.Parameters.AddWithValue("Telefon", obj.Telefon);
                cmd.Parameters.AddWithValue("Email", obj.Email);
                
               
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteReader();
                transaction.Commit();
                //Close Connection No need for continuous Connection
                con.Close();

            }
            catch (Exception ex)
            {
                op = false;
                throw ex;


            }

            return op;
        }
        public bool Delete_Mitarbeiter(int id)
        {
            bool op = true;
            try
            {

            }
            catch (Exception)
            {
                op = false;

            }
            return op;
        }

        public bool Insert_Projektbeteiligung(Projektbeteiligung obj )
        {
            bool op = true;
            try
            {
                cmd = new OleDbCommand();
                transaction = null;
                if (con.State != ConnectionState.Open)
                    con.Open();

                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO Projektbeteiligung(MtarbeiterID,ProjektID) VALUES(@MtarbeiterID,@ProjektID)";
                // Fixing "" ' "" Error
                cmd.Parameters.AddWithValue("MtarbeiterID", obj.MtarbeiterID);
                cmd.Parameters.AddWithValue("ProjektID", obj.ProjektID);
                
                cmd.ExecuteNonQuery();
                transaction.Commit();
                //Close Connection No need for continuous Connection
                con.Close();

            }
            catch (Exception ex)
            {
                op = false;
                throw ex;


            }
            
            return op;
        }
        public bool Update_Projektbeteiligung(Projektbeteiligung obj,int id)
        {
            bool op = true;
            try
            {

            }
            catch (Exception)
            {
                op = false;

            }
            return op;
        }
        public bool Delete_Projektbeteiligung(int id)
        {
            bool op = true;
            try
            {

            }
            catch (Exception)
            {
                op = false;

            }
            return op;
        }


        public bool Insert_Projekte(Projekte obj)
        {
            bool op = true;
            try
            {
                cmd = new OleDbCommand();
                transaction = null;
                if (con.State != ConnectionState.Open)
                    con.Open();

                transaction = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = transaction;
                cmd.CommandText = "INSERT INTO  Projekte(Titel,Startdatum,Enddatum,Priorität,Ressourcen,Beschreibung,Erledigt,[ist Projektmappe],ParrentID) VALUES(@Titel,@Startdatum,@Enddatum,@Priorität,@Ressourcen,@Beschreibung,@Erledigt,@ist_Projektmappe,@ParrentID)";
                // Fixing "" ' "" Error
                cmd.Parameters.AddWithValue("Titel", obj.Titel);
                cmd.Parameters.AddWithValue("Startdatum", obj.Startdatum);
                cmd.Parameters.AddWithValue("Enddatum", obj.Enddatum);
                cmd.Parameters.AddWithValue("Priorität", obj.Priorität);
                cmd.Parameters.AddWithValue("Ressourcen", obj.Ressourcen);
                cmd.Parameters.AddWithValue("Beschreibung", obj.Beschreibung);
                cmd.Parameters.AddWithValue("Erledigt", bool.Parse(obj.Erledigt));
                cmd.Parameters.AddWithValue("ist_Projektmappe",bool.Parse( obj.ist_Projektmappe));
                cmd.Parameters.AddWithValue("ParrentID", int.Parse(obj.ParrentID));
                cmd.ExecuteNonQuery();
                transaction.Commit();
                //Close Connection No need for continuous Connection
                con.Close();

            }
            catch (Exception ex)
            {
                op = false;
                throw ex;


            }
            
            return op;
        }
        public bool Update_Projekte(Projekte obj,int id)
        {
            bool op = true;
            try
            {

            }
            catch (Exception)
            {
                op = false;

            }
            return op;
        }
        public bool Delete_Projekte(int id)
        {
            bool op = true;
            try
            {

            }
            catch (Exception)
            {
                op = false;

            }
            return op;
        }
        public  byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
                filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] photo = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return photo;
        }

    }
}
