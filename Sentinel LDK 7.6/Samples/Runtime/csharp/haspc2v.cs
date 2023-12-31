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
    public class HaspDemoC2v : HaspDemo
    {
        /// <summary>
        /// Constructor.
        /// Initializes the object
        /// </summary>
        public HaspDemoC2v(System.Windows.Forms.TextBox textHistory)
            : base(textHistory)
        {
        }


        /// <summary>
        /// Prints the footer message into the
        /// referenced TextBox
        /// </summary>
        protected override void Footer()
        {
            FormHaspDemo.WriteToTextbox(textHistory, "Generation of Status Information completed\r\n");
        }


        /// <summary>
        /// Prints the header into the
        /// referenced TextBox
        /// </summary>
        protected override void Header()
        {
            if (textHistory != null && 0xFFFF < textHistory.TextLength)
                textHistory.Clear();

            FormHaspDemo.WriteToTextbox(textHistory,
                "____________________________________________________________\r\n"+
                string.Format("Generation of Status Information started ({0})\r\n\r\n",
                                              DateTime.Now.ToString()));
        }


        /// <summary>
        /// Retrieves the update information
        /// without logging in using
        /// the Hasp's GetInfo method.
        /// </summary>
        public string RunDemo()
        {
            string info = "";
            try
            {
                Header();

                Verbose("Retrieving Update Information");

                // now get the update information
                Verbose("Retrieving Information");
                HaspStatus status = Hasp.GetInfo(localScope, Hasp.UpdateInfo, VendorCode.Code, ref info);
                ReportStatus(status);

                if (HaspStatus.StatusOk == status)
                    Verbose(info.Replace("\n", "\r\n     "));
                else
                    Verbose("");

                Verbose("");
                Footer();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,
                                                     "Exception",
                                                      System.Windows.Forms.MessageBoxButtons.OK);
            }
            return info;
        }
    }
}
