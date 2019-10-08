using NETCore.Encrypt;
using Newtonsoft.Json;
using SteamAccGen.JSON;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SteamAccGen.Forms
{
    public partial class Settings : Form
    {

        private string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";
        private bool BypassQuit = false;

        public Settings()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/dev/apikey");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Setup data varible
            var data = new dataJson();

            //Setup json
            data.key = textBox1.Text;
            var JSON = JsonConvert.SerializeObject(data);

            //Setup keys for encryption
            var creds = EncryptProvider.CreateAesKey();
            var key = creds.Key;
            var IV = creds.IV;

            //Encrypt data
            var encrypted = EncryptProvider.AESEncrypt(JSON, key, IV);

            //FInishing touches for encryption
            encrypted = key + "," + IV + "," + encrypted;

            //Write data to file
            File.WriteAllText(appDataPath + @"\data.json", encrypted);

            //Exit
            BypassQuit = true;
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //Setup data for use outside of if statement
            var data = new dataJson();

            //Create Defaults
            data.key = "";

            //Load current data
            if (File.Exists(appDataPath + @"\data.json"))
            {
                var creds = File.ReadAllText(appDataPath + @"\data.json").Split(Convert.ToChar(","));
                try
                {
                    data = JsonConvert.DeserializeObject<dataJson>(EncryptProvider.AESDecrypt(creds[2], creds[0], creds[1]));
                }
                catch
                {
                    File.Delete(appDataPath + @"\data.json");
                }
            }

            //Apply saved data
            textBox1.Text = data.key;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Handle close
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!BypassQuit)
                {
                    //e.Cancel = true;
                    Console.WriteLine("Unauthorized close attempt, cancelling");
                }
            }
            else
            {
                Console.WriteLine("Attempted close with reason: " + e.CloseReason);
            }
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }


    }
}
