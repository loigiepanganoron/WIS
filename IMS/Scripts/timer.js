﻿(function () {
    // Defining a connection to the server hub.
    var myHub = $.connection.myHub1;
    // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
    $.connection.hub.logging = true;
    // Start the hub
    $.connection.hub.start();

    // This is the client method which is being called inside the MyHub constructor method every 3 seconds
    //myHub.client.hello = function (serverTime) {
    //    // Set the received serverTime in the span to show in browser
    //    $("#newTime").text(serverTime);
    //};

    myHub.client.read();


    $.connection.hub.start().done(function () {
        $('#btd').click(function () {
            myHub.server.hello();
        })


    })
}());  