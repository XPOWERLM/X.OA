/// <reference path="Common.CAPTCHA.js" />

triggerCAPTCHA("#imgCAPTCHA", "#imgCAPTCHA")

function onSuccess(data) {
    if (data.Result) {
        $("#errorMsg").text(data.Msg);
        window.location.href = "/Password/Verify";
    } else {
        $("#errorMsg").text(data.Msg);
    }
    changeCAPTCHA("#imgCAPTCHA");
}

function onFailure() {
    changeCAPTCHA("#imgCAPTCHA");
}

$('#c').submit(function () {
    var notEqual = $('#resetPassword').val() != $('#resetConfirm').val();
    notEqual = $('#resetPassword').val() == null ? true : false;
    if (notEqual) {
        $("#errorMsg").text("两次密码输入不一致");
        return false;
    }
})
