/*!
** Unobtrusive validation extensions
*/
$.extend($.validator.unobtrusive, {

    rebindForm: function () {
        var $form = $("form"); 
        $form.removeData("validator");
        $form.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("form");
        this.setupBootstrapValidation($form);
    },
    escapeAttributeValue: function (value) {
        // As mentioned on http://api.jquery.com/category/selectors/
        return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
    },
    setupBootstrapValidation: function (formElement) {
        formElement.data('validator').settings.errorPlacement = function (error, inputElement) {
            var container = $("form").find("[data-valmsg-for='" + $.validator.unobtrusive.escapeAttributeValue(inputElement[0].name) + "']"),
                replaceAttrValue = container.attr("data-valmsg-replace"),
                replace = replaceAttrValue ? $.parseJSON(replaceAttrValue) !== false : null;

            container.removeClass("field-validation-valid").addClass("field-validation-error");
            error.data("unobtrusiveContainer", container);

            //if (replace) {
            //    container.empty();
            //    error.removeClass("input-validation-error").appendco To(container);
            //}
            //else {
            //    error.hide();
            //}
            error.hide();
        };

        formElement.data('validator').settings.highlight = function (element, errorClass, validClass) { //
            $(element).closest('.control-group').addClass('error');            
        };

        formElement.data('validator').settings.unhighlight = function (element, errorClass, validClass) {
            $(element).closest('.control-group').removeClass('error');
        };

        formElement.unbind("invalid-form.validate");
        formElement.bind("invalid-form.validate", function (event, validator) {
            var container = $("form").find("[data-valmsg-summary=true]"),
                list = container.find("ul");

            if (list && list.length && validator.errorList.length) {
                list.empty();
                container.addClass("validation-summary-errors").removeClass("validation-summary-valid");
                container.addClass("alert alert-error");

                if (container.find("span#validationSummaryHeader").length == 0) {
                    container.prepend("<span class='alert-heading' id='validationSummaryHeader'><strong>" + Application.messages.validationErrorsDivHeader + "</strong></span>");
                }

                $.each(validator.errorList, function () {
                    $("<li />").html(this.message).appendTo(list);
                });
            }
        });

        formElement.unbind("reset.unobtrusiveValidation");
        formElement.bind("reset.unobtrusiveValidation", function (event) {
            var $form = $("form");
            $form.data("validator").resetForm();
            $form.find(".validation-summary-errors")
                .addClass("validation-summary-valid")
                .removeClass("validation-summary-errors")
                .removeClass("alert alert-error")
                .remove("span#validationSummaryHeader");
            $form.find(".field-validation-error")
                .addClass("field-validation-valid")
                .removeClass("field-validation-error")
                .removeData("unobtrusiveContainer")
                .find(">*")  // If we were using valmsg-replace, get the underlying error
                    .removeData("unobtrusiveContainer");
        });

        //localize number validation
        $.validator.methods.number = function (value, element) {
            if (Globalize.parseFloat(value) !== NaN  ) {
                return true;
            }
            return false;
        };
        //localize date validation
        $.validator.methods.date = function (value, element) {
            if (Globalize.parseDate(value)!== null) {
                return true;
            }
            return false;
        };
    }
});
