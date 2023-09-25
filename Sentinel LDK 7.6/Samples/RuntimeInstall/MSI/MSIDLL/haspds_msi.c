/****************************************************************************
*
* MSI Wrapper DLL for Sentinel HASP Run-time Environment Installer
*
* Copyright (C) 2010, SafeNet, Inc. All rights reserved.
*
*
****************************************************************************/


#include <windows.h>
#include <msiquery.h>
#include <process.h>
#include <winioctl.h>

#include "haspds.h"
#include "TCHAR.H"

typedef haspds_status_t  (__stdcall* HASPDS_INSTALL)(unsigned long Param);
typedef haspds_status_t  (__stdcall* HASPDS_UNINSTALL)(unsigned long Param);
typedef haspds_status_t  (__stdcall* HASPDS_GETINFO)(PHASPDS_INFO,unsigned long* pSize);
typedef haspds_error     (__stdcall* HASPDS_GETLASTERROR)(void);
typedef haspds_status_t  (__stdcall* HASPDS_GETLASTERRORMESSAGE)(char* pBuffer,unsigned long* pSize);
typedef unsigned long    (__stdcall* HASPDS_GETVERSION)(void);

#define MESSAGE_STRING_LEN 250

static int stop_service(SC_HANDLE sc_managa, char *service_name)
{
    SC_HANDLE sc_service;
    BOOL            ret;
    SERVICE_STATUS  service_status;
    
#ifdef DEBUG
    DebugPrint(("strtstop: stop_service: '%s'\n",service_name));
#endif

    sc_service = OpenService(sc_managa,
                             service_name,
                             SERVICE_ALL_ACCESS);

    if (sc_service == NULL) {
#ifdef DEBUG
        DebugPrint(("strtstop: stop_service: cannot open service '%s': 0x%x\n",service_name,GetLastError()));
#endif
        return FALSE;
    }

    ret = ControlService(sc_service,
                         SERVICE_CONTROL_STOP,
                         &service_status);
    if (ret)
    {
        Sleep(10000); /* wait until service stopped */
    }
    
    
#ifdef DEBUG
    if (ret) 
    {
        DebugPrint(("service '%s' stopped\n",service_name));
    }
    else DebugPrint(("failure: cannot stop service '%s' (0x%02x)\n",service_name,GetLastError()));
#endif

    CloseServiceHandle(sc_service);
    return ret;
}

static int start_service(SC_HANDLE sc_managa, char *service_name)
{
    SC_HANDLE sc_service;
    BOOL            ret;
    SERVICE_STATUS  service_status;

    sc_service = OpenService(sc_managa,
                             service_name,
                             SERVICE_ALL_ACCESS);

    if (sc_service == NULL) {
#ifdef DEBUG
        DebugPrint(("strtstop: start_service: cannot open service '%s': 0x%x\n",service_name,GetLastError()));
#endif
        return FALSE;
    }

    ret = StartService(sc_service,
                       0,
                       NULL);
#ifdef DEBUG
    if (ret) DebugPrint(("service '%s' started\n",service_name));
    else DebugPrint(("failure: cannot start service '%s' (0x%02x)\n",service_name,GetLastError()));
#endif

    CloseServiceHandle(sc_service);
    return ret;
}

int stop(void)
{
    SC_HANDLE sc_managa;
    BOOL ret;

    sc_managa = OpenSCManager(NULL,                   /* machine (NULL == local) */
                              NULL,                   /* database (NULL == default) */
                              SC_MANAGER_ALL_ACCESS); /* access required */

    ret = stop_service(sc_managa, "HASP Loader");

    ret = stop_service(sc_managa, "HLServer");

    ret = stop_service(sc_managa, "HASP SRM Business Studio Server");

  raus:
    CloseServiceHandle(sc_managa);
    return ERROR_SUCCESS;
}

int start(void)
{
    SC_HANDLE sc_managa;
    BOOL ret;

    sc_managa = OpenSCManager(NULL,                   /* machine (NULL == local) */
                              NULL,                   /* database (NULL == default) */
                              SC_MANAGER_ALL_ACCESS); /* access required */

    ret = start_service(sc_managa, "HASP Loader");

    ret = start_service(sc_managa, "HLServer");

    ret = start_service(sc_managa, "HASP SRM Business Studio Server");

  raus:
    CloseServiceHandle(sc_managa);
    return ERROR_SUCCESS;
}

BOOL IsWin9X()
{
   OSVERSIONINFOEX osvi;
   ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));
   osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);

   if( !GetVersionEx ((OSVERSIONINFO *) &osvi) )
       return FALSE;

   if (osvi.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS)
     return TRUE;

   return FALSE;
}

UINT __stdcall HaspdsMsiInst(MSIHANDLE hInstall)
{
    HASPDS_INSTALL haspds_Install = 0;
    HASPDS_INSTALL haspds_UnInstall = 0;
    HASPDS_GETINFO haspds_GetInfo = 0;
    HASPDS_GETLASTERRORMESSAGE haspds_GetLastErrorMessage = 0;
    HASPDS_GETLASTERROR haspds_GetLastError = 0;
    HASPDS_GETVERSION haspds_GetVersion = 0;
    HINSTANCE hhlinstDll=0;

    haspds_status_t retCode = HASPDS_STATUS_SUCCESS;
    haspds_error errorCode = 0;
    BYTE FinalMessageString[ MESSAGE_STRING_LEN ];
    DWORD size = 0;

    /* check whether install or remove */
    TCHAR szValue[MAX_PATH] = {0};
    DWORD dwBuffer = MAX_PATH;

    if (!GetEnvironmentVariable("CommonProgramFiles", szValue, MAX_PATH))
    {
      GetWindowsDirectory(szValue, MAX_PATH);
      szValue[3]=0;
      strcat(szValue, "programme\\gemeinsame dateien\\Aladdin Shared\\HASP\\haspds_windows.dll");
      if (!(hhlinstDll = LoadLibrary(szValue)))
      {
        szValue[3]=0;
        strcat(szValue, "Program Files\\Common Files\\Aladdin Shared\\HASP\\haspds_windows.dll");
        hhlinstDll = LoadLibrary(szValue);
      }
    }
    else
    {
      strcat(szValue, "\\Aladdin Shared\\HASP\\haspds_windows.dll");
      hhlinstDll = LoadLibrary(szValue);
    }

    if( !hhlinstDll ){
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to load library"),
        TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      return ERROR_INSTALL_FAILURE;
    }

    haspds_Install = (HASPDS_INSTALL)GetProcAddress(hhlinstDll,"haspds_Install");
    haspds_UnInstall = (HASPDS_UNINSTALL)GetProcAddress(hhlinstDll,"haspds_UnInstall");
    haspds_GetInfo = (HASPDS_GETINFO)GetProcAddress(hhlinstDll,"haspds_GetInfo");
    haspds_GetLastErrorMessage = (HASPDS_GETLASTERRORMESSAGE)GetProcAddress(hhlinstDll,"haspds_GetLastErrorMessage");
    haspds_GetLastError = (HASPDS_GETLASTERROR)GetProcAddress(hhlinstDll,"haspds_GetLastError");
    haspds_GetVersion = (HASPDS_GETVERSION)GetProcAddress(hhlinstDll,"haspds_GetVersion");

    if( !haspds_Install || !haspds_UnInstall || !haspds_GetInfo || !haspds_GetLastErrorMessage || !haspds_GetLastError || !haspds_GetVersion) {
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to access library"),
        TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      FreeLibrary(hhlinstDll);
      return ERROR_INSTALL_FAILURE;
    }

    stop();
    retCode = haspds_Install(HASPDS_PARAM_KILLPROC);
    start();

    size = MESSAGE_STRING_LEN;
    errorCode = haspds_GetLastError();

    if (errorCode != HASPDS_ERR_OK)
    {
      size = MESSAGE_STRING_LEN;
      haspds_GetLastErrorMessage((char*)FinalMessageString,&size);

      // special case for -fremove
      if (errorCode == HASPDS_ERR_USEFR_REQUIRED)
        strcpy (FinalMessageString, "Drivers from previous installations are still present. Use the previous installer to completely remove these drivers.");

	  if(errorCode != HASPDS_ERR_INSTALL_OLD)
		  MessageBox(GetForegroundWindow( ),
			FinalMessageString,
			TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);

      if(!((errorCode == HASPDS_ERR_INSERT_REQUIRED)
      || (errorCode == HASPDS_ERR_USEFR_REQUIRED)
      || (errorCode == HASPDS_ERR_USEHINST_REQUIRED)
      || (errorCode == HASPDS_ERR_INSTALL_OLD)
	  || (errorCode == HASPDS_ERR_REBOOT_REQUIRED)))
      {
          FreeLibrary(hhlinstDll);
          return ERROR_INSTALL_FAILURE;
      }
    }
    else
    {
      if(IsWin9X())
      {
        strcpy((char*)FinalMessageString,"Installation successful.\nNote that the Run-time Environment on this operating system only supports HASP HL, HASP4 and Hardlock.");

        MessageBox(GetForegroundWindow( ),
          FinalMessageString,
          TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      }
    }

    FreeLibrary(hhlinstDll);
    return ERROR_SUCCESS;
}

UINT __stdcall HaspdsMsiRepair(MSIHANDLE hInstall/*TCHAR* szPath*/)
{
    HASPDS_INSTALL haspds_Install = 0;
    HASPDS_INSTALL haspds_UnInstall = 0;
    HASPDS_GETINFO haspds_GetInfo = 0;
    HASPDS_GETLASTERRORMESSAGE haspds_GetLastErrorMessage = 0;
    HASPDS_GETLASTERROR haspds_GetLastError = 0;
    HASPDS_GETVERSION haspds_GetVersion = 0;
    HINSTANCE hhlinstDll=0;

    haspds_status_t retCode = HASPDS_STATUS_SUCCESS;
    haspds_error errorCode = 0;
    BYTE FinalMessageString[ MESSAGE_STRING_LEN ];
    DWORD size = 0;

    /* check whether install or remove */
    TCHAR szValue[MAX_PATH] = {0};
    DWORD dwBuffer = MAX_PATH;

    if (!GetEnvironmentVariable("CommonProgramFiles", szValue, MAX_PATH))
    {
      GetWindowsDirectory(szValue, MAX_PATH);
      szValue[3]=0;
      strcat(szValue, "programme\\gemeinsame dateien\\Aladdin Shared\\HASP\\haspds_windows.dll");
      if (!(hhlinstDll = LoadLibrary(szValue)))
      {
        szValue[3]=0;
        strcat(szValue, "Program Files\\Common Files\\Aladdin Shared\\HASP\\haspds_windows.dll");
        hhlinstDll = LoadLibrary(szValue);
      }
    }
    else
    {
      strcat(szValue, "\\Aladdin Shared\\HASP\\haspds_windows.dll");
      hhlinstDll = LoadLibrary(szValue);
    }

    if( !hhlinstDll ){
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to load library"),
        TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      return ERROR_INSTALL_FAILURE;
    }

    haspds_Install = (HASPDS_INSTALL)GetProcAddress(hhlinstDll,"haspds_Install");
    haspds_UnInstall = (HASPDS_UNINSTALL)GetProcAddress(hhlinstDll,"haspds_UnInstall");
    haspds_GetInfo = (HASPDS_GETINFO)GetProcAddress(hhlinstDll,"haspds_GetInfo");
    haspds_GetLastErrorMessage = (HASPDS_GETLASTERRORMESSAGE)GetProcAddress(hhlinstDll,"haspds_GetLastErrorMessage");
    haspds_GetLastError = (HASPDS_GETLASTERROR)GetProcAddress(hhlinstDll,"haspds_GetLastError");
    haspds_GetVersion = (HASPDS_GETVERSION)GetProcAddress(hhlinstDll,"haspds_GetVersion");

    if( !haspds_Install || !haspds_UnInstall || !haspds_GetInfo || !haspds_GetLastErrorMessage || !haspds_GetLastError || !haspds_GetVersion) {
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to access library"),
        TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      FreeLibrary(hhlinstDll);
      return ERROR_INSTALL_FAILURE;
    }

    stop();
    retCode = haspds_Install(HASPDS_PARAM_KILLPROC|HASPDS_PARAM_REPAIR);
    start();

    size = MESSAGE_STRING_LEN;
    errorCode = haspds_GetLastError();

    if (errorCode != HASPDS_ERR_OK)
    {
      size = MESSAGE_STRING_LEN;
      haspds_GetLastErrorMessage((char*)FinalMessageString,&size);

      // special case for -fremove
      if (errorCode == HASPDS_ERR_USEFR_REQUIRED)
        strcpy (FinalMessageString, "Drivers from previous installations are still present. Use the previous installer to completely remove these drivers.");

	  if(errorCode != HASPDS_ERR_INSTALL_OLD)
		  MessageBox(GetForegroundWindow( ),
			FinalMessageString,
			TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);

      if(!((errorCode == HASPDS_ERR_INSERT_REQUIRED)
      || (errorCode == HASPDS_ERR_USEFR_REQUIRED)
      || (errorCode == HASPDS_ERR_USEHINST_REQUIRED)
      || (errorCode == HASPDS_ERR_INSTALL_OLD)
      || (errorCode == HASPDS_ERR_REBOOT_REQUIRED)))
      {
          FreeLibrary(hhlinstDll);
          return ERROR_INSTALL_FAILURE;
      }
    }
    else
    {
      if(IsWin9X())
      {
        strcpy((char*)FinalMessageString,"Installation successful.\nNote that the Run-time Environment on this operating system only supports HASP HL, HASP4 and Hardlock.");

        MessageBox(GetForegroundWindow( ),
          FinalMessageString,
          TEXT("Sentinel HASP Run-time installation"), MB_OK | MB_ICONINFORMATION);
      }
    }

    FreeLibrary(hhlinstDll);
    return ERROR_SUCCESS;
}

UINT __stdcall HaspdsMsiUninst(MSIHANDLE hInstall)
{
    HASPDS_INSTALL haspds_Install = 0;
    HASPDS_INSTALL haspds_UnInstall = 0;
    HASPDS_GETINFO haspds_GetInfo = 0;
    HASPDS_GETLASTERRORMESSAGE haspds_GetLastErrorMessage = 0;
    HASPDS_GETLASTERROR haspds_GetLastError = 0;
    HASPDS_GETVERSION haspds_GetVersion = 0;
    HINSTANCE hhlinstDll=0;

    haspds_status_t retCode = HASPDS_STATUS_SUCCESS;
    haspds_error errorCode = 0;
    BYTE FinalMessageString[ MESSAGE_STRING_LEN ];
    DWORD size = 0;

    TCHAR szValue[MAX_PATH] = {0};
    DWORD dwBuffer = MAX_PATH;

    if (!GetEnvironmentVariable("CommonProgramFiles", szValue, MAX_PATH))
    {
      GetWindowsDirectory(szValue, MAX_PATH);
      szValue[3]=0;
      strcat(szValue, "programme\\gemeinsame dateien\\Aladdin Shared\\HASP\\haspds_windows.dll");
      if (!(hhlinstDll = LoadLibrary(szValue)))
      {
        szValue[3]=0;
        strcat(szValue, "Program Files\\Common Files\\Aladdin Shared\\HASP\\haspds_windows.dll");
        hhlinstDll = LoadLibrary(szValue);
      }
    }
    else
    {
      strcat(szValue, "\\Aladdin Shared\\HASP\\haspds_windows.dll");
      hhlinstDll = LoadLibrary(szValue);
    }

    if( !hhlinstDll ){
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to load library"),
        TEXT("Sentinel HASP Run-time removal"), MB_OK | MB_ICONINFORMATION);
      return ERROR_INSTALL_FAILURE;
    }

    haspds_Install = (HASPDS_INSTALL)GetProcAddress(hhlinstDll,"haspds_Install");
    haspds_UnInstall = (HASPDS_UNINSTALL)GetProcAddress(hhlinstDll,"haspds_UnInstall");
    haspds_GetInfo = (HASPDS_GETINFO)GetProcAddress(hhlinstDll,"haspds_GetInfo");
    haspds_GetLastErrorMessage = (HASPDS_GETLASTERRORMESSAGE)GetProcAddress(hhlinstDll,"haspds_GetLastErrorMessage");
    haspds_GetLastError = (HASPDS_GETLASTERROR)GetProcAddress(hhlinstDll,"haspds_GetLastError");
    haspds_GetVersion = (HASPDS_GETVERSION)GetProcAddress(hhlinstDll,"haspds_GetVersion");

    if( !haspds_Install || !haspds_UnInstall || !haspds_GetInfo || !haspds_GetLastErrorMessage || !haspds_GetLastError || !haspds_GetVersion) {
      MessageBox(GetForegroundWindow( ),
        TEXT("Failed to access library"),
        TEXT("Sentinel HASP Run-time removal"), MB_OK | MB_ICONINFORMATION);
      FreeLibrary(hhlinstDll);
      return ERROR_INSTALL_FAILURE;
    }

    stop();
    retCode = haspds_UnInstall(HASPDS_PARAM_KILLPROC);

    size = MESSAGE_STRING_LEN;
    errorCode = haspds_GetLastError();

    if (errorCode != HASPDS_ERR_OK)
    {
      size = MESSAGE_STRING_LEN;
      haspds_GetLastErrorMessage((char*)FinalMessageString,&size);

      // special case for -fremove
      if (errorCode == HASPDS_ERR_USEFR_REQUIRED)
        strcpy (FinalMessageString, "Drivers from previous installations are still present. Use the previous installer to completely remove these drivers.");

      MessageBox(GetForegroundWindow( ),
        FinalMessageString,
        TEXT("Sentinel HASP Run-time removal"), MB_OK | MB_ICONINFORMATION);

      if(!((errorCode == HASPDS_ERR_INSERT_REQUIRED)
      || (errorCode == HASPDS_ERR_USEFR_REQUIRED)
      || (errorCode == HASPDS_ERR_USEHINST_REQUIRED)
      || (errorCode == HASPDS_ERR_REBOOT_REQUIRED)))
      {
          FreeLibrary(hhlinstDll);
          return ERROR_INSTALL_FAILURE;
      }
    }

    FreeLibrary(hhlinstDll);
    return ERROR_SUCCESS;
}

