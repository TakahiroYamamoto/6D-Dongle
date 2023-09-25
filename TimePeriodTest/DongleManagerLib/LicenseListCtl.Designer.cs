namespace DongleManagerLib
{
    partial class LicenseListCtl
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
            this.listLicense = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLicense = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listLicense
            // 
            this.listLicense.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnLicense});
            this.listLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLicense.FullRowSelect = true;
            this.listLicense.GridLines = true;
            this.listLicense.HideSelection = false;
            this.listLicense.Location = new System.Drawing.Point(0, 0);
            this.listLicense.Name = "listLicense";
            this.listLicense.Size = new System.Drawing.Size(767, 150);
            this.listLicense.TabIndex = 12;
            this.listLicense.UseCompatibleStateImageBehavior = false;
            this.listLicense.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Product";
            this.columnHeader1.Width = 129;
            // 
            // columnLicense
            // 
            this.columnLicense.Text = "Features";
            this.columnLicense.Width = 634;
            // 
            // LicenseListCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listLicense);
            this.Name = "LicenseListCtl";
            this.Size = new System.Drawing.Size(767, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listLicense;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnLicense;
    }
}
