using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleManager
{
    public partial class LicenseEditor : Form
    {
        DongleManagerLib.LicenseManager licMgr;

        public LicenseEditor()
        {
            InitializeComponent();
            featureWithPeriodCtl.dele_Period_Changed += ev_PeriodUpdated;
        }

        public void ev_PeriodUpdated()
        {
            licenseListCtlForBurn.UpdateList(true);
            licenseListCtlAll.UpdateList(true);
        }

        public void SetData(DongleManagerLib.LicenseManager licMgr, List<int>productIdxs)
        {
            this.licMgr = licMgr;
            licenseListCtlAll.SetAllProducts(licMgr);
            licenseListCtlForBurn.SetProductIdxs(licMgr,productIdxs);
            featureWithPeriodCtl.SetFeaturesWithPeriod(licMgr, productIdxs);
        }

        public List<int> GetProductIdxs()
        {
            return licenseListCtlForBurn.GetProductIdxs();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<int> currProductIdxs = licenseListCtlForBurn.GetProductIdxs();
            List<int> addProductIdxs = licenseListCtlAll.GetSelectedProductIdxs();
            string errmsg;
            List<int> margeProductIdxs = licMgr.MargeProductIdxs(currProductIdxs, addProductIdxs, out errmsg);
            if( margeProductIdxs == null )
            {
                MessageBox.Show(errmsg);
                return;
            }
            licenseListCtlForBurn.SetProductIdxs(licMgr,margeProductIdxs);
            featureWithPeriodCtl.SetFeaturesWithPeriod(licMgr, margeProductIdxs);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            licenseListCtlForBurn.DeleteSelected();
            List<int> currProductIdxs = licenseListCtlForBurn.GetProductIdxs();
            featureWithPeriodCtl.SetFeaturesWithPeriod(licMgr, currProductIdxs);
        }
    }
}
