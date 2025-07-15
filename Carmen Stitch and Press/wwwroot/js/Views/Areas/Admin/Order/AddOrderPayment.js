"use strict"

App.createModule("App.CSP.Area.Admin.Order.AddOrderPayment", function () {


    function init() {
        $("#addPaymentForm").validate({
            errorClass: "is-invalid", // Bootstrap class for red border
            validClass: "is-valid",   // Optional: green border when valid
            //errorPlacement: function (error, element) {
            //    if (element.is("select")) {
            //        error.insertAfter(element); // place error under the select
            //    } else {
            //        error.insertAfter(element);
            //    }
            //},
            errorPlacement: function (error) {
                toastr.error(error);
            },
            rules: {
                Amount: {
                    required: true
                },
                PayTo: {
                    required: true
                },
                PayerName: {
                    required: true
                }
            },
            messages: {
                Amount: {
                    required: "Amount is required."
                },
                PayTo: {
                    required: "Receiver is required."
                },
                PayerName: {
                    required: "Payer Name is required."
                }
            },
            submitHandler: function (form) {
                let formData = $(form);

                $.ajax({
                    url: `/api/Admin/Order/AddOrderPayment`,
                    data: formData.serialize(),
                    type: "POST",
                    beforeSend: function () {
                        $("#addOrderPaymentSubmitBtn").prop("disabled", true);
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                })
                .done(function (response) {
                    if (response.success === true) {
                        sessionStorage.setItem("toastMessage", response.message);
                        sessionStorage.setItem("editMode", "true");
                        $("#addOrderPaymentMod").modal("hide");
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).always(function () {
                    $("#addOrderPaymentSubmitBtn").prop("disabled", true);
                });
            }
        });
    }//end init

    return {
        init:init
    }

}, [jQuery]);