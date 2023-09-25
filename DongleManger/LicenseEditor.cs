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
        LicenseManager licMgr;

        public LicenseEditor()
        {
            InitializeComponent();
        }

        public void SetData(LicenseManager licMgr, List<int>productIdxs)
        {
            this.licMgr = licMgr;
            licenseListCtlAll.SetAllProducts(licMgr);
            licenseListCtlForBurn.SetProductIdxs(licMgr,productIdxs);
        }

        public List<int> GetProductIdxs()
        {
            return licenseListCtlForBurn.GetProductIdxs();
        }

        private void button1_Click(object sender, EventArgs e)
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            licenseListCtlForBurn.DeleteSelected();
        }
    }
}
