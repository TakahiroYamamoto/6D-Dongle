namespace DongleManager
{
    partial class LookIntoContentsCtl
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
            this.btnLookIntoContents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLookIntoContents
            // 
            this.btnLookIntoContents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnLookIntoContents.Location = new System.Drawing.Point(3, 0);
            this.btnLookIntoContents.Name = "btnLookIntoContents";
            this.btnLookIntoContents.Size = new System.Drawing.Size(122, 23);
            this.btnLookIntoContents.TabIndex = 13;
            this.btnLookIntoContents.Text = "Look into Contents";
            this.btnLookIntoContents.UseVisualStyleBackColor = false;
            this.btnLookIntoContents.Click += new System.EventHandler(this.btnLookIntoContents_Click);
            // 
            // LookIntoContentsCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLookIntoContents);
            this.Name = "LookIntoContentsCtl";
            this.Size = new System.Drawing.Size(558, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLookIntoContents;
    }
}
