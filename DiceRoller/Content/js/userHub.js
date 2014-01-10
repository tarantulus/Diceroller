var userHub = {

    init: function (hub) {
        this.hub = hub;
    },

    setNames: function (users, element) {
        element.innerHTML = helper.getNames(users);
    },
}