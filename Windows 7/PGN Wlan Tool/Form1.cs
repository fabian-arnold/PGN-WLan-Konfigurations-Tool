using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NativeWifi;

namespace PGN_WLAN_TOOL
{
    public partial class Form1 : Form
    {
        WlanClient client;
        List <Wlan.WlanAvailableNetwork> hosts;
        public Form1()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// Wandelt den übergebenen String in die Hexadezimaldarstellung
        /// </summary>
        /// <param name="Hexstring">der umzuwandelnde String</param>
        /// <returns>Hexadezimaldarstellung</returns>
        private string StringToHex(string hexstring)
        {
            var sb = new StringBuilder();
            hexstring.Trim();
            foreach (char t in hexstring)
                sb.Append(Convert.ToInt32(t).ToString("x"));
            return sb.ToString().ToUpper();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            client = new WlanClient();
            hosts = new List<Wlan.WlanAvailableNetwork>();
            if (client.Interfaces.Length < 1)
            {
                if (MessageBox.Show("Keine WLan-Gerät verfügbar.\n Fortfahren?", "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {

                }
                else
                {
                    this.Close();
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string profileName, profileXml;
            profileName = "PGN-Wlan";
           
            try
            {
                profileXml = String.Format(Properties.Resources.Template, profileName, StringToHex(profileName));
                client.Interfaces[0].SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.GetBaseException().ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button2.Text = "W-Lan wurde konfiguriert!";
            button2.Enabled = false;

        }
    }
}
