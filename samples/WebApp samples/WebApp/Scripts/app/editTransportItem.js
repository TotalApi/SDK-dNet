(function () {

    var TRANSPORTS_URL = '/api/transport/';
    var urlLastPart = location.pathname.slice(location.pathname.lastIndexOf('/') + 1);
    var transportItemId = urlLastPart.length > 0 ? urlLastPart : null;

    // disabling input fields if transport item is default (id - number from 1 to 4)
    if (transportItemId && transportItemId.length === 1) {
        $('fieldset').attr('disabled', '');
    }

    if (transportItemId) {
        // loading transport item and setting input values
        $.get(TRANSPORTS_URL + transportItemId).done(function (data) {
            $('input[name=name]').val(data.Name);
            $('select[name=deviceIdType]').val(data.DeviceIdentifier.Type);
            $('input[name=deviceIdValue]').val(data.DeviceIdentifier.Id);
        });
    }

    $('form').submit(function () {
        $('.button-save').attr('disabled', '');

        // object to save
        var transportItem = {
            Id: transportItemId,
            Name: $('input[name=name]').val(),
            DeviceIdentifier: {
                Id: $('input[name=deviceIdValue]').val(),
                Type: parseInt($('select[name=deviceIdType]').val())
            }
        }

        $.ajax(TRANSPORTS_URL, {
            // 'PUT' for edit, 'POST' for create new item
            type: transportItemId ? 'PUT' : 'POST',
            data: transportItem,
            complete: function (result) {
                $('.button-save').removeAttr('disabled');

                if (result.status === 200)
                    location.pathname = '/transport/';
                else
                    alert('Saving failed!');
            }
        });

        return false;
    });

})();