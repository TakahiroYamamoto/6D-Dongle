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

namespace DongleManagerLib
{
    public delegate void Dele_Void();

    public partial class FeatureWithPeriodCtl : UserControl
    {
        LicenseMiniManager licMgr;
        List<int> productIdxs = new List<int>();

        public Dele_Void dele_Period_Changed;

        public FeatureWithPeriodCtl()
        {
            InitializeComponent();
            AdujustColumnWidth();
            Update_PeriodCtl();
        }

        void AdujustColumnWidth()
        {
            for (int i = 0; i < listFeatures.Columns.Count; i++)
                listFeatures.Columns[i].Width = -1;
        }

        public void SetFeaturesWithPeriod(LicenseMiniManager licMgr, List<int> productIdxs)
        {
            this.licMgr = licMgr;
            this.productIdxs = productIdxs;
            listFeatures.Items.Clear(); // トップアイテムを忘れさせるために
            UpdateListFeatures();
        }

        void UpdateListFeatures()
        {
            int topIndex = -1;
            int selectedIndex = -1;
            if (listFeatures.TopItem != null)
            {
                topIndex = listFeatures.TopItem.Index;
                selectedIndex = listFeatures.SelectedIndices.Count > 0 ? listFeatures.SelectedIndices[0] : -1;
            }

            listFeatures.Items.Clear();
            List<int> featureIdxs = new List<int>();
            foreach(int p_idx in productIdxs)
            {
                ProductItem pItem = licMgr.products[p_idx];
                for( int i_f = 0; i_f < pItem.featureIdxs.Count; i_f++)
                {
                    int f_idx = pItem.featureIdxs[i_f];
                    if (featureIdxs.IndexOf(f_idx) >= 0)
                        continue;
                    FeatureItem fItem = licMgr.features[f_idx];
                    if (!fItem.valid_for_dongle)
                        continue;
                    if (fItem.period_kind == Enum_Period.None)
                        continue;
                    featureIdxs.Add(f_idx);
                }
            }
            foreach (int f_idx in featureIdxs)
            {
                FeatureItem fItem = licMgr.features[f_idx];

                string kindStr;
                string periodStr;
                if (fItem.period_kind == Enum_Period.Date)
                {
                    kindStr = "Date";
                    if (fItem.period_date == DateTime.MinValue)
                        periodStr = "NO DATE";
                    else
                        periodStr = fItem.period_date.ToString("yyyy-MM-dd");
                }
                else
                {
                    kindStr = "Days";
                    periodStr = $"{fItem.period_days}";
                }
                string[] items = { $"{fItem.name}({fItem.id})",kindStr,periodStr };
                ListViewItemWithIdx item = new ListViewItemWithIdx(items, f_idx);
                listFeatures.Items.Add(item);
            }
            AdujustColumnWidth();

            if( selectedIndex >= 0 )
            {
                listFeatures.SelectedIndices.Add(selectedIndex);
            }
            if (topIndex >= 0)
            {
                listFeatures.TopItem = listFeatures.Items[topIndex];
            }
        }
        void Update_PeriodCtl()
        {
            if (listFeatures.SelectedIndices.Count == 0)
            {
                panel_PeriodDays.Enabled = false;
                dateTime_PeriodDate.Enabled = false;
                btn_SetPeriod.Enabled = false;
            }
            else
            {
                btn_SetPeriod.Enabled = true;
                ListViewItemWithIdx item = listFeatures.SelectedItems[0] as ListViewItemWithIdx;
                FeatureItem fItem = licMgr.features[item.index];
                if (fItem.period_kind == Enum_Period.Date)
                {
                    panel_PeriodDays.Enabled = false;
                    dateTime_PeriodDate.Enabled = true;
                    DateTime dt;
                    if (fItem.period_date == DateTime.MinValue)
                    {
                        dt = DateTime.Today;
                        dt = dt.AddMonths(13);
                    }
                    else
                        dt = fItem.period_date;
                    dateTime_PeriodDate.Value = dt;
                }
                else
                {
                    dateTime_PeriodDate.Enabled = false;
                    panel_PeriodDays.Enabled = true;
                    num_PeriodDays.Value = fItem.period_days;
                }
            }
        }

        private void listFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
                Update_PeriodCtl();
        }

        private void btn_SetPeriod_Click(object sender, EventArgs e)
        {
            ListViewItemWithIdx item = listFeatures.SelectedItems[0] as ListViewItemWithIdx;
            FeatureItem fItem = licMgr.features[item.index];

            if( fItem.period_kind == Enum_Period.Date )
            {
                if (dateTime_PeriodDate.Value < DateTime.Today)
                {
                    MessageBox.Show("過去の日付は指定できません");
                    return;
                }
                fItem.period_date = dateTime_PeriodDate.Value;
            }
            else
            {
                fItem.period_days = (int)num_PeriodDays.Value;
            }
            UpdateListFeatures();
            if (dele_Period_Changed != null)
                dele_Period_Changed();
        }
    }
}
