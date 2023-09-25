@rem
@rem Build sntl_adminapi_demo.exe using libsntl_adminapi_windows.lib
@rem
@rem The naming conventions for the 32 and 64 bit Windows DLLs are:
@rem
@rem    libsntl_adminapi_windows.lib
@rem    libsntl_adminapi_windows_x64.lib
@rem
@rem Please stick to this convention to avoid problems.
@rem

cl -W3 -DWIN32 -D_CRT_SECURE_NO_DEPRECATE ..\..\sntl_adminapi_demo.c -Fesntl_adminapi_demo.exe libsntl_adminapi_windows.lib

@if exist sntl_adminapi_demo.obj del sntl_adminapi_demo.obj

@echo.
@dir *.exe
