/* 
 *
 * random: Android Envelope jar sample
 * Copyright (C) 2011, SafeNet, Inc. All rights reserved.
 *
 */

#include <jni.h>
#include <time.h>
#include <string.h>

/*
 * Your Java namespace - change it to fit your package name, otherwise
 * the JVM is not able to load your functions!
 */
#define FUNCNAME(x)   Java_com_safenet_envelopejardemo_EnvelopeJarDemo_##x


JNIEXPORT jint JNICALL 
FUNCNAME(GetRandomNumber)(JNIEnv *env, jclass obj)
{    
	struct timeval tm;
  jint result = 0;

  (void)obj;

	gettimeofday(&tm, NULL);
		
	result += (tm.tv_usec >> 2) & 0x111;
	result += tm.tv_sec & 0x11;
	result += (tm.tv_sec >> 2) & 0x11;
	result *= tm.tv_usec & 0x111;	
	
  return (result & 0x11111111);
}


