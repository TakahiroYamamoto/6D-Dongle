{ *****************************************************************************
  * 
  * Sentinel LDK Licensing API Interface
  *
  * Copyright (C) 2016, SafeNet, Inc. All rights reserved.
  *
  * Include file for importing external functions
  *
  ***************************************************************************** }

{$IFDEF WIN32}
{$L win32\hasp_windows_delphixe_demo.obj}
{$ENDIF}

{$IFDEF WIN64}
{$L x64\hasp_windows_x64_delphixe_demo.obj}
{$ENDIF}

{$IFDEF WIN32}
procedure _mktime;external 'msvcrt.dll' name 'mktime';
procedure _exit;external 'msvcrt.dll' name 'exit';
procedure _ftell;external 'msvcrt.dll' name 'ftell';
procedure __localtime64;external 'msvcrt.dll' name '_localtime64';
procedure _wrmdir;external 'msvcrt.dll' name '_wrmdir';
procedure _fread;external 'msvcrt.dll' name 'fread';
procedure __wstat;external 'msvcrt.dll' name '_wstat';
procedure _fwrite;external 'msvcrt.dll' name 'fwrite';
procedure __wfindfirst;external 'msvcrt.dll' name '_wfindfirst';
procedure __read;external 'msvcrt.dll' name '_read';
procedure __wfopen;external 'msvcrt.dll' name '_wfopen';
procedure __findclose;external 'msvcrt.dll' name '_findclose';
procedure __gmtime64;external 'msvcrt.dll' name '_gmtime64';
procedure __wrename;external 'msvcrt.dll' name '_wrename';
procedure __fileno;external 'msvcrt.dll' name '_fileno';
procedure __fstat;external 'msvcrt.dll' name '_fstat';
procedure __wremove;external 'msvcrt.dll' name '_wremove';
procedure _fflush;external 'msvcrt.dll' name 'fflush';
procedure __lseek;external 'msvcrt.dll' name '_lseek';
procedure _fseek;external 'msvcrt.dll' name 'fseek';
procedure __beginthreadex;external 'msvcrt.dll' name '_beginthreadex';
procedure __wopen;external 'msvcrt.dll' name '_wopen';
procedure __close;external 'msvcrt.dll' name '_close';
procedure __write;external 'msvcrt.dll' name '_write';
procedure _fclose;external 'msvcrt.dll' name 'fclose';
procedure __chsize;external 'msvcrt.dll' name '_chsize';
procedure __wmkdir;external 'msvcrt.dll' name '_wmkdir';
procedure __wunlink;external 'msvcrt.dll' name '_wunlink';
procedure __wrmdir;external 'msvcrt.dll' name '_wrmdir';
procedure __wfindnext;external 'msvcrt.dll' name '_wfindnext';
procedure __errno;external 'msvcrt.dll' name '_errno';
procedure __chkstk;external 'ntdll.dll' name '_chkstk';
procedure __aullrem;external 'ntdll.dll' name '_aullrem';
procedure __aulldiv;external 'ntdll.dll' name '_aulldiv';
procedure __allmul;external 'ntdll.dll' name '_allmul';
procedure __aullshr;external 'ntdll.dll' name '_aullshr';
procedure __alldiv;external 'ntdll.dll' name '_alldiv';
procedure __aulldvrm;external 'ntdll.dll' name '_aulldvrm';
procedure __alldvrm;external 'ntdll.dll' name '_alldvrm';
procedure __allrem;external 'ntdll.dll' name '_allrem';
procedure __allshl;external 'ntdll.dll' name '_allshl';
procedure __allshr;external 'ntdll.dll' name '_allshr';
{$ENDIF}

{$IFDEF WIN64}
procedure mktime;external 'msvcrt.dll' name 'mktime';
procedure exit;external 'msvcrt.dll' name 'exit';
procedure ftell;external 'msvcrt.dll' name 'ftell';
procedure _localtime64;external 'msvcrt.dll' name '_localtime64';
procedure wrmdir;external 'msvcrt.dll' name '_wrmdir';
procedure fread;external 'msvcrt.dll' name 'fread';
procedure _wstat;external 'msvcrt.dll' name '_wstat';
procedure fwrite;external 'msvcrt.dll' name 'fwrite';
procedure _wfindfirst;external 'msvcrt.dll' name '_wfindfirst';
procedure _wfindfirst64;external 'msvcrt.dll' name '_wfindfirst64';
procedure _read;external 'msvcrt.dll' name '_read';
procedure _wfopen;external 'msvcrt.dll' name '_wfopen';
procedure _findclose;external 'msvcrt.dll' name '_findclose';
procedure _gmtime64;external 'msvcrt.dll' name '_gmtime64';
procedure _wrename;external 'msvcrt.dll' name '_wrename';
procedure _fileno;external 'msvcrt.dll' name '_fileno';
procedure _fstat;external 'msvcrt.dll' name '_fstat';
procedure _wremove;external 'msvcrt.dll' name '_wremove';
procedure fflush;external 'msvcrt.dll' name 'fflush';
procedure _lseek;external 'msvcrt.dll' name '_lseek';
procedure fseek;external 'msvcrt.dll' name 'fseek';
procedure _beginthreadex;external 'msvcrt.dll' name '_beginthreadex';
procedure _wopen;external 'msvcrt.dll' name '_wopen';
procedure _close;external 'msvcrt.dll' name '_close';
procedure _write;external 'msvcrt.dll' name '_write';
procedure fclose;external 'msvcrt.dll' name 'fclose';
procedure _chsize;external 'msvcrt.dll' name '_chsize';
procedure _wmkdir;external 'msvcrt.dll' name '_wmkdir';
procedure _wunlink;external 'msvcrt.dll' name '_wunlink';
procedure _wrmdir;external 'msvcrt.dll' name '_wrmdir';
procedure _wfindnext;external 'msvcrt.dll' name '_wfindnext';
procedure _wfindnext64;external 'msvcrt.dll' name '_wfindnext64';
procedure _errno;external 'msvcrt.dll' name '_errno';
procedure __chkstk;external 'ntdll.dll' name '__chkstk';
{$ENDIF}

procedure CreateFiberEx;external kernel32 name 'CreateFiberEx';
procedure FileTimeToSystemTime;external kernel32 name 'FileTimeToSystemTime';
function ConvertFiberToThread: BOOL; stdcall;external kernel32 name 'ConvertFiberToThread';
