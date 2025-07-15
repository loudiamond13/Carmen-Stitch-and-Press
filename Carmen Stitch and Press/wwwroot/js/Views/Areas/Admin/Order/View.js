"use strict"

App.createModule("App.CSP.Area.Admin.Order.View", function () {

    function init() {
   
        $("#editOrderBtn").on("click", function (e) {
            e.preventDefault();
            enableEditMode();
        });

        //add item btn
        $("#addItemBtn").off("click").on("click", function () {
            let btn = $(this);
            const orderId = btn.data("index");
            
            if (orderId !== 0 || orderId !== null || orderId !== "undefined")
            {
                $.ajax({
                    url: `/Admin/Order/AddOrderItem/${orderId}`,
                    type: "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success == false) {
                        toastr.error(response.message);
                    }
                    $("#addOrderItemModal").html(response);
                    $("#addOrderItemModal").children(".modal").first().modal("show");
                
                    App.CSP.Area.Admin.Order.AddOrderItem.init();
                }).always(function () {
                    btn.prop("disabled", false);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        });// end add item

        // remove order item btn
        $(".removeItemBtn").on("click", function () {
            let btn = $(this);
            const orderItemId = btn.attr("id");
            const orderDesc = btn.attr("name");
            App.CSP.Site.DeleteModal(`/api/Admin/Order/RemoveOrderItem/${orderItemId}`, orderDesc,true, function () {
                sessionStorage.setItem("editMode", "true");
                window.location.reload();
            });
        }); // remove order item btn

        //add payment btn
        $("#addPaymenBtn").off("click").on("click", function () {
            let btn = $(this);
            const orderId = btn.data("index");

            if (orderId !== 0 || orderId !== null || orderId !== "undefined") {
                $.ajax({
                    url: `/Admin/Order/AddOrderPayment/${orderId}`,
                    type: "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success === false) {
                        toastr.error(response.message)
                    }

                    $("#addOrderPaymentModal").html(response);
                    $("#addOrderPaymentModal").children(".modal").first().modal("show");
                    //initialize js
                    App.CSP.Area.Admin.Order.AddOrderPayment.init();
                }).always(function () {
                    btn.prop("disabled", false);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        });//end add payment btn

        //remove payment btn listener
        $(".removePaymentBtn").off("click").on("click", function () {
            const btn = $(this)
            let paymentId = btn.attr("id");
            let payerName = btn.data("name");

            App.CSP.Site.DeleteModal(`/api/Admin/Order/RemoveOrderPayment/${paymentId}`, `${payerName}'s payment`,true, function () {
                sessionStorage.setItem("editMode", "true");
                window.location.reload();
            });

        });//end remove payment btn listener

        //add discount btn
        $("#addDiscountBtn").off("click").on("click", function () {
            const btn = $(this);
            let orderId = btn.data('index');

            if (orderId !== 0 || orderId !== null || orderId !== "undefined") { 
                $.ajax({
                    url: `/Admin/Order/AddOrderDiscount/${orderId}`,
                    type: "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success === false) {
                        toastr.error(response.message)
                    }
                    $("#addOrderDiscountModal").html(response);
                    $("#addOrderDiscountModal").children(".modal").first().modal("show");

                    //initiate js
                    App.CSP.Area.Admin.Order.AddOrderDiscount.init();
                }).always(function () {
                    btn.prop("disabled", false);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }
        });//end add discount btn

        //edit order payment
        $(".editPayment").off("click").on("click", function () {
            let btn = $(this);
            let paymentId = btn.attr("id");

            if (paymentId !== 0 || paymentId !== null || paymentId !== "undefined") {
                $.ajax({
                    url: "/Admin/Order/EditOrderPayment/" + paymentId,
                    type: "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response === false) {
                        toastr.error(response.message);
                    }
                    else {
                        $("#editOrderPaymenModal").html(response);
                        $("#editOrderPaymenModal").children(".modal").first().modal("show");
                        App.CSP.Area.Admin.Order.EditOrderPayment.init();
                    }

                }).always(function () {
                    btn.prop("disabled", true);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });


            }
        });//end edit order


        //remove discount btn
        $(".removeDiscBtn").on("click", function () {
            let btn = $(this);
            let discId = btn.attr("id");
            let desc = btn.data("desc");

            App.CSP.Site.DeleteModal(`/api/Admin/Order/RemoveDiscount/${discId}`, `${desc} discount`,true, function () {
                sessionStorage.setItem("editMode", "true");
                window.location.reload();
            });
        });//end remove discount btn


        //************************** EDIT         *********************************/
        $(".editItemBtn").on("click", function () {
            let btn = $(this);
            let itemId = btn.attr("id");

            if (itemId !== 0 || itemId !== null || itemId !== "undefined") {
                $.ajax({
                    url: "/Admin/Order/EditOrderItem/" + itemId,
                    "type": "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (res) {
                    if (res.success === false) {
                        toastr.error(res.message);
                    }
                    $("#editOrderItemModal").html(res);
                    $("#editOrderItemModal").children(".modal").first().modal("show");
                    //initialize js
                    App.CSP.Area.Admin.Order.EditOrderItem.init();
                }).always(function () {
                    btn.prop("disabled", false);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    console.error(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                });
            }

        });

        //close order btn
        $(".closeOrderBtn").on("click", function () {
            let btn = $(this);
            let orderId = btn.data("id");
            let isOpen = btn.data("isopen");
            let closeOpen = "";
            if (isOpen === "True") {
                closeOpen = "close";
            } else {
                closeOpen = "open";
            }

            if (orderId !== undefined) {
                Swal.fire({
                    title: `Are you sure you want to ${closeOpen} this order?`,
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, close it!'
                }).then(function (result) {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: `/api/Admin/Order/CloseOpenOrder?orderId=${orderId}&isOpen=${isOpen}`,
                            type: "POST",
                        }).done(function (response) {
                            if (response.success === true) {
                                sessionStorage.setItem("toastMessage", response.message);
                                if (response.message === "Opened the order successfully.") {
                                    window.location.reload();
                                }
                                else {
                                    window.location.href = "/Admin/Order/Index"
                                }
                            }
                            else {
                                toastr.error(response.message);
                            }
                        });
                    }

                });
            }
        });//end close order btn






        //edit mode
        const isEditMode = sessionStorage.getItem("editMode");
        if (isEditMode === "true") {
            enableEditMode();
            sessionStorage.removeItem("editMode");
        }

        function enableEditMode() {
         /*   $("input").removeAttr("readonly");*/
            $("button").removeClass("d-none");
            $("#editOrderBtn").fadeOut("fast");
/*            $("select").removeAttr("disabled");*/
            $("#backEditBtn").removeClass("d-none");
        }
    };//end init



   

    return {
        init: init
    }

}, [jQuery]);


$(function () {
    App.CSP.Area.Admin.Order.View.init();
  
})