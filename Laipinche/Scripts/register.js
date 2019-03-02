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

    init_in();
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
    set_vertical_center(page, register);
}
//初始化输入框
function init_in() {

    //输入改变
    function value_change(item) {
        item.bind("input propertychange", function () {
            if (item.val().length == 0)
                item.siblings(".tip").show();
            else item.siblings(".tip").hide();

            //var func = item.siblings(".info-tips-wrap").children();
            //console.log(func);

        })
    }
    //获取焦点
    function in_focus(item) {
        item.focus(function () {
            item.siblings(".info-tips-wrap").slideDown(200);
        });
    }
    //失去焦点
    function in_blur(item) {
        item.blur(function () {
            item.siblings(".info-tips-wrap").slideUp(200);
        });
    }
    value_change($("#username"));

    in_focus($("#username"));
    in_blur($("#username"));
}

//值判断
function value_is_true(item, reg) {
    var val = item.val();
    return reg.test(val);
}
//入口
$(function () {
    init();
    
    //new Vue({
    //    el: "#app",
    //    data: {
    //        username: "",
    //        password: "",
    //        tel: ""
    //    },
    //    watch: {
    //        check_username_length: function (n_v) {
    //            if (/^[\w]{5,12}$/.test(n_v))
    //                return true;
    //            else return false;
    //        }
    //    }
    //});

})