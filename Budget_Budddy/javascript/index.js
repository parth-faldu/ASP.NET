function updateToggle() {
    var rbUser = document.getElementById(rbUserID);
    var rbAdmin = document.getElementById(rbAdminID);
    var labelUser = document.querySelector('label[for="' + rbUser.id + '"]');
    var labelAdmin = document.querySelector('label[for="' + rbAdmin.id + '"]');
    if (rbUser.checked) {
        labelUser.classList.add('active');
        labelAdmin.classList.remove('active');
    } else {
        labelAdmin.classList.add('active');
        labelUser.classList.remove('active');
    }
}

window.onload = function () {
    updateToggle();
    document.getElementById(rbUserID).onchange = updateToggle;
    document.getElementById(rbAdminID).onchange = updateToggle;
};
