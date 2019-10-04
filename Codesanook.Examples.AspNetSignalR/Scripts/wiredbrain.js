
$(document).ready(() => {
    var coffeeHub = $.connection.coffeeHub;

    // Create a function that the hub can call back to display messages.
    coffeeHub.client.receiveOrderUpdate = function (result) {
        console.log(result);
    };

    $.connection.hub.start().done(function () {
        console.log('connected');
        coffeeHub.server.getUpdateForOrder({ id: 1, product: 'xxx', size: 'tall' });
    });

    $.connection.hub.disconnected(function () {
        console.log('disconnected');
        //setTimeout(function () {
        //    $.connection.hub.start().done(function () {
        //        coffeeHub.server.getUpdateForOrder({ id: 1, product: 'xxx', size: 'tall' });
        //    });
        //}, 3000);
    });
});