/*
**  
**
**  haspdsd_sample.cpp - Sentinel Run-time Environment Installer sample
**
*/


#include <windows.h>
#include <winbase.h>
#include <stdlib.h>
#include <stdio.h>
#include <process.h>
#include "resource.h"
#include <setupapi.h>
#include "haspds.h"
#include "osdef.h"
#include "haspdsd_sample.h"

/*! \file haspdsd_sample.cpp 
    \brief HASP RTE Sample
*/


#pragma warning(disable:4201)
#pragma warning (disable:4996)




#ifdef LOG
FILE* srmlogfh;
#endif


#ifdef WIN64
#ifdef LEGACY
#define DLLNAME      "hdinst_windows_x64.dll"
#else
#define DLLNAME      "haspds_windows_x64.dll"
#endif //LEGACY
#else
#ifdef LEGACY
#define DLLNAME      "hdinst_windows.dll"
#else
#define DLLNAME      "haspds_windows.dll"
#endif
#endif


extern BOOL GetProductVersion(char * szFile, char * buffer);

#define MESSAGE_STRING_LEN 5000

#define RESTART_MAX_CMD_LINE 100
WCHAR wCmdLine[RESTART_MAX_CMD_LINE];
int NewMessageDisplay(HWND hWnd, HINSTANCE hInst,char* FinalMessageString,DWORD Version);

BYTE HinstallTitle[512];
BYTE OsTitle [512];
BYTE FinalMessageString[ MESSAGE_STRING_LEN ]={{0}};
char UsageMsg[ MESSAGE_STRING_LEN ]={{0}};
BYTE DisplayMessages[ MAX_MSG ]={{0}};
HWND HelphWnd;
HINSTANCE hInst;
HINSTANCE HelphInst;
DWORD endproc = 0;
DWORD ExitCode = 0;                                /** < in case of errors we exit with exitcode = Status << 16 | Error */

HANDLE hthread1=0;


HASPDS_GETVERSION hhls_GetVersion = 0;
HASPDS_GETLASTERRORMESSAGE hhls_GetLastErrorMessage = 0;                                
HASPDS_EXITPROCESS hhls_ExitProcess=0;

PRegisterApplicationRestart pRegisterApplicationRestart = NULL;
PUnregisterApplicationRestart pUnregisterApplicationRestart = NULL;


BOOL CALLBACK
InfoDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam );
void CreateDisplayInfo( HINSTANCE hInstance, HASPDS_INFOEX* HhlInfo,HASPDS_VENDOR_INFO_EX* VendorInfo,DWORD InstVer );
void ArrangeHelp( char *UsageMsg );


#define HASPDS_INSTALL_HLP        " - Installs the Sentinel Device Driver."
#define HASPDS_HELP_HLP           " - Displays this screen."
#define HASPDS_INFO_HLP           " - Lists the driver versions that are installed, and those that are contained in this installer."
#define HASPDS_REMOVE_HLP         " - Removes Sentinel Run-time Environment. This switch cannot be used if HASP4 or \n\t\tHardlock legacy drivers are present."
#define HASPDS_KILLPROCESS_HLP    " - Enables the installation program to terminate processes accessing the driver."
#define HASPDS_NOMSG_HLP          " - Disables message display.  No messages are displayed when this switch is used."
#define HASPDS_CRITICALMSG_HLP    " - Displays only critical messages, such as failures or reboot instructions."
#define HASPDS_FREMOVE_HLP        " - Removes the Sentinel Run-time Environment. Previously installed HASP4 and \n\t\tHardlock drivers will remain on your system but are unusable."
#define HASPDS_VERBOSE_HLP        " - Adds extended information in the install logfile."
#define HASPDS_FINSTALL_HLP       " - Ignores other Windows installations processes. Relevant only with the -i switch."
#define HASPDS_FSRVSTOP_HLP       " - Automatically stops HASP Loader or Hardlock server.Relevant only with the -i switch."
#define HASPDS_CHKLLM_HLP         " - Checks for LLM active sesions."


typedef struct ValidArgs{
    char* SwitchName;
    char* ShortSwitch;
    DWORD value;
    char* help;
    DWORD Mandatory;
    DWORD ValidOs;
}VALID_ARGS;

VALID_ARGS ValidArguments[]=
{
    {"-install","-i",HASPDS_EXE_PARAM_INSTALL,HASPDS_INSTALL_HLP,1, VALID_OS | WINX64 },
    {"-remove","-r",HASPDS_EXE_PARAM_REMOVE,HASPDS_REMOVE_HLP,1,VALID_OS  | WINX64},
    {"-fremove","-fr",HASPDS_EXE_PARAM_FREMOVE,HASPDS_FREMOVE_HLP,1,VALID_OS  | WINX64},
    {"-info","-info",HASPDS_EXE_PARAM_INFO,HASPDS_INFO_HLP,1,VALID_OS  | WINX64},
    {"-help","-h",HASPDS_EXE_PARAM_HELP,HASPDS_HELP_HLP,1,VALID_OS  | WINX64},
    {"-?","-?",HASPDS_EXE_PARAM_HELP,HASPDS_HELP_HLP,1,VALID_OS},
    {"-killprocess","-kp",HASPDS_EXE_PARAM_KILLPROC,HASPDS_KILLPROCESS_HLP,0,VALID_OS_NT | WINX64},
    {"-nomsg","-nomsg",HASPDS_EXE_PARAM_NOMSG,HASPDS_NOMSG_HLP,0,VALID_OS  | WINX64},
    {"-criticalmsg","-cm",HASPDS_EXE_PARAM_CRITMSG,HASPDS_CRITICALMSG_HLP,0,VALID_OS  | WINX64},
    {"-fi","-fi",HASPDS_EXE_PARAM_FINSTALL,HASPDS_FINSTALL_HLP,0,VALID_OS  | WINX64},
    {"-getver","-getver",HASPDS_EXE_PARAM_GETVER,NULL,1,VALID_OS  | WINX64},
    {"-v","-v",HASPDS_EXE_PARAM_VERBOSE,NULL,0,VALID_OS  | WINX64},                             
    {"-fss","-fss",HASPDS_EXE_PARAM_FSRVSTOP,HASPDS_FSRVSTOP_HLP,0,VALID_OS  | WINX64}, 
    {"-chkllm","-chkllm",HASPDS_EXE_PARAM_CHKLLM,HASPDS_CHKLLM_HLP,0,VALID_OS  | WINX64}, 
    {0,0,0,0,0,0}
};


typedef struct ThreadArg{
    DWORD Parameters;
    haspds_status_t Res;
    HWND hWnd;
    HINSTANCE hInstance;
    HANDLE hThread;
}THREAD_ARG;


DWORD WINAPI InstallProc(LPVOID lpParameter)
{
    CHAR TempPath[MAX_PATH]={0};
    WCHAR DllPath[MAX_PATH]={0};                                            // no sample
    HASPDS_INSTALL hhls_Install = 0;
    HASPDS_INSTALL hhls_UnInstall = 0;
    HASPDS_GETINFOEX hhls_GetInfo = 0;
    HASPDS_GETVENDORINFO hhls_GetVendorInfo = 0;
	HASPDS_GETVENDORINFOEX hhls_GetVendorInfoEx = 0;
    HASPDS_SETPARAMETER  hhls_SetParaneter = 0;
    HASPDS_GETLASTERROR hhls_GetLastError = 0;
    HASPDS_GETLLMSESSIONS hhls_GetLlmSessions = 0;
    HINSTANCE hhlinstDll=0;
    HMODULE SetupApiDll = 0;
    PSetupPromptReboot pSetupPromptReboot = NULL;
    DWORD Version = 0;
    THREAD_ARG* args = (THREAD_ARG*)lpParameter;
    haspds_status_t Res=HASPDS_STATUS_SUCCESS,Res1;
    DWORD size = 0;
    DWORD OSVer = 0;
    haspds_error lasterr;
    char* Buffer = 0;
    char* Buffer1 = 0;



    OSVer = hhls_GetSystemVersion();

    if( OSVer == 0 ){
        Res = HASPDS_STATUS_FAILED;
        lasterr = HASPDS_ERR_OS_NOT_SUPPORTED;
        strcpy((char*)FinalMessageString,"Operating system not supported.");     
        goto err_ok;
    }




    strcat((char*)TempPath, DLLNAME);                                        
    
      hhlinstDll = LoadLibrary(TempPath);                                               
        
    if (!hhlinstDll)
    {
        Res = HASPDS_STATUS_FAILED;
        lasterr = HASPDS_ERR_INVALID_PARAM;
        sprintf((char*)FinalMessageString,"Could not find %s",TempPath);
        goto err_ok;
    }



    SetupApiDll = LoadLibrary("setupapi.dll");
    if (SetupApiDll == NULL)
    {
        Res = HASPDS_STATUS_FAILED;
        lasterr = HASPDS_ERR_LOAD_LIB;
        strcpy((char*)FinalMessageString,"Failed to load setupapi.dll.");
        goto err_ok;
    }

    pSetupPromptReboot = (PSetupPromptReboot)GetProcAddress(SetupApiDll,"SetupPromptReboot");
    if (pSetupPromptReboot == NULL)
    {
        Res = HASPDS_STATUS_FAILED;
        lasterr = HASPDS_ERR_FCT_PTR;
        strcpy((char*)FinalMessageString,"Failed get pointer to a setupapi.dll function.");
        goto err_ok;
    }

    hhls_Install = (HASPDS_INSTALL)GetProcAddress(hhlinstDll,"haspds_Install");
    hhls_UnInstall = (HASPDS_INSTALL)GetProcAddress(hhlinstDll,"haspds_UnInstall");
    hhls_GetInfo = (HASPDS_GETINFOEX)GetProcAddress(hhlinstDll,"haspds_GetInfoEx");
    hhls_GetVendorInfo = (HASPDS_GETVENDORINFO)GetProcAddress(hhlinstDll,"haspds_GetVendorInfo");
	hhls_GetVendorInfoEx = (HASPDS_GETVENDORINFOEX)GetProcAddress(hhlinstDll,"haspds_GetVendorInfoEx");
    hhls_GetLastErrorMessage = (HASPDS_GETLASTERRORMESSAGE)GetProcAddress(hhlinstDll,"haspds_GetLastErrorMessage");
    hhls_GetVersion = (HASPDS_GETVERSION)GetProcAddress(hhlinstDll,"haspds_GetVersion");
    hhls_SetParaneter=(HASPDS_SETPARAMETER)GetProcAddress(hhlinstDll,"haspds_SetParameter");
    hhls_GetLastError = (HASPDS_GETLASTERROR)GetProcAddress(hhlinstDll,"haspds_GetLastError");                   
    hhls_ExitProcess = (HASPDS_EXITPROCESS)GetProcAddress(hhlinstDll,"haspds_ExitProcess");
    hhls_GetLlmSessions = (HASPDS_GETLLMSESSIONS)GetProcAddress(hhlinstDll,"haspds_GetLlmSessions");

    if (!hhls_Install 
        || !hhls_GetLastError                                                                   
        || !hhls_UnInstall  
        || !hhls_GetInfo 
        || !hhls_GetVendorInfo 
		|| !hhls_GetVendorInfoEx
        || !hhls_GetLastErrorMessage 
        || !hhls_GetVersion     
        || !hhls_SetParaneter 
        || !hhls_GetLlmSessions
      )
    {
        Res = HASPDS_STATUS_FAILED;
        lasterr = HASPDS_ERR_FCT_PTR;
        strcpy((char*)FinalMessageString,"Failed to unpack drivers.");
        goto err_ok;
    }

    Version = hhls_GetVersion();

    ArrangeHelp( UsageMsg );
    if (args->Parameters & HASPDS_EXE_PARAM_HELP)
    {
        ShowWindow( args->hWnd, SW_HIDE );
        PrintUsageMsg( args->hInstance,Version );
        if (args->hWnd)
        {
            SendMessage(args->hWnd,WM_DESTROY,0,0);
            args->hWnd = 0;
        }
        if(args->hThread)
            TerminateThread(args->hThread,0);
        ExitCode = HASPDS_STATUS_FAILED << 16 | HASPDS_ERR_INVALID_PARAM;
        exit(0);
    }

    /**
     * check for LLM sessions if requested
     */
    if ( (args->Parameters & HASPDS_EXE_PARAM_CHKLLM) &&
	((args->Parameters & HASPDS_EXE_PARAM_INSTALL) || 
          (args->Parameters & HASPDS_EXE_PARAM_REMOVE || 
		args->Parameters & HASPDS_EXE_PARAM_FREMOVE ))){
       size_t size=0;
       haspds_status_t status = HASPDS_STATUS_SUCCESS;
       LLM_SESSIONS* sessions = NULL;

       /*
        * get size of the required information 
        */
       status = hhls_GetLlmSessions(0,&size);
       /*
        * in case of success no sessions or no drivers installed
        */
       if ( status == HASPDS_STATUS_SMALL_BUFFER ){
	 for(;;){
            sessions = (LLM_SESSIONS*)realloc(sessions,size);
           /*
            * if buffer null ignore the problem 
            */
           if( sessions == NULL )
             break;


           status = hhls_GetLlmSessions(sessions,&size);
           /*
            * we might have another session arrived
            */
           if( status == HASPDS_STATUS_SMALL_BUFFER )   
             continue;

	   break;
         }
         /*
          * if we have sessions display them and ask for decision
          */
          if( sessions != NULL && sessions->count != 0 ){
	    if( FALSE == DialogBoxParam( args->hInstance, MAKEINTRESOURCE( IDC_LLM_SESSIONS ), NULL, ( DLGPROC )SessionsDialog,(LPARAM)sessions ))
                 goto end_proc;
          }
       }
       
    }

    /**
     * simply perform install or uninstall
     */
    if (args->Parameters & HASPDS_EXE_PARAM_INSTALL)
    {
        DWORD InstArg = 0;
        InstArg |= (args->Parameters & HASPDS_EXE_PARAM_KILLPROC)? HASPDS_PARAM_KILLPROC : 0;
        InstArg |= (args->Parameters & HASPDS_EXE_PARAM_FINSTALL)? HASPDS_PARAM_FINSTALL : 0;
	InstArg |= (args->Parameters & HASPDS_EXE_PARAM_FSRVSTOP) ? HASPDS_PARAM_FSRVSTOP : 0;  /* force legacy server stop  */
        Res = hhls_Install(InstArg);
        lasterr = hhls_GetLastError();
    }

    if (args->Parameters & HASPDS_EXE_PARAM_REMOVE || 
	args->Parameters & HASPDS_EXE_PARAM_FREMOVE  )
    {
        DWORD UninstArg = 0;
        UninstArg |= (args->Parameters & HASPDS_EXE_PARAM_KILLPROC)? HASPDS_PARAM_KILLPROC : 0;
        UninstArg |= (args->Parameters & HASPDS_EXE_PARAM_FREMOVE)? HASPDS_PARAM_FREMOVE : 0;
	UninstArg |= (args->Parameters & HASPDS_EXE_PARAM_FSRVSTOP) ? HASPDS_PARAM_FSRVSTOP : 0;  /* force legacy server stop  */
        Res = hhls_UnInstall(UninstArg);
        lasterr = hhls_GetLastError();
    }



    if (args->Parameters & HASPDS_EXE_PARAM_INFO)
    {
        DWORD size = 0;
        Res = hhls_GetInfo((HASPDS_INFOEX*)Buffer,&size);
        if (Res == HASPDS_STATUS_SMALL_BUFFER)  
        {
            Buffer = (char*)LocalAlloc(LPTR,size);
            Res = hhls_GetInfo((HASPDS_INFOEX*)Buffer,&size);
        }

        lasterr = hhls_GetLastError();

        /**
        * get the vendor info
        */

        size = 0;
        Res1 = hhls_GetVendorInfoEx((HASPDS_VENDOR_INFO_EX*)Buffer1,&size);
        if (size)
        {
            if( Res1 == HASPDS_STATUS_SMALL_BUFFER )
            {
                Buffer1 = (char*)LocalAlloc(LPTR,size);
                Res1 = hhls_GetVendorInfoEx((HASPDS_VENDOR_INFO_EX*)Buffer1,&size);
                if (Res1 != HASPDS_STATUS_SUCCESS)
                {
                    Res = Res1;
                    if( Buffer1 ) LocalFree(Buffer1);
                }
            }
        }

        if( Res == HASPDS_STATUS_SUCCESS )
            if( args->hWnd )
                ShowWindow( args->hWnd, SW_HIDE );


          if( Res != HASPDS_STATUS_SUCCESS ){
            goto err_ok;
          }

          CreateDisplayInfo(args->hInstance,(HASPDS_INFOEX*)Buffer,(HASPDS_VENDOR_INFO_EX*)Buffer1,Version);

        if(args->hThread){
            endproc = 1;
            args->hThread = 0;
        }
        if( Buffer ){
          LocalFree(Buffer);
          Buffer = 0;
        }
        if( Buffer1 ){
          LocalFree(Buffer1);
          Buffer1 = 0;
        }
    }

err_ok:

    if (!(args->Parameters & HASPDS_EXE_PARAM_NOMSG))
    {
        switch(Res)
        {
            case HASPDS_STATUS_SUCCESS:
                if (!(args->Parameters & HASPDS_EXE_PARAM_CRITMSG))
                {
                    size = MESSAGE_STRING_LEN;
                    if (hhls_GetLastErrorMessage)
                        hhls_GetLastErrorMessage((char*)FinalMessageString,&size);
                }
                else
                {
                    if(args->hThread)
                    {
                        endproc = 1;
                        args->hThread = 0;
                    }
                    goto end_proc;
                }
                break;
            case HASPDS_STATUS_REBOOT_REQUIRED:
                Res = HASPDS_STATUS_SUCCESS;
                pSetupPromptReboot(NULL,NULL,FALSE);

                if (args->hThread)
                {
                    endproc = 1;
                    args->hThread=0;
                }
                goto end_proc;
                break;
            case HASPDS_STATUS_FAILED:
                if (FinalMessageString[0] == 0)
                {
                    size = MESSAGE_STRING_LEN;
                    hhls_GetLastErrorMessage((char*)FinalMessageString,&size); 
                    if (hhls_GetLastError() == HASPDS_ERR_OLD_BRANDED_INST)
                    {
                        char* errstr=0;
                        errstr = strstr((char*)FinalMessageString,"Error");
                        if (errstr != 0)
                            *errstr = 0;
                    }
                }
                break;
            case HASPDS_STATUS_REINSERT_REQUIRED:
            case HASPDS_STATUS_USEFR_REQUIRED:
            case HASPDS_STATUS_USEHINST_REQUIRED:
            case HASPDS_STATUS_INSERT_REQUIRED:
            case HASPDS_STATUS_WARNING:
                if (FinalMessageString[0] == 0)
                {
                    char* errstr=0;
                    size = MESSAGE_STRING_LEN;
                    hhls_GetLastErrorMessage((char*)FinalMessageString,&size);
                    errstr = strstr((char*)FinalMessageString,"Error");
                    if (errstr != 0)
                        *errstr = 0;
                }

                break;
        }
        if (args->hWnd && (args->Parameters & HASPDS_EXE_PARAM_INSTALL 
                           || args->Parameters & HASPDS_EXE_PARAM_HELP
                           || args->Parameters & HASPDS_EXE_PARAM_REMOVE
                           || args->Parameters & HASPDS_EXE_PARAM_FREMOVE
                           || args->Parameters & HASPDS_EXE_PARAM_AV
                           || args->Parameters & HASPDS_EXE_PARAM_NOAV
                           || (args->Parameters & HASPDS_EXE_PARAM_INFO && Res == HASPDS_STATUS_FAILED)))
        {
            if (OSVer & VALID_OS_W9X && Res == HASPDS_STATUS_SUCCESS)
                strcpy((char*)FinalMessageString,"Installation successful.\n"
                        "Note that the Run-time Environment on this operating "
                        "system only supports HASP HL, HASP4 and Hardlock.");

            if (OSVer & VALID_OS_W9X && Res == HASPDS_STATUS_REBOOT_REQUIRED)
                strcpy((char*)FinalMessageString,"Operation successfully completed."
                "Reboot system to activate new drivers.\nNote that the Run-time "
                "Environment on this operating system only supports HASP HL, HASP4 and Hardlock.");
	MessageDisplay(args->hWnd,(char*)FinalMessageString,Version);			        
        }
    }

end_proc:

    endproc = 1;
//    Sleep(1500);                                        // wait 1500 msec the proc to end
    WaitForSingleObject(hthread1,INFINITE);

    if( hhlinstDll )
    {
        FreeLibrary(hhlinstDll);
        hhlinstDll = NULL;
    }


    if (args->hWnd)
        SendMessage(args->hWnd,WM_DESTROY,0,0);

    args->Res = Res;

    if( Buffer ){
      LocalFree(Buffer);
      Buffer = 0;
    }
    if( Buffer1 ){
      LocalFree(Buffer1);
      Buffer1 = 0;
    }




    if (Res != HASPDS_STATUS_SUCCESS)
      ExitCode = Res << 16 | lasterr;

    if (args->Parameters & HASPDS_EXE_PARAM_CRITMSG || args->Parameters & HASPDS_EXE_PARAM_NOMSG)
        return ExitCode;
    else
        ExitThread(ExitCode);
}


DWORD WINAPI TickCountProc(LPVOID lpParameter)
{
    HWND hWnd = (HWND)lpParameter;

    for ( ; endproc!=1; )
    {
        Sleep(1000);
        InvalidateRect(hWnd,NULL,TRUE);
        UpdateWindow(hWnd);
    }

    ExitThread(0);
}

VOID OnPaint(HWND hwnd)
{
    PAINTSTRUCT ps;
    HDC hdc;
    char message[256] ={0};
    int j=0;
    static int i = 0;

    hdc = BeginPaint(hwnd, &ps);
    strcpy( message,"Please wait ");
    i = ( i + 1 ) % 12;
    for ( j = 0; j < i; j++)
    {
        strcat(message,".");
    }

    SetBkColor( hdc, 0x00c5c5c5 );
    SetBkMode( hdc, TRANSPARENT );
    TextOut( hdc, 10, 10, message, (int)strlen(message) );
    EndPaint(hwnd, &ps);
}




void strtolow(char* string)
{
    unsigned int i = 0;
    for ( i = 0; i < strlen(string); i++)
        string[i] = (char)tolower(string[i]);
}

DWORD ParseParameters(DWORD ArgNo,char**Args)
{
    unsigned int j,i = 0;
    DWORD Result = 0;
    char* param;
    DWORD Mandatory = 0;
    DWORD OSVer = hhls_GetSystemVersion();

    for ( i = 0; i < ArgNo; i++)
    {
        if (i != 0 && strlen(Args[i]) == 0)
            return Result;

        if (Args[i][0] != '/' && Args[i][0] != '-')
            return HASPDS_EXE_PARAM_HELP;

        param = Args[i];
        strtolow(++param);

        for ( j = 0; ValidArguments[j].SwitchName != 0; j++)
        {
            if (!strcmp(param,ValidArguments[j].SwitchName+1) || !strcmp(param,ValidArguments[j].ShortSwitch+1))
            {
                if (Result & ValidArguments[j].value)
                    return HASPDS_EXE_PARAM_HELP;

                Result |= ValidArguments[j].value;
                Mandatory |= ValidArguments[j].Mandatory;

                if (ValidArguments[j].ValidOs & OSVer)
                    if (OSVer & WINX64)
                        if (!(ValidArguments[j].ValidOs & WINX64))
                             return HASPDS_EXE_PARAM_HELP;
        
                break;
            }
        }
        if( ValidArguments[j].SwitchName == 0 )
            return HASPDS_EXE_PARAM_HELP;
    }

    /**
     * avoid contradiction between parameters
     */
    if( ((Result & HASPDS_EXE_PARAM_AV) || (Result & HASPDS_EXE_PARAM_NOAV)) && !(Result & HASPDS_EXE_PARAM_INSTALL)){
        Result = HASPDS_EXE_PARAM_HELP;
        goto end_parse;
   
    }
    if ((Result & HASPDS_EXE_PARAM_INSTALL) && 
		((Result & HASPDS_EXE_PARAM_REMOVE) || 
		(Result & HASPDS_EXE_PARAM_FREMOVE) )){
        Result = HASPDS_EXE_PARAM_HELP;
        goto end_parse;
    }

    if ((Result & HASPDS_EXE_PARAM_FINSTALL) && !(Result & HASPDS_EXE_PARAM_INSTALL)){
        Result = HASPDS_EXE_PARAM_HELP;
        goto end_parse;
    }

    if (Result == 0){
        Result = HASPDS_EXE_PARAM_HELP;
        goto end_parse;
    }

    if (Mandatory == 0)
        Result = HASPDS_EXE_PARAM_HELP;

end_parse:
    return Result;
}

void ArrangeHelp( char *UsageMsg )
{
    int i;
    DWORD version;
    char MajorVer[4]={0};
    char MinorVer[4]={0};
    DWORD OSVer = hhls_GetSystemVersion();


    const char* banner =
        "\n"
        ""
        "\n"
        "\n"
        "Usage: haspdinst <mandatory switch> <optional switch>\n"
        "\n";


    const char* footnote =
        "Note: Switch character also can be \"/\"\n";


    memset( UsageMsg,0x0,MESSAGE_STRING_LEN);
    strcpy( UsageMsg, "Sentinel Run-time Environment Installer v. " );

    version = hhls_GetVersion();

    itoa(version >> 8 ,MajorVer,10);
    itoa(version & 0x000000FF ,MinorVer,10);
    strcat((char*)UsageMsg,(char*)MajorVer);
    strcat((char*)UsageMsg,".");
    strcat((char*)UsageMsg,(char*)MinorVer);


    strcat( UsageMsg, banner );
    strcat( UsageMsg, footnote );
    strcat( UsageMsg,"\nMandatory switches:\n\n");
    for (i = 0; ValidArguments[i].Mandatory != 0; i++ )
    {
        if (ValidArguments[i].help != NULL
            && ValidArguments[i].ValidOs & OSVer 
            && (OSVer & WINX64 ? (ValidArguments[i].ValidOs & WINX64):1))
        {
            strcat( UsageMsg, ValidArguments[i].SwitchName );
            if (strcmp(ValidArguments[i].SwitchName ,ValidArguments[i].ShortSwitch))
            {
                strcat( UsageMsg, " (");
                strcat( UsageMsg, ValidArguments[i].ShortSwitch+1);
                strcat( UsageMsg, ")");
            }
            strcat( UsageMsg, ValidArguments[i].help );
            strcat( UsageMsg, "\n" );
            strcat( UsageMsg, "\n" );
        }
    }


    strcat( UsageMsg,"Optional switches:\n\n");
    strcat( UsageMsg,"Note: Optional switches can only be used with the following mandatory switches: -i,-r and -fr.\n\n");
    for (; ValidArguments[i].SwitchName != 0 ; i++)
    {
        if (ValidArguments[i].help != NULL
                && ValidArguments[i].ValidOs & OSVer
                && (OSVer & WINX64 ? (ValidArguments[i].ValidOs & WINX64):1))
        {
            strcat( UsageMsg, ValidArguments[i].SwitchName );
            strcat( UsageMsg, " (");
            strcat( UsageMsg, ValidArguments[i].ShortSwitch+1);
            strcat( UsageMsg, ")");
            strcat( UsageMsg, ValidArguments[i].help );
            strcat( UsageMsg, "\n" );
            strcat( UsageMsg, "\n" );
        }
    }

}

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
    int argc_Orign = 0;
    int argc = 0;
    char *argv_Orign[MAXNUMOFPARAM]={0};
    char **argv;
    HWND hWnd = 0;
    int CommandLineSize;
    char HelpCmdLine[] = {"\\help"};
    DWORD Parameters = 0;
    THREAD_ARG args = {0};
    MSG msg;
    HANDLE hthread=0;
    ULONG ThreadId;
    int bRet = 0;
    DWORD OSVer = hhls_GetSystemVersion();
    HMODULE Krnl=LoadLibrary("kernel32.dll");
    HANDLE hMutex;

    UNREFERENCED_PARAMETER(nCmdShow);
    UNREFERENCED_PARAMETER(hPrevInstance);
    hInst = hInstance;

#ifdef LOG
    srmlogfh = fopen("testfile.txt","w");
#endif  


#ifndef WIN64
    if ((OSVer & VISTA) || (OSVer & WIN_2K8))
    {
        if (Krnl)
        {
            pRegisterApplicationRestart = (PRegisterApplicationRestart)GetProcAddress(Krnl,"RegisterApplicationRestart");
            pUnregisterApplicationRestart = (PUnregisterApplicationRestart)GetProcAddress(Krnl,"UnregisterApplicationRestart");
        }
    }
#endif

#ifndef WIN64
    MultiByteToWideChar( CP_ACP, 0, lpCmdLine,strlen(lpCmdLine)+1, wCmdLine, RESTART_MAX_CMD_LINE );
#endif

    hWnd = NULL;
    HelphInst = hInstance;

    CommandLineSize = (int)strlen( lpCmdLine );
    if (!CommandLineSize)
        lpCmdLine = (LPSTR)HelpCmdLine;

    ConvertCmdLn( lpCmdLine, &argc_Orign, argv_Orign );
    argv = argv_Orign;
    argc = argc_Orign;
    Parameters = ParseParameters(argc,argv);

    hMutex = CreateMutex(NULL,TRUE,"haspds_mutex");
    if (hMutex && GetLastError() == ERROR_ALREADY_EXISTS)
    {
        if (!(Parameters & HASPDS_EXE_PARAM_NOMSG) && !(Parameters & HASPDS_EXE_PARAM_CRITMSG))
        {
            strcpy((char*)FinalMessageString,"Another instance of the installer is already running.");
            CloseHandle(hMutex);
            MessageBox(NULL,(char*)FinalMessageString,NT_PROGRAM_TITLE_NOVER,MB_OK);
        }
        exit(1);
    }




    if (!(Parameters & HASPDS_EXE_PARAM_NOMSG) && !(Parameters & HASPDS_EXE_PARAM_CRITMSG))
    {
        hWnd = CreateWaitDialog( hInstance ,1);
        hthread1 = CreateThread(NULL,0,TickCountProc,hWnd,0,&ThreadId);
        args.hThread = hthread1;
    }
    else
    {
        hWnd = CreateWaitDialog( hInstance ,0);
        args.hThread = 0;
    }

    args.Parameters = Parameters;
    args.hWnd = hWnd;
    args.hInstance = hInstance;

    hthread = CreateThread(NULL,0,InstallProc,&args,0,&ThreadId);

    while ((bRet = GetMessage( &msg, NULL, 0, 0 )) != 0)
    {
        if (bRet == -1)
            break;
        else    
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }
    
    WaitForSingleObject(hthread,INFINITE);

    CloseHandle(hMutex);

    return ExitCode;
}

HWND CreateWaitDialog( HINSTANCE hInstance ,int Show)
{
    WNDCLASS  wc;
    PAINTSTRUCT PaintStruct;
    HDC hDC;
    RECT SystemRect;
    int x, y, w, h;
    HWND hWnd= 0;

    char szAppName[] = "hinstall";
    char szTitle[] = "Sentinel Run-time Environment Installer ";

    hInst = hInstance;

    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = (WNDPROC) WndProc;
    wc.cbClsExtra = 0;
    wc.cbWndExtra = 0;
    wc.hInstance = hInst;
    wc.hIcon = LoadIcon( hInst, MAKEINTRESOURCE ( HINSTALL_ICON ) );
    wc.hCursor = LoadCursor (NULL, IDC_ARROW);
    wc.hbrBackground = (HBRUSH) (COLOR_MENU + 1);
    wc.lpszMenuName = NULL;
    wc.lpszClassName = szAppName;

    RegisterClass( &wc );

    w = 220;
    h = 60;
    SystemParametersInfo( SPI_GETWORKAREA, 0, &SystemRect, 0 );
    x = ( SystemRect.right - SystemRect.left ) / 2 - w / 2;
    y = ( SystemRect.bottom - SystemRect.top ) / 2 - h / 2;
    hWnd = CreateWindow( szAppName, szTitle, WS_CAPTION,
                            x, y, w, h,
                            NULL, NULL, hInst, NULL);

    if (!hWnd)
        return 0;

    if (Show)
        ShowWindow(hWnd, SW_RESTORE );
    else
        ShowWindow(hWnd, SW_HIDE);

    hDC = BeginPaint( hWnd, &PaintStruct );
    SetBkColor( hDC, 0x00c5c5c5 );
    SetBkMode( hDC, TRANSPARENT );
    TextOut( hDC, 10, 10, "Please wait .", 13 );
    EndPaint( hWnd, &PaintStruct );

    UpdateWindow (hWnd);
    return hWnd;
}

LRESULT CALLBACK WndProc( HWND hWnd, UINT message, WPARAM uParam, LPARAM lParam )
{
     switch (message)
     {
        case WM_COMMAND:
            break;

        case WM_QUIT:
        case WM_DESTROY:
            PostQuitMessage( 0 );
            break;
        case WM_PAINT:
            OnPaint(hWnd);
            break;
        case WM_QUERYENDSESSION:
#ifndef WIN64
            if (pRegisterApplicationRestart)
                pRegisterApplicationRestart(wCmdLine,0xf);
#endif
            return 1L;
        case WM_ENDSESSION:
            /**
             * Vista Restart managber handling - register for a restart and exit 
             */
            if (lParam == 1)  
            {
                if (uParam == TRUE)
                {
                    if (hhls_ExitProcess)
                        hhls_ExitProcess();
                    ExitProcess(0);
                }
#ifndef WIN64
                else
                    if( pUnregisterApplicationRestart )
                        pUnregisterApplicationRestart();
#endif
                return 0L;
            }    
        default:
            return DefWindowProc (hWnd, message, uParam, lParam);
    }
    return 0L;
}

void ConvertCmdLn( char* CmdLine, int *argc, char **argv )
{
    char* pStr = CmdLine;
    int i;

    for (i = 0; i < MAXNUMOFPARAM; i++)
    {
        argv[i] = pStr;
        pStr = strchr(pStr,' ');
        if (pStr == NULL)
            break;
        else
            for (; *pStr == ' '; *pStr++ = '\0')
                ;
    }
    *argc = i;
    if (i < MAXNUMOFPARAM)
        *argc += 1;
}

void CreateDisplayInfo( HINSTANCE hInstance, HASPDS_INFOEX* HhlInfo,HASPDS_VENDOR_INFO_EX* VendorInfo,DWORD InstVer )
{

    INFO info;

    info.Hhl = HhlInfo;
    info.Vendor = VendorInfo;
    info.InstVer = InstVer;

    DialogBoxParam( hInstance, MAKEINTRESOURCE( IDC_INFO ), NULL, ( DLGPROC )InfoDialog,(LPARAM)&info );
}

unsigned long GetStringCaptionSize(HWND hWnd,char* Text)
{
  NONCLIENTMETRICS metrics;
  HDC hDc = GetDC(hWnd);
  HFONT hFont;
  SIZE size;
  DWORD width = 0;
  HGDIOBJ oldFont = NULL;

  if( hDc == NULL ){
    return 0;
  }

  metrics.cbSize = sizeof(NONCLIENTMETRICS);
  if(!SystemParametersInfo(SPI_GETNONCLIENTMETRICS,sizeof(NONCLIENTMETRICS),&metrics,0)){
    width = 0;
    goto end;
  }

  hFont = CreateFontIndirect(&metrics.lfCaptionFont);
  if( hFont == NULL ){
    width = 0;
    goto end;
  }

  oldFont = SelectObject(hDc,hFont);
  if( oldFont == NULL ){
    width = 0;
    goto end;
  }

  if( 0 == GetTextExtentPoint32(hDc,Text,strlen(Text),&size)){
    width = 0;
  }
  else
    width = size.cx;

end:
  if( oldFont ) SelectObject(hDc,oldFont);
  if( hFont ) DeleteObject(hFont);
  if( hDc ) ReleaseDC(hWnd,hDc);
 
  return width;
}

void DisplayInfo( HWND hDlg, INFO* info )
{
    char MajorVer[4]={0};
    char MinorVer[4]={0};
    WORD i=0;
    RECT scrListRect,cltListRect;
    RECT scrDlgRect,cltDlgRect;
    RECT scrOkRect,cltOkRect;
    RECT rect;
    WORD size = 0,sizever = 0;
    WORD maxlen = 0,maxver = 0;
    HWND hWnd = GetDlgItem( hDlg, IDC_TEXT );
    HDC hdc = GetDC(hWnd);
    WORD space = 0;
    HWND hOk;
    RECT ListItemRect;
    WORD itemcount;
    WORD ListHeight;
    int borderWidth = GetSystemMetrics(SM_CXBORDER);
    int captionButtonWidth = GetSystemMetrics(SM_CXSIZE);
    int captionWidth=0;
    DWORD OSVer = hhls_GetSystemVersion(); 
	int ver1 = 0,ver2 = 0, ver3 = 0, ver4 = 0;
	
    EnableWindow(hWnd,FALSE);

    /**
     * calibrate the text in the window
     */


    for (i = 0; i < info->Hhl->Info.ItemsNo; i++)
    {
        size = (WORD)GetTabbedTextExtent(hdc,info->Hhl->Info.Items[i].FileName,(int)strlen(info->Hhl->Info.Items[i].FileName),0,NULL);
        if (maxlen < size)
            maxlen = size;

        itoa(info->Hhl->Info.Items[i].PackageVersion >> 8 ,MajorVer,10);
        sizever = (WORD)GetTabbedTextExtent(hdc,MajorVer,(int)strlen(MajorVer),0,NULL);
        if (maxver < sizever)
            maxver = sizever;

    }

    size = (WORD)GetTabbedTextExtent(hdc,"        ",(int)strlen("        "),0,NULL);
    maxlen =(WORD)(maxlen + size);

    space = (WORD)GetTabbedTextExtent(hdc," ",1,0,NULL);

    itoa(info->InstVer >> 8 ,MajorVer,10);
    itoa(info->InstVer & 0x000000FF ,MinorVer,10);
    strcat((char*)DisplayMessages,(char*)MajorVer);
    strcat((char*)DisplayMessages,".");
    strcat((char*)DisplayMessages,(char*)MinorVer);


    strcpy ( (char*)DisplayMessages, "Sentinel Run-time Environment Installer  v. ");

    itoa(info->InstVer >> 8 ,MajorVer,10);
    itoa(info->InstVer & 0x000000FF ,MinorVer,10);
    strcat((char*)DisplayMessages,(char*)MajorVer);
    strcat((char*)DisplayMessages,".");
    strcat((char*)DisplayMessages,(char*)MinorVer);

    SetWindowText(hDlg,(char*)DisplayMessages);

    captionWidth = GetStringCaptionSize(hDlg,(char*)DisplayMessages);
    captionWidth += borderWidth + captionButtonWidth;

    GetWindowRect(hDlg,&rect);

    if( (rect.right - rect.left) < captionWidth ){
      MoveWindow(hDlg,rect.left,rect.top,captionWidth,rect.bottom-rect.top,FALSE);
    }
    
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );

    if( !(OSVer & VALID_OS_W9X) ){
      if( info->Hhl->DrvInstPkgVer )
        sprintf((char*)DisplayMessages, " Installed Package Drivers %d.%d:",info->Hhl->DrvInstPkgVer >> 16,info->Hhl->DrvInstPkgVer & 0xFFFF );
      else
        sprintf((char*)DisplayMessages, " Installed Package Drivers : not installed" );
    }
    else
      sprintf((char*)DisplayMessages, " Installed Package Drivers:");
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );
    for (i = 0; i < info->Hhl->Info.ItemsNo; i++)
    {
	if( info->Hhl->Info.Items[i].InstalledVersion ){
          sprintf((char*)DisplayMessages,"        %-16s %5d.%d",
                    (char*)(info->Hhl->Info.Items[i].FileName),
                    info->Hhl->Info.Items[i].InstalledVersion >> 8 ,
                    info->Hhl->Info.Items[i].InstalledVersion & 0x000000FF);
        }
	else
          sprintf((char*)DisplayMessages,"        %-16s %s",
                    (char*)(info->Hhl->Info.Items[i].FileName),
                    "not loaded");

        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
    }

    if( !(OSVer & VALID_OS_W9X) )
      sprintf((char*) DisplayMessages, " Installer Package Drivers %d.%d:",info->Hhl->DrvPkgVer>>16,info->Hhl->DrvPkgVer & 0xFFFF );
    else
      sprintf((char*) DisplayMessages, " Installer Package Drivers:");

    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );

    for (i = 0; i < info->Hhl->Info.ItemsNo; i++)
    {
        sprintf((char*)DisplayMessages,"        %-16s %5d.%d",(char*)(info->Hhl->Info.Items[i].FileName),
                                info->Hhl->Info.Items[i].PackageVersion >> 8 ,
                                info->Hhl->Info.Items[i].PackageVersion & 0x000000FF);
        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
    }

    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );

    if (info->Vendor)
    {
        strcpy ((char*) DisplayMessages, " Installer Package Vendor Files:" );
        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );

        for (i = 0; i < info->Vendor->ItemsNo; i++)
        {
            char* point = strrchr((char*)(info->Vendor->Items[i].FileName),'.');

            if (!strcmp(point,".dll")){

				ver1 = (info->Vendor->Items[i].PackageVersion & 0xFFFF000000000000) >> 48;
				ver2 = (info->Vendor->Items[i].PackageVersion & 0x0000FFFF00000000) >> 32;
				ver3 = (info->Vendor->Items[i].PackageVersion & 0x00000000FFFF0000) >> 16;
				ver4 = (info->Vendor->Items[i].PackageVersion & 0x000000000000FFFF);
				
                if( (point - (char*)(info->Vendor->Items[i].FileName)) > 12 ){
                  info->Vendor->Items[i].FileName[12]=0;
                  sprintf((char*)DisplayMessages,"        %-12s...%s %2d.%d.%d.%d",(char*)(info->Vendor->Items[i].FileName),point,ver1,ver2,ver3,ver4
  								);
                }
                else
                  sprintf((char*)DisplayMessages,"        %-12s...%s %2d.%d.%d.%d",(char*)(info->Vendor->Items[i].FileName),point,ver1,ver2,ver3,ver4
  								);
            }
            else{
                if( (point - (char*)(info->Vendor->Items[i].FileName)) > 12 ){
                  info->Vendor->Items[i].FileName[12]=0;
                  sprintf((char*)DisplayMessages,"        %-12s",(char*)(info->Vendor->Items[i].FileName));
                  strcat((char*)DisplayMessages,"...");
                  strcat((char*)DisplayMessages,point);
                }
                else
                  sprintf((char*)DisplayMessages,"        %-12s",(char*)(info->Vendor->Items[i].FileName));
            }
            SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
        }
    }

    ReleaseDC(hWnd,hdc);

    /**
     * resize the window
     */

    /**
     * - get size of ListBox
     */
    SendMessage( hWnd, LB_GETITEMRECT, 0, (LPARAM)&ListItemRect );

    /**
     * get no of items
     */

    itemcount = (WORD)SendMessage( hWnd, LB_GETCOUNT, 0, (LPARAM)&ListItemRect );

    /**
     * - recompute the size of List
     */

    ListHeight = (WORD)(ListItemRect.bottom - ListItemRect.top)*itemcount;

    /**
     * resize the List
     */

    GetWindowRect(hWnd,&scrListRect);
    GetClientRect(hWnd,&cltListRect);
   

    GetWindowRect(hDlg,&scrDlgRect);
    GetClientRect(hDlg,&cltDlgRect);


    MoveWindow(hWnd, (cltDlgRect.right - cltListRect.right)/2,scrListRect.top-scrDlgRect.top,cltListRect.right,ListHeight + 20,TRUE);


    /**
     * - place the OK button
     */

    hOk = GetDlgItem(hDlg,IDC_OK);
    GetClientRect(hOk,&cltOkRect);

    GetWindowRect(hWnd,&scrListRect);
    GetClientRect(hWnd,&cltListRect);

    MoveWindow(hOk,(cltDlgRect.right - cltOkRect.right) / 2,scrListRect.top - scrDlgRect.top + cltListRect.bottom + 20 ,cltOkRect.right,cltOkRect.bottom,TRUE)  ;

    /**
     * now resize also the dialog box
     */

    GetWindowRect(hOk,&scrOkRect);

    MoveWindow(hDlg, scrDlgRect.left,scrDlgRect.top, cltDlgRect.right, scrOkRect.bottom - scrDlgRect.top + 40 ,TRUE);

}

void MessageDisplay( HWND hWnd,char*Message,DWORD Version )
{

    char MajorVer[4]={0};
    char MinorVer[4]={0};

    itoa(Version >> 8 ,MajorVer,10);
    itoa(Version & 0x000000FF ,MinorVer,10);

    sprintf((char*)HinstallTitle,NT_PROGRAM_TITLE,MajorVer,MinorVer);
 
    if(hWnd) ShowWindow( hWnd, SW_HIDE );

    MessageBox(hWnd, (LPCTSTR)Message, (LPCTSTR)HinstallTitle,MB_OK | MB_SYSTEMMODAL);
}

void PrintUsageMsg( HINSTANCE hInstance,DWORD Version )
{

    char MajorVer[4]={0};
    char MinorVer[4]={0};

    itoa(Version >> 8 ,MajorVer,10);
    itoa(Version & 0x000000FF ,MinorVer,10);

    sprintf ( (char*)HinstallTitle,NT_PROGRAM_TITLE,MajorVer,MinorVer);

    DialogBox( hInstance, MAKEINTRESOURCE( IDC_USAGE ), NULL, ( DLGPROC )UsageDialog );
}

BOOL CALLBACK InfoDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam )
{
    int wmId;

    UNREFERENCED_PARAMETER(lParam);
    wmId = LOWORD (uParam);

    switch (message)
    {
        /**
         *    Initialize all values to defualt values.
         */
        case WM_INITDIALOG:
            DisplayInfo(hDlg,(INFO*)lParam);
            return TRUE;

        case WM_COMMAND:
            if (wmId){
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            break;
        case WM_SYSCOMMAND:
            if (LOWORD(uParam) == SC_CLOSE)
            {
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            break;
    }
    return FALSE;
}



BOOL CALLBACK UsageDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam )
{
    int wmId;
    int x, y, i;
    char text[512];
    HWND hWnd;

    UNREFERENCED_PARAMETER(lParam);
    wmId = LOWORD (uParam);
   
    switch(message)
    {
        /**
         *    Initialize all values to defualt values.
         */
        case WM_INITDIALOG:
            hWnd = GetDlgItem( hDlg, IDC_TEXT );
            y = 0;
            do
            {
                x = 0;
                do
                {
                    if (UsageMsg[y] == 0x09)
                    {
                        for (i = 0; i < 3; i++, x++)
                            text[x] = ' ';
                    }
                    else
                    {
                        text[x] = UsageMsg[y];
                        x++;
                    }
                    y++;
                }
                while (UsageMsg[y] != 0x0a && UsageMsg[y] != 0) 
                    ;
                text[x] = 0x0;
                SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)text );
                while (UsageMsg[y] == 0x0a)
                {
                    if (UsageMsg[y + 1] == 0x0a)
                        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );
                    y++;
                }
            }
            while( UsageMsg[y] != '\0' )
                ;
           return TRUE;

        case WM_COMMAND:
            if (wmId == IDC_OK)
            {
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            break;
        case WM_SYSCOMMAND:
            if (LOWORD (uParam) == SC_CLOSE)
            {
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            break;
    }
    return FALSE;
}


BOOL CALLBACK SessionsDialog( HWND hDlg, UINT message, WPARAM uParam, LPARAM lParam )
{
    int wmId;

    LLM_SESSIONS *sessions = (LLM_SESSIONS*)lParam;
    wmId = LOWORD (uParam);

    switch (message)
    {
        /**
         *    Initialize all values to defualt values.
         */
        case WM_INITDIALOG:
            DisplaySessions(hDlg,sessions);
            return TRUE;

        case WM_COMMAND:
            if (wmId == IDC_OK ){
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            if (wmId == IDC_EXIT ){
                EndDialog (hDlg, FALSE);
                return TRUE;
            }
            break;
        case WM_SYSCOMMAND:
            if (LOWORD(uParam) == SC_CLOSE)
            {
                EndDialog (hDlg, TRUE);
                return TRUE;
            }
            break;
    }
    return FALSE;
}


void DisplaySessions( HWND hDlg, LLM_SESSIONS* sessions )
{
    char MajorVer[4]={0};
    char MinorVer[4]={0};
    WORD i=0;
    RECT scrListRect,cltListRect;
    RECT scrDlgRect,cltDlgRect;
    RECT scrOkRect,cltOkRect;
    RECT rect;
    WORD size = 0,sizever = 0;
    WORD maxlen = 0,maxver = 0;
    HWND hWnd = GetDlgItem( hDlg, IDC_TEXT );
    HDC hdc = GetDC(hWnd);
    WORD space = 0;
    HWND hOk;
    RECT ListItemRect;
    WORD itemcount;
    WORD ListHeight;
    int borderWidth = GetSystemMetrics(SM_CXBORDER);
    int captionButtonWidth = GetSystemMetrics(SM_CXSIZE);
    int captionWidth=0;
    DWORD OSVer = hhls_GetSystemVersion(); 



    EnableWindow(hWnd,FALSE);

    /**
     * calibrate the text in the window
     */


    for (i = 0; i < sessions->count; i++)
    {
        size = (WORD)GetTabbedTextExtent(hdc,sessions->sessions[i].machine,(int)strlen(sessions->sessions[i].machine),0,NULL);
        if (maxlen < size)
            maxlen = size;

#if 0
        itoa(info->Hhl->Info.Items[i].PackageVersion >> 8 ,MajorVer,10);
        sizever = (WORD)GetTabbedTextExtent(hdc,MajorVer,(int)strlen(MajorVer),0,NULL);
        if (maxver < sizever)
            maxver = sizever;
#endif
    }

    size = (WORD)GetTabbedTextExtent(hdc," ",(int)strlen(" "),0,NULL);
    maxlen =(WORD)(maxlen + size);

    space = (WORD)GetTabbedTextExtent(hdc," ",1,0,NULL);


    strcpy ( (char*)DisplayMessages, "Sentinel Local Licence Manager sessions: ");

    SetWindowText(hDlg,(char*)DisplayMessages);

    captionWidth = GetStringCaptionSize(hDlg,(char*)DisplayMessages);
    captionWidth += borderWidth + captionButtonWidth;

    GetWindowRect(hDlg,&rect);

    if( (rect.right - rect.left) < captionWidth ){
      MoveWindow(hDlg,rect.left,rect.top,captionWidth,rect.bottom-rect.top,FALSE);
    }
    
    SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)"" );
    for (i = 0; i < sessions->count; i++)
    {
        sprintf((char*)DisplayMessages," machine:%-16s %s pid: %d",
                    sessions->sessions[i].machine,
                    sessions->sessions[i].ip,
                    sessions->sessions[i].pid);

        SendMessage( hWnd, LB_ADDSTRING, 0, (LPARAM)(LPCTSTR)DisplayMessages );
    }


    ReleaseDC(hWnd,hdc);

    /**
     * resize the window
     */

    /**
     * - get size of ListBox
     */
    SendMessage( hWnd, LB_GETITEMRECT, 0, (LPARAM)&ListItemRect );

    /**
     * get no of items
     */

    itemcount = (WORD)SendMessage( hWnd, LB_GETCOUNT, 0, (LPARAM)&ListItemRect );

    /**
     * - recompute the size of List
     */

    ListHeight = (WORD)(ListItemRect.bottom - ListItemRect.top)*itemcount;

    /**
     * resize the List
     */

    GetWindowRect(hWnd,&scrListRect);
    GetClientRect(hWnd,&cltListRect);
   

    GetWindowRect(hDlg,&scrDlgRect);
    GetClientRect(hDlg,&cltDlgRect);


    MoveWindow(hWnd, (cltDlgRect.right - cltListRect.right)/2,scrListRect.top-scrDlgRect.top,cltListRect.right,ListHeight + 20,TRUE);


    /**
     * - place the OK button
     */

    hOk = GetDlgItem(hDlg,IDC_OK);
    GetClientRect(hOk,&cltOkRect);

    GetWindowRect(hWnd,&scrListRect);
    GetClientRect(hWnd,&cltListRect);

    MoveWindow(hOk,(cltDlgRect.right - cltOkRect.right) / 2 - cltOkRect.right,scrListRect.top - scrDlgRect.top + cltListRect.bottom + 40 ,cltOkRect.right,cltOkRect.bottom ,TRUE)  ;

    hOk = GetDlgItem(hDlg,IDC_EXIT);
    GetClientRect(hOk,&cltOkRect);

    GetWindowRect(hWnd,&scrListRect);
    GetClientRect(hWnd,&cltListRect);

    MoveWindow(hOk,(cltDlgRect.right - cltOkRect.right) / 2 + cltOkRect.right,scrListRect.top - scrDlgRect.top + cltListRect.bottom + 40 ,cltOkRect.right,cltOkRect.bottom ,TRUE)  ;
    /**
     * now resize also the dialog box
     */

    GetWindowRect(hOk,&scrOkRect);

    MoveWindow(hDlg, scrDlgRect.left,scrDlgRect.top, cltDlgRect.right, scrOkRect.bottom - scrDlgRect.top + 40,TRUE);

}
#ifndef WIN64
#endif

typedef BOOL (WINAPI *LPFN_ISWOW64PROCESS) (HANDLE hProcess,PBOOL Wow64Process);


BOOL IsWow64()
{
    BOOL bIsWow64 = FALSE;
 
    LPFN_ISWOW64PROCESS fnIsWow64Process = (LPFN_ISWOW64PROCESS)GetProcAddress(GetModuleHandle("kernel32"),"IsWow64Process");

    if (NULL != fnIsWow64Process)
    {
        if (!fnIsWow64Process(GetCurrentProcess(),&bIsWow64))
        {
           return FALSE;
        }
    }
    return bIsWow64;
}


DWORD hhls_GetSystemVersion()
{
    OSVERSIONINFOEX osvi;
    BOOL bOsVersionInfoEx;
    DWORD result;
    DWORD X64 = 0;
    SYSTEM_INFO sysInfo={0};

    GetSystemInfo(&sysInfo);
    if( IsWow64() || sysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64 
            || sysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64 )
        X64 = WINX64;

    /**
     * Try calling GetVersionEx using the OSVERSIONINFOEX structure.
     * If that fails, try using the OSVERSIONINFO structure.
     */

    ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));
    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
    bOsVersionInfoEx = GetVersionEx ((OSVERSIONINFO *) &osvi);
    if (!bOsVersionInfoEx)
    {
        result = GetLastError();
        osvi.dwOSVersionInfoSize = sizeof (OSVERSIONINFO);
        if (!GetVersionEx ( (OSVERSIONINFO *) &osvi))
            return FALSE;
    }

    switch (osvi.dwPlatformId)
    {
        /*
         * Test for the Windows NT product family.
         */
        case VER_PLATFORM_WIN32_NT:
            /*
             * Test for the specific product family.
             */
            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2)
                return X64 | WIN_2K3;

            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 1)
                return X64 | WIN_XP;

            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 0)
                return WIN_2K;

            if (osvi.dwMajorVersion <= 4)
                return WIN_NT;

            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0){
              if( osvi.wProductType == VER_NT_WORKSTATION ){
                return X64 | VISTA;
              }
              else{
                return X64 | WIN_2K8;
              }
            }
                                                                
            if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1){
              if( osvi.wProductType == VER_NT_WORKSTATION ){
                return X64 | WIN7;
              }
              else{
                return X64 | WIN_2K8R2;
              }
            }

            if (osvi.dwMajorVersion == 6 && (osvi.dwMinorVersion == 2 || osvi.dwMinorVersion == 3)){
                return X64 | WIN_8;
            }

            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0){
              if( osvi.wProductType == VER_NT_WORKSTATION ){
                return X64 | WIN_10;
              }
              else{
                return X64 | WIN_2016;
              }
            }


              
            break;

            /* 
             * Test for the Windows 95 product family.
             */
        case VER_PLATFORM_WIN32_WINDOWS:
            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 0)
                return WIN_95;

            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 10)
                return WIN_98;

            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 90)
                return WIN_ME;

        default:
            return FALSE;
    }

    return FALSE;
}


DWORD hhls_GetSystemVersion_Ex(DWORD* ServicePack)
{
    OSVERSIONINFOEX osvi;
    BOOL bOsVersionInfoEx;
    DWORD result;
    DWORD X64 = 0;
    SYSTEM_INFO sysInfo={0};

    GetSystemInfo(&sysInfo);
    if( IsWow64() || sysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64 
            || sysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_IA64 )
        X64 = WINX64;


    /**
     * Try calling GetVersionEx using the OSVERSIONINFOEX structure.
     * If that fails, try using the OSVERSIONINFO structure.
     */

    ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));
    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
    bOsVersionInfoEx = GetVersionEx ((OSVERSIONINFO *) &osvi);

    if (!bOsVersionInfoEx)
    {
        result = GetLastError();
        osvi.dwOSVersionInfoSize = sizeof (OSVERSIONINFO);
        if (!GetVersionEx((OSVERSIONINFO*)&osvi))
            return FALSE;
    }

    switch(osvi.dwPlatformId)
    {
        /**
         * Test for the Windows NT product family.
         */
        case VER_PLATFORM_WIN32_NT:
            /**
             * Test for the specific product family.
             */ 
            *ServicePack = osvi.wServicePackMajor;       
 
            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2)
                return X64 | WIN_2K3;

            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 1)
                return X64 | WIN_XP;

            if (osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 0)
                return WIN_2K;

            if (osvi.dwMajorVersion <= 4)
                return WIN_NT;

           if (osvi.dwMajorVersion >= 6)
                    return X64 | VISTA;

           if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 0)
              if( osvi.wProductType == VER_NT_WORKSTATION )
                    return X64 | VISTA;
              else
                    return X64 | WIN_2K8;

           if (osvi.dwMajorVersion == 6 && osvi.dwMinorVersion == 1)
              if( osvi.wProductType == VER_NT_WORKSTATION )
                    return X64 | WIN7;
              else
                    return X64 | WIN_2K8R2;


            if (osvi.dwMajorVersion == 6 && (osvi.dwMinorVersion == 2 || osvi.dwMinorVersion == 3)){
                return X64 | WIN_8;
            }

            if (osvi.dwMajorVersion == 10 && osvi.dwMinorVersion == 0){
              if( osvi.wProductType == VER_NT_WORKSTATION ){
                return X64 | WIN_10;
              }
              else{
                return X64 | WIN_2016;
              }
            }

            return FALSE;
            break;

        /**
         * Test for the Windows 95 product family.
         */
        case VER_PLATFORM_WIN32_WINDOWS:
            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 0)
                return WIN_95;

            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 10)
                return WIN_98;

            if (osvi.dwMajorVersion == 4 && osvi.dwMinorVersion == 90)
                return WIN_ME;

        default:
            return FALSE;
    }
    return TRUE;
}

