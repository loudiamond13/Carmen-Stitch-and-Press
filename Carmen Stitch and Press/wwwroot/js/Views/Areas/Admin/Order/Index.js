"use strict"


App.createModule("App.CSP.Area.Admin.Order.Index", function () {
    let orderTbl;
    let tblInitialized = false;
    function init() {
        orderTbl = $("#orderTbl").DataTable({
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
            responsive: true,
            ajax: { url: "/api/Admin/Order/Index", dataSrc: '' },
            columns: [
                { data:"orderName"},
                { data: "paidAmount", render: $.fn.dataTable.render.number(',', '.', 0, '₱ ') },
                { data: "totalBalance", render: $.fn.dataTable.render.number(',', '.', 0, '₱ ') },
                { data: "totalAmount", render: $.fn.dataTable.render.number(',', '.', 0, '₱ ') }, //string formattedPhp = paidAmt.ToString("C", CultureInfo.GetCultureInfo("fil-PH")); // "₱1,234.56"
                { data:"isOpen", visible:false},
                {
                    data: "orderId",
                    "render": function (data) {
                      return   `<div class="m-0 p-0">
                                    <button id="${data}" class="btn text-center viewBtn text-nowrap btn-primary"><i class="bi bi-eye"></i> View</button>
                               </div>`
                    }
                }
            ],
            columnDefs: [
                { className: "text-center mx-0 px-0", targets: [1]},
                { className: "text-center mx-0 px-0", targets: [2]},
                {className: "text-center", targets: [3]},
                { className: "text-md-center", targets: [5]},
            ],
            rowReorder: {
                selector: 'td:nth-child(2)'
            },
            createdRow: function (row,data) {
                if (data.isOpen === false) {
                    $(row).addClass("table-danger")
                }
            },
            order: [[2, 'desc']],
            initComplete: function () {
                this.api().column(4).search("true").draw();
            }
        }); //datatable end

        //filter select listener
        $("#orderFilter").on("change", function () {
            const value = $(this).val();
            orderTbl.column(4).search(value).draw();
        });

        //view order button
        $("#orderTbl").on("click", ".viewBtn", function (e) {
            e.preventDefault();
            let btn = $(this);
            let row = btn.closest("tr");
            const data = orderTbl.row(row).data();
            let orderId = data["orderId"];


           
            window.location.href = `/Admin/Order/View/${orderId}`;
            App.CSP.Area.Admin.Order.View.init();
        });//view order button end



    };//end init

    return {
        init:init
    }

}, [jQuery]);


$(function () {
    let selectedVal = $("#orderFilter").val();
    var asd = "asd";
    App.CSP.Area.Admin.Order.Index.init();

});