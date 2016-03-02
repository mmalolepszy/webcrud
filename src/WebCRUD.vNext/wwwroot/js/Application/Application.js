
function ApplicationController() {
    var self = this;
    var currentCulture;

    self.setCulture = function(culture) {
        Globalize.culture(culture);

        if (Messages.translations.hasOwnProperty(culture))
            currentCulture = culture;

        self.messages = Messages.translations[currentCulture];
    };
    
    self.showConfirmation = function (message, title, onConfirmed, onRejected) {
        /// <summary>Shows confirmation modal dialog with given text.</summary>
        /// <param name="message">confirmation message</param>
        /// <param name="title">(optional) title for the confirmation dialog</param>
        /// <param name="onConfirmed">(optional) handler when confirmation succeeded</param>
        /// <param name="onRejected">(optional) handler when confirmation succeeded</param>
        title = title || "Confirmation";
        onConfirmed = onConfirmed || function () { };
        onRejected = onRejected || function () { };

        $("#dialog-confirm").attr('title', title);
        $("#dialog-confirm-text").text(message);

        $("#dialog-confirm").dialog({
            resizable: false,
            draggable: false,
            modal: true,
            buttons: [
            {
                text: self.messages.yes,
                "class": "btn btn-primary",
                click: function () {
                    $(this).dialog("close");
                    onConfirmed();
                }
            },
            {
                text: self.messages.no,
                "class": "btn",
                click: function () {
                    $(this).dialog("close");
                    onRejected();
                },
            }]
        });
    };

    self.showError = function (message, title) {
        /// <summary>Shows modal dialog with given text.</summary>
        /// <param name="message">message</param>
        /// <param name="title">(optional) title for the dialog</param>
        title = title || "Error";
        
        $("#dialog-error").attr('title', title);
        $("#dialog-error-text").text(message);

        $("#dialog-error").dialog({
            resizable: false,
            draggable: false,
            modal: true,
            buttons: [
            {
                text: self.messages.ok,
                "class": "btn btn-primary",
                click: function () {
                    $(this).dialog("close");
                }
            }]
        });
    };
    
    self.initializeDatepickers = function initializeDatepickers(element) {
        $.datepicker.setDefaults($.datepicker.regional['']);
        var culture = getDatepickersCulture();
        if (element != null)
            element.datepicker($.datepicker.regional[culture]);
        else
            $(".datepicker, .date").datepicker($.datepicker.regional[culture]);
    }

    function getDatepickersCulture() {
        var cultureName = $.cookie('locale') || 'en-GB';

        if (!(cultureName in $.datepicker.regional) && cultureName.indexOf("-") != -1) {
            cultureName = cultureName.substr(0, cultureName.indexOf("-"));
        }
        if (!(cultureName in $.datepicker.regional)) {
            cultureName = 'en-GB';
        }
        
        return cultureName;
    }

    self.init = function () {
        self.initializeDatepickers();
        self.setCulture("en-GB");
    };
}

var Application;

$(function () {
    Application = new ApplicationController();
    Application.init();
});
