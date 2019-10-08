using NETCore.Encrypt;
using SteamAccGen.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SteamAccGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void ReloadAccounts()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";
            if (File.Exists(appDataPath + @"\accounts.json"))
            {
                var creds = File.ReadAllText(appDataPath + @"\accounts.json").Split(Convert.ToChar(","));
                var data = EncryptProvider.AESDecrypt(creds[2], creds[0], creds[1]);
                richTextBox1.Text = data;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Open Accounts";
            diag.Multiselect = false;
            if(diag.ShowDialog() == DialogResult.OK)
            {
                var filePath = diag.FileName;
                var fileContent = File.ReadAllLines(filePath);
                List<string> usernames = new List<string>();
                List<string> passwords = new List<string>();
                foreach(var line in fileContent)
                {
                    var split = line.Split(Convert.ToChar(":"));
                    var username = split[0];
                    var password = split[1];
                    usernames.Add(username);
                    passwords.Add(password);
                }
                try
                {
                    Forms.ImportedAccounts accs = new Forms.ImportedAccounts();
                    accs.Show();
                    accs.AssingParent(this);
                    accs.LoadAccounts(usernames, passwords);
                }
                catch { }
            }
        }

        private void Settings_button_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        public void ShowSettings()
        {
            Settings Form = new Settings();
            Form.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            if (File.Exists(appDataPath + @"\accounts.json"))
            {
                var creds = File.ReadAllText(appDataPath + @"\accounts.json").Split(Convert.ToChar(","));
                var data = EncryptProvider.AESDecrypt(creds[2], creds[0], creds[1]);
                richTextBox1.Text = data;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";
            if (File.Exists(appDataPath + @"\accounts.json"))
            {
                File.Delete(appDataPath + @"\accounts.json");
                richTextBox1.Text = "";
            }
        }
    }
}
