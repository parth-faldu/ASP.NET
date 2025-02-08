document.addEventListener("DOMContentLoaded", function () {
    // Use the global variables defined in the ASPX page
    var rbUser = document.getElementById(rbUserID);
    var rbAdmin = document.getElementById(rbAdminID);

    // Make sure both elements were found
    if (!rbUser || !rbAdmin) {
        console.error("Radio button elements not found. Check the IDs.");
        return;
    }

    var userLabel = document.querySelector("label[for='" + rbUser.id + "']");
    var adminLabel = document.querySelector("label[for='" + rbAdmin.id + "']");

    function updateToggle() {
        if (rbUser.checked) {
            userLabel.classList.add("active");
            adminLabel.classList.remove("active");
        } else if (rbAdmin.checked) {
            adminLabel.classList.add("active");
            userLabel.classList.remove("active");
        }
    }

    // Initialize and listen for changes
    updateToggle();
    rbUser.addEventListener("change", updateToggle);
    rbAdmin.addEventListener("change", updateToggle);
});
