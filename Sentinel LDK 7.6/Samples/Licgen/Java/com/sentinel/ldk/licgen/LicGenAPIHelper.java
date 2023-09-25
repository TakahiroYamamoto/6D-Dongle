package com.sentinel.ldk.licgen;

import com.sun.jna.*;
import com.sun.jna.ptr.*;

public final class LicGenAPIHelper {

    /** Invalid value for the license type, to be used for initialization purpose */
    public static final int SNTL_LG_LICENSE_TYPE_INVALID = 0;

    /** Generates the license for new or modified features */
    public static final int SNTL_LG_LICENSE_TYPE_UPDATE  = 1;

    /** Generates the license that deletes all existing products/features on the Sentinel HASP protection key 
        and burns the new or modified features. */
    public static final int SNTL_LG_LICENSE_TYPE_CLEAR_AND_UPDATE = 2;

    /** Generates the license that format the Sentinel HASP protection key.
        It deletes all existing products/features on the Sentinel HASP protection key and burns the new or 
        modified features. It erases the key memory area before applying the update of memory. */
    public static final int SNTL_LG_LICENSE_TYPE_FORMAT_AND_UPDATE = 3;

    /** Generates a provisional license for SL-AdminMode, SL-UserMode and SL keys. */
    public static final int SNTL_LG_LICENSE_TYPE_PROVISIONAL = 4;

    /** Generates a generic license that can be applied to all keys of a certain vendor. 
        Reserved for Gemalto Internal use. */
    public static final int SNTL_LG_LICENSE_TYPE_GENERIC_UPDATE = 5;

    /** Generates a license to clear clone for SL Keys. */ 
    public static final int SNTL_LG_LICENSE_TYPE_CLEAR_CLONE = 6;

    /** Invalid value for the info type, to be used for initialization purpose */
    public static final int SNTL_LG_INFO_INVALID = 0;

    /** To get capable device information. It will be used to identify the devices that are capable
        to hold the license state attached with the handle.   */
    public static final int SNTL_LG_INFO_CAPABLE_DEVICES = 1;

    /** To get the last error message associated with a handle. */
    public static final int SNTL_LG_INFO_LAST_ERROR_MESSAGE = 2;

    /** Request successfully completed */
    public static final int SNTL_LG_STATUS_OK = 0;

    /** Library initialization failed. */
    public static final int SNTL_LG_LIB_INITIALIZATION_FAILED = 5001;

    /** Failure in locking a resource */
    public static final int SNTL_LG_RESOURCE_LOCK_FAILURE = 5002;

    /** System is out of memory */
    public static final int SNTL_LG_OUT_OF_MEMORY = 5003;

    /** Invalid function parameter */
    public static final int SNTL_LG_INVALID_PARAMETER = 5004;

    /** Invalid handle passed to function */
    public static final int SNTL_LG_INVALID_HANDLE = 5005;

    /** Too many open handles */
    public static final int SNTL_LG_TOO_MANY_OPEN_HANDLES = 5006;

    /** Invalid API call sequence */
    public static final int SNTL_LG_INVALID_API_CALL_SEQUENCE = 5007;

    /** Invalid Vendor Code passed */
    public static final int SNTL_LG_INVALID_VENDOR_CODE = 5008;

    /** Invalid license definition */
    public static final int SNTL_LG_INVALID_LICENSE_DEFINITION = 5009;

    /** Invalid key current state */
    public static final int SNTL_LG_INVALID_KEY_CURRENT_STATE = 5010;

    /** Invalid schema version */
    public static final int SNTL_LG_INVALID_SCHEMA_VERSION = 5011;

    /** Internal error occured. */
    public static final int SNTL_LG_INTERNAL_ERROR = 5012;

    /** Key is full */
    public static final int SNTL_LG_KEY_FULL = 5013;

    /** Key type is not supported */
    public static final int SNTL_LG_NOT_SUPPORTED_KEY_TYPE = 5014;

    /** HL Basic key cannot be programmed. */
    public static final int SNTL_LG_HL_BASIC_KEY_CANNOT_BE_PROGRAMMED = 5015;

    /** Key does not support license based on days to expiration. */
    public static final int SNTL_LG_DAYS_TO_EXPIRATION_NOT_SUPPORTED = 5016;

    /** Key does not support license based on expiration date. */
    public static final int SNTL_LG_EXPIRY_DATE_NOT_SUPPORTED = 5017;

    /** Unlimited concurrency count is not supported. */
    public static final int SNTL_LG_UNLIMITED_CONCURRENCY_COUNT_NOT_SUPPORTED = 5018;

    /** Concurrency is not supported. */
    public static final int SNTL_LG_CONCURRENCY_NOT_SUPPORTED = 5019;

    /** Data size is larger than RO memory capacity. */
    public static final int SNTL_LG_TOO_BIG_RO_MEM_DATA = 5020;

    /** Data size is larger than RW memory capacity. */
    public static final int SNTL_LG_TOO_BIG_RW_MEM_DATA = 5021;

    /** Concurrency count exceeds the maximum limit. */
    public static final int SNTL_LG_EXCEEDING_MAX_CONCURRENCY_COUNT_LIMIT = 5022;

    /** Execution count exceeds the maximum limit. */
    public static final int SNTL_LG_EXCEEDING_MAX_EXECUTION_COUNT_LIMIT = 5023;

    /** Error occurred while connecting the Master key. */
    public static final int SNTL_LG_MASTER_KEY_CONNECT_ERROR = 5024;

    /** Communication with Master key failed. Master key might not be present. */
    public static final int SNTL_LG_MASTER_KEY_IO_ERROR = 5025;

    /** Some error is observed in accessing the Master key. */
    public static final int SNTL_LG_MASTER_KEY_ACCESS_ERROR = 5026;

    /** Current state of key is mandatory to generate a license. */
    public static final int SNTL_LG_KEY_CURRENT_STATE_MISSING = 5027;

    /** Expiration date exceeds the maximum expiration date supported */
    public static final int SNTL_LG_EXCEEDING_MAX_EXPIRY_DATE_LIMIT = 5028;

    /** Expiration date is below the minimum expiration date supported */
    public static final int SNTL_LG_BELOW_MIN_EXPIRY_DATE_LIMIT = 5029;

    /** There is nothing to be generated as a license. */
    public static final int SNTL_LG_NOTHING_TO_GENERATE = 5030;

    /** No capable device found */
    public static final int SNTL_LG_NO_CAPABLE_DEVICE_FOUND = 5032;

    /** Mismatch in enforcement type. */
    public static final int SNTL_LG_INCOMPATIBLE_ENFORCEMENT_TYPE = 5034;

    /** Invalid fingerprint. */
    public static final int SNTL_LG_INVALID_FINGERPRINT =  5035;

    /** Clear operation not allowed on the given key. */
    public static final int SNTL_LG_CLEAR_NOT_ALLOWED = 5036;

    /** Format operation not allowed on the given key. */
    public static final int SNTL_LG_FORMAT_NOT_ALLOWED = 5037;

    /** Current state not allowed. */
    public static final int SNTL_LG_CURRENT_STATE_NOT_ALLOWED = 5038;

    /** Clone detected. */
    public static final int SNTL_LG_CLONE_DETECTED = 5039;

    /** No license definition. */
    public static final int SNTL_LG_NO_LICENSE_DEFINITION = 5040;

    /** Insufficient number of seats available on Master Key for charging. */
    public static final int SNTL_LG_INSUFFICIENT_SEAT_POOL = 5041;

    /** Feature not found on Master Key. */
    public static final int SNTL_LG_MASTER_FEATURE_NOT_FOUND = 5043;

    /** Feature on Master Key has expired. */
    public static final int SNTL_LG_MASTER_FEATURE_EXPIRED = 5044;

    /** Battery in the Master key is depleted. */
    public static final int SNTL_LG_MASTER_NO_BATTERY_POWER = 5045;

    /** Concurrency count below the minimum limit. */
    public static final int SNTL_LG_BELOW_MIN_CONCURRENCY_COUNT_LIMIT = 5046;

    /** Execution count below the minimum limit. */
    public static final int SNTL_LG_BELOW_MIN_EXECUTION_COUNT_LIMIT = 5047;

    /** Invalid enforcement type. */
    public static final int SNTL_LG_INVALID_ENFORCEMENT_TYPE = 5048;

    /** Enforcement type not supported. */
    public static final int SNTL_LG_ENFORCEMENT_TYPE_NOT_SUPPORTED = 5049;

    /** Either Sentinel HL driver (update to 1.22 or later) is or Sentinel HL Firmware (update to 3.25 or later) too old. */
    public static final int SNTL_LG_OLD_DRIVER = 5050;

    /** Sentinel License Manager (update to 13.0 or later) too old. */
    public static final int SNTL_LG_OLD_LM = 5051;

    /** VClock not enabled on the key. */
    public static final int SNTL_LG_VCLOCK_NOT_ENABLED = 5052;

    /**Sentinel HL Firmware is too old. */
    public static final int SNTL_LG_OLD_FIRMWARE = 5053;

    /**Key does not support dynamic resources*/
    public static final int SNTL_LG_DYNAMIC_RESOURCE_NOT_SUPPORTED = 5054;

    /** clear clone operation not allowed on the given key. */
    public static final int SNTL_LG_CLEAR_CLONE_NOT_ALLOWED = 5055;

    /** License definition not allowed. */
    public static final int SNTL_LG_LICENSE_DEFINITION_NOT_ALLOWED = 5056;

    /** License not allowed on virtual machine. */
    public static final int SNTL_LG_LICENSE_DENIED_ON_VM = 5057;

    private LicGenAPI instance;
    private Int32Buffer handle;

    public LicGenAPIHelper() {
        if (Platform.isWindows()) {
            try {
                instance = LicGenLibWIN.INSTANCE;
            }
            catch (java.lang.UnsatisfiedLinkError e) {
                instance = null;
                throw e;
            }
        }
        else if (Platform.isLinux()){
            try {
                instance = LicGenLibLinux.INSTANCE;
            }
            catch (java.lang.UnsatisfiedLinkError e) {
                instance = null;
                throw e;
            }
        }
        else {
        ;//do nothing
        }

        handle = new Int32Buffer();
    }

    public int sntl_lg_initialize(String init_param) {
        int status = 0;
        IntByReference tmp = new IntByReference();

        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        status = instance.sntl_lg_initialize(init_param, tmp);
        if (status == 0) {
            handle.set(tmp.getValue());
        }

        return status;
    }


   public int sntl_lg_decode_current_state(String vendor_code, String current_state, CharStringBuffer readable_state) throws java.io.UnsupportedEncodingException {
        int status = 0;
        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        PointerByReference pstate = new PointerByReference();

        status = instance.sntl_lg_decode_current_state(handle.get(), vendor_code, current_state, pstate);
        if (status == 0) {
            Pointer p1 = pstate.getValue();
            readable_state.set(new String(p1.getByteArray(0, (int) p1.indexOf(0, (byte) 0)), "UTF-8"));
            instance.sntl_lg_free_memory(pstate.getValue());
        }

        return status;
    }

    public int sntl_lg_start(String start_param, String vendor_code, int license_type, String license_definition, String current_state) {
        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        return instance.sntl_lg_start(handle.get(), start_param, vendor_code, license_type, license_definition, current_state);
    }

    public int sntl_lg_apply_template(String license_definition) {
        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        return instance.sntl_lg_apply_template(handle.get(), license_definition);
    }

    public int sntl_lg_generate_license(String generation_param, CharStringBuffer license, CharStringBuffer resultant_state) throws java.io.UnsupportedEncodingException {
        int status = 0;
        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        PointerByReference plicense = new PointerByReference();
        PointerByReference pstate = new PointerByReference();

        status = instance.sntl_lg_generate_license(handle.get(), generation_param, plicense, pstate);
        if (status == 0) {
            Pointer p1 = plicense.getValue();
            license.set(new String(p1.getByteArray(0, (int) p1.indexOf(0, (byte) 0)), "UTF-8"));
            instance.sntl_lg_free_memory(plicense.getValue());

            if (pstate.getValue() != null) {
                Pointer p2 = pstate.getValue();
                resultant_state.set(new String(p2.getByteArray(0, (int) p2.indexOf(0, (byte) 0)), "UTF-8"));
                instance.sntl_lg_free_memory(pstate.getValue());
            }
        }

        return status;
    }

    public int sntl_lg_get_info(int info_type, CharStringBuffer info) throws java.io.UnsupportedEncodingException {
        int status = 0;
        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        PointerByReference pinfo = new PointerByReference();

        status = instance.sntl_lg_get_info(handle.get(), info_type, pinfo);
        if (status == 0) {
            Pointer p1 = pinfo.getValue();
            info.set(new String(p1.getByteArray(0, (int) p1.indexOf(0, (byte) 0)), "UTF-8"));
            instance.sntl_lg_free_memory(pinfo.getValue());
        }

        return status;
    }

    public int sntl_lg_cleanup() {
        int status = 0;
        IntByReference tmp = new IntByReference();

        if (instance == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        tmp.setValue(handle.get());
        status = instance.sntl_lg_cleanup(tmp);

        return status;
    }

    public static int sntl_lg_get_version(Int32Buffer major_version, Int32Buffer minor_version, Int32Buffer build_server, Int32Buffer build_number) {
        int status = 0;
        LicGenAPI instance1;

        if (Platform.isWindows()) {
            try {
                instance1 = LicGenLibWIN.INSTANCE;
            }
            catch (java.lang.UnsatisfiedLinkError e) {
                instance1 = null;
                throw e;
            }
        }
        else if (Platform.isLinux()){
            try {
                instance1 = LicGenLibLinux.INSTANCE;
            }
            catch (java.lang.UnsatisfiedLinkError e) {
                instance1 = null;
                throw e;
            }
        }
        else {
            instance1 = null;
        }

        IntByReference major = new IntByReference();
        IntByReference minor = new IntByReference();
        IntByReference server = new IntByReference();
        IntByReference number = new IntByReference();

        if (instance1 == null) {
            throw new java.lang.UnsatisfiedLinkError();
        }

        status = instance1.sntl_lg_get_version(major, minor, server, number);
        if (status == 0) {
            major_version.set(major.getValue());
            minor_version.set(minor.getValue());
            build_server.set(server.getValue());
            build_number.set(number.getValue());
        }

        return status;
    }
}
