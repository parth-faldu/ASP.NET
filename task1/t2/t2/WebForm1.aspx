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
        body {
            height:100vh;
            background: linear-gradient(to right, #156890, #149366);
            color:white;
            font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif
        }
        .container {
            padding:20px;
            width:600px;
        }
        .form-con {
            width:100%;
            padding:2rem 2rem;
            background-color: rgba(0, 0, 0, 0.4);
            border-radius: 5px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
        }
        .checkbox-forgot {
            justify-content:space-between;
        }
        .grid {
            display:grid;
            gap:2rem;
        }
        input[type=text] {
            width:100%;
            padding:10px;
            margin:5px 0;
            border:none;
        }
        .sign-in-con {
            text-align:center;
            gap:0;
        }
        .c-g {
            color:#507279;
        }
        .d-f {
            display:flex;
        }
        .j-c {
            justify-content:center;
        }
        .j-s {
            justify-content:space-between;
        }
        .a-c{
            align-items:center;
        }
        button {
            padding:10px;
            background-color:#050706;
            color:white;
            border:none;
        }
        a {
            text-decoration:none;
            color:#3aa8e3;
        }
        .br-5 {
            border-radius:5px;
        }
        .br-10 {
            border-radius:10px;
        }
        h1 {
            margin:20px 0;
            text-align:center;
        }
    </style>
</head>
<body class="d-f j-c a-c">
    <form id="form1" runat="server">
    <div class="container"">
        <h1>Login Form Using HTML and CSS</h1>
        <div class="form-con grid br-10">
            <div class="sign-in-con grid">
                <label>Sign in</label></br>
                <label class="cz">Sign in with your username and password</label>
            </div>
            
            <div>
                <label>Your Username</label>
                <input type="text" class="br-5" placeholder="Enter Username" required/>
            </div>

            <div>
                <label>Your Password</label>
                <input type="text" class="br-5" placeholder="Enter Password" required/>
            </div>
            <div class="d-f j-s a-c">
                <div>
                    <input  type="checkbox"/>
                    <label>Remember me</label>
                </div>
                <a href="#">Forgot Password?</a>
            </div>
            <button class="br-5">Login</button>
            <div class="d-f j-c a-c">
                <p>Not a member?</p>
                <a href="#">Register Here!</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
