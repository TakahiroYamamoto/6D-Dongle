@rem
@rem Build activation sample
@rem

cl  ActivationSample.c -Feactivation_sample.exe libhasp_windows_demo.lib WinInet.Lib


@echo.
@dir *.exe
pause