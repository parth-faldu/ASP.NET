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

        /* Header Section - Top Right Corner */
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

        /* Main Dashboard Container */
        .dashboard-container {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 60px;
            max-width: 1200px;
            margin: 0 auto;
            padding:30px;
        }

        /* Left Section (Expense Tracker) */
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

        input[type="text"], input[type="number"], input[type="date"] {
            width: 95%;
            padding: 10px;
            margin: 5px 0;
            background-color: #333;
            border: 1px solid #444;
            border-radius: 5px;
            color: #fff;
        }

        input[type="text"]:focus, input[type="number"]:focus, input[type="date"]:focus {
            outline: none;
            border-color: #005cff;
        }

        button {
            background-color: #ff4081;
            color: #fff;
            padding: 12px 20px;
            border-radius: 5px;
            width: 100%;
            border: none;
            cursor: pointer;
            margin-top: 15px;
        }

        button:hover {
            background-color: #e20073;
        }
        #expenseChart {
            padding:5px 50px 50px 50px;
        }
        /* Right Section (Pie Chart) */
        .right-section {
            background-color: #1e1e1e;
            padding: 20px;
            border-radius: 8px;
            width: 37%;
            height:460px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            padding:5px 50px 30px 50px;
        }

        /* Footer Section (if required) */
        .footer {
            text-align: center;
            color: #aaa;
            font-size: 14px;
            margin-top: 20px;
        }

    </style>
</head>
<body>

    <!-- Header with username on the top-right -->
    <div class="header">
        <h2>Budget Buddy</h2>
        <h2>Welcome, [Username]</h2>
    </div>

    <!-- Main Dashboard Section -->
    <div class="dashboard-container">
        <!-- Left Section: Expense Tracker Form -->
        <div class="left-section">
            <h3>Expense Tracker</h3>

            <!-- Expense Input Form -->
            <div class="form-group">
                <label for="txtCategory">Category:</label>
                <input type="text" id="txtCategory" placeholder="Enter Category (e.g., Food, Rent)" required />
            </div>

            <div class="form-group">
                <label for="txtAmount">Amount:</label>
                <input type="number" id="txtAmount" placeholder="Enter Amount" required />
            </div>

            <div class="form-group">
                <label for="txtDescription">Description:</label>
                <input type="text" id="txtDescription" placeholder="Enter Description (optional)" />
            </div>

            <div class="form-group">
                <label for="txtDate">Date:</label>
                <input type="date" id="txtDate" required />
            </div>

            <button type="submit">Add Expense</button>
        </div>

        <!-- Right Section: Pie Chart -->
        <div class="right-section">
            <h3>Expense Overview</h3>
            <canvas id="expenseChart"></canvas>
        </div>
    </div>

    <!-- Footer (Optional) -->
    <div class="footer">
        <p>&copy; 2025 Budget Buddy. All Rights Reserved.</p>
    </div>

    <script>
        // Example Data for Pie Chart
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
                responsive: true
            }
        });
    </script>

</body>
</html>