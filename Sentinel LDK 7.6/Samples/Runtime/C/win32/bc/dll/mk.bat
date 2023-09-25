@rem
@rem build hasp_demo.exe and hasp_update.exe
@rem using hasp_windows_demo.dll and import library utility
@rem
@rem static linkage with libhasp_windows_bcc.lib is recommended
@rem see 'static' directory
@rem 

implib.exe hasp_windows_bcc.lib hasp_windows_demo.dll
bcc32.exe -DWIN32 ..\..\..\hasp_demo.c -ehasp_demo.exe hasp_windows_bcc.lib -link
bcc32.exe -DWIN32 ..\..\..\hasp_update.c -ehasp_update.exe hasp_windows_bcc.lib -link
bcc32.exe -DWIN32 ..\..\..\sntl_admin_demo.c -esntl_admin_demo.exe hasp_windows_bcc.lib -link

@if exist hasp_demo.obj del hasp_demo.obj
@if exist hasp_demo.tds del hasp_demo.tds

@if exist hasp_update.obj del hasp_update.obj
@if exist hasp_update.tds del hasp_update.tds

@if exist sntl_admin_demo.obj del sntl_admin_demo.obj
@if exist sntl_admin_demo.tds del sntl_admin_demo.tds

@if exist hasp_windows_bcc.lib del hasp_windows_bcc.lib

@echo.
@dir *.exe
