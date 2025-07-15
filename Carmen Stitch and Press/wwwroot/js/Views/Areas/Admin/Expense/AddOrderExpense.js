"use strict"

App.createModule("App.CSP.Area.Admin.Expense.AddOrderExpense", function () {

    function init() {

        $("#addOrderExpenseForm").validate({
            validClass: "is-valid",
            errorClass: "is-invalid",
            errorPlacement: function (error) {
                toastr.error(error);
            },
            rules: {
                Amount: {
                    required: true
                },
                Description: {
                    required: true
                },
                PaidBy: {
                    required: true
                }
            },
            messages: {
                Amount: {
                    required: "Amount is required."
                },
                Description: {
                    required: "Description is required."
                },
                PaidBy: {
                    required: "Paid By is required."
                }
            },
            submitHandler: function (form) {
                let formData = $(form);

                $.ajax({
                    url: `/api/Admin/Expense/AddOrderExpense`,
                    data: formData.serialize(),
                    type: "POST",
                    beforeSend: function () {
                        $("#addOrderExpenseSubmitBtn").prop("disabled", true);
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR);
                    console.error("Error occurred:", textStatus, errorThrown);
                })
                .done(function (response) {
                    if (response.success === true) {
                        toastr.success(response.message);

                        let currentLocation = window.location.href;

                        if (window.location.href.includes("ViewOrderExpenses")) {
                            App.CSP.Area.Admin.Expense.ViewOrderExpenses.reloadGrid();
                        }
                        else{
                            App.CSP.Area.Admin.Expense.Index.reloadGrid();
                        }

                        $("#addOrderExpenseMod").modal("hide");
                        //sessionStorage.setItem("toastMessage", response.message);
                        //sessionStorage.setItem("editMode", "true");
                        //$("#addOrderExpenseMod").modal("hide");
                        //window.location.reload();
                    }

                    else {
                        toastr.error(response.message);
                    }
                }).always(function () {
                    $("#addOrderExpenseSubmitBtn").prop("disabled", true);
                });
            }

        });


    }  // end init

    return {
        init:init
    }

},[jQuery])