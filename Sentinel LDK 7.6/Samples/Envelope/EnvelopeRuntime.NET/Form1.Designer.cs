namespace Sample
{
    partial class SampleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.calcButton = new System.Windows.Forms.Button();
            this.alertAndRetryRadioButton = new System.Windows.Forms.RadioButton();
            this.retryRadioButton = new System.Windows.Forms.RadioButton();
            this.returnNothingRadioButton = new System.Windows.Forms.RadioButton();
            this.throwExceptionRadioButton = new System.Windows.Forms.RadioButton();
            this.infoRichTextBox = new System.Windows.Forms.RichTextBox();
            this.notificationDelegateCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.value1TextBox = new System.Windows.Forms.TextBox();
            this.value2TextBox = new System.Windows.Forms.TextBox();
            this.calcResultTextBox = new System.Windows.Forms.TextBox();
            this.equalSignLabel = new System.Windows.Forms.Label();
            this.operatorComboBox = new System.Windows.Forms.ComboBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // calcButton
            // 
            this.calcButton.Location = new System.Drawing.Point(388, 271);
            this.calcButton.Name = "calcButton";
            this.calcButton.Size = new System.Drawing.Size(98, 27);
            this.calcButton.TabIndex = 0;
            this.calcButton.Text = "&Calculate";
            this.calcButton.UseVisualStyleBackColor = true;
            this.calcButton.Click += new System.EventHandler(this.calcButton_Click);
            // 
            // alertAndRetryRadioButton
            // 
            this.alertAndRetryRadioButton.AutoSize = true;
            this.alertAndRetryRadioButton.Checked = true;
            this.alertAndRetryRadioButton.Location = new System.Drawing.Point(8, 18);
            this.alertAndRetryRadioButton.Name = "alertAndRetryRadioButton";
            this.alertAndRetryRadioButton.Size = new System.Drawing.Size(120, 17);
            this.alertAndRetryRadioButton.TabIndex = 6;
            this.alertAndRetryRadioButton.TabStop = true;
            this.alertAndRetryRadioButton.Text = "StatusAlertAndRetry";
            this.alertAndRetryRadioButton.UseVisualStyleBackColor = true;
            this.alertAndRetryRadioButton.CheckedChanged += new System.EventHandler(this.alertAndRetryRadioButton_CheckedChanged);
            // 
            // retryRadioButton
            // 
            this.retryRadioButton.AutoSize = true;
            this.retryRadioButton.Location = new System.Drawing.Point(8, 42);
            this.retryRadioButton.Name = "retryRadioButton";
            this.retryRadioButton.Size = new System.Drawing.Size(80, 17);
            this.retryRadioButton.TabIndex = 7;
            this.retryRadioButton.Text = "StatusRetry";
            this.retryRadioButton.UseVisualStyleBackColor = true;
            this.retryRadioButton.CheckedChanged += new System.EventHandler(this.retryRadioButton_CheckedChanged);
            // 
            // returnNothingRadioButton
            // 
            this.returnNothingRadioButton.AutoSize = true;
            this.returnNothingRadioButton.Location = new System.Drawing.Point(8, 65);
            this.returnNothingRadioButton.Name = "returnNothingRadioButton";
            this.returnNothingRadioButton.Size = new System.Drawing.Size(124, 17);
            this.returnNothingRadioButton.TabIndex = 8;
            this.returnNothingRadioButton.Text = "StatusReturnNothing";
            this.returnNothingRadioButton.UseVisualStyleBackColor = true;
            this.returnNothingRadioButton.CheckedChanged += new System.EventHandler(this.returnNothingRadioButton_CheckedChanged);
            // 
            // throwExceptionRadioButton
            // 
            this.throwExceptionRadioButton.AutoSize = true;
            this.throwExceptionRadioButton.Location = new System.Drawing.Point(8, 88);
            this.throwExceptionRadioButton.Name = "throwExceptionRadioButton";
            this.throwExceptionRadioButton.Size = new System.Drawing.Size(132, 17);
            this.throwExceptionRadioButton.TabIndex = 9;
            this.throwExceptionRadioButton.Text = "StatusThrowException";
            this.throwExceptionRadioButton.UseVisualStyleBackColor = true;
            this.throwExceptionRadioButton.CheckedChanged += new System.EventHandler(this.throwExceptionRadioButton_CheckedChanged);
            // 
            // infoRichTextBox
            // 
            this.infoRichTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.infoRichTextBox.Location = new System.Drawing.Point(147, 18);
            this.infoRichTextBox.Name = "infoRichTextBox";
            this.infoRichTextBox.ReadOnly = true;
            this.infoRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.infoRichTextBox.Size = new System.Drawing.Size(318, 87);
            this.infoRichTextBox.TabIndex = 5;
            this.infoRichTextBox.TabStop = false;
            this.infoRichTextBox.Text = "";
            // 
            // notificationDelegateCheckBox
            // 
            this.notificationDelegateCheckBox.AutoSize = true;
            this.notificationDelegateCheckBox.Checked = true;
            this.notificationDelegateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.notificationDelegateCheckBox.Location = new System.Drawing.Point(12, 111);
            this.notificationDelegateCheckBox.Name = "notificationDelegateCheckBox";
            this.notificationDelegateCheckBox.Size = new System.Drawing.Size(158, 17);
            this.notificationDelegateCheckBox.TabIndex = 4;
            this.notificationDelegateCheckBox.Text = "Enable NotificationDelegate";
            this.notificationDelegateCheckBox.UseVisualStyleBackColor = true;
            this.notificationDelegateCheckBox.CheckedChanged += new System.EventHandler(this.notificationDelegateCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.alertAndRetryRadioButton);
            this.groupBox1.Controls.Add(this.retryRadioButton);
            this.groupBox1.Controls.Add(this.returnNothingRadioButton);
            this.groupBox1.Controls.Add(this.throwExceptionRadioButton);
            this.groupBox1.Controls.Add(this.infoRichTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 115);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Return value";
            // 
            // value1TextBox
            // 
            this.value1TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.value1TextBox.Location = new System.Drawing.Point(45, 42);
            this.value1TextBox.Margin = new System.Windows.Forms.Padding(5);
            this.value1TextBox.MaxLength = 6;
            this.value1TextBox.Name = "value1TextBox";
            this.value1TextBox.Size = new System.Drawing.Size(86, 29);
            this.value1TextBox.TabIndex = 1;
            this.value1TextBox.Text = "3";
            this.value1TextBox.Leave += new System.EventHandler(this.valueTextBox_Leave);
            this.value1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueTextBox_KeyPress);
            // 
            // value2TextBox
            // 
            this.value2TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.value2TextBox.Location = new System.Drawing.Point(183, 42);
            this.value2TextBox.Margin = new System.Windows.Forms.Padding(5);
            this.value2TextBox.MaxLength = 6;
            this.value2TextBox.Name = "value2TextBox";
            this.value2TextBox.Size = new System.Drawing.Size(86, 29);
            this.value2TextBox.TabIndex = 3;
            this.value2TextBox.Text = "4";
            this.value2TextBox.Leave += new System.EventHandler(this.valueTextBox_Leave);
            this.value2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueTextBox_KeyPress);
            // 
            // calcResultTextBox
            // 
            this.calcResultTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calcResultTextBox.Location = new System.Drawing.Point(305, 42);
            this.calcResultTextBox.Name = "calcResultTextBox";
            this.calcResultTextBox.ReadOnly = true;
            this.calcResultTextBox.Size = new System.Drawing.Size(145, 29);
            this.calcResultTextBox.TabIndex = 11;
            this.calcResultTextBox.TabStop = false;
            // 
            // equalSignLabel
            // 
            this.equalSignLabel.AutoSize = true;
            this.equalSignLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equalSignLabel.Location = new System.Drawing.Point(277, 45);
            this.equalSignLabel.Name = "equalSignLabel";
            this.equalSignLabel.Size = new System.Drawing.Size(22, 24);
            this.equalSignLabel.TabIndex = 13;
            this.equalSignLabel.Text = "=";
            // 
            // operatorComboBox
            // 
            this.operatorComboBox.AllowDrop = true;
            this.operatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operatorComboBox.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.operatorComboBox.FormattingEnabled = true;
            this.operatorComboBox.Items.AddRange(new object[] {
            "+",
            "-",
            "×",
            "÷"});
            this.operatorComboBox.Location = new System.Drawing.Point(139, 41);
            this.operatorComboBox.Name = "operatorComboBox";
            this.operatorComboBox.Size = new System.Drawing.Size(36, 30);
            this.operatorComboBox.TabIndex = 2;
            this.operatorComboBox.Tag = "";
            this.operatorComboBox.SelectedIndexChanged += new System.EventHandler(this.operatorComboBox_SelectedIndexChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(9, 285);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(227, 13);
            this.infoLabel.TabIndex = 16;
            this.infoLabel.Text = "Calculator.Add is protected with Feature ID 51.";
            // 
            // SampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(498, 310);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.operatorComboBox);
            this.Controls.Add(this.equalSignLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.calcResultTextBox);
            this.Controls.Add(this.notificationDelegateCheckBox);
            this.Controls.Add(this.value2TextBox);
            this.Controls.Add(this.value1TextBox);
            this.Controls.Add(this.calcButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SampleForm";
            this.Text = ".NET EnvelopeRuntime Sample";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button calcButton;
        private System.Windows.Forms.RadioButton alertAndRetryRadioButton;
        private System.Windows.Forms.RadioButton retryRadioButton;
        private System.Windows.Forms.RadioButton returnNothingRadioButton;
        private System.Windows.Forms.RadioButton throwExceptionRadioButton;
        private System.Windows.Forms.RichTextBox infoRichTextBox;
        private System.Windows.Forms.CheckBox notificationDelegateCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox value1TextBox;
        private System.Windows.Forms.TextBox value2TextBox;
        private System.Windows.Forms.TextBox calcResultTextBox;
        private System.Windows.Forms.Label equalSignLabel;
        private System.Windows.Forms.ComboBox operatorComboBox;
        private System.Windows.Forms.Label infoLabel;
    }
}
