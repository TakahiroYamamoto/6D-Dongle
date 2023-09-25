@rem
@rem build hasp_demo.exe and hasp_update.exe
@rem using libhasp_windows_bcc_demo.lib
@rem

bcc32.exe -DWIN32 ..\..\..\hasp_demo.c -ehasp_demo.exe libhasp_windows_bcc_demo.lib
bcc32.exe -DWIN32 ..\..\..\hasp_update.c -ehasp_update.exe libhasp_windows_bcc_demo.lib

@if exist hasp_demo.obj del hasp_demo.obj
@if exist hasp_demo.tds del hasp_demo.tds
@if exist hasp_update.obj del hasp_update.obj
@if exist hasp_update.tds del hasp_update.tds

@echo.
@dir *.exe
