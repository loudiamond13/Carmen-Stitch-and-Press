"use strict"

App.createModule("App.CSP.Area.Admin.FinancialManagement.TransferTransactions", function () {

    let transferTransactionsTbl;
    function init() {
        transferTransactionsTbl = $("#transferTransacionsTbl").DataTable({
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
                url: "/api/Admin/FinancialManagement/TransferTransactions",
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
                { data: "transferDateString" },
                { data: "transferFrom" },
                { data: "transferTo" },
                { data: "transferAmount", render: $.fn.dataTable.render.number(',', '.', 2, '₱ '),/* width: "15%" */ },
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
            

        });//end data table

    }// end init

    return {
        init :init
    }

}, [jQuery])

$(function () {
    App.CSP.Area.Admin.FinancialManagement.TransferTransactions.init();
});