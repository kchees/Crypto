using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoTest
{
    public partial class PasswordDlg : Form
    {
        private const int MinCharacters = 8;    // minumum number of password characters

        public string Password { get; set; }

        private string m_error;
        public String Error { get { return m_error; } }


        public PasswordDlg()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_error = string.Empty;
            if (tbPassword.TextLength < MinCharacters)
            {
                m_error = string.Format("Password must be a minimum of {0} characters", MinCharacters);
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                Password = tbPassword.Text;
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            tbPassword.UseSystemPasswordChar = !cbShowPassword.Checked;
        }
    }
}
