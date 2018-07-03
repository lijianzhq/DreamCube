
if (window.Mediator) {
    var thenChannel = "mediator_extend_then";
    Mediator.prototype.then = function (fn) {
        this.subscribe(thenChannel, fn);
    };
    Mediator.prototype.publishThen = function (data) {
        this.publish(thenChannel, data);
        this.off(thenChannel, data);
    };
}