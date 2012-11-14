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

        private void button1_Click(object sender, EventArgs e)
        {
            if (client.Interfaces.Length < 1)
            {
                MessageBox.Show("Keine WLan-Gerät verfügbar.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
         /*   foreach (Wlan.WlanProfileInfo profileInfo in client.Interfaces[0].GetProfiles())
            {
                string name = profileInfo.profileName; // this is typically the network's SSID
                string xml = client.Interfaces[0].GetProfileXml(profileInfo.profileName);
                Clipboard.SetText(xml);
                MessageBox.Show(xml);
                
            }*/
            button2.Enabled = false;
            if (client.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Disconnected)
            {
                client.Interfaces[0].Scan();
                comboBox1.Items.Clear();
                hosts.Clear();
                
                    
                foreach (Wlan.WlanAvailableNetwork host in client.Interfaces[0].GetAvailableNetworkList(0))
                {
                    if (host.dot11DefaultAuthAlgorithm == Wlan.Dot11AuthAlgorithm.RSNA)
                    {
                        string ssid = System.Text.Encoding.UTF8.GetString(host.dot11Ssid.SSID).Remove((int)host.dot11Ssid.SSIDLength);
                        //MessageBox.Show(StringToHex(ssid));
                        comboBox1.Items.Add(ssid);
                        hosts.Add(host);
                        comboBox1.SelectedIndex = 0;
                        comboBox1.Enabled = true;
                        button2.Enabled = true;
                    }
                    
                }
                
                
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Fehler: Device " + client.Interfaces[0].InterfaceState.ToString());
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = false;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new WlanClient();
            hosts = new List<Wlan.WlanAvailableNetwork>();
            if (client.Interfaces.Length < 1)
            {
                MessageBox.Show("Keine WLan-Gerät verfügbar.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client.Interfaces.Length < 1)
            {
                MessageBox.Show("Keine WLan-Gerät verfügbar.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (client.Interfaces[0].InterfaceState == Wlan.WlanInterfaceState.Disconnected)
            {

                string profileName, profileXml;
                profileName = comboBox1.SelectedItem.ToString();
                if (profileName.Length > 1)
                {
                    try
                    {
                        profileXml = String.Format(Properties.Resources.Template, profileName, StringToHex(profileName));
                        client.Interfaces[0].SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler: " + ex.GetBaseException().ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Fehler");
                }
            }
            else
            {
                MessageBox.Show("Fehler: Device " + client.Interfaces[0].InterfaceState.ToString());
            }
            
            
        }
    }
}
