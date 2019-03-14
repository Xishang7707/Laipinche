var mode_vue1;
$(function () {
    document.oncontextmenu = () => { return false };
    document.onselectstart = () => { return false };
    $('img').on('dragstart', function (event) { event.preventDefault(); });
    //$('.page').width(win.width());
    $('.page').height(win.height());

    set_align_center(win, $("div.header .wrapper"), page_min_width);
    set_align_center(win, $("div.page .content-wrap"), page_min_width);
    set_vertical_center(win, $("div.page .content-wrap"));

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
    mode_vue1 = new Vue({
        el: "#mode-1",
        data: {
            orderdetailsid: +new Date(),
            map_1: null,
            carpool_list: [],
            type: 0,
            curpage: 1,
            pagecount: 1,
            datacount: 12,
            //sumcount: 0,
            order_details: "",
            paytype: ['免费', '面议', '一口价'],
            cartype: ['轿车', 'MPV', 'SUV', '跑车', '客车', '其他'],
            cursearchapi: '',
            searchdata: {},
            searchcallback: null
        },
        methods: {
            init_map: function (is_autoload = false) {
                this.map_1 = new BMap.Map("mode-1-map");

                var point = new BMap.Point(116.404, 39.915);  // 创建点坐标  
                this.map_1.centerAndZoom(point, 15);                 // 初始化地图，设置中心点坐标和地图级别  
                if (is_autoload)
                    this.longtrip();
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
            getorderinfo: function (id) {
                //刷新数据
                this.orderdetailsid = +new Date();
                send_data({
                    url: api_url + 'orders/getorderinfo',
                    data: { id: id },
                    callback: (in_data) => {

                        var data = in_data;
                        if (in_data['code'] != 200)
                            return;
                        this.order_details = in_data['data'];
                        $("#mode-1-carpools-details").show();
                        $("#mode-1-carpools").hide();
                        this.route_planning(this.order_details['from'], this.order_details['to']);
                    }
                });
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
            longtrip: function () {
                //this.carpool_list = [];
                //this.order_details = "";
                //this.map_1.centerAndZoom(new BMap.Point(116.404, 39.915), 15);
                this.init_map();
                $("#mode-1-carpools-details").hide();
                $("#mode-1-carpools").show();

                this.cursearchapi = api_url + "orders/gethotpagesinfo";
                this.searchdata = { 'type': this.type, 'count': this.datacount };
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
             * 搜索拼车信息
             * */
            search: function () {
                var from = $("#mode1-start-city").val();
                var to = $("#mode1-end-city").val();
                var time = $("#mode1-date").val();
                if (from == "" || to == "" || time == "") {
                    alert("请将搜索信息填写完整");
                    return;
                }

                this.cursearchapi = api_url + 'orders/search';
                this.searchdata = { 'from': from, 'to': to, 'time': time, 'type': this.type, 'count': this.datacount };
                this.searchcallback = (in_data) => {
                    if (in_data['code'] != 200)
                        return null;
                    var data = in_data['data'];

                    this.carpool_list = data['data'];
                    this.pagecount = data['pagecount'];
                }
                this.check_page(1);
                //send_data({
                //    url: api_url + 'orders/search',
                //    data: { 'from': from, 'to': to, 'time': time, 'type': 0, 'page': 1, 'count': this.pagedatacount },
                //    callback: (in_data) => {
                //        console.log(in_data);
                //    }
                //})
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
                        else if (in_data['code'] == 11003) {
                            alert(in_data['status']);
                        } else alert(in_data['status']);
                    }
                })
            }
        },

        watch: {

        }
    })
    mode_vue1.init_map(true);
})