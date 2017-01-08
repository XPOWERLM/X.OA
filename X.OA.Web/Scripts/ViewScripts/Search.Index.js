/// <reference path="../jquery-ui-1-12-1.js" />

document.getElementById('btnSearch').onclick = function () {
    search();
};

function search() {
    var keyWord = document.getElementById('keyWord').value;
    var xhr = new XMLHttpRequest() || new ActiveXObject('Microsoft.XMLHTTP');
    xhr.open('POST', '/Search/Search', true);
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    xhr.send("keyWord=" + keyWord);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                document.getElementById('divResult').children[0].children[1].innerHTML = createTable(xhr.responseText, keyWord);
            }
        }
    };
}

function createTable(data, keyWord) {
    data = JSON.parse(data);
    var htmlContent = '';
    for (var i = 0; i < data.content.length; i++) {
        // Security risk !!! Make sure the data is secure.
        htmlContent += '<tr>\
            <td>' + data.content[i].Id + '</td>\
            <td>' + data.content[i].Title + '</td>\
            <td>' + data.content[i].ContentDescription + '</td>\
            </tr>';
    }

    // Keyword highlight
    for (var i = 0; i < data.keyWords.length; i++) {
        var regex = new RegExp(data.keyWords[i], 'gi'); // g: global, i: ignore case
        htmlContent = htmlContent.replace(regex, '<em>' + '$&' + '</em>');  // $&: 与 regexp 相匹配的字符串。
    }
    return htmlContent;
}

function searchAutoComplete(sender) {
    $(sender).autocomplete({
        source: '/Search/SearchTip'
    });
}

searchAutoComplete('#keyWord');