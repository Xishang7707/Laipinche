var win = $(window)
var bll_url = 'http://localhost:50843/'
var page_min_width = 1200;

//初始化
function init() {
    document.oncontextmenu = () => { return false };
    document.onselectstart = () => { return false };
    $('img').on('dragstart', function (event) { event.preventDefault(); });

    set_align_center(win, $("div.header .wrapper"), page_min_width);
    init_caroucel();
    start_caroucel();

    $(".page").height(win.height());

    init_advantage();
    init_intro();

    //更改窗口大小
    win.resize(function (e) {
        set_align_center(win, $("div.header .wrapper"), page_min_width);
        init_caroucel();
        $(".page").height(win.height());

        init_advantage();
        init_intro();
    })

    //鼠标滚轮
    mousewheel_flag = true;
    window.onmousewheel = function (e) {
        if (!mousewheel_flag) {
            return false;
        }
        mousewheel_flag = false;
        setTimeout(() => {
            mousewheel_flag = true;
        }, 700);

        var page = $(".page");
        var page_count = page.length;
        var scroll_y = win.scrollTop();
        //上
        if (e.wheelDelta > 0) {
            //win.scrollTop(scroll_y - page.height());
            $("html,body").stop().animate({ "scrollTop": scroll_y - page.height() }, 700);
        }
        //下
        else if (e.wheelDelta < 0) {
            //win.scrollTop(scroll_y + page.height());
            $("html,body").stop().animate({ "scrollTop": scroll_y + page.height() }, 700);
        }

        return false
    }
}
//初始化轮播
function init_caroucel() {
    var caroucel = $(".page .caroucel");
    var wrapper = caroucel.children(".wrapper");

    wrapper.find("img").width(win.width() > page_min_width ? win.width() : page_min_width);
    wrapper.find("img").height(win.height());

    wrapper.width(wrapper.find("img").width());
    wrapper.height(wrapper.find("img").height());
}
//开始轮播
function start_caroucel() {
    var caroucel = $(".page .caroucel");
    var wrapper = caroucel.children(".wrapper");
    var caroucel_index = 0;

    setInterval(function () {
        $(wrapper.children()[caroucel_index]).animate({ "opacity": 0 }, 300);
        caroucel_index++;
        if (caroucel_index >= wrapper.children().length)
            caroucel_index = 0;
        $(wrapper.children()[caroucel_index]).animate({ "opacity": 1 }, 300);
    }, 5000);
}
//初始化优势
function init_advantage() {
    var page = $(".page");
    var advantage = page.find(".advantage");
    var item = advantage.children(".item");
    var page_width = page.width();
    var page_height = page.height();

    var scale_val = Math.min(page_width, page_height);

    item_size = (scale_val > page_min_width ? scale_val : page_min_width) * 0.3;
    item.width(item_size);
    item.height(item_size);

    advantage.width((item_size + 40) * 2);


    //设置居中
    set_align_center(page, advantage);
    set_vertical_center(page, advantage);
}
//初始化简介
function init_intro() {
    var page = $(".page");
    var intro = page.find(".intro");
    var item = intro.children(".item");
    var item_width = page.width() * 0.5 - 14;

    item.width(item_width);

    //设置居中
    set_vertical_center(page, intro);
}
$(function () {
    init();
})