@rem
@rem build hasp_demo.exe and hasp_update.exe using libhasp_windows_bcc.lib static library
@rem

@rem build C++ wrapper library
cmd /C "cd ..\..\..\..\API\Runtime\cpp\bc && mk.cmd"

@if exist hasp_demo.exe del hasp_demo.exe
@if exist hasp_update.exe del hasp_update.exe

bcc32 /DWIN32 /DNDEBUG /D_CONSOLE /D_MBCS /M /x /I..\..\..\inc  ..\hasp_demo.cpp ..\errorprinter.cpp ..\..\..\..\API\Runtime\cpp\bc\libhasp_cpp_windows_bcc.lib ..\..\..\..\API\Runtime\c\win32\libhasp_windows_bcc_demo.lib
@if errorlevel 1 pause

bcc32 /DWIN32 /DNDEBUG /D_CONSOLE /D_MBCS /M /x /I..\..\..\inc  ..\hasp_update.cpp ..\errorprinter.cpp ..\..\..\..\API\Runtime\cpp\bc\libhasp_cpp_windows_bcc.lib ..\..\..\..\API\Runtime\c\win32\libhasp_windows_bcc_demo.lib
@if errorlevel 1 pause
