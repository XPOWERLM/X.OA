/// <reference path="../jquery-1-11-3.js" />

function changeCAPTCHA(idCAPTCHA, idSender) {
    /// <signature>
    ///   <summary>Change CAPTCHA code</summary>
    ///   <param name="idCAPTCHA" type="String">Image CAPTCHA id</param>
    ///   <param name="idSender" type="String">Trigger</param>
    /// </signature>
    $(idSender).click(function () {
        $(idCAPTCHA).attr("src", $(idCAPTCHA).data("src") + Math.random(1, 100));
    })
}

changeCAPTCHA('#imgCAPTCHA', '#imgCAPTCHA');

function onSuccess(data) {
    if (data.result) {
        $('#callbackMsg').text(data.msg);
        window.location.href = "/SignIn";
    } else {
        $('#callbackMsg').text(data.msg);
        $('#imgCAPTCHA').attr("src", $('#imgCAPTCHA').data("src") + Math.random(1, 100));
    }
}

function onFailure() {
    $('#imgCAPTCHA').attr("src", $('#imgCAPTCHA').data("src") + Math.random(1, 100));
}