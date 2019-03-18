
$(function () {
    document.oncontextmenu = () => { return false };
    document.onselectstart = () => { return false };
    $('img').on('dragstart', function (event) { event.preventDefault(); });
    //$('.page').width(win.width());
    //$('.page').height(win.height());

    //set_align_center(win, $("div.header .wrapper"), page_min_width);
    //set_align_center(win, $("div.page .content-wrap"), page_min_width);
    //set_vertical_center(win, $("div.page .content-wrap"));

    //var date = new Date();
    //var month = date.getMonth() + 1;
    //month = month < 10 ? '0' + month : month;
    //$("#mode1-date").val(date.getFullYear() + '/' + month + '/' + date.getDate());


    win.bind("resize", function () {
        //set_align_center(win, $("div.header .wrapper"), page_min_width);
        //set_align_center(win, $("div.page .content-wrap"), page_min_width);
        //set_vertical_center(win, $("div.page .content-wrap"));
        //$('.page').width(win.width());
        //$('.page').height(win.height());

    });
    //--长途拼车
    var mode_vue1 = new Vue({
        el: "#mode-1",
        data: {
            orderdetailsid: +new Date(),
            mode2id: +new Date(),
            map_1: null,
            carpool_list: [],
            curpage: 1,
            pagecount: 1,
            datacount: 12,
            //sumcount: 0,
            order_details: "",
            orderstate: ['正在进行', '已经开始', '完成', '关闭', '申请中'],
            paytype: ['免费', '面议', '一口价'],
            cartype: ['轿车', 'MPV', 'SUV', '跑车', '客车', '其他'],
            cursearchapi: '',
            searchdata: {},
            searchcallback: null,
            workinglist: {}
            //mode_2_from: '',
            //mode_2_in: ''
        },
        methods: {
            init_map: function (id) {


                this.map_1 = new BMap.Map(id);

                var point = new BMap.Point(116.404, 39.915);  // 创建点坐标  
                this.map_1.centerAndZoom(point, 15);                 // 初始化地图，设置中心点坐标和地图级别  
                //if (is_autoload)
                //    this.longtrip();
                //this.check_page(1);
            },
            /**
             * 切换搜索页
             * @param {any} page_id
             */
            check_page: function (page_id) {
                this.curpage = page_id;
                var data = this.searchdata;
                data['page'] = this.curpage;
                send_data({
                    url: this.cursearchapi,
                    data: data,
                    callback: this.searchcallback
                })
                //callback: (in_data) => {
                //    var data = in_data['data'];
                //    if (in_data['code'] != 200)
                //        return null;
                //    //var data = JSON.parse(data_json['data'])
                //    //this.carpool_list = JSON.parse(data['data']);
                //    //this.curpage = data['curpage'];
                //    //this.sumcount = data['sumcount'];
                //    //this.pages = Number.parseInt(this.sumcount / this.pagedatacount) + (this.sumcount % this.pagedatacount > 0 ? 1 : 0);
                //    return data;
                //}
            },
            parse_pagelist_date: function (str) {
                var date = new Date(str);
                var month = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1);
                var day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
                var time = "";

                /*
                 清晨：05：01-06：59

                 早上：07：01-08：59
                 
                 上午：09：00-12：00
                 
                 中午：12：01-13：59
                 
                 下午：14：00-17：59
                 
                 傍晚：18：00-18：59
                 
                 晚上：19：00-23：59
                 
                 凌晨：24：00-05：00
                 */

                if (date.getHours() < 5)
                    time = "凌晨";
                else if (date.getHours() < 7)
                    time = "清晨";
                else if (date.getHours() < 9)
                    time = "早上";
                else if (date.getHours() < 12)
                    time = "上午";
                else if (date.getHours() < 14)
                    time = "中午";
                else if (date.getHours() < 18)
                    time = "下午";
                else if (date.getHours() < 19)
                    time = "傍晚";
                else if (date.getHours() < 24)
                    time = "晚上";

                return month + '-' + day + ' ' + time;
            },
            /**
             * 获取指定拼车信息
             * @param {any} id
             */
            getorderinfo: function (id, callback) {
                //刷新数据
                this.orderdetailsid = +new Date();

                send_data({
                    url: api_url + 'orders/getorderinfo',
                    data: { id: id },
                    callback: (in_data) => {
                        callback(in_data);
                    }
                });
            },
            /**
             * 获取长途拼车详细信息
             * @param {any} id
             */
            longtripgetinfo: function (id) {

                this.getorderinfo(id, (in_data) => {

                    if (in_data['code'] != 200)
                        return;
                    this.order_details = in_data['data'];
                    $("#mode-1-carpools-details").show();
                    $("#mode-1-carpools").hide();
                    this.route_planning(this.order_details['from'], this.order_details['to']);
                })

            },
            /**
             * 获取上下班拼车详细信息
             * @param {any} id
             */
            workinggetinfo: function (id) {

                this.getorderinfo(id, (in_data) => {

                    if (in_data['code'] != 200)
                        return;
                    this.order_details = in_data['data'];
                    $("#mode-2-carpools-details").show();
                    $("#mode-2-car-pool-list").hide();
                    this.route_planning(this.order_details['from'], this.order_details['to']);
                })

            },
            /**
             * 路线规划
             * @param {any} from
             * @param {any} to
             */
            route_planning: function (from, to) {
                var options = {
                    onSearchComplete: (results) => {
                        if (driving.getStatus() == BMAP_STATUS_SUCCESS) {
                            //this.order_details['distance'] = results.taxiFare.distance;
                            this.$set(this.order_details, 'distance', results.taxiFare.distance)
                        }
                    },
                    renderOptions: {
                        map: this.map_1,
                        panel: "results",
                        autoViewport: true
                    }
                };
                var driving = new BMap.DrivingRoute(this.map_1, options);
                driving.search(from, to);
                //driving.search(to, '深圳');

                //console.log(res);
            },
            //获取手机号码
            gettel: function (e) {
                if (!is_login(true))
                    return;
                _this = $(e.target);
                _this.text(this.order_details['tel']);
            },
            /**
             * 长途
             * */
            longtrip: function (e) {
                if (e != null) {
                    var _this = $(e.target);
                    $(".mode-menu a").removeClass("cur");
                    _this.addClass("cur");
                }
                //this.carpool_list = [];
                //this.order_details = "";
                //this.map_1.centerAndZoom(new BMap.Point(116.404, 39.915), 15);
                $(".mode-1").show();
                $(".mode-2").hide();

                this.init_map("mode-1-map");
                $("#mode-1-carpools-details").hide();
                $("#mode-1-carpools").show();

                this.cursearchapi = api_url + "orders/gethotpagesinfo";
                this.searchdata = { 'type': 0, 'count': 12 };
                this.searchcallback = (in_data) => {
                    if (in_data['code'] != 200)
                        return;
                    var data = in_data['data'];
                    this.carpool_list = data['data'];
                    this.curpage = data['curpage'];
                    this.pagecount = data['pagecount'];
                }
                this.check_page(1);
            },
            /**
             * 设置搜索信息
             * */
            search: function (from, to, time, type, count, callback) {
                //var from = $("#mode1-start-city").val();
                //var to = $("#mode1-end-city").val();
                //var time = $("#mode1-date").val();

                this.cursearchapi = api_url + 'orders/search';
                this.searchdata = { 'from': from, 'to': to, 'time': time, 'type': type, 'count': count };
                this.searchcallback = (in_data) => {
                    if (callback) {
                        callback(in_data);
                    }
                }
                //this.check_page(1);
                //send_data({
                //    url: api_url + 'orders/search',
                //    data: { 'from': from, 'to': to, 'time': time, 'type': 0, 'page': 1, 'count': this.pagedatacount },
                //    callback: (in_data) => {
                //        console.log(in_data);
                //    }
                //})
            },
            /**
             * 设置搜索条件
             * @param {any} param0
             */
            setsearch: function ({ url, from, to, time, type, count, callback }) {
                this.cursearchapi = url;
                this.searchdata = { 'from': from, 'to': to, 'time': time, 'type': type, 'count': count };

                this.searchcallback = (in_data) => {
                    if (callback)
                        callback(in_data);
                }
            },
            /**
             * 长途拼车搜索
             * */
            longtripsearch: function () {

                var from = $("#mode1-start-city").val();
                var to = $("#mode1-end-city").val();
                var time = $("#mode1-date").val();
                if (from == "" || to == "" || time == "") {
                    alert("请将搜索信息填写完整");
                    return;
                }
                //设置搜索信息
                this.setsearch({
                    url: api_url + 'orders/search',
                    from: from,
                    to: to,
                    time: time,
                    type: '0',
                    count: 12,
                    callback: (in_data) => {
                        if (in_data == null)
                            return;
                        $("#mode-1-carpools-details").hide();
                        $("#mode-1-carpools").show();

                        var data = in_data['data'];
                        this.carpool_list = data['data'];
                        this.pagecount = data['pagecount'];
                    }
                })
                //this.search(from, to, time, "0", (in_data) => {
                //    if (in_data == null)
                //        return;

                //    var data = in_data['data'];
                //    this.carpool_list = data['data'];
                //    this.pagecount = data['pagecount'];
                //})
                this.check_page(1);
            },
            applyfor: function (or_id) {
                if (!is_login(true))
                    return;
                authorization_data({
                    url: api_url + 'orders/applyfor',
                    data: { 'or_id': or_id },
                    callback: (in_data) => {
                        if (in_data['code'] == 200)
                            alert("加入成功");
                        else if (in_data['code'] == 11002) {
                            alert(in_data['status']);
                        } else if (in_data['code'] == 10011)
                            Login_Show();
                    }
                })
            },
            /**
             * 上下班拼车
             * */
            workingcarpool: function (e) {
                if (e != null) {
                    var _this = $(e.target);
                    $(".mode-menu a").removeClass("cur");
                    _this.addClass("cur");
                }
                mode2id = 'mode2' + +new Date();
                this.order_details = "";


                $(".mode-1").hide();
                $(".mode-2").show();

                $("#mode-2-car-pool-list").show();
                $("#mode-2-carpools-details").hide();

                this.init_map("mode-2-map");

                //this.map_1.addEventListener('click', (e) => {
                //    var marker = new BMap.Marker(e.point);
                //    this.map_1.addOverlay(marker);
                //    console.log(e);
                //})

                //设置搜索条件
                this.setsearch({
                    url: api_url + 'orders/getworklist',
                    //from: from,
                    //to: to,
                    //time: time,
                    type: '1',
                    count: -1,
                    callback: (in_data) => {
                        if (in_data == null)
                            return;

                        var data = in_data['data'];
                        this.workinglist = data['data'];
                        this.pagecount = data['pagecount'];
                    }
                })

                //this.cursearchapi = api_url + "orders/getworklist";
                ////this.searchdata = { 'type': 1, 'count': 15 };
                //this.search(null, null, null, '1', -1, null);
                //this.searchcallback = (in_data) => {
                //    if (in_data['code'] != 200)
                //        return;
                //    var data = in_data['data'];
                //    this.workinglist = data['data'];
                //    this.curpage = data['curpage'];
                //    this.pagecount = data['pagecount'];
                //}
                this.check_page(1);
            },
            /**
             * 上下班拼车搜索
             * */
            workingcarpoolsearch: function () {
                var from = $("#mode-2-from").val();
                var to = $("#mode-2-to").val();
                if (from == "" || to == "") {
                    alert("请将搜索内容填写完整");
                    return;
                }

                //this.searchdata = { 'type': 1, 'count': 15 };
                //this.search(from, to, null, (in_data) => {
                //    console.log(in_data);

                //    if (in_data['code'] != 200)
                //        return null;
                //    this.workinglist = in_data['data'];
                //})
                this.setsearch({
                    url: api_url + 'orders/search',
                    from: from,
                    to: to,
                    type: '1',
                    count: -1,
                    callback: (in_data) => {
                        if (in_data == null)
                            return;
                        $("#mode-2-car-pool-list").show();
                        $("#mode-2-carpools-details").hide();


                        var data = in_data['data'];
                        this.workinglist = data['data'];
                        this.pagecount = data['pagecount'];
                    }
                })
                this.check_page(1);
            }
            //mode_2_check_from: function () {
            //    var from = $("#mode-2-from");
            //    if (from.val() == "")
            //        return true;
            //    return false;
            //}
        },

        watch: {

        }
    })
    mode_vue1.longtrip();
})