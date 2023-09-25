namespace DongleTool
{
    partial class DongleToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DongleToolForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.enumDongleCtlC2V = new DongleManagerLib.EnumDongleCtl();
            this.panelBottomC2V = new System.Windows.Forms.Panel();
            this.btnHelpC2V = new System.Windows.Forms.Button();
            this.btnCreateC2V = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listV2C = new System.Windows.Forms.ListView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBurnV2C = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnLoadV2C = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelBottomC2V.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.enumDongleCtlC2V);
            this.tabPage1.Controls.Add(this.panelBottomC2V);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // enumDongleCtlC2V
            // 
            resources.ApplyResources(this.enumDongleCtlC2V, "enumDongleCtlC2V");
            this.enumDongleCtlC2V.Name = "enumDongleCtlC2V";
            // 
            // panelBottomC2V
            // 
            this.panelBottomC2V.Controls.Add(this.btnHelpC2V);
            this.panelBottomC2V.Controls.Add(this.btnCreateC2V);
            resources.ApplyResources(this.panelBottomC2V, "panelBottomC2V");
            this.panelBottomC2V.Name = "panelBottomC2V";
            // 
            // btnHelpC2V
            // 
            resources.ApplyResources(this.btnHelpC2V, "btnHelpC2V");
            this.btnHelpC2V.Name = "btnHelpC2V";
            this.btnHelpC2V.UseVisualStyleBackColor = true;
            this.btnHelpC2V.Click += new System.EventHandler(this.btnHelpC2V_Click);
            // 
            // btnCreateC2V
            // 
            resources.ApplyResources(this.btnCreateC2V, "btnCreateC2V");
            this.btnCreateC2V.Name = "btnCreateC2V";
            this.btnCreateC2V.UseVisualStyleBackColor = true;
            this.btnCreateC2V.Click += new System.EventHandler(this.btnCreateC2V_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.listV2C);
            this.tabPage2.Controls.Add(this.panel8);
            this.tabPage2.Controls.Add(this.panel7);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // listV2C
            // 
            resources.ApplyResources(this.listV2C, "listV2C");
            this.listV2C.GridLines = true;
            this.listV2C.Name = "listV2C";
            this.listV2C.UseCompatibleStateImageBehavior = false;
            this.listV2C.View = System.Windows.Forms.View.List;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.button1);
            this.panel8.Controls.Add(this.btnBurnV2C);
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBurnV2C
            // 
            resources.ApplyResources(this.btnBurnV2C, "btnBurnV2C");
            this.btnBurnV2C.Name = "btnBurnV2C";
            this.btnBurnV2C.UseVisualStyleBackColor = true;
            this.btnBurnV2C.Click += new System.EventHandler(this.btnBurnV2C_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnLoadV2C);
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Name = "panel7";
            // 
            // btnLoadV2C
            // 
            resources.ApplyResources(this.btnLoadV2C, "btnLoadV2C");
            this.btnLoadV2C.Name = "btnLoadV2C";
            this.btnLoadV2C.UseVisualStyleBackColor = true;
            this.btnLoadV2C.Click += new System.EventHandler(this.btnLoadV2C_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DongleToolForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.label1);
            this.Name = "DongleToolForm";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panelBottomC2V.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DongleManagerLib.EnumDongleCtl enumDongleCtlC2V;
        private System.Windows.Forms.Panel panelBottomC2V;
        private System.Windows.Forms.Button btnCreateC2V;
        private System.Windows.Forms.Button btnHelpC2V;
        private System.Windows.Forms.ListView listV2C;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnBurnV2C;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnLoadV2C;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}

