// Add
$('#tbBtnAdd').click(function () {
    $('#dgAddUserForm').css('display', 'block');
    $('#dgAddUserForm input').val('');
    $('#dgAddUser').dialog({
        title: "添加用户",
        modal: true,
        collapsible: true,
        width: 300,
        height: 200,
        buttons: [{
            text: "OK",
            iconCls: 'icon-ok',
            handler: function () {
                // Before submit . validate the data
                $('#dgAddUserForm').submit();
            },
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgAddUser').dialog('close');
            }
        }]
    });

})

// Edit
$("#tbBtnEdit").click(function () {

    // Get selected rows
    var rows = $('#dgUserInfo').datagrid("getSelections");
    if (rows.length != 1) {
        $.messager.alert('Error', 'Please at least select one item', 'error');
        return;
    }

    // Initital UI
    $('#dgEditUserForm').css('display', 'block');
    //$('#dgEditUserForm input').val('');

    // Set data
    $('#editUName').val(rows[0].UName);
    $('#editUPwd').val(rows[0].UPwd);
    $('#editRemark').val(rows[0].Remark);
    $('#editID').val(rows[0].ID);


    // dialog
    $('#dgEditUserForm').dialog({
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
                $('#dgEditUserForm').submit();
            },
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgEditUserForm').dialog('close');
            }
        }]
    })

})

// Delete
$("#tbBtnDel").click(function () {
    var rows = $('#dgUserInfo').datagrid("getSelections");
    // No select.
    if (!rows || rows.length == 0) {
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
                $.post("/UserInfo/DeleteUser", { userIDs: userIDs }, function (data) {
                    if (data.result) {
                        $('#dgUserInfo').datagrid('reload').datagrid('clearSelections');
                    }
                    $.messager.alert("Info", data.msg, "icon-tip");
                }, "json");
            }
        });
    }
})

// Set role
$('#tbBtnRole').click(function () {

    // Get selected rows
    var rows = $('#dgUserInfo').datagrid("getSelections");
    if (rows.length != 1) {
        $.messager.alert('Error', 'Please at least select one item', 'error');
        return;
    }

    $('#iframeRole').attr('src', '/UserInfo/SetRole?id=' + rows[0].ID)
    $('#divRole').css('display', 'block');

    $('#divRole').dialog({
        title: "设置角色",
        modal: true,
        collapsible: true,
        width: 300,
        height: 200,
        buttons: [{
            text: "OK",
            iconCls: "icon-ok",
            handler: function () {
                document.getElementById('iframeRole').contentWindow.iframeSubmit();
            },
        },
        {
            text: "Cancel",
            handler: function () {
                $('#divRole').dialog('close');
            }
        }]

    })

    $('#dgUserInfo').datagrid('clearSelections');
})

// Set action
$('#tbBtnAction').click(function () {
    // Get selected rows
    var rows = $('#dgUserInfo').datagrid("getSelections");
    if (rows.length != 1) {
        $.messager.alert('Error', 'Please at least select one item', 'error');
        return;
    }

    // Set iframe src
    document.getElementById('iframeAction').src = '/UserInfo/SetAction?userId=' + rows[0].ID;
    //document.getElementById('divAction').style.display = 'block';

    $('#divAction').dialog({
        title: "设置权限",
        modal: true,
        collapsible: true,
        width: 800,
        height: 600,
        buttons: [{
            text: "OK",
            iconCls: "icon-ok",
            handler: function () {
                //document.getElementById('iframeAction').contentWindow.iframeSubmit();
                $('#divAction').dialog('close');
            },
        },
        {
            text: "Cancel",
            handler: function () {
                $('#divAction').dialog('close');
            }
        }]

    })
})

// Search
$('#btnSearch').click(function () {
    var searchParams = $('#searchForm').serialize();
    $('#dgUserInfo').datagrid('load', {
        searchName: $('#searchName').val(),
        searchRemark: $('#searchRemark').val()
    });
});

function addCallback(data) {
    /// <signature>
    ///   <summary>The callback of Adduser form , close the add user dialog and reaload the data</summary>
    /// </signature>
    $.messager.alert('info', data.msg);
    $('#dgAddUser').dialog('close');
    $('#dgUserInfo').datagrid('reload');
}

function editCallback(data) {
    /// <signature>
    ///   <summary>The callback of Edituser form , close the edit user dialog and reaload the data</summary>
    /// </signature>
    $.messager.alert('info', data.msg);
    $('#dgEditUserForm').dialog('close');
    $('#dgUserInfo').datagrid('reload');
}

//function isOnlyOneSelected(datagrid,rows) {
//    /// <signature>
//    /// <summary>Ensure only one row selected</summary>
//    /// <param name='datagrid' type='String'>The datagrid id,with #</param>
//    /// <param name='rows' type='Variable'>The rows</param>
//    /// <returns type='Boolean'></returns>
//    /// </signature>
//    rows = $(datagrid).datagrid('getSelections');
//    if (rows.length != 1) {
//        $.messager.alert('Error', 'Please only select ont item', 'error');
//        return false;
//    }
//    return true;
//}