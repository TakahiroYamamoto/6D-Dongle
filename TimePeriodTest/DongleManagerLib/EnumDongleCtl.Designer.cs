namespace DongleManagerLib
{
    partial class EnumDongleCtl
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
            this.btnEnumDongle = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnToClipboard = new System.Windows.Forms.Button();
            this.btnBlinkDongle = new System.Windows.Forms.Button();
            this.dongleListCtl = new DongleManagerLib.DongleListCtl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEnumDongle
            // 
            this.btnEnumDongle.Location = new System.Drawing.Point(2, 1);
            this.btnEnumDongle.Name = "btnEnumDongle";
            this.btnEnumDongle.Size = new System.Drawing.Size(120, 23);
            this.btnEnumDongle.TabIndex = 6;
            this.btnEnumDongle.Text = "Detect Dongle";
            this.btnEnumDongle.UseVisualStyleBackColor = true;
            this.btnEnumDongle.Click += new System.EventHandler(this.btnEnumDongle_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnToClipboard);
            this.panel1.Controls.Add(this.btnBlinkDongle);
            this.panel1.Controls.Add(this.btnEnumDongle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(754, 26);
            this.panel1.TabIndex = 8;
            // 
            // btnToClipboard
            // 
            this.btnToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToClipboard.Location = new System.Drawing.Point(640, 2);
            this.btnToClipboard.Name = "btnToClipboard";
            this.btnToClipboard.Size = new System.Drawing.Size(111, 23);
            this.btnToClipboard.TabIndex = 13;
            this.btnToClipboard.Text = "To Clipboard";
            this.btnToClipboard.UseVisualStyleBackColor = true;
            this.btnToClipboard.Click += new System.EventHandler(this.btnToClipboard_Click);
            // 
            // btnBlinkDongle
            // 
            this.btnBlinkDongle.Location = new System.Drawing.Point(126, 2);
            this.btnBlinkDongle.Name = "btnBlinkDongle";
            this.btnBlinkDongle.Size = new System.Drawing.Size(135, 23);
            this.btnBlinkDongle.TabIndex = 12;
            this.btnBlinkDongle.Text = "Blink Selected Dongle";
            this.btnBlinkDongle.UseVisualStyleBackColor = true;
            this.btnBlinkDongle.Click += new System.EventHandler(this.btnBlinkDongle_Click);
            // 
            // dongleListCtl
            // 
            this.dongleListCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dongleListCtl.Location = new System.Drawing.Point(0, 26);
            this.dongleListCtl.Name = "dongleListCtl";
            this.dongleListCtl.Size = new System.Drawing.Size(754, 124);
            this.dongleListCtl.TabIndex = 7;
            this.dongleListCtl.Load += new System.EventHandler(this.c2VListCtl1_Load);
            // 
            // EnumDongleCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dongleListCtl);
            this.Controls.Add(this.panel1);
            this.Name = "EnumDongleCtl";
            this.Size = new System.Drawing.Size(754, 150);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEnumDongle;
        private DongleListCtl dongleListCtl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBlinkDongle;
        private System.Windows.Forms.Button btnToClipboard;
    }
}
