var win = $(window)
var bll_url = 'http://localhost:50843/'
var page_min_width = 720;

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
        telcode: ""
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
        },
        username_blur: function (e) {
            var _this = $(e.target);

            _this.siblings(".info-tips-wrap").slideUp(100);
            if (this.verify_username(1))
                _this.siblings(".error-tips-wrap").slideDown(100);
            if (this.verify_username()) {
                _this.siblings(".tick").addClass("show");
            }
            else if (this.verify_username(1))
                _this.addClass("error");
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
        }, idcard_focus: function (e) {
            var _this = $(e.target);
            _this.siblings(".error-tips-wrap").slideUp(100);
            _this.siblings(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
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
        }, tel_focus: function (e) {
            var _this = $(e.target);

            _this.parent().parent().find(".error-tips-wrap").slideUp(100);
            _this.parent().parent().find(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
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
        }, telcode_focus: function (e) {
            var _this = $(e.target);

            _this.parent().parent().find(".error-tips-wrap").slideUp(100);
            _this.parent().parent().find(".info-tips-wrap").slideDown(100);
            _this.siblings(".tick").removeClass("show");
            _this.removeClass("error");
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
                default:
                    if (reg.test(val) &&
                        /^\w{5,15}$/.test(val))
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
                /[1-9]\d{5}[19|20]\d{2}((0)[1-9]|(1[0-2]))((0[1-9])|([1|2]\d)|(3[0-1]))\d{3}[\dXx]/ //15或18为身份证号码
            ];
            switch (id) {
                case 1://判断为空
                    return val_empty(val);
                case 2://2-4个中文
                    return regs[0].test(val);
            }
            return regs[0].test(val);
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
            }

            return regs[0].test(val);
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
        }
    },
    //监听
    watch: {

    }
};

//入口
$(function () {
    init();
    new Vue(form_model);
})