
# LoginSystem

LoginSystem is a simple project that shows how to perform a local login for your application. It is based on the **Winform-Mvvm-Disign** Pattern and UserControls.

In order for © LoginSystem 2024 to work, a registration must first be made. Only then can a login be made. 

You will find a short guide below.


### Regist
In order to use © LoginSystem 2024, you must first register. 

The following parameters are required:
- A Username (without the @ character and at least 10 characters long) 
- A Password of at least 10 characters
- A valid E-Mail address.

During registration, a file called 'LoginSystemSecretData' is created which contains all your personal login data for © LoginSystem 2024. Keep this data in a safe place and keep it secret. An LS-SPW-F key is also generated, which is used for PwForget and PwChange.

If the registration has worked, the login page is loaded automatically.
![](https://github.com/michelenatale/Cryptography/blob/main/LoginSystem/Documentation/01_Regist.gif) 

### Login
The password, username or email address is used for the login.

The AppLoginSettings class is your customer class. In this class, you can store the relevant information that ultimately makes up the login process.

If only a Masterpassword is important to you as a login, then this can be built into the AppLoginSettings class. If it is other information, then set up the class according to your needs.

The trick of the login procedure is very simple. If you are logged in, the AppLoginSettings class is also available (e.g. for your application). If you are not logged in, this class is not available.

You can change the information you store in the AppLoginSettings class at any time and save it again.

On the other hand, you can also close LoginSystem completely once you have the information in AppLoginSettings. 

However, there is still an option if this is not desired. An event is provided that returns feedback to your system. This can prevent your system from starting up twice.
![](https://github.com/michelenatale/Cryptography/blob/main/LoginSystem/Documentation/02_Login.gif)


### PwForget
The password, username or email address is used for the login.

If the password has been forgotten, a new password can be activated in © LoginSystem 2024.

The valid email address is used for this purpose. You can use your personal email account for this. An input mask is available for this purpose.

© LoginSystem 2024 resets the password and creates a new one, and sends it directly back to your email account. This password can then be used the next time you log in.

The LS-SPW-F key is also required for PwForget to work properly. 

If PwForget has worked, the login mask is loaded again. Please change your password again after running PwForget.
![](https://github.com/michelenatale/Cryptography/blob/main/LoginSystem/Documentation/03_PwForget.gif)


### PwChange
Please change your password again after running PwForget.

You must be logged in, to change the password. Changing the password is very simple and self-explanatory and the LS-SPW-F key is used again at the end.

From this point on, you can use your new password for the login process.
![](https://github.com/michelenatale/Cryptography/blob/main/LoginSystem/Documentation/04_PwChange.gif)


### Conclusion
As soon as you are logged in, the AppLoginSettings class can be requested for your application, which contains your personal information that is needed for your application.

As an example, the master password is requested here and changed again. 

You can see immediately that as long as you are logged in, you can make changes in the AppLoginSettings class. If you close © LoginSystem 2024 (corresponds to logging out), access to the AppLoginSettings class is no longer granted.
![](https://github.com/michelenatale/Cryptography/blob/main/LoginSystem/Documentation/05_FunctionLoginSystem.gif)






