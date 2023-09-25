/****************************************************************************
*
* Demo program for Sentinel LDK
*
*
* Copyright (C) 2014, SafeNet, Inc. All rights reserved.
*
****************************************************************************/

#include "sntl_adminapi_cpp.h"
#include <string.h>
#include <iostream>
#include <sstream>

using namespace std;

CAdminAPIContext *adminApi;

string intToString(int i)
{
  ostringstream os;
  os << i;
  return os.str();
}

string getErrorText(int status)
{
  sntl_admin_error_codes t = (sntl_admin_status_t) status;

  switch (t)
  {
    case SNTL_ADMIN_STATUS_OK:
      return "StatusOk";
    case SNTL_ADMIN_INSUF_MEM:
      return "InsufMem";
    case SNTL_ADMIN_INVALID_CONTEXT:
      return "InvalidContext";
    case SNTL_ADMIN_LM_NOT_FOUND:
      return "LmNotFound";
    case SNTL_ADMIN_LM_TOO_OLD:
      return "LmTooOld";
    case SNTL_ADMIN_BAD_PARAMETERS:
      return "BadParameters";
    case SNTL_ADMIN_LOCAL_NETWORK_ERR:
      return "LocalNetworkErr";
    case SNTL_ADMIN_CANNOT_READ_FILE:
      return "CannotReadFile";
    case SNTL_ADMIN_SCOPE_ERROR:
      return "ScopeError";
    case SNTL_ADMIN_PASSWORD_REQUIRED:
      return "PasswordRequired";
    case SNTL_ADMIN_CANNOT_SET_PASSWORD:
      return "CannotSetPassword";
    case SNTL_ADMIN_UPDATE_ERROR:
      return "UpdateError";
    case SNTL_ADMIN_LOCAL_ONLY:
      return "LocalOnly";
    case SNTL_ADMIN_BAD_VALUE:
      return "BadValue";
    case SNTL_ADMIN_READ_ONLY:
      return "ReadOnly";
    case SNTL_ADMIN_ELEMENT_UNDEFINED:
      return "ElementUndefined";
    case SNTL_ADMIN_INVALID_PTR:
      return "InvalidPtr";
    case SNTL_ADMIN_NO_INTEGRATED_LM:
      return "NoIntegratedLm";
    case SNTL_ADMIN_RESULT_TOO_BIG:
      return "ResultTooBig";
    case SNTL_ADMIN_SCOPE_RESULTS_EMPTY:
      return "ScopeResultsEmpty";
    case SNTL_ADMIN_INV_VCODE:
      return "InvVcode";
    case SNTL_ADMIN_UNKNOWN_VCODE:
      return "UnknownVcode";
  }
  return "";
}

void printState(int status, string info)
{
  if(!info.empty())
  {
    cout<<endl<<info;
  }

  cout << endl << "Result: " << getErrorText(status) << " Statuscode: " << status << "\n";
}

void printState(int status)
{
  printState(status, string(""));
}

void sampleAdminConnect()
{
  int rc = 0;

  rc = adminApi->connect();
  printState(rc);
}

string sampleAdminGet(const string scope, const string format)
{
  string info;
  sntl_admin_status_t status;

  cout<<endl<<"sntl_admin_get()";
  status = adminApi->get(scope, format, info);
  printState(status, info);

  return info;
}

void sampleAdminSet(const string config)
{
  sntl_admin_status_t status;
  string info;

  cout<<endl<<"sntl_admin_set()";

  status = adminApi->set(config, info);
  printState(status, info);
}

string commonSamples()
{
  //Retrieve current context
  sampleAdminGet("", "<context></context>");

  //Retrieve all configuration settings (for backup)
  string backupdata = sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"*\"/>"
    " </config>"
    "</admin>"
    );

  sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"friendlyname\" />"
    " </config>"
    "</admin>");

  sampleAdminSet(
    "<config>"
    " <enabledetach>1</enabledetach>"
    "</config>");

  //Set configuration defaults and write to ini file
  sampleAdminSet(
    "<config>"
    " <set_defaults />"
    " <writeconfig />"
    "</config>");

  //Change some settings and write to ini file"
  //(For a list of available elements, see reply for \"*\" element below)
  sampleAdminSet(
    "<config>"
    " <serveraddrs_clear/>"
    " <serveraddr>10.24.2.16   </serveraddr>"
    " <serveraddr>    10.24.2.16</serveraddr>"
    " <serveraddr>p4p</serveraddr>"
    " <serveraddr>10.24.2.255</serveraddr>"
    " <user_restrictions_clear/>"
    " <user_restriction>deny=baerbel@all</user_restriction>"
    " <access_restrictions_clear />"
    " <access_restriction>deny=10.23.*</access_restriction>"
    " <enabledetach>1</enabledetach>"
    " <writeconfig />"
    "</config>");

  //Retrieve all configuration settings
  sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"*\" />"
    " </config>"
    "</admin>");

  //Add some access restrictions
  sampleAdminSet(
    "<config>"
    " <access_restriction>allow=123</access_restriction>"
    " <access_restriction>allow=abcd</access_restriction>"
    " <access_restriction>allow=hello_world</access_restriction>"
    " <writeconfig />"
    "</config>");

  //Retrieve all access restrictions
  sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"access_restriction\" />"
    " </config>"
    "</admin>");

  //Add some more access restrictions
  sampleAdminSet(
    "<config>"
    " <access_restriction>allow=more_123</access_restriction>"
    " <access_restriction>allow=more_abcd</access_restriction>"
    " <access_restriction>allow=more_hello_world</access_restriction>"
    " <writeconfig />"
    "</config>");

  //Retrieve all access restrictions
  sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"access_restriction\" />"
    " </config>"
    "</admin>");

  //Delete existing access restrictions and add some new ones
  sampleAdminSet(
    "<config>"
    " <access_restrictions_clear/>"
    " <access_restriction>allow=new_123</access_restriction>"
    " <access_restriction>allow=new_abcd</access_restriction>"
    " <writeconfig />"
    "</config>");

  //Retrieve all access restrictions
  sampleAdminGet("",
    "<admin>"
    " <config>"
    "  <element name=\"access_restriction\" />"
    " </config>"
    "</admin>");

  return backupdata;
}

//using haspscope to retrieve filtered data
void scopeSamples(int vendorId)
{
  sampleAdminGet(
    "<haspscope>"
    " <vendor id=\"" + intToString(vendorId) + "\" />"
    "</haspscope>",
    "<admin>"
    " <hasp>\\n"
    "  <element name=\"vendorid\" />"
    "  <element name=\"haspid\" />"
    "  <element name=\"typename\" />"
    "  <element name=\"local\" />"
    "  <element name=\"localname\" />"
    " </hasp> "
    "</admin>"
    );

  //Retrieve key data for specified vendor (scope with element notation)
  sampleAdminGet(
    "<haspscope>\\n"
    " <vendor><id>" + intToString(vendorId) + "</id></vendor>"
    "</haspscope>",
    "<admin>"
    " <hasp>"
    "  <element name=\"vendorid\" />"
    "  <element name=\"haspid\" />"
    "  <element name=\"typename\" />"
    "  <element name=\"local\" />"
    "  <element name=\"localname\" />"
    " </hasp>"
    "</admin>");

  //Retrieve all product data for specified vendor id"
  sampleAdminGet(
    "<haspscope>\\n"
    " <vendor><id>" + intToString(vendorId) + "</id></vendor>"
    "</haspscope>",
    "<admin>"
    " <product>"
    "  <element name=\"*\" />"
    " </product>"
    "</admin>");

  //Retrieve selected session data for all keys of a specified vendor
  sampleAdminGet(
    "<haspscope>\\n"
    " <vendor><id>" + intToString(vendorId) + "</id></vendor>"
    "</haspscope>",
    "<admin>"
    " <session>"
    "  <element name=\"user\" />"
    "  <element name=\"machine\" />"
    "  <element name=\"logintime\" />"
    " </session>"
    "</admin>");

}

//Sample where the scope uses the key-id
void keyIdSamples(int keyId)
{
  //Retrieve all key data for specified key id"
  sampleAdminGet(
    "<haspscope>"
    " <hasp><id>" + intToString(keyId) + "</id></hasp>"
    "</haspscope>",
    "<admin>"
    " <hasp>"
    "  <element name=\"*\" />"
    " </hasp>"
    "</admin>");

  //Retrieve all feature data for specified key id"
  sampleAdminGet(
    "<haspscope>"
    " <hasp><id>" + intToString(keyId) + "</id></hasp>"
    "</haspscope>", "<admin>"
    " <feature>"
    "  <element name=\"*\" />"
    " </feature>"
    "</admin>");

  //Retrieve list of current sessions for a specified key
  sampleAdminGet(
    "<haspscope>"
    " <hasp><id>" + intToString(keyId) + "</id></hasp>"
    "</haspscope>", "<admin>"
    " <session>"
    "  <element name=\"*\" />"
    " </session>"
    "</admin>");

  //Retrieve all product data for specified key id"
  sampleAdminGet(
    "<haspscope>"
    " <hasp><id>" + intToString(keyId) + "</id></hasp>"
    "</haspscope>", "<admin>"
    " <product>"
    "  <element name=\"productid\" />"
    "  <element name=\"productname\" />"
    "  <element name=\"detachable\" />"
    "  <element name=\"maxseats\" />"
    "  <element name=\"seatsfree\" />"
    " </product>"
    "</admin>");
}

//Retrieve License Manager and license related data
void licenseMangerSample()
{
  //Retrieve license manager data in XML format (default)
  sampleAdminGet("",
    "<admin>"
    " <license_manager>"
    "  <element name=\"*\" />"
    " </license_manager>"
    "</admin>");

  //Retrieve license manager data in JSON format
  sampleAdminGet("",
    "<admin>"
    " <license_manager format=\"json\">"
    "  <element name=\"*\" />"
    " </license_manager>"
    "</admin>");

  //Retrieve list of detached licenses
  sampleAdminGet("",
    "<admin>"
    " <detached>"
    "  <element name=\"*\" />"
    " </detached>"
    "</admin>");
}

//Setting a key description (disabled because it was not backed up)
void keyDescriptionSample()
{
  //Add a key description (legacy format)
  sampleAdminSet(
    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
    "<keydescription>"
    " <hasp>"
    "  <id>123456</id>"
    "  <name>One two three four five six</name>"
    " </hasp>"
    "</keydescription>");
}

//Examples for uploading of files (disabled because reverting not possible)
void fileUploadSample()
{
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
void deleteSessionSample()
{
  sampleAdminSet(
    "<admin>"
    " <deletesession>"
    "  <sessionid>1</sessionid>"
    "  <session id=\"2\" />"
    "  <session id=\"3\" />"
    "  <sessionid>4</sessionid>"
    " </deletesession>"
    "</admin>");
}

//Retrieve list of certificates for specified key (XML)
void certificateSample(int keyId)
{
  sampleAdminGet(
    "<haspscope>"
    " <hasp><id>" + intToString(keyId) + "</id></hasp>"
    "</haspscope>", "<admin>"
    " <certificates>"
    "  <element name=\"*\" />"
    " </certificates>"
    "</admin>");
}

string generateConfigFromOutput(string output)
{
  if(output != "")
  {
    string result = output.substr(0, output.find("<admin_status"));
    return result.replace(output.find("</config>"), 9, "<writeconfig /></config>");
  }
  return "";
}

void restoreData(string backupdata)
{
  const string config = generateConfigFromOutput(backupdata);
  string info;

  adminApi->set(
    "<config>"
    " <serveraddrs_clear/>"
    " <user_restrictions_clear/>"
    " <access_restrictions_clear/>"
    " <writeconfig/>"
    "</config>"
    , info
    );

//  if(config != "")
//    adminApi->set(config, NULL);
}

int main()
{
  string backupdata;
  int vendorId = 37515;

  cout<<endl<<"A simple demo program for the Sentinel LDK administration functions";
  cout<<endl<<"Copyright (C) SafeNet, Inc. All rights reserved.\n";

  adminApi = new CAdminAPIContext("localhost", 1947, "");

  //this initializes the adminApi
  sampleAdminConnect();

  //different samples
  backupdata = commonSamples();
  scopeSamples(vendorId);
  licenseMangerSample();
  keyDescriptionSample();
  restoreData(backupdata);

  //delete the context
  //adminApi->delete_context();

  return 0;
}

