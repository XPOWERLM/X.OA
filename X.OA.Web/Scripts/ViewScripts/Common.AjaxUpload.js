/// <reference path="../jquery-1.12.4.js" />

$.fn.ajaxUpload = function (config) {
    /// <signature>
    ///   <summary>Upload file using ajax</summary>
    ///   <param name="config" type="Json">The only parameter accept, rest of them are detail</param>
    ///   <param name="config.url" type="String">The url</param>
    ///   <param name="config.success" type="Function">On success call back</param>
    ///   <param name="config.progressCallback" type="Function">Progress call back</param>
    /// </signature>
    var formData = new FormData($(this)[0]);
    $(config.progressBar).attr('value', 0);
    $.ajax({
        url: config.url,
        type: 'POST',
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', config.progressCallback, false);
                return myXhr;
            }
        },
        success: config.success,
        data: formData,
        cache: false,
        contentType: false,
        processData: false
    });
};