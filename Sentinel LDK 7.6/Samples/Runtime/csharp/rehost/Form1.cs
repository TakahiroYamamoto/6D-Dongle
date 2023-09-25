using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Aladdin.HASP;

namespace hasp_rehost
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Sample Vendor Code
        /// </summary>
        private String vendorCode =
             "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsV" +
             "vIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrB" +
             "rh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6" +
             "AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf" +
             "8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1" +
             "e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBh" +
             "aJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SK" +
             "unFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bL" +
             "Cx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwL" +
             "zaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9s" +
             "JN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaT" +
             "tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y";

        /// <summary>
        /// The form's constructor.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ArrayList RehostHaspId = new ArrayList();

            HaspStatus status;

            String info = null;

            /// retrieve accessible Sentinel key Ids
            status = Hasp.GetInfo("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                   "<haspscope>" +
                                        "<hasp type=\"HASP-SL-AdminMode\" />" +
                                        "<hasp type=\"HASP-SL-UserMode\" />" +
                                   "</haspscope>",
                                    "<haspformat root=\"haspscope\">" +
                                    "       <hasp>" +
                                    "       <attribute name=\"id\" />" +
                                    "       <attribute name=\"rehost\" />" +
                                    "       <attribute name=\"type\" />"+
                                    "       </hasp>"+
                                    "</haspformat>",
                                    vendorCode,
                                    ref info);
            if (status != HaspStatus.StatusOk)
                MessageBox.Show("Error while retrieving Sentinel key Ids ErrorCode :" + status.ToString());
            else
            {
                XmlDocument document = new XmlDocument();
                XmlNodeList nodeList;
                XmlElement root;
                document.LoadXml(info);
                root = document.DocumentElement;
                nodeList = root.SelectNodes("/haspscope/hasp");

                /// add each Hasp id  to DataSource
                for (int iCtr = 0; iCtr < nodeList.Count; iCtr++)
                {
                    XmlAttributeCollection attributes = nodeList.Item(iCtr).Attributes;
                    if (attributes["rehost_enduser_managed"].Value == "true")
                    {
                        RehostHaspId.Add(new HaspId(attributes["id"].Value + " (" + attributes["type"].Value + ") "));
                    }
                }
                if (RehostHaspId.Count > 0)
                    ComboHaspId.DataSource = RehostHaspId;
            }

            if (ComboHaspId.DataSource == null)
                ButtonRehost.Enabled = false;
            else
                ButtonRehost.Enabled = true;
        }

        private void ButtonRehost_Click(object sender, EventArgs e)
        {
            HaspStatus status;
            String info = null;
            String scope;

            if (RadioRemote.Checked && RadioRemote.Enabled ==true)
                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<haspscope><license_manager hostname=\""
                        + ComboRemoteDestination.Text
                        + "\" /></haspscope>";
            else
                scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                        "<haspscope>" +
                        "    <license_manager hostname =\"localhost\" />" +
                        "</haspscope>";

            String format = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                            "<haspformat root=\"location\">" +
                            "   <license_manager>" +
                            "      <attribute name=\"id\" />" +
                            "      <attribute name=\"time\" />" +
                            "      <element name=\"hostname\" />" +
                            "      <element name=\"version\" />" +
                            "      <element name=\"host_fingerprint\" />" +
                            "   </license_manager>" +
                            "</haspformat>";

            /// get selected rehost recipients
            status = Hasp.GetInfo(scope, format, vendorCode, ref info);
            if (status != HaspStatus.StatusOk)
                MessageBox.Show("Error while resolve selected destinations ErrorCode :" + status.ToString());

            String rehost_output = null;
            String rehost_recipient = info;

            /// rehost V2C license
            status = Hasp.Transfer("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                   "<rehost>"+
                                        "<hasp id=\""+((HaspId)ComboHaspId.SelectedItem).getId()+"\"/>" +
                                   "</rehost>",
                                 "<haspscope><hasp id=\"" +
                                  ((HaspId)ComboHaspId.SelectedItem).getId() +
                                  "\" /></haspscope>",
                                 vendorCode,
                                 rehost_recipient,
                                 ref rehost_output);

            if (status != HaspStatus.StatusOk)
                MessageBox.Show("Error while calling hasp_transfer for rehost ErrorCode :" + status.ToString());
            else
            {
                if (SaveH2H.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    /// write H2H data to file
                    StreamWriter writer = new StreamWriter(SaveH2H.FileName);
                    writer.Write(rehost_output);
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// "Local recipient (test purposes only)" radio button
        /// </summary>
        private void RadioLocal_CheckedChanged(object sender, EventArgs e)
        {
            ComboRemoteDestination.Enabled = false;

            if (ComboHaspId.DataSource != null)
                ButtonRehost.Enabled = true;
            else
                ButtonRehost.Enabled = false;
        }


        /// <summary>
        /// "Remote recipient" radio button
        /// </summary>
        /// 


        private void ButtonAttach_Click(object sender, EventArgs e)
        {
            /// select V2C file which should be attached
            if (OpenH2H.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                /// read data from H2H file
                StreamReader reader = new StreamReader(OpenH2H.FileName);
                String v2c = reader.ReadToEnd();
                reader.Close();

                HaspStatus status;
                String acknowledge = null;

                /// update/attach retrieved data
                status = Hasp.Update(v2c, ref acknowledge);
                if (status != HaspStatus.StatusOk)
                    MessageBox.Show("Error while attach license (hasp_update)ErrorCode :" + status.ToString());
            }
        }

        private void RadioRemote_CheckedChanged_1(object sender, EventArgs e)
        {
            ComboRemoteDestination.Enabled = true;

            ArrayList destinations = new ArrayList();
            HaspStatus status;
            String info = null;


            String format = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                    "<haspformat root=\"location\">" +
                                    "   <license_manager>" +
                                    "      <attribute name=\"id\" />" +
                                    "      <attribute name=\"time\" />" +
                                    "      <element name=\"hostname\" />" +
                                    "      <element name=\"version\" />" +
                                    "      <element name=\"host_fingerprint\" />" +
                                    "   </license_manager>" +
                                    "</haspformat>";

            /// retrieve XML list of accessible recipients
            status = Hasp.GetInfo("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                   "<haspscope/>",
                                   format,
                                   vendorCode,
                                   ref info);
            if (status != HaspStatus.StatusOk)
                MessageBox.Show("Error while getting destinations ErrorCode :" + status.ToString());
            else
            {
                XmlDocument document = new XmlDocument();
                XmlNodeList nodeList;
                XmlElement root;
                document.LoadXml(info);
                root = document.DocumentElement;
                nodeList = root.SelectNodes("/location/license_manager/hostname");

                /// extract hostname for each recipient in the XML result
                for (int iCtr = 0; iCtr < nodeList.Count; iCtr++)
                {
                    XmlNode node = nodeList.Item(iCtr);
                    destinations.Add(node.InnerText);
                }
                if (destinations.Count > 0)
                    ComboRemoteDestination.DataSource = destinations;
            }

            if (ComboRemoteDestination.DataSource == null
                || ComboHaspId.DataSource == null)
                ButtonRehost.Enabled = false;
            else
                ButtonRehost.Enabled = true;
        }

        private void ComboHaspId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string slUsermode = "HASP-SL-UserMode";
            
            if (ComboHaspId.SelectedValue.ToString().IndexOf(slUsermode) == -1)
            {
                RadioRemote.Enabled = true;
                ComboRemoteDestination.Enabled = true;
            }
            else
            {
                RadioRemote.Enabled = false;
                ComboRemoteDestination.Enabled = false;
            }

                
        }
    }
}
