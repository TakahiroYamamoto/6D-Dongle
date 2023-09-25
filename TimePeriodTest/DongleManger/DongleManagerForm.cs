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
using DongleToolLib;
using Aladdin.HASP;

using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;

namespace DongleManager
{
    public partial class FormMain : Form
    {
        DongleManagerLib.LicenseManager licMgr = new DongleManagerLib.LicenseManager();

        public FormMain()
        {
            InitializeComponent();

            lookIntoContentsCtl_Direct.SetDongleListCtl(enumDongleCtl_Direct.GetDongleListCtl());
            lookIntoContentsCtl_V2C.SetDongleListCtl(c2VListCtl);
            lookIntoContentsCtl_C2V.SetDongleListCtl(enumDongleCtlC2V.GetDongleListCtl());

            enumDongleCtl_Direct.Init(licMgr, DongleManagerLib.LicenseManager.GetDongleInfo);
            enumDongleCtlC2V.Init(licMgr, DongleManagerLib.LicenseManager.GetDongleInfo);

            string errmsg;
            if( licMgr.LoadInitialSetting(out errmsg) < 0 )
            {
                if( errmsg != "" )
                {
                    MessageBox.Show(errmsg);
                }
            }
            InitLicenseDefinition();
        }

        void InitLicenseDefinition()
        {
            textLicenseDefVer.Text = licMgr.version;
            licenseListWithEditBtnCtlInDirect.Init(licMgr);
            licenseListWithEditBtnCtlInV2C.Init(licMgr);
        }

        public void SetAdmin()
        {
            //btnCreateSampleLicDefFile.Visible = true;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            string scope =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                + "<haspscope>"
                + "    <hasp type=\"HASP-HL\" >"
                + "        <license_manager hostname=\"localhost\" />"
                + "    </hasp>"
                + "</haspscope>";
            string format = "<haspformat format=\"updateinfo\"/>";
            string vendor_code =
                "/k0mD4BadhLfXroVy3W1JdVvHSh+J12bxl0mCM/Zr9YV8BHNULxyaTZSC01LDGKfZ+fqfMCQ4XfFi1vA"
                + "wQuIIWfca7pkdV79ycUr2e+69SnuW7dfCr7aEKbuzVBYnlDHlbRgUVgtRs+gsqIXsGMW8DB89vG2gJxd"
                + "M17mI3+Tbu5BnN9tNv7Z6O5rU83HmSi99ULn/ZkQmLGkdlLC8pf0Q5fo5xKtA9NgNIdNY64HwPO32fcH"
                + "IMJPuPsEQr46ERTXDHoXCbiX7/CMNbQ0COwB3nIdqUyjFMJccg7JZ9NFpfAiSf0FbWskFufF8+4aAnvj"
                + "R2ZMgA6Fg7JqqVLuo5Fd+f1Jtw6/5BOp2GDpNV2D+JDsJAJiShOfeNiZnsYFvMnLfaVYPF8pmZ59BVnM"
                + "Vye6v/BsZe5atov/83bYfr4ke3GsrYas96CiZUE0ARIaLBrXPL/3upY2Gxtl/XC1lGm6qDt4CYZ9OW3i"
                + "9QuSsql1LNCzpIjdHl6v9WrlMM0n0Jyr2EUTkCmy6MU0znDWh7aj9/LlzGt0+L80FijMP/9mRtEi0SEJ"
                + "w+D2DKPR4WAZtBonXnGOQPcLFMegKZlgZpsgQyQYFfbWg18ztoUDnOtXGi7E6qkM7WKgAxi3N4yjWwil"
                + "89k43J9QsxwPPDGLX2HryP0SlTLx8Elxc+n4+VNur0Z6jQ9y0D6O1CEg+T4nvuHf0pqlcFxHDnqB1bFc"
                + "lyokxAU6023A0vf0Ri6p4F73McIdt2208A+0Op8uHxeXThjFVtt7JV7b7+Nm6qJ4pyBcgyaZqS9ukn7q"
                + "3fQSBZXYIllsoeviXCCyWSlm4RePvTguUyFrfB6uMxrQ0PDZF4kbZt1aflIxd1SxL7CZfo4r+JrTuH/m"
                + "b3B58ll4xUlqG/yjuF0IU4nYRFzLNPZjF+Nz5+JfxA1BFPTdOmByRTrQUa/+00GtEEZDm1dqIEP2Z60k"
                + "frWTkec6iLfbQw5cB7mliQ==";

            string info = "";
            HaspStatus status = Hasp.GetInfo(scope, format, vendor_code, ref info);
            if( status != HaspStatus.StatusOk )
            {
                MessageBox.Show("情報を取得できません");
                return;
            }
        }

#if false
        private void btnCreateSampleLicDefFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML file(*.xml)|*.xml|All files(*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            dlg.OverwritePrompt = true;
            dlg.CheckPathExists = true;
            DialogResult res = dlg.ShowDialog();
            if (res != DialogResult.OK)
                return;
            LicenseXMLData.SaveSampleData(dlg.FileName);
        }
#endif

        private void btnLicenseDefUpdate_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt file(*.txt)|*.txt|All files(*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            DialogResult res = dlg.ShowDialog();
            if (res != DialogResult.OK)
                return;
            string errmsg;
            if (licMgr.UpdateLicenseDef(dlg.FileName, out errmsg) < 0)
            {
                MessageBox.Show(errmsg);
                return;
            }
            InitLicenseDefinition();
        }

        private void enumDongleCtl1_Load(object sender, EventArgs e)
        {

        }

        private void btnBurn_Click(object sender, EventArgs e)
        {
            List<DongleListItem> dongleItems = enumDongleCtl_Direct.GetDongleListItem(check_SelectedOnly.Checked);
            List<int> productIdxs = licenseListWithEditBtnCtlInDirect.GetProductIdxs();

            if( dongleItems.Count == 0 )
            {
                MessageBox.Show("No dongle to write");
                return;
            }

            if (check_SelectedOnly.Checked)
            {
                DialogResult res = MessageBox.Show($"Selected dongles are overwritten!! Really OK?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res != DialogResult.OK)
                    return;
            }
            else
            {
                DialogResult res = MessageBox.Show($"All dongle connected to PC is overwritten!! Really OK?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res != DialogResult.OK)
                    return;
            }

            if ( productIdxs.Count == 0 )
            {
                DialogResult res = MessageBox.Show(
                    "No license to write.\r\n"
                    + "Do you write empty license?",
                    "Confirmation",MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if (res != DialogResult.Yes)
                    return;
            }

            ProcessBurnDlg pd = new ProcessBurnDlg(licMgr, dongleItems, productIdxs, null);

            //進行状況ダイアログを表示し、その中で書き込みを実行
            pd.ShowDialog(this);

            List<DongleListItem> burnedDongleItems = pd.GetBurnedDoubleItems();
            if (burnedDongleItems.Count > 0)
            {
                enumDongleCtl_Direct.DoEnum();
                string errmsg;
                if (licMgr.SaveLog(burnedDongleItems, productIdxs,"BURN-DIRECT", out errmsg) < 0)
                {
                    MessageBox.Show(errmsg);
                }
            }
        }

        void ChangeLogFolder()
        {
            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.EnsurePathExists = true;
            dlg.IsFolderPicker = true;
            dlg.Multiselect = false;
            dlg.InitialDirectory = licMgr.GetLogPath();
            dlg.Title = "Select folder to save log files";
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

            string errmsg;
            if( licMgr.SetLogPath(folder, out errmsg) < 0 )
            {
                MessageBox.Show( errmsg );
            }
        }

        private void btnLofFolder_Click(object sender, EventArgs e)
        {
            ChangeLogFolder();
        }

        private void btnLogFolder2_Click(object sender, EventArgs e)
        {
            ChangeLogFolder();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            string path = licMgr.GetLogPath();
            if (path != "")
                return;
            MessageBox.Show("Set folder for log.");
            ChangeLogFolder();
        }

        private void btnCreateC2V_Click(object sender, EventArgs e)
        {
            List<DongleListItem> dongleItems = enumDongleCtlC2V.GetDongleListItem(check_SelectedOnly.Checked);

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
                    string c2v = DongleManagerLib.LicenseManager.GetDongleC2V(dongleItem.dongleId, out errmsg);
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
            dlg.InitialDirectory = licMgr.GetC2VPath();
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

            {
                string errmsg;
                if (licMgr.SetC2VPath(folder, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing will be continued.");
                }
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                int count = 0;
                foreach (string c2v in c2vs)
                {
                    string file = string.Format(@"{0}\{1}_{2:D4}{3:D2}{4:D2}.c2v",
                        folder, dongleItems[count].dongleId,
                        DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
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

        private void btnLoadC2V_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "c2v files(*.c2v)|*.c2v|All files(*.*)|*.*";
            dlg.Title = "Open c2v files";
            dlg.InitialDirectory = licMgr.GetC2VPath();
            dlg.RestoreDirectory = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = true;

            DialogResult res = dlg.ShowDialog();
            if (res != DialogResult.OK)
                return;

            if (dlg.FileNames.Length > 0)
            {
                string path = System.IO.Path.GetDirectoryName(dlg.FileNames[0]);
                string errmsg;

                if (licMgr.SetC2VPath(path, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing is continued.");
                }
            }

            List<DongleListItem> dongleItems = new List<DongleListItem>();
            {
                string errmsg = "";
                try
                {
                    foreach (string file in dlg.FileNames)
                    {
                        string c2v = DongleManagerLib.LicenseManager.LoadFile(file, out errmsg);
                        if (c2v == null)
                            throw new Exception();
                        DongleListItem dongleItem = new DongleListItem();
                        dongleItem.c2vData = c2v;
                        dongleItem.dongleId = DongleManagerLib.LicenseManager.GetDongleIdFromC2V(c2v, out errmsg);
                        dongleItem.c2v_basename = System.IO.Path.GetFileNameWithoutExtension(file);
                        if (dongleItem.dongleId == null)
                        {
                            errmsg += "\r\n" + string.Format("({0})", file);
                            throw new Exception();
                        }
                        dongleItems.Add(dongleItem);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(errmsg);
                    return;
                }
            }
            {
                string errmsg = "";
                if (!DongleManagerLib.LicenseManager.GetDongleInfo(dongleItems, out errmsg))
                {
                    MessageBox.Show(errmsg);
                    return;
                }
            }
            c2VListCtl.SetDongleData(licMgr, dongleItems);
        }

        private void c2VListCtl_Load(object sender, EventArgs e)
        {

        }

        private void btnCreateV2C_Click(object sender, EventArgs e)
        {
            List<DongleListItem> dongleItems = c2VListCtl.GetDongleListItem(check_SelectedOnly.Checked);
            List<int> productIdxs = licenseListWithEditBtnCtlInV2C.GetProductIdxs();

            if (dongleItems.Count == 0)
            {
                MessageBox.Show("No C2V to process");
                return;
            }
            if (productIdxs.Count == 0)
            {
                DialogResult res = MessageBox.Show(
                    "No license setting.\r\n"
                    + "Do you create V2C of empty license?",
                    "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                    return;
            }

            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
            dlg.EnsurePathExists = true;
            dlg.IsFolderPicker = true;
            dlg.Multiselect = false;
            dlg.InitialDirectory = licMgr.GetV2CPath();
            dlg.Title = "Select folder to save V2C files";
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

            {
                string errmsg;
                if (licMgr.SetV2CPath(folder, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing is continued.");
                }
            }

            ProcessBurnDlg pd = new ProcessBurnDlg(licMgr, dongleItems, productIdxs, folder);

            //進行状況ダイアログを表示し、その中で書き込みを実行
            pd.ShowDialog(this);

            // ログ保存
            List<DongleListItem> burnedDongleItems = pd.GetBurnedDoubleItems();
            if (burnedDongleItems.Count > 0)
            {
                string errmsg;
                if (licMgr.SaveLog(burnedDongleItems, productIdxs, "V2C-CREATE", out errmsg) < 0)
                {
                    MessageBox.Show(errmsg);
                }
            }
        }

        private void btnLoadV2C_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "v2c files(*.v2c)|*.v2c|All files(*.*)|*.*";
            dlg.Title = "Open V2C files";
            dlg.InitialDirectory = licMgr.GetV2CPath();
            dlg.RestoreDirectory = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.Multiselect = true;

            DialogResult res = dlg.ShowDialog();
            if (res != DialogResult.OK)
                return;

            if( dlg.FileNames.Length > 0 )
            {
                string path = System.IO.Path.GetDirectoryName(dlg.FileNames[0]);
                string errmsg;

                if (licMgr.SetV2CPath(path, out errmsg) < 0)
                {
                    MessageBox.Show(errmsg + "\r\nBut processing is continued.");
                }
            }

            List<DongleListItem> dongleItems = new List<DongleListItem>();

            {
                string errmsg = "";
                try
                {
                    foreach (string file in dlg.FileNames)
                    {
                        string v2cData = DongleManagerLib.LicenseManager.LoadFile(file, out errmsg);
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
            foreach(DongleListItem dongleItem in dongleItems)
            {
                string[] itemStr = new string[] { dongleItem.v2c_filename };
                ListViewItemDongle item = new ListViewItemDongle(itemStr, dongleItem);
                listV2C.Items.Add(item);
            }
        }

        private void btnBurnV2C_Click(object sender, EventArgs e)
        {
            List<DongleListItem> dongleItems = new List<DongleListItem>();

            if (check_SelectedOnly.Checked)
            {
                foreach (ListViewItem i in listV2C.SelectedItems)
                    dongleItems.Add(((ListViewItemDongle)i).dongleItem);
            }
            else
            {
                foreach (ListViewItem i in listV2C.Items)
                    dongleItems.Add(((ListViewItemDongle)i).dongleItem);
            }

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
                string errmsg;
                if (licMgr.SaveLog(burnedDongleItems, null, "BURN-V2C", out errmsg) < 0)
                {
                    MessageBox.Show(errmsg);
                }
            }
        }
    }
}
