using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;

using Aladdin.HASP;


namespace DongleToolLib
{
    public delegate bool Dele_GetDongleInfo(List<DongleListItem> dongleItems, out string errmsg);

    public class LicenseMiniManager
    {
        public List<ProductItem> products = new List<ProductItem>();
        public List<FeatureItem> features = new List<FeatureItem>();

#if _VENDOR_DEMO
       // private static String base64code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public const bool isDemoMode = true;

        public const String vendor_code =
           "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMA" +
           "sVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWT" +
           "OZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06" +
           "waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4V" +
           "nYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/Id" +
           "gLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1" +
           "YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMU" +
           "uRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7" +
           "s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLV" +
           "TvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q" +
           "9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0" +
           "m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VF" +
           "ITB3mazzFiyQuKf4J6+b/a/Y";
#else
        public const bool isDemoMode = false;
        public static string vendor_code =
            "/k0mD4BadhLfXroVy3W1JdVvHSh+J12bxl0mCM/Zr9YV8BHNULxyaTZSC01LDGKfZ+fqfMCQ4XfFi1vA"
            + "wQuIIWfca7pkdV79ycUr2e+69SnuW7dfCr7aEKbuzVBYnlDHlbRgUVgtRs+gsqIXsGMW8DB89vG2gJxd"
            + "M17mI3+Tbu5BnN9tNv7Z6O5rU83HmSi99ULn/ZkQmLGkdlLC8pf0Q5fo5xKtA9NgNIdNY64HwPO32fcH"
            + "IMJPuPsEQr46ERTXDHoXCbiX7/CMNbQ0COwB3nIdqUyjFMJccg7JZ9NFpfAiSf0FbWskFufF8+4aAnvj"
            + "R2ZMgA6Fg7JqqVLuo5Fd+f1Jtw6/5BOp2GDpNV2D+JDsJAJiShOfeNiZnsYFvMnLfaVYPF8pmZ59BVnM"
            + "Vye6v/BsZe5atov/83bYfr4ke3GsrYas96CiZUE0ARIaLBrXPL/3upY2Gxtl/XC1lGm6qDt4CYZ9OW3i"
            + "9QuSsql1LNCzpIjdHl6v9WrlMM0n0Jyr2EUTkCmy6MU0znDWh7aj9/LlzGt0+L80FijMP/9mRtEi0SEJ"
            + "w+D2DKPR4WAZtBonXnGOQPcLFMegKZlgZpsgQyQYFfbWg18ztoUDnOtXGi7E6qkM7WKgAxi3N4yjWwil"
            + "89k43J9QsxwPPDGLX2HryP0SlTLx8Elxc+n4+VNur0Z6jQ9y0D6O1CEg+T4nvuHf0pqlcFxHDnqB1bFc"
            + "lyokxAU6023A0vf0Ri6p4F73McIdt2208A+0Op8uHxeXThjFVtt7JV7b7+Nm6qJ4pyBcgyaZqS9ukn7q"
            + "3fQSBZXYIllsoeviXCCyWSlm4RePvTguUyFrfB6uMxrQ0PDZF4kbZt1aflIxd1SxL7CZfo4r+JrTuH/m"
            + "b3B58ll4xUlqG/yjuF0IU4nYRFzLNPZjF+Nz5+JfxA1BFPTdOmByRTrQUa/+00GtEEZDm1dqIEP2Z60k"
            + "frWTkec6iLfbQw5cB7mliQ==";
#endif

        public static string GetDongleC2V(string dongleId, out string errmsg)
        {
            errmsg = "";
            Hasp hasp = new Hasp(new HaspFeature(0));
            string scope_fmt =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                + "<haspscope>"
                + "    <hasp id=\"{0}\" />"
                + "</haspscope>"
                + "";

            string scope = string.Format(scope_fmt, dongleId);
            HaspStatus status = hasp.Login(LicenseMiniManager.vendor_code, scope);
            if (status != HaspStatus.StatusOk)
            {
                errmsg = string.Format("Cannot access to dongle #{0} [status={1}]", dongleId, status);
                return null;
            }

            string c2vData = "";
            try
            {
                status = hasp.GetSessionInfo("<haspformat format=\"updateinfo\"/>", ref c2vData);
                if (status != HaspStatus.StatusOk)
                {
                    errmsg = string.Format("Cannot access to dongle #{0} [status={1}]", dongleId, status);
                    return null;
                }
            }
            finally
            {
                hasp.Logout();
            }
            return c2vData;
        }

        public static List<DongleListItem> EnumDongle(Dele_GetDongleInfo dele_GetDongleInfo, out string errmsg)
        {
            errmsg = "";

            List<DongleListItem> dongleItems = new List<DongleListItem>();
            string scope =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                + "<haspscope>"
                + "    <hasp type=\"HASP-HL\" >"
                + "        <license_manager hostname=\"localhost\" />"
                + "    </hasp>"
                + "</haspscope>";

            // Feature ids
            {
                string format =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                    + "<haspformat root=\"hasp_info\">"
                    + "    <hasp>"
                    + "        <attribute name=\"id\" />"
                    + "        <feature>"
                    + "            <attribute name=\"id\" />"
                    + "        </feature>"
                    + "        <product>"
                    + "            <element name=\"id\"/>"
                    + "        </product>"
                    + "    </hasp>"
                    + "</haspformat>";
                string info = "";
                HaspStatus status = Hasp.GetInfo(scope, format, LicenseMiniManager.vendor_code, ref info);
                if (status != HaspStatus.StatusOk)
                {
                    errmsg = string.Format("Cannot access dongle.[status={0}]", status);
                    return null;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(info);
                XmlElement elem = xmlDoc.DocumentElement;
                if (elem.LocalName == "hasp_info")
                {
                    DongleListItem dongleItem = null;

                    for (XmlNode haspNode = elem.FirstChild; haspNode != null; haspNode = haspNode.NextSibling)
                    {
                        if (haspNode.LocalName != "hasp")
                            continue;

                        for (int i = 0; i < haspNode.Attributes.Count; i++)
                        {
                            XmlAttribute dongleId_att = haspNode.Attributes[i];
                            if (dongleId_att.LocalName == "id")
                            {
                                dongleItem = new DongleListItem();
                                dongleItem.dongleId = dongleId_att.Value;
                                dongleItems.Add(dongleItem);
                                break;
                            }
                        }
                        for (XmlNode node1 = haspNode.FirstChild; node1 != null; node1 = node1.NextSibling)
                        {
                            if (node1.LocalName == "feature")
                            {
                                for (int j = 0; j < node1.Attributes.Count; j++)
                                {
                                    XmlAttribute feathreId_att = node1.Attributes[j];
                                    if (feathreId_att.LocalName == "id")
                                    {
                                        FeatureIdAndPeriodInDognle fItem = new FeatureIdAndPeriodInDognle(Convert.ToInt32(feathreId_att.Value));
                                        dongleItem.featureIds.Add(fItem);
                                    }
                                }
                            }
                            else if (node1.LocalName == "product")
                            {
                                for (XmlNode node2 = node1.FirstChild; node2 != null; node2 = node2.NextSibling)
                                {
                                    if (node2.LocalName == "id")
                                    {
                                        dongleItem.productIds.Add(Convert.ToInt32(node2.InnerText));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // YET 別PCに移動しての動作確認
            // YET 本番DongleManagerツール、ライブラリを作る必要あり
            // YET MAXドングルを仕入れる必要あり
            //-------ここは調査後
            // YET 各ツールごとに期限が近づくとライセンス延長を促すガイダンス表示が必要
            //     ・DongleToolを起動しヘルプに沿って処理することを促す
            //　　 ・説明ページURLを表示するのが良いかもしれない。
            // YET SDKにも期限情報が取得できる関数を用意する必要がある
            //     ・LinuxからはWindowsで対応してもらう必要がある
            //--------------------------------------------------

            // featureの期限を取り出す
            foreach (DongleListItem dongleItem in dongleItems)
            {
                List<string> dongleList = new List<string>();
                dongleList.Add(dongleItem.dongleId);

                if (dongleItem.c2vData == null)
                {
                    foreach (FeatureIdAndPeriodInDognle fItem in dongleItem.featureIds)
                    {
                        LibSentinelCLI.ExpirationInfoCLI expInfo;

                        if (!LibSentinelCLI.SentinelCLI.HaspGetExpirationInfo_Scope(fItem.id, dongleList, out expInfo, out errmsg))
                            return null;

                        if (expInfo.kind == 0)
                            fItem.period_kind = Enum_Period.None;
                        else if (expInfo.kind == 1)
                        {
                            fItem.period_kind = Enum_Period.Date;
                            fItem.period_date = expInfo.date;
                        }
                        else if (expInfo.kind == 2)
                        {
                            fItem.period_kind = Enum_Period.Days;
                            if (expInfo.is_already_access)
                                fItem.period_days_end = expInfo.date;
                            fItem.period_days = expInfo.remaining_days;
                        }
                    }
                }
                else
                {
                    // C2Vから読み込むときは、デリゲートでLicenseManagerのメソッドで処理する必要がある
                    // C2Vからの読込はライセンス生成に含まれる関数を呼ぶ必要があるので。
                    if( dele_GetDongleInfo == null )
                    {
                        errmsg = "Internal error. No dele_GetDongleInfo setting.";
                        return null;
                    }
                    if (!dele_GetDongleInfo(dongleItems, out errmsg))
                        return null;
                }
            }
            return dongleItems;
        }
        public static int BurnToDongle(string dongleUniqStr, string v2cData, out string errmsg)
        {
            errmsg = "";
            Hasp hasp = new Hasp(new HaspFeature(0));
            /*
            string scope_fmt =
                "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                + "<haspscope>"
                + "    <hasp id=\"{0}\" />"
                + "</haspscope>"
                + "";

            string scope = string.Format(scope_fmt, dongleId);
            HaspStatus status = hasp.Login(LicenseManager.vendor_code, scope);
            if (status != HaspStatus.StatusOk)
            {
                errmsg = string.Format("Cannot access to dongle #{0}", dongleId);
                return -1;
            }
            */

            string acknowledgeXml = "";
            try
            {
                HaspStatus status = Hasp.Update(v2cData, ref acknowledgeXml);
                if (status != HaspStatus.StatusOk)
                {
                    errmsg = string.Format("Cannot access to dongle #{0} [status={1}]", dongleUniqStr, status);
                    return -1;
                }
            }
            finally
            {
                // hasp.Logout();
            }
            return 0;
        }
        public static void StartBlinkDongle(string dongleId)
        {
            string url = string.Format("http://127.0.0.1:1947/action.html?blinkon={0}", dongleId);
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
        }
        public static void StopBlinkDongle(string dongleId)
        {
            string url = string.Format("http://127.0.0.1:1947/action.html?blinkoff={0}", dongleId);
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
        }
        public ProductItem GetProductItem(int pId)
        {
            foreach (ProductItem pItem in products)
            {
                if (pItem.id == pId)
                    return pItem;
            }
            return null;
        }

        public FeatureItem GetFeatureItem(int fId)
        {
            foreach (FeatureItem fItem in features)
            {
                if (fItem.id == fId)
                    return fItem;
            }
            return null;
        }
        public static string LoadFile(string file, out string errmsg)
        {
            errmsg = "";
            string data = null;
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(file, new System.Text.UTF8Encoding(false));
                try
                {
                    data = sr.ReadToEnd();
                }
                finally
                {
                    sr.Close();
                }
            }
            catch (Exception)
            {
                errmsg = string.Format("Cannot load\r\n({0})", file);
                return null;
            }
            return data;
        }
    }

    // For enum dongle
    public class DongleListItem
    {
        public string dongleId = "";
        public List<int> productIds = new List<int>();
        public List<FeatureIdAndPeriodInDognle> featureIds = new List<FeatureIdAndPeriodInDognle>();
        public bool updated = false; // ドングルをアップデートする際に、ライセンス内容が同じときはfalseになる 
        public string c2vData = null; // C2Vファイルを読んだときだけセットされる
        public string c2v_basename = null;
        public string v2cData = null;
        public string v2c_filename = null;
    }
    public class FeatureIdAndPeriodInDognle
    {
        public int id;
        public Enum_Period period_kind = Enum_Period.None;
        public DateTime period_date = DateTime.MinValue;
        public int period_days;
        public DateTime period_days_end = DateTime.MinValue;

        public FeatureIdAndPeriodInDognle(int id)
        {
            this.id = id;
        }
    }
    public enum Enum_Period
    {
        None,
        Date,
        Days
    }
    public class FeatureItem
    {
        public int id;
        public string name;
        public bool valid_for_dongle; // ネットワークアクティベーション用や予約されているものはfalse
        public bool remote_desktop_access;
        public bool concurrency;
        public int concurrency_count; // 0: Unlimited
        public EnumConcurrencyCountCriteria concurrency_count_criteria;
        public bool concurrency_network_access;
        public Enum_Period period_kind = Enum_Period.None;
        public DateTime period_date = DateTime.MinValue;
        public int period_days;
    }
    public enum EnumConcurrencyCountCriteria
    {
        PerProcess = 0,
        PerStation = 1,
        PerLogin = 2
    }

    public class ProductItem
    {
        public int id;
        public string name;
        public List<int> featureIdxs = new List<int>();
        // public string featuresStr = "";
    }
}