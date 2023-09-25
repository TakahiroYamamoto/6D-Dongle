package com.sentinel.ldk.licgen;

import com.sun.jna.*;
import com.sun.jna.win32.StdCallLibrary;

public interface LicGenLibWIN extends StdCallLibrary, LicGenAPI {
    String old_jna_encoding = System.setProperty("jna.encoding", "UTF-8");  
    String LibraryName = (System.getProperties().getProperty("os.arch").equals("x86"))?"./sntl_licgen_windows":"./sntl_licgen_windows_x64";
    LicGenLibWIN INSTANCE = (LicGenLibWIN) Native.loadLibrary(
            System.getProperty("sntl.licgen.win.library.name", LibraryName),
            LicGenLibWIN.class);
}
