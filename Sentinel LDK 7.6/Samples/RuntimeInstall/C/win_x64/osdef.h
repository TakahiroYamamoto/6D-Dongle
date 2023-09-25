/*  
**
**  Copyright (C) 2013, SafeNet, Inc. All rights reserved.
**
**  osdef.h - OS Version definitions
**
*/

/*
 * OS bit definitions
 *
 * !!!!!!!! WARNING !!!!!!!!!!!!!!!
 * Whenever you add a new OS WIN_XXXX defines REMEMBER to update also the
 * below MAX_XXXXX, VALID_XXXX, XXXX_ALL and other values AND the "0pdc.txt" AND "udt.txt" files!
 */
#define WIN_95      0x00000001	
#define WIN_98      0x00000002	
#define WIN_ME      0x00000004	
#define WIN_NT      0x00000008	
#define WIN_2K      0x00000010	
#define WIN_XP      0x00000020	
#define WIN_2K3     0x00000040	
#define VISTA       0x00000080
#define WIN_2K8     0x00000100
#define WIN7        0x00000200
#define WIN_2K8R2   0x00000400
#define WIN_8       0x00000800
#define WIN_10	    0x00001000
#define WIN_2016    0x00002000


#define VALID_OS  	(WIN_98 | WIN_ME | WIN_2K | WIN_XP | WIN_2K3 | VISTA | WIN_2K8 | WIN7 | WIN_2K8R2 |WIN_8 | WIN_10 | WIN_2016)
#define VALID_OS_W9X 	(WIN_98 | WIN_ME)
#define VALID_OS_NT     (WIN_2K | WIN_XP | WIN_2K3 | VISTA | WIN_2K8 | WIN7 | WIN_2K8R2|WIN_8 | WIN_10 | WIN_2016)

#define WINX64		0x80000000
#define WINX64_XP   	(WINX64 | WIN_XP)
#define WINX64_2K3   	(WINX64 | WIN_2K3)
#define WINX64_VISTA 	(WINX64 | VISTA)
#define WINX64_2K8	(WINX64 | WIN_2K8)
#define WINX64_WIN7	(WINX64 | WIN7)
#define WINX64_2K8R2	(WINX64 | WIN_2K8R2)
#define WINX64_WIN8	(WINX64 | WIN_8)
#define WINX64_WIN10	(WINX64 | WIN_10)
#define WINX64_WIN2016	(WINX64 | WIN_2016)

#define WINX64_ALL  	(WINX64_2K3 | WINX64_VISTA | WINX64_2K8 | WIN7 | WIN_2K8R2 |WINX64_WIN8 | WINX64_WIN10 | WINX64_WIN2016)
