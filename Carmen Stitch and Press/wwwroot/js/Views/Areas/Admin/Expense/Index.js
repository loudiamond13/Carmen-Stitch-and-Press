"use strict";


App.createModule("App.CSP.Area.Admin.Expense.Index", function () {

    let expenseTbl;

    function init() {
        expenseTbl = $("#expenseTbl").DataTable({
            dom: `
                <"row mb-2"
                    <"col-12 col-md-6 mb-2 mb-md-0"l>
                    <"col-12 col-md-6 text-md-end"f>
                >
                rt
                <"row mt-2"
                    <"col-12 col-md-5"i>
                    <"col-12 col-md-7 text-md-end"p>>
              `,
            language: {
                searchPlaceholder: "Search orders...",
                lengthMenu: "Entries _MENU_ ",
            },
            ajax: { url: "/api/Admin/Expense/Index" ,dataSrc:""},
            responsive: true,
            columns: [
                { data: "orderName" },
                { data: "totalExpenses", render: $.fn.dataTable.render.number(',', '.', 2, '₱ '), width: "15%" },
                { data: "orderDate" },
                {
                    data: "orderId",
                    "render": function (data, type, row) {
                        let orderName = row.orderName;
                        return `<div class="mx-0  text-nowrap  gap-1  px-0">
                                    <button id="${data}" class="btn text-center addOrderExpenseBtn text-nowrap btn-primary">
                                    <i class="bi bi-database-add"></i>
                                        <span class="d-none d-md-inline"> Add Expense</span>
                                    </button>
                                    <button id="${orderName}" class="btn text-center viewOrderExpenseBtn text-nowrap btn-secondary">
                                        <i class="bi bi-eye"></i>
                                        <span class="d-none d-md-inline"> View Expenses</span>
                                    </button>
                               </div>`
                    }
                },
            ],
            columnDefs: [
                { className: "text-center px-1 mx-0", targets: "_all" },
                { className: "text-center justify-content-center d-flex  text-nowrap px-0 mx-0", targets: [2] },
                {targets:2 ,visible:false,searchable:false}
            ],
            rowReorder: {
                selector: 'td:nth-child(2)'
            },
            order: [[2, 'desc']]
        }); //

        //add order expense btn
        $("#expenseTbl").on("click", ".addOrderExpenseBtn", function () {
            let btn = $(this);
            let row = btn.closest("tr");
            let data = expenseTbl.row(row).data();
            let orderId = data["orderId"];

            if (orderId != undefined) {
                $.ajax({
                    url: `/Admin/Expense/AddOrderExpense/${orderId}`,
                    type: "GET",
                    beforeSend: function () {
                        btn.prop("disabled", true);
                    }
                }).done(function (response) {
                    if (response.success === false) {
                        toastr.error(response.message);
                    }
                    $("#addOrderExpenseModal").html(response);
                    $("#addOrderExpenseModal").children(".modal").first().modal("show");
                    //init js
                    App.CSP.Area.Admin.Expense.AddOrderExpense.init();

                }).always(function () {
                    btn.prop("disabled", false);
                });
            }
        }); //end add expense button

        // view order expense button
        $("#expenseTbl").off("click", ".viewOrderExpenseBtn").on("click", ".viewOrderExpenseBtn", function () {
            let btn = $(this);
            let row = btn.closest("tr");
            let data = expenseTbl.row(row).data();
            let orderId = data["orderId"];
            let orderName = data["orderName"]

            if (orderId != undefined) {
                window.location.href = `/Admin/Expense/ViewOrderExpenses?orderId=${orderId}&orderName=${orderName}`;

            }

        });// end view order expense button



        // company expenses btn
        $("#addCompanyExpensesBtn").on("click", function () {
            let btn = $(this);

            $.ajax({
                url: '/Admin/Expense/AddCompanyExpense',
                type: "GET",
                beforeSend: function () {
                    btn.prop("disabled", true);
                }
            }).done(function (response) {
                if (response.success === false) {
                    toastr.error(response.message);
                }

                $("#addCompanyExpenseModal").html(response);
                $("#addCompanyExpenseModal").children(".modal").first().modal("show");
                //initialize js
                App.CSP.Area.Admin.Expense.AddCompanyExpense.init();

            }).always(function () {
                btn.prop("disabled", false);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR);
                console.error("Error occurred:", textStatus, errorThrown);
            });
        });// end add company expense btn

        //view company expense btn
        $("#viewCompanyExpensesBtn").off("click").on("click", function () {
            window.location.href = `/Admin/Expense/ViewCompanyExpense`;
        });

    }// end init

    //reload
    function reloadGrid() {
        expenseTbl.ajax.reload();
    }
    //end reload


    return {
        init: init,
        reloadGrid:reloadGrid
    }
}, [jQuery]);

$(function () {
    App.CSP.Area.Admin.Expense.Index.init();
});