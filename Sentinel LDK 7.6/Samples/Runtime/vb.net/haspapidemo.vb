''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'
'
' 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System
Imports System.Windows.Forms
Imports System.Collections.Specialized
Imports System.Text
Imports System.Runtime.InteropServices
Imports Aladdin.HASP

Public Class HaspDemo
    Protected stringCollection As stringCollection
    Protected textHistory As System.Windows.Forms.TextBox

    Public Const localScope As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?> " + _
                                          "<haspscope> " + _
                                          "   <license_manager hostname =""localhost"" /> " + _
                                          "</haspscope>"

    Public Const defaultScope As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?> " + _
                                          "<haspscope/>"

    'Scope used for all logins
    Private scope As String


    Public Sub New(ByVal textHistory As System.Windows.Forms.TextBox)
        MyBase.New()

        'keep a reference to the TextBox which
        'shall dump the results of the operations.
        Me.textHistory = textHistory

        'next could be considered ugly.
        'build up a string collection holding
        'the status codes in a human readable manner.
        Dim stringRange() As String = New String() { _
            "Success.", _
            "Invalid memory address.", _
            "Unknown/invalid feature id option.", _
            "Memory allocation failed.", _
            "Too many open features.", _
            "Feature access denied.", _
            "Incompatible feature.", _
            "Container not found.", _
            "En-/decryption length too short.", _
            "Invalid handle.", _
            "Invalid file id / memory descriptor.", _
            "Driver or support daemon version too old.", _
            "Real time support not available.", _
            "Generic error from host system call.", _
            "Hardware key driver not found.", _
            "Unrecognized info format.", _
            "Request not supported.", _
            "Invalid update object.", _
            "Key with requested id was not found.", _
            "Update data consistency check failed.", _
            "Update not supported by this key.", _
            "Update counter mismatch.", _
            "Invalid vendor code.", _
            "Requested encryption algorithm not supported.", _
            "Invalid date / time.", _
            "Clock out of power.", _
            "Update requested ack., but no area to return it.", _
            "Terminal services (remote terminal) detected.", _
            "Feature type not implemented.", _
            "Unknown algorithm.", _
            "Signature check failed.", _
            "Feature not found.", _
            "Trace log not enabled.", _
            "Communication error between application and local LM", _
            "Vendor code unknown to API library (run apigen to make it known)", _
            "Invalid XML spec", _
            "Invalid XML scope", _
            "Too many keys connected", _
            "Too many users", _
            "Broken session", _
            "Communication error between local and remote LM", _
            "The feature is expired", _
            "Sentinel LM version too old", _
            "Sentinel SL secure storage I/O error or USB request error", _
            "Update installation not allowed", _
            "System time has been tampered", _
            "Secure channel communication error", _
            "Secure storage contains garbage", _
            "Vendor lib cannot be found", _
            "Vendor lib cannot be loaded", _
            "No feature matching scope found", _
            "Virtual machine detected", _
            "Sentinel update incompatible with this hardware; Sentinel key is locked to other hardware", _
            "Login denied because of user restrictions", _
            "Update was already installed", _
            "Another update must be installed first", _
            "Vendor library version too old", _
            "Upload error", _
            "Invalid XML recipient parameter", _
            "Invalid XML action parameter", _
            "Scope does not select a unique Product", _
            "Invalid Product information", _
            "Update can only be applied to recipient which was specified", _
            "Invalid duration", _
            "Cloned Sentinel SL secure storage detected", _
            "Specified V2C update already installed in the LM", _
            "Specified HASP ID is in inactive state", _
            "No detachable feature exists", _
            "Scope does not specify a unique host", _
            "Rehost is not allowed for any license", _
            "License is rehosted to other machine", _
            "Old rehost license try to apply", _
            "File not found or access denied", _
            "Extension of license not allowed", _
            "Detach of license is not allowed", _
            "Rehost of license is not allowed", _
            "Operation now allowed as container has detached license", _
            "Recipient of the requested operation is older than expected", _
            "Secure storage ID mismatch", _
            "Duplicate Hostname found while key contains Hostname Fingerprinting", _
            "The Sentinel License Manager is required for this operation", _
            "The license for the Feature does not contain enough remaining executions" _
        }

        stringCollection = New StringCollection
        stringCollection.AddRange(stringRange)

        For n As Int32 = stringCollection.Count To 399
            stringCollection.Insert(n, "")
        Next n

        stringRange = New String() { _
            "A required API dynamic library was not found", _
            "The found and assigned API dynamic library could not be verified"}

        stringCollection.AddRange(stringRange)

        For n As Int32 = stringCollection.Count To 499
            stringCollection.Insert(n, "")
        Next n

        stringRange = New String() { _
            "Calling invalid object.", _
            "A parameter is invalid.", _
            "Already logged in.", _
            "Already logged out."}

        stringCollection.AddRange(stringRange)

        For n As Int32 = stringCollection.Count To 524
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(525, "Unable to excecute/complete the operation.")

        For n As Int32 = stringCollection.Count To 599
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(600, "No classic memory extension block available.")

        For n As Int32 = stringCollection.Count To 649
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(650, "Invalid port type.")
        stringCollection.Insert(651, "Invalid port.")
        stringCollection.Insert(652, "Dot-Net DLL found broken.")

        For n As Int32 = stringCollection.Count To 697
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(698, "Capability is not available.")
        stringCollection.Insert(699, "Internal API error.")

        For n As Int32 = stringCollection.Count To 2000
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(2001, "Reserved for Sentinel helper libraries.")

        For n As Int32 = stringCollection.Count To 3000
            stringCollection.Insert(n, "")
        Next n

        stringCollection.Insert(3001, "Reserved for Sentinel Activation API.")
    End Sub

    'Dumps a bunch of bytes into the referenced TextBox.
    Protected Sub DumpBytes(ByVal bytes() As Byte)
        Verbose("Dumping data (max. 64 Bytes):")

        For index As Int32 = 0 To bytes.Length - 1
            If (0 = (index Mod 8)) Then
                If (0 < index) Then
                    FormHaspDemo.WriteToTextbox(textHistory, ControlChars.CrLf)
                End If
                FormHaspDemo.WriteToTextbox(textHistory, "          ")
            End If

            FormHaspDemo.WriteToTextbox(textHistory, "&H" + bytes(index).ToString("X2") + " ")

            ' for performance reason we only dump 64 bytes
            If (63 <= index) Then
                FormHaspDemo.WriteToTextbox(textHistory, ControlChars.CrLf + "          ...")
                Exit For
            End If
        Next index

        Verbose("")
    End Sub

    'Writes the Demo header into the referenced TextBox.
    Protected Overridable Sub Header()
        If (textHistory IsNot Nothing AndAlso &HFFFF < textHistory.TextLength) Then
            textHistory.Clear()
        End If

        FormHaspDemo.WriteToTextbox(textHistory, _
            "____________________________________________________________" + _
            ControlChars.CrLf + _
            String.Format("API Demo started ({0})", _
            DateTime.Now.ToString()) + _
            ControlChars.CrLf + ControlChars.CrLf)
    End Sub

    'Demonstrates the usage of the AES encryption and
    'decryption methods.
    Protected Sub EncryptDecryptDemo(ByVal hasp As Hasp)
        'sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("Encrypt/Decrypt Demo")

        'the string to be encrypted/decrypted.
        Dim text As String = "Sentinel LDK is great"
        Verbose("Encrypting " + ControlChars.Quote + _
                text + ControlChars.Quote)

        'convert the string into a byte array.
        Dim data() As Byte = UTF8Encoding.Default.GetBytes(text)

        'encrypt the data.
        Dim status As HaspStatus = hasp.Encrypt(data)

        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            text = UTF8Encoding.Default.GetString(data)
            Verbose("Encrypted string: " + ControlChars.Quote + _
                    text + ControlChars.Quote)

            Verbose("")
            Verbose("Decrypting " + ControlChars.Quote + _
                    text + ControlChars.Quote)

            'decrypt the data.
            'on success we convert the data back into a
            'human readable string.
            status = hasp.Decrypt(data)
            ReportStatus(status)

            If (HaspStatus.StatusOk = status) Then
                text = UTF8Encoding.Default.GetString(data)
                Verbose("Decrypted string: " + ControlChars.Quote + _
                        text + ControlChars.Quote)
            End If
        End If

        Verbose("")

        'Second choice - encrypting a string using the
        'native .net API
        text = "Encrypt/Decrypt String"
        Verbose("Encrypting " + ControlChars.Quote + _
                text + ControlChars.Quote)

        status = hasp.Encrypt(text)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            Verbose("Encrypted string: " + ControlChars.Quote + _
                    text + ControlChars.Quote)

            Verbose("")
            Verbose("Decrypting " + ControlChars.Quote + _
                    text + ControlChars.Quote)

            status = hasp.Decrypt(text)
            ReportStatus(status)

            If (HaspStatus.StatusOk = status) Then
                Verbose("Decrypted string: " + ControlChars.Quote + _
                        text + ControlChars.Quote)
            End If
        End If

        Verbose("")
    End Sub

    'Demonstrates how to login using a feature id.
    Protected Function LoginDemo(ByVal feature As HaspFeature) As Hasp
        If (feature.IsProgNum) Then
            Verbose("Login Demo with Feature: " + _
                    feature.FeatureId.ToString() + _
                    " (Program Number)")
        Else
            Verbose("Login Demo with Feature: " + _
                    feature.FeatureId.ToString())

        End If

        'create a key object using a feature
        'and perform a login using the vendor code.
        Dim hasp As Hasp = New Hasp(feature)

        Dim status As HaspStatus = hasp.Login(VendorCode.Code, scope)
        ReportStatus(status)

        Verbose("")

        If (Not hasp.IsLoggedIn()) Then
            Return Nothing
        End If

        Return hasp
    End Function

    'Demonstrates how to login into a key using the
    'default feature. The default feature is ALWAYS
    'available in every key.
    Protected Function LoginDefaultDemo() As Hasp
        Verbose("Login Demo with Default Feature (HaspFeature.Default)")

        Dim hasp As Hasp = New Hasp(HaspFeature.Default)

        Dim status As HaspStatus = hasp.Login(VendorCode.Code, scope)
        ReportStatus(status)
        Verbose("")

        'Please note that there is no need to call
        'a logout function explicitly - although it is
        'recommended. The garbage collector will perform
        'the logout when disposing the object.
        'if you need more control over the logout procedure
        'perform one of the more advanced tasks.
        If (Not hasp.IsLoggedIn()) Then
            Return Nothing
        End If

        Return hasp
    End Function

    'Demonstrates how to perform login and logout
    'using the default feature.
    Protected Sub LoginLogoutDefaultDemo()
        Verbose("Login/Logout Demo with Default Feature (HaspFeature.Default)")

        Verbose("Login:")
        Dim hasp As Hasp = New Hasp(HaspFeature.Default)

        Dim status As HaspStatus = hasp.Login(VendorCode.Code, scope)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            Verbose("Logout:")
            status = hasp.Logout()

            ReportStatus(status)
        End If

        'recommended, but not mandatory
        'this call ensures that all resources to the key
        'are freed immediately.
        hasp.Dispose()
        Verbose("")
    End Sub

    'Demonstrates how to perform a login using the default
    'feature and how to perform an automatic logout
    'using the Dispose method.
    Protected Sub LoginDisposeDemo()
        Verbose("Login/Dispose Demo with Default Feature (HaspFeature.Default)")

        Dim hasp As Hasp = New Hasp(HaspFeature.Default)

        Dim status As HaspStatus = hasp.Login(VendorCode.Code, scope)
        ReportStatus(status)

        Verbose("Disposing object - will perform an automatic logout")
        hasp.Dispose()

        Verbose("")
    End Sub

    'Performs a logout operation on the Hasp
    Protected Sub LogoutDemo(ByRef hasp As Hasp)
        'sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("Logout Demo")

        Dim status As HaspStatus = hasp.Logout()
        ReportStatus(status)

        'get rid of the object immediately.
        hasp.Dispose()
        hasp = Nothing
        Verbose("")
    End Sub

    'Prints the footer into the referenced TextBox.
    Protected Sub Footer()
        FormHaspDemo.WriteToTextbox(textHistory, "API Demo completed" + ControlChars.CrLf)
    End Sub


    'Demonstrates how to perform read and write
    'operations on a key's memory
    Protected Sub ReadWriteDemo(ByVal hasp As Hasp, _
                                ByVal fileId As HaspFileId)
        'sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("Read/Write Demo")

        'Get a file object to a key's memory file.
        'please note: the file object is tightly connected
        'to its key object. logging out from a key also
        'invalidates the file object.
        'doing the following will result in an invalid
        'file object:
        'hasp.login(...)
        'Dim file As HaspFile = hasp.GetFile()
        'hasp.Logout()
        'Debug.Assert(file.IsValid()) will assert
        Dim file As HaspFile = hasp.GetFile(fileId)
        If (Not file.IsLoggedIn()) Then
            'Not logged into a key - nothing left to do.
            Verbose("Failed to get file object\r\n")
            Return
        End If

        Verbose("Reading contents of file: " + file.FileId.ToString())

        Verbose("Retrieving the size of the file")

        'get the file's size
        Dim size As Int32 = 0
        Dim status As HaspStatus = file.FileSize(size)
        ReportStatus(status)

        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        Verbose("Size of the file is: " + size.ToString() + " Bytes")

        'read the contents of the file into a buffer
        'allocate sufficient buffer
        Dim bytes() As Byte = New Byte(size - 1) {}

        Verbose("Reading data")
        status = file.Read(bytes, 0, bytes.Length)
        ReportStatus(status)

        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        DumpBytes(bytes)

        Verbose("Writing to file")

        'now let's write some data into the file
        Dim newBytes() As Byte = New Byte() {1, 2, 3, 4, 5, 6, 7}

        status = file.Write(newBytes, 0, newBytes.Length)
        ReportStatus(status)
        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        DumpBytes(newBytes)

        'and read them again
        Verbose("Reading written data")
        status = file.Read(newBytes, 0, newBytes.Length)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            DumpBytes(newBytes)
        End If

        'restore the original contents
        file.Write(bytes, 0, bytes.Length)
        Verbose("")
    End Sub

    'Demonstrates how to read and write to/from a key's
    'memory at a certain position
    Protected Sub ReadWritePosDemo(ByVal hasp As Hasp, _
                                   ByVal fileId As HaspFileId)
        ' sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("GetFileSize/FilePos Demo")

        'firstly get a file object to a key's file.
        Dim file As HaspFile = hasp.GetFile(fileId)
        If (Not file.IsLoggedIn()) Then
            'Not logged into key - nothing left to do.
            Verbose("Failed to get file object\r\n")
            Return
        End If

        Verbose("Reading contents of file: " + file.FileId.ToString())
        Verbose("Retrieving the size of the file")

        'we want to write an int at the end of the file.
        'therefore we are going to
        '- get the file's size
        '- set the object's read and write position to
        '  the appropriate offset.
        Dim size As Int32 = 0
        Dim status As HaspStatus = file.FileSize(size)
        ReportStatus(status)

        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        Verbose("Size of the file is: " + size.ToString() + " Bytes")
        Verbose("Setting file position to last int and reading value")

        'set the file pos to the end minus the size of int
        file.FilePos = size - HaspFile.TypeSize(GetType(Int32))

        'now read what's there
        Dim aValue As Int32 = 0
        status = file.Read(aValue)
        ReportStatus(status)

        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        Verbose("Writing to file: &H" + Int32.MaxValue.ToString("X2"))

        'write some data.
        status = file.Write(Int32.MaxValue)
        ReportStatus(status)

        If (HaspStatus.StatusOk <> status) Then
            Verbose("")
            Return
        End If

        'read back the written value.
        Dim newValue As Int32 = 0
        Verbose("Reading written data")
        status = file.Read(newValue)

        ReportStatus(status)
        If (HaspStatus.StatusOk = status) Then
            Verbose("Data read: &H" + newValue.ToString("X2"))
        End If

        'restore the original data.
        file.Write(aValue)
        Verbose("")
    End Sub

    'Dumps an operation status into the
    'referenced TextBox.
    Protected Sub ReportStatus(ByVal status As HaspStatus)
        FormHaspDemo.WriteToTextbox(textHistory, _
            String.Format("     Result: {0} (HaspStatus.{1}){2}", _
                                          stringCollection(status), _
                                          status.ToString(), _
                                          ControlChars.CrLf))

        If (textHistory IsNot Nothing) Then
            If (HaspStatus.StatusOk = status) Then
                textHistory.Refresh()
            Else
                textHistory.Parent.Refresh()
            End If
        End If
    End Sub

    'Demonstrates how to set the real time clock of
    'a key when available.
    Protected Sub RtcDemo(ByVal hasp As Hasp)
        ' sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("Reading the Real Time Clock Demo")

        Dim time As DateTime = DateTime.Now
        Dim status As HaspStatus = hasp.GetRtc(time)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            Verbose("Real Time Clock is " + time.ToString())
        End If

        Verbose("")
    End Sub

    'Runs the API demo.
    Public Sub RunDemo(ByVal scope As String)
        Try
            Me.scope = scope

            Header()

            'Demonstrate the different login methods
            LoginLogoutDefaultDemo()
            LoginDisposeDemo()

            'Demonstrates how to get a list of available features
            GetInfoDemo()

            'run the API demo using the default feature
            '(ALWAYS present in every key)
            Dim hasp As Hasp = LoginDefaultDemo()

            SessionInfoDemo(hasp)
            ReadWriteDemo(hasp, HaspFileId.ReadWrite)
            ReadWritePosDemo(hasp, HaspFileId.ReadWrite)
            EncryptDecryptDemo(hasp)
            RtcDemo(hasp)
            LogoutDemo(hasp)

            Dim features() As Int32 = New Int32() { _
                HaspFeature.FromFeature(1).Feature, _
                HaspFeature.FromFeature(3).Feature, _
                HaspFeature.FromFeature(42).Feature, _
                HaspFeature.FromFeature(101).Feature}

            'run the API demo using variuos feature ids
            For index As Int32 = 0 To features.Length - 1
                hasp = LoginDemo(New HaspFeature(features(index)))
                SessionInfoDemo(hasp)
                ReadWriteDemo(hasp, HaspFileId.ReadWrite)
                ReadWritePosDemo(hasp, HaspFileId.ReadWrite)
                EncryptDecryptDemo(hasp)
                RtcDemo(hasp)
               LogoutDemo(hasp)
            Next index
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message, _
                                                 "Exception", _
                                                  System.Windows.Forms.MessageBoxButtons.OK)

        End Try

        Footer()
    End Sub
    'Demonstrates how to use to retrieve a XML containing all available features.
    Protected Sub GetInfoDemo()

        Dim queryFormat As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?>" + _
                                    "<haspformat root=""hasp_info"">" + _
                                    " <feature>" + _
                                    "  <attribute name=""id"" />" + _
                                    "  <element name=""license"" />" + _
                                    " </feature>" + _
                                    "</haspformat>"

        Verbose("Get Information Demo")

        Verbose("Retrieving Feature Information")

        Dim info As String = Nothing

        Dim status As HaspStatus = Hasp.GetInfo(scope, queryFormat, VendorCode.Code, info)

        ReportStatus(status)
        If (HaspStatus.StatusOk = status) Then
            Verbose("Key Information:")
            Verbose(info.Replace(ControlChars.Lf, ControlChars.CrLf + "          "))
        Else
            Verbose("")
        End If

    End Sub

    'Demonstrates how to retrieve information from a key.
    Protected Sub SessionInfoDemo(ByVal hasp As Hasp)
        ' sanity check
        If (Nothing Is hasp) Then
            Return
        End If

        If (Not hasp.IsLoggedIn()) Then
            Return
        End If

        Verbose("Get Session Information Demo")

        Verbose("Retrieving Key Information")

        'firstly we will retrieve the key info.
        Dim info As String = Nothing
        Dim status As HaspStatus = hasp.GetSessionInfo(hasp.KeyInfo, _
                                                         info)
        ReportStatus(status)
        If (HaspStatus.StatusOk = status) Then

            Verbose("Key Information:")
            Verbose(info.Replace(ControlChars.Lf, _
                                 ControlChars.CrLf + "          "))
        Else
            Verbose("")
        End If

        Verbose("Retrieving Session Information")

        'next the session info.
        status = hasp.GetSessionInfo(hasp.SessionInfo, info)
        ReportStatus(status)
        If (HaspStatus.StatusOk = status) Then
            Verbose("Session Information:")
            Verbose(info.Replace(ControlChars.Lf, _
                                 ControlChars.CrLf + "          "))
        Else
            Verbose("")
        End If

        Verbose("Retrieving Update Information")

        'last the update information.
        status = hasp.GetSessionInfo(hasp.UpdateInfo, info)
        ReportStatus(status)
        If (HaspStatus.StatusOk = status) Then
            Verbose("Update Information:")
            Verbose(info.Replace(ControlChars.Lf, _
                                 ControlChars.CrLf + "          "))
        Else
            Verbose("")
        End If
    End Sub

    'Writes some descriptive text into the
    'referenced TextBox.
    Protected Sub Verbose(ByVal text As String)
        If (Nothing Is text) Then
            Return
        End If

        FormHaspDemo.WriteToTextbox(textHistory, _
            "     " + text + ControlChars.CrLf)

    End Sub
End Class
