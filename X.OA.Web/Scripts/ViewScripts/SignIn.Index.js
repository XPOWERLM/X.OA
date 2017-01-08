/// <reference path="../jquery-1-11-3.js" />

function changeCAPTCHA(idCAPTCHA, idSender) {
    /// <signature>
    ///   <summary>Change CAPTCHA code</summary>
    ///   <param name="idCAPTCHA" type="String">Image CAPTCHA id</param>
    ///   <param name="idSender" type="String">Trigger</param>
    /// </signature>
    $(idSender).click(function () {
        $(idCAPTCHA).attr("src", $(idCAPTCHA).data("src") + Math.random(1, 100));
    });
}

function signInCallback(data) {
    //var serverData = data.split(':');
    //if (serverData[0] == "ok") {
    //    window.location.href = "/Home/Index"
    //} else if (serverData[0] == "no") {
    //    $("#errorMsg").text(serverData[1]);
    //    changeCheckCode();
    //} else {
    //    window.location.href = "/Error.html"
    //}

    $('#errorMsg').text(data.msg);
    if (data.result) {
        window.location.href = "/Home";
    } else {
        // Change CAPTCHA after user submit the form
        $('#imgCAPTCHA').attr("src", $('#imgCAPTCHA').data("src") + Math.random(1, 100));
    }
    //alert(data.msg + ": " + data.result);
}

function FailureCallback() {
    $('#imgCAPTCHA').attr("src", $('#imgCAPTCHA').data("src") + Math.random(1, 100));
}

changeCAPTCHA('#imgCAPTCHA', '#imgCAPTCHA');

$('#btnLogin').click(function () {
    $('#errorMsg').text('登录中....');
});

