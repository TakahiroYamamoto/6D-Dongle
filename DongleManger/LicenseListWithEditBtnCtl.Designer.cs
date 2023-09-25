namespace DongleManager
{
    partial class LicenseListWithEditBtnCtl
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEditLicense = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.licenseListCtl = new DongleManagerLib.LicenseListCtl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEditLicense
            // 
            this.btnEditLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnEditLicense.Location = new System.Drawing.Point(3, 3);
            this.btnEditLicense.Name = "btnEditLicense";
            this.btnEditLicense.Size = new System.Drawing.Size(99, 23);
            this.btnEditLicense.TabIndex = 1;
            this.btnEditLicense.Text = "Edit License";
            this.btnEditLicense.UseVisualStyleBackColor = false;
            this.btnEditLicense.Click += new System.EventHandler(this.btnEditLicense_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEditLicense);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 28);
            this.panel1.TabIndex = 2;
            // 
            // licenseListCtl
            // 
            this.licenseListCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.licenseListCtl.Location = new System.Drawing.Point(0, 28);
            this.licenseListCtl.Name = "licenseListCtl";
            this.licenseListCtl.Size = new System.Drawing.Size(797, 122);
            this.licenseListCtl.TabIndex = 3;
            // 
            // LicenseListWithEditBtnCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.licenseListCtl);
            this.Controls.Add(this.panel1);
            this.Name = "LicenseListWithEditBtnCtl";
            this.Size = new System.Drawing.Size(797, 150);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnEditLicense;
        private System.Windows.Forms.Panel panel1;
        private DongleManagerLib.LicenseListCtl licenseListCtl;
    }
}
