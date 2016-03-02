function OrderFormController(mode, canChangeStatus) {
    var self = this;

    var viewMode = mode;

    self.addBlankRow = function (event) {
        $.ajax({
            url: Application.Routes.Orders.AddBlankOrderItem,
            cache: false,
            success: function (html) {
                $('#orderItems tbody').append(html);
                $.validator.unobtrusive.rebindForm();
            }
        });
        return false;
    };

    self.deleteOrderItem = function (event) {
        var tableRow = $(this).parents("tr:first");
        recalculateTotalAmount(tableRow);
        tableRow.remove();
        return false;
    };

    self.onProductChanged = function (event) {
        var tableRow = $(this).parents("tr:first");
        $.getJSON(Application.Routes.Orders.GetProductDetails, { productId: $(this).val() }, function (data) {
            reloadProductData(tableRow, data);
        });
    };

    self.onQuantityOrDiscountChanged = function (event) {
        var tableRow = $(this).parents("tr:first");
        recalculateTotalAmount(tableRow);
    }

    self.init = function () {
        //disable controls depending on view mode
        if (viewMode == OrderFormController.OrderFormControllerModes.EDIT) {
            $('#OrderForm_OrderNumber').attr("readonly", "readonly");
            $('#OrderForm_Date').attr("readonly", "readonly");
        }
        else if (viewMode == OrderFormController.OrderFormControllerModes.CREATE) {
            $('#OrderForm_Status').attr("readonly", "readonly");
            $('#OrderForm_Date').addClass("datepicker date");
            Application.initializeDatepickers($('#OrderForm_Date'));
        }

        //setup add new row handler
        $('#addBlankOrderItem').on('click', self.addBlankRow);

        //setup remove rows handlers
        $('#orderItems tbody').on('click', 'a.removeOrderItem', self.deleteOrderItem);

        //setup change product handlers
        $('#orderItems tbody').on('change', 'select.productSelector', self.onProductChanged);
        $('#orderItems tbody').on('change', 'input.quantityInput', self.onQuantityOrDiscountChanged);
        $('#orderItems tbody').on('change', 'input.discountInput', self.onQuantityOrDiscountChanged);

        //init autocomplete on customers name
        initCustomerAutocomplete();

        //try to hookup custom validation
        $.validator.unobtrusive.setupBootstrapValidation($("form#OrderForm"));

        recalculateOrderTotalAmount();
    };
}

function reloadProductData(productTableRow, product) {
    productTableRow.find('input.priceInput').val(product.Price);
    productTableRow.find('input.quantityInput').val(1);
    productTableRow.find('input.discountInput').val(0);
    recalculateTotalAmount(productTableRow);
}

function recalculateTotalAmount(tableRow) {
    var price = tableRow.find('input.priceInput').val();
    var qty = tableRow.find('input.quantityInput').val();
    var discount = tableRow.find('input.discountInput').val();

    var totalBeforeDiscount = price * qty;
    var totalAmount = totalBeforeDiscount - (totalBeforeDiscount * discount / 100)
    totalAmount = Math.round(totalAmount * 100) / 100;

    tableRow.find('input.totalAmountInput').val(totalAmount);

    recalculateOrderTotalAmount();
}

function recalculateOrderTotalAmount() {
    var total = parseFloat(0);
    $('input.totalAmountInput').each(function () {
        total += parseFloat($(this).val());
    });

    $('#orderTotalAmount').html(total.toFixed(2));
}

function initCustomerAutocomplete() {
    var searchResults;

    var bestPictures = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: Application.Routes.Orders.GetCustomers + '/?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('.typeahead').typeahead(null, {
        name: 'customers',
        displayKey: 'Value',
        source: bestPictures,
        limit: 10,
        highlight: true        
    }).on('typeahead:select', function (event, suggestion) {
        $('#OrderForm_CustomerId').val(suggestion.id);
    });

    $("input.tt-hint").removeAttr("data-val data-val-required data-val-maxlength data-val-maxlength-max");
}

OrderFormController.OrderFormControllerModes = {
    CREATE: 0,
    EDIT: 1
}