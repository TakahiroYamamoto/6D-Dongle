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
using Sentinel.Ldk.LicGen;

using DongleToolLib;

namespace DongleManagerLib
{
    public partial class DongleListCtl : UserControl
    {
        public DongleListCtl()
        {
            InitializeComponent();
            AdujustColumnWidth();
        }
        void AdujustColumnWidth()
        {
            for (int i = 0; i < listDongle.Columns.Count; i++)
                listDongle.Columns[i].Width = -1;
        }
        public void SetDongleData(LicenseMiniManager licMgr, List<DongleListItem> dongleItems)
        {
            listDongle.Items.Clear();
            if (dongleItems == null)
                return;

            foreach (DongleListItem dongleItem in dongleItems)
            {
                string productStr = "";
                for (int i = 0; i < dongleItem.productIds.Count; i++)
                {
                    if (productStr != "")
                        productStr += ",";
                    int pId = dongleItem.productIds[i];
                    ProductItem pItem = licMgr.GetProductItem(pId);
                    if( pItem != null )
                        productStr += string.Format("{0}({1})", pItem.name, pId);
                    else
                        productStr += string.Format("{0}({1})", "", pId);
                }
                string featureStr = "";
                for (int i = 0; i < dongleItem.featureIds.Count; i++)
                {
                    if (featureStr != "")
                        featureStr += ",";
                    FeatureIdAndPeriodInDognle dongle_fItem = dongleItem.featureIds[i];
                    int fId = dongle_fItem.id;
                    FeatureItem fItem = licMgr.GetFeatureItem(fId);
                    if (fItem != null)
                        featureStr += string.Format("{0}({1})", fItem.name, fId);
                    else if (fId == 0)
                        featureStr += string.Format("{0}({1})", "", fId);
                    else
                        featureStr += string.Format("{0}({1})", "", fId);

                    if (fItem != null)
                    {
                        if (dongle_fItem.period_kind == Enum_Period.Date)
                            featureStr += $"[{dongle_fItem.period_date.ToString("yyyy-MM-dd")}]";
                        else if (fItem.period_kind == Enum_Period.Days)
                        {
                            if( dongle_fItem.period_days_end != DateTime.MinValue) // アクセス済
                                featureStr += $"[{dongle_fItem.period_days_end.ToString("yyyy-MM-dd")}(days={dongle_fItem.period_days})]";
                            else
                                featureStr += $"[{dongle_fItem.period_days}days]";
                        }
                    }
                }
                string[] item = { dongleItem.dongleId, productStr, featureStr };
                listDongle.Items.Add(new ListViewItemDongle(item, dongleItem));
            }
            AdujustColumnWidth();
        }

#if false
        public void SetDongleFeatures(LicenseManager licMgr, int index, List<int> fIds)
        {
            ListViewItem item = listDongle.Items[index];
            string str = "";
            for (int i = 0; i < fIds.Count; i++)
            {
                int id = fIds[i];
                if (str != "")
                    str += ",";
                str += string.Format("{0}", fIds[i]);
            }
            item.SubItems[1].Text = str;
            AdujustColumnWidth();
        }
#endif

        public DongleListItem GetSelectedDongleItem()
        {
            if (listDongle.SelectedItems.Count == 0)
                return null;
            ListViewItemDongle item = (ListViewItemDongle)listDongle.SelectedItems[0];
            return item.dongleItem;
        }

        public List<DongleListItem> GetDongleListItem(bool isSelectedOnly)
        {
            List<DongleListItem> items = new List<DongleListItem>();
            if (isSelectedOnly)
            {
                foreach (ListViewItem i in listDongle.SelectedItems)
                {
                    ListViewItemDongle item = (ListViewItemDongle)i;
                    items.Add(item.dongleItem);
                }
            }
            else
            {
                foreach (ListViewItem i in listDongle.Items)
                {
                    ListViewItemDongle item = (ListViewItemDongle)i;
                    items.Add(item.dongleItem);
                }
            }
            return items;
        }

#if false
        private void btnLookIntoContents_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string licInfo;
                string errmsg;
                DongleListItem dongleItem = GetSelectedDongleItem();
                if (dongleItem == null)
                {
                    MessageBox.Show("Item not selected");
                    return;
                }
                if (dongleItem.c2vData != null)
                    licInfo = LicenseManager.GetC2VInfo(dongleItem.dongleId, dongleItem.c2vData, out errmsg);
                else
                    licInfo = LicenseManager.GetDongleInfo(dongleItem.dongleId, out errmsg);
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
#endif
    }

    public class ListViewItemDongle : ListViewItem
    {
        public DongleListItem dongleItem;
        public ListViewItemDongle(string [] item, DongleListItem dongleItem): base(item)
        {
            this.dongleItem = dongleItem;
        }
    }
}
