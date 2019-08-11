setupConnection = (hubProxy) => {
    hubProxy.client.receiveOrderUpdate = (updateObject) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = `Order: ${updateObject.OrderId}: ${updateObject.Update}`;
    };
    hubProxy.client.newOrder = (order) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = `Somebody ordered an ${order.Product}`;
    };
    hubProxy.client.finished = (order) => {
        //stop? $.connection.hub.stop();
        console.log(`Finished coffee order ${order}`);
    };
};

$(document).ready(() => {
    var hubProxy = $.connection.coffeeHub;
    setupConnection(hubProxy);
    $.connection.hub.start();

    document.getElementById("submit").addEventListener("click",
        e => {
            e.preventDefault();
            var statusDiv = document.getElementById("status");
            statusDiv.innerHTML = "Submitting order..";

            const product = document.getElementById("product").value;
            const size = document.getElementById("size").value;

            fetch("api/Coffee",
                    {
                        method: "POST",
                        body: JSON.stringify({ product, size }),
                        headers: {
                            'content-type': 'application/json'
                        }
                    })
                .then(response => response.text())
                .then(id => hubProxy.server.getUpdateForOrder({ id, product, size })
                    .fail(error => console.log(error))
                );

        });
});