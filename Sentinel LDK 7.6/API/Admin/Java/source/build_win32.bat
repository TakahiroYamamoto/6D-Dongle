@if "%JDK%"=="" goto variable_undef

@if exist AdminApiJava.obj del AdminApiJava.obj
@if exist AdminApiJava.dll del AdminApiJava.dll

cl.exe -I"%JDK%\include" -I"%JDK%\include\win32" -I..\..\C -DSUN_JNI -c -Ox -Zl -Ob1 -Gy -DWIN32 -D_CRT_SECURE_NO_WARNINGS -D_X86_ -W3 -FoAdminApiJava.obj AdminApiJava.c
link.exe /DLL -out:AdminApiJava.dll AdminApiJava.obj  ..\..\C\win32\sntl_adminapi_windows.lib libcmt.lib
@goto exit

:variable_undef
@echo.
@echo Error: please set the variable JDK first

:exit
