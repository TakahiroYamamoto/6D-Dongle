using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongleManagerLib
{
    public partial class LicenseListCtl : UserControl
    {
        LicenseMiniManager licMgr;

        public LicenseListCtl()
        {
            InitializeComponent();
            AdujustColumnWidth();
        }

        void AdujustColumnWidth()
        {
            for (int i = 0; i < listLicense.Columns.Count; i++)
                listLicense.Columns[i].Width = -1;
        }

        public void Clear()
        {
            listLicense.Items.Clear();
        }

        public void DeleteSelected()
        {
            foreach (ListViewItem item in listLicense.SelectedItems)
            {
                listLicense.Items.Remove(item);
            }
        }

        public List<int> GetProductIdxs()
        {
            List<int> productIdxs = new List<int>();
            foreach (ListViewItem item in listLicense.Items)
            {
                ListViewItemWithIdx itemIdx = (ListViewItemWithIdx)item;
                productIdxs.Add(itemIdx.index);
            }
            return productIdxs;
        }

        public List<int> GetSelectedProductIdxs()
        {
            List<int> idxs = new List<int>();

            foreach(ListViewItem item in listLicense.SelectedItems)
            {
                ListViewItemWithIdx itemIdx = (ListViewItemWithIdx)item;
                idxs.Add(itemIdx.index);
            }
            return idxs;
        }

        public void SetAllProducts(LicenseMiniManager licMgr)
        {
            List<int> productIdxs = new List<int>();
            for ( int i = 0; i < licMgr.products.Count; i++ )
                productIdxs.Add(i);
            SetProductIdxs(licMgr, productIdxs);
        }

        public void SetProductIdxs(LicenseMiniManager licMgr,List<int>productIdxs)
        {
            this.licMgr = licMgr;
            listLicense.Items.Clear();
            for ( int i = 0; i < productIdxs.Count; i++ )
            {
                int idx = productIdxs[i];
                ProductItem pItem = licMgr.products[idx];
                string[] items = { string.Format( "{0}({1})",pItem.name, pItem.id), pItem.featuresStr };
                ListViewItemWithIdx item = new ListViewItemWithIdx(items,idx);
                listLicense.Items.Add(item);
            }
            AdujustColumnWidth();
        }
    }

    public class ListViewItemWithIdx : ListViewItem
    {
        public int index;
        public ListViewItemWithIdx(string[] item, int index) : base(item)
        {
            this.index = index;
        }
    }
}
