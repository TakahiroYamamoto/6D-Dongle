
namespace PeriodTestForm
{
    partial class PeriodTestForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.btn_Test = new System.Windows.Forms.Button();
            this.enumDongleCtl = new DongleToolLib.EnumDongleCtl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.check_SelectedOnly = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Test
            // 
            this.btn_Test.Location = new System.Drawing.Point(3, 3);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(75, 23);
            this.btn_Test.TabIndex = 0;
            this.btn_Test.Text = "Test";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // enumDongleCtl
            // 
            this.enumDongleCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumDongleCtl.Location = new System.Drawing.Point(0, 0);
            this.enumDongleCtl.Name = "enumDongleCtl";
            this.enumDongleCtl.Size = new System.Drawing.Size(788, 196);
            this.enumDongleCtl.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.check_SelectedOnly);
            this.panel1.Controls.Add(this.btn_Test);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 167);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 29);
            this.panel1.TabIndex = 2;
            // 
            // check_SelectedOnly
            // 
            this.check_SelectedOnly.AutoSize = true;
            this.check_SelectedOnly.Location = new System.Drawing.Point(99, 7);
            this.check_SelectedOnly.Name = "check_SelectedOnly";
            this.check_SelectedOnly.Size = new System.Drawing.Size(95, 16);
            this.check_SelectedOnly.TabIndex = 1;
            this.check_SelectedOnly.Text = "Selected Only";
            this.check_SelectedOnly.UseVisualStyleBackColor = true;
            // 
            // PeriodTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 196);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.enumDongleCtl);
            this.Name = "PeriodTestForm";
            this.Text = "Period Test";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Test;
        private DongleToolLib.EnumDongleCtl enumDongleCtl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox check_SelectedOnly;
    }
}

