#include "sntl_adminapi_cpp.h"
#include "hasp_api_cpp.h"
#include <string.h>
#include <iostream>
#include <sstream>

using namespace std;

CAdminAPIContext *adminApiIntegrated;
CAdminAPIContext *adminApiStandalone;

string getErrorText(int status)
{
	switch ((sntl_admin_status_t) status)
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

void printState(int status, string info = "")
{
	if(!info.empty())
	{
		cout<<endl<<info;
	}

	cout << endl << "Result: " << getErrorText(status) << " Statuscode: " << status << "\n\n";
}

int main()
{
	int status = 0;
	std::string data;
	// You can specify some other server IP address (from different subnet also) hosting Sentinel License Manager Service
	std::string server = "127.0.0.1";

	cout << endl << "A simple demo program for the Sentinel LDK administration functions";
	cout << endl << "Copyright (C) SafeNet, Inc. All rights reserved.\n\n";

	// Below is DEMOMA vendor code. ISV can update below with their assigned vendor code
	VendorCodeType vc(
		"AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsV"
		"vIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrB"
		"rh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6"
		"AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf"
		"8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1"
		"e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBh"
		"aJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SK"
		"unFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bL"
		"Cx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwL"
		"zaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9s"
		"JN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaT"
		"tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"
		);

	cout << "connect to sntl_integrated_lm " << endl;

	adminApiIntegrated = new CAdminAPIContext(vc, "sntl_integrated_lm");

	status = adminApiIntegrated->connect();

	printState(status);

	AdminInfo adminInfo;

	cout << "admin_get " << endl;

	status = adminApiIntegrated->get(
							"<haspscope/>",
							"<context></context>",
							adminInfo
							);

	printState(status, adminInfo.getInfo());

	cout << "admin_set " << endl;

	status = adminApiIntegrated->set(
							"<config>"
							" <serveraddrs_clear/>"
							" <server_select>" + server + "</server_select>"
							"</config>",
							data);

	printState(status, data);

	Chasp haspAny(ChaspFeature::defaultFeature());

	cout << "login to default feature of any key on " << server << endl;

	status = haspAny.login(vc.getValue(),
							"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
							"<haspscope>"
							" <license_manager ip=\"" + server + "\" />"
							"</haspscope>"
						);

	printState(status);

	cout << "login to default feature of SL-AdminMode on " << server << endl;

	Chasp haspSlAm(ChaspFeature::defaultFeature());
	status = haspSlAm.login(vc.getValue(),
							"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"
							"<haspscope>"
							" <hasp type=\"HASP-SL-AdminMode\" >"
							"  <license_manager ip=\"" + server + "\" />"
							" </hasp>"
							"</haspscope>"
						);

	printState(status);

	cout << "admin_get " << endl;

	status = adminApiIntegrated->get(
							"<haspscope/>",
							"<admin>"
							"  <license_manager>"
							"	<element name=\"version\" />"
							"	<element name=\"servername\" />"
							"	<element name=\"uptime\" />"
							"	<element name=\"driver_info\" />"
							"  </license_manager>"
							"</admin>",
							adminInfo
							);

	printState(status, adminInfo.getInfo());


	// Access Standalone LMS using Integrated Admin API

	cout << "connect to Standalone LMS" << endl;

	adminApiStandalone = new CAdminAPIContext(server , 1947, "");

	status = adminApiStandalone->connect();
	printState(status);

	cout << "admin_get " << endl;
	// retrieve sessions
	status = adminApiStandalone->get(
							"<haspscope/>",
							"<admin>"
							"  <session>"
							"	<element name=\"*\" />"
							"  </session>"
							"</admin>",
							data
							);

	printState(status, data);

	cout << "admin_get " << endl;

	status = adminApiStandalone->get(
							"<haspscope/>",
							"<context></context>",
							data
							);

	printState(status, data);

	cout << "admin_get " << endl;

	status = adminApiStandalone->get(
							"<haspscope/>",
							"<admin>"
							"  <license_manager>"
							"	<element name=\"version\" />"
							"	<element name=\"servername\" />"
							"	<element name=\"uptime\" />"
							"	<element name=\"driver_info\" />"
							"  </license_manager>"
							"</admin>",
							data
							);

	printState(status, data);

	haspAny.logout();
	haspSlAm.logout();

	cout<<endl<<"press ENTER to close the sample"<<endl;

	fflush(stdin);
	while (getchar() == EOF);

	return 0;
}
