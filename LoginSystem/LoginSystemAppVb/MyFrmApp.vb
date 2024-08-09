Option Strict On
Option Explicit On

'The AppLoginSettings class Is your customer class.
'In this class, you can store the relevant information
'that ultimately makes up the login process.

'If only a Masterpassword Is important to you as a
'login, then this can be built into the AppLoginSettings
'class. If it Is other information, then set up the class
'according to your needs.

'The trick of the login procedure Is very simple. If you are
'logged in, the AppLoginSettings class Is also available
'(e.g. for your application). If you are Not logged in,
'this class Is Not available.

'You can change the information you store in the AppLoginSettings
'class at any time And save it again.

'On the other hand, you can also close LoginSystem completely
'once you have the information in AppLoginSettings.

Namespace LoginSystemApp

  Partial Public Class MyFrmApp

    Public Sub New()
      Me.InitializeComponent()
      Me.BtToLI.Enabled = True
      Me.BtSetLI.Enabled = False
      Me.FrmAppCustomerCTor()
      Me.Icon = My.Resources.MyLogo64
    End Sub

    Private Sub MyFrmApp_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
      Me.FrmAppCustomerClose()
    End Sub

    Private Sub BtToLI_Click(sender As Object, e As EventArgs) Handles BtToLI.Click
      Me.TbMPw.Text = "[ ... ]"

      If Me.To_App_Customer_Login_Setting() Then
        Me.BtSetLI.Enabled = True
        Me.TbMPw.Text = Convert.ToHexString(Me.MyMasterPassword.ToArray())
      End If
    End Sub

    Private Sub BtSetLI_Click(sender As Object, e As EventArgs) Handles BtSetLI.Click
      Me.TbMPw.Text = "[ ... ]"
      If Me.Set_App_Customer_Login_Setting() Then Me.TbMPw.Text = Convert.ToHexString(Me.MyMasterPassword.ToArray())
    End Sub
  End Class

End Namespace