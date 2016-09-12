var app;
(function (app) {
    "use strict";

    var started;
    var noRestart;

    app.events = new app.AppEvents();

    app.OnReadySignalR = function (isReconnect) {
        if (isReconnect)
            console.log('SignalR: Now reconnected, connection id = ' + $.connection.hub.id);
        else
            console.log('SignalR: Now connected, connection id = ' + $.connection.hub.id);
    };
    app.OnErrorSignalR = function (err, isReconnect) {
        if (isReconnect === undefined) {
            console.error('SignalR: ' + err);
        }
        else if (isReconnect) {
            console.error('SignalR: Could not reconnect: ' + err);
            setTimeout(function () { return startHub(true); }, 3000);
        }
        else {
            console.error('SignalR: Could not connect: ' + err);
        }
    };
    app.OnSlowConnectionSignalR = function () {
        console.warn('SignalR: We are currently experiencing difficulties with the connection.');
    };

    function startHub(isReconnect) {
        if (isReconnect === void 0) { isReconnect = false; }
        if (started)
            return;
        $.connection.hub.start()
            .done(function () {
                started = true;
                app.OnReadySignalR(isReconnect);
            })
            .fail(function (err) { return app.OnErrorSignalR(err, isReconnect); });
    }
    function updateHub() {
        if (started) {
            noRestart = false;
            $.connection.hub.stop(true);
        }
        else {
            startHub();
        }
    }
    function stopHub() {
        noRestart = true;
        $.connection.hub.stop(true);
    }
    $.connection.hub.connectionSlow(app.OnSlowConnectionSignalR);
    $.connection.hub.error(app.OnErrorSignalR);
    $.connection.hub.disconnected(function () {
        started = false;
        if (!noRestart)
            setTimeout(function () { return startHub(true); }, 10);
    });
    $(document).ready(function () {
        //$.connection.hub.logging = true;
        $.connection.messagingHub.client.Update = updateHub;
        $.connection.messagingHub.client.Stop = stopHub;
    });
})(app || (app = {}));