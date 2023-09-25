package com.sentinel.ldk.licgen;

import com.sun.jna.*;
import com.sun.jna.ptr.*;

public interface LicGenAPI {

    int sntl_lg_initialize(
            String init_param,
            IntByReference handle);
	
	int sntl_lg_decode_current_state(
			int handle,
			String vendor_code,
			String current_state,
			PointerByReference readable_state);
			
    int sntl_lg_start(
            int handle,
            String start_param,
            String vendor_code,
            int license_type,
            String license_definition,
            String current_state);

    int sntl_lg_apply_template(
            int handle,
            String license_definition);

    int sntl_lg_generate_license(
            int handle,
            String generation_param,
            PointerByReference license,
            PointerByReference resultant_state);

    int sntl_lg_get_info(
            int handle,
            int info_type,
            PointerByReference info);

    void sntl_lg_free_memory(
            Pointer memory_reference);

    int sntl_lg_cleanup(
            IntByReference handle);

    int sntl_lg_get_version(
            IntByReference major_version,
            IntByReference minor_version,
            IntByReference build_server,
            IntByReference build_number);
}
