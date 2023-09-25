using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DongleManagerLib;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;

namespace DongleTool
{
    public partial class DongleToolForm : Form
    {
        HelpForm helpForm = new HelpForm();
        LicenseMiniManager licMgr = new LicenseMiniManager();

        public DongleToolForm()
        {
            InitializeComponent();

            enumDongleCtlC2V.Init(licMgr);
            enumDongleCtlC2V.EnableBlinkBtn(false);
        }

        private void btnHelpC2V_Click(object sender, EventArgs e)
        {
            helpForm.Show();
        }

        private void btnCreateC2V_Click(object sender, EventArgs e)
        {

            List<DongleListItem> dongleItems = enumDongleCtlC2V.GetDongleListItem();

            if (dongleItems.Count == 0)
            {
                MessageBox.Show("No dongle");
                return;
            }

            List<string> c2vs = new List<string>();

            Cursor = Cursors.WaitCursor;
            try
            {
                foreach (DongleListItem dongleItem in dongleItems)
                {
                    string errmsg;
                    string c2v = LicenseMiniManager.GetDongleC2V(dongleItem.dongleId, out errmsg);
                    if (c2v == null)
                    {
                        MessageBox.Show(errmsg);
                        return;
                    }
                    c2vs.Add(c2v);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.EnsurePathExists = true;
            dlg.IsFolderPicker = true;
            dlg.Multiselect = false;
            //dlg.InitialDirectory = licMgr.GetC2VPath();
            dlg.Title = "Select folder to save c2v files";
            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            string folder = dlg.FileName;
            if (folder == "")
            {
                MessageBox.Show("No item selected");
                return;
            }
            if (!System.IO.Directory.Exists(folder))
            {
                MessageBox.Show("Not folder");
                return;
            }
#if false
            {
                string errmsg;
                if (licMgr.SetC2VPath(folder, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing will be continued.");
                }
            }
#endif

            Cursor = Cursors.WaitCursor;
            try
            {
                int count = 0;
                foreach (string c2v in c2vs)
                {
                    string file = string.Format(@"{0}\{1}_{2:D4}{3:D2}{4:D2}.c2v",
                        folder, dongleItems[count].dongleId,
                        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    count++;
                    try
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, new System.Text.UTF8Encoding(false));
                        try
                        {
                            sw.Write(c2v);
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
                }
                MessageBox.Show("Save completed");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnLoadV2C_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "v2c files(*.v2c)|*.v2c|All files(*.*)|*.*";
            dlg.Title = "Open V2C files";
            //dlg.InitialDirectory = licMgr.GetV2CPath();
            dlg.RestoreDirectory = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = true;

            DialogResult res = dlg.ShowDialog();
            if (res != DialogResult.OK)
                return;

#if false
            if (dlg.FileNames.Length > 0)
            {
                string path = System.IO.Path.GetDirectoryName(dlg.FileNames[0]);
                string errmsg;

                if (licMgr.SetV2CPath(path, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing is continued.");
                }
            }
#endif

            List<DongleListItem> dongleItems = new List<DongleListItem>();

            {
                string errmsg = "";
                try
                {
                    foreach (string file in dlg.FileNames)
                    {
                        string v2cData = LicenseMiniManager.LoadFile(file, out errmsg);
                        if (v2cData == null)
                            throw new Exception();
                        DongleListItem dongleItem = new DongleListItem();
                        dongleItem.v2cData = v2cData;
                        dongleItem.v2c_filename = System.IO.Path.GetFileName(file);
                        dongleItems.Add(dongleItem);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(errmsg);
                    return;
                }
            }
            listV2C.Items.Clear();
            foreach (DongleListItem dongleItem in dongleItems)
            {
                string[] itemStr = new string[] { dongleItem.v2c_filename };
                ListViewItemDongle item = new ListViewItemDongle(itemStr, dongleItem);
                listV2C.Items.Add(item);
            }
        }

        private void btnBurnV2C_Click(object sender, EventArgs e)
        {
            List<DongleListItem> dongleItems = new List<DongleListItem>();

            foreach (ListViewItem i in listV2C.Items)
                dongleItems.Add(((ListViewItemDongle)i).dongleItem);

            if (dongleItems.Count == 0)
            {
                MessageBox.Show("No V2C to write");
                return;
            }

            DialogResult res = MessageBox.Show("Really write ?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (res != DialogResult.OK)
                return;

            ProcessBurnDlg pd = new ProcessBurnDlg(licMgr, dongleItems);

            //進行状況ダイアログを表示し、その中で書き込みを実行
            pd.ShowDialog(this);

            List<DongleListItem> burnedDongleItems = pd.GetBurnedDoubleItems();
            if (burnedDongleItems.Count > 0)
            {
                enumDongleCtlC2V.DoEnum();
#if false
                string errmsg;
                if (licMgr.SaveLog(burnedDongleItems, null, "BURN-V2C", out errmsg) < 0)
                {
                    MessageBox.Show(errmsg);
                }
#endif
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            helpForm.Show();
        }
    }
}
