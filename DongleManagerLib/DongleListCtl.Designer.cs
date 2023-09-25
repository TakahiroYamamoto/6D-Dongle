namespace DongleManagerLib
{
    partial class DongleListCtl
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
            this.listDongle = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProducts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFeatures = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listDongle
            // 
            this.listDongle.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnProducts,
            this.columnFeatures});
            this.listDongle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDongle.FullRowSelect = true;
            this.listDongle.GridLines = true;
            this.listDongle.HideSelection = false;
            this.listDongle.Location = new System.Drawing.Point(0, 0);
            this.listDongle.MultiSelect = false;
            this.listDongle.Name = "listDongle";
            this.listDongle.Size = new System.Drawing.Size(710, 130);
            this.listDongle.TabIndex = 11;
            this.listDongle.UseCompatibleStateImageBehavior = false;
            this.listDongle.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Dongle ID";
            this.columnHeader1.Width = 70;
            // 
            // columnProducts
            // 
            this.columnProducts.Text = "Products";
            this.columnProducts.Width = 219;
            // 
            // columnFeatures
            // 
            this.columnFeatures.Text = "Features";
            this.columnFeatures.Width = 161;
            // 
            // DongleListCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listDongle);
            this.Name = "DongleListCtl";
            this.Size = new System.Drawing.Size(710, 130);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listDongle;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnProducts;
        private System.Windows.Forms.ColumnHeader columnFeatures;
    }
}
