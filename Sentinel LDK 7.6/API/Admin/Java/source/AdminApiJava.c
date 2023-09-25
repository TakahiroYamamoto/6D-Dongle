#include <jni.h>
#include <string.h>
#include "sntl_adminapi.h"

JNIEXPORT jint JNICALL Java_SafeNet_AdminApi_contextNew
  (JNIEnv *env, jobject obj, jlongArray context, jstring host, jint port, jstring password)
{
	const char *ptr_host = (*env)->GetStringUTFChars(env, host, NULL);
	const char *ptr_password = (*env)->GetStringUTFChars(env, password, NULL);
	int result = 0;
	 
	sntl_admin_context_t *sntl_struct_native = 0; 
	
	jlong *ptr_context = (*env)->GetLongArrayElements(env, context, 0);
	
	result = (int)sntl_admin_context_new(&sntl_struct_native , ptr_host, (int)port, ptr_password);
	ptr_context[0] = (jlong)sntl_struct_native;
	
	(*env)->ReleaseStringUTFChars(env, host, ptr_host);
	(*env)->ReleaseStringUTFChars(env, password, ptr_password);
	(*env)->ReleaseLongArrayElements(env, context, ptr_context, 0);
	
	return result;
}

JNIEXPORT jint JNICALL Java_SafeNet_AdminApi_contextDelete
  (JNIEnv *env, jobject obj, jlongArray context)
{
	int result = 0;
	jlong *ptr_context = (*env)->GetLongArrayElements(env, context, 0);
	
	result = (int)sntl_admin_context_delete((sntl_admin_context_t*)ptr_context[0]);

	(*env)->ReleaseLongArrayElements(env, context, ptr_context, 0);
	
	return result;
}

JNIEXPORT jstring JNICALL Java_SafeNet_AdminApi_set
  (JNIEnv *env, jobject obj, jlongArray context, jstring input, jlongArray result)
{		

	char *ptr_status = 0;
	const char *ptr_input = (*env)->GetStringUTFChars(env, input, 0);
	jstring status = NULL;
	jbyteArray java_status = 0;
	
	jlong *ptr_context = (*env)->GetLongArrayElements(env, context, 0);
	jlong *ptr_result = (*env)->GetLongArrayElements(env, result, 0);
	
	ptr_result[0] = sntl_admin_set((sntl_admin_context_t*)ptr_context[0], ptr_input, &ptr_status);
	
	if(ptr_status)
	{
		status = (*env)->NewStringUTF(env, ptr_status);
		sntl_admin_free(ptr_status);
	}

	(*env)->ReleaseStringUTFChars(env, input, ptr_input);
	(*env)->ReleaseLongArrayElements(env, result, ptr_result, 0);
    (*env)->ReleaseLongArrayElements(env, context, ptr_context, 0);

	return status;
}

JNIEXPORT jstring JNICALL Java_SafeNet_AdminApi_get
  (JNIEnv *env, jobject obj, jlongArray context, jstring scope, jstring format, jlongArray result)
{	
	char *native_info = 0;
	jstring info = NULL;
	
	const char *ptr_scope = (*env)->GetStringUTFChars(env, scope, 0);
	const char *ptr_format = (*env)->GetStringUTFChars(env, format, 0);
	jlong *ptr_context = (*env)->GetLongArrayElements(env, context, 0);
	jlong *ptr_result = (*env)->GetLongArrayElements(env, result, 0);
		
	ptr_result[0] = sntl_admin_get((sntl_admin_context_t*)ptr_context[0], ptr_scope, ptr_format, &native_info);
	
	if(native_info)
	{
		info = (*env)->NewStringUTF(env, native_info);
		sntl_admin_free(native_info);
	}

	(*env)->ReleaseStringUTFChars(env, format, ptr_format);
	(*env)->ReleaseStringUTFChars(env, scope, ptr_scope);
	(*env)->ReleaseLongArrayElements(env, result, ptr_result, 0);
	(*env)->ReleaseLongArrayElements(env, context, ptr_context, 0);

	return info;
}
