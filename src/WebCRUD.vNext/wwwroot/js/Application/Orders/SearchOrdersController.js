function SearchOrdersController() {
    var self = this;

    self.init = function () {
        $('#OrderSearchForm').on('submit', self.executeSearch);

        $(document).on('click', 'button.approveOrder', self.approveOrder);
        $(document).on('click', 'button.closeOrder', self.closeOrder);
    }

    self.executeSearch = function (evt) {
        evt.preventDefault();
        evt.returnValue = false;
        var $form = $(evt.target);
        $.post($form.attr('action'), $form.serialize(), function (data) {
            $('#orderSearchResults').html(data);
        });
    }

    self.approveOrder = function (e) {
        var $button = $(e.target);
        var $orderRow = $button.parents("tr:first");
        var orderId = $orderRow.attr("data-order-id");
        var orderNumber = $orderRow.attr("data-order-number");

        e.preventDefault();

        Application.showConfirmation(
            Application.messages.orderApproveConfirmationText.replace("{0}", orderNumber),
            Application.messages.orderApproveConfirmationTitle,
            function () {
                $.post(Application.Routes.Orders.Approve, { orderId: orderId }, function (data) {
                    $orderRow.replaceWith(data);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Application.showError(errorThrown, Application.messages.orderApproveFailedTitle);
                });
            });
    };

    self.closeOrder = function (e) {
        var $button = $(e.target);
        var $orderRow = $button.parents("tr:first");
        var orderId = $orderRow.attr("data-order-id");
        var orderNumber = $orderRow.attr("data-order-number");

        e.preventDefault();

        Application.showConfirmation(
            Application.messages.orderCloseConfirmationText.replace("{0}", orderNumber),
            Application.messages.orderCloseConfirmationTitle,
            function () {
                $.post(Application.Routes.Orders.Close, { orderId: orderId },
                    function (data) {
                        $orderRow.replaceWith(data);
                    })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    Application.showError(errorThrown, Application.messages.orderCloseFailedTitle);
                });
            });
    };
}