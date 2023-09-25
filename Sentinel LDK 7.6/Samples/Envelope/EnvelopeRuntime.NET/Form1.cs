using System;
using System.Windows.Forms;
using System.Diagnostics;

using Aladdin.HASP.Envelope;
using Aladdin.HASP.EnvelopeRuntime;

namespace Sample
{
    public partial class SampleForm : Form
    {
        private static EnvelopeRuntimeStatus m_Status = EnvelopeRuntimeStatus.StatusAlertAndRetry;

        private bool m_NotificationsEnabled = true;
        private NotificationDelegate m_NotificationDelegate = null;

        private Calculator m_Calc = null;

        /// <summary>
        /// This delegate is called when a protected function could not be invoked.
        /// </summary>
        /// <param name="haspStatus">The HASP status code tells why the invokation failed 
        ///   (i.e. HASP key not found, license not present, license expired, ...).</param>
        /// <returns>EnvelopeRuntimeStatus</returns>
        public static EnvelopeRuntimeStatus Handler(int haspStatus)
        {
            StackTrace st = new StackTrace();

            string messageStr = "HASP status " + haspStatus + ".\n\n";

            for (int i = 5; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                messageStr += sf.GetMethod() + "\n";

                // Only show a few entries from stack frame.
                if (i > 12) break;
            }

            messageStr += "\nClick OK to return with status ";

            // ToString() will not work - status names are obfuscated in protected assembly.
            switch (m_Status)
            {
                case EnvelopeRuntimeStatus.StatusAlertAndRetry:
                    messageStr += "StatusAlertAndRetry";
                    break;

                case EnvelopeRuntimeStatus.StatusRetry:
                    messageStr += "StatusRetry";
                    break;

                case EnvelopeRuntimeStatus.StatusReturnNothing:
                    messageStr += "StatusReturnNothing";
                    break;

                case EnvelopeRuntimeStatus.StatusThrowException:
                    messageStr += "StatusThrowException";
                    break;
            }

            messageStr += ".";

            MessageBox.Show(messageStr, "NotificationDelegate Handler");

            return m_Status;
        }

        public SampleForm()
        {
            InitializeComponent();
            m_NotificationDelegate = new NotificationDelegate(Handler);
            m_Calc = new Calculator();

            operatorComboBox.Text = "+";
            infoRichTextBox.Text = "NotificationDelegate handler will return StatusAlertAndRetry.\n\n" +
                "The EnvelopeRuntime will display an error message and retry to decrypt the method. " +
                "This is also the default behavior if no NotificationDelegate handler is installed.";
        }

        private void alertAndRetryRadioButton_CheckedChanged(object sender, EventArgs ea)
        {
            infoRichTextBox.Text = "NotificationDelegate handler will return StatusAlertAndRetry.\n\n" +
                "The EnvelopeRuntime will display an error message and retry to decrypt the method. " +
                "This is also the default behavior if no NotificationDelegate handler is installed.";

            m_Status = EnvelopeRuntimeStatus.StatusAlertAndRetry;
        }

        private void retryRadioButton_CheckedChanged(object sender, EventArgs ea)
        {
            infoRichTextBox.Text = "NotificationDelegate handler will return StatusRetry.\n\n" +
                "The EnvelopeRuntime will silently retry to decrypt the method.";

            m_Status = EnvelopeRuntimeStatus.StatusRetry;
        }

        private void returnNothingRadioButton_CheckedChanged(object sender, EventArgs ea)
        {
            infoRichTextBox.Text = "NotificationDelegate handler will return StatusReturnNothing.\n\n" +
                "The EnvelopeRuntime will let the protected method fail by returning null (Nothing in Visual Basic).";

            m_Status = EnvelopeRuntimeStatus.StatusReturnNothing;
        }

        private void throwExceptionRadioButton_CheckedChanged(object sender, EventArgs ea)
        {
            infoRichTextBox.Text = "NotificationDelegate handler will return StatusThrowException.\n\n" +
                "The EnvelopeRuntime will let the protected method to throw an exception, which needs to be " +
                "catched by the caller.";

            m_Status = EnvelopeRuntimeStatus.StatusThrowException;
        }

        private void calcButton_Click(object sender, EventArgs ea)
        {
            if (m_NotificationsEnabled)
                EnvelopeRuntimeEvent.Instance.Notification += m_NotificationDelegate;

            try
            {
                switch (operatorComboBox.Text)
                {
                    case "+":
                        calcResultTextBox.Text = m_Calc.Add(int.Parse(value1TextBox.Text), int.Parse(value2TextBox.Text)).ToString();
                        break;

                    case "-":
                        calcResultTextBox.Text = m_Calc.Subtract(int.Parse(value1TextBox.Text), int.Parse(value2TextBox.Text)).ToString();
                        break;

                    case "×":
                        calcResultTextBox.Text = m_Calc.Multiply(int.Parse(value1TextBox.Text), int.Parse(value2TextBox.Text)).ToString();
                        break;

                    case "÷":
                        calcResultTextBox.Text = m_Calc.Divide(int.Parse(value1TextBox.Text), int.Parse(value2TextBox.Text)).ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                calcResultTextBox.Text = "Error";
                MessageBox.Show(e.ToString());
            }

            if (m_NotificationsEnabled)
                EnvelopeRuntimeEvent.Instance.Notification -= m_NotificationDelegate;
        }

        private void notificationDelegateCheckBox_CheckedChanged(object sender, EventArgs ea)
        {
            m_NotificationsEnabled = notificationDelegateCheckBox.Checked;

            if (m_NotificationsEnabled == false)
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
            }
        }

        private void valueTextBox_KeyPress(object sender, KeyPressEventArgs ea)
        {
            if (!char.IsDigit(ea.KeyChar) &&
                !char.IsControl(ea.KeyChar))
                ea.Handled = true;
        }

        private void valueTextBox_Leave(object sender, EventArgs ea)
        {
            try
            {
                ((TextBox)sender).Text = int.Parse(((TextBox)sender).Text).ToString();
            }
            catch
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void operatorComboBox_SelectedIndexChanged(object sender, EventArgs ea)
        {
            switch (operatorComboBox.Text)
            {
                case "+":
                    infoLabel.Text = "Calculator.Add is protected with Feature ID 51.";
                    break;

                case "-":
                    infoLabel.Text = "Calculator.Subtract is protected with Feature ID 52.";
                    break;

                case "×":
                    infoLabel.Text = "Calculator.Multiply is protected with Feature ID 53.";
                    break;

                case "÷":
                    infoLabel.Text = "Calculator.Divide is protected with Feature ID 54, once per application.";
                    break;
            }
        }
    }
}
