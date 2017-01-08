
// Add
$('#tbBtnAdd').click(function () {
    $('#dgAddActionForm').css('display', 'block');
    $('#dgAddActionForm input').val('');
    $('#dgAddAction').dialog({
        title: "添加权限",
        modal: true,
        collapsible: true,
        width: 600,
        height: 400,
        buttons: [{
            text: "OK",
            iconCls: 'icon-ok',
            handler: function () {
                // Before submit . validate the data
                $('#dgAddActionForm').submit();
            },
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgAddAction').dialog('close');
            }
        }]
    });

})

// Edit
$("#tbBtnEdit").click(function () {

    // Get selected rows
    var rows = $('#dgActionInfo').datagrid("getSelections");
    if (rows.length != 1) {
        $.messager.alert('Error', 'Please at least select one item', 'error');
        return;
    }

    // Initital UI
    document.getElementById('iframeEdit').src = "/ActionInfo/Edit?id=" + rows[0].ID;
    $('#dgEditAction').css('display', 'block');
    //$('#dgEditActionForm input').val('');

    // Set data
    //$('#editUName').val(rows[0].ActionName);
    //$('#editUPwd').val(rows[0].ModifiedOn);
    //$('#editRemark').val(rows[0].Remark);
    //$('#editID').val(rows[0].ID);


    // dialog
    $('#dgEditAction').dialog({
        title: "编辑用户",
        modal: true,
        collapsible: true,
        width: 1000,
        height: 600,
        resizable: true,
        buttons: [{
            text: "OK",
            iconCls: 'icon-ok',
            handler: function () {
                // Before submit . validate the data
                $('#dgEditActionForm').submit();
                document.getElementById('iframeEdit').contentWindow.submitEdit();
                // Clear selections
                $('#dgActionInfo').datagrid('clearSelections');
            },
        },
        {
            text: 'Cancel',
            handler: function () {
                $('#dgEditAction').dialog('close');
            }
        }]
    })
})

// Delete
$("#tbBtnDel").click(function () {
    var rows = $('#dgActionInfo').datagrid("getSelections");
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
                $.post("/ActionInfo/DeleteAction", { userIDs: userIDs }, function (data) {
                    if (data.result) {
                        $('#dgActionInfo').datagrid('reload').datagrid('clearSelections');
                    }
                    $.messager.alert("Info", data.msg, "icon-tip");
                }, "json");
            }
        });
    }
})

// Search
$('#btnSearch').click(function () {
    var searchParams = $('#searchForm').serialize();
    $('#dgActionInfo').datagrid('load', {
        searchName: $('#searchName').val(),
        searchRemark: $('#searchRemark').val()
    });
});

// Set role
$('#tbBtnSetRole').click(function () {
    //document.getElementById('divSetRole').style.display = 'block';
    //iframeSetRole
    var rows = $('#dgActionInfo').datagrid('getSelections');
    if (rows.length != 1) {
        $.messager.alert('Tip', "Please only select one item");
        return;
    } else {
        document.getElementById('iframeSetRole').src = "/ActionInfo/SetRole?id=" + rows[0].ID;
    }
    $('#divSetRole').dialog({
        title: '设置角色',
        modal: false,
        collapsible: true,
        width: 1000,
        height: 600,
        resizable: true,
        buttons: [{
            text: 'OK',
            iconCls: 'icon-ok',
            handler: function () {
                document.getElementById('iframeSetRole').contentWindow.iframeSubmit();
            }
        },
        {
            text: 'OK',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#divSetRole').dialog('close');
            }
        }]
    })
    $('#dgActionInfo').datagrid('clearSelections');
})

// Add onSuccess
function addCallback(data) {
    /// <signature>
    ///   <summary>The callback of AddAction form , close the add role dialog and reaload the data</summary>
    /// </signature>
    if (data.result) {
        $('#dgAddAction').dialog('close');
        $('#dgActionInfo').datagrid('reload');
    }
    $.messager.alert('info', data.msg);
}

// Edit onSuccess
function editCallback(data) {
    /// <signature>
    ///   <summary>The callback of Edituser form , close the edit user dialog and reaload the data</summary>
    /// </signature>
    $.messager.alert('info', data.msg);
    $('#dgEditActionForm').dialog('close');
    $('#dgActionInfo').datagrid('reload');
}

// UI Initial
$('#dgActionInfo').datagrid({
    columns: [[
        { field: 'ID', title: '编号' },
        { field: 'ActionInfoName', title: '权限名称' },
        { field: 'Sort', title: '排序' },
        { field: 'Remark', title: '备注' },
        { field: 'Url', title: '请求地址' },
        { field: 'HttpMethod', title: '请求方式' },
        {
            field: 'ActionTypeEnum', title: '权限类型', formatter: function (value, row, index) {
                return value == 1 ? "菜单权限" : "普通权限";
            }
        }
    ]]
})

// Select hidden area
$('#ActionTypeEnum').change(function () {
    if (document.getElementById('ActionTypeEnum').value == 1) {
        $('#btnUploadIcon').attr('value', '上传');
        $('.hiddenArea').css('display', 'block');
    } else {
        $('.hiddenArea').css('display', 'none');
    }
})

// MenuIcon upload
$('#btnUploadIcon').click(function () {
    var formData = new FormData(document.getElementById('dgAddActionForm'));
    $('#uploadProgress').attr('value', 0);
    $.ajax({
        url: '/ActionInfo/Upload',
        type: 'POST',
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', progressCallback, false);
                return myXhr;
            }
        },
        success: uploadOnSuccess,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
    });
})

// Upload progress callback
function progressCallback(e) {
    if (e.lengthComputable) {
        $('#uploadProgress').attr('max', e.total).attr('value', e.loaded);
    }
}

// Upload on success
function uploadOnSuccess(data) {
    if (data.result) {
        $('#MenuIcon').val(data.msg);
    }
    else {

    }
    $.messager.alert('Tip', data.msg);
}

// Upload picture preview
document.getElementById('fileMenuIcon').onchange = function () {
    uploadPicPreview(event);
}
function uploadPicPreview(event) {
    var compatibleEvent = window.event || event;
    var input = compatibleEvent.target || compatibleEvent.srcElement;
    var output = document.getElementById('uploadPic');
    var reader = new FileReader();

    reader.onload = function () {
        var dataUrl = reader.result;
        var imgMeter = new Image();
        imgMeter.src = dataUrl;

        // Set value for hidden area
        document.getElementById('uploadIconWidth').value = imgMeter.width;
        document.getElementById('uploadIconHeight').value = imgMeter.height;

        // Set src for preview image
        output.src = dataUrl;
    }
    reader.readAsDataURL(input.files[0]);
}