var app;
(function (app) {
    'use strict';
    var appEvents = (function () {
        function appEvents() {
            this._subscribers = [];
            this._events = $.connection.messagingHub.client;
            this.Subscriber = appEvents.CreateSubscriber(this);
        }
        appEvents.prototype.updateEventSubscriber = function (eventName) {
            var _this = this;
            var subscribers = this._subscribers.where(function (s) { return s.hash.indexOf(eventName + ":") === 0; }).toArray();
            if (subscribers.length === 0) {
                this._events[eventName] = undefined;
            }
            else {
                this._events[eventName] = function () {
                    var args = [];
                    for (var i = 0; i < arguments.length; i++) {
                        args[i - 0] = arguments[i];
                    }
                    var self = _this;
                    _this._subscribers
                        .toArray()
                        .where(function (s) { return s.hash.indexOf(eventName + ":") === 0; })
                        .forEach(function (s) {
                            return setTimeout(function () {
                                var e;
                                try {
                                    if (s.subscriber && s.subscriber.handler)
                                        s.subscriber.handler.apply(self, args);
                                    else
                                        _this._subscribers.splice(_this._subscribers.indexOf(s), 1);
                                }
                                catch (e) {
                                    _this._subscribers.splice(_this._subscribers.indexOf(s), 1);
                                    console.error(e.message);
                                }
                            }, 0);
                        });
                };
            }
        };
        appEvents.prototype.updateSubscribers = function () {
            var _this = this;
            if (this._updServices)
                clearTimeout(this._updServices);
            this._updServices = setTimeout(function () {
                _this._updServices = undefined;
                var eventNames = _this._subscribers.select(function (s) { return s.hash.split(":")[0]; }).distinct().toArray();
                _this._subscribers.where(function (s) { return !s.subscriber; }).forEach(function (s) { _this._subscribers.splice(_this._subscribers.indexOf(s), 1); });
                eventNames.forEach(function (e) { return _this.updateEventSubscriber(e); });
                if (_this._subscribers.length > 0) {
                    var currentEventNames = _this._subscribers.select(function (s) { return s.hash.split(":")[0]; }).distinct().orderBy(function (x) { return x; }).toArray().join(',');
                    if (_this._lastEventNames !== currentEventNames)
                        _this._events.Update();
                    _this._lastEventNames = currentEventNames;
                }
                else
                    _this._events.Stop();
            }, 50);
        };
        appEvents.prototype.Subscribe = function (eventName, handler, context) {
            context = context ? ":" + context : "";
            var hash = eventName + ":" + handler.toString().ToMd5() + context;
            var subscriber = this._subscribers.firstOrDefault(function (s) { return s.hash === hash; });
            if (subscriber) {
                if (subscriber.subscriber)
                    subscriber.subscriber.handler = handler;
            }
            else {
                this._subscribers.push({ hash: hash, subscriber: { eventName: eventName, handler: handler } });
                this.updateSubscribers();
            }
        };
        appEvents.prototype.UnSubscribe = function (eventName, handler, context) {
            context = context ? ":" + context : "";
            var hash = eventName + ":" + (handler || "").toString().ToMd5() + context;
            this._subscribers.forEach(function (s) {
                if (!eventName || !handler && s.hash.StartsWith(eventName + ":") || handler && s.hash === hash) {
                    s.subscriber = null;
                }
            });
            this.updateSubscribers();
        };
        appEvents.CreateSubscriber = function (eventSubscriber) {
            var res = (function (eventName, handler) { return eventSubscriber.Subscribe(eventName, handler); });
            res.OnPing = function (handler) { return res("OnPing", handler); };
            res.OnDataChanged = function (handler) { return res("OnDataChanged", handler); };
            res.OnDeviceStatusChanged = function (handler) { return res("OnDeviceStatusChanged", handler); };
            res.OnProgress = function (handler) { return res("OnProgress", handler); };
            res.Subscribe = function (eventName, handler, context) { return eventSubscriber.Subscribe(eventName, handler, context); };
            res.UnSubscribe = function (eventName, handler, context) { return eventSubscriber.UnSubscribe(eventName, handler, context); };
            return res;
        };
        return appEvents;
    })();
    app.AppEvents = appEvents;
})(app || (app = {}));
