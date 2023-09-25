using Aladdin.HASP;
using System;
using System.Windows.Forms;
using System.Xml;

public class Sentinel
{
    private const string vendorCodeString =
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

    public static int Check(int id, out string errmsg)
    {
        errmsg = "";
        HaspFeature feature = new HaspFeature(id);
        Hasp hasp = new Hasp(feature);

        HaspStatus status = hasp.Login(vendorCodeString);
        try
        {
            if (status == HaspStatus.StatusOk || status == HaspStatus.FeatureExpired)
            {
                // ライセンス期限を調べる
                string scope =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
                    + "<haspscope>"
                    + "    <hasp type=\"HASP-HL\">"
                    + "        <license_manager hostname=\"localhost\"/>"
                    + "    </hasp>"
                    + "</haspscope>";
                const string info_str =
                    "<haspformat root=\"hasp_info\">"
                    + "<feature>"
                    + "<attribute name=\"id\"/>"
                    + "<element name=\"license\"/>"
                    + "<hasp>"
                    + "<attribute name=\"id\"/>"
                    + "<attribute name = \"type\"/>"
                    + "</hasp>"
                    + "</feature>"
                    + "</haspformat>";

                string info = "";
                HaspStatus status_l = Hasp.GetInfo(scope, info_str, vendorCodeString, ref info);
                if (status_l != HaspStatus.StatusOk)
                {
                    errmsg = $"Cannot access dongle.[status={status_l.ToString()}]";
                    return (int)status_l;
                }

                int license_type = -1;
                DateTime expiration_date = DateTime.MinValue;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(info);
                XmlElement elem = xmlDoc.DocumentElement;
                if (elem.LocalName == "hasp_info")
                {
                    for (XmlNode haspNode = elem.FirstChild; haspNode != null; haspNode = haspNode.NextSibling)
                    {
                        if (license_type == 0)
                            break; // 永久ならそれで決まり

                        if (haspNode.LocalName != "feature")
                            continue;

                        for (int i = 0; i < haspNode.Attributes.Count; i++)
                        {
                            if (license_type == 0)
                                break; // 永久ならそれで決まり

                            XmlAttribute feature_att = haspNode.Attributes[i];
                            if (feature_att.LocalName == "id")
                            {
                                int fid = int.Parse(feature_att.Value);
                                if (fid != id)
                                    continue; // 対象のフィーチャー以外は見ない

                                for (XmlNode featureNode = elem.FirstChild; featureNode != null; featureNode = featureNode.NextSibling)
                                {
                                    if (license_type == 0)
                                        break; // 永久ならそれで決まり

                                    if (featureNode.LocalName != "license")
                                        continue;
                                    int t_ltype = -1;
                                    DateTime t_exp_date = DateTime.MinValue;
                                    DateTime t_start_time = DateTime.MinValue;
                                    int t_days_of_exp = 0;
                                    for (XmlNode licenseNode = elem.FirstChild; licenseNode != null; licenseNode = licenseNode.NextSibling)
                                    {
                                        if (license_type == 0)
                                            break; // 永久ならそれで決まり

                                        if (licenseNode.LocalName == "license_type")
                                        {
                                            if (licenseNode.Value == "perpetual")
                                                license_type = 0;
                                            else
                                            {
                                                if (licenseNode.Value == "expiration")
                                                    t_ltype = 1;
                                                else if (licenseNode.Value == "trial")
                                                    t_ltype = 2;
                                            }
                                        }
                                        else if( licenseNode.LocalName == "exp_date")
                                        {
                                            // YET 日時変換
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                if (status == HaspStatus.FeatureExpired)
                {
                    errmsg = $"License Error.[status={status.ToString()}]";
                    return (int)status;
                }
            }
            else
            {
                errmsg = $"License Error.[status={status.ToString()}]";
                return (int)status;
            }
        }
        finally
        {
            hasp.Logout();
            hasp.Dispose();
        }
        return 0;
    }
}