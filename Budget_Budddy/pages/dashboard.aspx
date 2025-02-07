<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Budget_Budddy.pages.dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy - Dashboard</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>
        /* Global Styles */
        body {
            font-family: 'Poppins', sans-serif;
            margin: 0;
            padding: 0;
            background-color: #121212;
            color: #fff;
        }

        /* Header Section */
        .header {
            display: flex;
            justify-content: space-between;
            padding: 20px 0;
            background-color: #1e1e1e;
            width: 100%;
            z-index: 10;
        }

        .header h2 {
            color: #ff4081;
            margin: 0;
            padding: 0 20px;
        }

        /* Logout Button */
        .logout-button {
            background-color: #ff4081;
            color: white;
            border: none;
            padding: 10px 15px;
            cursor: pointer;
            font-size: 14px;
            border-radius: 5px;
        }

        .logout-button:hover {
            background-color: #e20073;
        }

        /* Dashboard Layout */
        .dashboard-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 60px;
            max-width: 1200px;
            margin: 0 auto;
            padding:30px;
        }

        /* Expense Tracker */
        .left-section {
            background-color: #1e1e1e;
            padding: 20px;
            border-radius: 8px;
            width: 48%;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
        }

        .left-section h3 {
            margin-bottom: 15px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .input-field {
            width: 95%;
            padding: 10px;
            margin: 5px 0;
            background-color: #333;
            border: 1px solid #444;
            border-radius: 5px;
            color: #fff;
        }

        .input-field:focus {
            outline: none;
            border-color: #005cff;
        }

        /* Add Expense Button */
        .btn-add {
            background-color: #ff4081;
            color: #fff;
            padding: 12px 20px;
            border-radius: 5px;
            width: 100%;
            border: none;
            cursor: pointer;
            margin-top: 15px;
        }

        .btn-add:hover {
            background-color: #e20073;
        }

        /* Expense Chart Section */
        .right-section {
            background-color: #1e1e1e;
            padding: 20px;
            border-radius: 8px;
            width: 37%;
            aspect-ratio:1;
            height: 460px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
        }

        /* Footer */
        .footer {
            text-align: center;
            color: #aaa;
            font-size: 14px;
            margin-top: 20px;
        }
        .user-logout-con {
            display:flex;
            justify-content:center;
            align-items:center;

            & a{
                text-decoration:none;
            }
        }
        #expenseChart {
            margin-top:50px;
        }
       .chart-container {
        position: relative;
        width: 100%;
        max-width: 500px;
        min-width: 300px; 
        aspect-ratio: 1 / 1; 
        margin: auto;
    }

    canvas {
        width: 100% !important;
        height: auto !important;
    }
    }

    @media screen and (max-width: 600px) {
        .chart-container {
            max-width: 350px;
            min-width: 250px;
        }

        #expenseChart {
            width:70% !important;
        }
       
    }
        @media screen and (max-width:778px) {
            .right-section {
                width:90%;
            }
             .dashboard-container {
            flex-direction: column;
        }
        }

    /* Responsive Adjustments */
    @media screen and (max-width: 998px) {
        #expenseChart {
            padding:0px 30px 100px 10px;
            margin-top:unset;
        }
    }
        @media screen and (max-width:1100px) {
            #expenseChart {
                padding:unset;
            }
        }
        .chart-container {
            max-width:400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header with Username and Logout -->
        <div class="header">
            <h2>Budget Buddy</h2>
            <div class="user-logout-con">
                <h2>Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal></h2>
                <asp:LinkButton ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click"
    CssClass="logout-button" CausesValidation="false"/>

            </div>
        </div>

        <!-- Main Dashboard Section -->
        <div class="dashboard-container">
            <!-- Left Section: Expense Tracker -->
            <div class="left-section">
                <h3>Expense Tracker</h3>

                <!-- Expense Input Form -->
                <div class="form-group">
                    <label for="txtCategory">Category:</label>
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="input-field" placeholder="Enter Category" required ></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtAmount">Amount:</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="input-field" TextMode="Number" required ValidationGroup="ExpenseForm"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtDescription">Description:</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="input-field" placeholder="Enter Description"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtDate">Date:</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="input-field" TextMode="Date" required ></asp:TextBox>
                </div>

                <asp:Button ID="btnAddExpense" runat="server" Text="Add Expense" CssClass="btn-add" OnClick="btnAddExpense_Click"/>
            </div>

            <!-- Right Section: Pie Chart -->
            <div class="right-section">
    <h3>Expense Overview</h3>
    <div class="chart-container">
        <canvas id="expenseChart"></canvas>
    </div>
</div>
        </div>

        <!-- Footer -->
        <div class="footer">
            <p>&copy; 2025 Budget Buddy. All Rights Reserved.</p>
        </div>
    </form>

    <script>
        var ctx = document.getElementById('expenseChart').getContext('2d');
        var expenseChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Food', 'Rent', 'Transportation', 'Entertainment'],
                datasets: [{
                    label: 'Expenses',
                    data: [50, 200, 100, 150], // Example amounts
                    backgroundColor: ['#ff4081', '#005cff', '#e20073', '#333'],
                    borderColor: '#fff',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                layout: {
                    padding: 10
                }
            }
        });
</script>
</body>
</html>
