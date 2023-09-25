/****************************************************************************
*
* Demo program for Sentinel LDK
*
*
* Copyright (C) 2014, SafeNet, Inc. All rights reserved.
*
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SafeNet.Sentinel;
using Aladdin.HASP;


namespace AdminApiSample
{
    class AdminApiSample
    {

        private static AdminApi adminApiIntegrated;
        private static AdminApi adminApiStandalone;

        public static void Main()
        {

            AdminStatus status;
            String data = "";
            // You can specify some other server IP address (from different subnet also) hosting Sentinel License Manager Service
            String server = "127.0.0.1";

            Console.WriteLine("A simple demo program for the Sentinel LDK administration functions");
            Console.WriteLine("Copyright (C) 2013, SafeNet, Inc. All rights reserved.\n\n");

            // Below is DEMOMA vendor code. ISV can update below with their assigned vendor code
            VendorCodeType vc = new VendorCodeType(
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
                "tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"
                );

            Console.WriteLine("connect to sntl_integrated_lm ");

            adminApiIntegrated = new AdminApi(vc, "sntl_integrated_lm");

            status = adminApiIntegrated.connect();

            printState(status);

            Console.WriteLine("adminGet ");

            status = adminApiIntegrated.adminGet(
                                    "<haspscope/>",
                                    "<context></context>",
                                    ref data
                                    );

            printState(status, data);

            Console.WriteLine("adminSet ");

            status = adminApiIntegrated.adminSet(
                                    "<config>" +
                                    " <serveraddrs_clear/>" +
                                    " <server_select>" + server + "</server_select>" +
                                    "</config>",
                                    ref data
                                    );

            printState(status, data);

            Console.WriteLine("login to default feature of any key on " + server);

            Hasp haspAny = new Hasp(HaspFeature.Default);
            HaspStatus haspStatus = haspAny.Login(vc.ToString(),
                                    "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                    "<haspscope>" +
                                    " <license_manager ip=\"" + server + "\" />" +
                                    "</haspscope>"
                                    );

            printState(haspStatus);

            Console.WriteLine("login to default feature of SL-AdminMode key on " + server);

            Hasp haspSlAm = new Hasp(HaspFeature.Default);
                        haspStatus = haspSlAm.Login(vc.ToString(),
                                    "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                    "<haspscope>" +
                                    " <hasp type=\"HASP-SL-AdminMode\" >" +
                                    "  <license_manager ip=\"" + server + "\" />" +
                                    " </hasp>" +
                                    "</haspscope>"
                                    );

            printState(haspStatus);

            status = adminApiIntegrated.adminGet(
                                    "<haspscope/>",
                                    "<admin>" +
                                    "  <license_manager>" +
                                    "   <element name=\"version\" />" +
                                    "   <element name=\"servername\" />" +
                                    "   <element name=\"uptime\" />" +
                                    "   <element name=\"driver_info\" />" +
                                    "  </license_manager>" +
                                    "</admin>",
                                    ref data
                                    );

            printState(status, data);

            // Access Standalone LMS using Integrated Admin API

            Console.WriteLine("connect to Standalone LMS");

            adminApiStandalone = new AdminApi(vc, server, 1947, "");

            status = adminApiStandalone.connect();

            printState(status);

            Console.WriteLine("adminGet ");
            // retrieve sessions
            status = adminApiIntegrated.adminSet(
                                    "<config>" +
                                    " <serveraddrs_clear/>" +
                                    " <server_select>" + server + "</server_select>" +
                                    "</config>",
                                    ref data
                                    );

            status = adminApiIntegrated.adminSet(
                        "<config>" +
                        " <serveraddrs_clear/>" +
                        " <server_select>" + server + "</server_select>" +
                        "</config>",
                        ref data
                        );

            printState(status, data);

            Console.WriteLine("adminGet ");

            status = adminApiStandalone.adminGet(
                                    "<haspscope/>",
                                    "<context></context>",
                                    ref data
                                    );

            printState(status, data);

            Console.WriteLine("adminGet ");

            status = adminApiStandalone.adminGet(
                                    "<haspscope/>",
                                    "<admin>" +
                                    "  <license_manager>" +
                                    "   <element name=\"version\" />" +
                                    "   <element name=\"servername\" />" +
                                    "   <element name=\"uptime\" />" +
                                    "   <element name=\"driver_info\" />" +
                                    "  </license_manager>" +
                                    "</admin>",
                                    ref data
                                    );

            printState(status, data);

            haspAny.Logout();
            haspSlAm.Logout();
        }


        #region "help functions"

        private static void printState(HaspStatus status)
        {
            printState((AdminStatus) (int) status, null);
        }

        private static void printState(AdminStatus status)
        {
            printState(status, null);
        }

        private static void printState(AdminStatus status, string info)
        {
            if (!(string.IsNullOrEmpty(info)))
            {
                Console.WriteLine(info);
            }

            Console.WriteLine("Result: " + getErrorText(status) + " Statuscode: " + (int)status + "\n");
        }

        private static void generateConfigFromOutput(string output, ref string config)
        {
            if (output != null)
            {
                Regex regex = new Regex("<admin_status>");
                string[] result = regex.Split(output);

                config = result[0].Replace("</config>", "<writeconfig /></config>");
            }
        }

        private static string getErrorText(AdminStatus status)
        {
            switch (status)
            {
                case AdminStatus.StatusOk: return "StatusOk";
                case AdminStatus.InvalidContext: return "InvalidContext";
                case AdminStatus.LmNotFound: return "LmNotFound";
                case AdminStatus.LmTooOld: return "LmTooOld";
                case AdminStatus.BadParameters: return "BadParameters";
                case AdminStatus.LocalNetWorkError: return "LocalNetWorkError";
                case AdminStatus.CannotReadFile: return "CannotReadFile";
                case AdminStatus.ScopeError: return "ScopeError";
                case AdminStatus.PasswordRequired: return "PasswordRequired";
                case AdminStatus.CannotSetPassword: return "CannotSetPassword";
                case AdminStatus.UpdateError: return "UpdateError";
                case AdminStatus.BadValue: return "BadValue";
                case AdminStatus.ReadOnly: return "ReadOnly";
                case AdminStatus.ElementUndefined: return "ElementUndefined";
                case AdminStatus.InvalidPointer: return "InvalidPointer";
                case AdminStatus.NoIntegratedLm: return "NoIntegratedLm";
                case AdminStatus.ResultTooBig: return "ResultTooBig";
                case AdminStatus.InvalidVendorCode: return "InvalidVendorCode";
                case AdminStatus.UnknownVendorCode: return "UnknownVendorCode";
            }
            return "";
        }

        #endregion

    }
}
