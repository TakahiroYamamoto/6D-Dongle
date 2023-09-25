/*! \file haspdsd_sample.h
 *  \brief Sentinel RTE Sample
 *
 *  (c) 2013 SafeNet Inc. All rights reserved.
 *
 *  haspdsd_sample.h - Sentinel Run-time Environment Installer sample
 *
 */

#define HINSTALL_ICON 200
#ifndef SPI_GETWORKAREA
#define SPI_GETWORKAREA            48
#endif

#define MAXNUMOFPARAM 64
#define MAX_MSG 2048

#define IBM_COMP "IBM computer"
#define NEC_COMP "NEC computer"

#define HASP_DRIVER_INSTALLED       "Sentinel Device Driver is installed.\n"
#define HASP_DRIVER_NOT_INSTALLED   "Sentinel Device Driver is not installed.\n"

#define NT_PROGRAM_TITLE_NOVER      "Sentinel Run-time Environment Installer"
#define NT_PROGRAM_TITLE            "Sentinel Run-time Environment Installer v. %s.%s"

#define HASPDS_EXE_PARAM_KILLPROC    0x00000001  /* killproc argument present         */
#define HASPDS_EXE_PARAM_INSTALL     0x00000002  /* install argument present          */
#define HASPDS_EXE_PARAM_REMOVE      0x00000004  /* uninstall argument present        */
#define HASPDS_EXE_PARAM_HELP        0x00000008  /* help argument present             */
#define HASPDS_EXE_PARAM_NOMSG       0x00000010  /* nomsg argument present            */
#define HASPDS_EXE_PARAM_INFO        0x00000020  /* info arguument present            */
#define HASPDS_EXE_PARAM_CRITMSG     0x00000040  /* critical message argument present */
#define HASPDS_EXE_PARAM_FREMOVE     0x00000080  /* uninstall argument present        */
#define HASPDS_EXE_PARAM_VERBOSE     0x00000100  /* verbose argument present          */
#define HASPDS_EXE_PARAM_FINSTALL    0x00000200  /* force install if other windows    */
                                                 /* installs in running.              */
#define HASPDS_EXE_PARAM_X64         0x00000400  /* 64bit installer spawned from 32bit*/
#define HASPDS_EXE_PARAM_GETVER      0x00000800  /* get version from dll   */
#define HASPDS_EXE_PARAM_AV          0x00001000  /* set the AvirSpceial parameter     */
#define HASPDS_EXE_PARAM_NOAV        0x00002000  /* delete the AvirSpceial parameter  */
#define HASPDS_EXE_PARAM_PURGE       0x00004000  /* remove all installations          */
#define HASPDS_EXE_PARAM_NOMAXVER    0x00008000  /* do not care about max os version  */
#define HASPDS_EXE_PARAM_REPAIR      0x00010000  /* repair a previous installation    */
#define HASPDS_EXE_PARAM_CHKLLM      0x00020000  /* check and dispplay existing LLM sessions  */
#define HASPDS_EXE_PARAM_FSRVSTOP    0x00040000  /* force legacy server stop  */


typedef struct Info{
  HASPDS_INFOEX* Hhl;
  HASPDS_VENDOR_INFO_EX* Vendor;
  ULONG InstVer;
  ULONG DrvPkgVersion;
  ULONG ContainedPkgVer;
}INFO,*PINFO;

BOOL CALLBACK SessionsDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam );
void DisplaySessions( HWND hDlg, LLM_SESSIONS* sessions );
BOOL CALLBACK UsageDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam );
void PrintUsageMsg( HINSTANCE hInstance ,DWORD Version);
void MessageDisplay( HWND hWnd ,char* Message,DWORD Version);
void DisplayInfo( HWND hWnd, INFO* info );
void ConvertCmdLn( char* CmdLine, int *argc, char **argv );
LRESULT CALLBACK WndProc( HWND hWnd, UINT message, WPARAM uParam, LPARAM lParam );
HWND CreateWaitDialog( HINSTANCE hInstance,int Show);
void AskForReboot(void);
DWORD hhls_GetSystemVersion(void);
BOOL IsWow64(void);



typedef haspds_status_t  (__stdcall* HASPDS_INSTALL)(unsigned long Param);
typedef haspds_status_t  (__stdcall* HASPDS_UNINSTALL)(unsigned long Param);
typedef haspds_status_t  (__stdcall* HASPDS_GETINFO)(PHASPDS_INFO pInfo,unsigned long * pSize);
typedef haspds_status_t  (__stdcall* HASPDS_GETINFOEX)(PHASPDS_INFOEX pInfo,unsigned long * pSize);
typedef haspds_error     (__stdcall* HASPDS_GETLASTERROR)(void);
typedef haspds_status_t  (__stdcall* HASPDS_GETLASTERRORMESSAGE)(char* pBuffer,unsigned long* pSize);
typedef haspds_status_t (__stdcall* HASPDS_SETPARAMETER)(DWORD parameter);
typedef unsigned long  (__stdcall* HASPDS_GETVERSION)(void);
typedef int (__stdcall* PSetupPromptReboot)(HSPFILEQ FileQueue,HWND Owner,BOOL ScanOnly);
typedef haspds_status_t (__stdcall* HASPDS_GETVENDORINFO)(HASPDS_VENDOR_INFO* Buffer,unsigned long* size); 
typedef haspds_status_t (__stdcall* HASPDS_GETVENDORINFOEX)(HASPDS_VENDOR_INFO_EX* Buffer,unsigned long* size); 
typedef unsigned long (__stdcall* HASPDS_EXITPROCESS)(void);
typedef haspds_status_t (__stdcall* HASPDS_GETLLMSESSIONS)(LLM_SESSIONS* Buffer,size_t* size); 


typedef HRESULT (WINAPI *PRegisterApplicationRestart)(PCWSTR wCmdLine,DWORD Flags);
typedef HRESULT (WINAPI *PUnregisterApplicationRestart)(void);



#include "usage.h"

