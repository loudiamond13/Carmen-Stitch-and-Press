"use strict"

App.createModule("App.CSP.Area.Admin.Order.EditOrderItem", function () {

    function init() {

        $("#editOrderItemForm").validate({
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
                    url: `/api/Admin/Order/EditOrderItem`,
                    type: "POST",
                    data: formData.serialize(),
                    beforeSend: function () {
                        $("#editOrderItemBtn").prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success) {
                        $("#editOrderItemMod").modal("hide");
                        sessionStorage.setItem("toastMessage", response.message);
                        sessionStorage.setItem("editMode", "true");
                        window.location.reload();
                    }

                    toastr.error(response.message);
                }).always(function () {
                    $("#editOrderItemBtn").prop("disabled", false);

                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        });

        //finish btn
        $(".itemIsDoneBtn").on("click", function () {
            let btn = $(this);
            let isDone = btn.data("done");
            let itemId = btn.data("id");

            $.ajax({
                url: `/api/Admin/Order/DoneUndoneOrderItem?itemId=${itemId}&isDone=${isDone}`,
                "type": "POST",
                beforeSend: function () {
                    btn.prop("disabled", true);
                }
            }).done(function (res) {
                if (res.success === true) {
                    $("#editOrderItemMod").modal("hide");
                    sessionStorage.setItem("toastMessage", res.message);
                    sessionStorage.setItem("editMode", "true");
                    window.location.reload();
                }
                else {
                    toastr.error(res.message)
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.error("Error occurred:", textStatus, errorThrown);
            }).always(function () {
                btn.prop("disabled", false);

            });

        });

    }//end init

    return {
        init:init
    }
}, [jQuery]);