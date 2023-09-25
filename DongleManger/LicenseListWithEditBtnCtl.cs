using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleManager
{
    public partial class LicenseListWithEditBtnCtl : UserControl
    {
        LicenseManager licMgr = null;
        public LicenseListWithEditBtnCtl()
        {
            InitializeComponent();
        }

        public void Init(LicenseManager licMgr)
        {
            this.licMgr = licMgr;
            licenseListCtl.Clear();
        }

        public List<int> GetProductIdxs()
        {
            return licenseListCtl.GetProductIdxs();
        }

        private void btnEditLicense_Click(object sender, EventArgs e)
        {
            LicenseEditor editor = new DongleManager.LicenseEditor();
            editor.SetData(licMgr, licenseListCtl.GetProductIdxs());

            DialogResult res = editor.ShowDialog();
            if (res != DialogResult.OK)
                return;
            licenseListCtl.SetProductIdxs(licMgr, editor.GetProductIdxs());
        }
    }
}
