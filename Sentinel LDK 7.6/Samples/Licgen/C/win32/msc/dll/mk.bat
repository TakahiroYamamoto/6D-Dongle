
cl.exe -DWIN32 -W3 -D_CRT_SECURE_NO_DEPRECATE ..\..\..\sample.c -Fesample.exe sntl_licgen_windows.lib
@if errorlevel 1 pause

@if exist sample.obj del sample.obj

@echo.
@dir *.exe
