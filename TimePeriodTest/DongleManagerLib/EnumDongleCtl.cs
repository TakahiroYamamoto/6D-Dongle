using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Aladdin.HASP;

using DongleToolLib;

namespace DongleManagerLib
{
    public partial class EnumDongleCtl : UserControl
    {
        LicenseMiniManager licenseMgr = null;
        

        public EnumDongleCtl()
        {
            InitializeComponent();
        }

        public void Init(LicenseMiniManager licenseMgr)
        {
            this.licenseMgr = licenseMgr;
        }

        public void EnableBlinkBtn(bool isBlink)
        {
            if (isBlink)
                btnBlinkDongle.Visible = true;
            else
                btnBlinkDongle.Visible = false;
        }

        public DongleListCtl GetDongleListCtl()
        {
            return dongleListCtl;
        }

        public List<DongleListItem> GetDongleListItem(bool isSelectedOnly)
        {
            return dongleListCtl.GetDongleListItem(isSelectedOnly);
        }

        public void DoEnum()
        {
            string errmsg;
            List<DongleListItem> dongleItems = LicenseMiniManager.EnumDongle(out errmsg);
            if (dongleItems == null)
            {
                MessageBox.Show(errmsg);
                return;
            }
            this.dongleListCtl.SetDongleData(licenseMgr,dongleItems);
        }

        private void btnEnumDongle_Click(object sender, EventArgs e)
        {
            DoEnum();
        }

        private void c2VListCtl1_Load(object sender, EventArgs e)
        {

        }

        private void btnBlinkDongle_Click(object sender, EventArgs e)
        {
            DongleListItem dongleItem;
            if( (dongleItem = dongleListCtl.GetSelectedDongleItem()) == null )
            {
                MessageBox.Show("Dongle not selected.");
                return;
            }
            LicenseMiniManager.StartBlinkDongle(dongleItem.dongleId);
            MessageBox.Show("Click OK to stop blinking.");
            LicenseMiniManager.StopBlinkDongle(dongleItem.dongleId);
        }

        private void btnToClipboard_Click(object sender, EventArgs e)
        {
            string str = "";
            List<DongleListItem> dongleItems = dongleListCtl.GetDongleListItem(false);
            foreach(DongleListItem item in dongleItems)
            {
                str += item.dongleId + "\r\n";
            }
            if( str == "" )
            {
                MessageBox.Show("No dongle in the dongle list");
                return;
            }
            Clipboard.SetText(str);
            MessageBox.Show(string.Format("{0} dongle(s) is set to clipboard", dongleItems.Count));
        }
    }
}
