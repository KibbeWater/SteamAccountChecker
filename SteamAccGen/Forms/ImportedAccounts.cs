using NETCore.Encrypt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamAccGen.JSON;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SteamAccGen.Forms
{
    public partial class ImportedAccounts : Form
    {
        public ImportedAccounts()
        {
            InitializeComponent();
        }

        private void ImportedAccounts_Load(object sender, EventArgs e)
        {

        }

        private List<string> usernames;
        private List<string> passwords;
        private int AccountIndex = 0;
        private int AccountIndexMax;
        private bool Loaded;
        private string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";

        static SteamClient steamClient;
        static CallbackManager manager;

        static SteamUser steamUser;

        static bool isRunning;

        bool importing = false;

        private string unbannedAccs = "Unbanned:";
        private string bannedAccs = "Banned:";

        private Form1 parent = null;

        public void AssingParent(Form1 formParent)
        {
            parent = formParent;
        }

        public void LoadAccounts(List<string> LoadUsernames, List<string> LoadPasswords)
        {
            usernames = LoadUsernames;
            passwords = LoadPasswords;
            Loaded = true;
            AccountIndex = 0;
            AccountIndexMax = LoadPasswords.Count;
            DisplayAccount(AccountIndex);
            Console.WriteLine("[Form] Succesfully loaded accounts\n   Usernames Count: " + usernames.Count + "\n   Passwords Count: " + passwords.Count + "\n");
            Console.WriteLine("[Form] Loading accounts and displaying..." + "\n");
        }

        private void Account_next_Click(object sender, EventArgs e)
        {
            if (Loaded)
            {
                AccountIndex += 1;
                if(AccountIndex > AccountIndexMax-1)
                {
                    AccountIndex = 1;
                }
                Console.WriteLine("[Form] Loading account\n   AccountIndex: " + AccountIndex + "\n   AccountIndexMax: " + AccountIndexMax + "\n");
                DisplayAccount(AccountIndex);
            }
            else
            {
                Console.WriteLine("[Form] Reached end, returning to start" + "\n");
            }
        }

        private void Account_back_Click(object sender, EventArgs e)
        {
            if (Loaded)
            {
                AccountIndex -= 1;
                if (AccountIndex < 0)
                {
                    AccountIndex = AccountIndexMax-1;
                }
                Console.WriteLine("[Form] Loading account\n   AccountIndex: " + AccountIndex + "\n   AccountIndexMax: " + AccountIndexMax + "\n");
                DisplayAccount(AccountIndex);
            }
            else
            {
                Console.WriteLine("[Form] Reached start, returning to end" + "\n");
            }
        }

        private void DisplayAccount(int id)
        {
            Console.WriteLine("[Form] Loading data to put on display" + "\n");
            Console.WriteLine("[Form] Displaying Account\n   AccountIndex: " + id + "\n   AccountIndexMax: " + AccountIndexMax + "\n");
            account_username.Text = usernames[id];
            account_password.Text = passwords[id];
            account_next.Enabled = false;
            account_back.Enabled = false;
            GetBanned();
        }

        private void GetBanned()
        {
            Console.WriteLine("[Steam] Checking Account if Banned" + "\n");
            account_status.Text = "Loading...";
            account_status.ForeColor = Color.Blue;
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();

            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            isRunning = true;

            Console.WriteLine("[Steam] Connecting to Steam..." + "\n");

            // initiate the connection
            steamClient.Connect();

            // create our callback handling loop
            while (isRunning)
            {
                // in order for the callbacks to get routed, they need to be handled by the manager
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }

            if (File.Exists(appDataPath + @"\data.json"))
            {
                var creds = File.ReadAllText(appDataPath + @"\data.json").Split(Convert.ToChar(","));
                var data = JsonConvert.DeserializeObject<dataJson>(EncryptProvider.AESDecrypt(creds[2], creds[0], creds[1]));
            }
            else
            {
                MessageBox.Show("Please set your settings\nthis menu will now close","Error");
                steamUser.LogOff();
                steamClient.Disconnect();
                this.Close();
            }
        }
        void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine("[Steam] Connected to Steam! Logging in '{0}'..." + "\n", usernames[AccountIndex]);

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = usernames[AccountIndex],
                Password = passwords[AccountIndex],
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("[Steam] Disconnected from Steam" + "\n");

            isRunning = false;
        }

        void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            var id = AccountIndex;
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    // if we recieve AccountLogonDenied or one of it's flavors (AccountLogonDeniedNoMailSent, etc)
                    // then the account we're logging into is SteamGuard protected
                    // see sample 5 for how SteamGuard can be handled

                    Console.WriteLine("[Steam] Unable to logon to Steam: This account is SteamGuard protected." + "\n");

                    isRunning = false;
                    return;
                }

                Console.WriteLine("[Steam] Unable to logon to Steam: {0} / {1}" + "\n", callback.Result, callback.ExtendedResult);

                isRunning = false;
                return;
            }

            Console.WriteLine("[Steam] Successfully logged on!" + "\n");

            var data = new dataJson();
            var ID = steamUser.SteamID;

            // at this point, we'd be able to perform actions on Steam
            if (File.Exists(appDataPath + @"\data.json"))
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteamAccountGenerator";
                var creds = File.ReadAllText(appDataPath + @"\data.json").Split(Convert.ToChar(","));
                data = JsonConvert.DeserializeObject<dataJson>(EncryptProvider.AESDecrypt(creds[2], creds[0], creds[1]));
            }
            else
            {
                MessageBox.Show("Please set your settings\nthis menu will now close", "Error");
                steamUser.LogOff();
                steamClient.Disconnect();
                this.Close();
            }
            Console.WriteLine("[Steam] SteamID: " + ID.ToString() + "\n");
            Console.WriteLine("[Converted] SteamID Converted: " + ID.ConvertToUInt64() + "\n");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.steampowered.com/ISteamUser/GetPlayerBans/v1/?key=" + data.key + "&steamids=" + ID.ConvertToUInt64());
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var html = reader.ReadToEnd();
                    var responses = JObject.Parse(html);
                    Console.WriteLine("[Steam API] Results:\n   HTML: " + html + "\n   Response: " + responses.ToString() + "\n");
                    try
                    {
                        if ((string)responses["players"][0]["CommunityBanned"].ToString() != "False")
                        {
                            Console.WriteLine("[BAN] Community banned\n   Returned: " + (string)responses["players"][0]["CommunityBanned"].ToString() + "\n");
                            account_status.Text = "BANNED";
                            account_status.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (!importing)
                            {
                                if ((string)responses["players"][0]["VACBanned"].ToString() != "False")
                                {
                                    Console.WriteLine("[BAN] VAC Banned\n   Returned: " + (string)responses["players"][0]["VACBanned"].ToString() + "\n");
                                    account_status.Text = "BANNED";
                                    account_status.ForeColor = Color.Red;
                                }
                                else
                                {
                                    if ((string)responses["players"][0]["NumberOfGameBans"].ToString() != "0")
                                    {
                                        Console.WriteLine("[BAN] Game Banned\n   Returned: " + (string)responses["players"][0]["NumberOfGameBans"].ToString() + "\n");
                                        account_status.Text = "BANNED";
                                        account_status.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        account_status.Text = "Unbanned";
                                        account_status.ForeColor = Color.Green;
                                    }
                                }
                            }
                            else
                            {
                                var banned = false;
                                if ((string)responses["players"][0]["VACBanned"].ToString() != "False")
                                {
                                    Console.WriteLine("[BAN] VAC Banned\n   Returned: " + (string)responses["players"][0]["VACBanned"].ToString() + "\n");
                                    account_status.Text = "BANNED";
                                    account_status.ForeColor = Color.Red;
                                    banned = true;
                                }
                                else
                                {
                                    if ((string)responses["players"][0]["NumberOfGameBans"].ToString() != "0")
                                    {
                                        Console.WriteLine("[BAN] Game Banned\n   Returned: " + (string)responses["players"][0]["NumberOfGameBans"].ToString() + "\n");
                                        account_status.Text = "BANNED";
                                        account_status.ForeColor = Color.Red;
                                        banned = true;
                                    }
                                    else
                                    {
                                        account_status.Text = "Unbanned";
                                        account_status.ForeColor = Color.Green;
                                    }
                                }
                                if (banned)
                                {
                                    Console.WriteLine("Listed banned account:\n   " + usernames[id] + ":" + passwords[id] + "\n");
                                    bannedAccs += "\n   " + usernames[id] + ":" + passwords[id];
                                }
                                else
                                {
                                    Console.WriteLine("Listed unbanned account:\n   " + usernames[id] + ":" + passwords[id] + "\n");
                                    unbannedAccs += "\n   " + usernames[id] + ":" + passwords[id];
                                }
                            }
                        }
                    }
                    catch (Exception a)
                    {
                        var result = MessageBox.Show("Unexpected error caught\nDid you input the right API key?", "Incorrect API key");
                        if (result == DialogResult.OK)
                        {
                            parent.ShowSettings();
                            this.Close();
                        }
                    }
                    Console.WriteLine("[Form] Reenabling next and back buttons, Logging off steam..." + "\n");
                    account_back.BeginInvoke(new MethodInvoker(() =>
                    {
                        account_back.Enabled = true;
                    }));
                    account_next.BeginInvoke(new MethodInvoker(() =>
                    {
                        account_next.Enabled = true;
                    }));
                    steamUser.LogOff();
                }
            }catch(Exception a)
            {
                if(a.Message == "The remote server returned an error: (403) Forbidden.")
                {
                    Console.WriteLine(a.Message);
                    var result = MessageBox.Show("Unexpected error caught\nDid you input the right API key?", "Incorrect API key");
                    if (result == DialogResult.OK)
                    {
                        parent.ShowSettings();
                        this.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Error Caught, Please open a issue on github!\n" + a.Message);
                    MessageBox.Show(a.Message, "Error Caught");
                }
            }
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("[Steam] Logged off of Steam: {0}", callback.Result + "\n");
        }

        private void import_click(object sender, EventArgs e)
        {
            importing = true;
            for (int i = 0; i < usernames.Count; i++)
            {
                AccountIndex = i;
                Console.WriteLine("[Import] Loading account " + i + "\n");
                GetBanned();
            }
            var data = unbannedAccs + "\n" + bannedAccs;
            saveAccountList(data);
        }

        private void saveAccountList(string dataToSave)
        {
            //Setup keys for encryption
            var creds = EncryptProvider.CreateAesKey();
            var key = creds.Key;
            var IV = creds.IV;

            //Encrypt data
            var encrypted = EncryptProvider.AESEncrypt(dataToSave, key, IV);

            //FInishing touches for encryption
            encrypted = key + "," + IV + "," + encrypted;

            //Write data to file
            File.WriteAllText(appDataPath + @"\accounts.json", encrypted);

            //Reload Main Form's Accounts Menu
            if (parent != null)
                parent.ReloadAccounts();
            this.Close();
        }
    }
}
