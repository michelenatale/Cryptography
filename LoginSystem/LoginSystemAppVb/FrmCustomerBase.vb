Option Strict On
Option Explicit On

Imports michele.natale.Pointers
Imports michele.natale.LoginSystems
Imports System.Security.Cryptography
Imports michele.natale.LoginSystems.Apps
Imports System.ComponentModel

Namespace LoginSystemApp
  Public Class FrmCustomerBase
    Inherits Form

    Public Const MASTER_PASSWORD_SIZE As Int32 = 48
    Public Const MASTER_PASSWORD_MAX_SIZE As Int32 = 128

    Public Login_System As WindowsManager = Nothing
    Public MyMasterPassword As UsIPtr(Of Byte) = UsIPtr(Of Byte).Empty

    Private ReadOnly Rand As RandomNumberGenerator = RandomNumberGenerator.Create()

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property App_Login_Setting As AppLoginSettings = Nothing

    Protected Sub FrmAppCustomerCTor()
      Dim new_master_password = Me.NewRngMasterPassword(MASTER_PASSWORD_SIZE)
      Me.MyMasterPassword = New UsIPtr(Of Byte)(new_master_password)
    End Sub

    Protected Sub FrmAppCustomerClose()
      If Not AppLoginSettings.IsNullOrEmpty(Me.App_Login_Setting) Then Me.App_Login_Setting.Reset()
      Me.MyMasterPassword?.Dispose()
    End Sub

    Protected Function To_App_Customer_Login_Setting() As Boolean
      If Me.Login_System IsNot Nothing Then
        Dim result As AppLoginSettings = Nothing
        If Me.Login_System.To_App_Login_Setting(result) Then
          Me.App_Login_Setting = result
          If result.MasterPassword.Length <> 0 Then Me.MyMasterPassword = New UsIPtr(Of Byte)(Me.App_Login_Setting.MasterPassword)
          Return True
        End If
      End If
      Return False
    End Function

    Protected Function Set_App_Customer_Login_Setting() As Boolean
      If Me.Login_System IsNot Nothing Then
        If Not AppLoginSettings.IsNullOrEmpty(Me.App_Login_Setting) Then
          Dim new_master_password = Me.NewRngMasterPassword(MASTER_PASSWORD_SIZE)
          Me.MyMasterPassword = New UsIPtr(Of Byte)(new_master_password)
          Me.App_Login_Setting.MasterPassword = Me.MyMasterPassword.ToArray()
          If Me.Login_System.Set_App_Login_Setting(Me.App_Login_Setting) Then Return True
        End If
      End If
      Return False
    End Function

    Protected Function NewRngMasterPassword(size As Int32) As Byte()
      Dim result = New Byte(size - 1) {}
      Me.Rand.GetNonZeroBytes(result)
      Return result
    End Function

    Protected Sub FrmCustomerDispose()
      Me.Login_System.FrmClose()
    End Sub
  End Class
End Namespace
