function Login_Show() {
    var _this = $("#login_form");
    set_align_center(win, _this, page_min_width);
    set_vertical_center(win, _this);

    win.bind("resize", function () {
        set_align_center(win, _this, page_min_width);
        set_vertical_center(win, _this);
    });
    _this.show();
}
function Login_LoginOut() {
    $.cookie('ssid', null);
    location.reload(true);
}
$(function () {

    var loginModel = {
        el: "#login_form",
        data: {
            username: "",
            password: "",
            verify_code: "",
            is_submit: false
        },
        methods: {
            in_focus: function (e) {
                var ethis = $(e.target);
                $("#form_login").val("登录")
                ethis.parent().parent().find(".tip_info").hide();
            },
            in_blur: function (e) {
                var ethis = $(e.target);
                if (ethis.val().length == 0)
                    ethis.parent().parent().find(".tip_info").show();
            },
            verify_username: function () {
                return this.username.length == 0;
            },
            verify_password: function () {
                return this.password.length == 0;
            },
            login_user: function () {
                var verify = 0;
                if (this.is_submit)
                    return;
                this.is_submit = true;
                if (this.verify_username()) {
                    verify++;
                    $("#username").parent().parent().find(".tip_info").show();
                }
                if (this.verify_password()) {
                    verify++;
                    $("#password").parent().parent().find(".tip_info").show();
                }

                if (verify != 0) {
                    this.is_submit = false;
                    return;
                }
                var btn_login = $("#form_login");
                btn_login.val("正在登陆．．．");
                send_data({
                    url: api_url + "users/login",
                    data: { username: this.username, password: this.password },
                    callback: (in_data) => {
                        var data = JSON.parse(in_data);
                        if (data['code'] == 200) {
                            $.cookie("ssid", data['data']);
                            btn_login.val("登录成功");
                            setTimeout(() => {
                                this.close_form();
                                location.reload(true);
                            }, 1000);

                        } else {
                            this.is_submit = false;
                            btn_login.val("用户名或密码错误");
                        }
                    }
                })
            },
            close_form: function () {
                $(this.$el).css({ "display": "none" });
            }
        }
    };
    new Vue(loginModel);
})