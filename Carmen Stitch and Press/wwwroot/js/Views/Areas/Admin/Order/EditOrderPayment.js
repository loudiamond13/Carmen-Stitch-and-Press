"use strict"

App.createModule("App.CSP.Area.Admin.Order.EditOrderPayment", function () {


    function init() {

        $("#editOrderPaymentForm").validate({
            errorClass: "is-invalid", //bootstrap class for red border
            validClass: "is-valid",   //optional: green border when valid
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
                    url: `/api/Admin/Order/EditOrderPayment`,
                    data: formData.serialize(),
                    type: "POST",
                    beforeSend: function () {
                        $("#editOrderPaymentSubmitBtn").prop("disabled", true);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                }).done(function (response) {
                    if (response.success === true) {
                        sessionStorage.setItem("toastMessage", response.message);
                        sessionStorage.setItem("editMode", "true");
                        $("#editOrderPaymentMod").modal("hide");
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).always(function () {
                    $("#editOrderPaymentSubmitBtn").prop("disabled", false);
                });
            }

        });//end validate
    }

    return {
        init:init
    }
},[jQuery])