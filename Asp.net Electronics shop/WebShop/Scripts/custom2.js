function CallChangefunc(val) {
    window.location.href = "/Cart/UpdateShipping/" + val;
}

$(function () {
    $('#reducequantity').click(function () {
        alert("Product quantity decreased!");
    });
});

$(function () {
    $('#increasequantity').click(function () {
        alert("Product quantity increased!");
    });
});
$(function () {
    $('#btn_update_product').click(function () {
        location.reload();
    });
});


