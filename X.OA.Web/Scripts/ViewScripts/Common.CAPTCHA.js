/// <reference path="../jquery-1-11-3.js" />

function triggerCAPTCHA(idCAPTCHA, idSender) {
    /// <signature>
    ///   <summary>Bind  CAPTCHA code trigger</summary>
    ///   <param name="idCAPTCHA" type="String">Image CAPTCHA id</param>
    ///   <param name="idSender" type="String">Trigger</param>
    /// </signature>
    $(idSender).click(function () {
        $(idCAPTCHA).attr("src", $(idCAPTCHA).data("src") + Math.random(1, 100));
    });
}

function changeCAPTCHA(idCAPTCHA) {
    /// <signature>
    ///   <summary>Change CAPTCHA code</summary>
    ///   <param name="idCAPTCHA" type="String">Image CAPTCHA id</param>
    /// </signature>
    $(idCAPTCHA).attr("src", $(idCAPTCHA).data("src") + Math.random(1, 100));
}