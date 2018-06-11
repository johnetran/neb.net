namespace Nebulas.Example
{
    partial class frmExample
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
            this.btnNewAccount = new System.Windows.Forms.Button();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.lblPassphrase = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtKeyString = new System.Windows.Forms.TextBox();
            this.lblAddressStatus = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblKeystore = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.GetBalance = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNewAccount
            // 
            this.btnNewAccount.Location = new System.Drawing.Point(357, 12);
            this.btnNewAccount.Name = "btnNewAccount";
            this.btnNewAccount.Size = new System.Drawing.Size(118, 23);
            this.btnNewAccount.TabIndex = 0;
            this.btnNewAccount.Text = "New Account";
            this.btnNewAccount.UseVisualStyleBackColor = true;
            this.btnNewAccount.Click += new System.EventHandler(this.btnNewAccount_Click);
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(81, 14);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.PasswordChar = '*';
            this.txtPassphrase.Size = new System.Drawing.Size(270, 20);
            this.txtPassphrase.TabIndex = 1;
            this.txtPassphrase.Text = "testtest1";
            // 
            // lblPassphrase
            // 
            this.lblPassphrase.AutoSize = true;
            this.lblPassphrase.Location = new System.Drawing.Point(12, 17);
            this.lblPassphrase.Name = "lblPassphrase";
            this.lblPassphrase.Size = new System.Drawing.Size(62, 13);
            this.lblPassphrase.TabIndex = 2;
            this.lblPassphrase.Text = "Passphrase";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(80, 41);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(270, 20);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.Text = "n1Km3M6wkUgKSD7HFrDMggPYsjY5FPxrXHi";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(29, 44);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "Address";
            // 
            // txtKeyString
            // 
            this.txtKeyString.Location = new System.Drawing.Point(495, 60);
            this.txtKeyString.Multiline = true;
            this.txtKeyString.Name = "txtKeyString";
            this.txtKeyString.ReadOnly = true;
            this.txtKeyString.Size = new System.Drawing.Size(437, 79);
            this.txtKeyString.TabIndex = 6;
            // 
            // lblAddressStatus
            // 
            this.lblAddressStatus.AutoSize = true;
            this.lblAddressStatus.Location = new System.Drawing.Point(492, 17);
            this.lblAddressStatus.Name = "lblAddressStatus";
            this.lblAddressStatus.Size = new System.Drawing.Size(35, 13);
            this.lblAddressStatus.TabIndex = 7;
            this.lblAddressStatus.Text = "label1";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(28, 73);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(46, 13);
            this.lblBalance.TabIndex = 8;
            this.lblBalance.Text = "Balance";
            // 
            // txtBalance
            // 
            this.txtBalance.Location = new System.Drawing.Point(81, 70);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(171, 20);
            this.txtBalance.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(495, 172);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(440, 20);
            this.textBox1.TabIndex = 10;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(495, 145);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(440, 20);
            this.textBox2.TabIndex = 11;
            // 
            // lblKeystore
            // 
            this.lblKeystore.AutoSize = true;
            this.lblKeystore.Location = new System.Drawing.Point(492, 44);
            this.lblKeystore.Name = "lblKeystore";
            this.lblKeystore.Size = new System.Drawing.Size(48, 13);
            this.lblKeystore.TabIndex = 12;
            this.lblKeystore.Text = "Keystore";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(495, 198);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(437, 275);
            this.txtResult.TabIndex = 15;
            // 
            // GetBalance
            // 
            this.GetBalance.Location = new System.Drawing.Point(357, 39);
            this.GetBalance.Name = "GetBalance";
            this.GetBalance.Size = new System.Drawing.Size(118, 23);
            this.GetBalance.TabIndex = 16;
            this.GetBalance.Text = "Get Balance";
            this.GetBalance.UseVisualStyleBackColor = true;
            this.GetBalance.Click += new System.EventHandler(this.GetBalance_Click);
            // 
            // frmExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 535);
            this.Controls.Add(this.GetBalance);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblKeystore);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblAddressStatus);
            this.Controls.Add(this.txtKeyString);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblPassphrase);
            this.Controls.Add(this.txtPassphrase);
            this.Controls.Add(this.btnNewAccount);
            this.Name = "frmExample";
            this.Text = "Example Nebulas .NET SDK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewAccount;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label lblPassphrase;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtKeyString;
        private System.Windows.Forms.Label lblAddressStatus;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.TextBox txtBalance;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblKeystore;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button GetBalance;
    }
}

