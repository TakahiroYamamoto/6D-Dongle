/****************************************************************************
*
* Demo program for Sentinel LDK
*
*
* Copyright (C) 2014, SafeNet, Inc. All rights reserved.
*
* To use UTF-8 characters in Strings the encoding must be specified 
* at compile time with "-encoding UTF-8".
****************************************************************************/
import SafeNet.AdminApi;
import SafeNet.AdminStatus;

public class AdminApiSample {

    private static AdminApi adminApi;
    private static int vendorId = 37515;

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        String backupdata = "";

        System.out.println("A simple demo program for the Sentinel LDK administration functions");
        System.out.println("Copyright (C) 2013, SafeNet, Inc. All rights reserved.\n");

        adminApi = new AdminApi("localhost", 1947, "");

        //this initializes the adminApi
        sampleAdminConnect();

        //different samples
        backupdata = commonSamples();
        scopeSamples(vendorId);
        licenseMangerSample();
        keyDescriptionSample();
        restoreData(backupdata);
		
		//close the session
		adminApi.dispose();
    }

    private static String commonSamples() {

        //Retrieve current context
        sampleAdminGet("", "<context></context>");

        //Retrieve all configuration settings (for backup)
        String backupdata = sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"*\"/>"
                + " </config>"
                + "</admin>");

        sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"friendlyname\" />"
                + " </config>"
                + "</admin>");

        sampleAdminSet(
                "<config>"
                + " <enabledetach>1</enabledetach>"
                + "</config>");

        //Set configuration defaults and write to ini file
        sampleAdminSet(
                "<config>"
                + " <set_defaults />"
                + " <writeconfig />"
                + "</config>");

        //Change some settings and write to ini file"
        //(For a list of available elements, see reply for \"*\" element below)
        sampleAdminSet(
                "<config>"
                + " <serveraddrs_clear/>"
                + " <serveraddr>10.24.2.16   </serveraddr>"
                + " <serveraddr>    10.24.2.16</serveraddr>"
                + " <serveraddr>p4p</serveraddr>"
                + " <serveraddr>10.24.2.255</serveraddr>"
                + " <user_restrictions_clear/>"
                + " <user_restriction>deny=baerbel@all</user_restriction>"
                + " <access_restrictions_clear />"
                + " <access_restriction>deny=10.23.*</access_restriction>"
                + " <enabledetach>1</enabledetach>"
                + " <writeconfig />"
                + "</config>");

        //Retrieve all configuration settings
        sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"*\" />"
                + " </config>"
                + "</admin>");

        //Add some access restrictions
        sampleAdminSet(
                "<config>"
                + " <access_restriction>allow=123</access_restriction>"
                + " <access_restriction>allow=abcd</access_restriction>"
                + " <access_restriction>allow=hello_world</access_restriction>"
                + " <writeconfig />"
                + "</config>");

        //Retrieve all access restrictions
        sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"access_restriction\" />"
                + " </config>"
                + "</admin>");

        //Add some more access restrictions
        sampleAdminSet(
                "<config>"
                + " <access_restriction>allow=more_123</access_restriction>"
                + " <access_restriction>allow=more_abcd</access_restriction>"
                + " <access_restriction>allow=more_hello_world</access_restriction>"
                + " <writeconfig />"
                + "</config>");

        //Retrieve all access restrictions
        sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"access_restriction\" />"
                + " </config>"
                + "</admin>");

        //Delete existing access restrictions and add some new ones
        sampleAdminSet(
                "<config>"
                + " <access_restrictions_clear/>"
                + " <access_restriction>allow=new_123</access_restriction>"
                + " <access_restriction>allow=new_abcd</access_restriction>"
                + " <writeconfig />"
                + "</config>");

        //Retrieve all access restrictions
        sampleAdminGet("",
                "<admin>"
                + " <config>"
                + "  <element name=\"access_restriction\" />"
                + " </config>"
                + "</admin>");

        return backupdata;
    }

    //using haspscope to retrieve filtered data
    private static void scopeSamples(int vendorId) {

        //Retrieve some key data for specified vendor (scope with attribute notation)
        //(for a list of available elements, see reply for \"*\" element below)
        sampleAdminGet(
                "<haspscope>"
                + " <vendor id=\"" + vendorId + "\" />"
                + "</haspscope>", "<admin>"
                + " <hasp>\\n"
                + "  <element name=\"vendorid\" />"
                + "  <element name=\"haspid\" />"
                + "  <element name=\"typename\" />"
                + "  <element name=\"local\" />"
                + "  <element name=\"localname\" />"
                + " </hasp> "
                + "</admin>");

        //Retrieve key data for specified vendor (scope with element notation)
        sampleAdminGet(
                "<haspscope>\\n"
                + " <vendor><id>" + vendorId + "</id></vendor>"
                + "</haspscope>", "<admin>"
                + " <hasp>"
                + "  <element name=\"vendorid\" />"
                + "  <element name=\"haspid\" />"
                + "  <element name=\"typename\" />"
                + "  <element name=\"local\" />"
                + "  <element name=\"localname\" />"
                + " </hasp>"
                + "</admin>");

        //Retrieve all product data for specified vendor id"
        sampleAdminGet(
                "<haspscope>\\n"
                + " <vendor><id>" + vendorId + "</id></vendor>"
                + "</haspscope>", "<admin>"
                + " <product>"
                + "  <element name=\"*\" />"
                + " </product>"
                + "</admin>");

        //Retrieve selected session data for all keys of a specified vendor
        sampleAdminGet(
                "<haspscope>\\n"
                + " <vendor><id>" + vendorId + "</id></vendor>"
                + "</haspscope>", "<admin>"
                + " <session>"
                + "  <element name=\"user\" />"
                + "  <element name=\"machine\" />"
                + "  <element name=\"logintime\" />"
                + " </session>"
                + "</admin>");


    }

    //Sample where the scope uses the key-id
    private static void keyIdSamples(int keyId) {
        
        //Retrieve all key data for specified key id"
        sampleAdminGet(
                "<haspscope>"
                + " <hasp><id>" + keyId + "</id></hasp>"
                + "</haspscope>", "<admin>"
                + " <hasp>"
                + "  <element name=\"*\" />"
                + " </hasp>"
                + "</admin>");

        //Retrieve all feature data for specified key id"
        sampleAdminGet(
                "<haspscope>"
                + " <hasp><id>" + keyId + "</id></hasp>"
                + "</haspscope>", "<admin>"
                + " <feature>"
                + "  <element name=\"*\" />"
                + " </feature>"
                + "</admin>");

        //Retrieve list of current sessions for a specified key
        sampleAdminGet(
                "<haspscope>"
                + " <hasp><id>" + keyId + "</id></hasp>"
                + "</haspscope>", "<admin>"
                + " <session>"
                + "  <element name=\"*\" />"
                + " </session>"
                + "</admin>");

        //Retrieve all product data for specified key id"
        sampleAdminGet(
                "<haspscope>"
                + " <hasp><id>" + keyId + "</id></hasp>"
                + "</haspscope>", "<admin>"
                + " <product>"
                + "  <element name=\"productid\" />"
                + "  <element name=\"productname\" />"
                + "  <element name=\"detachable\" />"
                + "  <element name=\"maxseats\" />"
                + "  <element name=\"seatsfree\" />"
                + " </product>"
                + "</admin>");
    }

    //Retrieve License Manager and license related data
    private static void licenseMangerSample() {
        
        //Retrieve license manager data in XML format (default)
        sampleAdminGet("",
                "<admin>"
                + " <license_manager>"
                + "  <element name=\"*\" />"
                + " </license_manager>"
                + "</admin>");

        //Retrieve license manager data in JSON format
        sampleAdminGet("",
                "<admin>"
                + " <license_manager format=\"json\">"
                + "  <element name=\"*\" />"
                + " </license_manager>"
                + "</admin>");

        //Retrieve list of detached licenses
        sampleAdminGet("",
                "<admin>"
                + " <detached>"
                + "  <element name=\"*\" />"
                + " </detached>"
                + "</admin>");
    }

    //Setting a key description (disabled because it was not backed up)
    private static void keyDescriptionSample() {
        
        //Add a key description (legacy format)
        sampleAdminSet(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<keydescription>"
                + " <hasp>"
                + "  <id>123456</id>"
                + "  <name>One two three four five six</name>"
                + " </hasp>"
                + "</keydescription>");
    }

    //Examples for uploading of files (disabled because reverting not possible)
    private static void fileUploadSample() {
        
        //Upload a detach location data file
        sampleAdminSet("file://test_location.xml");

        //Upload a key names metadata file");
        sampleAdminSet("file://test_key.xml");

        //Upload a product names metadata file
        sampleAdminSet("file://test_product.xml");

        //Upload a vendor names metadata file
        sampleAdminSet("file://test_vendor.xml");

        //Applying V2C
        sampleAdminSet("file://test_update.v2c");
    }

    //Example how to delete a some sessions
    private static void deleteSessionSample() {
        sampleAdminSet(
                "<admin>"
                + " <deletesession>"
                + "  <sessionid>1</sessionid>"
                + "  <session id=\"2\" />"
                + "  <session id=\"3\" />"
                + "  <sessionid>4</sessionid>"
                + " </deletesession>"
                + "</admin>");
    }

    //Retrieve list of certificates for specified key (XML)
    private static void certificateSample(int keyId) {
        sampleAdminGet(
                "<haspscope>"
                + " <hasp><id>" + keyId + "</id></hasp>"
                + "</haspscope>", "<admin>"
                + " <certificates>"
                + "  <element name=\"*\" />"
                + " </certificates>"
                + "</admin>");
    }

    private static String sampleAdminGet(String scope, String format) {
        int rc = 0;
        String info = "";

        System.out.println("sntl_admin_get()");

        info = adminApi.getConfig(scope, format);
        rc = adminApi.getLastApiError();
        printState(rc, info);

        return info;
    }

    private static void sampleAdminSet(String config) {
        int rc = 0;
        String returnStatus = "";

        System.out.println("sntl_admin_set()");
        
        returnStatus = adminApi.setConfig(config);
        rc = adminApi.getLastApiError();
        printState(rc, returnStatus);
    }

    private static void sampleAdminConnect() {
        int rc = 0;
        
        rc = adminApi.connect();
        printState(rc);
    }

    private static void restoreData(String backupdata) {
        String config = generateConfigFromOutput(backupdata);

        adminApi.setConfig(
                "<config>"
                + " <serveraddrs_clear/>"
                + " <user_restrictions_clear/>"
                + " <access_restrictions_clear/>"
                + " <writeconfig/>"
                + "</config>");

        adminApi.setConfig(config);
    }

    private static void printState(int status) {
        printState(status, null);
    }

    private static void printState(int status, String info) {
        if (!(info == null || ("").equals(info))) {
            System.out.println(info);
        }

        System.out.println("Result: " + getErrorText(status) + " Statuscode: " + status + "\n");
    }

    private static String generateConfigFromOutput(String output) {
        String[] result = output.split("<admin_status");
        return result[0].replace("</config>", "<writeconfig /></config>");
    }

    private static String getErrorText(int status) {
        switch (status) {
            case AdminStatus.SNTL_ADMIN_STATUS_OK:
                return "StatusOk";
            case AdminStatus.SNTL_ADMIN_LM_NOT_FOUND:
                return "LmNotFound";
            case AdminStatus.SNTL_ADMIN_LM_TOO_OLD:
                return "LmTooOld";
            case AdminStatus.SNTL_ADMIN_DLL_BROKEN:
                return "DllBroken";
            case AdminStatus.SNTL_ADMIN_CONNECT_MISSING:
                return "ConnectMissing";
            case AdminStatus.SNTL_ADMIN_BAD_PARAMETERS:
                return "BadParameters";
            case AdminStatus.SNTL_ADMIN_BAD_VALUE:
                return "BadValue";
        }
        return "";
    }
}
