@rem -----------------------------------------------------------------------------------------------
@rem 
@rem build the samples
@rem -----------------------------------------------------------------------------------------------

@if "%JDK%"=="" goto variable_undef

if exist hasp_demo.class del hasp_demo.class
if exist hasp_update.class del hasp_update.class
"%JDK%\bin\javac" -classpath . -Xlint:deprecation hasp_demo.java
"%JDK%\bin\javac" -classpath . -Xlint:deprecation hasp_update.java
@goto exit

:variable_undef
@echo.
@echo Error: please set the variable JDK first

:exit
