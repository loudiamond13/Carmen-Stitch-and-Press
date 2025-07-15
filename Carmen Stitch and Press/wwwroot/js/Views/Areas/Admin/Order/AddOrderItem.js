"use strict"

App.createModule("App.CSP.Area.Admin.Order.AddOrderItem", function () {
    function init() {
        $("#addItemForm").validate({
            errorClass: "is-invalid", // Bootstrap class for red border
            validClass: "is-valid",
            errorPlacement: function (error) {
                toastr.error(error);
            },
            rules: {
                Description: {
                    required: true
                },
                Quantity: {
                    required: true
                },
                Price: {
                    required: true
                }

            },
            messages: {
                Description: {
                    required: "Item Description is required."
                },
                Quantity: {
                    required: "Quantity is required."
                },
                Price: {
                    required: "Price is required."
                }
            },
            submitHandler: function (form) {
                let formData = $(form);

                $.ajax({
                    url: `/api/Admin/Order/AddOrderItem`,
                    type: "POST",
                    data: formData.serialize(),
                    beforeSend: function () {
                        $("#addOrderItemBtn").prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success) {
                        $("#addOrderItemMod").modal("hide");
                        sessionStorage.setItem("toastMessage", response.message);
                        sessionStorage.setItem("editMode", "true");
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).always(function () {
                    $("#addOrderItemBtn").prop("disabled", false);

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        }); //end form validate
    }

    return {
        init: init
    }
}, [jQuery]);