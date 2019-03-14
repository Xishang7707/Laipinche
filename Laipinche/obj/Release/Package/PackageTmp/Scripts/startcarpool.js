$(function () {
    document.oncontextmenu = () => { return false };
    document.onselectstart = () => { return false };
    $('img').on('dragstart', function (event) { event.preventDefault(); });
    //$('.page').width(win.width());
    $('.page').height(win.height());

    set_align_center(win, $("div.header .wrapper"), page_min_width);
    set_align_center(win, $("div.page .content-wrap"), page_min_width);
    set_vertical_center(win, $("div.page .content-wrap"));
    set_align_center($(".mode-1-page-wrap"), $(".mode-1-pages"));

    //var date = new Date();
    //var month = date.getMonth() + 1;
    //month = month < 10 ? '0' + month : month;
    //$("#mode1-date").val(date.getFullYear() + '/' + month + '/' + date.getDate());


    win.bind("resize", function () {
        set_align_center(win, $("div.header .wrapper"), page_min_width);
        set_align_center(win, $("div.page .content-wrap"), page_min_width);
        set_vertical_center(win, $("div.page .content-wrap"));
        //$('.page').width(win.width());
        $('.page').height(win.height());

    });
    //--长途拼车
    var mode_vue1 = new Vue({
        el: "#mode-1",
        data: {
            map_1: null,
            carpool_list: []
        },
        methods: {
            init_map: function () {
                this.map_1 = new BMap.Map("mode-1-map");

                var point = new BMap.Point(116.404, 39.915);  // 创建点坐标  
                this.map_1.centerAndZoom(point, 15);                 // 初始化地图，设置中心点坐标和地图级别  

                for (var i = 1; i < 107; i++)
                    this.carpool_list.push(JSON.parse(JSON.stringify({ type: i, starttime: 0, price: 0, cartype: 0, startcity: "长沙", endcity: "永州", option: 0 })));
            }
        }
    })
    mode_vue1.init_map();
})