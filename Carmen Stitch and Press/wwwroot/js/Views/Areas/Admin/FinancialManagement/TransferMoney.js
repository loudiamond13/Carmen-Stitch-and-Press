"use strict"

App.createModule("App.CSP.Area.Admin.FinancialManagement.TransferMoney", function () {

    function init() {
        $("#transferMoneyForm").validate({
            errorClass: "is-invalid", // Bootstrap class for red border
            validClass: "is-valid",
            errorPlacement: function (error) {
                toastr.error(error);
            },
            rules: {
                TransferFrom: {
                    required: true
                },
                TransferAmount: {
                    required: true
                },
                TransferTo: {
                    required: true
                }

            },
            messages: {
                TransferFrom: {
                    required: "Transfer From is required."
                },
                TransferAmount: {
                    required: "Transfer Amount is required."
                },
                TransferTo: {
                    required: "Transfer To is required."
                }
            },
            submitHandler: function (form) {
                let formData = $(form);

                $.ajax({
                    url: `/api/Admin/FinancialManagement/TransferMoney`,
                    type: "POST",
                    data: formData.serialize(),
                    beforeSend: function () {
                        $("#transferMoneyBtn").prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success) {
                        $("#addOrderItemMod").modal("hide");
                        sessionStorage.setItem("toastMessage", response.message);
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).always(function () {
                    $("#transferMoneyBtn").prop("disabled", false);

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        })
    }// end init

    return {
        init:init
    }

}, [jQuery]);