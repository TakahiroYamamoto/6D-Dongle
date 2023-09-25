////////////////////////////////////////////////////////////////////
// Copyright (C) 2014, SafeNet, Inc. All rights reserved.
//
//
//
//
////////////////////////////////////////////////////////////////////
using System;
using Aladdin.HASP;


namespace HaspDemo
{
    /// <summary>
    /// </summary>
    public class HaspDemoV2c : HaspDemo
    {
        /// <summary>
        /// Constructor.
        /// Initializes the object
        /// </summary>
        public HaspDemoV2c(System.Windows.Forms.TextBox textHistory)
            : base(textHistory)
        {
        }


        /// <summary>
        /// Prints the footer message into the
        /// referenced TextBox
        /// </summary>
        protected override void Footer()
        {
            FormHaspDemo.WriteToTextbox(textHistory, "Sentinel LDK Update completed\r\n");
        }


        /// <summary>
        /// Prints the header message into the
        /// referenced TextBox
        /// </summary>
        protected override void Header()
        {
            if (textHistory != null && 0xFFFF < textHistory.TextLength)
                textHistory.Clear();

            FormHaspDemo.WriteToTextbox(textHistory,
                "____________________________________________________________\r\n" +
                string.Format("Sentinel LDK Update started ({0})\r\n\r\n",
                    DateTime.Now.ToString()));
        }


        /// <summary>
        /// Updates the key using the passed update string
        /// and writes the returned acknowledge (if available)
        /// into the referenced TextBox.
        /// </summary>
        public new void RunDemo(string update)
        {
            try
            {
                Header();

                // print the update string
                Verbose("Update information:");
                Verbose(update.Replace("\n", "\r\n     "));
                Verbose("");

                string ack = null;

                // perform the update
                // please note that the Hasp's Update method is
                // static.
                HaspStatus status = Hasp.Update(update, ref ack);
                ReportStatus(status);

                if (HaspStatus.StatusOk == status)
                {
                    // print the acknowledgement
                    // the return of an ack. is controlled via the
                    // update package.
                    Verbose("Acknowledge information:");
                    Verbose(((null == ack) || (0 == ack.Length)) ? "Not available" :
                                                            ack.Replace("\n", "\r\n     "));
                }
                Verbose("");
                Footer();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,
                                                     "Exception",
                                                      System.Windows.Forms.MessageBoxButtons.OK);
            }

        }
    }
}
