@rem
@rem Copyright (C) 2010, SafeNet, Inc. All rights reserved.
@rem build the Windows x64 driver installer sample haspdsd_sample_x64.exe
@rem

@if exist haspdsd_sample_x64.exe del haspdsd_sample_x64.exe
@if exist haspdsd_sample.obj del haspdsd_sample.obj
@if exist haspdsd_sample.res del haspdsd_sample.res
cl.exe /DWIN64 /D__64BIT__ /D_MBCS /D_USRDLL /DHHLINST_EXPORTS /c haspdsd_sample.cpp

rc.exe /dWIN64 haspdsd_sample.rc

link.exe haspdsd_sample.obj haspdsd_sample.res /out:haspdsd_sample_x64.exe  /libpath:. setupapi.lib kernel32.lib user32.lib version.lib gdi32.lib comdlg32.lib advapi32.lib shell32.lib uuid.lib libcmt.lib /nodefaultlib:libcmt /subsystem:windows /machine:AMD64

