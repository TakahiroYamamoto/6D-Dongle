;;; Demo program for Sentinel LDK update process
;;; 
;;; Copyright (C) 2014, SafeNet, Inc. All rights reserved.
;;;
;;; Sentinel DEMOMA key with features 1 and 42 required
;;; requires Sentinel LDK COM module


(defun hasp_update
		   (/	       app	  ft	     hasp
		    ret	       vendorcode userinput  info
		    infotype   updateinfo filename   filehandle
		    acknowledge		  ackdata    v2c
		    v2c_line
		   )

  (princ
    "\nThis is a simple demo program for Sentinel LDK update services\n"
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
      :tlb-filename "C:/Program Files/SafeNet Sentinel/Sentinel LDK/API/Runtime/Com/Win32/hasp_com_windows.tlb"
      ; please edit the path (for example  "c:/hasp_com_windows.tlb")             
      :methods-prefix      "HASPm-"
      :properties-prefix   "HASPp-"
      :constants-prefix    "HASPc-"
     )
  )

  (setq app (vlax-get-or-create-object "AKSHASP.HaspApplication"))
  (if (equal nil app)
    (princ "cannot access Sentinel LDK COM module\n")
  )

  (setq ft (haspp-get-DefaultFeature app))
  (if (equal nil ft)
    (princ "Default feature not accessible\n")
  )

  ;; retrieve HASP object
  (setq hasp (HASPm-Hasp app ft))

  ;; login to default feature
  ;; note, the default feature is available on any Sentinel key
  ;; search for local and remote Sentinel key(princ "\nlogin to program number 1\n")
  (setq ret (HASPm-Login hasp vendorcode))
  (if (equal ret HASPc-HaspStatusOK)
    (progn
      (princ "login default feature OK\n")
    )
    (progn
      (princ "login default feature failed\n")
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
	(princ "outdated driver version installed\n")
      )
      (if (equal ret HASPc-HaspStatusDriverNotFound)
	(princ "Sentinel LDK Run-time Environment not installed\n")
      )
      (if (equal ret HASPc-HaspStatusInvalidVendorCode)
	(princ "invalid vendor code\n")
      )
    )
  )

  (princ
    "Please choose whether to retrieve Sentinel key information or to update a key\n"
  )

  (initget 1 "1 2")

  (setq
    userinput
     (getkword
       "\nEnter an option [1] Key update, [2] Key info: "
     )
  )

  ;; generate update information
  (if (equal "2" userinput)
    (progn
      (princ "generate update information on customer site\n")
      (hasp_demo_info app hasp vendorcode)
    )
    (progn
      (if (equal "1" userinput)
	(progn				; update key
	  ;; update Sentinel key
	  (princ "update Sentinel key\n")
	  (hasp_demo_update app hasp vendorcode)
	)
      )
    )
  )


  (setq ret (HASPm-Logout hasp))
  (if (equal stat HASPc-HaspStatusOK)
    (princ "\nlogout OK\n")
    (princ "\nlogout failed\n")
  )
    (princ )
)
;; Sentinel LDK Info Demo


(defun hasp_demo_info (app hasp vendorcode)

  (setq infotype (Haspp-get-UpdateInfo hasp))
  (setq info (Haspm-GetSessionInfo hasp infotype))

  (setq stat (Haspp-get-Status info))

  (if (equal stat HASPc-HaspStatusOK)
    (progn
      (princ
	"update information successfully retrieved from key\n"
      )
      (princ "please enter CustomerToVendor file name and path\n")
      (setq filename NIL)
      (setq filename (getstring cr))
      (if (equal "" filename)
	(setq filename "hasp_update.c2v")
					; default filename
      )
      (setq filehandle (open filename "w"))
      (if (equal nil filehandle)
	(progn
	  (princ "cannot create CustomerToVendor file")
	  (princ filename)
	  (princ "\n")
	)
	(progn				; else    filehandle is valid
	  (setq	updateinfo
		 (vlax-variant-value (Haspp-get-Value info))
	  )
	  (write-line updateinfo filehandle)
	  (princ
	    "Sentinel key information successfully stored in file "
	  )
	  (princ filename)
	  (princ "\n")
	  (close filehandle)
	)
      )
    )
    (progn
      (princ "\nSentinel LDK get update information failed\n")
      (if (equal stat HASPc-HaspStatusInvalidHandle)
	(princ "HASP object not valid\n")
      )
      (if (equal stat HASPc-HaspStatusContainerNotFound)
	(princ "feature container not available\n")
      )
    )
  )
)
;; End of hasp_demo_info



(defun hasp_demo_update	(app hasp vendorcode)
  (princ "please enter VendorToCustomer file name and path\n")
  (setq filename NIL)
  (setq filename (getstring cr))
  (if (equal "" filename)
    (setq filename "hasp_update.v2c")
					; default filename
  )

  (setq filehandle (open filename "r"))
  (if (equal nil filehandle)
    (progn
      (princ "cannot open VendorToCustomer file\n")
      (princ filename)
      (princ "\n")
    )
    (progn				; else    filehandle is valid

      (setq v2c "")
      (while
	(setq v2c_line (read-line filehandle))
	 (setq v2c (strcat v2c v2c_line))
      )

      (close filehandle)
      (if (equal nil v2c)
	(princ "cannot read VendorToCustomer file\n")
	(progn

	  (setq ret (Haspm-UpdateHasp app v2c))
					; update Sentinel key
	  (setq stat (Haspp-get-Status ret))

	  (if (equal stat HASPc-HaspStatusOK)
	    (progn
	      (princ "Sentinel LDK update OK\n")

	      (setq acknowledge (Haspp-get-Value ret))
					; retrieve acknowledge data
	      (if (equal "" (vlax-variant-value acknowledge))
		(
		)
		(progn
		  (setq
		    ackdata (vlax-variant-value acknowledge)
		  )
		  (princ
		    "please enter acknowledge file name and path\n"
		  )
		  (setq filename NIL)
		  (setq filename (getstring cr))
		  (if (equal "" filename)
		    (setq filename "acknowledge.c2v")
		  )
		  (setq	filehandle
			 (open filename
			       "w"
			 )
		  )
		  (if (equal nil filehandle)
		    (princ "cannot create acknowledge file\n")

		    (progn		; else    filehandle is valid
		      (write-line ackdata filehandle)
		      (princ "update acknowledge file ")
		      (princ filename)
		      (princ " written to disk\n")
		      (close filehandle)
		    )
		  )
		)
	      )
	    )
	    (progn
	      (princ "\nSentinel LDK update failed\n")

	      (if (equal stat HASPc-HaspStatusInvalidHandle)
		(princ "HASP object not valid\n")
	      )
	      (if
		(equal stat HASPc-HaspStatusContainerNotFound)
		 (princ "feature container not available\n")
	      )
	      (if (equal stat HASPc-HaspStatusInvalidFormat)
		(princ "invalid format of update data\n")
	      )
	      (if
		(equal stat HASPc-HaspStatusInvalidUpdataData)
		 (princ "update data not valid\n")
	      )
	      (if
		(equal stat
		       HASPc-HaspStatusUpdateNotSupported
		)
		 (princ	"Sentinel key does not support updates\n"
		 )
	      )
	      (if (equal stat
			 HASPc-HaspStatusInvalidUpdateCounter
		  )
		(princ "invalid update counter\n")
	      )
	    )
	  )
	)
      )
    )
  )
)



;; EOF
