package com.sentinel.ldk.licgen;

import com.sun.jna.*;

public interface LicGenLibLinux extends LicGenAPI, Library {
    String LibraryName = (System.getProperties().getProperty("os.arch").equals("x86") || System.getProperties().getProperty("os.arch").equals("i386"))?"./libsntl_licgen_linux_pic.so":"./libsntl_licgen_linux_x86_64_pic.so";
    LicGenLibLinux INSTANCE = (LicGenLibLinux) Native.loadLibrary( LibraryName, LicGenLibLinux.class );
}
