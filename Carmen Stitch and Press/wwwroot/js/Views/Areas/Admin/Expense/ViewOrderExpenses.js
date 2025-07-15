"use strict"

App.createModule("App.CSP.Area.Admin.Expense.ViewOrderExpenses", function () {

    let expensesTbl;
    function init(orderId) {
        expensesTbl = $("#expensesTbl").DataTable({
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
            ajax: { url: "/api/Admin/Expense/OrderExpenses/" + orderId, dataSrc: "" },
            responsive: true,
            columns: [
                { data: "spendDateString" },
                { data: "description" },
                { data: "amount", render: $.fn.dataTable.render.number(',', '.', 2, '₱ '),/* width: "15%" */},
                {
                    data: "expensesId",
                    "render": function (data) {
                        return `<div class="mx-0  text-nowrap   px-0">
                                    <button id="${data}" class="btn text-center deleteExpenseBtn text-nowrap btn-danger text-white">
                                        <i class="bi bi-trash"></i>
                                        <span class="d-none d-md-inline"> Delete</span>
                                    </button>
                               </div>`
                    }
                },
            ],
            columnDefs: [
                { className: "text-center px-1 mx-0", targets: "_all" },
                {type:"date", targets:[0]}
                //{ className: "text-center justify-content-center d-flex  text-nowrap px-0 mx-0", targets: [2] },
            ],
            rowReorder: {
                selector: 'td:nth-child(2)'
            },
            order: [[0, 'desc']],
            footerCallback: function (row, data, start, end, display) {
                var api = this.api();

                //total over all pages
                var total = api
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return parseFloat(a) + parseFloat(b || 0);
                    }, 0);


                // Format as 0,000.00
                var formattedTotal = total.toLocaleString(undefined, {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });

                // Update footer
                $(api.column(2).footer()).html(
                    `₱ ${formattedTotal}`
                );
            }
        });
        
        //add order expense btn on top of the expense tbl
        $(".addOrderExpenseBtn").off("click").on("click", function () {
            let btn = $(this);
            let orderId = btn.attr("id");


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
        }); ///end add order expense btn on top of the expense tbl

        $("#expensesTbl").off("click").on("click", ".deleteExpenseBtn", function () {
            const btn = $(this);
            const row = btn.closest("tr");
            const data = expensesTbl.row(row).data();
            const description = data["description"];
            const expenseId = data["expensesId"];

            if (expenseId !== undefined) {
                App.CSP.Site.DeleteModal(`/api/Admin/Expense/DeleteOrderExpense/${expenseId}`, `${description} expense`,false, function () {
                    reloadGrid();
                    //window.location.reload();
                });
            }
        });

    }//end inittt

    function reloadGrid() {
        expensesTbl.ajax.reload();
    }


    return {
        init: init,
        reloadGrid: reloadGrid
    }



}, [jQuery]);

$(function () {
    var orderId = $('#orderId').val();
    App.CSP.Area.Admin.Expense.ViewOrderExpenses.init(orderId)
});