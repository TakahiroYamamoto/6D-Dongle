using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DongleManagerLib;
using DongleToolLib;

namespace DongleManager
{
    public partial class LookIntoContentsCtl : UserControl
    {
        DongleListCtl dongleListCtl = null;

        public LookIntoContentsCtl()
        {
            InitializeComponent();
        }

        public void SetDongleListCtl(DongleListCtl dongleListCtl)
        {
            this.dongleListCtl = dongleListCtl; 
        }

        private void btnLookIntoContents_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;
            try
            {
                string licInfo;
                string errmsg;
                DongleListItem dongleItem = dongleListCtl.GetSelectedDongleItem();
                if (dongleItem == null)
                {
                    MessageBox.Show("Item not selected");
                    return;
                }
                if (dongleItem.c2vData != null)
                    licInfo = DongleManagerLib.LicenseManager.GetC2VInfoFromFile(dongleItem.dongleId, dongleItem.c2vData, out errmsg);
                else
                    licInfo = DongleManagerLib.LicenseManager.GetC2VInfoFromDongle(dongleItem.dongleId, out errmsg);
                if (licInfo == null)
                {
                    MessageBox.Show(errmsg);
                    return;
                }
                LicenseContentsDlg dlg = new LicenseContentsDlg();
                dlg.SetText(licInfo);
                dlg.ShowDialog();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
