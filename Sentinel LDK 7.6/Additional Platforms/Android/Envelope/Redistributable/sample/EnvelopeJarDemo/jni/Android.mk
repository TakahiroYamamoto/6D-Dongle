LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE    := random
LOCAL_SRC_FILES := random.c

LOCAL_LDLIBS += -llog

include $(BUILD_SHARED_LIBRARY)
