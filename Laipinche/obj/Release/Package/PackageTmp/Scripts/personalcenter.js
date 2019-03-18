$(function () {
    function GetQueryString(name) {

        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");

        var r = window.location.search.substr(1).match(reg);

        if (r != null) return unescape(r[2]); return null;

    }

    if (is_login() == false) {
        location.href = "http://localhost:50822";
    }

    new Vue({
        el: "#personcenter",
        data: {
            orderstate: ['正在进行', '已经开始', '完成', '关闭', '申请中'],
            us_info: "",
            statistical: "",
            carpools: "",
        },
        methods: {
            /**
             * 用户中心
             * @param {any} e
             */
            menu_personcenter: function (e) {
                console.log(this.$el);
                $(".content-wrap").children().hide();
                $(".us-center").show();

                $("#personcenter .menu").children().removeClass('cur');
                $("#personcenter .menu").children().eq(1).addClass("cur");
                this.getuserinfo();
            },
            /**
             * 发布的长途拼车
             * @param {any} e
             */
            menu_longcarpooling: function (e) {
                $(".content-wrap").children().hide();
                $(".longcarpooling").show();

                $("#personcenter .menu").children().removeClass('cur');
                $("#personcenter .menu").children().eq(3).addClass("cur");
                this.getcarpools(0);
            },
            menu_longcarpool_apply: function (e) {
                $(".content-wrap").children().hide();
                $(".longcarpool-apply").show();

                $("#personcenter .menu").children().removeClass('cur');
                $("#personcenter .menu").children().eq(4).addClass("cur");
                this.getapplying(0);
            },
            /**
             * 发布的上下班拼车
             * @param {any} e
             */
            menu_workcarpooling: function (e) {
                $(".content-wrap").children().hide();
                $(".workcarpooling").show();

                $("#personcenter .menu").children().removeClass('cur');
                $("#personcenter .menu").children().eq(6).addClass("cur");
                this.getcarpools(1);
            },
            menu_workcarpool_apply: function (e) {
                $(".content-wrap").children().hide();
                $(".workcarpool-apply").show();

                $("#personcenter .menu").children().removeClass('cur');
                $("#personcenter .menu").children().eq(7).addClass("cur");
                this.getapplying(1);
            },
            /**
             * 获取信息
             * */
            getuserinfo: function () {
                authorization_data({
                    url: api_url + "users/getinfo",
                    callback: (in_data) => {
                        if (in_data['code'] == 200)
                            this.us_info = in_data['data'];
                    }
                })
                authorization_data({
                    url: api_url + "orders/getstatistical",

                    callback: (in_data) => {
                        if (in_data['code'] == 10011) {
                            Login_Show();
                            return;
                        }
                        if (in_data['code'] != 200)
                            return;
                        this.statistical = in_data['data'];
                    }
                })
            },
            getcarpools: function (type) {
                authorization_data({
                    url: api_url + 'orders/getcarpools',
                    data: { 'type': type },
                    callback: (in_data) => {
                        console.log(in_data);
                        if (in_data['code'] == 10011) {
                            Login_Show();
                            return;
                        }
                        if (in_data['code'] != 200)
                            return;
                        this.carpools = in_data['data'];

                    }
                })
            },
            closeorder: function (id, type) {
                authorization_data({
                    url: api_url + 'orders/closeorder',
                    data: { id: id },
                    callback: (in_data) => {

                        if (in_data['code'] == 200) {
                            this.getcarpools(type);
                        }
                    }
                })
            }, closeapply: function (id, type) {
                authorization_data({
                    url: api_url + 'orders/closeapplyorder',
                    data: { id: id },
                    callback: (in_data) => {
                        console.log(in_data);
                        if (in_data['code'] == 200) {
                            this.getapplying(type);
                        }
                    }
                })
            },
            getapplying: function (type) {
                authorization_data({
                    url: api_url + 'orders/getapplying',
                    data: { 'type': type },
                    callback: (in_data) => {
                        this.carpools = {};
                        if (in_data['code'] == 10011) {
                            Login_Show();
                            return;
                        }
                        if (in_data['code'] != 200)
                            return;
                        this.carpools = in_data['data'];
                    }
                })
            },
            check_menu: function (option) {
                console.log(option);
                switch (option) {
                    case '1':
                        this.menu_personcenter();
                        break;
                    case '2':
                        this.menu_longcarpooling();
                        break;
                    case '3':
                        this.menu_longcarpool_apply();
                        break;
                    default:
                        this.menu_personcenter();
                        break;
                }
            }
        }
    }).check_menu(GetQueryString('op'))
})