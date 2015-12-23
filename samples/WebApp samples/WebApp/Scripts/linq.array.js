// extension for Array

(function (root) {
    if (root.Enumerable == null) {
        throw new Error("can't find Enumerable. linq.jquery.js must load after linq.js");
    }
    if (root.jQuery == null) {
        throw new Error("can't find jQuery. linq.jquery.js must load after jQuery");
    }

    var Enumerable = root.Enumerable;
    
    Enumerable.Utils.extendTo(Array);

})(this);