(function() {

    var TRACK_URL = 'totalapi/telematics/readcoordinates';

    $('.range-input').on('change', function () {
        $('.range-display').text($('.range-input').val());
    });

    $('form').submit(function () {
        // disabling button and displaying loading animation button inside
        $('.button-load').html('<i class="glyphicon glyphicon-refresh spin"></i> Loading...').attr('disabled', '');
        var startTime = new Date();
        var date = new Date();
        var period = $('.range-input').val();
        var params = {
            DeviceId: {
                // Tr1
                Id: '00000000-0000-0000-0000-222200000002',
                // database ID
                Type: 0
            },
            From: new Date(date.setHours(date.getHours() - period)).toISOString(),
            To: new Date().toISOString()
        }
        // loading data
        $.post(TRACK_URL, params)
            .done(function (data) {
                var finishTime = new Date();
                $('.button-load').text('Load').removeAttr('disabled');

                // creating and displaying alert message
                var alert = $('<div class="alert alert-info alert-dismissible fade in"></div>');
                alert.html('The count of coordinates: ' + '<b>' + data.$values.length + '</b><br>' + 'elapsed time: ' + '<b>' + (finishTime - startTime) + 'ms </b>');
                $('.msg-box').append(alert);
            })
            .error(function (err) {
                $('.button-load').text('Load').removeAttr('disabled');
                alert(err.statusText);
            });

        return false;
    });

})();