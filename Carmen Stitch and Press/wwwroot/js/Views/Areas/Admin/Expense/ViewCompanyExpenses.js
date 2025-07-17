"Use strict"

App.createModule("App.CSP.Area.Admin.Expense.ViewCompanyExpense", function () {

    let companyExpenseTbl;

    function init() {

        companyExpenseTbl = $("#companyExpensesTbl").DataTable({
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
            ajax: {
                url: "/api/Admin/Expense/CompanyExpenses",
                dataSrc: "",
                error: function (xhr, error, thrown) {
                    console.error("DataTable AJAX Error:", {
                        status: xhr.status,
                        statusText: xhr.statusText,
                        responseText: xhr.responseText,
                        error: error,
                        thrown: thrown
                    });
                }
            },
            responsive: true,
            columns: [
             //   { data: 'distinctYears', visible:false},
                { data: "spendDateString" },
                { data: "description" },
                { data: "amount", render: $.fn.dataTable.render.number(',', '.', 2, '₱ '),/* width: "15%" */ },
                {
                    data: "expensesId",
                    "render": function (data) {
                        return `<div class="mx-0  text-nowrap  gap-1  px-0">
                                    <button id="${data}" class="btn text-center deleteCompanyExpense text-nowrap btn-danger text-white">
                                        <i class="bi bi-trash"></i>
                                        <span class="d-none d-md-inline">Delete</span>
                                    </button>
                               </div>`
                    }
                },
            ],
            columnDefs: [
                { className: "text-center px-1 mx-0", targets: "_all" },
                { type: "date", targets: [0] }
                //{ className: "text-center justify-content-center d-flex  text-nowrap px-0 mx-0", targets: [2] },
            ],
            rowReorder: {
                selector: 'td:nth-child(2)'
            },
            order: [[0, 'desc']],
            footerCallback: function (row, data, start, end, display) {
                var api = this.api();

                // Total over all pages
                var total = api
                    .column(2)
                    .data()
                    .reduce(function (a, b) {
                        return parseFloat(a) + parseFloat(b || 0);
                    }, 0);


                // Format as 0,000 (no decimals)
                var formattedTotal = total.toLocaleString(undefined, {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });

                // Update footer
                $(api.column(2).footer()).html(
                    `₱ ${formattedTotal}`
                );
            },
            initComplete: function () {
                let currentYear = $("#viewCompanyYearSelect").val();//new Date().getFullYear();
                this.api().column(0).search(currentYear).draw();
            }
        });// end datatable

        $("#viewCompanyYearSelect").off("change").on("change", function () {
            let selectedYr = $(this).val();
            companyExpenseTbl.column(0).search(selectedYr).draw();
        });

        //add company expense btn
        $(".addCompanyExpense").off("click").on("click", function () {
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

        });


        //delete company expense btn
        $("#companyExpensesTbl").off("click", ".deleteCompanyExpense").on("click", ".deleteCompanyExpense", function () {
            const btn = $(this);
            const row = btn.closest("tr");
            const data = companyExpenseTbl.row(row).data();
            const description = data["description"];
            const expenseId = data["expensesId"];

            if (expenseId !== undefined) {
                App.CSP.Site.DeleteModal(`/api/Admin/Expense/DeleteOrderExpense/${expenseId}`, `${description} expense`,false, function () {
                    reloadGrid();
                    //window.location.reload();
                });
            }

        });//end delete company expense btn
    }// end init

    function reloadGrid() {
        companyExpenseTbl.ajax.reload();
    }

    return {
        init: init,
        reloadGrid: reloadGrid
    }

});

$(function () {
    App.CSP.Area.Admin.Expense.ViewCompanyExpense.init();
});