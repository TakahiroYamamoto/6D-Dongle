;;; Demo program for Sentinel LDK licensing services
;;; 
;;; Copyright (C) 2014, SafeNet, Inc. All rights reserved.
;;;
;;; Sentinel DEMOMA key with features 1 and 42 required
;;; requires Sentinel LDK COM module


;;Sentinel Login Scope
(defun hasp_login (ft app vendorcode scope)
  ;; retrieve HASP object
  (setq hasp (HASPm-Hasp app ft))

  ;; login to default feature
  ;; note, the default feature is available on any Sentinel key
  ;; search for local and remote Sentinel key
  (setq ret (HASPm-LoginScope hasp vendorcode scope))
  (if (equal ret HASPc-HaspStatusOK)
    (progn
      (princ "login OK\n")
    )
    (progn
      (princ "login failed\n")
      (princ "error code=")
      (princ ret)
      (princ "\n")
      (if (equal ret HASPc-HaspStatusFeatureNotFound)
	(princ "the requested feature is not available\n")
      )
      (if (equal ret HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal ret HASPc-HaspStatusDriverTooOld)
	(princ "outdated Run_time Environment version installed\n")
      )
      (if (equal ret HASPc-HaspStatusDriverNotFound)
	(princ "Sentinel Run_time Environment not installed\n")
      )
      (if (equal ret HASPc-HaspStatusInvalidVendorCode)
	(princ "invalid vendor code\n")
      )
    )
  );;End of Sentinel Login To DefaultFeature
);; End of Login scope


;; retrieve Sentinel key attributes
(defun hasp_getinfo (hasp) 
  (princ "\nGet Session Information Demo\n")

  (princ "\nRetrieving Key Information\n")
  (setq infotype (Haspp-get-KeyInfo hasp))
  (setq info (Haspm-GetSessionInfo hasp infotype))
  (setq stat (Haspp-get-Status info))
  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq data (vlax-variant-value (Haspp-get-Value info)))
      (princ data)
      (princ "\n")
    )
    (progn
      (princ "get key info failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
    )
  )

  (princ "\nRetrieving Session Information\n")
  (setq infotype (Haspp-get-SessionInfo hasp))
  (setq info (Haspm-GetSessionInfo hasp infotype))
  (setq stat (Haspp-get-Status info))
  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq data (vlax-variant-value (Haspp-get-Value info)))
      (princ data)
      (princ "\n")
    )
    (progn
      (princ "get session info failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
    )
  )
)
;; End Of Retrieving Attributes


;; Read Write Demo
;; Get file size, write data to memory, read data from memory
(defun hasp_readwrite (hasp)

  ;; retrieve the memory size of Sentinel key
  (princ "\nretrieve the memory size of Sentinel key\n")
  (setq file (Haspm-GetFile hasp Haspc-haspFileReadWrite))

  (setq filesize (Haspp-get-FileSize file))
  (setq stat (Haspp-get-Status filesize))
  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq fszval (Haspp-get-Long filesize))
      (princ "Sentinel key memory size is ")
      (princ fszval)
      (princ " bytes\n")
    )
    (progn
      (princ "get file size failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
    )
  )
  ;; write to Sentinel key memory with an offset of 10 bytes
  (princ "\nwrite to Sentinel key memory\n")

  (Haspp-put-FilePos file 10)		; 10 bytes file offset

  (setq stat (Haspm-WriteString file "hello world\0"))

  (if (equal stat HASPc-HaspStatusOK)
    (princ "memory write OK\n")
    (progn
      (princ "memory write failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal stat HASPc-HaspStatusInvalidAddress)
	(princ "memory address not valid\n")
      )
    )
  )

  ;; read from Sentinel key memory with an offset of 16 bytes
  (princ "\nread from Sentinel key memory\n")

  (Haspp-put-FilePos file 16)		; 16 bytes file offset

  (setq ret (Haspm-ReadString file))
  (setq stat (Haspp-get-Status ret))

  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (princ "string successfully read from memory: ")
      (setq teststring (Haspp-get-String ret))
      (print (substr teststring 1 5))
      (princ "\n")
    )
    (progn
      (princ "memory read failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal stat HASPc-HaspStatusInvalidAddress)
	(princ "memory address not valid\n")
      )
    )
  )
);;End of ReadWriteDemo



;; Encrypt, Decrypt Demo 
(defun hasp_encryptdecrypt (hasp)
  ;; Sentinel key crypto functions  
  (setq coll (HASPp-get-Collection app)); for encrypt/decrypt the Collection is used

  (setq encdata (list -1234 815 -3333 4444))
  (foreach element encdata
    (vlax-make-variant element vlax-vbLong)
    (HASPm-Add coll element)
  )

  (princ "\nencrypt a list of 32 bit integer values: ")
  (princ encdata)
  (princ "\n")
  (setq stat (HASPm-Encrypt hasp coll))

  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq cnt (Haspp-get-Count coll))	; retrieve the encryption result
      (setq resultval (list))
      (setq ctr (- cnt 1))
      (repeat cnt
	(setq resultval
	       (cons (vlax-variant-value (Haspm-Item coll ctr)) resultval)
	)
	(setq ctr (- ctr 1))
      )
      (princ "Sentinel key encryption ok, result values: ")
      (princ resultval)
      (princ "\n")
    )

    (progn
      (princ "encryption failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal stat HASPc-HaspStatusBufferTooShort)
	(princ "encryption list shorter than the minimum 16 bytes\n"
	)
      )
      (if (equal stat HASPc-HaspStatusEncryptionNotSupported)
	(princ
	  "attached Sentinel key does not support AES encryption\n"
	)
      )
    )
  )

  (princ "\ndecrypt the list of integer values\n")
  (setq ret (HASPm-Decrypt hasp coll))

  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq cnt (Haspp-get-Count coll))
      (setq resultval (list))
      (setq ctr (- cnt 1))
      (repeat cnt
	(setq resultval
	       (cons (vlax-variant-value (Haspm-Item coll ctr)) resultval)
	)
	(setq ctr (- ctr 1))
      )
      (princ "Sentinel key decryption ok, result values: ")
      (princ resultval)
      (princ "\n")
    )
    (progn
      (princ "decryption failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal stat HASPc-HaspStatusBufferTooShort)
	(princ "encryption list shorter than the minimum 16 bytes\n"
	)
      )
      (if (equal stat HASPc-HaspStatusEncryptionNotSupported)
	(princ
	  "attached Sentinel key does not support AES encryption\n"
	)
      )
    )
  )
);;End of crypto Demo


;; Get Time and Date from RTC 
(defun hasp_rtc (hasp)
  ;; read the current time from Sentinel Time key
  (princ
    "\nread current date and time from Sentinel Time key\n"
  )
  (setq time (Haspm-GetRtc hasp))

  (setq stat (Haspp-get-Status time))
  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (setq data (vlax-variant-value (Haspp-get-Value time)))

      (setq year (Haspp-get-Year data))	; retrieve seperate values
      (setq month (Haspp-get-Month data))
      (setq day (Haspp-get-Day data))
      (setq hour (Haspp-get-Hour data))
      (setq minutes (Haspp-get-Minute data))
      (setq seconds (Haspp-get-Second data))

      (setq timestr (Haspp-get-String time))
					; or retrieve complete time string
      (print timestr)
    )
    (progn
      (princ "read time failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
      (if (equal stat HASPc-HaspStatusNoTime)
	(princ "accessed Sentinel key is not a time key\n")
      )
      (if (equal stat HASPc-HaspStatusNoBatteryPower)
	(princ "Sentinel Time key battery empty\n")
      )
    )
  )
);; End of RTC demo

;; All Demo functions
(defun hasp_repeatDemo (app vendorcode scope ftid)

  (princ "\nExecuting Demo for feature ID:" )
  (princ ftid)
  (princ "\n")
  (setq ft (haspm-Feature app ftid))
  (if (equal nil ft)
      (progn
	(princ "Feature not accessible\n")
      )
      (progn
        ;;Run all functions for specific feature id
        (hasp_login ft app vendorcode scope)
        (hasp_getinfo hasp)
        (hasp_readwrite hasp)
        (hasp_encryptdecrypt hasp)
        (hasp_rtc hasp)
	  
        (setq stat (HASPm-Logout hasp))
        (if (equal stat HASPc-HaspStatusOK)
          (princ "\nlogout OK\n")
          (princ "\nlogout failed\n")
        )
      )
    )
    (princ)
   
);;End of All Demo Functions


(defun hasp_demo
		 (/	   app	    ft	     hasp     ret      scope
		  vendorcode	    infotype info     stat     data
		  file	   filesize fszval   teststring	       coll
		  element  encdata  cnt	     ctr      resultval
		  time	   timestr  year     month    day      hour
		  minute   second
		 )


  (princ
    "\nThis is a simple demo program for Sentinel LDK licensing services\n"
  )
  (princ "Copyright (C) SafeNet, Inc.\n\n")


  (vl-load-com)				; load ActiveX support

					; Sentinel DEMOMA vendor code
  (setq	vendorcode
	 (vlax-make-variant
	   "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"
	   vlax-vbString
	 )
  )


  ;; import Sentinel LDK type library
  (if (equal nil HASPc-haspStatusOk)	; check for a HASP symbol
    (vlax-import-type-library
      :tlb-filename	     "C:/Program Files/SafeNet Sentinel/Sentinel LDK/API/Runtime/Com/Win32/hasp_com_windows.tlb"
      ; please edit the path (for example  "c:/hasp_com_windows.tlb")
      :methods-prefix	     "HASPm-"
      :properties-prefix     "HASPp-"
      :constants-prefix	     "HASPc-"
     )
  )
 (setq app (vlax-get-or-create-object "AKSHASP.HaspApplication"))
  (if (equal nil app)
    (princ "cannot access Sentinel LDK COM module\n")
  )
  
  (initget "1 2")  
  (setq pytaj (getkword "\nEnter an option [1] Local_and_Network, [2] Local_Only: "))  
  (if (equal pytaj "1")
    (progn 
      (setq scope "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><haspscope/>")
    )
    (progn
      (setq scope "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> <haspscope> <license_manager hostname =\"localhost\" /> </haspscope>")
    )
  ) ;;end if

  (setq ft (haspp-get-DefaultFeature app))
    (if (equal nil ft)
      (princ "Default feature not accessible\n")
    )

  ;; demo fo default feature
  (princ "|Executing Demo for Default feature. \n")
  (hasp_login ft app vendorcode scope)
  (hasp_getinfo hasp)
  (hasp_readwrite hasp)
  (hasp_encryptdecrypt hasp)
  (hasp_rtc hasp)
  
  (setq stat (HASPm-Logout hasp))
  (if (equal stat HASPc-HaspStatusOK)
    (princ "\nlogout OK\n")
    (princ "\nlogout failed\n")
  )

  ;;Repeat Demo for other feature ID
  (hasp_repeatDemo app vendorcode scope 1)
  (hasp_repeatDemo app vendorcode scope 42)
  

); defun hasp_demo




;; EOF
