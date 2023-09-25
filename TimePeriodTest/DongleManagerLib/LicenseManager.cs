using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;

using Aladdin.HASP;
using Sentinel.Ldk.LicGen;

using DongleManagerLib;
using DongleToolLib;

namespace DongleManagerLib
{
    public class LicenseManager : LicenseMiniManager
    {
        public InitialSettingXML setting = new InitialSettingXML();

        public string version = "";

        public static LicGenAPIHelper licGenHelp = new LicGenAPIHelper();

        // 注意.
        // ProductやFeatureに名前を設定してもドングルには書き込まれない

        public static string license_all_fmt =
            "<?xml version = \"1.0\" encoding=\"utf-8\"?>\r\n"
            + " <sentinel_ldk:license schema_version = \"1.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:sentinel_ldk=\"http://www.safenet-inc.com/sentinelldk\">\r\n"
            + "   <enforcement_type>HL</enforcement_type>\r\n"
            + "{0}"                 // product挿入
            + "</sentinel_ldk:license>\r\n";
        public static string product_fmt =
            "    <product>\r\n"
            + "      <id>{0}</id>\r\n"
            + "      <name>{1}</name>\r\n"
            + "      <use_vclock>Yes</use_vclock>\r\n"
            + "{2}"                   // feature挿入
            + "    </product>\r\n";
        public static string feature_fmt_single =
            "      <feature>\r\n"
            + "        <id>{0}</id>\r\n"
            + "        <name>{1}</name>\r\n"
            + "        <license_properties>\r\n"
            + "          {2}"                       // 期限設定
            + "          <remote_desktop_access>{3}</remote_desktop_access>\r\n"
            + "        </license_properties>\r\n"
            + "      </feature>\r\n";

        public static string feature_fmt_concurrency =
            "      <feature>\r\n"
            + "        <id>{0}</id>\r\n"
            + "        <name>{1}</name>\r\n"
            + "        <license_properties>\r\n"
            + "          {2}"                       // 期限設定
            + "          <concurrency>\r\n"
            + "            <count>{3}</count>\r\n"
            + "            <count_criteria>{4}</count_criteria>\r\n"
            + "            <network_access>{5}</network_access>\r\n"
            + "          </concurrency>\r\n"
            + "          <remote_desktop_access>{6}</remote_desktop_access>\r\n"
            + "        </license_properties>\r\n"
            + "      </feature>\r\n";
        public static string feature_concurrency_unlimited = "Unlimited";
        public static string[] feature_concurrency_count_criteria = { "Per Process", "Per Station", "Per Login" };
        public static string[] yes_no = { "Yes", "No" };

        // 期限用フォーマット
        public static string period_perpetual_fmt = "<perpetual/>\r\n";
        public static string period_expiration_date_fmt =
            "           <expiration_date>{0:yyyy-MM-dd}</expiration_date>\r\n";
        public static string period_days_to_expiration_fmt =
            "           <days_to_expiration>{0}</days_to_expiration>\r\n";


        public int LoadInitialSetting(out string errmsg)
        {
            if (LoadInitialLicenseDefinition(out errmsg) < 0)
            {
                MessageBox.Show(errmsg);
            }
            {
                InitialSettingXML new_setting = InitialSettingXML.Load(out errmsg);
                if (new_setting != null)
                    setting = new_setting;
            }
            return 0;
        }

        public int LoadInitialLicenseDefinition(out string errmsg)
        {
            string xmlFile = GetInitialLicenseDefinitionFile(false, out errmsg);
            if (xmlFile == null)
                return -1;
            LicenseXMLData xmlData = LicenseXMLData.LoadDefFile(xmlFile, out errmsg);
            if (xmlData == null)
                return -1;

            List<ProductItem> tmp_products = new List<ProductItem>();
            List<FeatureItem> tmp_features = new List<FeatureItem>();

            for (int i = 0; i < xmlData.features.Count; i++)
            {
                FeatureItem fItem = xmlData.features[i];
                tmp_features.Add(fItem);
            }
            for (int i = 0; i < xmlData.products.Count; i++)
            {
                ProductXMLItem xml_pItem = xmlData.products[i];

                if (xml_pItem.valid_for_dongle)
                {
                    ProductItem pItem = new ProductItem();
                    for (int j = 0; j < xml_pItem.featureIdxs.Count; j++)
                    {
                        int idx = xml_pItem.featureIdxs[j];
                        if (tmp_features[idx].valid_for_dongle)
                        {
                            pItem.featureIdxs.Add(idx);
#if false
                            FeatureItem fItem = tmp_features[idx];
                            if (pItem.featuresStr != "")
                                pItem.featuresStr += ",";
                            pItem.featuresStr += string.Format("{0}({1})", fItem.name, fItem.id); 
#endif
                        }
                    }
                    pItem.id = xml_pItem.id;
                    pItem.name = xml_pItem.name;
                    tmp_products.Add(pItem);
                }
            }
            products = tmp_products;
            features = tmp_features;
            version = xmlData.version;
            return 0;
        }

        int FindFeatureIdx(List<FeatureItem> features, int id)
        {
            for (int i = 0; i < features.Count; i++)
            {
                if (id == features[i].id)
                    return i;
            }
            return -1;
        }

        public int UpdateLicenseDef(string xmlFile, out string errmsg)
        {
            LicenseXMLData xmlData = LicenseXMLData.LoadDefFile(xmlFile, out errmsg);
            if (xmlData == null)
                return -1;
            string initFile = GetLicenseDefinitionUpdateFile(true, out errmsg);
            if (initFile == null)
                return -1;
            System.IO.File.Copy(xmlFile, initFile, true);
            return LoadInitialSetting(out errmsg);
        }

        public List<int> MargeProductIdxs(List<int> curr, List<int> add, out string errmsg)
        {
            List<int> idxs = new List<int>();
            errmsg = "";
            foreach (int idx in curr)
            {
                idxs.Add(idx);
            }

            foreach (int addIdx in add)
            {
                bool isExists = false;
                foreach (int currIdx in curr)
                {
                    if (addIdx == currIdx)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists)
                    continue;
                idxs.Add(addIdx);
            }
            return idxs;
        }
        public string CreateV2C(string dongleId, string c2vData, List<int> productIdxs, out string errmsg)
        {
            errmsg = "";
            string v2cData = "";
            string license_definition = "";

            // set license_definition
            {
                string productStr = "";
                foreach (int pIdx in productIdxs)
                {
                    ProductItem pItem = products[pIdx];

                    string featureStr = "";
                    foreach (int fIdx in pItem.featureIdxs)
                    {
                        FeatureItem fItem = features[fIdx];
                        string featureOne;
                        string period_str;
                        {
                            if (fItem.period_kind == Enum_Period.Date)
                                period_str = string.Format(period_expiration_date_fmt, fItem.period_date);
                            else if (fItem.period_kind == Enum_Period.Days)
                                period_str = string.Format(period_days_to_expiration_fmt, fItem.period_days);
                            else
                                period_str = period_perpetual_fmt;
                        }
                        if (fItem.concurrency)
                        {
                            featureOne = string.Format(feature_fmt_concurrency,
                                fItem.id, //id
                                fItem.name,
                                period_str,
                                fItem.concurrency_count == 0 ? feature_concurrency_unlimited : fItem.concurrency_count.ToString(), // count
                                feature_concurrency_count_criteria[(int)fItem.concurrency_count_criteria], // count_criteria
                                yes_no[fItem.concurrency_network_access ? 0 : 1], // network_access
                                yes_no[fItem.remote_desktop_access ? 0 : 1] // remote_desktop_access
                                );
                        }
                        else
                        {
                            featureOne = string.Format(feature_fmt_single,
                                fItem.id, // id
                                fItem.name,
                                period_str,
                                yes_no[fItem.remote_desktop_access ? 0 : 1] // remote_desktop_access
                                );
                        }
                        featureStr += featureOne;
                    }
                    string productOne = string.Format(product_fmt, pItem.id, pItem.name, featureStr);
                    productStr += productOne;
                }
                license_definition = string.Format(license_all_fmt, productStr);
            }

            string initParam = null;
            LicenseManager.licGenHelp.sntl_lg_initialize(initParam);

            try
            {
                sntl_lg_status_t sntl_status = LicenseManager.licGenHelp.sntl_lg_start(null, vendor_code,
                     sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_FORMAT_AND_UPDATE, license_definition, c2vData);

                if (sntl_status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
                {
                    errmsg = string.Format("Cannot define license for dongle=#{0} [status={1}]", dongleId, sntl_status);
                    return null;
                }
                string resultStr = "";
                sntl_status = licGenHelp.sntl_lg_generate_license(null, ref v2cData, ref resultStr);
                if (sntl_status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
                {
                    if (sntl_status == sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE)
                        v2cData = "";
                    else
                    {
                        errmsg = string.Format("Cannot genarate v2c for dongle=#{0} [status={1}]", dongleId, sntl_status);
                        return null;
                    }
                }
            }
            finally
            {
                LicenseManager.licGenHelp.sntl_lg_cleanup();
            }
            return v2cData;
        }

        public int SetLogPath(string path, out string errmsg)
        {
            setting.logPath = path;
            if (setting.Save(out errmsg) < 0)
                return -1;
            return 0;
        }

        public string GetLogPath()
        {
            return setting.logPath;
        }

        public int SetC2VPath(string path, out string errmsg)
        {
            setting.c2vPath = path;
            if (setting.Save(out errmsg) < 0)
                return -1;
            return 0;
        }

        public string GetC2VPath()
        {
            return setting.c2vPath;
        }

        public int SetV2CPath(string path, out string errmsg)
        {
            setting.v2cPath = path;
            if (setting.Save(out errmsg) < 0)
                return -1;
            return 0;
        }

        public string GetV2CPath()
        {
            return setting.v2cPath;
        }

        // productIdxs==nullはV2Cから焼いたログを示す
        public int SaveLog(List<DongleListItem> dongleItems, List<int> productIdxs, string processStr, out string errmsg)
        {
            errmsg = "";
            string path = GetLogPath();
            if (path == "")
            {
                errmsg = "Path for log file is not set";
                return -1;
            }

            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    errmsg = string.Format("Cannot create folder for logfile:({0})", path);
                    return -1;
                }
            }
            string logFile = GetLogFileName();
            string fullPath = path + @"\" + logFile;

            string productsStr = "";

            if (productIdxs != null)
            {
                foreach (int pIdx in productIdxs)
                {
                    ProductItem pItem = products[pIdx];
                    string featuresStr = "";
                    foreach (int fIdx in pItem.featureIdxs)
                    {
                        FeatureItem fItem = features[fIdx];
                        string featureOne =
                            string.Format(
                                "{0}" // feature name
                                + "({1})" // id
                                , fItem.name, fItem.id
                                );
                        if (featuresStr != "")
                            featuresStr += ",";
                        featuresStr += featureOne;
                    }

                    string productOne =
                        string.Format(
                            "{0}" // product name
                            + "({1})" // id
                            + "[{2}]" // features
                            , pItem.name, pItem.id, featuresStr);
                    if (productsStr != "")
                        productsStr += "/";
                    productsStr += productOne;
                }
            }
            else
            { // V2Cから焼いた
                productsStr = null;
            }

            try
            {
                // 同じ日のログファイルには追加で記録する
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fullPath, true, new System.Text.UTF8Encoding(false));

                try
                {
                    foreach (DongleListItem item in dongleItems)
                    {
                        string date = string.Format("{0:D4}/{1:D2}/{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        string line;
                        if (productsStr != null)
                        {
                            line = string.Format(
                                "{0}" // date
                                + "\t{1}" // processStr
                                + "\t{2}" // update or same
                                + "\t{3}" // definition version
                                + "\t{4}" // dongleId
                                + "\t{5}" // products
                                , date,
                                processStr,
                                item.updated ? "update" : "SAME",
                                version,
                                item.dongleId,
                                productsStr == "" ? "EMPTY" : productsStr);
                        }
                        else
                        { // V2Cから焼いた
                            line = string.Format(
                                "{0}" // date
                                + "\t{1}" // processStr
                                + "\t{2}" // update or same
                                + "\t{3}" // definition version
                                + "\t{4}" // dongleId
                                + "\t{5}" // products
                                , date,
                                processStr,
                                item.updated ? "update" : "SAME", // YET
                                "unknown",
                                item.v2c_filename,
                                "unknown");
                        }
                        sw.WriteLine(line);
                    }
                }
                finally
                {
                    sw.Close();
                }
            }
            catch (Exception)
            {
                errmsg = string.Format("Fail to save log:({0})", fullPath);
                return -1;
            }
            return 0;
        }

        //------------------------------- static -----------------------------

        public static string GetLicenseDefinitionUpdateFile(bool createFolder, out string errmsg)
        {
            string path = GetSettingFolder(createFolder, out errmsg);
            if (path == null)
                return null;
            return string.Format("{0}\\DongleLicenseDef.txt", path);
        }

        public static string GetInitialLicenseDefinitionFile(bool createFolder, out string errmsg)
        {
            // アップデートされたファイルを探す
            string xmlFile = GetLicenseDefinitionUpdateFile(createFolder, out errmsg);
            if (xmlFile == null)
                return null;

            if (System.IO.File.Exists(xmlFile))
                return xmlFile;

            {
                string exePath = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                string[] files = System.IO.Directory.GetFiles(exePath, "DongleLicenseDef.*.txt", System.IO.SearchOption.TopDirectoryOnly);
                if (files.Length == 0)
                {
                    errmsg = "No license definition file";
                    return null;
                }
                Array.Sort(files, StringComparer.OrdinalIgnoreCase);
                xmlFile = files[files.Length - 1];
            }
            return xmlFile;
        }

        public static string GetDefaultLogFilePath()
        {
            return string.Format(@"{0}\Photron\DongleLicense\Log",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
        }

        public static string GetLogFileName()
        {
            return string.Format("log_{0:D4}{1:D2}{2:D2}.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public static string GetC2VInfoFromFile(string dongleId, string c2vData, out string errmsg)
        {
            errmsg = "";
            string initParam = null;
            LicenseManager.licGenHelp.sntl_lg_initialize(initParam);

            string licInfo = "";
            sntl_lg_status_t sntl_status = LicenseManager.licGenHelp.sntl_lg_decode_current_state(
                LicenseManager.vendor_code, c2vData, ref licInfo);

            LicenseManager.licGenHelp.sntl_lg_cleanup();

            if (sntl_status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
            {
                errmsg = string.Format("Cannot access to dongle #{0} [status={1}]", dongleId, sntl_status);
                return null;
            }
            return licInfo;
        }

        public static string GetC2VInfoFromDongle(string id, out string errmsg)
        {
            string c2vData = GetDongleC2V(id, out errmsg);
            if (c2vData == null)
                return null;
            return GetC2VInfoFromFile(id, c2vData, out errmsg);
        }

        public static string GetDongleIdFromC2V(string c2vData, out string errmsg)
        {
            errmsg = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(c2vData);
            XmlElement elem = xmlDoc.DocumentElement;
            if (elem.LocalName != "hasp_info")
            {
                errmsg = "no <hasp_info> tag";
                return null;
            }
            for (XmlNode haspNode = elem.FirstChild; haspNode != null; haspNode = haspNode.NextSibling)
            {
                if (haspNode.LocalName != "haspscope")
                    continue;
                for (XmlNode node1 = haspNode.FirstChild; node1 != null; node1 = node1.NextSibling)
                {
                    if (node1.LocalName != "hasp")
                        continue;
                    string vendorId = "";
                    for (XmlNode node2 = node1.FirstChild; node2 != null; node2 = node1.NextSibling)
                    {
                        if (node2.LocalName != "vendor")
                            continue;
                        for (int i = 0; i < node2.Attributes.Count; i++)
                        {
                            XmlAttribute att = node2.Attributes[i];
                            if (att.LocalName == "id")
                            {
                                vendorId = att.Value;
                            }
                        }
                    }
                    if (LicenseMiniManager.isDemoMode)
                    {
                        if (vendorId != "37515")
                        {
                            errmsg = "dongle is not for DEMO";
                            return null;
                        }
                    }
                    else
                    {
                        if (vendorId != "114623")
                        {
                            errmsg = "dongle is not for Photron";
                            return null;
                        }
                    }
                    for (int i = 0; i < node1.Attributes.Count; i++)
                    {
                        XmlAttribute dongleId_att = node1.Attributes[i];
                        if (dongleId_att.LocalName == "id")
                            return dongleId_att.Value;
                    }
                }
            }
            errmsg = "no dongle id in c2v";
            return null;
        }

        public static string GetSettingFolder(bool createFolder, out string errmsg)
        {
            errmsg = "";
            string path = string.Format(@"{0}\Photron\DongleLicense",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            if (!System.IO.Directory.Exists(path))
            {
                if (createFolder)
                    System.IO.Directory.CreateDirectory(path);
                else
                {
                    errmsg = string.Format("Cannot create folder({0})", path);
                    return null;
                }
            }
            return path;
        }

        public static bool GetDongleInfo(List<DongleListItem> dongleItems, out string errmsg)
        {
            errmsg = "";

            foreach (DongleListItem dongleItem in dongleItems)
            {
                string licInfo;
                if (dongleItem.c2vData != null)
                    licInfo = LicenseManager.GetC2VInfoFromFile(dongleItem.dongleId, dongleItem.c2vData, out errmsg);
                else
                    licInfo = LicenseManager.GetC2VInfoFromDongle(dongleItem.dongleId, out errmsg);
                if (licInfo == null)
                    return false;

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(licInfo);
                XmlElement elem = xmlDoc.DocumentElement;
                if (elem.LocalName != "sentinel_ldk_info")
                    continue;

                for (XmlNode infoNode = elem.FirstChild; infoNode != null; infoNode = infoNode.NextSibling)
                {
                    if (infoNode.LocalName != "key")
                        continue;
                    for (XmlNode keyNode = infoNode.FirstChild; keyNode != null; keyNode = keyNode.NextSibling)
                    {
                        if (keyNode.LocalName != "product")
                            continue;
                        for (XmlNode productNode = keyNode.FirstChild; productNode != null; productNode = productNode.NextSibling)
                        {
                            if( productNode.LocalName == "id")
                            {
                                dongleItem.productIds.Add(Convert.ToInt32(productNode.InnerText));
                                continue;
                            }
                            else if (productNode.LocalName != "feature")
                                continue;

                            int fid = -1;
                            Enum_Period period_kind = Enum_Period.None;
                            int period_days = -1;
                            DateTime period_date = DateTime.MinValue;
                            DateTime period_days_end = DateTime.MinValue;
                            for (XmlNode featureNode = productNode.FirstChild; featureNode != null; featureNode = featureNode.NextSibling)
                            {
                                if (featureNode.LocalName == "id")
                                {
                                    fid = int.Parse(featureNode.InnerText);
                                    continue;
                                }
                                if (featureNode.LocalName == "license_properties")
                                {
                                    for (XmlNode lic_propNode = featureNode.FirstChild; lic_propNode != null; lic_propNode = lic_propNode.NextSibling)
                                    {
                                        if (lic_propNode.LocalName == "perpetual")
                                            break;
                                        if (lic_propNode.LocalName == "expiration_date")
                                        {
                                            period_kind = Enum_Period.Date;
                                            period_date = DateTime.ParseExact(lic_propNode.InnerText, "yyyy-MM-dd", null);
                                            break;
                                        }
                                        if (lic_propNode.LocalName == "days_to_expiration")
                                        {
                                            period_kind = Enum_Period.Days;
                                            period_days = int.Parse(lic_propNode.InnerText);
                                        }
                                        else if (lic_propNode.LocalName == "usage_end_date")
                                        {
                                            DateTime.TryParseExact(
                                                lic_propNode.InnerText, "yyyy-MM-dd",
                                                CultureInfo.CurrentCulture, DateTimeStyles.None,
                                                out period_days_end);
                                        }
                                    }
                                }
                            }
                            if (fid < 0)
                                continue;

                            bool isAlreadyExists = false;
                            foreach (FeatureIdAndPeriodInDognle fItem in dongleItem.featureIds)
                            {
                                if (fItem.id == fid)
                                {
                                    isAlreadyExists = true;
                                    break;
                                }
                            }
                            if (!isAlreadyExists)
                            {
                                FeatureIdAndPeriodInDognle fItem = new FeatureIdAndPeriodInDognle(fid);
                                fItem.period_kind = period_kind;
                                fItem.period_date = period_date;
                                fItem.period_days = period_days;
                                fItem.period_days_end = period_days_end;
                                dongleItem.featureIds.Add(fItem);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

    public class StringUtil
    {
        public static int MatchIndex(string str, string [] str_array)
        {
            for( int i = 0; i < str_array.Length; i++)
            {
                if (str == str_array[i])
                    return i;
            }
            return -1;
        }
    }

    public class InitialSettingXML
    {
        public string logPath = "";
        public string c2vPath = "";
        public string v2cPath = "";

        public int Save(out string errmsg)
        {
            string filename = GetFilename(out errmsg);
            if (filename == null)
                return -1;

            System.Xml.Serialization.XmlSerializer seri = null;
            try
            {
                seri = new System.Xml.Serialization.XmlSerializer(typeof(InitialSettingXML));
            }
            catch (Exception)
            {
                errmsg = "Cannot load setting file";
                return -1;
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, false, new System.Text.UTF8Encoding(false));
            seri.Serialize(sw, this);
            sw.Close();
            return 0;
        }

        public static string GetFilename(out string errmsg)
        {
            string path = LicenseManager.GetSettingFolder(true,out errmsg);
            if (path == null)
                return null;
            return string.Format(@"{0}\Setting.xml", path);
        }

        public static InitialSettingXML Load(out string errmsg)
        {
            InitialSettingXML setting = null;

            string settingFile = GetFilename(out errmsg);
            if (settingFile == null)
                return null;

            System.Xml.Serialization.XmlSerializer seri = null;
            try
            {
                seri = new System.Xml.Serialization.XmlSerializer(typeof(InitialSettingXML));
            }
            catch (Exception e)
            {
                errmsg = "Cannot load license definition file";
                return null;
            }

            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(settingFile, new System.Text.UTF8Encoding(false));
                try
                {
                    setting = (InitialSettingXML)seri.Deserialize(sr);
                }

                finally
                {
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                errmsg = "Cannot load license definition file";
                return null;
            }
            return setting;
        }
    }

    //---------------------------------------------------------------
    // For License Definition file
    //
    public class ProductXMLItem
    {
        public int id;
        public string name;
        public List<int> featureIds = new List<int>();
        public List<int> featureIdxs = new List<int>();
        public bool valid_for_dongle;
        public void SetData(int id, string name,int [] fIds)
        {
            this.id = id;
            this.name = name;
            for (int i = 0; i < fIds.Length; i++)
                featureIds.Add(fIds[i]);
        }
    }

    public class FeatureXMLItem: FeatureItem
    {
#if false
        public void SetData(int id, string name,
             bool valid_for_dongle,
             bool remote_desktop_access,
             bool concurrency,
             int concurrency_count, // 0: Unlimited
             EnumConcurrencyCountCriteria concurrency_count_criteria,
             bool concurrency_network_access)
        {
            this.id = id;
            this.name = name;
            this.valid_for_dongle = valid_for_dongle;
            this.remote_desktop_access = remote_desktop_access;
            this.concurrency = concurrency;
            this.concurrency_count = concurrency_count;
            this.concurrency_count_criteria = concurrency_count_criteria;
            this.concurrency_network_access = concurrency_network_access;
        }
#endif
    }

    public class LicenseXMLData
    {
        public string version = "";
        public List<FeatureXMLItem> features = new List<FeatureXMLItem>();
        public List<ProductXMLItem> products = new List<ProductXMLItem>();

        public static LicenseXMLData LoadDefFile(string xmlFile, out string errmsg)
        {
            errmsg = "";

            if (!System.IO.File.Exists(xmlFile))
                return null;

            LicenseXMLData xmlData = new LicenseXMLData();

#if true
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(xmlFile, new System.Text.UTF8Encoding(false));
                try
                {
                    int line = 0;
                    while (true)
                    {
                        string linestr;
                        linestr = sr.ReadLine();
                        if (linestr == null)
                            break;
                        string[] args = linestr.Split('\t');
                        line++;

                        if( line == 1 )
                        {
                            if (args.Length < 1)
                                throw new Exception("not definition file");
                            if( args[0] != "PHOTRON DONGLE LICENSE DEFINITION")
                                throw new Exception("not definition file");
                            continue;
                        }
                        if( line == 2 )
                        {
                            if(args.Length < 2)
                                throw new Exception("VERSION line is invalid");
                            if(args[0] != "VERSION")
                                throw new Exception("VERSION line is invalid");
                            xmlData.version = args[1];
                            continue;
                        }
                        if (args.Length == 0)
                            continue;
                        if (args[0] == "comment")
                            continue;
                        if( args[0] == "PRODUCT")
                        {
                            if (args.Length < 5)
                                throw new Exception(string.Format("number of PRODUCT items is short(line={0})", line));
                            string arg_id = args[1];
                            string arg_name = args[2];
                            string arg_features = args[3];
                            string arg_valid_for_dongle = args[4];

                            ProductXMLItem pItem = new ProductXMLItem();
                            try
                            {
                                pItem.id = Convert.ToInt32(arg_id);
                            }
                            catch(Exception)
                            {
                                throw new Exception(string.Format("PRODUCT id is not digit(line={0})", line));
                            }
                            if( pItem.id < 1 || 65471 < pItem.id )
                            {
                                throw new Exception(string.Format("PRODUCT id must be 1-65471(line={0})", line));
                            }
                            pItem.name = arg_name;
                            arg_features = arg_features.Trim();
                            if (arg_features[0] == '\"' && arg_features[arg_features.Length - 1] == '\"')
                                arg_features = arg_features.Substring(1, arg_features.Length - 2);
                            string[] fargs = arg_features.Split(',');
                            for( int i = 0; i < fargs.Length; i++)
                            {
                                int fid;
                                try
                                {
                                    fid = Convert.ToInt32(fargs[i]);
                                }
                                catch(Exception)
                                {
                                    throw new Exception(string.Format("feature id not digit is included(line={0})", line));
                                }
                                pItem.featureIds.Add(fid);
                            }
                            if( pItem.featureIds.Count == 0 )
                            {
                                throw new Exception(string.Format("PRODUCT has no features(line={0})", line));
                            }
                            {
                                int idx = StringUtil.MatchIndex(arg_valid_for_dongle, LicenseManager.yes_no);
                                if( idx < 0 )
                                    throw new Exception(string.Format("valid_for_dongle string is invalid(line={0})", line));
                                if (idx == 0)
                                    pItem.valid_for_dongle = true;
                                else
                                    pItem.valid_for_dongle = false;
                            }
                            xmlData.products.Add(pItem);
                        }
                        else if( args[0] == "FEATURE" )
                        {
                            if (args.Length < 9)
                                throw new Exception(string.Format("number of FEATURE items is short(line={0})", line));
                            for(int i = 0; i < args.Length; i++)
                                args[i] = args[i].Trim();

                            string arg_id = args[1];
                            string arg_name = args[2];
                            string arg_valid_for_dongle = args[3];
                            string arg_remote_desktop_access = args[4];
                            string arg_concurrency = args[5];
                            string arg_concurrency_count = args[6];
                            string arg_concurrency_count_criteria = args[7];
                            string arg_concurrency_network_access = args[8];
                            string arg_period = "";
                            if( args.Length >= 10 )
                                arg_period = args[9];

                            FeatureXMLItem fItem = new FeatureXMLItem();
                            try
                            {
                                fItem.id = Convert.ToInt32(arg_id);
                            }
                            catch (Exception)
                            {
                                throw new Exception(string.Format("FEATURE id is not digit(line={0})", line));
                            }
                            fItem.name = arg_name;
                            {
                                int idx = StringUtil.MatchIndex(arg_valid_for_dongle, LicenseManager.yes_no);
                                if (idx < 0)
                                    throw new Exception(string.Format("valid_for_dongle string is invalid(line={0})", line));
                                if (idx == 0)
                                    fItem.valid_for_dongle = true;
                                else
                                    fItem.valid_for_dongle = false;
                            }
                            {
                                int idx = StringUtil.MatchIndex(arg_remote_desktop_access, LicenseManager.yes_no);
                                if (idx < 0)
                                    throw new Exception(string.Format("remote_desktop_access string is invalid(line={0})", line));
                                if (idx == 0)
                                    fItem.remote_desktop_access = true;
                                else
                                    fItem.remote_desktop_access = false;
                            }
                            {
                                int idx = StringUtil.MatchIndex(arg_concurrency, LicenseManager.yes_no);
                                if (idx < 0)
                                    throw new Exception(string.Format("Concurrency string is invalid(line={0})", line));
                                if (idx == 0)
                                    fItem.concurrency = true;
                                else
                                    fItem.concurrency = false;
                            }
                            try
                            {
                                fItem.concurrency_count = Convert.ToInt32(arg_concurrency_count);
                            }
                            catch (Exception)
                            {
                                throw new Exception(string.Format("Concurrency_count is not digit(line={0})", line));
                            }
                            if (fItem.concurrency_count < 0)
                            {
                                throw new Exception(string.Format("Concurrency_count is invalid(line={0})", line));
                            }
                            {
                                int idx = StringUtil.MatchIndex(arg_concurrency_count_criteria, LicenseManager.feature_concurrency_count_criteria);
                                if (idx < 0)
                                    throw new Exception(string.Format("Concurrency_count_criteria string is invalid(line={0})", line));
                                fItem.concurrency_count_criteria =  (EnumConcurrencyCountCriteria)idx;
                            }
                            {
                                int idx = StringUtil.MatchIndex(arg_concurrency_network_access, LicenseManager.yes_no);
                                if (idx < 0)
                                    throw new Exception(string.Format("Concurrency_network_access string is invalid(line={0})", line));
                                if (idx == 0)
                                    fItem.concurrency_network_access = true;
                                else
                                    fItem.concurrency_network_access = false;
                            }
                            {
                                if (arg_period == "" || arg_period == "None" )
                                    fItem.period_kind = Enum_Period.None;
                                else
                                {
                                    int eq_idx = arg_period.IndexOf("=");
                                    if( eq_idx >= 0 )
                                    {
                                        string pre_str = arg_period.Substring(0, eq_idx);
                                        string post_str = arg_period.Substring(eq_idx + 1);
                                        if( pre_str != "Days")
                                            throw new Exception(string.Format("Period string is invalid(line={0})", line));
                                        fItem.period_kind = Enum_Period.Days;
                                        try
                                        {
                                            fItem.period_days = int.Parse(post_str);
                                            if (fItem.period_days <= 0)
                                                throw new Exception();
                                        }
                                        catch(Exception)
                                        {
                                            throw new Exception(string.Format("Period days value is invalid(line={0})", line));
                                        }
                                    }
                                    else if(arg_period == "Date")
                                    {
                                        fItem.period_kind = Enum_Period.Date;
                                        // ここでは期限は設定されない！！！
                                    }
                                    else
                                        throw new Exception(string.Format("Period string is invalid(line={0})", line));
                                }
                            }
                            xmlData.features.Add(fItem);
                        }
                    }
                }
                catch(Exception e)
                {
                    errmsg = e.Message;
                    return null;
                }
                finally
                {
                    sr.Close();
                }

                if( xmlData.version == "" )
                {
                    errmsg = "no data in definition file";
                    return null;
                }
            }
            catch(Exception e)
            {
                errmsg = e.Message;
                return null;
            }
#else
            {
                System.Xml.Serialization.XmlSerializer seri = null;
                try
                {
                    seri = new System.Xml.Serialization.XmlSerializer(typeof(LicenseXMLData));
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return null;
                }
                System.IO.StreamReader sr = new System.IO.StreamReader(xmlFile, new System.Text.UTF8Encoding(false));
                try
                {
                    xmlData = (LicenseXMLData)seri.Deserialize(sr);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    return null;
                }
                finally
                {
                    sr.Close();
                }
            }
#endif

            // Check id consistancy
            for ( int i = 0; i < xmlData.features.Count; i++ )
            {
                FeatureXMLItem fItem0 = xmlData.features[i];
                for( int j = i+1; j < xmlData.features.Count; j++ )
                {
                    FeatureXMLItem fItem1 = xmlData.features[j];
                    if( fItem0.id == fItem1.id )
                    {
                        errmsg = string.Format("Same feature{0} exist in License Definition file.", fItem0.id);
                        return null;
                    }
                }
            }
            for (int i = 0; i < xmlData.products.Count; i++)
            {
                ProductXMLItem item0 = xmlData.products[i];

                for( int j = 0; j < item0.featureIds.Count; j++ )
                {
                    int fid0 = item0.featureIds[j];
                    bool is_fid_exists = false;
                    for( int k = 0; k < xmlData.features.Count; k++ )
                    {
                        if( xmlData.features[k].id == fid0 )
                        {
                            if ( item0.valid_for_dongle && !xmlData.features[k].valid_for_dongle)
                            {
                                errmsg = string.Format("Invalid feature #{0} is included in product #{1} in License Definition file.", fid0, item0.id);
                                return null;
                            }
                            is_fid_exists = true;
                            item0.featureIdxs.Add(k);
                            break;
                        }
                    }
                    if( !is_fid_exists )
                    {
                        errmsg = string.Format("Feature #{0} not exist in product #{1} in License Definition file.",fid0,item0.id);
                        return null;
                    }
                    for( int k = j+1; k < item0.featureIds.Count; k++ )
                    {
                        int fid1 = item0.featureIds[k];
                        if( fid0 == fid1 )
                        {
                            errmsg = string.Format("Same feature #{0} exist in product #{1} in License Definition file.", fid0, item0.id);
                            return null;
                        }
                    }
                }
                for (int j = i + 1; j < xmlData.products.Count; j++)
                {
                    ProductXMLItem item1 = xmlData.products[j];
                    if (item0.id == item1.id)
                    {
                        errmsg = string.Format("Same product #{0} exist in License Definition file.", item0.id);
                        return null;
                    }
                }
            }
            return xmlData;
        }

#if false
        public static LicenseXMLData CreateSampleData()
        {
            FeatureXMLItem fItem0 = new DongleManager.FeatureXMLItem();
            fItem0.SetData(1, "Simple tracking", false, false, true, 0, ConcurrencyCountCriteria.PerStation, false);
            FeatureXMLItem fItem1 = new DongleManager.FeatureXMLItem();
            fItem1.SetData(2, "DAQ", true, false, true, 0, ConcurrencyCountCriteria.PerStation, false);
            FeatureXMLItem fItem2 = new DongleManager.FeatureXMLItem();
            fItem2.SetData(3, "6D MARKER Analyst", true, false, true, 0, ConcurrencyCountCriteria.PerStation, false);
            FeatureXMLItem fItem3 = new DongleManager.FeatureXMLItem();
            fItem3.SetData(4, "6D MARKER Reserved", false, false, true, 0, ConcurrencyCountCriteria.PerStation, false);

            FeatureXMLItem[] fItems = { fItem0, fItem1, fItem2, fItem3 };

            ProductXMLItem pItem0 = new DongleManager.ProductXMLItem();
            pItem0.SetData(2, "PFA", new int[] { 1 });
            ProductXMLItem pItem1 = new DongleManager.ProductXMLItem();
            pItem1.SetData(3, "PFV4 DAQ", new int[] { 2 });
            ProductXMLItem pItem2 = new DongleManager.ProductXMLItem();
            pItem2.SetData(4, "PFA trial", new int[] { 1 });
            ProductXMLItem pItem3 = new DongleManager.ProductXMLItem();
            pItem3.SetData(5, "6D MARKER Analyst", new int[] { 3 });

            ProductXMLItem[] pItems = { pItem0, pItem1, pItem2, pItem3 };

            LicenseXMLData xmlData = new LicenseXMLData();

            for( int i = 0; i < fItems.Length; i++)
            {
                xmlData.features.Add(fItems[i]);
            }
            for (int i = 0; i < pItems.Length; i++)
            {
                xmlData.products.Add(pItems[i]);
            }

            return xmlData;
        }
        public static int SaveSampleData(string xmlFile)
        {
            LicenseXMLData xmlData = CreateSampleData();
            System.Xml.Serialization.XmlSerializer seri = null;
            try
            {
                seri = new System.Xml.Serialization.XmlSerializer(typeof(LicenseXMLData));
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return -1;
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlFile, false, new System.Text.UTF8Encoding(false));
            seri.Serialize(sw, xmlData);
            sw.Close();
            return 0;
        }
#endif
    }

}
