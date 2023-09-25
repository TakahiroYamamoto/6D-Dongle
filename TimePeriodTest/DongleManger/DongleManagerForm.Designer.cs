namespace DongleManager
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreateV2C = new System.Windows.Forms.Button();
            this.btnBurn = new System.Windows.Forms.Button();
            this.btnLoadC2V = new System.Windows.Forms.Button();
            this.tabControlMode = new System.Windows.Forms.TabControl();
            this.tabPageDirectBurn = new System.Windows.Forms.TabPage();
            this.licenseListWithEditBtnCtlInDirect = new DongleManager.LicenseListWithEditBtnCtl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnLofFolder = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lookIntoContentsCtl_Direct = new DongleManager.LookIntoContentsCtl();
            this.enumDongleCtl_Direct = new DongleToolLib.EnumDongleCtl();
            this.tabV2C = new System.Windows.Forms.TabPage();
            this.licenseListWithEditBtnCtlInV2C = new DongleManager.LicenseListWithEditBtnCtl();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLogFolder2 = new System.Windows.Forms.Button();
            this.lookIntoContentsCtl_V2C = new DongleManager.LookIntoContentsCtl();
            this.c2VListCtl = new DongleToolLib.DongleListCtl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabC2V = new System.Windows.Forms.TabPage();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.listV2C = new System.Windows.Forms.ListView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnBurnV2C = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnLoadV2C = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panelCreateC2V = new System.Windows.Forms.Panel();
            this.enumDongleCtlC2V = new DongleToolLib.EnumDongleCtl();
            this.lookIntoContentsCtl_C2V = new DongleManager.LookIntoContentsCtl();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnCreateC2V = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.check_SelectedOnly = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textLicenseDefVer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLicenseDefUpdate = new System.Windows.Forms.Button();
            this.tabControlMode.SuspendLayout();
            this.tabPageDirectBurn.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabV2C.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabC2V.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panelCreateC2V.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateV2C
            // 
            this.btnCreateV2C.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCreateV2C.Location = new System.Drawing.Point(3, 5);
            this.btnCreateV2C.Name = "btnCreateV2C";
            this.btnCreateV2C.Size = new System.Drawing.Size(83, 23);
            this.btnCreateV2C.TabIndex = 1;
            this.btnCreateV2C.Text = "Create V2C";
            this.btnCreateV2C.UseVisualStyleBackColor = false;
            this.btnCreateV2C.Click += new System.EventHandler(this.btnCreateV2C_Click);
            // 
            // btnBurn
            // 
            this.btnBurn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnBurn.Location = new System.Drawing.Point(3, 3);
            this.btnBurn.Name = "btnBurn";
            this.btnBurn.Size = new System.Drawing.Size(106, 23);
            this.btnBurn.TabIndex = 2;
            this.btnBurn.Text = "Burn to Dongle";
            this.btnBurn.UseVisualStyleBackColor = false;
            this.btnBurn.Click += new System.EventHandler(this.btnBurn_Click);
            // 
            // btnLoadC2V
            // 
            this.btnLoadC2V.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnLoadC2V.Location = new System.Drawing.Point(3, 3);
            this.btnLoadC2V.Name = "btnLoadC2V";
            this.btnLoadC2V.Size = new System.Drawing.Size(75, 23);
            this.btnLoadC2V.TabIndex = 8;
            this.btnLoadC2V.Text = "Load C2V";
            this.btnLoadC2V.UseVisualStyleBackColor = false;
            this.btnLoadC2V.Click += new System.EventHandler(this.btnLoadC2V_Click);
            // 
            // tabControlMode
            // 
            this.tabControlMode.Controls.Add(this.tabPageDirectBurn);
            this.tabControlMode.Controls.Add(this.tabV2C);
            this.tabControlMode.Controls.Add(this.tabC2V);
            this.tabControlMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMode.Location = new System.Drawing.Point(0, 33);
            this.tabControlMode.Name = "tabControlMode";
            this.tabControlMode.SelectedIndex = 0;
            this.tabControlMode.Size = new System.Drawing.Size(696, 485);
            this.tabControlMode.TabIndex = 15;
            // 
            // tabPageDirectBurn
            // 
            this.tabPageDirectBurn.Controls.Add(this.licenseListWithEditBtnCtlInDirect);
            this.tabPageDirectBurn.Controls.Add(this.panel3);
            this.tabPageDirectBurn.Controls.Add(this.splitter1);
            this.tabPageDirectBurn.Controls.Add(this.lookIntoContentsCtl_Direct);
            this.tabPageDirectBurn.Controls.Add(this.enumDongleCtl_Direct);
            this.tabPageDirectBurn.Location = new System.Drawing.Point(4, 22);
            this.tabPageDirectBurn.Name = "tabPageDirectBurn";
            this.tabPageDirectBurn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDirectBurn.Size = new System.Drawing.Size(688, 459);
            this.tabPageDirectBurn.TabIndex = 0;
            this.tabPageDirectBurn.Text = "Direct Burning";
            this.tabPageDirectBurn.UseVisualStyleBackColor = true;
            // 
            // licenseListWithEditBtnCtlInDirect
            // 
            this.licenseListWithEditBtnCtlInDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseListWithEditBtnCtlInDirect.Location = new System.Drawing.Point(3, 179);
            this.licenseListWithEditBtnCtlInDirect.Name = "licenseListWithEditBtnCtlInDirect";
            this.licenseListWithEditBtnCtlInDirect.Size = new System.Drawing.Size(682, 248);
            this.licenseListWithEditBtnCtlInDirect.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnLofFolder);
            this.panel3.Controls.Add(this.btnBurn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 427);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(682, 29);
            this.panel3.TabIndex = 4;
            // 
            // btnLofFolder
            // 
            this.btnLofFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLofFolder.Location = new System.Drawing.Point(602, 3);
            this.btnLofFolder.Name = "btnLofFolder";
            this.btnLofFolder.Size = new System.Drawing.Size(75, 23);
            this.btnLofFolder.TabIndex = 3;
            this.btnLofFolder.Text = "Log Folder";
            this.btnLofFolder.UseVisualStyleBackColor = true;
            this.btnLofFolder.Click += new System.EventHandler(this.btnLofFolder_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(3, 176);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(682, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // lookIntoContentsCtl_Direct
            // 
            this.lookIntoContentsCtl_Direct.Dock = System.Windows.Forms.DockStyle.Top;
            this.lookIntoContentsCtl_Direct.Location = new System.Drawing.Point(3, 153);
            this.lookIntoContentsCtl_Direct.Name = "lookIntoContentsCtl_Direct";
            this.lookIntoContentsCtl_Direct.Size = new System.Drawing.Size(682, 23);
            this.lookIntoContentsCtl_Direct.TabIndex = 6;
            // 
            // enumDongleCtl_Direct
            // 
            this.enumDongleCtl_Direct.Dock = System.Windows.Forms.DockStyle.Top;
            this.enumDongleCtl_Direct.Location = new System.Drawing.Point(3, 3);
            this.enumDongleCtl_Direct.Name = "enumDongleCtl_Direct";
            this.enumDongleCtl_Direct.Size = new System.Drawing.Size(682, 150);
            this.enumDongleCtl_Direct.TabIndex = 7;
            // 
            // tabV2C
            // 
            this.tabV2C.Controls.Add(this.licenseListWithEditBtnCtlInV2C);
            this.tabV2C.Controls.Add(this.splitter2);
            this.tabV2C.Controls.Add(this.panel2);
            this.tabV2C.Controls.Add(this.lookIntoContentsCtl_V2C);
            this.tabV2C.Controls.Add(this.c2VListCtl);
            this.tabV2C.Controls.Add(this.panel1);
            this.tabV2C.Location = new System.Drawing.Point(4, 22);
            this.tabV2C.Name = "tabV2C";
            this.tabV2C.Padding = new System.Windows.Forms.Padding(3);
            this.tabV2C.Size = new System.Drawing.Size(688, 459);
            this.tabV2C.TabIndex = 1;
            this.tabV2C.Text = "Create V2C";
            this.tabV2C.UseVisualStyleBackColor = true;
            // 
            // licenseListWithEditBtnCtlInV2C
            // 
            this.licenseListWithEditBtnCtlInV2C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseListWithEditBtnCtlInV2C.Location = new System.Drawing.Point(3, 189);
            this.licenseListWithEditBtnCtlInV2C.Name = "licenseListWithEditBtnCtlInV2C";
            this.licenseListWithEditBtnCtlInV2C.Size = new System.Drawing.Size(682, 236);
            this.licenseListWithEditBtnCtlInV2C.TabIndex = 14;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(3, 186);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(682, 3);
            this.splitter2.TabIndex = 13;
            this.splitter2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLogFolder2);
            this.panel2.Controls.Add(this.btnCreateV2C);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 425);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(682, 31);
            this.panel2.TabIndex = 12;
            // 
            // btnLogFolder2
            // 
            this.btnLogFolder2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFolder2.Location = new System.Drawing.Point(602, 5);
            this.btnLogFolder2.Name = "btnLogFolder2";
            this.btnLogFolder2.Size = new System.Drawing.Size(75, 23);
            this.btnLogFolder2.TabIndex = 4;
            this.btnLogFolder2.Text = "Log Folder";
            this.btnLogFolder2.UseVisualStyleBackColor = true;
            this.btnLogFolder2.Click += new System.EventHandler(this.btnLogFolder2_Click);
            // 
            // lookIntoContentsCtl_V2C
            // 
            this.lookIntoContentsCtl_V2C.Dock = System.Windows.Forms.DockStyle.Top;
            this.lookIntoContentsCtl_V2C.Location = new System.Drawing.Point(3, 161);
            this.lookIntoContentsCtl_V2C.Name = "lookIntoContentsCtl_V2C";
            this.lookIntoContentsCtl_V2C.Size = new System.Drawing.Size(682, 25);
            this.lookIntoContentsCtl_V2C.TabIndex = 15;
            // 
            // c2VListCtl
            // 
            this.c2VListCtl.Dock = System.Windows.Forms.DockStyle.Top;
            this.c2VListCtl.Location = new System.Drawing.Point(3, 31);
            this.c2VListCtl.Name = "c2VListCtl";
            this.c2VListCtl.Size = new System.Drawing.Size(682, 130);
            this.c2VListCtl.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoadC2V);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 28);
            this.panel1.TabIndex = 11;
            // 
            // tabC2V
            // 
            this.tabC2V.Controls.Add(this.panelBottom);
            this.tabC2V.Controls.Add(this.splitter3);
            this.tabC2V.Controls.Add(this.panelCreateC2V);
            this.tabC2V.Location = new System.Drawing.Point(4, 22);
            this.tabC2V.Name = "tabC2V";
            this.tabC2V.Size = new System.Drawing.Size(688, 459);
            this.tabC2V.TabIndex = 2;
            this.tabC2V.Text = "Create C2V and Burn";
            this.tabC2V.UseVisualStyleBackColor = true;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.listV2C);
            this.panelBottom.Controls.Add(this.panel8);
            this.panelBottom.Controls.Add(this.panel7);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 235);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(688, 224);
            this.panelBottom.TabIndex = 14;
            // 
            // listV2C
            // 
            this.listV2C.GridLines = true;
            this.listV2C.HideSelection = false;
            this.listV2C.Location = new System.Drawing.Point(0, 25);
            this.listV2C.Name = "listV2C";
            this.listV2C.Size = new System.Drawing.Size(688, 171);
            this.listV2C.TabIndex = 20;
            this.listV2C.UseCompatibleStateImageBehavior = false;
            this.listV2C.View = System.Windows.Forms.View.List;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnBurnV2C);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 196);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(688, 28);
            this.panel8.TabIndex = 19;
            // 
            // btnBurnV2C
            // 
            this.btnBurnV2C.Location = new System.Drawing.Point(3, 3);
            this.btnBurnV2C.Name = "btnBurnV2C";
            this.btnBurnV2C.Size = new System.Drawing.Size(106, 23);
            this.btnBurnV2C.TabIndex = 17;
            this.btnBurnV2C.Text = "Burn to Dongle";
            this.btnBurnV2C.UseVisualStyleBackColor = true;
            this.btnBurnV2C.Click += new System.EventHandler(this.btnBurnV2C_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnLoadV2C);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(688, 25);
            this.panel7.TabIndex = 18;
            // 
            // btnLoadV2C
            // 
            this.btnLoadV2C.Location = new System.Drawing.Point(0, 1);
            this.btnLoadV2C.Name = "btnLoadV2C";
            this.btnLoadV2C.Size = new System.Drawing.Size(75, 23);
            this.btnLoadV2C.TabIndex = 15;
            this.btnLoadV2C.Text = "Load　V2C";
            this.btnLoadV2C.UseVisualStyleBackColor = true;
            this.btnLoadV2C.Click += new System.EventHandler(this.btnLoadV2C_Click);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 232);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(688, 3);
            this.splitter3.TabIndex = 16;
            this.splitter3.TabStop = false;
            // 
            // panelCreateC2V
            // 
            this.panelCreateC2V.Controls.Add(this.enumDongleCtlC2V);
            this.panelCreateC2V.Controls.Add(this.lookIntoContentsCtl_C2V);
            this.panelCreateC2V.Controls.Add(this.panel6);
            this.panelCreateC2V.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCreateC2V.Location = new System.Drawing.Point(0, 0);
            this.panelCreateC2V.Name = "panelCreateC2V";
            this.panelCreateC2V.Size = new System.Drawing.Size(688, 232);
            this.panelCreateC2V.TabIndex = 15;
            // 
            // enumDongleCtlC2V
            // 
            this.enumDongleCtlC2V.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumDongleCtlC2V.Location = new System.Drawing.Point(0, 0);
            this.enumDongleCtlC2V.Name = "enumDongleCtlC2V";
            this.enumDongleCtlC2V.Size = new System.Drawing.Size(688, 154);
            this.enumDongleCtlC2V.TabIndex = 19;
            // 
            // lookIntoContentsCtl_C2V
            // 
            this.lookIntoContentsCtl_C2V.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lookIntoContentsCtl_C2V.Location = new System.Drawing.Point(0, 154);
            this.lookIntoContentsCtl_C2V.Name = "lookIntoContentsCtl_C2V";
            this.lookIntoContentsCtl_C2V.Size = new System.Drawing.Size(688, 25);
            this.lookIntoContentsCtl_C2V.TabIndex = 18;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnCreateC2V);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 179);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(688, 53);
            this.panel6.TabIndex = 17;
            // 
            // btnCreateC2V
            // 
            this.btnCreateC2V.Location = new System.Drawing.Point(3, 3);
            this.btnCreateC2V.Name = "btnCreateC2V";
            this.btnCreateC2V.Size = new System.Drawing.Size(75, 23);
            this.btnCreateC2V.TabIndex = 14;
            this.btnCreateC2V.Text = "Create C2V";
            this.btnCreateC2V.UseVisualStyleBackColor = true;
            this.btnCreateC2V.Click += new System.EventHandler(this.btnCreateC2V_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "Send C2V files, then receive V2C files and Burn V2C files to dongles";
            // 
            // check_SelectedOnly
            // 
            this.check_SelectedOnly.AutoSize = true;
            this.check_SelectedOnly.Location = new System.Drawing.Point(506, 8);
            this.check_SelectedOnly.Name = "check_SelectedOnly";
            this.check_SelectedOnly.Size = new System.Drawing.Size(140, 16);
            this.check_SelectedOnly.TabIndex = 4;
            this.check_SelectedOnly.Text = "[Spetial] Selected only";
            this.check_SelectedOnly.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.check_SelectedOnly);
            this.panel5.Controls.Add(this.textLicenseDefVer);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.btnLicenseDefUpdate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(696, 33);
            this.panel5.TabIndex = 16;
            // 
            // textLicenseDefVer
            // 
            this.textLicenseDefVer.Location = new System.Drawing.Point(141, 5);
            this.textLicenseDefVer.Name = "textLicenseDefVer";
            this.textLicenseDefVer.ReadOnly = true;
            this.textLicenseDefVer.Size = new System.Drawing.Size(100, 19);
            this.textLicenseDefVer.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "License Definition Ver. =";
            // 
            // btnLicenseDefUpdate
            // 
            this.btnLicenseDefUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.btnLicenseDefUpdate.Location = new System.Drawing.Point(254, 3);
            this.btnLicenseDefUpdate.Name = "btnLicenseDefUpdate";
            this.btnLicenseDefUpdate.Size = new System.Drawing.Size(156, 23);
            this.btnLicenseDefUpdate.TabIndex = 0;
            this.btnLicenseDefUpdate.Text = "Update License Definition";
            this.btnLicenseDefUpdate.UseVisualStyleBackColor = false;
            this.btnLicenseDefUpdate.Click += new System.EventHandler(this.btnLicenseDefUpdate_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 518);
            this.Controls.Add(this.tabControlMode);
            this.Controls.Add(this.panel5);
            this.Name = "FormMain";
            this.Text = "Dongle Manager";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tabControlMode.ResumeLayout(false);
            this.tabPageDirectBurn.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabV2C.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabC2V.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panelCreateC2V.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCreateV2C;
        private System.Windows.Forms.Button btnBurn;
        private System.Windows.Forms.Button btnLoadC2V;
        private System.Windows.Forms.TabControl tabControlMode;
        private System.Windows.Forms.TabPage tabPageDirectBurn;
        private System.Windows.Forms.TabPage tabV2C;
        private DongleManager.LicenseListWithEditBtnCtl licenseListWithEditBtnCtlInDirect;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private DongleManager.LicenseListWithEditBtnCtl licenseListWithEditBtnCtlInV2C;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabC2V;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnBurnV2C;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadV2C;
        private System.Windows.Forms.Button btnCreateC2V;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox textLicenseDefVer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLicenseDefUpdate;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button btnLofFolder;
        private System.Windows.Forms.Button btnLogFolder2;
        private System.Windows.Forms.Panel panelCreateC2V;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.ListView listV2C;
        private LookIntoContentsCtl lookIntoContentsCtl_Direct;
        private LookIntoContentsCtl lookIntoContentsCtl_V2C;
        private LookIntoContentsCtl lookIntoContentsCtl_C2V;
        private System.Windows.Forms.CheckBox check_SelectedOnly;
        private DongleToolLib.EnumDongleCtl enumDongleCtl_Direct;
        private DongleToolLib.DongleListCtl c2VListCtl;
        private DongleToolLib.EnumDongleCtl enumDongleCtlC2V;
    }
}

