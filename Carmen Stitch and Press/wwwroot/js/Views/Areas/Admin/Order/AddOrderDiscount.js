"use strict"

App.createModule("App.CSP.Area.Admin.Order.AddOrderDiscount", function () {

    function init() {

        $("#addDiscountForm").validate({
            errorClass: "is-invalid", // Bootstrap class for red border
            validClass: "is-valid",
            errorPlacement: function (error) {
                toastr.error(error);
            },
            rules: {
                Description: {
                    required: true
                },
                Amount: {
                    required: true
                }
            },
            messages: {
                Description: {
                    required: "Discount Description is required"
                },
                Amount: {
                    required: "Discount Amount is required"
                }
            },
            submitHandler: function (form) {
                let formData = $(form);

                $.ajax({
                    url: `/api/Admin/Order/AddOrderDiscount`,
                    data: formData.serialize(),
                    type: "POST",
                    beforeSend: function () {
                        $("#addOrderDiscountSubmitBtn").prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success) {
                        sessionStorage.setItem("toastMessage", response.message);
                        sessionStorage.setItem("editMode", "true");
                        $("#addOrderPaymentMod").modal("hide");
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                }).always(function () {
                    $("#addOrderDiscountSubmitBtn").prop("disabled", true);
                });
            }
        });
    } // end init

    return {
        init:init
    }

}, [jQuery])