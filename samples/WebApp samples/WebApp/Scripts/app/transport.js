(function () {

    var TRANSPORTS_URL = '/api/transport/';
    var deviceIdTypeNames = {
        1: 'Phone number',
        2: 'IMEI',
        4: 'Own ID',
        8: 'MAC address'
    }

    /**
     * Setting content into cell depending on the index
     * @param <Node> cell
     * @param <Object> data
     * @param <Number> index
     * @returns void
     */
    function setContentForTableCell(cell, data, index) {
        var className = '';
        var content = '';

        switch (index) {
            case 0:
                content = data.Name;
                break;
            case 1:
                content = data.DeviceIdentifier ? deviceIdTypeNames[data.DeviceIdentifier.Type] : 'No data';
                break;
            case 2:
                content = data.DeviceIdentifier ? data.DeviceIdentifier.Id : 'No data';
                break;
            case 3:
                content = data.LastStatus ? new Date(data.LastStatus.LastActivityTime).toLocaleString() : 'No device';
                className = data.Device ? 'LastActivity' + data.Device.Id : '';
                break;
            case 4:
                content = data.LastStatus ? new Date(data.LastStatus.LastCoordinate.UtcDate).toLocaleString() : 'No device';
                className = data.Device ? 'LastCoord' + data.Device.Id : '';
                break;
            case 5:
                var editUrl = '"/transport/edit/' + data.Id + '"';
                content = '<a href=' + editUrl + '><i class="glyphicon glyphicon-pencil"></i> Edit</a>';
                break;
        }

        cell.html(content).addClass(className);
    }

    /**
     * Adding row with transport item to table
     * @param <Object> transportItem
     * @returns void
     */
    function addToTable(transportItem) {
        var rowNode = $('<tr>');
        var colsCount = $('th:not([colspan])').length;

        for (var i = 0; i < colsCount; i++) {
            var cell = $('<td>');
            setContentForTableCell(cell, transportItem, i);
            rowNode.append(cell);
        }

        $('tbody').append(rowNode);
    };

    // loading and proccessing transports
    $.get(TRANSPORTS_URL).done(function (transports) {
        transports.forEach(function (t) {
            addToTable(t);
        });
    });

    /**
     * Subscribe on event of changing tracking device status
     */
    app.events.Subscribe("OnDeviceStatusChanged", function (deviceStatus) {
        $('.' + 'LastActivity' + deviceStatus.Id).html(new Date(deviceStatus.LastActivityTime).toLocaleString());
        $('.' + 'LastCoord' + deviceStatus.Id).html(new Date(deviceStatus.LastCoordinate.UtcDate).toLocaleString());
    });

    /**
     * Subscribe on event of data changing
     */
    app.events.Subscribe('OnDataChanged', function (event) {
        if (event.TypeId === 'TotalApi.Telematics.Device' && event.ActionType === 2)
            // removing row with deleted transport
            $('tr').find('.' + 'LastActivity' + event.ObjectId).parent().remove();
    });

})();