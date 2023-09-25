@rem
@rem build MSI Wrapper DLL for Sentinel HASP Run-time Environment Installer
@rem


cl.exe -DWIN32 -MT -LD haspds_msi.def haspds_msi.c advapi32.lib msi.lib user32.lib -Fehaspds_msi.dll


@if exist haspds_msi.obj del haspds_msi.obj
