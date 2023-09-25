'****************************************************************************
'
' Demo program for Sentinel LDK
'
'
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'****************************************************************************/

Imports SafeNet.Sentinel
Imports System.Text.RegularExpressions

Module AdminApiSample

    Private adminApi As AdminApi
    Private vendorId As Integer = "37515"

    Sub Main()
        Dim backupdata As String = ""

        Console.WriteLine("A simple demo program for the Sentinel LDK administration functions")
        Console.WriteLine("Copyright (C) 2013, SafeNet, Inc. All rights reserved.")
        Console.WriteLine()

        adminApi = New AdminApi("localhost", 1947, "")

        ''this initializes the adminApi
        sampleAdminConnect()

        ''different samples
        commonSamples(backupdata)
        scopeSamples(vendorId)
        licenseMangerSample()
        keyDescriptionSample()
        restoreData(backupdata)
    End Sub

#Region "samples"

    Sub commonSamples(ByRef backupdata As String)

        ''Retrieve current context
        sampleAdminGet("", "<context></context>")

        ''Retrieve all configuration settings (for backup)
        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""*""/>" & _
                    " </config>" & _
                    "</admin>", _
                    backupdata _
                    )

        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""friendlyname"" />" & _
                    " </config>" & _
                    "</admin>" _
                    )

        sampleAdminSet( _
                    "<config>" & _
                    " <enabledetach>1</enabledetach>" & _
                    "</config>" _
                    )

        ''Set configuration defaults and write to ini file
        sampleAdminSet( _
                    "<config>" & _
                    " <set_defaults />" & _
                    " <writeconfig />" & _
                    "</config>" _
                    )

        ''Change some settings and write to ini file"
        ''(For a list of available elements, see reply for \"*\" element below)
        sampleAdminSet( _
                    "<config>" & _
                    " <serveraddrs_clear/>" & _
                    " <serveraddr>10.24.2.16   </serveraddr>" & _
                    " <serveraddr>    10.24.2.16</serveraddr>" & _
                    " <serveraddr>p4p</serveraddr>" & _
                    " <serveraddr>10.24.2.255</serveraddr>" & _
                    " <user_restrictions_clear/>" & _
                    " <user_restriction>deny=baerbel@all</user_restriction>" & _
                    " <access_restrictions_clear />" & _
                    " <access_restriction>deny=10.23.*</access_restriction>" & _
                    " <enabledetach>1</enabledetach>" & _
                    " <writeconfig />" & _
                    "</config>" _
                    )

        ''Retrieve all configuration settings
        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""*"" />" & _
                    " </config>" & _
                    "</admin>" _
                    )

        ''Add some access restrictions
        sampleAdminSet( _
                    "<config>" & _
                    " <access_restriction>allow=123</access_restriction>" & _
                    " <access_restriction>allow=abcd</access_restriction>" & _
                    " <access_restriction>allow=hello_world</access_restriction>" & _
                    " <writeconfig />" & _
                    "</config>" _
                    )

        ''Retrieve all access restrictions
        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""access_restriction"" />" & _
                    " </config>" & _
                    "</admin>" _
                    )

        ''Add some more access restrictions
        sampleAdminSet( _
                    "<config>" & _
                    " <access_restriction>allow=more_123</access_restriction>" & _
                    " <access_restriction>allow=more_abcd</access_restriction>" & _
                    " <access_restriction>allow=more_hello_world</access_restriction>" & _
                    " <writeconfig />" & _
                    "</config>" _
                    )

        ''Retrieve all access restrictions
        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""access_restriction"" />" & _
                    " </config>" & _
                    "</admin>" _
                    )

        ''Delete existing access restrictions and add some new ones
        sampleAdminSet( _
                    "<config>" & _
                    " <access_restrictions_clear/>" & _
                    " <access_restriction>allow=new_123</access_restriction>" & _
                    " <access_restriction>allow=new_abcd</access_restriction>" & _
                    " <writeconfig />" & _
                    "</config>" _
                    )

        ''Retrieve all access restrictions
        sampleAdminGet("", _
                    "<admin>" & _
                    " <config>" & _
                    "  <element name=""access_restriction"" />" & _
                    " </config>" & _
                    "</admin>" _
                    )
    End Sub

    ''using haspscope to retrieve filtered data
    Sub scopeSamples(ByVal vendorId As Integer)

        ''Retrieve some key data for specified vendor (scope with attribute notation)
        ''(for a list of available elements, see reply for \"*\" element below)
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <vendor id=""" & vendorId & """ />" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <hasp>\n" & _
                    "  <element name=""vendorid"" />" & _
                    "  <element name=""haspid"" />" & _
                    "  <element name=""typename"" />" & _
                    "  <element name=""local"" />" & _
                    "  <element name=""localname"" />" & _
                    " </hasp> " & _
                    "</admin>" _
                    )

        ''Retrieve key data for specified vendor (scope with element notation)
        sampleAdminGet( _
                    "<haspscope>\n" & _
                    " <vendor><id>" & vendorId & "</id></vendor>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <hasp>" & _
                    "  <element name=""vendorid"" />" & _
                    "  <element name=""haspid"" />" & _
                    "  <element name=""typename"" />" & _
                    "  <element name=""local"" />" & _
                    "  <element name=""localname"" />" & _
                    " </hasp>" & _
                    "</admin>" _
                    )

        ''Retrieve all product data for specified vendor id"
        sampleAdminGet( _
                    "<haspscope>\n" & _
                    " <vendor><id>" & vendorId & "</id></vendor>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <product>" & _
                    "  <element name=""*"" />" & _
                    " </product>" & _
                    "</admin>" _
                    )

        ''Retrieve selected session data for all keys of a specified vendor
        sampleAdminGet( _
                    "<haspscope>\n" & _
                    " <vendor><id>" & vendorId & "</id></vendor>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <session>" & _
                    "  <element name=""user"" />" & _
                    "  <element name=""machine"" />" & _
                    "  <element name=""logintime"" />" & _
                    " </session>" & _
                    "</admin>" _
                    )


    End Sub

    ''Sample where the scope uses the key-id
    Sub keyIdSamples(ByVal keyId As Int32)

        ''Retrieve all key data for specified key id"
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <hasp><id>" & keyId & "</id></hasp>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <hasp>" & _
                    "  <element name=""*"" />" & _
                    " </hasp>" & _
                    "</admin>" _
                    )


        ''Retrieve all feature data for specified key id"
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <hasp><id>" & keyId & "</id></hasp>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <feature>" & _
                    "  <element name=""*"" />" & _
                    " </feature>" & _
                    "</admin>" _
                    )

        ''Retrieve list of current sessions for a specified key
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <hasp><id>" & keyId & "</id></hasp>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <session>" & _
                    "  <element name=""*"" />" & _
                    " </session>" & _
                    "</admin>" _
                    )

        ''Retrieve all product data for specified key id"
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <hasp><id>" & keyId & "</id></hasp>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <product>" & _
                    "  <element name=""productid"" />" & _
                    "  <element name=""productname"" />" & _
                    "  <element name=""detachable"" />" & _
                    "  <element name=""maxseats"" />" & _
                    "  <element name=""seatsfree"" />" & _
                    " </product>" & _
                    "</admin>" _
                    )


    End Sub

    ''Retrieve License Manager and license related data
    Sub licenseMangerSample()

        ''Retrieve license manager data in XML format (default)
        sampleAdminGet("", _
                   "<admin>" & _
                    " <license_manager>" & _
                    "  <element name=""*"" />" & _
                    " </license_manager>" & _
                    "</admin>", _
                    )


        ''Retrieve license manager data in JSON format
        sampleAdminGet("", _
                    "<admin>" & _
                    " <license_manager format=""json"">" & _
                    "  <element name=""*"" />" & _
                    " </license_manager>" & _
                    "</admin>" _
                    )

        ''Retrieve list of detached licenses
        sampleAdminGet("", _
                    "<admin>" & _
                    " <detached>" & _
                    "  <element name=""*"" />" & _
                    " </detached>" & _
                    "</admin>" _
                    )
    End Sub

    ''Setting a key description (disabled because it was not backed up)
    Sub keyDescriptionSample()
        ''Add a key description (legacy format)
        sampleAdminSet( _
                    "<?xml version=""1.0"" encoding=""UTF-8""?>" & _
                    "<keydescription>" & _
                    " <hasp>" & _
                    "  <id>123456</id>" & _
                    "  <name>One two three four five six</name>" & _
                    " </hasp>" & _
                    "</keydescription>" _
                    )
    End Sub

    ''Examples for uploading of files (disabled because reverting not possible)
    Sub fileUploadSample()

        ''Upload a detach location data file
        sampleAdminSet("file://test_location.xml")

        ''Upload a key names metadata file");
        sampleAdminSet("file://test_key.xml")

        ''Upload a product names metadata file
        sampleAdminSet("file://test_product.xml")

        ''Upload a vendor names metadata file
        sampleAdminSet("file://test_vendor.xml")

        ''Applying V2C
        sampleAdminSet("file://test_update.v2c")

    End Sub

    ''Example how to delete a some sessions
    Sub deleteSessionSample()
        sampleAdminSet( _
                    "<admin>" & _
                    " <deletesession>" & _
                    "  <sessionid>1</sessionid>" & _
                    "  <session id=""2"" />" & _
                    "  <session id=""3"" />" & _
                    "  <sessionid>4</sessionid>" & _
                    " </deletesession>" & _
                    "</admin>" _
                    )

    End Sub

    ''Retrieve list of certificates for specified key (XML)
    Sub certificateSample(ByVal keyId As Int32)
        sampleAdminGet( _
                    "<haspscope>" & _
                    " <hasp><id>" & keyId & "</id></hasp>" & _
                    "</haspscope>", _
                    "<admin>" & _
                    " <certificates>" & _
                    "  <element name=""*"" />" & _
                    " </certificates>" & _
                    "</admin>" _
                    )
    End Sub

#End Region

#Region "wrapper functions"

    Sub sampleAdminGet(ByVal scope As String, ByVal format As String, Optional ByRef target As String = Nothing)

        Dim rc As AdminStatus
        Dim info As String = ""

        Console.WriteLine("sntl_admin_get()")

        rc = adminApi.adminGet(scope, format, info)

        If Not (target Is Nothing) Then
            target = info
        Else
            printState(rc, info)
        End If

    End Sub

    Sub sampleAdminSet(ByVal config As String)

        Dim rc As AdminStatus
        Dim returnStatus As String = ""

        Console.WriteLine("sntl_admin_set()")
        rc = adminApi.adminSet(config, returnStatus)
        printState(rc, returnStatus)

    End Sub

    Sub sampleAdminConnect()
        Dim rc As AdminStatus

        rc = adminApi.connect()
        printState(rc)

    End Sub

#End Region

#Region "help functions"

    Sub restoreData(ByVal backupdata As String)
        Dim config As String = ""
        Dim returnStatus As String = ""

        generateConfigFromOutput(backupdata, config)

        adminApi.adminSet( _
                        "<config>" & _
                        " <serveraddrs_clear/>" & _
                        " <user_restrictions_clear/>" & _
                        " <access_restrictions_clear/>" & _
                        " <writeconfig/>" & _
                        "</config>", returnStatus _
                        )

        adminApi.adminSet(config, returnStatus)

    End Sub

    Sub printState(ByVal status As AdminStatus, Optional ByVal info As String = Nothing)

        If Not (String.IsNullOrEmpty(info)) Then
            Console.WriteLine(info)
        End If

        Console.WriteLine("Result: " & getErrorText(status) & " Statuscode: " & status)
        Console.WriteLine()

    End Sub

    Sub generateConfigFromOutput(ByVal output As String, ByRef config As String)
        If Not (String.IsNullOrEmpty(output)) Then
            Dim regex As New Regex("<admin_status>")
            Dim result() As String = regex.Split(output)

            config = result(0).Replace("</config>", "<writeconfig /></config>")
        End If
    End Sub

    Function getErrorText(ByVal status As AdminStatus) As String
        Select Case status
            Case AdminStatus.StatusOk
                Return "StatusOk"
            Case AdminStatus.InsufMem
                Return "InsufMem"
            Case AdminStatus.InvalidContext
                Return "InvalidContext"
            Case AdminStatus.LmNotFound
                Return "LmNotFound"
            Case AdminStatus.LmTooOld
                Return "LmTooOld"
            Case AdminStatus.BadParameters
                Return "BadParameters"
            Case AdminStatus.LocalNetWorkError
                Return "LocalNetWorkError"
            Case AdminStatus.CannotReadFile
                Return "CannotReadFile"
            Case AdminStatus.ScopeError
                Return "ScopeError"
            Case AdminStatus.PasswordRequired
                Return "PasswordRequired"
            Case AdminStatus.CannotSetPassword
                Return "CannotSetPassword"
            Case AdminStatus.UpdateError
                Return "UpdateError"
            Case AdminStatus.LocalOnly
                Return "LocalOnly"
            Case AdminStatus.BadValue
                Return "BadValue"
            Case AdminStatus.ReadOnly
                Return "ReadOnly"
            Case AdminStatus.ElementUndefined
                Return "ElementUndefined"
            Case AdminStatus.InvalidPointer
                Return "InvalidPointer"
            Case AdminStatus.NoIntegratedLm
                Return "NoIntegratedLm"
            Case AdminStatus.ResultTooBig
                Return "ResultTooBig"
            Case AdminStatus.ScopeResultsEmpty
                Return "ScopeResultsEmpty"
            Case AdminStatus.InvalidVendorCode
                Return "InvalidVendorCode"
            Case AdminStatus.UnknownVendorCode
                Return "UnknownVendorCode"
            Case AdminStatus.DotNetDllBroken
                Return "DotNetDllBroken"
            Case AdminStatus.ConnectMissing
                Return "DotNetDllBroken"
        End Select
        Return ""
    End Function
#End Region

End Module
