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
    public partial class frmDecrypted : Form
    {
        public frmDecrypted()
        {
            InitializeComponent();
        }

        public string Decrypted
        {
            set { tbDecrypted.Text = value; }
        }

    }
}
