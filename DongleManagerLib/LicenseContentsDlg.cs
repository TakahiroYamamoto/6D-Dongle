using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleManagerLib
{
    public partial class LicenseContentsDlg : Form
    {
        public LicenseContentsDlg()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            textContents.Text = text;
        }
    }
}
