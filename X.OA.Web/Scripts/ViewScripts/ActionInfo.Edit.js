
// Submit the edit form
function submitEdit() {
    document.getElementById('btnSubmitEdit').click();
}

// Edit on success callback
function onSuccess(data) {
    if (data.result) {
        window.parent.$('#dgEditAction').dialog('close');
        window.parent.$('#dgActionInfo').datagrid('reload');
    }
    window.parent.$.messager.alert("Tip", data.msg);
}

// Edit on failure callback
function onFailure() {
    $.messager.alert("Tip", "Edit failed");
}

// MenuIcon upload
function uploadMenuIcon(sender) {
    /// <signature>
    ///   <summary>Upload MenuIcon</summary>
    ///   <param name="sender" type="String">The trigger id, by click,no #</param>
    /// </signature>
    document.getElementById(sender).onclick = function () {
        $('#uploadProgress').attr('value', 0);
        $('#editForm').ajaxUpload({
            url: "/ActionInfo/Upload",
            success: uploadOnSuccess,
            progressCallback: progressCallback,
        });
    }
}
uploadMenuIcon('btnUploadIcon');



// On success call back
function uploadOnSuccess(data) {
    if (data.result) {
        $('#MenuIcon').val(data.msg)
    } else {
        $.messager.alert("Warn", 'Upload failed');
    }
}

// Progress call back
function progressCallback(e) {
    e = window.event || e;
    if (e.lengthComputable) {
        $('#uploadProgress').attr('max', e.total).attr('value', e.loaded);
    }
}

// Select hidden area
function selectOnChange() {
    if (document.getElementById('ActionTypeEnum').value == 1) {
        //$('#btnUploadIcon').attr('value', '上传');
        $('.hiddenArea').css('display', 'block');
    } else {
        $('.hiddenArea').css('display', 'none');
    }
}
$('#ActionTypeEnum').change(function () {
    selectOnChange();
})
document.getElementById('ActionTypeEnum').value = $('#hiddenForJs').data('js');
selectOnChange();

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
