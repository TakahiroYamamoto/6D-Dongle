@rem
@rem Build hasp_demo.exe and hasp_update.exe using hasp_windows_x64_demo.dll
@rem hasp_windows_x64_demo.lib is an import library.
@rem
@rem Static linkage with libhasp_windows_x64_demo.lib is recommended.
@rem See 'lib' directory.
@rem
@rem The naming conventions for the 32 and 64 bit Windows DLLs are:
@rem
@rem    hasp_windows_<vendorid>.dll
@rem    hasp_windows_x64_<vendorid>.dll
@rem
@rem Please stick to this convention to avoid problems.
@rem

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\hasp_demo.c -Fehasp_demo.exe hasp_windows_x64_demo.lib
@if errorlevel 1 exit 1

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\hasp_update.c -Fehasp_update.exe hasp_windows_x64_demo.lib
@if errorlevel 1 exit 1

cl -MT -W3 -Fm -D_CRT_SECURE_NO_DEPRECATE ..\..\..\sntl_admin_demo.c -Fesntl_admin_demo.exe hasp_windows_x64_demo.lib
@if errorlevel 1 exit 1

@if exist hasp_demo.obj del hasp_demo.obj
@if exist hasp_update.obj del hasp_update.obj
@if exist sntl_admin_demo.obj del sntl_admin_demo.obj

@echo.
@dir *.exe
