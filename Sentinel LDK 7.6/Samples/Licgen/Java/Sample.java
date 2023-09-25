//
// Demo program for Sentinel LDK license generation API
//
// Copyright (C) 2014, SafeNet, Inc. All rights reserved.
//

import java.io.*;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.DocumentBuilder;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import org.w3c.dom.Node;
import com.sentinel.ldk.licgen.*;

public class Sample {

	//define the sample error value
    private int SNTL_LG_SAMPLE_DECODE_STATE_FAILED				= 1;
    private int SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED		= 2;   
    private int SNTL_LG_SAMPLE_GENERATE_NEW_FAILED				= 3;    
    private int SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED  			= 4;
    private int SNTL_LG_SAMPLE_LOAD_FILE_FAILED					= 5;
    private int SNTL_LG_SAMPLE_STORE_FILE_FAILED				= 6;
    private int SNTL_LG_SAMPLE_INITIALIZE_FAILED				= 7;
    private int SNTL_LG_SAMPLE_START_FAILED						= 8;

	//define the sample license type
    private int SNTL_LG_SAMPLE_LICENSE_HL			 			= 1;
    private int SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE 			= 2;
    private int SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE  			= 3;

	
    private static final String base64code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    public static final String vendorCode = new String(
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
            "ITB3mazzFiyQuKf4J6+b/a/Y");

    public String szC2V;
    public String[] szDXML;

    public String szV2CFilename = null;
    public String szUXMLFilename = null;
    public String szCurrentStateFilename = null;

    public int option;
    public int license_template_number;

    public int nothing_to_clear = 0;
    public int nothing_to_format = 0;
	
	public String szKey_type = null;
    public String szKey_configuration = null;
    public boolean dynamic_memory_supported_key = false;

    public LicGenAPIHelper oneinstance = null;

    public Sample()
    {
        szDXML = new String[3];
        oneinstance = new LicGenAPIHelper();
    }

    /**
     * use BASE64 to encode the input string
     *
     * @param input
     *            string that is ready for encode
     * @return encoded string or null when error
     */
    public static String base64_encode(byte[] input)
    {
        int length = 0;
        int pad_count = 0;

        byte[] input_string;

        StringBuilder output;

        length = input.length;
        if (length == 0) 
        {
            return null;
        }

        pad_count = (3 - (length % 3)) % 3;

        output = new StringBuilder((length + pad_count) / 3 * 4);

        input_string = new byte[length + pad_count];
        System.arraycopy(input, 0, input_string, 0, length);

        int i = 0;
        for (i = 0; i < length + pad_count; i += 3) 
        {
            int idx = 0;
            
            idx = (input_string[i] >> 2) & 0x3F;
            output.append(base64code.charAt(idx));
           
            idx = ((input_string[i] & 0x3) << 4) + ((input_string[i+1] >> 4) & 0x0F);
            output.append(base64code.charAt(idx));
            
            idx = ((input_string[i+1] & 0x0F) << 2) + ((input_string[i+2] >> 6) & 0x03);
            output.append(base64code.charAt(idx));
            
            idx = (input_string[i+2] & 0x3F);
            output.append(base64code.charAt(idx));            
        }

        if (pad_count > 0)
        {
            output.setCharAt(output.length() - 1, '=');
            pad_count--;

            if (pad_count > 0)
            {
                output.setCharAt(output.length() - 2, '=');
            }
        }

        return output.toString();
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
            return (byte) (input - 'A' + 0); // 0 range starts at 'A'
        }

        if ((input >= 'a') && (input <= 'z'))
        {
            return (byte) (input - 'a' + 26); // 26 range starts at 'a'
        }

        if ((input >= '0') && (input <= '9'))
        {
            return (byte) (input - '0' + 52); // 52 range starts at '0'
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

        length = input.length();

        // verify if input is a legal BASE64 string
        if ((length == 0) || (length % 4 != 0))
        {
            return null;
        }

        for (int i = 0; i < length; i++)
        {
            temp = base64_decode_char(input.charAt(i));
            if (temp == 64)
            {
                if ((i != (length - 1)) && (i != (length - 2))) 
                {
                    return null;
                }
                
                if (i == (length - 2)) 
                {
                    if (base64_decode_char(input.charAt(i + 1)) != 64)
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
                byte index = base64_decode_char(input.charAt(i + k));
                val |= index & 0x3f;
                val <<= 6;
            }
            val <<= 2;

            for (int l = 0; l < 3; l++) 
            {
                output_array[j + l] = (byte) (val >> 24);
                val <<= 8;
            }
        }

        output = new byte[length / 4 * 3 - pad_count];
        System.arraycopy(output_array, 0, output, 0, length / 4 * 3 - pad_count);

        return output;
    }

    /**
     * read content of file and return
     *
     * @param filename
     *            name of file to read
     * @return content of file
     */
    public String load_file(String filename) 
    {
        int size = 0;
        int bom_size = 3;
        byte[] temp;
        byte[] bom;

        try
        {
            RandomAccessFile f = new RandomAccessFile(filename, "r");
            size = (int) f.length();
            if (size > bom_size)
            {
                bom = new byte[bom_size];

                bom[0] = (byte)f.read();
                bom[1] = (byte)f.read();
                bom[2] = (byte)f.read();

                // Skip UTF-8 BOM (0xEF, 0xBB, 0xBF) if found
                if (bom[0] == -17 && bom[1] == -69 && bom[2] == -65)
                {
                    temp = new byte[size - bom_size];
                    f.read(temp);
                    f.close();
                    return new String(temp, "UTF-8");
                }
                else
                {
                    temp = new byte[size];
                    f.seek(0);
                    f.read(temp);
                    f.close();
                    return new String(temp);
                }
            }
        } 
        catch (IOException e) 
        {
            e.printStackTrace();
        }

        return null;
    }

    /**
     * save content of to file
     *
     * @param buffer
     *            content will be saved
     * @param filename
     *            name of file that content will be saved to
     * @return 0 if success, other if failure
     */
    public int store_file(String buffer, String filename) 
    {
        try {
        	 // Check for existence.
        	 File folder = new File("output");

             if( false == folder.exists() )
             {
                if ( true == folder.mkdir() )
                {
                	File folderHL = new File("output/HL");
                    if ( false == folderHL.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }

                    File folderAM = new File("output/SL-AdminMode");
                    if ( false == folderAM.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }

                    File folderUM = new File("output/SL-UserMode");
                    if ( false == folderUM.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }
                } 
                else
                {
                	System.out.print(" Create directory failed.\n");
                    return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                }
            }
            else
            {
            	File folderHL = new File("output/HL");
                if ( false == folderHL.exists() )
                {
                    if ( false == folderHL.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }
                }

                File folderAM = new File("output/SL-AdminMode");
                if ( false == folderAM.exists() )
                {
                    if ( false == folderAM.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }
                }

                File folderUM = new File("output/SL-UserMode");
                if ( false == folderUM.exists() )
                {
                    if ( false == folderUM.mkdir() )
                    {
                    	System.out.print(" Create directory failed.\n");
                        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
                    }
                }
            }
             
            File file = new File(filename);            
            if ( file.exists() )
            {
            	file.delete();           	
            }           
                        
            RandomAccessFile f = new RandomAccessFile(filename, "rw");           
            f.write(buffer.getBytes("UTF-8"));
            f.close();
            return 0;
        } 
        catch (IOException e)
        {
            e.printStackTrace();
        }

        return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
    }

    /**
     * show copyright information
     */
    public void show_copyright() 
    {
        System.out.print(" \n");
        System.out.print(" A demo program for the LDK license generation functions\n");
        System.out.print(" Copyright (c) 2014, SafeNet, Inc. All rights reserved.\n");
        System.out.print(" \n");
    }

    /**
     * show version information of license generation library
     */
    public void show_licgen_version() 
    {
        int status = 0;

        Int32Buffer major_version = new Int32Buffer();
        Int32Buffer minor_version = new Int32Buffer();
        Int32Buffer build_server = new Int32Buffer();
        Int32Buffer build_number = new Int32Buffer();

        status = LicGenAPIHelper.sntl_lg_get_version(major_version, minor_version, build_server, build_number);
        if (status != LicGenAPIHelper.SNTL_LG_STATUS_OK) 
        {
            return;
        }

        System.out.print(" \n");
        System.out.print(" Sentinel LDK License Generation Windows DLL " + major_version.get() + "." + minor_version.get() + " build " + build_number.get() + "\n");
        System.out.print(" \n");
    }
    
	 /**
     * Decodes the current state
     */
    public int process_decode_current_state(int license_type)
    {
        int status = 0;
       
	    String szInitParamXML = null;
        CharStringBuffer readable_state = new CharStringBuffer();

        CharStringBuffer error_msg = new CharStringBuffer();

        System.out.print(" Decode current state:\n");

        szC2V = null;

        // load c2v file
        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	szC2V = load_file("input/HL/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/HL/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	szC2V = load_file("input/SL-AdminMode/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/SL-AdminMode/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	szC2V = load_file("input/SL-UserMode/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/SL-UserMode/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
        }
        else
        {
            ;//do nothing
        }        

        try {
        //
        // sntl_lg_initialize
        // Initializes license generation library
        // and returns handle to work further
        //
        status = oneinstance.sntl_lg_initialize(szInitParamXML);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_initialize: " + error_msg.get() + "\n");
        if (status != 0) 
        {
            return SNTL_LG_SAMPLE_DECODE_STATE_FAILED;
        }

        //
        // sntl_lg_decode_current_state
        // Decodes the current state
        //
        status = oneinstance.sntl_lg_decode_current_state(vendorCode, szC2V, readable_state);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_decode_current_state: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_DECODE_STATE_FAILED;
        }
        } // try block ends here
        catch(java.io.UnsupportedEncodingException e)
        {
            System.out.print(e.getMessage() + "\n");
            return SNTL_LG_SAMPLE_DECODE_STATE_FAILED;
        }
        
        // load key_type and key_configuration from key readable state
        load_key_type_and_config(readable_state.get());
		
        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	status = store_file(readable_state.get(), "output/HL/decoded_current_state.xml");
            if (status != 0)
            {
                System.out.print("  output/HL/decoded current state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            } 
			
            // Check for dynamic memory support
			if (szKey_configuration != null && !("".equals(szKey_configuration)))
            {
				if (szKey_configuration.contains("driverless"))
                {
                    if (szKey_type != "Sentinel-HL-Pro" && szKey_type != "Sentinel-HL-Basic")
                    {
                        dynamic_memory_supported_key = true;
                    }
                }
            }
			if (dynamic_memory_supported_key == false)
            {
                System.out.print("  This key doesn't support dynamic memory!\n");
            }
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	status = store_file(readable_state.get(), "output/SL-AdminMode/decoded_current_state.xml");
            if (status != 0)
            {
                System.out.print("  output/SL-AdminMode/decoded current state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE== license_type )
        {
        	status = store_file(readable_state.get(), "output/SL-UserMode/decoded_current_state.xml");
            if (status != 0)
            {
                System.out.print("  output/SL-UserMode/decoded current state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }            
        }
        else
        {
            ;//do nothing
        }
        
        /**
         * Clean up memories that used in the routine
         */
        oneinstance.sntl_lg_cleanup();

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  Decoded current state file \"output/HL/decoded_current_state.xml\" has been generated successfully.\n\n");
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  Decoded current state file \"output/SL-AdminMode/decoded_current_state.xml\" has been generated successfully.\n\n");
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  Decoded current state file \"output/SL-UserMode/decoded_current_state.xml\" has been generated successfully.\n\n");
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
    public void load_key_type_and_config(String key_readable_state)
    {
		try
		{
		szKey_type = "";
		szKey_configuration = "";
		
		// convert String into InputStream
		InputStream readable_state = new ByteArrayInputStream(key_readable_state.getBytes());
		
		DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
		dbFactory.setCoalescing(true);
		dbFactory.setNamespaceAware(false);
		dbFactory.setValidating(false);
		dbFactory.setIgnoringElementContentWhitespace(true);
		
		DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
		Document doc = dBuilder.parse(readable_state);
		doc.getDocumentElement().normalize();
 
		// Loading Key Type
		NodeList nList = doc.getElementsByTagName("type");
		if (nList.getLength() != 0)
		{
			Node key_type_node = nList.item(0);
			szKey_type = key_type_node.getTextContent();
		}
	
		// Loading Key Configuration
		nList = doc.getElementsByTagName("configuration_info");
		if (nList.getLength() != 0)
		{
			Node config_info_node = nList.item(0);
			if (config_info_node != null && config_info_node.hasChildNodes())
			{
				NodeList child_node_list = config_info_node.getChildNodes();
				for (int i = 0; i < child_node_list.getLength(); i++)
				{
					if(child_node_list.item(i).getNodeName() == "hasphl" ||
					child_node_list.item(i).getNodeName() == "sentinelhl" ||
					child_node_list.item(i).getNodeName() == "driverless")
					{
						szKey_configuration += child_node_list.item(i).getNodeName();
						szKey_configuration += " ";
					}
				}
			}
		}
		} // End of try block
		
		catch (Exception e)
		{
			e.printStackTrace();
		}
	}
    
    /**
     * generate provisional license
     */
    public int generate_provisional_license(int license_type)
    {
        int status = 0;

        String szInitParamXML = null;
        String szStartParamXML = null;
        String szLicenseDefinitionXML = null;
        String szGenerationParamXML = null;
        String szC2V = null;

        CharStringBuffer license = new CharStringBuffer();
        CharStringBuffer resultant_state = new CharStringBuffer();

        CharStringBuffer error_msg = new CharStringBuffer();

        System.out.print(" Process provisional license:\n");
        
        if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	szLicenseDefinitionXML = load_file("input/SL-AdminMode/provisional_template.xml");
            if (szLicenseDefinitionXML == null)
            {
                System.out.print("  error in loading input/SL-AdminMode/provisional_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }           
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	szLicenseDefinitionXML = load_file("input/SL-UserMode/provisional_template.xml");
            if (szLicenseDefinitionXML == null)
            {
                System.out.print("  error in loading input/SL-UserMode/provisional_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }            
        }
        else
        {
            ;//do nothing
        }        

        try
        {
        //
        // sntl_lg_initialize
        // Initializes license generation library
        // and returns handle to work further
        //
        status = oneinstance.sntl_lg_initialize(szInitParamXML);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_initialize: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
        }

        //
        // sntl_lg_start
        // Starts the license generation.
        //
        status = oneinstance.sntl_lg_start(szStartParamXML, vendorCode, LicGenAPIHelper.SNTL_LG_LICENSE_TYPE_PROVISIONAL, szLicenseDefinitionXML, szC2V);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_start: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
        }

        //
        // sntl_lg_generate_license
        // Generates the license.
        //
        status = oneinstance.sntl_lg_generate_license(szGenerationParamXML, license, resultant_state);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_generate_license: " + error_msg.get() + "\n");
        if ((status != 0) &&  (status != LicGenAPIHelper.SNTL_LG_NOTHING_TO_GENERATE))
        {
            return SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
        }
        } // try block ends here
        catch(java.io.UnsupportedEncodingException e)
        {
            System.out.print(e.getMessage() + "\n");
            return SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	status = store_file(license.get(), "output/SL-AdminMode/provisional_license.v2c");
            if (status != 0)
            {
                System.out.print("  output/SL-AdminMode/provisional license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }         
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	status = store_file(license.get(), "output/SL-UserMode/provisional_license.v2c");
            if (status != 0)
            {
                System.out.print("  output/SL-UserMode/provisional license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }       
        }
        else
        {
            ;//do nothing
        }
        
        /**
         * Clean up memories that used in the routine
         */
        oneinstance.sntl_lg_cleanup();

        if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/provisional_license.v2c\" has been generated successfully.\n\n");            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/provisional_license.v2c\" has been generated successfully.\n\n");
        }
        else
        {
            ;//do nothing
        }        

        return 0;
    }
	
    /**
     * generate clear key license
     */
    public int process_clear_key(int license_type)
    {
        int status = 0;

        String szInitParamXML = null;
        String szStartParamXML = null;
        String szLicenseDefinitionXML = null;
        String szGenerationParamXML = null;

        CharStringBuffer license = new CharStringBuffer();
        CharStringBuffer resultant_state = new CharStringBuffer();

        CharStringBuffer error_msg = new CharStringBuffer();

        System.out.print(" Process clear license:\n");

        szC2V = null;

        // load c2v file
        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	szC2V = load_file("input/HL/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/HL/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	szC2V = load_file("input/SL-AdminMode/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/SL-AdminMode/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	szC2V = load_file("input/SL-UserMode/original_state.c2v");
            if (szC2V == null)
            {
                System.out.print("  error in loading input/SL-UserMode/original_state.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }            
        }
        else
        {
            ;//do nothing
        }
        
        try
        {
        //
        // sntl_lg_initialize
        // Initializes license generation library
        // and returns handle to work further
        //
        status = oneinstance.sntl_lg_initialize(szInitParamXML);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_initialize: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_INITIALIZE_FAILED;
        }

        //
        // sntl_lg_start
        // Starts the license generation.
        //
        status = oneinstance.sntl_lg_start(szStartParamXML, vendorCode, LicGenAPIHelper.SNTL_LG_LICENSE_TYPE_CLEAR_AND_UPDATE, szLicenseDefinitionXML, szC2V);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_start: " + error_msg.get() + "\n");
        if (status != 0) 
        {
            return SNTL_LG_SAMPLE_START_FAILED;
        }

        //
        // sntl_lg_generate_license
        // Generates the license.
        //
        status = oneinstance.sntl_lg_generate_license(szGenerationParamXML, license, resultant_state);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_generate_license: " + error_msg.get() + "\n");
        if ((status != 0) &&  (status != LicGenAPIHelper.SNTL_LG_NOTHING_TO_GENERATE)) 
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }
        } // try block ends here
        catch(java.io.UnsupportedEncodingException e)
        {
            System.out.print(e.getMessage() + "\n");
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if (status == LicGenAPIHelper.SNTL_LG_NOTHING_TO_GENERATE)
        {
            nothing_to_clear = 1;
            System.out.print("\n");
            return 0;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	 status = store_file(license.get(), "output/HL/clear_license.v2c");
             if (status != 0)
             {
                 System.out.print("  license file fail to save.\n");
                 return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
             }

             status = store_file(resultant_state.get(), "output/HL/resultant_state_after_clear.xml");
             if (status != 0)
             {
                 System.out.print("  resultant license container state file fail to save.\n");
                 return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
             }      
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	 status = store_file(license.get(), "output/SL-AdminMode/clear_license.v2c");
             if (status != 0)
             {
                 System.out.print("  license file fail to save.\n");
                 return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
             }

             status = store_file(resultant_state.get(), "output/SL-AdminMode/resultant_state_after_clear.xml");
             if (status != 0)
             {
                 System.out.print("  resultant license container state file fail to save.\n");
                 return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
             }            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	status = store_file(license.get(), "output/SL-UserMode/clear_license.v2c");
            if (status != 0)
            {
                System.out.print("  license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = store_file(resultant_state.get(), "output/SL-UserMode/resultant_state_after_clear.xml");
            if (status != 0)
            {
                System.out.print("  resultant license container state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }
        }
        else
        {
            ;//do nothing
        }
       
        /**
         * Clean up memories that used in the routine
         */
        oneinstance.sntl_lg_cleanup();

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  License file \"output/HL/clear_license.v2c\" has been generated successfully.\n\n");
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/clear_license.v2c\" has been generated successfully.\n\n");
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/clear_license.v2c\" has been generated successfully.\n\n");
        }
        else
        {
            ;//do nothing
        }        

        return 0;
    }

    /**
     * generate format key license
     */
    public int process_format_key(int license_type)
    {
        int status = 0;

        String szInitParamXML = null;
        String szStartParamXML = null;
        String szLicenseDefinitionXML = null;
        String szGenerationParamXML = null;

        CharStringBuffer license = new CharStringBuffer();
        CharStringBuffer resultant_state = new CharStringBuffer();

        CharStringBuffer error_msg = new CharStringBuffer();

        System.out.print(" Process format license:\n");

        szC2V = null;

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
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
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
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
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
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
            System.out.print("  error in loading resultant_state_after_clear.xml file.\n");
            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
        }

        try
        {
        //
        // sntl_lg_initialize
        // Initializes license generation library
        // and returns handle to work further
        //
        status = oneinstance.sntl_lg_initialize(szInitParamXML);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_initialize: " + error_msg.get() + "\n");
        if (status != 0) 
        {
            return SNTL_LG_SAMPLE_INITIALIZE_FAILED;
        }

        //
        // sntl_lg_start
        // Starts the license generation.
        //
        status = oneinstance.sntl_lg_start(szStartParamXML, vendorCode, LicGenAPIHelper.SNTL_LG_LICENSE_TYPE_FORMAT_AND_UPDATE, szLicenseDefinitionXML, szC2V);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_start: " + error_msg.get() + "\n");
        if (status != 0) 
        {
            return SNTL_LG_SAMPLE_START_FAILED;
        }

        //
        // sntl_lg_generate_license
        // Generates the license.
        //
        status = oneinstance.sntl_lg_generate_license(szGenerationParamXML, license, resultant_state);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_generate_license: " + error_msg.get() + "\n");
        if ((status != 0) &&  (status != LicGenAPIHelper.SNTL_LG_NOTHING_TO_GENERATE))
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }
        } // try block ends here
        catch(java.io.UnsupportedEncodingException e)
        {
            System.out.print(e.getMessage() + "\n");
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if (status == LicGenAPIHelper.SNTL_LG_NOTHING_TO_GENERATE) 
        {
            nothing_to_format = 1;
            System.out.print("\n");
            return 0;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	status = store_file(license.get(), "output/HL/format_license.v2c");
            if (status != 0)
            {
                System.out.print("  license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = store_file(resultant_state.get(), "output/HL/resultant_state_after_format.xml");
            if (status != 0) 
            {
                System.out.print("  resultant license container state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	status = store_file(license.get(), "output/SL-AdminMode/format_license.v2c");
            if (status != 0)
            {
                System.out.print("  license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = store_file(resultant_state.get(), "output/SL-AdminMode/resultant_state_after_format.xml");
            if (status != 0) 
            {
                System.out.print("  resultant license container state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	status = store_file(license.get(), "output/SL-UserMode/format_license.v2c");
            if (status != 0)
            {
                System.out.print("  license file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }

            status = store_file(resultant_state.get(), "output/SL-UserMode/resultant_state_after_format.xml");
            if (status != 0) 
            {
                System.out.print("  resultant license container state file fail to save.\n");
                return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
            }
        }
        else
        {
            ;//do nothing
        }

        /**
         * Clean up memories that used in the routine
         */
        oneinstance.sntl_lg_cleanup();

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  License file \"output/HL/format_license.v2c\" has been generated successfully.\n\n");            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/format_license.v2c\" has been generated successfully.\n\n");            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/format_license.v2c\" has been generated successfully.\n\n");
        }
        else
        {
            ;//do nothing
        }

        return 0;
    }
    
    public void show_help()
    {
    	System.out.print("\n\n");
        System.out.print(" Command line parameter usage of this program:\n");
        System.out.print("     -h        Display this help message and exit.\n");
        System.out.print("     -c        [default] Generate new, modify and cancel licenses.\n");
        System.out.print("     -m        Generate new and modify licenses.\n");
        System.out.print("     -n        Generate new license only.\n");
        System.out.print("\n\n");
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
        int length = args.length;
        
        if (length == 0)
        {
            option = 'c';
            return 0;
        }

        if (length > 1)
        {
        	System.out.print(" Error: unrecongnized or incomplete command line.\n");
        	show_help();
            return 1;
        }

        if (args[0].equals("-c"))
        {
            option = 'c';
            return 0;
        }

        if (args[0].equals("-m"))
        {
            option = 'm';
            return 0;
        }

        if (args[0].equals("-n")) 
        {
            option = 'n';
            return 0;
        }
        
        if (!args[0].equals("-h")) 
        {
        	System.out.print(" Error: unrecongnized or incomplete command line.\n");
        }
        
        show_help();
        return 2;
    }

    /**
     * process new license generation routine from original_state.c2v,
     * new_license_template1.xml and new_license_template2.xml
     *
     * @return 0 if success, other if failure
     */
    public int process_new(int license_type) 
    {
        szC2V = null;
        szDXML[0] = null;
        szDXML[1] = null;
		szDXML[2] = null;

        System.out.print(" Process new license:\n");

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	 if ( (nothing_to_format != 0) && (nothing_to_clear != 0) )
        	 {
                 szCurrentStateFilename = "input/HL/original_state.c2v";
             }
             else if ( (nothing_to_format != 0) && (nothing_to_clear == 0) )
             {
                 szCurrentStateFilename = "output/HL/resultant_state_after_clear.xml";
             }
             else
             {
                 szCurrentStateFilename = "output/HL/resultant_state_after_format.xml";
             }          
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	if ( (nothing_to_format != 0) && (nothing_to_clear != 0) )
       	 	{
                szCurrentStateFilename = "input/SL-AdminMode/original_state.c2v";
            }
            else if ( (nothing_to_format != 0) && (nothing_to_clear == 0) )
            {
                szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_clear.xml";
            }
            else
            {
                szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_format.xml";
            }      
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	if ( (nothing_to_format != 0) && (nothing_to_clear != 0) )
       	 	{
                szCurrentStateFilename = "input/SL-UserMode/original_state.c2v";
            }
            else if ( (nothing_to_format != 0) && (nothing_to_clear == 0) )
            {
                szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_clear.xml";
            }
            else
            {
                szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_format.xml";
            }
        }
        else
        {
            ;//do nothing
        }
       
        // load C2V file
        szC2V = load_file(szCurrentStateFilename);
        if (szC2V == null) 
        {
            System.out.print("  error in loading resultant_state_after_format.xml file.\n");
            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
        }

        // load license definition file
        license_template_number = 2;
        
        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	 szDXML[0] = load_file("input/HL/new_license_template1.xml");
             if ( szDXML[0] == null )
             {
                 System.out.print("  error in loading input/HL/new_license_template1.xml file.\n");
                 return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
             }

             szDXML[1] = load_file("input/HL/new_license_template2.xml");
             if ( szDXML[1] == null ) 
             {
                 System.out.print("  error in loading input/HL/new_license_template2.xml file.\n");
                 return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
             }

             if (dynamic_memory_supported_key == true)
             {
				 szDXML[2] = load_file("input/HL/new_license_template3.xml");
                 if (szDXML[2] == null)
                 {
                     System.out.print("  error in loading input/HL/new_license_template3.xml file.\n");
                     return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                 }
                 license_template_number += 1;
             }

             szV2CFilename = "output/HL/new_license.v2c";
             szUXMLFilename = "output/HL/resultant_state_after_new.xml";
            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	szDXML[0] = load_file("input/SL-AdminMode/new_license_template1.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-AdminMode/new_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szDXML[1] = load_file("input/SL-AdminMode/new_license_template2.xml");
            if ( szDXML[1] == null ) 
            {
                System.out.print("  error in loading input/SL-AdminMode/new_license_template2.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-AdminMode/new_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new.xml";
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	szDXML[0] = load_file("input/SL-UserMode/new_license_template1.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-UserMode/new_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szDXML[1] = load_file("input/SL-UserMode/new_license_template2.xml");
            if ( szDXML[1] == null ) 
            {
                System.out.print("  error in loading input/SL-UserMode/new_license_template2.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-UserMode/new_license.v2c";
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_new.xml";	
        }
        else
        {
            ;//do nothing
        }
       
        if (generate_license() != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  License file \"output/HL/new_license.v2c\" has been generated successfully.\n\n");
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/new_license.v2c\" has been generated successfully.\n\n");
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/new_license.v2c\" has been generated successfully.\n\n");
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
    int process_modify(int license_type)
    {
        szC2V = null;
        szDXML[0] = null;
		szDXML[1] = null;

        System.out.print(" Process modify license:\n");

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/HL/resultant_state_after_new.xml");
            if (szC2V == null)
            {
                System.out.print("  error in loading output/HL/resultant_state_after_new.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/HL/modify_license_template1.xml");
            if (szDXML[0] == null)
            {
                System.out.print("  error in loading input/HL/modify_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
			
			if (dynamic_memory_supported_key == true)
            {
                szDXML[1] = load_file("input/HL/modify_license_template2.xml");
                if (szDXML[1] == null)
                {
                   System.out.print("  error in loading modify_license_template2.xml file.\n");
                   return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
                license_template_number += 1;
            }

            szV2CFilename = "output/HL/modify_license.v2c";
            szUXMLFilename = "output/HL/resultant_state_after_modify.xml";
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/SL-AdminMode/resultant_state_after_new.xml");
            if (szC2V == null)
            {
                System.out.print("  error in loading output/SL-AdminMode/resultant_state_after_new.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/SL-AdminMode/modify_license_template1.xml");
            if (szDXML[0] == null)
            {
                System.out.print("  error in loading input/SL-AdminMode/modify_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-AdminMode/modify_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_modify.xml";
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/SL-UserMode/resultant_state_after_new.xml");
            if (szC2V == null)
            {
                System.out.print("  error in loading output/SL-UserMode/resultant_state_after_new.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/SL-UserMode/modify_license_template1.xml");
            if (szDXML[0] == null)
            {
                System.out.print("  error in loading input/SL-UserMode/modify_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-UserMode/modify_license.v2c";
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_modify.xml";
        }
        else
        {
            ;//do nothing
        }        

        if (generate_license() != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  License file \"output/HL/modify_license.v2c\" has been generated successfully.\n\n"); 
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/modify_license.v2c\" has been generated successfully.\n\n");            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/modify_license.v2c\" has been generated successfully.\n\n");
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
    int process_cancel(int license_type) 
    {
        szC2V = null;
        szDXML[0] = null;
		szDXML[1] = null;

        System.out.print(" Process cancel license:\n");

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/HL/resultant_state_after_modify.xml");
            if ( szC2V == null )
            {
                System.out.print("  error in loading output/HL/resultant_state_after_modify.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/HL/cancel_license_template1.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/HL/cancel_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
			if (dynamic_memory_supported_key == true)
            {
                szDXML[1] = load_file("input/HL/cancel_license_template2.xml");
                if (szDXML[1] == null)
                {
                   System.out.print("  error in loading input/HL/cancel_license_template2.xml file.\n");
                   return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
                }
                license_template_number += 1;
            }

            szV2CFilename = "output/HL/cancel_license.v2c";
            szUXMLFilename = "output/HL/resultant_state_after_cancel.xml";
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/SL-AdminMode/resultant_state_after_modify.xml");
            if ( szC2V == null )
            {
                System.out.print("  error in loading output/SL-AdminMode/resultant_state_after_modify.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/SL-AdminMode/cancel_license_template1.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-AdminMode/cancel_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-AdminMode/cancel_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_cancel.xml";
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	// load C2V file
            szC2V = load_file("output/SL-UserMode/resultant_state_after_modify.xml");
            if ( szC2V == null )
            {
                System.out.print("  error in loading output/SL-UserMode/resultant_state_after_modify.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            // load license definition file
            license_template_number = 1;
            szDXML[0] = load_file("input/SL-UserMode/cancel_license_template1.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-UserMode/cancel_license_template1.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-UserMode/cancel_license.v2c";
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_cancel.xml";
        }
        else
        {
            ;//do nothing
        }     

        if (generate_license() != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
        {
        	System.out.print("  License file \"output/HL/cancel_license.v2c\" has been generated successfully.\n\n");            
        } 
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/cancel_license.v2c\" has been generated successfully.\n\n");            
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/cancel_license.v2c\" has been generated successfully.\n\n");
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
    int generate_license() 
    {
        int i = 0;
        int status = 0;

        String szInitParamXML = null;
        String szStartParamXML = null;
        String szLicenseDefinitionXML = null;
        String szGenerationParamXML = null;

        CharStringBuffer license = new CharStringBuffer();
        CharStringBuffer resultant_state = new CharStringBuffer();

        CharStringBuffer error_msg = new CharStringBuffer();

        try
        {
        /**
         * sntl_lg_initialize Initializes license generation library
         */
        status = oneinstance.sntl_lg_initialize(szInitParamXML);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_initialize: " + error_msg.get() + "\n");
        if ( status != 0 ) 
        {
            return SNTL_LG_SAMPLE_INITIALIZE_FAILED;
        }

        /**
         * sntl_lg_start Starts the license generation.
         */
        status = oneinstance.sntl_lg_start(szStartParamXML, vendorCode, LicGenAPIHelper.SNTL_LG_LICENSE_TYPE_UPDATE , szLicenseDefinitionXML, szC2V);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_start: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_START_FAILED;
        }

        for ( i = 0; i < license_template_number; i++ )
        {
            /**
             * sntl_lg_apply_template Apply license definition to the license
             * state associated with the handle. You can call this API multiple
             * times in one license generation routine.
             */
            status = oneinstance.sntl_lg_apply_template(szDXML[i]);
            oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
            System.out.print("  sntl_lg_apply_template: " + error_msg.get() + "\n");
            if (status != 0)
            {
                return SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED;
            }
        }

        /**
         * sntl_lg_generate_license Generates the license.
         */
        status = oneinstance.sntl_lg_generate_license(szGenerationParamXML, license, resultant_state);
        oneinstance.sntl_lg_get_info(LicGenAPIHelper.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg);
        System.out.print("  sntl_lg_generate_license: " + error_msg.get() + "\n");
        if (status != 0)
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }
        } // try block ends here
        catch(java.io.UnsupportedEncodingException e)
        {
            System.out.print(e.getMessage() + "\n");
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        status = store_file(license.get(), szV2CFilename);
        if (status != 0)
        {
            System.out.print("  license file fail to save.\n");
            return SNTL_LG_SAMPLE_STORE_FILE_FAILED;
        }

        status = store_file(resultant_state.get(), szUXMLFilename);
        if (status != 0)
        {
            System.out.print("  resultant license container state file fail to save.\n");
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        /**
         * Clean up memories that used in the routine
         */
        oneinstance.sntl_lg_cleanup();

        return 0;
    }
    
    /**
     * demonstrate the license type
     */
    public void show_sample_license_type(int license_type)
    {
    	if ( SNTL_LG_SAMPLE_LICENSE_HL == license_type )
    	{
    		System.out.print(" \n");
            System.out.print(" The following is for Sentinel LDK HL key type\n");        
            System.out.print(" \n");    	
    	}
    	else if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
    	{
    		System.out.print(" \n");
	        System.out.print(" The following is for Sentinel LDK SL Admin Mode key type\n");
	        System.out.print(" \n");    		
    	}
    	else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
    	{
    		System.out.print(" \n");
            System.out.print(" The following is for Sentinel LDK SL User Mode key type\n");
            System.out.print(" \n");    		
    	}
    	else
    	{
    		;//do nothing
    	}
    }    
   
	 /**
	  * process generation of base independent license
	  *
	  * @return 0 if success, other if failure
	  */
	int process_new_base_independent(int license_type) 
	{
		szDXML[0] = null;
	    szDXML[1] = null;

	    System.out.print(" Process base independent license:\n");    

	    // load license definition file
	    license_template_number = 1;
	  
	    if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
	    {
	    	szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

		    // load C2V file
	        szC2V = load_file(szCurrentStateFilename);
	        if (szC2V == null) 
	        {
	            System.out.print("  error in loading input/SL-AdminMode/fingerprint.c2v file.\n");
	            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
	        }
	        
	    	szDXML[0] = load_file("input/SL-AdminMode/base_independent_template.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-AdminMode/base_independent_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-AdminMode/new_base_independent_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new_base_independent.xml";
	    }
	    else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
	    {
	    	szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v";

		    // load C2V file
	        szC2V = load_file(szCurrentStateFilename);
	        if (szC2V == null) 
	        {
	            System.out.print("  error in loading input/SL-UserMode/fingerprint.c2v file.\n");
	            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
	        }
	        
	    	szDXML[0] = load_file("input/SL-UserMode/base_independent_template.xml");
	    	if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-UserMode/base_independent_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

	    	szV2CFilename = "output/SL-UserMode/new_base_independent_license.v2c";
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_new_base_independent.xml";
	    }
	    else
	    {
	        ;//do nothing
	    }
	    
	    if ( generate_license() != 0 )
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }
	    
	    if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-AdminMode/new_base_independent_license.v2c\" has been generated successfully.\n\n");           
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	System.out.print("  License file \"output/SL-UserMode/new_base_independent_license.v2c\" has been generated successfully.\n\n");
        }
        else
        {
            ;//do nothing
        }
	
	    return 0;
	}
	
	/**
	  * process generation of base independent license that allows rehosting
	  *
	  * @return 0 if success, other if failure
	  */
	int process_new_rehost(int license_type) 
    {
        szDXML[0] = null;
        szDXML[1] = null;

        System.out.print(" Process base independent license that allows rehosting:\n");    

        // load license definition file
        license_template_number = 1;

        if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
        	szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

            // load C2V file
            szC2V = load_file(szCurrentStateFilename);
            if (szC2V == null) 
            {
                System.out.print("  error in loading input/SL-AdminMode/fingerprint.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
            
            szDXML[0] = load_file("input/SL-AdminMode/rehost_license_template.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-AdminMode/rehost_license_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-AdminMode/rehost_license.v2c";
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_rehost.xml";
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
        	szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v";

            // load C2V file
            szC2V = load_file(szCurrentStateFilename);
            if (szC2V == null) 
            {
                System.out.print("  error in loading input/SL-UserMode/fingerprint.c2v file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }
            
            szDXML[0] = load_file("input/SL-UserMode/rehost_license_template.xml");
            if ( szDXML[0] == null )
            {
                System.out.print("  error in loading input/SL-UserMode/rehost_license_template.xml file.\n");
                return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
            }

            szV2CFilename = "output/SL-UserMode/rehost_license.v2c";
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_rehost.xml";
        }
        else
        {
            ;//do nothing
        }

        if ( generate_license() != 0 )
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        if ( SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE == license_type )
        {
            System.out.print("  License file \"output/SL-AdminMode/rehost_license.v2c\" has been generated successfully.\n\n");           
        }
        else if ( SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE == license_type )
        {
            System.out.print("  License file \"output/SL-UserMode/rehost_license.v2c\" has been generated successfully.\n\n");
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
    int process_new_detach() 
    {
        szDXML[0] = null;
        szDXML[1] = null;

        System.out.print(" Process base independent detachable license:\n");     

        szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v";

        // load C2V file
        szC2V = load_file(szCurrentStateFilename);
        if (szC2V == null) 
        {
            System.out.print("  error in loading input/SL-AdminMode/fingerprint.c2v file.\n");
            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
        }

        // load license definition file
        license_template_number = 1;

        szDXML[0] = load_file("input/SL-AdminMode/detach_license_template.xml");
        if ( szDXML[0] == null )
        {
            System.out.print("  error in loading input/SL-AdminMode/detach_license_template.xml file.\n");
            return SNTL_LG_SAMPLE_LOAD_FILE_FAILED;
        }

        szV2CFilename = "output/SL-AdminMode/detach_license.v2c";
        szUXMLFilename = "output/SL-AdminMode/resultant_state_after_detach.xml";

        if ( generate_license() != 0 )
        {
            return SNTL_LG_SAMPLE_GENERATE_NEW_FAILED;
        }

        System.out.print("  License file \"output/SL-AdminMode/detach_license.v2c\" has been generated successfully.\n\n");

        return 0;
    }

    public static void main(String[] args)
    {
        Sample sample = null;

        try
        {
            sample = new Sample();

            sample.show_copyright();

            sample.show_licgen_version();

            if (sample.handle_parameter(args) != 0)
            {
                // wrong parameter
                return;
            }
            
            //demonstrate the HL example
            sample.show_sample_license_type(sample.SNTL_LG_SAMPLE_LICENSE_HL);

            if (sample.process_decode_current_state(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
            {
                System.out.print(" Fail to generate decoded current state.\n\n");
            }
         
            if (sample.process_clear_key(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
            {
                System.out.print(" Fail to generate clear license.\n\n");
            }

            if (sample.process_format_key(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
            {
                System.out.print(" Fail to generate format license.\n\n");
            }
          
            if (sample.process_new(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
            {
                System.out.print(" Fail to generate new license.\n\n");
            }            
            
            if (sample.option != 'n')
            {
            	
            	if (sample.process_modify(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    System.out.print(" Fail to generate modify license.\n\n");
                }
            }             
            
            if (sample.option != 'n' && sample.option != 'm')
            {
                if (sample.process_cancel(sample.SNTL_LG_SAMPLE_LICENSE_HL) != 0)
                {
                    System.out.print(" Fail to generate cancel license.\n\n");
                }
            }     
            
            //demonstrate the SL-AdminMode example
            sample.show_sample_license_type(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE);

            if (sample.generate_provisional_license(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
            {
                System.out.print(" Fail to generate provisional license.\n\n");
            }
            
            if ( sample.process_new_base_independent(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0 )
            {
            	System.out.print(" Fail to generate base independent license.\n\n");
            }
            
            if ( sample.process_new_rehost(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0 )
            {
            	System.out.print(" Fail to generate base independent license that allows rehosting.\n\n");
            }
            
            if ( sample.process_new_detach() != 0 )
            {
            	System.out.print(" Fail to generate base independent detachable license.\n\n");
            }
            
            if (sample.process_decode_current_state(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
            {
                System.out.print(" Fail to generate decoded current state.\n\n");
            }           
			
            if (sample.process_clear_key(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
            {
                System.out.print(" Fail to generate clear license.\n\n");
            }

            if (sample.process_format_key(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
            {
                System.out.print(" Fail to generate format license.\n\n");
            }

            if (sample.process_new(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
            {
                System.out.print(" Fail to generate new license.\n\n");
            }
            
            if (sample.option != 'n')
            {            	
            	if (sample.process_modify(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    System.out.print(" Fail to generate modify license.\n\n");
                }
            }             
            
            if (sample.option != 'n' && sample.option != 'm')
            {
                if (sample.process_cancel(sample.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) != 0)
                {
                    System.out.print(" Fail to generate cancel license.\n\n");
                }
            }                        
            
            //demonstrate the SL-UserMode example
            sample.show_sample_license_type(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE);

            if (sample.generate_provisional_license(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
            {
                System.out.print(" Fail to generate provisional license.\n\n");
            }
            
            if ( sample.process_new_base_independent(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0 )
            {
            	System.out.print(" Fail to generate base independent license.\n\n");
            }
            
            if ( sample.process_new_rehost(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0 )
            {
            	System.out.print(" Fail to generate base independent license that allows rehosting.\n\n");
            }
            
            if (sample.process_decode_current_state(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
            {
                System.out.print(" Fail to generate decoded current state.\n\n");
            }           
			
            if (sample.process_clear_key(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
            {
                System.out.print(" Fail to generate clear license.\n\n");
            }

            if (sample.process_format_key(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
            {
                System.out.print(" Fail to generate format license.\n\n");
            }

            if (sample.process_new(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0) 
            {
                System.out.print(" Fail to generate new license.\n\n");
            }
            
            if (sample.option != 'n')
            {
            	
            	if (sample.process_modify(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    System.out.print(" Fail to generate modify license.\n\n");
                }
            }
            
            if (sample.option != 'n' && sample.option != 'm')
            {            	
                if (sample.process_cancel(sample.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) != 0)
                {
                    System.out.print(" Fail to generate cancel license.\n\n");
                }
            }
            
        }
        catch (java.lang.UnsatisfiedLinkError e) 
        {
            System.out.print(" Failed to load Sentinel LDK License Generation DLL.\n\n");
            return;
        }
    }
}
