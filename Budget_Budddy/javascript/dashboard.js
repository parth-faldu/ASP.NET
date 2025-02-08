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
        var hiddenData = document.getElementById(hiddenExpenseDataClientID).value || "{}";
        var expenseData = JSON.parse(hiddenData);
        var chartLabels = Object.keys(expenseData);
        var canvas = document.getElementById("expenseChart");
        var messageEl = document.getElementById("chartMessage");

        if (chartLabels.length === 0) {
            canvas.style.display = "none";
            messageEl.style.display = "block";
            messageEl.textContent = "Please add data";
            return;
        } else {
            canvas.style.display = "block";
            messageEl.style.display = "none";
        }

        var chartValues = chartLabels.map(function (label) {
            return expenseData[label].amount;
        });

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

        console.log("Chart updated with data:", expenseData);
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
    updateChart();

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
