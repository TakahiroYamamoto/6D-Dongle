''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (C) 2014 SafeNet, Inc. All rights reserved.
'
'
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualBasic
Imports Sentinel.Ldk.LicGen

Public Class VBSample

    Public Enum Error_Message
        SNTL_LG_SAMPLE_RETURN_ERROR = 0
        SNTL_LG_SAMPLE_PARAMETER_ERROR
        SNTL_LG_SAMPLE_DECODE_STATE_FAILED
        SNTL_LG_SAMPLE_GENERATE_PROVISIONAL_FAILED
        SNTL_LG_SAMPLE_GENERATE_BASE_INDEPENDENT_FAILED
        SNTL_LG_SAMPLE_CLEAR_KEY_FAILED
        SNTL_LG_SAMPLE_FORMAT_KEY_FAILED
        SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        SNTL_LG_SAMPLE_GENERATE_MODIFY_FAILED
        SNTL_LG_SAMPLE_GENERATE_CANCEL_FAILED
        SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED
        SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        SNTL_LG_SAMPLE_STORE_FILE_FAILED
        SNTL_LG_SAMPLE_INITIALIZE_FAILED
        SNTL_LG_SAMPLE_START_FAILED
    End Enum

    Public Enum License_Type
        SNTL_LG_SAMPLE_LICENSE_TYPE = 0
        SNTL_LG_SAMPLE_LICENSE_HL
        SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE
        SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE
    End Enum

    Public Shared Function base64_decode(ByVal input As String) As Byte()
        Dim length As Integer = 0
        Dim pad_count As Integer = 0

        If input = Nothing Then
            Return Nothing
        End If

        length = input.Length

        If ((length = 0) OrElse ((length Mod 4) <> 0)) Then
            Return Nothing
        End If

        Dim i As Integer
        For i = 0 To length - 1
            Select Case base64_decode_char(input.Chars(i))
                Case &H40
                    If ((i <> (length - 1)) AndAlso (i <> (length - 2))) Then
                        Return Nothing
                    End If
                    If (i = (length - 2)) Then
                        If (base64_decode_char(input.Chars((i + 1))) <> &H40) Then
                            Return Nothing
                        End If
                        pad_count += 1
                    Else
                        pad_count += 1
                    End If
                    Exit Select
                Case &H41
                    Return Nothing
            End Select
        Next i

        Dim output_array As Byte() = New Byte(((length / 4) * 3) - 1) {}

        Dim j As Integer = 0
        i = 0
        Do While (i < length)
            Dim val As Long = 0
            Dim k As Integer
            For k = 0 To 4 - 1
                Dim index As Byte = base64_decode_char(input.Chars((i + k)))
                val = (val Or (index And &H3F))
                val = (val << 6)
            Next k

            val = (val << 2)

            Dim l As Integer
            For l = 0 To 3 - 1
                output_array((j + l)) = (CByte(((val >> &H18) And &HFF)))
                val = (val << 8)
            Next l
            i = (i + 4)
            j = (j + 3)
        Loop

        Dim output As Byte() = New Byte((((length / 4) * 3) - pad_count) - 1) {}

        Array.Copy(output_array, output, CInt((((length / 4) * 3) - pad_count)))

        Return output
    End Function

    Public Shared Function base64_decode_char(ByVal input As Char) As Byte
        If ((input.CompareTo("A"c) >= 0) AndAlso (input.CompareTo("Z"c)) <= 0) Then
            Return CByte((Asc(input) - Asc("A"c)))
        End If

        If ((input.CompareTo("a"c) >= 0) AndAlso (input.CompareTo("z"c) <= 0)) Then
            Return CByte(Asc(input) - Asc("a"c) + &H1A)
        End If

        If ((input >= "0"c) AndAlso (input <= "9"c)) Then
            Return CByte(Asc(input) - Asc("0"c) + &H34)
        End If

        If (input = "+"c) Then
            Return &H3E
        End If

        If (input = "/"c) Then
            Return &H3F
        End If

        If (input = "="c) Then
            Return &H40
        End If

        Return &H41
    End Function

    Public Shared Function base64_encode(ByVal input As Byte()) As String
        Dim length As Integer = 0
        Dim pad_count As Integer = 0
        length = input.Length
        If (length = 0) Then
            Return Nothing
        End If
        pad_count = ((3 - (length Mod 3)) Mod 3)
        Dim output As New StringBuilder
        Dim input_string As Byte() = New Byte((length + pad_count) - 1) {}
        Array.Copy(input, input_string, length)
        Dim i As Integer = 0
        Do While (i < length)
            Dim val As Integer = 0
            Dim j As Integer
            For j = 0 To 3 - 1
                val = (val Or input_string((i + j)))
                val = (val << 8)
            Next j
            Dim k As Integer
            For k = 0 To 4 - 1
                Dim index As Byte = CByte(((val >> &H1A) And &H3F))
                output.Append(base64code.Chars(index))
                val = (val << 6)
            Next k
            i = (i + 3)
        Loop
        If (pad_count > 0) Then
            output.Chars((output.Length - 1)) = "="c
            pad_count -= 1
            If (pad_count > 0) Then
                output.Chars((output.Length - 2)) = "="c
            End If
        End If
        Return output.ToString
    End Function

    Private Shared base64code As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"

    Public Const vendorCode As String = _
   "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMA" + _
   "sVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWT" + _
   "OZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06" + _
   "waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4V" + _
   "nYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/Id" + _
   "gLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1" + _
   "YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMU" + _
   "uRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7" + _
   "s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLV" + _
   "TvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q" + _
   "9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0" + _
   "m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VF" + _
   "ITB3mazzFiyQuKf4J6+b/a/Y"

    Public error_msg As String
    Public license_template_number As Integer
    Public Shared licGenHelp As LicGenAPIHelper = New LicGenAPIHelper
    Public nothing_to_clear As Integer
    Public nothing_to_format As Integer
    Public szOption As String
    Public szC2V As String
    Public szCurrentStateFilename As String
    Public szDXML As String() = New String(2) {}
    Public szUXMLFilename As String
    Public szV2CFilename As String
    Public szKey_type As String
    Public szKey_configuration As String
    Public dynamic_memory_supported_key As Boolean

    Public Shared Sub Main(ByVal args() As String)
        Try
            Dim sample As New VBSample

            sample.show_copyright()

            sample.show_LicGen_version()

            If (sample.handle_parameter(args) = 0) Then

                'demonstrate the HL example
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_HL)

                If (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                    Console.WriteLine("  Fail to decode current state")
                    Console.WriteLine("")
                End If

                If (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                    Console.WriteLine("  Fail to generate clear license")
                    Console.WriteLine("")
                End If

                If (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                    Console.WriteLine("  Fail to generate format license")
                    Console.WriteLine("")
                End If

                If (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                    Console.WriteLine("  Fail to generate new license")
                    Console.WriteLine("")
                End If

                If (Not sample.szOption.Equals("n")) Then
                    If (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                        Console.WriteLine("  Fail to generate modify")
                        Console.WriteLine("")
                    End If

                    If (Not sample.szOption.Equals("m")) Then
                        If (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_HL) <> 0) Then
                            Console.WriteLine("  Fail to generate cancel license")
                            Console.WriteLine("")
                        End If
                        Console.WriteLine("")
                    End If
                End If

                'demonstrate the SL-AdminMode example
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE)

                If (sample.generate_provisional_license(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate provisional license")
                    Console.WriteLine("")
                End If

                If (sample.process_new_base_independent(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate base independent license")
                    Console.WriteLine("")
                End If

                If (sample.process_new_rehost(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate base independent license that allows rehosting")
                    Console.WriteLine("")
                End If

                If (sample.process_new_detach(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate base independent detachable license")
                    Console.WriteLine("")
                End If

                If (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to decode current state")
                    Console.WriteLine("")
                End If

                If (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate clear license")
                    Console.WriteLine("")
                End If

                If (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate format license")
                    Console.WriteLine("")
                End If

                If (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate new license")
                    Console.WriteLine("")
                End If

                If (Not sample.szOption.Equals("n")) Then
                    If (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                        Console.WriteLine("  Fail to generate modify")
                        Console.WriteLine("")
                    End If

                    If (Not sample.szOption.Equals("m")) Then
                        If (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE) <> 0) Then
                            Console.WriteLine("  Fail to generate cancel license")
                            Console.WriteLine("")
                        End If
                        Console.WriteLine("")
                    End If
                End If

                'demonstrate the SL-UserMode example
                sample.show_sample_license_type(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE)

                If (sample.generate_provisional_license(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate provisional license")
                    Console.WriteLine("")
                End If

                If (sample.process_new_base_independent(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate base independent license")
                    Console.WriteLine("")
                End If

                If (sample.process_new_rehost(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate base independent license that allows rehosting")
                    Console.WriteLine("")
                End If

                If (sample.process_decode_current_state(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to decode current state")
                    Console.WriteLine("")
                End If

                If (sample.process_clear_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate clear license")
                    Console.WriteLine("")
                End If

                If (sample.process_format_key(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate format license")
                    Console.WriteLine("")
                End If

                If (sample.process_new(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                    Console.WriteLine("  Fail to generate new license")
                    Console.WriteLine("")
                End If

                If (Not sample.szOption.Equals("n")) Then
                    If (sample.process_modify(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                        Console.WriteLine("  Fail to generate modify")
                        Console.WriteLine("")
                    End If

                    If (Not sample.szOption.Equals("m")) Then
                        If (sample.process_cancel(License_Type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE) <> 0) Then
                            Console.WriteLine("  Fail to generate cancel license")
                            Console.WriteLine("")
                        End If
                        Console.WriteLine("")
                    End If
                End If

            End If
        Finally
            If licGenHelp IsNot Nothing Then
                licGenHelp.Dispose()
            End If
        End Try

    End Sub

    Public Shared Function load_file(ByVal filename As String) As String
        Dim fi As New FileInfo(filename)
        If Not fi.Exists Then
            Return Nothing
        End If
        Dim fs As StreamReader = fi.OpenText
        Dim length As Integer = CInt(fi.Length)
        Dim temp As Char() = New Char(fi.Length - 1) {}
        fs.Read(temp, 0, length)
        fs.Close()
        Return New String(temp)
    End Function

    Public Shared Function store_file(ByVal buffer As String, ByVal filename As String) As Integer

        If buffer = Nothing Then
            Return 1
        End If

        Try
            Dim path As String = "output"
            If Not Directory.Exists(path) Then
                Directory.CreateDirectory("output")
                Directory.CreateDirectory("output/HL")
                Directory.CreateDirectory("output/SL-AdminMode")
                Directory.CreateDirectory("output/SL-UserMode")
            Else
                If Not Directory.Exists("output/HL") Then
                    Directory.CreateDirectory("output/HL")
                End If
                If Not Directory.Exists("output/SL-AdminMode") Then
                    Directory.CreateDirectory("output/SL-AdminMode")
                End If
                If Not Directory.Exists("output/SL-UserMode") Then
                    Directory.CreateDirectory("output/SL-UserMode")
                End If
            End If

        Catch e As Exception
            Console.WriteLine("The process failed: {0}", e.ToString)
        End Try

        If File.Exists(filename) Then
            File.Delete(filename)
        End If

        Dim fs As New FileStream(filename, FileMode.Create)
        If (fs Is Nothing) Then
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If
        fs.Write(Encoding.ASCII.GetBytes(buffer), 0, buffer.Length)
        fs.Close()
        Return 0
    End Function
    'Display help message
    Public Sub show_help()
        Console.WriteLine(ChrW(10))
        Console.WriteLine(" Command line parameter usage of this program:")
        Console.WriteLine("     -h        Print this menu and exit.")
        Console.WriteLine("     -c        [default] Generate new, modify and cancel licenses.")
        Console.WriteLine("     -m        Generate new and modify licenses.")
        Console.WriteLine("     -n        Generate new license only.")
        Console.WriteLine(ChrW(10))
    End Sub
    'deal with command line parameters
    '
    'param args
    '            command line args
    'return 0 if success, other if failure
    Public Function handle_parameter(ByVal args As String()) As Integer
        Dim length As Integer = args.Length
        If (length = 0) Then
            Me.szOption = "c"
            Return 0
        End If

        If (length > 1) Then
            Console.WriteLine(" Error: unrecongnized or incomplete command line.")
            show_help()
            Return 1
        End If

        If args(0).Equals("-c") Then
            Me.szOption = "c"
            Return 0
        End If

        If args(0).Equals("-m") Then
            Me.szOption = "m"
            Return 0
        End If

        If args(0).Equals("-n") Then
            Me.szOption = "n"
            Return 0
        End If

        If Not args(0).Equals("-h") Then
            Console.WriteLine(" Error: unrecongnized or incomplete command line.")
        End If

        show_help()
        Return Error_Message.SNTL_LG_SAMPLE_PARAMETER_ERROR
    End Function

    Public Sub show_copyright()
        Console.WriteLine("")
        Console.WriteLine(" A demo program for the Sentinel LDK license generation functions")
        Console.WriteLine(" Copyright (c) 2014 SafeNet, Inc. All rights reserved.")
        Console.WriteLine("")
    End Sub

    Public Sub show_LicGen_version()
        Dim major_version As Integer = 0
        Dim minor_version As Integer = 0
        Dim build_server As Integer = 0
        Dim build_number As Integer = 0

        If (LicGenAPIHelper.sntl_lg_get_version(major_version, minor_version, build_server, build_number) = sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Console.WriteLine("")
            Console.WriteLine(String.Concat(New String() {" Sentinel LDK License Generation Windows DLL ", major_version.ToString, ".", minor_version.ToString, " build ", build_number.ToString}))
            Console.WriteLine("")
        End If
    End Sub

    'demonstrate the license type
    Public Sub show_sample_license_type(ByVal license_type As License_Type)
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("")
            Console.WriteLine(" The following examples is for Sentinel LDK HL key type" & ChrW(10))
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("")
            Console.WriteLine(" The following examples is for Sentinel LDK SL Admin Mode key type" & ChrW(10))
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("")
            Console.WriteLine(" The following examples is for Sentinel LDK SL User Mode key type" & ChrW(10))
            Console.WriteLine("")
        End If
    End Sub

    'generate provisional license
    Public Function generate_provisional_license(ByVal license_type As License_Type) As Integer
        Dim status As sntl_lg_status_t = sntl_lg_status_t.SNTL_LG_STATUS_OK
        Dim szInitParamXML As String = Nothing
        Dim szStartParamXML As String = Nothing
        Dim szLicenseDefinitionXML As String = Nothing
        Dim szGenerationParamXML As String = Nothing
        Dim license As String = Nothing
        Dim resultant_state As String = Nothing
        szC2V = Nothing

        Console.WriteLine(" Process provisional license:")

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            szLicenseDefinitionXML = load_file("input/SL-AdminMode/provisional_template.xml")
            If (szLicenseDefinitionXML Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-AdminMode/provisional_template.xml file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            szLicenseDefinitionXML = load_file("input/SL-UserMode/provisional_template.xml")
            If (szLicenseDefinitionXML Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-UserMode/provisional_template.xml file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
        End If

        'sntl_lg_initialize
        'Initializes license generator library
        'and returns handle to work further
        status = licGenHelp.sntl_lg_initialize(szInitParamXML)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_initialize: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_INITIALIZE_FAILED
        End If

        'sntl_lg_start
        'Starts the license generation.
        status = licGenHelp.sntl_lg_start(szStartParamXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_PROVISIONAL, szLicenseDefinitionXML, szC2V)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_start: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_START_FAILED
        End If

        'sntl_lg_generate_license
        'Generates the license.
        status = licGenHelp.sntl_lg_generate_license(szGenerationParamXML, license, resultant_state)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_generate_license: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        Dim licenseFilePath As String = Nothing
        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            licenseFilePath = "output/SL-AdminMode/provisional_license.v2c"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            licenseFilePath = "output/SL-UserMode/provisional_license.v2c"
        End If

        If (store_file(license, licenseFilePath) <> 0) Then
            Console.WriteLine("  provisional license file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        'Clean up memories that used in the routine
        licGenHelp.sntl_lg_cleanup()
        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/provisional_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/provisional_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'process generation of base independent license
    '
    'return 0 if success, other if failure
    Public Function process_new_base_independent(ByVal license_type As License_Type) As Integer
        szDXML(0) = Nothing
        szDXML(1) = Nothing
        Dim templatePath As String = Nothing

        Console.WriteLine(" Process base independent license:")

        'load license definition file
        license_template_number = 1

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then

            'load C2V file
            szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v"
            szC2V = load_file(szCurrentStateFilename)
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If

            templatePath = "input/SL-AdminMode/base_independent_template.xml"
            szV2CFilename = "output/SL-AdminMode/new_base_independent_license.v2c"
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new_base_independent.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then

            'load C2V file
            szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v"
            szC2V = load_file(szCurrentStateFilename)
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-UserMode/fingerprint.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If

            templatePath = "input/SL-UserMode/base_independent_template.xml"
            szV2CFilename = "output/SL-UserMode/new_base_independent_license.v2c"
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_new_base_independent.xml"
        End If

        szDXML(0) = load_file(templatePath)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading base_independent_template.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/new_base_independent_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/new_base_independent_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'decodes the current state
    Public Function process_decode_current_state(ByVal license_type As License_Type) As Integer
        Dim status As sntl_lg_status_t = sntl_lg_status_t.SNTL_LG_STATUS_OK
        Dim c2vFilePath As String = Nothing
        Dim szInitParamXML As String = Nothing
        Dim readable_state As String = Nothing
        Dim decode_XML_Path As String = Nothing

        Console.WriteLine(" Decode current state:")

        'load c2v file
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            c2vFilePath = "input/HL/original_state.c2v"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            c2vFilePath = "input/SL-AdminMode/original_state.c2v"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            c2vFilePath = "input/SL-UserMode/original_state.c2v"
        End If

        szC2V = load_file(c2vFilePath)
        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading original_state.c2v file.")
            Console.WriteLine("")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'sntl_lg_initialize
        'Initializes license generator library
        'and returns handle to work further
        status = licGenHelp.sntl_lg_initialize(szInitParamXML)
        status = licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_initialize: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_INITIALIZE_FAILED
        End If

        'sntl_lg_decode_current_state
        'Decodes the current state
        status = licGenHelp.sntl_lg_decode_current_state(vendorCode, szC2V, readable_state)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_decode_current_state: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_DECODE_STATE_FAILED
        End If

        'Load key_type and key_configuration from key readable state
        load_key_type_and_config(readable_state)

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            decode_XML_Path = "output/HL/decoded_current_state.xml"
            'Check for dynamic memory supported key
            If Not (String.IsNullOrEmpty(szKey_configuration)) Then
                If (szKey_configuration.Contains("driverless")) Then
                    If (szKey_type <> "Sentinel-HL-Pro" And szKey_type <> "Sentinel-HL-Basic") Then
                        dynamic_memory_supported_key = True
                    End If
                End If
            End If
            If (dynamic_memory_supported_key = False)
                Console.WriteLine("  This key doesn't support dynamic memory!")
            End If

        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            decode_XML_Path = "output/SL-AdminMode/decoded_current_state.xml"

        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            decode_XML_Path = "output/SL-UserMode/decoded_current_state.xml"
        End If

        If (store_file(readable_state, decode_XML_Path) <> 0) Then
            Console.WriteLine("  decoded current state file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        'Clean up memories that used in the routine
        licGenHelp.sntl_lg_cleanup()

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  Decoded current state file ""output/HL/decoded_current_state.xml"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  Decoded current state file ""output/SL-AdminMode/decoded_current_state.xml"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  Decoded current state file ""output/SL-UserMode/decoded_current_state.xml"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'load key-type and key-configuration
    Public Sub load_key_type_and_config(ByVal key_readable_state As String)
        szKey_type = ""
        szKey_configuration = ""
        'Create an XmlReader
        Using reader As XmlReader = XmlReader.Create(New StringReader(key_readable_state))

            'fetching key type
            If (reader.ReadToFollowing("type")) Then
                szKey_type = reader.ReadElementContentAsString()
            End If

            'fetching key-configuration
            If (reader.ReadToFollowing("configuration_info")) Then
                Do
                    reader.Read()
                    If (reader.IsEmptyElement) Then
                        If (reader.Name.Equals("hasphl") Or reader.Name.Equals("sentinelhl") Or reader.Name.Equals("driverless")) Then
                            szKey_configuration += reader.Name
                            szKey_configuration += " "
                        End If
                    End If
                Loop While Not (reader.Name.Equals("configuration_info"))
            End If
        End Using
    End Sub

    'generate clear key license
    Public Function process_clear_key(ByVal license_type As License_Type) As Integer
        Dim szInitScopeXML As String = Nothing
        Dim szStartScopeXML As String = Nothing
        Dim szLicenseDefinitionXML As String = Nothing
        Dim szGenerationScopeXML As String = Nothing
        Dim license As String = Nothing
        Dim resultant_state As String = Nothing

        Console.WriteLine(" Process clear license:")

        'load c2v file
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            szC2V = load_file("input/HL/original_state.c2v")
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/HL/original_state.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            szC2V = load_file("input/SL-AdminMode/original_state.c2v")
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-AdminMode/original_state.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            szC2V = load_file("input/SL-UserMode/original_state.c2v")
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-UserMode/original_state.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
        End If

        'sntl_lg_initialize
        'Initializes license generation library
        'and returns handle to work further
        Dim status As sntl_lg_status_t = licGenHelp.sntl_lg_initialize(szInitScopeXML)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_initialize: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            Return Error_Message.SNTL_LG_SAMPLE_INITIALIZE_FAILED
        End If

        'sntl_lg_start
        'Starts the license generation.
        status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_CLEAR_AND_UPDATE, szLicenseDefinitionXML, szC2V)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_start: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_START_FAILED
        End If

        'sntl_lg_generate_license()
        'Generates the license.
        status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, license, resultant_state)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_generate_license: " & error_msg))
        If ((status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) AndAlso (status <> sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE)) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (status = sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE) Then
            nothing_to_clear = 1
            Console.WriteLine("")
            licGenHelp.sntl_lg_cleanup()
            Return 0
        End If

        Dim licensePath As String = Nothing
        Dim statePath As String = Nothing
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            licensePath = "output/HL/clear_license.v2c"
            statePath = "output/HL/resultant_state_after_clear.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            licensePath = "output/SL-AdminMode/clear_license.v2c"
            statePath = "output/SL-AdminMode/resultant_state_after_clear.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            licensePath = "output/SL-UserMode/clear_license.v2c"
            statePath = "output/SL-UserMode/resultant_state_after_clear.xml"
        End If

        If (store_file(license, licensePath) <> 0) Then
            licGenHelp.sntl_lg_cleanup()
            Console.WriteLine("  license file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        If (store_file(resultant_state, statePath) <> 0) Then
            licGenHelp.sntl_lg_cleanup()
            Console.WriteLine("  resultant license container state file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        'Clean up memories that used in the routine
        licGenHelp.sntl_lg_cleanup()

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  License file ""output/HL/clear_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/clear_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/clear_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'generate format key license
    Public Function process_format_key(ByVal license_type As License_Type) As Integer
        Dim szInitScopeXML As String = Nothing
        Dim szStartScopeXML As String = Nothing
        Dim szLicenseDefinitionXML As String = Nothing
        Dim szGenerationScopeXML As String = Nothing
        Dim license As String = ""
        Dim resultant_state As String = ""

        Console.WriteLine(" Process format license:")

        szC2V = Nothing
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            If (nothing_to_clear <> 0) Then
                szCurrentStateFilename = "input/HL/original_state.c2v"
            Else
                szCurrentStateFilename = "output/HL/resultant_state_after_clear.xml"
            End If
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            If (nothing_to_clear <> 0) Then
                szCurrentStateFilename = "input/SL-AdminMode/original_state.c2v"
            Else
                szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_clear.xml"
            End If
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            If (nothing_to_clear <> 0) Then
                szCurrentStateFilename = "input/SL-UserMode/original_state.c2v"
            Else
                szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_clear.xml"
            End If
        End If

        'load c2v file
        szC2V = load_file(szCurrentStateFilename)

        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading resultant_state_after_clear.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'sntl_lg_initialize
        'Initializes license generation library
        'and returns handle to work further
        Dim status As sntl_lg_status_t = licGenHelp.sntl_lg_initialize(szInitScopeXML)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_initialize: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_INITIALIZE_FAILED
        End If

        'sntl_lg_start
        'Starts the license generation.
        status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_FORMAT_AND_UPDATE, szLicenseDefinitionXML, szC2V)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_start: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_START_FAILED
        End If

        'sntl_lg_generate_license
        'Generates the license.
        status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, license, resultant_state)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_generate_license: " & error_msg))
        If ((status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) AndAlso (status <> sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE)) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (status = sntl_lg_status_t.SNTL_LG_NOTHING_TO_GENERATE) Then
            nothing_to_format = 1
            Console.WriteLine("")
            licGenHelp.sntl_lg_cleanup()
            Return 0
        End If

        Dim licensePath As String = Nothing
        Dim statePath As String = Nothing
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            licensePath = "output/HL/format_license.v2c"
            statePath = "output/HL/resultant_state_after_format.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            licensePath = "output/SL-AdminMode/format_license.v2c"
            statePath = "output/SL-AdminMode/resultant_state_after_format.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            licensePath = "output/SL-UserMode/format_license.v2c"
            statePath = "output/SL-UserMode/resultant_state_after_format.xml"
        End If

        If (store_file(license, licensePath) <> 0) Then
            Console.WriteLine("  license file fail to save.")
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        If (store_file(resultant_state, statePath) <> 0) Then
            Console.WriteLine("  resultant license container state file fail to save.")
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        licGenHelp.sntl_lg_cleanup()
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  License file ""output/HL/format_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/format_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/format_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'process new license generation routine from original_state.c2v,
    'new_license_template1.xml and new_license_template2.xml
    '
    'return 0 if success, other if failure
    Public Function process_new(ByVal license_type As License_Type) As Integer
        szC2V = Nothing
        szDXML(0) = ""
        szDXML(1) = ""
        szDXML(2) = ""
        Dim xmlFilePath1 As String = Nothing
        Dim xmlFilePath2 As String = Nothing
        Dim xmlFilePath3 As String = Nothing

        Console.WriteLine(" Process new license:")

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            If ((nothing_to_format <> 0) AndAlso (nothing_to_clear <> 0)) Then
                szCurrentStateFilename = "input/HL/original_state.c2v"
            ElseIf ((nothing_to_format <> 0) AndAlso (nothing_to_clear = 0)) Then
                szCurrentStateFilename = "output/HL/resultant_state_after_clear.xml"
            Else
                szCurrentStateFilename = "output/HL/resultant_state_after_format.xml"
            End If

            xmlFilePath1 = "input/HL/new_license_template1.xml"
            xmlFilePath2 = "input/HL/new_license_template2.xml"
            xmlFilePath3 = "input/HL/new_license_template3.xml"
            szV2CFilename = "output/HL/new_license.v2c"
            szUXMLFilename = "output/HL/resultant_state_after_new.xml"

        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            If ((nothing_to_format <> 0) AndAlso (nothing_to_clear <> 0)) Then
                szCurrentStateFilename = "input/SL-AdminMode/original_state.c2v"
            ElseIf ((nothing_to_format <> 0) AndAlso (nothing_to_clear = 0)) Then
                szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_clear.xml"
            Else
                szCurrentStateFilename = "output/SL-AdminMode/resultant_state_after_format.xml"
            End If

            xmlFilePath1 = "input/SL-AdminMode/new_license_template1.xml"
            xmlFilePath2 = "input/SL-AdminMode/new_license_template2.xml"
            szV2CFilename = "output/SL-AdminMode/new_license.v2c"
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_new.xml"

        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            If ((nothing_to_format <> 0) AndAlso (nothing_to_clear <> 0)) Then
                szCurrentStateFilename = "input/SL-UserMode/original_state.c2v"
            ElseIf ((nothing_to_format <> 0) AndAlso (nothing_to_clear = 0)) Then
                szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_clear.xml"
            Else
                szCurrentStateFilename = "output/SL-UserMode/resultant_state_after_format.xml"
            End If

            xmlFilePath1 = "input/SL-UserMode/new_license_template1.xml"
            xmlFilePath2 = "input/SL-UserMode/new_license_template2.xml"
            szV2CFilename = "output/SL-UserMode/new_license.v2c"
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_new.xml"
        End If

        'load C2V file
        szC2V = load_file(szCurrentStateFilename)
        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading resultant_state_after_format.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'load license definition file
        license_template_number = 2
        szDXML(0) = load_file(xmlFilePath1)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading input/HL/new_license_template1.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        szDXML(1) = load_file(xmlFilePath2)
        If (szDXML(1) Is Nothing) Then
            Console.WriteLine("  error in loading input/HL/new_license_template2.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type And dynamic_memory_supported_key = True) Then
            szDXML(2) = load_file(xmlFilePath3)
            If (szDXML(2) Is Nothing) Then
                Console.WriteLine("  error in loading input/HL/new_license_template3.xml file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
            license_template_number += 1
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  License file ""output/HL/new_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/new_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/new_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'process modify license generation routine from
    'resultant_state_after_new.xml and modify_license_template1.xml
    '
    'return 0 if success, other if failure
    Public Function process_modify(ByVal license_type As License_Type) As Integer
        szC2V = Nothing
        szDXML(0) = Nothing
        szDXML(1) = Nothing
        Dim c2vFilePath As String = Nothing
        Dim templatePath1 As String = Nothing
        Dim templatePath2 As String = Nothing

        Console.WriteLine(" Process modify license:")

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            c2vFilePath = "output/HL/resultant_state_after_new.xml"
            templatePath1 = "input/HL/modify_license_template1.xml"
            templatePath2 = "input/HL/modify_license_template2.xml"
            szV2CFilename = "output/HL/modify_license.v2c"
            szUXMLFilename = "output/HL/resultant_state_after_modify.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            c2vFilePath = "output/SL-AdminMode/resultant_state_after_new.xml"
            templatePath1 = "input/SL-AdminMode/modify_license_template1.xml"
            szV2CFilename = "output/SL-AdminMode/modify_license.v2c"
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_modify.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            c2vFilePath = "output/SL-UserMode/resultant_state_after_new.xml"
            templatePath1 = "input/SL-UserMode/modify_license_template1.xml"
            szV2CFilename = "output/SL-UserMode/modify_license.v2c"
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_modify.xml"
        End If

        'load C2V file
        szC2V = load_file(c2vFilePath)
        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading resultant_state_after_new.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'load license definition file
        license_template_number = 1
        szDXML(0) = load_file(templatePath1)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading modify_license_template1.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type And dynamic_memory_supported_key = True) Then
            szDXML(1) = load_file(templatePath2)
            If (szDXML(1) Is Nothing) Then
                Console.WriteLine("  error in loading modify_license_template2.xml file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
            license_template_number += 1
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  License file ""output/HL/modify_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/modify_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/modify_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'process cancel license generation routine from
    'resultant_state_after_modify.xml and cancel_license_template1.xml
    '
    'return 0 if success, other if failure
    Public Function process_cancel(ByVal license_type As License_Type) As Integer
        szC2V = Nothing
        szDXML(0) = Nothing
        szDXML(1) = Nothing
        Dim c2vFilePath As String = Nothing
        Dim templatePath1 As String = Nothing
        Dim templatePath2 As String = Nothing

        Console.WriteLine(" Process cancel license:")

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            c2vFilePath = "output/HL/resultant_state_after_modify.xml"
            templatePath1 = "input/HL/cancel_license_template1.xml"
            templatePath2 = "input/HL/cancel_license_template2.xml"
            szV2CFilename = "output/HL/cancel_license.v2c"
            szUXMLFilename = "output/HL/resultant_state_after_cancel.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            c2vFilePath = "output/SL-AdminMode/resultant_state_after_modify.xml"
            templatePath1 = "input/SL-AdminMode/cancel_license_template1.xml"
            szV2CFilename = "output/SL-AdminMode/cancel_license.v2c"
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_cancel.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            c2vFilePath = "output/SL-UserMode/resultant_state_after_modify.xml"
            templatePath1 = "input/SL-UserMode/cancel_license_template1.xml"
            szV2CFilename = "output/SL-UserMode/cancel_license.v2c"
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_cancel.xml"
        End If

        'load C2V file
        szC2V = load_file(c2vFilePath)
        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading resultant_state_after_modify.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'load license definition file
        license_template_number = 1
        szDXML(0) = load_file(templatePath1)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading input/HL/cancel_license_template1.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If
        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type And dynamic_memory_supported_key = True) Then
            szDXML(1) = load_file(templatePath2)
            If (szDXML(1) Is Nothing) Then
                Console.WriteLine("  error in loading input/HL/cancel_license_template2.xml file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If
            license_template_number += 1
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_HL = license_type) Then
            Console.WriteLine("  License file ""output/HL/cancel_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/cancel_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/cancel_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'one license generation routine
    '
    'return 0 if success, other if failure
    Private Function generate_license() As Integer
        Dim i As Integer = 0
        Dim status As sntl_lg_status_t = sntl_lg_status_t.SNTL_LG_STATUS_OK
        Dim szInitScopeXML As String = Nothing
        Dim szStartScopeXML As String = Nothing
        Dim szLicenseDefinitionXML As String = Nothing
        Dim szGenerationScopeXML As String = Nothing
        Dim license As String = ""
        Dim resultant_state As String = ""

        'sntl_lg_initialize Initializes license generation library
        status = licGenHelp.sntl_lg_initialize(szInitScopeXML)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_initialize: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_INITIALIZE_FAILED
        End If

        'sntl_lg_start Starts the license generation.
        status = licGenHelp.sntl_lg_start(szStartScopeXML, vendorCode, sntl_lg_license_type_t.SNTL_LG_LICENSE_TYPE_UPDATE, szLicenseDefinitionXML, szC2V)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_start: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_START_FAILED
        End If

        'sntl_lg_apply_template Apply license definition to the license
        'state associated with the handle. You can call this API multiple
        'times in one license generation routine.
        For i = 0 To license_template_number - 1
            status = licGenHelp.sntl_lg_apply_template(szDXML(i))
            licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
            Console.WriteLine(("  sntl_lg_apply_template: " & error_msg))
            If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
                licGenHelp.sntl_lg_cleanup()
                Return Error_Message.SNTL_LG_SAMPLE_APPLY_TEMPLATE_FAILED
            End If
        Next i

        'sntl_lg_generate_license Generates the license.
        status = licGenHelp.sntl_lg_generate_license(szGenerationScopeXML, license, resultant_state)
        licGenHelp.sntl_lg_get_info(sntl_lg_info_type_t.SNTL_LG_INFO_LAST_ERROR_MESSAGE, error_msg)
        Console.WriteLine(("  sntl_lg_generate_license: " & error_msg))
        If (status <> sntl_lg_status_t.SNTL_LG_STATUS_OK) Then
            licGenHelp.sntl_lg_cleanup()
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (store_file(license, szV2CFilename) <> 0) Then
            licGenHelp.sntl_lg_cleanup()
            Console.WriteLine("  license file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        If (store_file(resultant_state, szUXMLFilename) <> 0) Then
            licGenHelp.sntl_lg_cleanup()
            Console.WriteLine("  resultant license container state file fail to save.")
            Return Error_Message.SNTL_LG_SAMPLE_STORE_FILE_FAILED
        End If

        'Clean up memories that used in the routine
        licGenHelp.sntl_lg_cleanup()
        Return 0
    End Function

    'process generation of base independent license that allows rehosting
    '
    'return 0 if success, other if failure
    Public Function process_new_rehost(ByVal license_type As License_Type) As Integer
        szDXML(0) = Nothing
        szDXML(1) = Nothing
        Dim templatePath As String = Nothing

        Console.WriteLine(" Process base independent license that allows rehosting:")

        'load license definition file
        license_template_number = 1

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            'load C2V file
            szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v"
            szC2V = load_file(szCurrentStateFilename)
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If

            templatePath = "input/SL-AdminMode/rehost_license_template.xml"
            szV2CFilename = "output/SL-AdminMode/rehost_license.v2c"
            szUXMLFilename = "output/SL-AdminMode/resultant_state_after_rehost.xml"
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            'load C2V file
            szCurrentStateFilename = "input/SL-UserMode/fingerprint.c2v"
            szC2V = load_file(szCurrentStateFilename)
            If (szC2V Is Nothing) Then
                Console.WriteLine("  error in loading input/SL-UserMode/fingerprint.c2v file.")
                Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
            End If

            templatePath = "input/SL-UserMode/rehost_license_template.xml"
            szV2CFilename = "output/SL-UserMode/rehost_license.v2c"
            szUXMLFilename = "output/SL-UserMode/resultant_state_after_rehost.xml"
        End If

        szDXML(0) = load_file(templatePath)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading rehost_license_template.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/rehost_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/rehost_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
        End If

        Return 0
    End Function

    'process generation of base independent detachable license
    '
    'return 0 if success, other if failure
    Public Function process_new_detach(ByVal license_type As License_Type) As Integer
        szDXML(0) = Nothing
        szDXML(1) = Nothing
        Dim templatePath As String = Nothing

        Console.WriteLine(" Process base independent detachable license:")

        'load C2V file
        szCurrentStateFilename = "input/SL-AdminMode/fingerprint.c2v"
        szC2V = load_file(szCurrentStateFilename)
        If (szC2V Is Nothing) Then
            Console.WriteLine("  error in loading input/SL-AdminMode/fingerprint.c2v file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        'load license definition file
        license_template_number = 1

        templatePath = "input/SL-AdminMode/detach_license_template.xml"
        szV2CFilename = "output/SL-AdminMode/detach_license.v2c"
        szUXMLFilename = "output/SL-AdminMode/resultant_state_after_detach.xml"

        szDXML(0) = load_file(templatePath)
        If (szDXML(0) Is Nothing) Then
            Console.WriteLine("  error in loading detach_license_template.xml file.")
            Return Error_Message.SNTL_LG_SAMPLE_LOAD_FILE_FAILED
        End If

        If (generate_license() <> 0) Then
            Return Error_Message.SNTL_LG_SAMPLE_GENERATE_NEW_FAILED
        End If

        If (license_type.SNTL_LG_SAMPLE_LICENSE_SL_ADMIN_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-AdminMode/detach_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
            Console.WriteLine("")
        ElseIf (license_type.SNTL_LG_SAMPLE_LICENSE_SL_USER_MODE = license_type) Then
            Console.WriteLine("  License file ""output/SL-UserMode/detach_license.v2c"" has been generated successfully.")
            Console.WriteLine("")
            Console.WriteLine("")
        End If

        Return 0
    End Function

End Class
