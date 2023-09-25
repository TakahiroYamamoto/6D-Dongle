'****************************************************************************
'
' Demo program for Sentinel LDK
'
'
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'****************************************************************************/
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports SafeNet.Sentinel
Imports Aladdin.HASP


Module AdminApiSample



    Private adminApiIntegrated As AdminApi
    Private adminApiStandalone As AdminApi

    Sub Main()

        Dim status As AdminStatus
        Dim data As String = ""

        ' You can specify some other server IP address (from different subnet also) hosting Sentinel License Manager Service
        Dim server As String = "127.0.0.1"

        Console.WriteLine("A simple demo program for the Sentinel LDK administration functions")
        Console.WriteLine("Copyright (C) 2013, SafeNet, Inc. All rights reserved." & vbLf & vbLf)

        ' Below is DEMOMA vendor code. ISV can update below with their assigned vendor code
        Dim vc As New VendorCodeType( _
            "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsV" & _
            "vIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrB" & _
            "rh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6" & _
            "AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf" & _
            "8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1" & _
            "e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBh" & _
            "aJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SK" & _
            "unFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bL" & _
            "Cx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwL" & _
            "zaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9s" & _
            "JN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaT" & _
            "tLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" _
            )

        Console.WriteLine("connect to sntl_integrated_lm ")

        adminApiIntegrated = New AdminApi(vc, "sntl_integrated_lm")

        status = adminApiIntegrated.connect()

        printState(status)

        Console.WriteLine("adminGet ")

        status = adminApiIntegrated.adminGet( _
            "<haspscope/>", _
            "<context></context>", _
            data)

        printState(status, data)

        Console.WriteLine("adminSet ")

        status = adminApiIntegrated.adminSet( _
            "<config>" & _
            " <serveraddrs_clear/>" & _
            " <server_select>" & server & "</server_select>" & _
            "</config>", _
            data _
            )

        printState(status, data)

        Console.WriteLine("login to default feature of any key on " & server)

        Dim haspAny As New Hasp(HaspFeature.Default)
        Dim haspStatus As HaspStatus = haspAny.Login(vc.ToString(), _
                                                     "<?xml version=""1.0"" encoding=""UTF-8"" ?>" & _
                                                     "<haspscope>" & _
                                                     " <license_manager ip=""" & server & """ />" & _
                                                     "</haspscope>" _
                                                     )

        printState(haspStatus)

        Console.WriteLine("login to default feature of SL-AdminMode key on " & server)

        Dim haspSlAm As New Hasp(HaspFeature.Default)
        haspStatus = haspSlAm.Login(vc.ToString(), _
                                    "<?xml version=""1.0"" encoding=""UTF-8"" ?>" & _
                                    "<haspscope>" & _
                                    " <hasp type=""HASP-SL-AdminMode"" >" & _
                                    "  <license_manager ip=""" & server & """ />" & " </hasp>" & _
                                    "</haspscope>" _
                                    )

        printState(haspStatus)

        status = adminApiIntegrated.adminGet( _
            "<haspscope/>", _
            "<admin>" & _
            "  <license_manager>" & _
            "   <element name=""version"" />" & _
            "   <element name=""servername"" />" & _
            "   <element name=""uptime"" />" & _
            "   <element name=""driver_info"" />" & _
            "  </license_manager>" & _
            "</admin>", _
            data _
            )

        printState(status, data)

        ' Access Standalone LMS using Integrated Admin API

        Console.WriteLine("connect to Standalone LMS")

        adminApiStandalone = New AdminApi(vc, server, 1947, "")

        status = adminApiStandalone.connect()

        printState(status)

        Console.WriteLine("adminGet ")
        ' retrieve sessions
        status = adminApiIntegrated.adminSet( _
            "<config>" & _
            " <serveraddrs_clear/>" & _
            " <server_select>" & server & "</server_select>" & _
            "</config>", _
            data _
            )

        status = adminApiIntegrated.adminSet( _
            "<config>" & _
            " <serveraddrs_clear/>" & _
            " <server_select>" & server & "</server_select>" & _
            "</config>", _
            data _
            )

        printState(status, data)

        Console.WriteLine("adminGet ")

        status = adminApiStandalone.adminGet( _
            "<haspscope/>", _
            "<context></context>", _
            data _
            )

        printState(status, data)

        Console.WriteLine("adminGet ")

        status = adminApiStandalone.adminGet( _
            "<haspscope/>", _
            "<admin>" & _
            "  <license_manager>" & _
            "   <element name=""version"" />" & _
            "   <element name=""servername"" />" & _
            "   <element name=""uptime"" />" & _
            "   <element name=""driver_info"" />" & _
            "  </license_manager>" & _
            "</admin>", _
            data _
            )

        printState(status, data)

        haspAny.Logout()
        haspSlAm.Logout()
    End Sub


#Region "help functions"

    Sub printState(status As HaspStatus)
        printState(DirectCast(CInt(status), AdminStatus), Nothing)
    End Sub

    Sub printState(status As AdminStatus)
        printState(status, Nothing)
    End Sub

    Sub printState(status As AdminStatus, info As String)
        If Not (String.IsNullOrEmpty(info)) Then
            Console.WriteLine(info)
        End If

        Console.WriteLine("Result: " & getErrorText(status) & " Statuscode: " & CInt(status) & vbLf)
    End Sub

    Sub generateConfigFromOutput(output As String, ByRef config As String)
        If output IsNot Nothing Then
            Dim regex As New Regex("<admin_status>")
            Dim result As String() = regex.Split(output)

            config = result(0).Replace("</config>", "<writeconfig /></config>")
        End If
    End Sub

    Function getErrorText(status As AdminStatus) As String
        Select Case status
            Case AdminStatus.StatusOk
                Return "StatusOk"
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
            Case AdminStatus.InvalidVendorCode
                Return "InvalidVendorCode"
            Case AdminStatus.UnknownVendorCode
                Return "UnknownVendorCode"
        End Select
        Return ""
    End Function

#End Region

End Module
