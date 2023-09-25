@echo off
@rem build the samples


@if "%JDK%"=="" goto no_variable_define
@If NOT EXIST "%JDK%\bin\javac.exe" goto no_jdk
del  .\classes\*.class

"%JDK%\bin\javac" -classpath ./classes;hasp-srm-api.jar;commons-logging-1.1.1.jar;log4j-1.2.13.jar -d ./classes  ./src/*.java
goto exit

:no_variable_define
@echo Error: please set the variable JDK first

:no_jdk
@echo Error: JDK is not installed 

:exit
pause;