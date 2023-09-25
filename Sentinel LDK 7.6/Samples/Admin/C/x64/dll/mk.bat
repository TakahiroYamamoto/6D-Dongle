@rem
@rem Build sntl_adminapi_demo.exe using sntl_adminapi_windows_x64.dll
@rem sntl_adminapi_windows_x64.lib is an import library.
@rem
@rem The naming conventions for the 32 and 64 bit Windows DLLs are:
@rem
@rem    sntl_adminapi_windows.dll
@rem    sntl_adminapi_windows_x64.dll
@rem
@rem Please stick to this convention to avoid problems.
@rem

cl -MT -W3 -DWIN32 -D_CRT_SECURE_NO_DEPRECATE ..\..\sntl_adminapi_demo.c -Fesntl_adminapi_demo.exe sntl_adminapi_windows_x64.lib

@if exist sntl_adminapi_demo.obj del sntl_adminapi_demo.obj

@echo.
@dir *.exe
