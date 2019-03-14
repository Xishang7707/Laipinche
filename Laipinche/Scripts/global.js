//var PublicKey =
//    "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCu1HIhpQtyVFhuoIjsUeoEAZ4/" +
//    "i0iSAruMmDlUzC1CuSpLxoT/QDkKJbAqd7m4J/L2fIFh5FNdPe8t1epxp9aWXjWi" +
//    "fta5oBBkIw1k4hPJdhaZ5W0SjQH6TIX8EMxfYuC1X+jUWz1Kok0vTmsAsCF5LnY7" +
//    "+oxr/+ItrFPQPb5w3QIDAQAB";
var win = $(window);
var api_url = "http://localhost:50843/api/";
var page_min_width = 1200;
var PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3C3llUq20aWBzBHAib7wIhGV7Pk+7vNOE4Ci2TinoDBA3xcch9nEB52IlghdUTLdk+Z5Wcj5SDjQkPVFMLvUBVGzM9Rv21cckvDcsUNqhDCQdpPxwNsFAeiQcMOyjoo6Q9ZzH4Qd+NbtWMV7kY3lES/AvzoKxGueMiZXa+iSCq50CQ1l7kmcuWaCWygOd7UcqLidg2oCdh1cN547Rv4RJONh771RpdwbnC6OSniLLkXvk5NDWk2r+v1Ngyh72LGuMiiySdhiuEhnMik797eF3cTm6IHKEaXAeYJdgt9G95Xjrj4t7WYxT1jpTYkK7c73RGyAvaDuH8GDlGoKqFJyqwIDAQAB";
/**
 * 设置position:absolute的水平居中
 * @param {Element} parent
 * @param {Element} child
 * @param {number}  limit
 */
function set_align_center(parent, child, limit = -1) {
    var p_w = parent.width();
    var c_w = child.width();
    if (p_w > (limit == -1 ? c_w : limit)) {
        child.css({ "left": (p_w - c_w) / 2 });
    }
}
/**
 * 设置position:absolute垂直居中
 * @param {Element} parent
 * @param {Element} child
 * @param {number}  limit
 */
function set_vertical_center(parent, child, limit = -1) {
    var p_h = parent.height();
    var c_h = child.height();
    if (p_h > (limit == -1 ? c_h : limit)) {
        child.css({ "top": (p_h - c_h) / 2 });
    }
}

/**
 * 发送数据
 * @param {any} url
 * @param {any} data
 * @param {any} callback
 */

function send_data({ url, data, callback }) {
    data['t'] = new Date().getTime();

    var cryp = new JSEncrypt();
    cryp.setPublicKey(PublicKey);
    var signval = cryp.encrypt(JSON.stringify(data));

    $.ajax({
        "url": url,
        "method": "POST",
        "headers": {
            "content-type": "application/json"
        },
        async: true,
        xhrFields: {
            withCredentials: true
        },
        "data": JSON.stringify({ "data": signval }),
        success: function (in_data) {
            callback(in_data)
        }
    })
}
/**
 * 是否登录
 * @param {any} popup
 */
function is_login(poplogin = false) {
    var ssid = $.cookie("LPCSSID");

    if (ssid == null || ssid == "null") {
        if (poplogin == true)
            Login_Show();
        return false;
    }

    return true;
}
/**
 * 访问授权数据
 * @param {any} param0
 */
function authorization_data({ url, data, pop = false, callback }) {
    if (!is_login(pop))
        return;
    var to_data = {};
    if (data == null)
        to_data = { 'LPCSSID': $.cookie('LPCSSID') };
    else {
        to_data = data;
        to_data['LPCSSID'] = $.cookie('LPCSSID');
    }

    send_data({
        url: url,
        data: to_data,
        callback: callback
    });
}