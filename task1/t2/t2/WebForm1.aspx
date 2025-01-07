<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="t2.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        * {
            margin:0;
            padding:0;
            box-sizing:border-box;
        }
        .flex {
            display:flex;
            justify-content:center;
            align-items:center;
        }
        body {
            height:100vh;
        }
        .container {
            padding:20px;
            width:500px;
            text-align:center;
        }
        .form-con {
            width:100%;
            text-align:center;
        }
        .checkbox-forgot {
            justify-content:space-between;
        }
    </style>
</head>
<body class="flex">
    <form id="form1" runat="server">
    <div class="container"">
        <h1>Login Form Using HTML and CSS</h1>
        <div class="form-con">
            <p>Sign in</p>
            <p>Sign in with your username and password</p>
            <div>
                <p>Your Username</p>
                <input type="text" placeholder="Enter Username" required/>
            </div>
            <div>
                <p>Your Password</p>
                <input type="text" placeholder="Enter Password" required/>
            </div>
            <div class="checkbox-forgot flex">
                <div>
                    <input  type="checkbox"/>
                    <label>Remember me</label>
                </div>
                <a href="#">Forgot Password?</a>
            </div>
            <button>Login</button>
            <p>Not a member?<a>Register Here!</a></p>
        </div>
    </div>
    </form>
</body>
</html>
