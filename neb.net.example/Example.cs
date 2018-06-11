using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nebulas.Example
{
    public partial class frmExample : Form
    {
        public frmExample()
        {
            InitializeComponent();
            NewAccount();
        }

        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            NewAccount();
        }

        private void NewAccount()
        {
            var newAccount = Account.NewAccount();
            if (newAccount != null)
            {
                txtAddress.Text = newAccount.GetAddressString();
                var keyStr = newAccount.ToKeyString(txtPassphrase.Text);
                txtKeyString.Text = keyStr;

                lblAddressStatus.Text = Account.isValidAddress(txtAddress.Text, Account.NORMALTYPE).ToString();

                textBox1.Text = "[";
                textBox2.Text = "{";

                var array = "";
                var del = "";
                foreach (byte b in newAccount.PrivKey)
                {
                    array += del + b.ToString();
                    del = ",";
                }

                textBox1.Text += array + "]";
                textBox2.Text += array + "}";


                /*
                var neb = new Neb("https://testnet.nebulas.io");
                neb.Api.GetAccountState(new GetAccountStateOptions
                {
                    address = txtAddress.Text
                }).ContinueWith((task) =>
                {
                    SetBalance(task.Result);
                });
                */
            }
        }

        delegate void StringArgReturningVoidDelegate(string text);
        delegate void TexBoxStringArgReturningVoidDelegate(TextBox textbox, string text);

        private void SetBalance(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.txtBalance.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetBalance);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBalance.Text = text;
            }
        }
        private void SetText(TextBox textbox, string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (textbox.InvokeRequired)
            {
                TexBoxStringArgReturningVoidDelegate d = new TexBoxStringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { textbox, text });
            }
            else
            {
                textbox.Text = text;
            }
        }

        private void GetBalance_Click(object sender, EventArgs e)
        {

            var neb = new Neb("https://testnet.nebulas.io");
            neb.Api.GetAccountState(new GetAccountStateOptions
            {
                address = txtAddress.Text
            }).ContinueWith((task) => { SetText(txtResult, task.Result); });

        }
    }
}
