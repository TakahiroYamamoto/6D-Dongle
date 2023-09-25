LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE    := hasp_android_demo
ifneq ($(TARGET_ARCH_ABI),x86)
LOCAL_SRC_FILES := libhasp_android_demo.a
else
LOCAL_SRC_FILES := libhasp_android_x86_demo.a
endif
include $(PREBUILT_STATIC_LIBRARY)

include $(CLEAR_VARS)

LOCAL_MODULE    := HASPJava
LOCAL_SRC_FILES := HASPJava.c

LOCAL_STATIC_LIBRARIES := hasp_android_demo
LOCAL_LDLIBS += -llog

include $(BUILD_SHARED_LIBRARY)


include $(CLEAR_VARS)

LOCAL_MODULE    := rus
LOCAL_SRC_FILES := rus.c

LOCAL_STATIC_LIBRARIES := hasp_android_demo
LOCAL_LDLIBS += -llog

include $(BUILD_SHARED_LIBRARY)