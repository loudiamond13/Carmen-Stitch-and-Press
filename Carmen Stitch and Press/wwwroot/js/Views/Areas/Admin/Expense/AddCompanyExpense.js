"use strict"

App.createModule("App.CSP.Area.Admin.Expense.AddCompanyExpense", function () {

    function init() {
        $("#addCompanyExpenseForm").validate({
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
                    url: `/api/Admin/Expense/AddCompanyExpense`,
                    data: formData.serialize(),
                    type: "POST",
                    beforeSend: function () {
                        $("#addCompanyExpenseSubmitBtn").prop("disabled", true);
                    }
                })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        console.log(jqXHR);
                        console.error("Error occurred:", textStatus, errorThrown);
                    })
                    .done(function (response) {
                        if (response.success === true) {
                            toastr.success(response.message);
                            $("#addCompanyExpenseMod").modal("hide");
                            if (window.location.href.includes("ViewCompanyExpense")) {

                                App.CSP.Area.Admin.Expense.ViewCompanyExpense.reloadGrid();
                            }
                        }

                        else {
                            toastr.error(response.message);
                        }
                    }).always(function () {
                        $("#addCompanyExpenseSubmitBtn").prop("disabled", true);
                    });
            }

        });
    }//end init

    return {
        init:init
    }
},[jQuery])