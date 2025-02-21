// Toggle the Settings dropdown menu
function toggleDropdown() {
    var dropdown = document.getElementById("settingsDropdown");
    if (dropdown.style.display === "none" || dropdown.style.display === "") {
        dropdown.style.display = "block";
    } else {
        dropdown.style.display = "none";
    }
}

// Update the chart using data from the hiddenExpenseData field
function updateChart() {
    try {
        var hiddenDataElement = document.getElementById(hiddenExpenseDataClientID);
        if (!hiddenDataElement) {
            console.error("Error: hiddenExpenseDataClientID element not found.");
            return;
        }

        var hiddenData = hiddenDataElement.value || "[]"; // Ensure it's a string
        var expenseData;
        try {
            expenseData = JSON.parse(hiddenData);
        } catch (err) {
            console.error("Error parsing JSON:", err);
            expenseData = [];
        }

        if (!Array.isArray(expenseData)) {
            console.error("Error: Parsed expenseData is not an array", expenseData);
            expenseData = [];
        }

        // Extract categories as labels
        var chartLabels = expenseData.map(item => item.category || "Unknown");

        if (chartLabels.length === 0) {
            console.warn("No labels found, chart will not be displayed.");
            document.getElementById("expenseChart").style.display = "none";
            document.getElementById("chartMessage").style.display = "block";
            document.getElementById("chartMessage").textContent = "Please add data";
            return;
        }

        var chartValues = expenseData.map(item => item.amount || 0);

        var canvas = document.getElementById("expenseChart");
        var ctx = canvas.getContext("2d");

        if (window.expenseChart && typeof window.expenseChart.destroy === "function") {
            window.expenseChart.destroy();
        }

        window.expenseChart = new Chart(ctx, {
            type: "pie",
            data: {
                labels: chartLabels,
                datasets: [{
                    data: chartValues,
                    backgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56",
                        "#4CAF50",
                        "#9966FF",
                        "#FF9F40"
                    ]
                }]
            }
        });

        //console.log("Chart updated with data:", expenseData);
    } catch (error) {
        console.error("Error updating chart:", error);
    }
}

// Capture the chart as a Base64 image for export
function captureChartForExport() {
    if (window.expenseChart) {
        var chartImageData = window.expenseChart.toBase64Image();
        document.getElementById(hiddenChartImageClientID).value = chartImageData;
    }
    return true;
}

// Update chart and set up event listeners on page load
document.addEventListener("DOMContentLoaded", function () {
    if (!window.chartInitialized) {
        window.chartInitialized = true;
        updateChart();
    }

    // Ensure hiddenExpenseDataClientID is defined before adding event listeners
    var hiddenExpenseInput = document.getElementById(hiddenExpenseDataClientID);
    if (hiddenExpenseInput) {
        // Remove any existing event listener to avoid duplicate calls
        hiddenExpenseInput.removeEventListener("change", updateChart);

        // Add event listener to update the chart when data changes
        hiddenExpenseInput.addEventListener("change", updateChart);
    }

    // Add event listener for the Amount field to prevent negative values.
    var txtAmount = document.getElementById("<%= txtAmount.ClientID %>");
    if (txtAmount) {
        // Set the minimum and step so the browser knows negative values are not allowed.
        txtAmount.setAttribute("min", "0");
        txtAmount.setAttribute("step", "1");

        // Prevent typing a '-' character.
        txtAmount.addEventListener("keydown", function (e) {
            if (e.key === '-' || e.key === 'Subtract' || e.keyCode === 189) {
                e.preventDefault();
            }
            // Prevent the ArrowDown key from decrementing when the value is 0 (or empty).
            if (e.key === "ArrowDown") {
                var currentValue = parseFloat(this.value) || 0;
                if (currentValue <= 0) {
                    e.preventDefault();
                }
            }
        });

        // Function to ensure the value never goes below zero.
        function clampValue() {
            var value = parseFloat(txtAmount.value);
            if (isNaN(value) || value < 0) {
                txtAmount.value = 0;
            }
        }

        // Use a short delay in case the spinner change happens after the event fires.
        var delayClamp = function () {
            setTimeout(clampValue, 100);
        };

        // Listen to various events.
        txtAmount.addEventListener("input", delayClamp);
        txtAmount.addEventListener("change", clampValue);
        txtAmount.addEventListener("mouseup", delayClamp);
        txtAmount.addEventListener("blur", clampValue);
    }
});

// Hide the dropdown if clicking outside of it
document.addEventListener("click", function (event) {
    var dropdown = document.getElementById("settingsDropdown");
    var settingsButton = document.getElementById(btnSettingsClientID);
    if (!settingsButton.contains(event.target) && !dropdown.contains(event.target)) {
        dropdown.style.display = "none";
    }
});
