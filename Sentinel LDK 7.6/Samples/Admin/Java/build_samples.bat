@if "%JDK%"=="" goto variable_undef

if exist AdminApiSample.class del AdminApiSample.class
"%JDK%\bin\javac" -classpath ./admin-api.jar -Xlint:deprecation AdminApiSample.java
@goto exit

:variable_undef
@echo.
@echo Error: please set the variable JDK first

:exit
