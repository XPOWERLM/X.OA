
//getSelectedId();
document.getElementById('hiddenId').value = getSelectedId();
$('.roles').click(function () {
    document.getElementById('hiddenId').value = getSelectedId();
})
function getSelectedId() {
    var $selectedRole = $(".roles:checkbox:checked");
    var userIds = '';
    for (var i = 0; i < $selectedRole.length; i++) {
        userIds += $($selectedRole[i]).data('userid') + ',';
    }
    return userIds.substr(0, userIds.length - 1);
}

function iframeSubmit() {
    $('#formSetRole').submit();
}

function onSuccess(data) {
    if (data.result) {
        window.parent.$('#divRole').dialog('close');
    }
    window.parent.$.messager.alert("Success", data.msg);
}

function onFailure() {
    $.messager.alert("Fail", "Set role failed");
}
