(function () {

    var map;
    var popup;
    var markers = [];
    var clickedMarkerId;
    var trackLatlngs;
    var trackLayerGroup;
    /**
     * API URL constants
     */
    var TRANSPORTS_URL = '/api/transport/';
    var TRACK_URL = 'totalapi/telematics/readcoordinates';

    /**
     * Creating and initialization the map on the "map" div
     * @returns void
     */
    function initMap() {
        map = L.map('map', {
            // centering in Dnipropetrovsk, Ukraine
            center: [48.464717, 35.046183],
            zoom: 12
        });

        // loading and displaying tile layers on the map
        L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);
    }

    /**
     * Creating and opening popup when on marker clicked
     * @param <Object> event
     * @returns void
     */
    function createMarkerPopup(event) {
        clickedMarkerId = event.target.options.markerId;
        var markerTitle = event.target.options.title;

        var popupContent =
            '<h4 class="text-center">' + markerTitle + '</h4>' +
            '<h5>Build a track per:</h5>' +
            '<div class="form-group">' +
                '<input type="range" class="range-track-period" value="12" min="1" max="24">' +
            '</div>' +
            '<button class="btn btn-info btn-block button-build-track">12 hour(s)</button>';

        popup = L.popup()
            .setLatLng(event.latlng)
            .setContent(popupContent)
            .openOn(map);
    }

    /**
     * Adding marker with event handler to map using him latitude and longitude
     * @param <Array> latlng - latitude and longitude
     * @param <String> id - ID that will be assigned marker
     * @returns void
     */
    function addMarkerToMap(latlng, id, title) {
        var marker = L.marker(latlng, {
            icon: L.icon({
                iconUrl: '/Content/images/marker-icon.png',
                shadowUrl: '/Content/images/marker-shadow.png',
                iconAnchor: [15, 36]
            }),
            markerId: id,
            title: title
        }).addTo(map);

        marker.on('click', createMarkerPopup);
        // adding marker in markers list
        markers.push(marker);
    }

    /**
     * Handler for transports
     * @param <Array> or <Object> transport
     * @returns void
     */
    function handleTransport(transport) {
        if (!Array.isArray(transport))
            transport = [transport];

        // adding each transport item to map
        transport.forEach(function (t) {
            if (t.LastStatus && t.LastStatus.LastCoordinate) {
                var lat = t.LastStatus.LastCoordinate.LatitudeDegree;
                var lng = t.LastStatus.LastCoordinate.LongitudeDegree;
                addMarkerToMap([lat, lng], t.Device.Id, t.Name);
            }
        });
    }

    /**
     * Drawing track on the map
     * @param <Object> data
     * @returns void
     */
    function drawTrack(data) {
        // no coordinates
        if (!data.$values.length) {
            map.closePopup();
            alert('During this period coordinates is not found!');
            return;
        }

        // list of overlays for layers group
        var trackLayers = [];

        // if any one track is drawn, remove this track
        if (trackLayerGroup)
            map.removeLayer(trackLayerGroup);

        trackLatlngs = data.$values.map(function (t) { return [t.LatitudeDegree, t.LongitudeDegree] });
        // creating track
        var track = L.polyline(trackLatlngs, { color: 'green' });
        trackLayers.push(track);

        data.$values.forEach(function (t) {
            if (t.IsStop) {
                // creating stop markers
                var stopMarker = L.circleMarker([t.LatitudeDegree, t.LongitudeDegree], {
                    color: 'red',
                    fillOpacity: 0.5,
                    weight: 1,
                    stopDateBegin: t.AdditionalInfo['StF.bd'],
                    stopDateEnd: t.AdditionalInfo['StF.ed']
                }).setRadius(7);

                // attaching event for each stop marker
                stopMarker.on('click', function (event) {
                    var stopDateBegin = new Date(event.target.options.stopDateBegin);
                    var stopDateEnd = new Date(event.target.options.stopDateEnd);
                    var parkingTime = Math.round((stopDateEnd - stopDateBegin) / 1000 / 60);
                    var popupContent =
                        '<h4 class="text-center">Pit-stop</h4>' +
                        '<div><b>Stop datetime:</b> ' + new Date(stopDateBegin).toLocaleString() + '</div>' +
                        '<div><b>Parking time:</b> ' + parkingTime + ' min' + '</div>';

                    // displaying popup with additional info about a stop
                    popup = L.popup()
                        .setLatLng(event.latlng)
                        .setContent(popupContent)
                        .openOn(map);
                });
                trackLayers.push(stopMarker);
            }
        });

        // adding layer group to map
        trackLayerGroup = L.layerGroup(trackLayers).addTo(map);

        // zooming the map to the track
        map.fitBounds(track.getBounds(), { maxZoom: 13 });

        $('.button-clear-track').removeAttr('disabled');
        map.closePopup();
    }

    // initialize the map
    initMap();
    // loading transports with own devices
    $.get(TRANSPORTS_URL, handleTransport);

    /**
     * Map and DOM events
     */
    map.on('popupopen', function () {
        // when a popup is opened subscribe on DOM-events click to the buttons
        $('.button-build-track').on('click', function () {
            var hourPeriod = $('.range-track-period').val();
            var date = new Date();
            var params = {
                DeviceId: {
                    Id: clickedMarkerId,
                    // database ID
                    Type: 0
                },
                // the past hour
                From: new Date(date.setHours(date.getHours() - hourPeriod)).toISOString(),
                To: new Date().toISOString()
            }
            // disabling button and displaying loading animation button inside
            $('.button-build-track').html('<i class="glyphicon glyphicon-refresh spin"></i> Loading...').attr('disabled', '');
            // loading data to build the track
            $.post(TRACK_URL, params, drawTrack);
        });

        $('.range-track-period').on('change', function() {
            $('.button-build-track').text($('.range-track-period').val() + ' hour(s)');
        });

        $('.button-clear-track').on('click', function () {
            // if any one track is drawn, remove this track
            if (trackLayerGroup) {
                map.removeLayer(trackLayerGroup);
                map.removeLayer(popup);
                $('.button-clear-track').attr('disabled', '');
            }
        });
    });

    /**
     * Subscribe on event of changing tracking device status
     */
    app.events.Subscribe('OnDeviceStatusChanged', function (deviceStatus) {
        var markerToMove;
        // finding target marker and moving him on map
        markers.forEach(function (m, i) {
            if (m.options.markerId === deviceStatus.Id) {
                markerToMove = markers[i];
                // converting latitude and longitude to degree
                var lat = deviceStatus.LastCoordinate.Latitude * 180 / Math.PI;
                var lng = deviceStatus.LastCoordinate.Longitude * 180 / Math.PI;
                markerToMove.setLatLng([lat, lng]);
            }
        });
    });

    /**
     * Subscribe on event of data changing
     */
    app.events.Subscribe('OnDataChanged', function (event) {
        function findAndRemoveMarker(markerId) {
            markers.forEach(function (m) {
                if (m.options.markerId === markerId)
                    map.removeLayer(m);
            });
        }

        switch (event.TypeId) {
            case 'WebApp.Models.Transport':
                $.get(TRANSPORTS_URL + event.ObjectId).done(function (transport) {
                    findAndRemoveMarker(transport.Device.Id);
                    handleTransport(transport);
                });
                break;
            case 'TotalApi.Telematics.Device':
                // delete tracking device item
                if (event.ActionType === 2) {
                    findAndRemoveMarker(event.ObjectId);
                }
                break;
        }
    });

})();
