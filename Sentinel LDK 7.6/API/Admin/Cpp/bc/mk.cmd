@rem
@rem build libsntl_adminapi_cpp_windows_bcc.lib
@rem

@if exist *.obj del *.obj
@if exist libsntl_adminapi_cpp_windows_bcc.lib del libsntl_adminapi_cpp_windows_bcc.lib

bcc32  /c /DWIN32 /DNDEBUG /D_CONSOLE /D_MBCS /x /W /I..\..\C  ..\sntl_adminapi_cpp.cpp
@if errorlevel 1 pause
bcc32  /c /DWIN32 /DNDEBUG /D_CONSOLE /D_MBCS /x /W /I..\..\C  ..\AdminInfo.cpp
@if errorlevel 1 pause

tlib libsntl_adminapi_cpp_windows_bcc.lib +sntl_adminapi_cpp.obj +AdminInfo.obj