@rem
@rem Copyright (C) 2010, SafeNet, Inc. All rights reserved.
@rem build the Windows x86 Sentinel HASP Run-time Installer sample haspdsd_sample.exe
@rem

cl.exe /DWIN32 /D__32BIT__ /D_MBCS /D_USRDLL /DHHLINST_EXPORTS /c haspdsd_sample.cpp

rc.exe /dWIN32 haspdsd_sample.rc

link.exe haspdsd_sample.obj haspdsd_sample.res /out:haspdsd_sample.exe  /libpath:. setupapi.lib kernel32.lib user32.lib version.lib gdi32.lib comdlg32.lib advapi32.lib shell32.lib uuid.lib libcmt.lib /nodefaultlib:libc /subsystem:windows /machine:X86

