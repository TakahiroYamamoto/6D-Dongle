using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DongleManagerLib;

namespace DongleManager
{
    public partial class ProcessBurnDlg : Form
    {
        LicenseManager licMgr;
        List<DongleListItem> dongleItems;
        List<int> productIdxs;
        string folderV2C;

        EventWaitHandle evThreadStopping = new EventWaitHandle(false, EventResetMode.AutoReset); // スレッド停止要求
        EventWaitHandle evThreadStopped = new EventWaitHandle(true, EventResetMode.ManualReset); // スレッド停状態

        delegate void Delegate_EventFromThread(int count, string msg);

        // ---- Thread Param---
        //string processStr;
        bool canceled = false;
        int finalCount;

        // For burning directry or creating V2C
        public ProcessBurnDlg(LicenseManager licMgr, List<DongleListItem> dongleItems, List<int> productIdxs, string folderV2C)
        {
            InitializeComponent();

            this.licMgr = licMgr;
            this.dongleItems = dongleItems;
            this.productIdxs = productIdxs;
            this.folderV2C = folderV2C;
            btnCancel.Text = "Cancel";
            canceled = false;
            finalCount = 0;

            foreach (DongleListItem dongleItem in dongleItems)
            {
                string[] itemStr = { dongleItem.dongleId, "" };
                ListViewItemDongle item = new ListViewItemDongle(itemStr, dongleItem);
                listDongle.Items.Add(item);
                //item.Checked = false;
            }
        }

        // For burning V2C to dongle
        public ProcessBurnDlg(LicenseManager licMgr, List<DongleListItem> dongleItems)
        {
            InitializeComponent();

            this.licMgr = licMgr;
            this.dongleItems = dongleItems;
            this.productIdxs = null;
            this.folderV2C = null;
            btnCancel.Text = "Cancel";
            canceled = false;
            finalCount = 0;

            foreach (DongleListItem dongleItem in dongleItems)
            {
                string[] itemStr = { dongleItem.v2c_filename, "" };
                ListViewItemDongle item = new ListViewItemDongle(itemStr, dongleItem);
                listDongle.Items.Add(item);
                //item.Checked = false;
            }
        }

        public List<DongleListItem> GetBurnedDoubleItems()
        {
            List<DongleListItem> items = new List<DongleListItem>();
            for( int i = 0; i < finalCount; i++)
            {
                items.Add(dongleItems[i]);
            }
            return items;
        }

        void EventFromThread(int count, string errmsg)
        {
            if( count == 0 )
            {
                btnCancel.Text = "Close";
                if (errmsg != "")
                    MessageBox.Show(errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (canceled && finalCount != dongleItems.Count)
                    MessageBox.Show("Canceled");
                else if (finalCount != dongleItems.Count)
                    MessageBox.Show("Some errors occur");
                else
                    MessageBox.Show("All completed.");
            }
            else
            {
                if(count < 0)
                {
                    count = -count;

                    ListViewItemDongle item = (ListViewItemDongle)listDongle.Items[count - 1];
                    item.SubItems[1].Text = "No need to update because of same contents";
                    item.dongleItem.updated = false;
                }
                else
                {
                    ListViewItemDongle item = (ListViewItemDongle)listDongle.Items[count - 1];
                    item.SubItems[1].Text = "Complete";
                    item.dongleItem.updated = true;
                }
                finalCount = count;
                //listDongle.Items[count - 1].Checked = true;
            }
        }

        async void StartThread()
        {
            finalCount = 0;
            //processStr = "";
            evThreadStopping.Reset();
            evThreadStopped.Reset();
            if( productIdxs == null ) // V2Cの書き込み
            {
                await Task.Run(() =>
                {
                    ThreadWorkBurnV2C();
                }
                );
            }
            else if (folderV2C == null) //　直接書き込み
            {
                await Task.Run(() =>
                {
                    ThreadWorkDirect();
                }
                );
            }
            else
            {   // V2C作成
                await Task.Run(() =>
                {
                    ThreadWorkCreateV2C();
                }
                );
            }
        }

        void ThreadWorkDirect()
        {
            int count = 0;
            string errmsg = "";
            foreach (DongleListItem dongleItem in dongleItems)
            {
                if (evThreadStopping.WaitOne(0))
                    break;
                try
                {
                    string c2vData = LicenseManager.GetDongleC2V(dongleItem.dongleId, out errmsg);
                    if (c2vData == null)
                        throw new Exception("");
                    string v2cData = licMgr.CreateV2C(dongleItem.dongleId, c2vData, productIdxs, out errmsg);
                    if (v2cData == null)
                        throw new Exception("");
                    ++count;
                    if (v2cData != "")
                    {
                        if (LicenseManager.BurnToDongle(dongleItem.dongleId, v2cData, out errmsg) < 0)
                            throw new Exception("");
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), count, "");
                    }
                    else // 内容が同じでアップデート不要
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), -count, "");
                }
                catch (Exception e)
                {
                    if (e.Message != "")
                        errmsg = e.Message;
                    break;
                }
            }
            evThreadStopped.Set();
            Invoke(new Delegate_EventFromThread(this.EventFromThread), 0, errmsg);
        }

        void ThreadWorkCreateV2C()
        {
            string errmsg = "";
            int count = 0;
            foreach (DongleListItem dongleItem in dongleItems)
            {
                if (evThreadStopping.WaitOne(0))
                    break;
                try
                {
                    string c2vData = dongleItem.c2vData;
                    string v2cData = licMgr.CreateV2C(dongleItem.dongleId, c2vData, productIdxs, out errmsg);
                    if (v2cData == null)
                        throw new Exception();
                    ++count;
                    if (v2cData != "")
                    {
                        string file = string.Format(@"{0}\{1}.v2c", folderV2C, dongleItem.c2v_basename);
                        try
                        {
                            System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, new System.Text.UTF8Encoding(false));
                            try
                            {
                                sw.Write(v2cData);
                            }
                            finally
                            {
                                sw.Close();
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(string.Format("Cannot save ({0})", file));
                            return;
                        }
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), count, "");
                    }
                    else // 内容が同じでアップデート不要
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), -count, "");
                }
                catch (Exception)
                {
                    break;
                }
            }
            evThreadStopped.Set();
            Invoke(new Delegate_EventFromThread(this.EventFromThread), 0, errmsg);
        }


        void ThreadWorkBurnV2C()
        {
            string errmsg = "";
            int count = 0;
            foreach (DongleListItem dongleItem in dongleItems)
            {
                if (evThreadStopping.WaitOne(0))
                    break;
                try
                {
                    string v2cData = dongleItem.v2cData;
                    if (v2cData == null)
                        throw new Exception();
                    ++count;
                    if (v2cData != "")
                    {
                        if (LicenseManager.BurnToDongle(dongleItem.v2c_filename, v2cData, out errmsg) < 0)
                            throw new Exception();
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), count, "");
                    }
                    else // 内容が同じでアップデート不要。(空のV2C生成はできないはずのでありえないはず)
                        Invoke(new Delegate_EventFromThread(this.EventFromThread), -count, "");
                }
                catch (Exception)
                {
                    break;
                }
            }
            evThreadStopped.Set();
            Invoke(new Delegate_EventFromThread(this.EventFromThread), 0, errmsg);
        }

        private void ProcessBurnDlg_Shown(object sender, EventArgs e)
        {
            StartThread();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Close")
                this.DialogResult = DialogResult.OK;

            evThreadStopping.Set();
            canceled = true;
        }
    }
}