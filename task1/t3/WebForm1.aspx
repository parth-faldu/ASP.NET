<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="t3.WebForm1" MasterPageFile="~/Site1.master" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div class="navbar">
        <div class="nav-logo">
        <img src="~/image/logo.jpg" alt="Progress Telerik Logo" class="logo" runat="server"  />
        <p>Progress &nbsp;Telerik</p>
        </div>
        <ul class="nav-links">
            <li><a href="#">Products</a></li>
            <li><a href="#">Enterprise</a></li>
            <li><a href="#">Free Trials</a></li>
            <li><a href="#">Pricing</a></li>
            <li><a href="#">Support & Learning</a></li>
            <li><a href="#">About Us</a></li>
        </ul>
    </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hero-section">
       <img src="~/image/hero-img.jpg" alt="Hero Background" class="hero-image" runat="server" />
        <div class="hero-text">
            <h1>Develop Experiences</h1>
            <p>UI frameworks and app development tools that 2.1 million developers love</p>
        </div>
    </div>

    <div class="cards">
        <div class="card">
            <h3>.NET</h3>
            <h2>DevCraft</h2>
            <p>.NET UI controls, reporting and developer productivity tools</p>
            <a href="#" class="btn">Learn More</a>
        </div>
        <div class="card">
            <h3>NATIVE MOBILE</h3>
            <h2>NativeScript</h2>
            <p>Open source framework for building truly native mobile apps with Angular, TypeScript or JavaScript</p>
            <a href="#" class="btn">Learn More</a>
        </div>
        <div class="card">
            <h3>HTML5</h3>
            <h2>Kendo UI</h2>
            <p>JavaScript, HTML5 UI widgets for responsive web and data visualization</p>
            <a href="#" class="btn">Learn More</a>
        </div>
        <div class="card">
            <h3>CMS</h3>
            <h2>Progress Sitefinity</h2>
            <p>Web Content Management and Customer Analytics for managing and optimizing digital experiences</p>
            <a href="#" class="btn">Learn More</a>
        </div>
        <div class="card">
            <h3>MOBILE</h3>
            <h2>Telerik Platform</h2>
            <p>Complete cross-platform solution to design, build, deploy, manage, and measure all your mobile apps</p>
            <a href="#" class="btn">Learn More</a>
        </div>
        <div class="card">
            <h3>TESTING</h3>
            <h2>Test Studio</h2>
            <p>Release better quality software faster with an intuitive and easy to use test automation solution</p>
            <a href="#" class="btn">Learn More</a>
        </div>
    </div>
</asp:Content>