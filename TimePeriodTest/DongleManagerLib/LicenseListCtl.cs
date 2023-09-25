using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DongleToolLib;

namespace DongleManagerLib
{
    public partial class LicenseListCtl : UserControl
    {
        LicenseMiniManager licMgr;
        List<int> productIdxs = new List<int>();

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
            productIdxs = new List<int>();
            foreach (ListViewItem item in listLicense.Items)
            {
                ListViewItemWithIdx itemIdx = (ListViewItemWithIdx)item;
                productIdxs.Add(itemIdx.index);
            }
        }

        public List<int> GetProductIdxs()
        {
#if true
            return productIdxs;
#else
            List<int> productIdxs = new List<int>();
            foreach (ListViewItem item in listLicense.Items)
            {
                ListViewItemWithIdx itemIdx = (ListViewItemWithIdx)item;
                productIdxs.Add(itemIdx.index);
            }
            return productIdxs;
#endif
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
#if true
            this.licMgr = licMgr;
            productIdxs = new List<int>();
            for (int i = 0; i < licMgr.products.Count; i++)
                productIdxs.Add(i);
            UpdateList(false);
#else
            List<int> productIdxs = new List<int>();
            for ( int i = 0; i < licMgr.products.Count; i++ )
                productIdxs.Add(i);
            SetProductIdxs(licMgr, productIdxs);
#endif
        }

        public void SetProductIdxs(LicenseMiniManager licMgr,List<int>in_productIdxs)
        {
            this.licMgr = licMgr;
#if true
            productIdxs = new List<int>();
            foreach( int idx in in_productIdxs)
                productIdxs.Add(idx);
            UpdateList(false);
#else
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
#endif
        }

        public void UpdateList(bool isRetainTop)
        {
            int topIndex = -1;
            if( isRetainTop )
            {
                if (listLicense.TopItem != null)
                    topIndex = listLicense.TopItem.Index;
            }
            listLicense.Items.Clear();
            for (int i = 0; i < productIdxs.Count; i++)
            {
                int idx = productIdxs[i];
                ProductItem pItem = licMgr.products[idx];
                string featuresStr = "";
                {
                    foreach(int fidx in pItem.featureIdxs)
                    {
                        FeatureItem fItem = licMgr.features[fidx];
                        if (!fItem.valid_for_dongle)
                            continue;
                        if (featuresStr != "")
                            featuresStr += ",";
                        featuresStr += $"{fItem.name}({fItem.id})";
                        if (fItem.period_kind == Enum_Period.Date)
                        {
                            if( fItem.period_date == DateTime.MinValue)
                                featuresStr += $"[NO DATE]";
                            else
                                featuresStr += $"[{fItem.period_date.ToString("yyyy-MM-dd")}]";
                        }
                        else if( fItem.period_kind == Enum_Period.Days)
                        {
                            featuresStr += $"[{fItem.period_days}days]";
                        }
                    }
                }
                string[] items = { string.Format("{0}({1})", pItem.name, pItem.id), featuresStr };
                ListViewItemWithIdx item = new ListViewItemWithIdx(items, idx);
                listLicense.Items.Add(item);
            }
            if( topIndex >= 0 )
            {
                listLicense.TopItem = listLicense.Items[topIndex];
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
