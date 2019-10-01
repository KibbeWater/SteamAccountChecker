namespace SteamAccGen.Forms
{
    partial class ImportedAccounts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.account_username = new System.Windows.Forms.TextBox();
            this.account_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.account_status = new System.Windows.Forms.Label();
            this.account_back = new System.Windows.Forms.Button();
            this.account_next = new System.Windows.Forms.Button();
            this.import = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // account_username
            // 
            this.account_username.Location = new System.Drawing.Point(76, 6);
            this.account_username.Name = "account_username";
            this.account_username.ReadOnly = true;
            this.account_username.Size = new System.Drawing.Size(218, 20);
            this.account_username.TabIndex = 2;
            // 
            // account_password
            // 
            this.account_password.Location = new System.Drawing.Point(76, 32);
            this.account_password.Name = "account_password";
            this.account_password.ReadOnly = true;
            this.account_password.Size = new System.Drawing.Size(218, 20);
            this.account_password.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Account Status:";
            // 
            // account_status
            // 
            this.account_status.AutoSize = true;
            this.account_status.BackColor = System.Drawing.SystemColors.Control;
            this.account_status.ForeColor = System.Drawing.Color.Blue;
            this.account_status.Location = new System.Drawing.Point(151, 55);
            this.account_status.Name = "account_status";
            this.account_status.Size = new System.Drawing.Size(54, 13);
            this.account_status.TabIndex = 5;
            this.account_status.Text = "Loading...";
            // 
            // account_back
            // 
            this.account_back.Location = new System.Drawing.Point(12, 74);
            this.account_back.Name = "account_back";
            this.account_back.Size = new System.Drawing.Size(144, 23);
            this.account_back.TabIndex = 6;
            this.account_back.Text = "Previous";
            this.account_back.UseVisualStyleBackColor = true;
            this.account_back.Click += new System.EventHandler(this.Account_back_Click);
            // 
            // account_next
            // 
            this.account_next.Location = new System.Drawing.Point(162, 74);
            this.account_next.Name = "account_next";
            this.account_next.Size = new System.Drawing.Size(132, 23);
            this.account_next.TabIndex = 7;
            this.account_next.Text = "Next";
            this.account_next.UseVisualStyleBackColor = true;
            this.account_next.Click += new System.EventHandler(this.Account_next_Click);
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(12, 103);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(282, 23);
            this.import.TabIndex = 8;
            this.import.Text = "Import";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_click);
            // 
            // ImportedAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 138);
            this.Controls.Add(this.import);
            this.Controls.Add(this.account_next);
            this.Controls.Add(this.account_back);
            this.Controls.Add(this.account_status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.account_password);
            this.Controls.Add(this.account_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ImportedAccounts";
            this.Text = "ImportedAccounts";
            this.Load += new System.EventHandler(this.ImportedAccounts_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox account_username;
        private System.Windows.Forms.TextBox account_password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label account_status;
        private System.Windows.Forms.Button account_back;
        private System.Windows.Forms.Button account_next;
        private System.Windows.Forms.Button import;
    }
}