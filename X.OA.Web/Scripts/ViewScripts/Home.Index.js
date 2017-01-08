/// <reference path="../jquery-1-11-3.js" />


function CreateIframe(sender, target) {
    /// <signature>
    ///   <summary>CreateIframe</summary>
    ///   <param name="sender" type="String">Trigger</param>
    ///   <param name="target" type="String">Target tab</param>
    /// </signature>

    function CreateContent(url) {
        return '<iframe class="iframeItem" src="' + url + '" frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>';
    }

    $(sender).click(function () {
        var title = $(this).text();
        var isExists = $(target).tabs('exists', title);
        if (isExists) {
            $(target).tabs('select', title);
        } else {
            $(target).tabs('add', {
                title: title,
                content: CreateContent($(this).data('url')),
                closable: true
            });
        }
    });
}


CreateIframe('.a-detail', '#tabs-detail');


// Load menu
$.post('/UserInfo/GetMenu', {}, function (data) {
    for (var i = 0; i < data.length; i++) {
        var htmlContent = '<a href="javascript:" ';
        htmlContent += 'data-url="' + data[i].Url + '" class="a-detail">'
        + data[i].ActionInfoName + '</a> <br />';

        $('#menuContainer').append(htmlContent);
    }
    CreateIframe('.a-detail', '#tabs-detail');
})