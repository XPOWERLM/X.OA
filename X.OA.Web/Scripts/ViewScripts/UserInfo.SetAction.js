
$('.interactCheckbox').click(function () {
    var $currentItem = $(this);
    var operateData = $currentItem.data('stats');
    $currentItem.siblings(':checkbox')[0].checked = false;
    $currentItem.siblings(':hidden').val($currentItem.data('stats'));
    if (this.checked == false && $currentItem.siblings(':checkbox')[0].checked == false) {
        $currentItem.siblings(':hidden').val(0);
        operateData = 0;
    }
    submitActionData($('#hiddenForJs').val(),$currentItem.siblings(':hidden')[0].id,operateData);
})

function processData() {
    var values = $('.submitStats');
    var data = '';
    for (var i = 0; i < values.length; i++) {
        data += values[i].id + ':' + values[i].value + ',';
    }
    return data.substr(0, data.length - 1);
}

//$('#formSetAction').submit(function () {
//    document.getElementById('actionData').value = processData();
//})

function submitActionData(userId, actionId, actionOperate) {
    $.post('/UserInfo/SetAction', {
        userId: userId,
        actionId: actionId,
        actionOperate: actionOperate,
    },
    function (data) {
        if (data.result) {
            $.messager.show({
                title:'tip',
                msg:data.msg,
                showType:'show'
            });
        } else {
            $.messager.alert('warn', data.msg);
        }
    })
}
