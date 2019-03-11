var dataset = {};
function authorization() {
    $(".us").hide();
    $(".us-infoh").show();
}
function unauthorization() {
    $(".us").show();
    $(".us-infoh").hide();
}
$(function () {
    if (is_login()) {
        authorization_data({
            url: api_url + 'users/getinfo',
            callback: (in_data) => {
                var data = JSON.parse(in_data);
                if (data['code'] == 200) {
                    authorization();
                    var info = JSON.parse(data['data']);
                    console.log($("#h-user-name"))
                    $("#h-user-name").text('用户：' + info['username']);
                }
                else {
                    unauthorization();
                }
            }
        });
    } else {
        unauthorization();
    }
})