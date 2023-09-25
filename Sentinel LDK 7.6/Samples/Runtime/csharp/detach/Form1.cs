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

namespace detach_cs
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

        /// <summary>
        /// "Detach License" button
        /// </summary>
        private void ButtonDetach_Click(object sender, EventArgs e)
        {
            HaspStatus status;

            String info = null;
            String scope;
             
            if(RadioRemote.Checked)
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

            /// get selected detach recipients
            status = Hasp.GetInfo(scope, format, vendorCode, ref info);
            if(status!=HaspStatus.StatusOk)
                MessageBox.Show("Error while resolve selected destination ErrorCode :" + status.ToString());

            String h2r = null;
            String detach_recipient = info;

            String detach_action = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                   "<detach><duration>"+NumericDuration.Value+"</duration></detach>";
            
            if(ComboProduct.SelectedItem == null)
            {
                MessageBox.Show("Error. No item for detaching selected");
                return;
            }
            
            String detach_scope = "<haspscope><product id=\"" + 
                                  ((Product)ComboProduct.SelectedItem).getId() +
                                  "\" /></haspscope>";

            /// detach H2R license
            status = Hasp.Transfer(detach_action,
                                     detach_scope,
                                     vendorCode,
                                     detach_recipient,
                                 ref h2r);

           if(status!=HaspStatus.StatusOk)
               MessageBox.Show("Error while calling hasp_detach ErrorCode :" + status.ToString());
            else
            {
                if(SaveH2R.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                    /// write H2R data to file
                        StreamWriter writer = new StreamWriter(SaveH2R.FileName);
                    writer.Write(h2r);
                    writer.Close();
                }
            }
       }


        /// <summary>
        /// "Attach License" button
        /// </summary>
        private void ButtonAttach_Click(object sender, EventArgs e)
        {
            /// select H2R file which should be attached
            if(OpenH2R.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                /// read data from H2R file
                StreamReader reader = new StreamReader(OpenH2R.FileName);
                String h2r = reader.ReadToEnd();
                reader.Close();

                HaspStatus status;
                String acknowledge = null;

                /// update/attach retrieved data
                status = Hasp.Update(h2r, ref acknowledge);
                if(status!=HaspStatus.StatusOk)
                    MessageBox.Show("Error while attach license (hasp_update)ErrorCode :" + status.ToString());
            }
        }

        /// <summary>
        /// "Local recipient (test purposes only)" radio button
        /// </summary>
        private void RadioLocal_CheckedChanged(object sender, EventArgs e)
        {
            ComboRemoteDestination.Enabled = false;
            
            if(ComboProduct.DataSource != null)
                ButtonDetach.Enabled = true;
            else
                ButtonDetach.Enabled = false;
        }
        
        /// <summary>
        /// "Remote recipient" radio button
        /// </summary>
        private void RadioRemote_CheckedChanged(object sender, EventArgs e)
        {
            ComboRemoteDestination.Enabled = true;

            ArrayList destionations = new ArrayList();
            HaspStatus status;

            String info = null;
            String scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                           "<haspscope/>";

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
            status = Hasp.GetInfo(scope, format, vendorCode, ref info);
            if(status!=HaspStatus.StatusOk)
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
                    destionations.Add(node.InnerText);
                }
                    ComboRemoteDestination.DataSource = destionations;
             }

            if (ComboRemoteDestination.DataSource == null
                || ComboProduct.DataSource == null)
                ButtonDetach.Enabled = false;
            else
                ButtonDetach.Enabled = true;
        }


        /// <summary>
        /// load handler for main form 
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
                 ArrayList DetachableProducts = new ArrayList();

                 HaspStatus status;
                 String info = null;
                 String scope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                "<haspscope/>";

                 String format =  "<haspformat root=\"haspscope\">" +
                                  "    <product>" +
                                  "       <attribute name=\"id\" />" +
                                  "       <attribute name=\"name\" />" +
                                  "       <attribute name=\"detachable\" />" +
                                  "    </product>" +
                                  "</haspformat>";   

                 /// retrieve accessible Products
                 status = Hasp.GetInfo(scope, format, vendorCode, ref info);
                 if(status!=HaspStatus.StatusOk)
                     MessageBox.Show("Error while retrieving products ErrorCode :" + status.ToString());
                 else
                   {
                     XmlDocument document = new XmlDocument();
                     XmlNodeList nodeList;
                     XmlElement root;
                     document.LoadXml(info);
                     root = document.DocumentElement;
                     nodeList = root.SelectNodes("/haspscope/product");
                    
                     /// add each Product to DataSource
                     for (int iCtr = 0; iCtr < nodeList.Count; iCtr++) {
                         XmlAttributeCollection attributes = nodeList.Item(iCtr).Attributes;
                         if (attributes["detachable"].Value == "true")
                         {
                             DetachableProducts.Add(new Product(attributes["id"].Value, attributes["name"].Value));
                         }
                     }
                     ComboProduct.DataSource = DetachableProducts;
                 }

                 if (ComboProduct.DataSource == null)
                     ButtonDetach.Enabled = false;
                 else
                     ButtonDetach.Enabled = true;
        }
    }
}
