document.getElementById('roleIds').value = getSelectedId();
$('.role').click(function () {
    document.getElementById('roleIds').value = getSelectedId();
})
function getSelectedId() {
    var $selectedRoles = $('.role:checkbox:checked');
    var userid = '';
    for (var i = 0; i < $selectedRoles.length; i++) {
        userid += $($selectedRoles[i]).data('userid') + ',';
    }
    return userid.substr(0, userid.length - 1);
}

// Submit the form
function iframeSubmit() {
    $('#formSetRole').submit();
}

function onSuccess(data) {
    if (data.result) {
        window.parent.$('#divSetRole').dialog('close');
    }
    window.parent.$.messager.alert("Success", data.msg);
}

function onFailure() {
    $.messager.alert("Fail", "Set role failed");
}