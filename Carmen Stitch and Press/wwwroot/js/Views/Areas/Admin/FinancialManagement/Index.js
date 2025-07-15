"use strict"

App.createModule("App.CSP.Area.Admin.FinancialManagement.Index", function () {

    function init() {
        $("#transferMoneyBtn").on("click", function () {
            let btn = $(this);
            $.ajax({
                url: `/Admin/FinancialManagement/TransferMoney`,
                type: "GET",
                beforeSend: function () {
                    btn.prop("disabled", true);
                }
            }).done(function (response) {
                if (response.success === false) {
                    toastr.error(response.message);
                }
                else {
                    $("#transferMoneyModal").html(response);
                    $("#transferMoneyModal").children(".modal").first().modal("show");
                    //initialize js
                    App.CSP.Area.Admin.FinancialManagement.TransferMoney.init();
                }

            }).always(function () {
                btn.prop("disabled", false);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.error("Error occurred:", textStatus, errorThrown);
            })
        });


    }// end init 

    return {
        init:init
    }
});

$(function () {

    App.CSP.Area.Admin.FinancialManagement.Index.init();
});