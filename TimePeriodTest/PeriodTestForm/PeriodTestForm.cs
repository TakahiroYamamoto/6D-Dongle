using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DongleToolLib;

namespace PeriodTestForm
{
    public partial class PeriodTestForm : Form
    {
        LicenseMiniManager licMgr = new LicenseMiniManager();

        public PeriodTestForm()
        {
            InitializeComponent();
            enumDongleCtl.Init(licMgr, null);
        }

        public string GetFeatureInfo(int feature_id)
        {
            LibSentinelCLI.ExpirationInfoCLI expInfo;
            string errmsg;
            string msg;

            bool status;
            if (check_SelectedOnly.Checked)
            {
                List<DongleToolLib.DongleListItem> dongleList = enumDongleCtl.GetDongleListItem(true);
                List<string> dongles = new List<string>();
                foreach(DongleToolLib.DongleListItem item in dongleList)
                    dongles.Add(item.dongleId);
                status = LibSentinelCLI.SentinelCLI.HaspCheck_Scope(feature_id, dongles, out expInfo, out errmsg);
            }
            else
                status = LibSentinelCLI.SentinelCLI.HaspCheck(feature_id, out expInfo, out errmsg);

            if (expInfo.kind != 0)
                msg = $"{feature_id}={status}({expInfo.date:yyyy-MM-dd}:{expInfo.remaining_days}days remainning)[{errmsg}]";
            else
                msg = $"{feature_id}={status}[{errmsg}]";
            return msg;
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            string msg100 = GetFeatureInfo(100);
            string msg101 = GetFeatureInfo(101);
            string msg102 = GetFeatureInfo(102);

            MessageBox.Show(
                $"{msg100}\r\n{msg101}\r\n{msg102}"
                ); ;
        }
    }
}
