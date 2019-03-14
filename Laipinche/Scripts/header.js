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
                var data = in_data['data'];
                if (in_data['code'] == 200) {
                    authorization();

                    $("#h-user-name").text('用户：' + data['name']);
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