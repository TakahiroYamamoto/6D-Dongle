@setlocal

@if "%JDK%"=="" goto variable_undef

"%JDK%/bin/javac" -cp jna.jar com/sentinel/ldk/licgen/*.java

"%JDK%/bin/jar" -cfm LicGenAPI.jar LICGENAPI.MF com/sentinel/ldk/licgen/*.class

"%JDK%/bin/javac" -cp LicGenAPI.jar Sample.java

"%JDK%/bin/jar" -cfm Sample.jar SAMPLE.MF Sample.class

@goto exit

:variable_undef
@echo.
@echo Error: please set the variable JDK first

:exit

@endlocal
