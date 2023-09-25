@rem
@rem Build activation sample
@rem

cl  ActivationSample.c -Feactivation_sample_x64.exe libhasp_windows_x64_demo.lib WinInet.Lib


@echo.
@dir *.exe
pause