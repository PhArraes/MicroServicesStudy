var cart = {

    items: [],
    add: function(item) {
        cart.items.push(`item=${item}`);
    },
    cartParams: function() {
        var params = `?${cart.items.join('&')}&ip=${clientIp}`;
        return params;
    },
    cartRedirect: function() {
        window.location = '/cart' + cart.cartParams();
    },
    buy: function() {
        window.location = '/order?ip=' + clientIp;
    }

}