@rem
@rem Build hasp_demo.exe and hasp_update.exe using libhasp_windows_demo.lib
@rem

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\hasp_demo.c -Fehasp_demo.exe libhasp_windows_demo.lib
@if errorlevel 1 exit 1

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\hasp_update.c -Fehasp_update.exe libhasp_windows_demo.lib
@if errorlevel 1 exit 1

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\sntl_admin_demo.c -Fesntl_admin_demo.exe libhasp_windows_demo.lib
@if errorlevel 1 exit 1

@if exist hasp_demo.obj del hasp_demo.obj
@if exist hasp_update.obj del hasp_update.obj
@if exist sntl_admin_demo.obj del sntl_admin_demo.obj

@echo.
@dir *.exe
