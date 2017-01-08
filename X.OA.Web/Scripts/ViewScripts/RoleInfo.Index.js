
$('#tbBtnAdd').click(function () {
    $('#dgAddRoleForm').css('display', 'block');
    $('#dgAddRoleForm input').val('');
    $('#dgAddRole').dialog({
        title: "添加角色",
        modal: true,
        collapsible: true,
        width: 300,
        height: 200,
        buttons: [{
            text: "OK",
            iconCls: 'icon-ok',
            handler: function () {
                // Before submit . validate the data
                $('#dgAddRoleForm').submit();
            }
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgAddRole').dialog('close');
            }
        }]
    });

});

$("#tbBtnEdit").click(function () {

    // Get selected rows
    var rows = $('#dgRoleInfo').datagrid("getSelections");
    if (rows.length !== 1) {
        $.messager.alert('Error', 'Please at least select one item', 'error');
        return;
    }

    // Initital UI
    $('#dgEditRoleForm').css('display', 'block');
    //$('#dgEditRoleForm input').val('');

    // Set data
    $('#editUName').val(rows[0].RoleName);
    $('#editUPwd').val(rows[0].ModifiedOn);
    $('#editRemark').val(rows[0].Remark);
    $('#editID').val(rows[0].ID);


    // dialog
    $('#dgEditRoleForm').dialog({
        title: "编辑用户",
        modal: true,
        collapsible: true,
        width: 300,
        height: 200,
        buttons: [{
            text: "OK",
            iconCls: 'icon-ok',
            handler: function () {
                // Before submit . validate the data
                $('#dgEditRoleForm').submit();
                // Clear selections
                $('#dgRoleInfo').datagrid('clearSelections');
            }
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgEditRoleForm').dialog('close');
            }
        }]
    });
});

$("#tbBtnDel").click(function () {
    var rows = $('#dgRoleInfo').datagrid("getSelections");
    // No select.
    if (!rows || rows.length === 0) {
        $.messager.alert("Tip", "Nothing selected", "error");
    }
        // Selected.
    else {
        console.log(rows);
        $.messager.confirm('提示', '确定要删除吗？', function (res) {
            if (res) {
                var userIDs = '';
                for (var i = 0; i < rows.length; i++) {
                    userIDs += rows[i].ID + ',';
                    console.log("Rows Length" + rows.length);
                }
                userIDs = userIDs.substr(0, userIDs.length - 1);
                // Ajax post data
                $.post("/RoleInfo/DeleteRole", { userIDs: userIDs }, function (data) {
                    if (data.result) {
                        $('#dgRoleInfo').datagrid('reload').datagrid('clearSelections');
                    }
                    $.messager.alert("Info", data.msg, "icon-tip");
                }, "json");
            }
        });
    }
});

$('#btnSearch').click(function () {
    var searchParams = $('#searchForm').serialize();
    $('#dgRoleInfo').datagrid('load', {
        searchName: $('#searchName').val(),
        searchRemark: $('#searchRemark').val()
    });
});

function addCallback(data) {
    /// <signature>
    ///   <summary>The callback of AddRole form , close the add role dialog and reaload the data</summary>
    /// </signature>
    $.messager.alert('info', data.msg);
    $('#dgAddRole').dialog('close');
    $('#dgRoleInfo').datagrid('reload');
}

function editCallback(data) {
    /// <signature>
    ///   <summary>The callback of Edituser form , close the edit user dialog and reaload the data</summary>
    /// </signature>
    $.messager.alert('info', data.msg);
    $('#dgEditRoleForm').dialog('close');
    $('#dgRoleInfo').datagrid('reload');
}
