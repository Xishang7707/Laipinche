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