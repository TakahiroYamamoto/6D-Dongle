
namespace DongleManagerLib
{
    partial class FeatureWithPeriodCtl
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.columnFeatureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnKind = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listFeatures = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_PeriodSetting = new System.Windows.Forms.Panel();
            this.btn_SetPeriod = new System.Windows.Forms.Button();
            this.panel_PeriodDays = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.num_PeriodDays = new System.Windows.Forms.NumericUpDown();
            this.dateTime_PeriodDate = new System.Windows.Forms.DateTimePicker();
            this.columnPeriod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel_PeriodSetting.SuspendLayout();
            this.panel_PeriodDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_PeriodDays)).BeginInit();
            this.SuspendLayout();
            // 
            // columnFeatureName
            // 
            this.columnFeatureName.Text = "Feature";
            this.columnFeatureName.Width = 129;
            // 
            // columnKind
            // 
            this.columnKind.Text = "種類";
            this.columnKind.Width = 30;
            // 
            // listFeatures
            // 
            this.listFeatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFeatureName,
            this.columnKind,
            this.columnPeriod});
            this.listFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listFeatures.FullRowSelect = true;
            this.listFeatures.GridLines = true;
            this.listFeatures.HideSelection = false;
            this.listFeatures.Location = new System.Drawing.Point(0, 16);
            this.listFeatures.MultiSelect = false;
            this.listFeatures.Name = "listFeatures";
            this.listFeatures.Size = new System.Drawing.Size(654, 104);
            this.listFeatures.TabIndex = 13;
            this.listFeatures.UseCompatibleStateImageBehavior = false;
            this.listFeatures.View = System.Windows.Forms.View.Details;
            this.listFeatures.SelectedIndexChanged += new System.EventHandler(this.listFeatures_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(654, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "期限設定";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_PeriodSetting
            // 
            this.panel_PeriodSetting.Controls.Add(this.btn_SetPeriod);
            this.panel_PeriodSetting.Controls.Add(this.panel_PeriodDays);
            this.panel_PeriodSetting.Controls.Add(this.dateTime_PeriodDate);
            this.panel_PeriodSetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_PeriodSetting.Location = new System.Drawing.Point(0, 120);
            this.panel_PeriodSetting.Name = "panel_PeriodSetting";
            this.panel_PeriodSetting.Size = new System.Drawing.Size(654, 30);
            this.panel_PeriodSetting.TabIndex = 15;
            // 
            // btn_SetPeriod
            // 
            this.btn_SetPeriod.Location = new System.Drawing.Point(247, 3);
            this.btn_SetPeriod.Name = "btn_SetPeriod";
            this.btn_SetPeriod.Size = new System.Drawing.Size(54, 23);
            this.btn_SetPeriod.TabIndex = 2;
            this.btn_SetPeriod.Text = "Set";
            this.btn_SetPeriod.UseVisualStyleBackColor = true;
            this.btn_SetPeriod.Click += new System.EventHandler(this.btn_SetPeriod_Click);
            // 
            // panel_PeriodDays
            // 
            this.panel_PeriodDays.Controls.Add(this.label2);
            this.panel_PeriodDays.Controls.Add(this.num_PeriodDays);
            this.panel_PeriodDays.Location = new System.Drawing.Point(146, 2);
            this.panel_PeriodDays.Name = "panel_PeriodDays";
            this.panel_PeriodDays.Size = new System.Drawing.Size(90, 23);
            this.panel_PeriodDays.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "日";
            // 
            // num_PeriodDays
            // 
            this.num_PeriodDays.Location = new System.Drawing.Point(3, 2);
            this.num_PeriodDays.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_PeriodDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_PeriodDays.Name = "num_PeriodDays";
            this.num_PeriodDays.Size = new System.Drawing.Size(60, 19);
            this.num_PeriodDays.TabIndex = 0;
            this.num_PeriodDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.num_PeriodDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dateTime_PeriodDate
            // 
            this.dateTime_PeriodDate.Location = new System.Drawing.Point(10, 4);
            this.dateTime_PeriodDate.Name = "dateTime_PeriodDate";
            this.dateTime_PeriodDate.Size = new System.Drawing.Size(122, 19);
            this.dateTime_PeriodDate.TabIndex = 0;
            // 
            // columnPeriod
            // 
            this.columnPeriod.Text = "期限";
            // 
            // FeatureWithPeriodCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listFeatures);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel_PeriodSetting);
            this.Name = "FeatureWithPeriodCtl";
            this.Size = new System.Drawing.Size(654, 150);
            this.panel_PeriodSetting.ResumeLayout(false);
            this.panel_PeriodDays.ResumeLayout(false);
            this.panel_PeriodDays.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_PeriodDays)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnFeatureName;
        private System.Windows.Forms.ColumnHeader columnKind;
        private System.Windows.Forms.ListView listFeatures;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_PeriodSetting;
        private System.Windows.Forms.DateTimePicker dateTime_PeriodDate;
        private System.Windows.Forms.Panel panel_PeriodDays;
        private System.Windows.Forms.NumericUpDown num_PeriodDays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_SetPeriod;
        private System.Windows.Forms.ColumnHeader columnPeriod;
    }
}
