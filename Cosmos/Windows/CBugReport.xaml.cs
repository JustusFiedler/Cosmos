﻿using Cosmos.Classes;
using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cosmos.Windows
{
    public partial class CBugReport : Window
    {
        private static string DBPATH = "";
        private static OleDbCommand dbCommand;
        //private static OleDbDataAdapter dbDataAdapter;
        private static OleDbConnection conn;
        private string connectionstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source = ";

        /// <summary>
        /// Only for internal database
        /// </summary>
        public CBugReport(string supportDBPath, string email, string version, string product)
        {
            InitializeComponent();

            DBPATH = supportDBPath;
            connectionstring += DBPATH;
            conn = new OleDbConnection(connectionstring);

            DBPATH = supportDBPath;
            TB_email.Text = email;
            TB_version.Text = version;
            TB_product.Text = product;

            //todo: try to get current os and select in cb
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void B_send_Click(object sender, RoutedEventArgs e)
        {
            //check correctness of input data
            if (TB_description.Text.Replace(" ", string.Empty) == string.Empty || 
                CB_os.SelectedIndex < 0 ||
                CB_impact.SelectedIndex < 0 ||
                (TB_email.Text.Replace(" ", string.Empty) != "" && !CValidation.ValidateEmail(TB_email.Text)))
            {
                CMessageBox message = new CMessageBox("Bitte füllen sie alle Felder die mit einem * gekennzeichnet sind und überprüfen sie die Daten auf ihre Richtigkeit", "Eingabe Korrigieren", CColor.Theme.DarkGrey, CImage.ImageType.edit_black, CMessageBox.CMessageBoxButton.OK);
                message.ShowDialog();
                return;
            }

            string impact = string.Empty;
            if (CB_impact.SelectedIndex >= 0)
                impact = (CB_impact.SelectedItem as ComboBoxItem).Content.ToString();

            try
            {
                if (!InsertRequest(TB_email.Text, DateTime.Today, "", false, TB_description.Text, CB_os.Text, CB_priority.Text, TB_version.Text, impact, TB_product.Text))
                    return;
            }
            catch (Exception ex)
            {
                CMessageBox errormessage = new CMessageBox(ex.InnerException.Message.ToString(), "Datenbankfehler", CColor.Theme.Red, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                errormessage.ShowDialog();
                return;
            }

            CMessageBox donemessage = new CMessageBox("Vielen Dank.\nIhre Anfrage wird so schnell wie möglich bearbeitet.", "Hinweis", CColor.Theme.DarkGrey, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
            donemessage.ShowDialog();

            Close();
        }

        private bool InsertRequest(string email, DateTime? date, string editor, bool? done, string description, string os, string priority, string version, string impact, string product)
        {
            try
            {
                conn.Open();
            }
            catch
            {
                CMessageBox message = new CMessageBox("Datenbank \"" + DBPATH + "\" konnte nicht gefunden werden.", "Fehler", CColor.Theme.Red, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                message.ShowDialog();
                return false;
            }

            dbCommand = new OleDbCommand("INSERT INTO Anfragen (Email, Datum, Bearbeiter, Erledigt, Problembeschreibung, Betriebssystem, Prioritaet, Version, Schwere, Produkt) VALUES(@Email, @Datum, @Bearbeiter, @Erledigt, @Problembeschreibung, @Betriebssystem, @Prioritaet, @Version, @Schwere, @Produkt)", conn);

            dbCommand.Parameters.Add("@Email", OleDbType.VarChar).Value = email;
            dbCommand.Parameters.Add("@Datum", OleDbType.Date).Value = date;
            dbCommand.Parameters.Add("@Bearbeiter", OleDbType.VarChar).Value = editor;
            dbCommand.Parameters.Add("@Erledigt", OleDbType.Boolean).Value = done;
            dbCommand.Parameters.Add("@Problembeschreibung", OleDbType.VarChar).Value = description;
            dbCommand.Parameters.Add("@Betriebssystem", OleDbType.VarChar).Value = os;
            dbCommand.Parameters.Add("@Prioritaet", OleDbType.VarChar).Value = priority;
            dbCommand.Parameters.Add("@Version", OleDbType.VarChar).Value = version;
            dbCommand.Parameters.Add("@Schwere", OleDbType.VarChar).Value = impact;
            dbCommand.Parameters.Add("@Produkt", OleDbType.VarChar).Value = product;

            foreach (OleDbParameter _parameter in dbCommand.Parameters)
                if (_parameter.Value == null)
                    _parameter.Value = DBNull.Value;

            //dbDataAdapter = new OleDbDataAdapter(dbCommand);
            dbCommand.ExecuteNonQuery();

            conn.Close();
            return true;
        }
    }
}
