////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Thales Group. All rights reserved.
//
//
//
////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sentinel.Ldk.LicGen;

namespace sample
{
    public enum Error_Msg
    {
        SNTL_LG_SAMPLE_RETURN_ERROR = 0,
        SNTL_LG_SAMPLE_PARAMETER_ERROR,
        SNTL_LG_SAMPLE_DECODE_STATE_FAILED,
        SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED,
        SNTL_LG_SAMPLE_GENERATE_BASE_INDEPENDENT_FAILED,
        SNTL_LG_SAMPLE_CLEAR_KEY_FAILED,
        SNTL_LG_SAMPLE_FORMAT_KEY_FAILED,
        SNTL_LG_SAMPLE_GENERATE_NEW_FAILED,
        SNTL_LG_SAMPLE_GENERATE_MODIFY_FAILED,
        SNTL_LG_SAMPLE_GENERATE_CANCEL_FAILED,
        SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED,
        SNTL_LG_SAMPLE_LOAD_FILE_FAILED,
        SNTL_LG_SAMPLE_STORE_FILE_FAILED,
        SNTL_LG_SAMPLE_INITIALIZE_FAILED,
        SNTL_LG_SAMPLE_START_FAILED,
    };

    public enum License_Type
    {
        SNTL_LG_SAMPLE_LICENSE_TYPE = 0,
        SNTL_LG_SAMPLE_LICENSE_HL,
        SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE,
        SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE,
    };

    class sample
    {
        private static String base64code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        public const String vendorCode =
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

        public String szC2V;
        public String[] szDXML;

        public String szV2CFilename;
        public String szUXMLFilename;
        public String szCurrentStateFilename;

        public int option;
        public int license_template_number;

        public int nothing_to_clear;
        public int nothing_to_format;

        public static LicGenAPIHelper licGenHelp = null;
        string error_msg;

        public String szKey_type;
        public String szKey_configuration;
        public bool dynamic_memory_supported_key;

        public sample()
        {
            szDXML = new string[3];
            dynamic_memory_supported_key = false;
        }

        static void Main(string[] args)
        {
            using (licGenHelp = new LicGenAPIHelper())
            {
                sample sample = new sample();

                sample.show_copyright();
                sample.show_LicGen_version();
                if (sample.handle_parameter(args) != 0)
                {
                    return;
                }

                //demonstrate the HL example
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_HL);

                if (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to decode current state");
                    Console.WriteLine("");
                }

                if (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to generate clear license");
                    Console.WriteLine("");
                }

                if (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to generate format license");
                    Console.WriteLine("");
                }

                if (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to generate new license");
                    Console.WriteLine("");
                }

                if (sample.option == 'n')
                {
                    goto GOTO_SL_AM_MODULE;
                }

                if (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to generate modify");
                    Console.WriteLine("");
                }

                if (sample.option == 'm')
                {
                    goto GOTO_SL_AM_MODULE;
                }

                if (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    Console.WriteLine("  Fail to generate cancel license");
                    Console.WriteLine("");
                }

                Console.WriteLine("");

           //demonstrate the SL-AdminMode example
            GOTO_SL_AM_MODULE:
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE);

                if (sample.generate_provisional_license(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate provisional license");
                    Console.WriteLine("");
                }

                if (sample.process_new_base_independent(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate base independent license");
                    Console.WriteLine("");
                }

                if (sample.process_new_rehost(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate base independent license that allows rehosting");
                    Console.WriteLine("");
                }

                if (sample.process_new_detach(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate base independent detachable license");
                    Console.WriteLine("");
                }

                if (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to decode current state");
                    Console.WriteLine("");
                }

                if (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate clear license");
                    Console.WriteLine("");
                }

                if (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate format license");
                    Console.WriteLine("");
                }

                if (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate new license");
                    Console.WriteLine("");
                }

                if (sample.option == 'n')
                {
                    goto GOTO_SL_UM_MODULE;
                }

                if (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate modify");
                    Console.WriteLine("");
                }

                if (sample.option == 'm')
                {
                    goto GOTO_SL_UM_MODULE;
                }

                if (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate cancel license");
                    Console.WriteLine("");
                }

                Console.WriteLine("");

           //demonstrate the SL-UserMode example
            GOTO_SL_UM_MODULE:
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE);

                if (sample.generate_provisional_license(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate provisional license");
                    Console.WriteLine("");
                }

                if (sample.process_new_base_independent(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate base independent license");
                    Console.WriteLine("");
                }

                if (sample.process_new_rehost(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate base independent license that allows rehosting");
                    Console.WriteLine("");
                }

                if (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to decode current state");
                    Console.WriteLine("");
                }

                if (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate clear license");
                    Console.WriteLine("");
                }

                if (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate format license");
                    Console.WriteLine("");
                }

                if (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate new license");
                    Console.WriteLine("");
                }

                if (sample.option == 'n')
                {
                    return;
                }

                if (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate modify");
                    Console.WriteLine("");
                }

                if (sample.option == 'm')
                {
                    return;
                }

                if (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    Console.WriteLine("  Fail to generate cancel license");
                    Console.WriteLine("");
                }
            }
        }

        /**
         * * use BASE64 to encode the input string
         * *
         * * @param input
         * *            string that is ready for encode
         * * @return encoded string or null when error
         * */
        public static String base64_encode(byte[] input)
        {
            int length = 0;
            int pad_count = 0;

            byte[] input_string;

            StringBuilder output;

            length = input.Length;
            if (length == 0)
            {
                return null;
            }

            pad_count = (3 - (length % 3)) % 3;

            output = new StringBuilder((length + pad_count) / 3 * 4);

            input_string = new byte[length + pad_count];
            Array.Copy(input, input_string, length);


            for (int i = 0; i < length; i += 3)
            {
                int val = 0;
                for (int j = 0; j < 3; j++)
                {
                    val |= input_string[i + j];
                    val <<= 8;
                }

                for (int k = 0; k < 4; k++)
                {
                    byte index = (byte)((val >> 26) & 0x3f);
                    output.Append(base64code[index]);
                    val <<= 6;
                }
            }

            if (pad_count > 0)
            {
                output[output.Length - 1] = '=';
                pad_count--;

                if (pad_count > 0)
                {
                    output[output.Length - 2] = '=';
                }
            }

            return output.ToString();
        }

        /**
         * use BASE64 map to map a char to byte
         *
         * @param input
         *            char that is ready for convert
         * @return corresponding byte value of the input char
         */
        public static byte base64_decode_char(char input)
        {
            if ((input >= 'A') && (input <= 'Z'))
            {
                return (byte)(input - 'A' + 0); // 0 range starts at 'A'
            }

            if ((input >= 'a') && (input <= 'z'))
            {
                return (byte)(input - 'a' + 26); // 26 range starts at 'a'
            }

            if ((input >= '0') && (input <= '9'))
            {
                return (byte)(input - '0' + 52); // 52 range starts at '0'
            }

            if (input == '+')
            {
                return 62;
            }

            if (input == '/')
            {
                return 63;
            }

            if (input == '=')
            {
                return 64;
            }

            return 65;
        }

        /**
         * use BASE64 to decode the input string
         *
         * @param input
         *            string that is ready for decode
         * @return decoded string or null when error
         */
        public static byte[] base64_decode(String input)
        {
            int length = 0;
            int pad_count = 0;
            int temp = 0;

            byte[] output;
            byte[] output_array;

            length = input.Length;

            // verify if input is a legal BASE64 string
            if ((length == 0) || (length % 4 != 0))
            {
                return null;
            }

            for (int i = 0; i < length; i++)
            {
                temp = base64_decode_char(input[i]);
                if (temp == 64)
                {
                    if ((i != (length - 1)) && (i != (length - 2)))
                    {
                        return null;
                    }
                    if (i == (length - 2))
                    {
                        if (base64_decode_char(input[i + 1]) != 64)
                        {
                            return null;
                        }
                        pad_count++;
                    }
                    else
                    {
                        pad_count++;
                    }
                }

                if (temp == 65)
                {
                    return null;
                }
            }

            output_array = new byte[length / 4 * 3];

            for (int i = 0, j = 0; i < length; i += 4, j += 3)
            {
                int val = 0;
                for (int k = 0; k < 4; k++)
                {
                    byte index = base64_decode_char(input[i + k]);
                    val |= index & 0x3f;
                    val <<= 6;
                }
                val <<= 2;

                for (int l = 0; l < 3; l++)
                {
                    output_array[j + l] = (byte)(val >> 24);
                    val <<= 8;
                }
            }

            output = new byte[length / 4 * 3 - pad_count];
            Array.Copy(output_array, output, length / 4 * 3 - pad_count);

            return output;
        }

        public static string load_file(string filename)
        {
            char[] temp;
            int length;

            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists)
            {
                return null;
            }

            StreamReader fs = fi.OpenText();
            length = (int)fi.Length;
            temp = new char[fi.Length];

            fs.Read(temp, 0, length);
            fs.Close();

            return new string(temp);
        }

        public static int store_file(string buffer, string filename)
        {
            try
            {
                string path = "output";

                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Create the directories
                    Directory.CreateDirectory("output");
                    Directory.CreateDirectory("output/HL");
                    Directory.CreateDirectory("output/SL-AdminMode");
                    Directory.CreateDirectory("output/SL-UserMode");
                }
                else
                {
                    if (!Directory.Exists("output/HL"))
                    {
                        Directory.CreateDirectory("output/HL");
                    }

                    if (!Directory.Exists("output/SL-AdminMode"))
                    {
                        Directory.CreateDirectory("output/SL-AdminMode");
                    }

                    if (!Directory.Exists("output/SL-UserMode"))
                    {
                        Directory.CreateDirectory("output/SL-UserMode");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            FileStream fs = new FileStream(filename, FileMode.Create);
            if (null == fs)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            fs.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(buffer), 0, buffer.Length);
            fs.Close();
            return 0;
        }

        public void show_help()
        {
            Console.WriteLine(" Command line parameter usage of this program:");
            Console.WriteLine("     -h        Display this help message and exit.");
            Console.WriteLine("     -c        [default] Generate new, modify and cancel licenses.");
            Console.WriteLine("     -m        Generate new and modify licenses.");
            Console.WriteLine("     -n        Generate new license only.");
            Console.WriteLine("\n\n");
        }

        /**
         * deal with command line parameters
         *
         * @param args
         *            command line args
         * @return 0 if success, other if failure
         */
        public int handle_parameter(String[] args)
        {
            int length = args.Length;

            if (length == 0)
            {
                option = 'c';
                return 0;
            }

            if (length > 1)
            {
                Console.WriteLine(" Error: unrecongnized or incomplete command line.\n");
                show_help();
                return 1;
            }

            if (args[0].Equals("-c"))
            {
                option = 'c';
                return 0;
            }

            if (args[0].Equals("-m"))
            {
                option = 'm';
                return 0;
            }

            if (args[0].Equals("-n"))
            {
                option = 'n';
                return 0;
            }

            if (!args[0].Equals("-h"))
            {
                Console.WriteLine(" Error: unrecongnized or incomplete command line.\n");
            }

            show_help();
            return 2;
        }

        public static void show_copyright()
        {
            Console.WriteLine("");
            Console.WriteLine(" A demo program for the Sentinel LDK license generation functions");
            Console.WriteLine(" Copyright (C) Thales Group. All rights reserved.");
            Console.WriteLine("");
        }

        public void show_LicGen_version()
        {
            sntl_lg_status_t status;
            int major_version = 0, minor_version = 0, build_server = 0, build_number = 0;
            status = LicGenAPIHelper.sntl_lg_get_version(ref major_version, ref minor_version, ref build_server, ref build_number);

            if (status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
            {
                return;
            }

            Console.WriteLine("");
            Console.WriteLine(" Sentinel LDK License Generation Windows DLL " + major_version.ToString() + "." + minor_version.ToString() + " build " + build_number.ToString());
            Console.WriteLine("");
        }

        public static void show_sample_license_type(License_Type license_type)
        {
            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("");
                Console.WriteLine(" The following is for Sentinel LDK HL key type\n");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("");
                Console.WriteLine(" The following example is for Sentinel LDK SL Admin Mode key type\n");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("");
                Console.WriteLine(" The following example is for Sentinel LDK SL User Mode key type\n");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }
        }

        /**
         * generate provisional license
         */
        public int generate_provisional_license(License_Type license_type)
        {
            sntl_lg_status_t status = 0;

            string szInitParamXML = null;
            string szStartParamXML = null;
            string szLicenseDefinitionXML = null;
            string szGenerationParamXML = null;
            string license = null;
            string resultant_state = null;

            szC2V = null;

            Console.WriteLine(" Process provisional license:");

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                szLicenseDefinitionXML = load_file("input/SL-AdminMode/provisional_template.xml");
                if (szLicenseDefinitionXML == null)
                {
                    Console.WriteLine("  error in loading input/SL-AdminMode/provisional_template.xml file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                szLicenseDefinitionXML = load_file("input/SL-UserMode/provisional_template.xml");
                if (szLicenseDefinitionXML == null)
                {
                    Console.WriteLine("  error in loading input/SL-UserMode/provisional_template.xml file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
            }
            else
            {
                ;//do nothing
            }

            //
            // sntl_lg_initialize
            // Initializes license generation library
            // and returns handle to work further
            //
            status = licGenHelp.sntl_lg_initialize(szInitParamXML);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_initialize: " + error_msg);
            if (status != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
            }

            //
            // sntl_lg_start
            // Starts the license generation.
            //
            status = licGenHelp.sntl_lg_start(szStartParamXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_PROVISIONAL, szLicenseDefinitionXML, szC2V);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_start: " + error_msg);
            if (status != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
            }

            //
            // sntl_lg_generate_license
            // Generates the license.
            //
            status = licGenHelp.sntl_lg_generate_license(szGenerationParamXML, ref  license, ref  resultant_state);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_generate_license: " + error_msg);
            if (status != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
            }

            string licenseFilePath = null;
            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                licenseFilePath = "output/SL-AdminMode/provisional_license.v2c";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                licenseFilePath = "output/SL-UserMode/provisional_license.v2c";
            }
            else
            {
                ;//do nothing
            }

            status = (sntl_lg_status_t)store_file(license, licenseFilePath);
            if (status != 0)
            {
                Console.WriteLine("  provisional license file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            //Clean up memories that used in the routine
            licGenHelp.sntl_lg_cleanup();

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/provisional_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/provisional_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * process generation of base independent license
         *
         * @return 0 if success, other if failure
         */
        public int process_new_base_independent(License_Type license_type)
        {
            szDXML[0] = null;
            szDXML[1] = null;
            string templatePath = null;

            Console.WriteLine(" Process base independent license:");

            // load license definition file
            license_template_number = 1;

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

                // load C2V file
                szC2V = load_file(szCurrentStateFilename);
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }

                templatePath = "input/SL-AdminMode/base_independent_template.xml";
                szV2CFilename = "output/SL-AdminMode/new_base_independent_license.v2c";
                szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new_base_independent.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v";

                // load C2V file
                szC2V = load_file(szCurrentStateFilename);
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-UserMode/fingerprint.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }

                templatePath = "input/SL-UserMode/base_independent_template.xml";
                szV2CFilename = "output/SL-UserMode/new_base_independent_license.v2c";
                szUXMLFilename = "output/SL-UserMode/resultant_state_after_new_base_independent.xml";
            }
            else
            {
                ;//do nothing
            }

            szDXML[0] = load_file(templatePath);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading base_independent_template.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/new_base_independent_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/new_base_independent_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * decodes the current state
         */
        public int process_decode_current_state(License_Type license_type)
        {
            sntl_lg_status_t status = 0;
            string c2vFilePath = null;
            string szInitParamXML = null;
            string readable_state = null;
            string decode_XML_Path = null;

            Console.WriteLine(" Process decode current state:");

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                c2vFilePath = "input/HL/original_state.c2v";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                c2vFilePath = "input/SL-AdminMode/original_state.c2v";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                c2vFilePath = "input/SL-UserMode/original_state.c2v";
            }
            else
            {
                ;//do nothing
            }

            // load c2v file
            szC2V = load_file(c2vFilePath);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading original_state.c2v file.");
                Console.WriteLine("");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            //
            // sntl_lg_initialize
            // Initializes license generation library
            // and returns handle to work further
            //
            status = licGenHelp.sntl_lg_initialize(szInitParamXML);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_initialize: " + error_msg);
            if (status != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_DECODE_STATE_FAILED;
            }

            //
            // sntl_lg_decode_current_state
            // Decodes the current state
            //
            status = licGenHelp.sntl_lg_decode_current_state(vendorCode, szC2V, ref  readable_state);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_decode_current_state: " + error_msg);
            if (status != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_DECODE_STATE_FAILED;
            }

            //load key_type and key_configuration from key readable state
            load_key_type_and_config(readable_state);

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                decode_XML_Path = "output/HL/decoded_current_state.xml";

                // Check for dynamic memory supported key
                if (!string.IsNullOrEmpty(szKey_configuration))
                {
                    if (szKey_configuration.Contains("driverless"))
                    {
                        if (szKey_type != "Sentinel-HL-Pro" &&
                        szKey_type != "Sentinel-HL-Basic")
                        {
                            dynamic_memory_supported_key = true;
                        }
                    }
                }
                if (dynamic_memory_supported_key == false)
                {
                    Console.WriteLine("  This key doesn't support dynamic memory!");
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                decode_XML_Path = "output/SL-AdminMode/decoded_current_state.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                decode_XML_Path = "output/SL-UserMode/decoded_current_state.xml";
            }
            else
            {
                ;//do nothing
            }

            status = (sntl_lg_status_t)store_file(readable_state, decode_XML_Path);
            if (status != 0)
            {
                Console.WriteLine("  decoded current state file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            //Clean up memories that used in the routine
            licGenHelp.sntl_lg_cleanup();

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  Decoded current state file \"output/HL/decoded_current_state.xml\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  Decoded current state file \"output/SL-AdminMode/decoded_current_state.xml\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  Decoded current state file \"output/SL-UserMode/decoded_current_state.xml\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
          * load key-type and key-configuration
          */
        public void load_key_type_and_config(string key_readable_state)
        {
            szKey_type = "";
            szKey_configuration = "";
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(key_readable_state)))
            {
                // fetching key-type
                if (reader.ReadToFollowing("type"))
                {
                    szKey_type = reader.ReadElementContentAsString();
                }

                // fetching key-configuration
                if (reader.ReadToFollowing("configuration_info"))
                {
                    do
                    {
                        reader.Read();
                        if (reader.IsEmptyElement)
                        {
                            if (reader.Name.Equals("hasphl") ||
                                reader.Name.Equals("sentinelhl") ||
                                reader.Name.Equals("driverless"))
                            {
                                szKey_configuration += reader.Name;
                                szKey_configuration += " ";
                            }
                        }
                    } while (!reader.Name.Equals("configuration_info"));
                }
            }
        }

        /**
         * generate clear key license
         */
        public int process_clear_key(License_Type license_type)
        {
            sntl_lg_status_t status;

            String szInitScopeXML = null;
            String szStartScopeXML = null;
            String szLicenseDefinitionXML = null;
            String szGenerationScopeXML = null;

            string license = null;
            string resultant_state = null;

            Console.WriteLine(" Process clear license:");

            // load c2v file
            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                szC2V = load_file("input/HL/original_state.c2v");
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/HL/original_state.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                szC2V = load_file("input/SL-AdminMode/original_state.c2v");
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-AdminMode/original_state.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                szC2V = load_file("input/SL-UserMode/original_state.c2v");
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-UserMode/original_state.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
            }
            else
            {
                ;//do nothing
            }

            //
            // sntl_lg_initialize
            // Initializes license generation library
            // and returns handle to work further
            //
            status = licGenHelp.sntl_lg_initialize(szInitScopeXML);

            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref error_msg);
            Console.WriteLine("  sntl_lg_initialize: " + error_msg);
            if (status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_INITIALIZE_FAILED;
            }

            //
            // sntl_lg_start
            // Starts the license generation.
            //
            status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_CLEAR_AND_UPDATE, szLicenseDefinitionXML, szC2V);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_start: " + error_msg);
            if (status != sntl_lg_status_t.SNTL_LG_STATUS_OK)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_START_FAILED;
            }

            //
            // sntl_lg_generate_license
            // Generates the license.
            //
            status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, ref  license, ref  resultant_state);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_generate_license: " + error_msg);
            if ((status != sntl_lg_status_t.SNTL_LG_STATUS_OK) && (status != sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE))
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (status == sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE)
            {
                nothing_to_clear = 1;
                Console.WriteLine("");
                licGenHelp.sntl_lg_cleanup();
                return 0;
            }

            string licensePath = null;
            string statePath = null;

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                licensePath = "output/HL/clear_license.v2c";
                statePath = "output/HL/resultant_state_after_clear.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                licensePath = "output/SL-AdminMode/clear_license.v2c";
                statePath = "output/SL-AdminMode/resultant_state_after_clear.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                licensePath = "output/SL-UserMode/clear_license.v2c";
                statePath = "output/SL-UserMode/resultant_state_after_clear.xml";
            }
            else
            {
                ;//do nothing
            }
            status = (sntl_lg_status_t)store_file(license, licensePath);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                Console.WriteLine("  license file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = (sntl_lg_status_t)store_file(resultant_state, statePath);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                Console.WriteLine("  resultant license container state file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            //Clean up memories that used in the routine
            licGenHelp.sntl_lg_cleanup();

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  License file \"output/HL/clear_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/clear_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/clear_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * Generate format key license
         */
        public int process_format_key(License_Type license_type)
        {
            sntl_lg_status_t status;

            String szInitScopeXML = null;
            String szStartScopeXML = null;
            String szLicenseDefinitionXML = null;
            String szGenerationScopeXML = null;

            string license = "";
            string resultant_state = "";

            Console.WriteLine(" Process format license:");

            szC2V = null;

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                if (nothing_to_clear != 0)
                {
                    szCurrentStateFilename = "input/HL/original_state.c2v";
                }
                else
                {
                    szCurrentStateFilename = "output/HL/resultant_state_after_clear.xml";
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                if (nothing_to_clear != 0)
                {
                    szCurrentStateFilename = "input/SL-AdminMode/original_state.c2v";
                }
                else
                {
                    szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_clear.xml";
                }
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                if (nothing_to_clear != 0)
                {
                    szCurrentStateFilename = "input/SL-UserMode/original_state.c2v";
                }
                else
                {
                    szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_clear.xml";
                }
            }
            else
            {
                ;//do nothing
            }

            // load c2v file
            szC2V = load_file(szCurrentStateFilename);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading resultant_state_after_clear.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            //
            // sntl_lg_initialize
            // Initializes license generation library
            // and returns handle to work further
            //
            status = licGenHelp.sntl_lg_initialize(szInitScopeXML);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_initialize: " + error_msg);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_INITIALIZE_FAILED;
            }

            //
            // sntl_lg_start
            // Starts the license generation.
            //
            status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_FORMAT_AND_UPDATE, szLicenseDefinitionXML, szC2V);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_start: " + error_msg);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_START_FAILED;
            }

            //
            // sntl_lg_generate_license
            // Generates the license.
            //
            status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, ref  license, ref  resultant_state);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_generate_license: " + error_msg);
            if ((status != 0) && (status != sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE))
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (status == sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE)
            {
                nothing_to_format = 1;
                Console.WriteLine("");
                licGenHelp.sntl_lg_cleanup();
                return 0;
            }

            string licensePath = null;
            string statePath = null;

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                licensePath = "output/HL/format_license.v2c";
                statePath = "output/HL/resultant_state_after_format.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                licensePath = "output/SL-AdminMode/format_license.v2c";
                statePath = "output/SL-AdminMode/resultant_state_after_format.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                licensePath = "output/SL-UserMode/format_license.v2c";
                statePath = "output/SL-UserMode/resultant_state_after_format.xml";
            }
            else
            {
                ;//do nothing
            }

            status = (sntl_lg_status_t)store_file(license, licensePath);
            if (status != 0)
            {
                Console.WriteLine("  license file fail to save.");
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = (sntl_lg_status_t)store_file(resultant_state, statePath);
            if (status != 0)
            {
                Console.WriteLine("  resultant license container state file fail to save.");
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            //Clean up memories that used in the routine
            licGenHelp.sntl_lg_cleanup();

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  License file \"output/HL/format_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/format_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/format_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * process new license generation routine from original_state.c2v,
         * new_license_template1.xml and new_license_template2.xml
         *
         * @return 0 if success, other if failure
         */
        public int process_new(License_Type license_type)
        {
            szC2V = null;

            szDXML[0] = "";
            szDXML[1] = "";
            szDXML[2] = "";
            string xmlFilePath1 = null;
            string xmlFilePath2 = null;
            string xmlFilePath3 = null;

            Console.WriteLine(" Process new license:");

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                if ((nothing_to_format != 0) && (nothing_to_clear != 0))
                {
                    szCurrentStateFilename = "input/HL/original_state.c2v";
                }
                else if ((nothing_to_format != 0) && (nothing_to_clear == 0))
                {
                    szCurrentStateFilename = "output/HL/resultant_state_after_clear.xml";
                }
                else
                {
                    szCurrentStateFilename = "output/HL/resultant_state_after_format.xml";
                }

                xmlFilePath1 = "input/HL/new_license_template1.xml";
                xmlFilePath2 = "input/HL/new_license_template2.xml";
                xmlFilePath3 = "input/HL/new_license_template3.xml";

                szV2CFilename = "output/HL/new_license.v2c";
                szUXMLFilename = "output/HL/resultant_state_after_new.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                if ((nothing_to_format != 0) && (nothing_to_clear != 0))
                {
                    szCurrentStateFilename = "input/SL-AdminMode/original_state.c2v";
                }
                else if ((nothing_to_format != 0) && (nothing_to_clear == 0))
                {
                    szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_clear.xml";
                }
                else
                {
                    szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_format.xml";
                }

                xmlFilePath1 = "input/SL-AdminMode/new_license_template1.xml";
                xmlFilePath2 = "input/SL-AdminMode/new_license_template2.xml";

                szV2CFilename = "output/SL-AdminMode/new_license.v2c";
                szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                if ((nothing_to_format != 0) && (nothing_to_clear != 0))
                {
                    szCurrentStateFilename = "input/SL-UserMode/original_state.c2v";
                }
                else if ((nothing_to_format != 0) && (nothing_to_clear == 0))
                {
                    szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_clear.xml";
                }
                else
                {
                    szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_format.xml";
                }

                xmlFilePath1 = "input/SL-UserMode/new_license_template1.xml";
                xmlFilePath2 = "input/SL-UserMode/new_license_template2.xml";

                szV2CFilename = "output/SL-UserMode/new_license.v2c";
                szUXMLFilename = "output/SL-UserMode/resultant_state_after_new.xml";
            }
            else
            {
                ;//do nothing
            }

            // load C2V file
            szC2V = load_file(szCurrentStateFilename);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading resultant_state_after_format.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 2;
            szDXML[0] = load_file(xmlFilePath1);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading input/HL/new_license_template1.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szDXML[1] = load_file(xmlFilePath2);
            if (szDXML[1] == null)
            {
                Console.WriteLine("  error in loading input/HL/new_license_template2.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type &&
               dynamic_memory_supported_key == true)
            {
               szDXML[2] = load_file(xmlFilePath3);
               if (szDXML[2] == null)
               {
                  Console.WriteLine("  error in loading input/HL/new_license_template3.xml file.");
                  return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
               }
               license_template_number += 1;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }


            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  License file \"output/HL/new_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/new_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/new_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * process modify license generation routine from
         * resultant_state_after_new.xml and modify_license_template1.xml
         *
         * @return 0 if success, other if failure
         */
        public int process_modify(License_Type license_type)
        {
            szC2V = null;
            szDXML[0] = null;
            szDXML[1] = null;
            string c2vFilePath = null;
            string templatePath1 = null;
            string templatePath2 = null;

            Console.WriteLine(" Process modify license:");

            // load C2V file
            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                c2vFilePath = "output/HL/resultant_state_after_new.xml";
                templatePath1 = "input/HL/modify_license_template1.xml";
                templatePath2 = "input/HL/modify_license_template2.xml";

                szV2CFilename = "output/HL/modify_license.v2c";
                szUXMLFilename = "output/HL/resultant_state_after_modify.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                c2vFilePath = "output/SL-AdminMode/resultant_state_after_new.xml";
                templatePath1 = "input/SL-AdminMode/modify_license_template1.xml";

                szV2CFilename = "output/SL-AdminMode/modify_license.v2c";
                szUXMLFilename = "output/SL-AdminMode/resultant_state_after_modify.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                c2vFilePath = "output/SL-UserMode/resultant_state_after_new.xml";
                templatePath1 = "input/SL-UserMode/modify_license_template1.xml";

                szV2CFilename = "output/SL-UserMode/modify_license.v2c";
                szUXMLFilename = "output/SL-UserMode/resultant_state_after_modify.xml";
            }
            else
            {
                ;//do nothing
            }

            szC2V = load_file(c2vFilePath);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading resultant_state_after_new.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file(templatePath1);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading modify_license_template1.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type &&
               dynamic_memory_supported_key == true)
            {
               szDXML[1] = load_file(templatePath2);
               if (szDXML[1] == null)
               {
                  Console.WriteLine("  error in loading modify_license_template2.xml file.");
                  return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
               }
               license_template_number += 1;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  License file \"output/HL/modify_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/modify_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/modify_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * process cancel license generation routine from
         * resultant_state_after_modify.xml and cancel_license_template1.xml
         *
         * @return 0 if success, other if failure
         */
        public int process_cancel(License_Type license_type)
        {
            szC2V = null;
            szDXML[0] = null;
            szDXML[1] = null;
            string c2vFilePath = null;
            string templatePath1 = null;
            string templatePath2 = null;

            Console.WriteLine(" Process cancel license:");

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                c2vFilePath = "output/HL/resultant_state_after_modify.xml";
                templatePath1 = "input/HL/cancel_license_template1.xml";
                templatePath2 = "input/HL/cancel_license_template2.xml";

                szV2CFilename = "output/HL/cancel_license.v2c";
                szUXMLFilename = "output/HL/resultant_state_after_cancel.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                c2vFilePath = "output/SL-AdminMode/resultant_state_after_modify.xml";
                templatePath1 = "input/SL-AdminMode/cancel_license_template1.xml";

                szV2CFilename = "output/SL-AdminMode/cancel_license.v2c";
                szUXMLFilename = "output/SL-AdminMode/resultant_state_after_cancel.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                c2vFilePath = "output/SL-UserMode/resultant_state_after_modify.xml";
                templatePath1 = "input/SL-UserMode/cancel_license_template1.xml";

                szV2CFilename = "output/SL-UserMode/cancel_license.v2c";
                szUXMLFilename = "output/SL-UserMode/resultant_state_after_cancel.xml";
            }
            else
            {
                ;//do nothing
            }

            // load C2V file
            szC2V = load_file(c2vFilePath);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading resultant_state_after_modify.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file(templatePath1);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading input/HL/cancel_license_template1.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type &&
               dynamic_memory_supported_key == true)
            {
               szDXML[1] = load_file(templatePath2);
               if (szDXML[1] == null)
               {
                  Console.WriteLine("  error in loading input/HL/cancel_license_template2.xml file.");
                  return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
               }
               license_template_number += 1;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_HL == license_type)
            {
                Console.WriteLine("  License file \"output/HL/cancel_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/cancel_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/cancel_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * one license generation routine
         *
         * @return 0 if success, other if failure
         */
        public int generate_license()
        {
            int i = 0;
            sntl_lg_status_t status = 0;

            String szInitScopeXML = null;
            String szStartScopeXML = null;
            String szLicenseDefinitionXML = null;
            String szGenerationScopeXML = null;

            string license = "";
            String resultant_state = "";

            //sntl_lg_initialize Initializes license generation library
            status = licGenHelp.sntl_lg_initialize(szInitScopeXML);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_initialize: " + error_msg);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_INITIALIZE_FAILED;
            }

            //sntl_lg_start Starts the license generation.
            status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_UPDATE , szLicenseDefinitionXML, szC2V);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_start: " + error_msg);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_START_FAILED;
            }

            for (i = 0; i < license_template_number; i++)
            {
                /**
                 * sntl_lg_apply_template Apply license definition to the license
                 * state associated with the handle. You can call this API multiple
                 * times in one license generation routine.
                 */
                status = licGenHelp.sntl_lg_apply_template(szDXML[i]);
                licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
                Console.WriteLine("  sntl_lg_apply_template: " + error_msg);
                if (status != 0)
                {
                    licGenHelp.sntl_lg_cleanup();
                    return (int)Error_Msg.SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED;
                }
            }

            //sntl_lg_generate_license Generates the license.
            status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, ref license, ref resultant_state);
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, ref  error_msg);
            Console.WriteLine("  sntl_lg_generate_license: " + error_msg);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                return (int)Error_Msg.SNTL_LG_SAMPLE_START_FAILED;
            }

            status = (sntl_lg_status_t)store_file(license, szV2CFilename);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                Console.WriteLine("  license file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = (sntl_lg_status_t)store_file(resultant_state, szUXMLFilename);
            if (status != 0)
            {
                licGenHelp.sntl_lg_cleanup();
                Console.WriteLine("  resultant license container state file fail to save.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            //Clean up memories that used in the routine
            licGenHelp.sntl_lg_cleanup();

            return 0;
        }

        /**
         * process generation of base independent license that allows rehosting 
         *
         * @return 0 if success, other if failure
         */
        public int process_new_rehost(License_Type license_type)
        {
            szDXML[0] = null;
            szDXML[1] = null;
            string templatePath = null;

            Console.WriteLine(" Process base independent license that allows rehosting:");

            // load license definition file
            license_template_number = 1;

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

                // load C2V file
                szC2V = load_file(szCurrentStateFilename);
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }

                templatePath = "input/SL-AdminMode/rehost_license_template.xml";
                szV2CFilename = "output/SL-AdminMode/rehost_license.v2c";
                szUXMLFilename = "output/SL-AdminMode/resultant_state_after_rehost.xml";
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v";

                // load C2V file
                szC2V = load_file(szCurrentStateFilename);
                if (szC2V == null)
                {
                    Console.WriteLine("  error in loading input/SL-UserMode/fingerprint.c2v file.");
                    return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }

                templatePath = "input/SL-UserMode/rehost_license_template.xml";
                szV2CFilename = "output/SL-UserMode/rehost_license.v2c";
                szUXMLFilename = "output/SL-UserMode/resultant_state_after_rehost.xml";
            }
            else
            {
                ;//do nothing
            }

            szDXML[0] = load_file(templatePath);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading rehost_license_template.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-AdminMode/rehost_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else if (License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type)
            {
                Console.WriteLine("  License file \"output/SL-UserMode/rehost_license.v2c\" has been generated successfully.");
                Console.WriteLine("");
            }
            else
            {
                ;//do nothing
            }

            return 0;
        }

        /**
         * process generation of base independent detachable license 
         *
         * @return 0 if success, other if failure
         */
        public int process_new_detach(License_Type license_type)
        {
            szDXML[0] = null;
            szDXML[1] = null;
            string templatePath = null;

            Console.WriteLine(" Process base independent detachable license:");

            szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

            // load C2V file
            szC2V = load_file(szCurrentStateFilename);
            if (szC2V == null)
            {
                Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;

            templatePath = "input/SL-AdminMode/detach_license_template.xml";
            szV2CFilename = "output/SL-AdminMode/detach_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_detach.xml";

            szDXML[0] = load_file(templatePath);
            if (szDXML[0] == null)
            {
                Console.WriteLine("  error in loading input/SL-AdminMode/detach_license_template.xml file.");
                return (int)Error_Msg.SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            if (generate_license() != 0)
            {
                return (int)Error_Msg.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
            }

            Console.WriteLine("  License file \"output/SL-AdminMode/detach_license.v2c\" has been generated successfully.");
            Console.WriteLine("");

            return 0;
        }
    }
}
