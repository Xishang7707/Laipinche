function init() {
    document.oncontextmenu = () => { return false };
    document.onselectstart = () => { return false };
    $('img').on('dragstart', function (event) { event.preventDefault(); });

    set_align_center(win, $("div.header .wrapper"), page_min_width);
    init_page();
    set_register_center();

    //更改窗口大小
    win.resize(function () {
        set_align_center(win, $("div.header .wrapper"), page_min_width);

        init_page();
        set_register_center();
    });
}
//设置page大小
function init_page() {
    var page = $(".page");
    page.width(win.width());
    page.height(win.height());
}
//设置注册居中
function set_register_center() {
    var page = $(".page");
    var register = page.find(".register");
    set_align_center(page, register);
    //set_vertical_center(page, register);
}

//验证值为空
function val_empty(val) {
    return val.length == 0 ? true : false;
}
/**
 * 表单模型
 * */
var form_model = {
    el: "#form-reg",
    data: {
        username: "",
        password: "",
        name: "",
        idcard: "",
        tel: "",
        telcode: "",
        exist_username: false,
        exist_tel: false,
        exist_idcard: false,
        is_true_telcode: 0,
        is_send_telcode: false,
        is_submit: false
    },
    methods: {
        /**
         * 获取焦点
         * */
        username_focus: function (e) {
            var _this = $(e.target);
            _this.siblings(".error-tips-wrap").slideUp(100);
            _this.siblings(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
            this.exist_username = false;
        },
        username_blur: function (e) {
            var _this = $(e.target);

            _this.siblings(".info-tips-wrap").slideUp(100);
            if (this.verify_username(1) || this.exist_username)
                _this.siblings(".error-tips-wrap").slideDown(100);
            if (this.verify_username()) {
                _this.siblings(".tick").addClass("show");
            }
            if (this.verify_username(1))
                _this.addClass("error");
            this.verify_username(4);
        },
        password_focus: function (e) {
            var _this = $(e.target);
            _this.siblings(".error-tips-wrap").slideUp(100);
            _this.siblings(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
        },
        password_blur: function (e) {
            var _this = $(e.target);

            _this.siblings(".info-tips-wrap").slideUp(100);
            if (this.verify_password(1))
                _this.siblings(".error-tips-wrap").slideDown(100);
            if (this.verify_password()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_password(1))
                _this.addClass("error");
        },
        name_focus: function (e) {
            var _this = $(e.target);
            _this.siblings(".error-tips-wrap").slideUp(100);
            _this.siblings(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
        },
        name_blur: function (e) {
            var _this = $(e.target);

            _this.siblings(".info-tips-wrap").slideUp(100);
            if (this.verify_name(1))
                _this.siblings(".error-tips-wrap").slideDown(100);
            if (this.verify_name()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_name(1))
                _this.addClass("error");
        },
        idcard_focus: function (e) {
            var _this = $(e.target);
            _this.siblings(".error-tips-wrap").slideUp(100);
            _this.siblings(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
            this.exist_idcard = false;
        },
        idcard_blur: function (e) {
            var _this = $(e.target);

            _this.siblings(".info-tips-wrap").slideUp(100);
            if (this.verify_idcard(1))
                _this.siblings(".error-tips-wrap").slideDown(100);
            if (this.verify_idcard()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_idcard(1))
                _this.addClass("error");
            this.verify_idcard(3);
        },
        tel_focus: function (e) {
            var _this = $(e.target);

            _this.parent().parent().find(".error-tips-wrap").slideUp(100);
            _this.parent().parent().find(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
            this.exist_tel = false;
        },
        tel_blur: function (e) {
            var _this = $(e.target);

            _this.parent().parent().find(".info-tips-wrap").slideUp(100);
            if (this.verify_tel(1))
                _this.parent().parent().find(".error-tips-wrap").slideDown(100);
            if (this.verify_tel()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_tel(1))
                _this.addClass("error");
            this.verify_tel(3);
        },
        telcode_focus: function (e) {
            var _this = $(e.target);

            _this.parent().parent().find(".error-tips-wrap").slideUp(100);
            _this.parent().parent().find(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
            this.is_true_telcode = 0;
        },
        telcode_blur: function (e) {
            var _this = $(e.target);

            //_this.parent().parent().find(".info-tips-wrap").slideUp(100);
            if (this.verify_telcode(1))
                _this.parent().parent().find(".error-tips-wrap").slideDown(100);
            if (this.verify_telcode()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_telcode(1))
                _this.addClass("error");
        },
        //验证
        /**
         * 验证用户名
         * @param {number} id
         */
        verify_username: function (id = 0) {

            //验证为空
            var val = this.username;
            var reg = /^([\w]*[a-zA-Z]+[\w]*)$/;
            switch (id) {
                case 1://判断为空
                    return val_empty(val);
                case 2://验证长度5-15
                    reg = /^.{5,15}$/;
                    break;
                case 3://包含字母
                    reg = /^([\w]*[a-zA-Z]+[\w]*)$/;
                    break;
                case 4://是否被注册
                    {
                        if (this.verify_username(2) && this.verify_username(3))
                            send_data({
                                url: api_url + "verify/exists",
                                data: { username: this.username, type: 0 },
                                callback: (in_data) => {
                                    var data = JSON.parse(in_data);

                                    if (data != null && data['code'] == 1)
                                        this.exist_username = true;
                                    else this.exist_username = false;
                                }
                            })
                    }
                    break;
                default:
                    if (reg.test(val) &&
                        /^\w{5,15}$/.test(val) && !this.exist_username)
                        return true;
                    else return false;

            }
            //验证
            return reg.test(val);
        },
        /**
         * 验证密码
         * @param {number} id
         */
        verify_password: function (id = 0) {

            var val = this.password;

            var regs = [
                /^[^ ]*$/,//包含空格
                /^.{6,18}$/,//6-18个字符
                [/[a-zA-Z]+/, /\d+/, /^[^\u4e00-\u9fa5]+$/] //由字母、数字或字符组成
            ];

            switch (id) {
                case 1://判断为空
                    return val_empty(val);
                case 2://不包含空格
                    return regs[0].test(val);
                case 3://6-18个字符
                    return regs[1].test(val);
                case 4://由字母、数字或字符组成
                    return regs[2][0].test(val) && regs[2][1].test(val) && regs[2][2].test(val);
            }

            return regs[0].test(val) && regs[1].test(val) && (regs[2][0].test(val) && regs[2][1].test(val) && regs[2][2].test(val));
        },
        verify_name: function (id = 0) {
            var val = this.name;

            var regs = [
                /^[\u4e00-\u9fa5]{2,4}$/ //2-4个中文
            ];
            switch (id) {
                case 1://判断为空
                    return val_empty(val);
                case 2://2-4个中文
                    return regs[0].test(val);
            }
            return regs[0].test(val);
        },
        verify_idcard: function (id = 0) {
            var val = this.idcard;

            var regs = [
                /^[1-9]\d{5}((19)|(20))\d{2}(0[1-9]|1[0-2])((0[1-9])|[1-2]\d|30|31)\d{3}[0-9Xx]$/ //18位身份证号码
            ];
            switch (id) {
                case 1://判断为空
                    return val_empty(val);
                case 2://2-4个中文
                    return regs[0].test(val);
                case 3:
                    if (this.verify_idcard(2))
                        send_data({
                            url: api_url + "verify/exists",
                            data: { idcard: this.idcard, type: 1 },
                            callback: (in_data) => {
                                data = JSON.parse(in_data);
                                if (data['code'] == 1) {
                                    this.exist_idcard = true;
                                } else {
                                    this.exist_idcard = false;
                                }
                            }
                        });
            }
            return regs[0].test(val) && !this.exist_idcard;
        },
        verify_tel: function (id = 0) {
            var val = this.tel;

            var regs = [
                /1((3\d)|(4[5-9])|(5[0-35-9])|(66)|(7[0-8])|(8\d)|(9[8-9]))\d{8}/ //13为手机号码
            ];

            switch (id) {
                case 1:
                    return val_empty(val);
                case 2:
                    return regs[0].test(val);
                case 3://手机号码被注册
                    if (this.verify_tel(2))
                        send_data({
                            url: api_url + "verify/exists",
                            data: { tel: this.tel, type: 2 },
                            callback: (in_data) => {
                                data = JSON.parse(in_data);
                                if (data['code'] == 1) {
                                    this.exist_tel = true;
                                } else {
                                    this.exist_tel = false;
                                }
                            }
                        });
                    return;
            }

            return regs[0].test(val) && !this.exist_tel;
        },
        verify_telcode: function (id = 0) {
            var val = this.telcode;

            var regs = [
                /\d{5}/ //5位验证码
            ];

            switch (id) {
                case 1:
                    return val_empty(val);
                case 2:
                    return regs[0].test(val);
            }

            return regs[0].test(val);
        },
        /**
         * 发送短信验证码
         * */
        send_telcode: function (e) {
            var _this = $(e.target);

            if (!this.verify_tel()) {
                $(this.$el).find("#tel").focus();
                $(this.$el).find("#tel").blur();
                return;
            }

            //当手机号码没有被注册
            if (this.exist_tel == false) {
                send_data({
                    url: api_url + "communicate/sendtelcode",
                    data: { tel: this.tel },
                    callback: function (in_data) {
                        var data = JSON.parse(in_data);
                        if (data['code'] == 200) {
                            _this.val("发送成功");

                        } else alert(data["status"])
                    }
                });

                if (this.is_send_telcode)
                    return;
                this.is_send_telcode = true;

                var t_send = 60;
                var clock = setInterval(() => {
                    t_send--;
                    if (t_send <= 0) {
                        clearInterval(clock);
                        this.is_send_telcode = false;
                        _this.val("发送短信验证码");
                        return;
                    }
                    _this.val("重新发送" + t_send + "秒");
                }, 1000)
            }
        },
        register: function () {
            if (this.is_submit)
                return;
            this.is_submit = true;
            var flag = 0;
            if (!this.verify_username()) {
                flag--;
                $(this.$el).find("#username").focus();
                $(this.$el).find("#username").blur();
            }

            if (!this.verify_password()) {
                flag--;
                $(this.$el).find("#password").focus();
                $(this.$el).find("#password").blur();
            }

            if (!this.verify_name()) {
                flag--;
                $(this.$el).find("#name").focus();
                $(this.$el).find("#name").blur();
            }

            if (!this.verify_idcard()) {
                flag--;
                $(this.$el).find("#idcard").focus();
                $(this.$el).find("#idcard").blur();
            }
            if (!this.verify_tel()) {
                flag--;
                $(this.$el).find("#tel").focus();
                $(this.$el).find("#tel").blur();
            }
            if (!this.verify_telcode()) {
                flag--;
                $(this.$el).find("#telcode").focus();
                $(this.$el).find("#telcode").blur();
            }
            if (flag != 0) {
                this.is_submit = false;
                return;
            }
            this.is_submit = true;
            var btn_register = $("#register");
            btn_register.val("请稍等．．．");
            send_data({
                url: api_url + "users/register",
                data: { username: this.username, password: this.password, name: this.name, idcard: this.idcard, tel: this.tel, telcode: this.telcode },
                type: "POST",
                callback: (in_data) => {
                    var data = JSON.parse(in_data);
                    if (data['code'] == 200) {
                        btn_register.val("注册成功");
                        setTimeout(function () {
                            location.reload();
                        }, 2000);
                    }
                    else {
                        if (data['code'] == 10004)
                            this.is_true_telcode = 1;
                        else if (data['code'] == 10005)
                            this.is_true_telcode = 2;
                        else btn_register.val("注册失败,请检查信息");
                        this.is_submit = false;
                    }
                }
            })
        }
    },
    //监听
    watch: {
        exist_username: function (n_v) {

            var _this = $(this.$el);
            if (n_v == true)
                _this.find("#username").siblings(".error-tips-wrap").slideDown(100);
            else _this.find("#username").siblings(".error-tips-wrap").slideUp(100);
        },
        exist_tel: function (n_v) {

            var _this = $(this.$el);

            if (n_v == true)
                _this.find("#tel").parent().parent().find(".error-tips-wrap").slideDown(100);
            else _this.find("#tel").parent().parent().find(".error-tips-wrap").slideUp(100);
        },
        exist_idcard: function (n_v) {
            var _this = $(this.$el);

            if (n_v == true)
                _this.find("#idcard").siblings(".error-tips-wrap").slideDown(100);
            else _this.find("#idcard").siblings(".error-tips-wrap").slideUp(100);
        },
        is_true_telcode: function (n_v) {
            var _this = $(this.$el);

            if (n_v != 0)
                _this.find("#telcode").parent().siblings(".error-tips-wrap").slideDown(100);
            else _this.find("#telcode").parent().siblings(".error-tips-wrap").slideUp(100);
        }
    }
};

$(function () {
    init();
    new Vue(form_model);
})