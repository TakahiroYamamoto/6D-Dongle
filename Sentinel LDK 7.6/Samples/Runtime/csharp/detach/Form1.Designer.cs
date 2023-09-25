namespace detach_cs
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        private void InitializeComponent()
        {
            this.OpenH2R = new System.Windows.Forms.OpenFileDialog();
            this.SaveH2R = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.NumericDuration = new System.Windows.Forms.NumericUpDown();
            this.ButtonAttach = new System.Windows.Forms.Button();
            this.ButtonDetach = new System.Windows.Forms.Button();
            this.ComboRemoteDestination = new System.Windows.Forms.ComboBox();
            this.RadioRemote = new System.Windows.Forms.RadioButton();
            this.RadioLocal = new System.Windows.Forms.RadioButton();
            this.ComboProduct = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDuration)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenH2R
            // 
            this.OpenH2R.DefaultExt = "h2r";
            this.OpenH2R.FileName = "detach_sample.h2r";
            this.OpenH2R.Filter = "h2r File|*.h2r|All files|*.*";
            this.OpenH2R.Title = "Open H2R file";
            // 
            // SaveH2R
            // 
            this.SaveH2R.DefaultExt = "h2r";
            this.SaveH2R.FileName = "detach_sample.h2r";
            this.SaveH2R.Filter = "h2r File|*.h2r|All files|*.*";
            this.SaveH2R.Title = "Save H2R file";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Detach Duration (sec):";
            // 
            // NumericDuration
            // 
            this.NumericDuration.AccessibleDescription = "";
            this.NumericDuration.AccessibleName = "";
            this.NumericDuration.Location = new System.Drawing.Point(200, 145);
            this.NumericDuration.Maximum = new decimal(new int[] {
            360000,
            0,
            0,
            0});
            this.NumericDuration.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NumericDuration.Name = "NumericDuration";
            this.NumericDuration.Size = new System.Drawing.Size(86, 20);
            this.NumericDuration.TabIndex = 24;
            this.NumericDuration.Tag = "";
            this.NumericDuration.ThousandsSeparator = true;
            this.NumericDuration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // ButtonAttach
            // 
            this.ButtonAttach.Location = new System.Drawing.Point(192, 28);
            this.ButtonAttach.Name = "ButtonAttach";
            this.ButtonAttach.Size = new System.Drawing.Size(135, 28);
            this.ButtonAttach.TabIndex = 19;
            this.ButtonAttach.Text = "Attach License";
            this.ButtonAttach.UseVisualStyleBackColor = true;
            this.ButtonAttach.Click += new System.EventHandler(this.ButtonAttach_Click);
            // 
            // ButtonDetach
            // 
            this.ButtonDetach.Location = new System.Drawing.Point(323, 139);
            this.ButtonDetach.Name = "ButtonDetach";
            this.ButtonDetach.Size = new System.Drawing.Size(135, 28);
            this.ButtonDetach.TabIndex = 18;
            this.ButtonDetach.Text = "Detach License";
            this.ButtonDetach.UseVisualStyleBackColor = true;
            this.ButtonDetach.Click += new System.EventHandler(this.ButtonDetach_Click);
            // 
            // ComboRemoteDestination
            // 
            this.ComboRemoteDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboRemoteDestination.Enabled = false;
            this.ComboRemoteDestination.FormattingEnabled = true;
            this.ComboRemoteDestination.Location = new System.Drawing.Point(219, 78);
            this.ComboRemoteDestination.Name = "ComboRemoteDestination";
            this.ComboRemoteDestination.Size = new System.Drawing.Size(239, 21);
            this.ComboRemoteDestination.TabIndex = 17;
            // 
            // RadioRemote
            // 
            this.RadioRemote.AutoSize = true;
            this.RadioRemote.Location = new System.Drawing.Point(22, 92);
            this.RadioRemote.Name = "RadioRemote";
            this.RadioRemote.Size = new System.Drawing.Size(108, 17);
            this.RadioRemote.TabIndex = 16;
            this.RadioRemote.Text = "Remote recipient:";
            this.RadioRemote.UseVisualStyleBackColor = true;
            this.RadioRemote.CheckedChanged += new System.EventHandler(this.RadioRemote_CheckedChanged);
            // 
            // RadioLocal
            // 
            this.RadioLocal.AutoSize = true;
            this.RadioLocal.Checked = true;
            this.RadioLocal.Location = new System.Drawing.Point(22, 69);
            this.RadioLocal.Name = "RadioLocal";
            this.RadioLocal.Size = new System.Drawing.Size(188, 17);
            this.RadioLocal.TabIndex = 15;
            this.RadioLocal.TabStop = true;
            this.RadioLocal.Text = "Local recipient (test purposes only)";
            this.RadioLocal.UseVisualStyleBackColor = true;
            this.RadioLocal.CheckedChanged += new System.EventHandler(this.RadioLocal_CheckedChanged);
            // 
            // ComboProduct
            // 
            this.ComboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboProduct.FormattingEnabled = true;
            this.ComboProduct.Location = new System.Drawing.Point(219, 21);
            this.ComboProduct.Name = "ComboProduct";
            this.ComboProduct.Size = new System.Drawing.Size(239, 21);
            this.ComboProduct.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Select detachable Product:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RadioLocal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.RadioRemote);
            this.groupBox1.Controls.Add(this.NumericDuration);
            this.groupBox1.Controls.Add(this.ComboRemoteDestination);
            this.groupBox1.Controls.Add(this.ButtonDetach);
            this.groupBox1.Controls.Add(this.ComboProduct);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 190);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Host";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonAttach);
            this.groupBox2.Location = new System.Drawing.Point(12, 217);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(494, 83);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recipient";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 312);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Sentinel LDK Detach Product Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumericDuration)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenH2R;
        private System.Windows.Forms.SaveFileDialog SaveH2R;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NumericDuration;
        private System.Windows.Forms.Button ButtonAttach;
        private System.Windows.Forms.Button ButtonDetach;
        private System.Windows.Forms.ComboBox ComboRemoteDestination;
        private System.Windows.Forms.RadioButton RadioRemote;
        private System.Windows.Forms.RadioButton RadioLocal;
        private System.Windows.Forms.ComboBox ComboProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;

    }
}

