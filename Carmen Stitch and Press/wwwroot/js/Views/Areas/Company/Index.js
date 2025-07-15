"use strict"

App.createModule("App.CSP.Area.Company.User.Index", function () {
    let userTbl;

    function init() {
        userTbl = $("#userTable").DataTable({
            responsive: true,
            ajax: { url: "/api/Company/user/index", dataSrc:'' },
            columns: [
                { data: 'fullName', "width": "15%" },
                { data: 'email', "width": "15%" },
                {data:"role", "width": "15%"},
                {
                    data:
                    {
                        id: "id", lockoutEnd: "lockoutEnd"
                    },
                    "render": function (data) {
                        var today = new Date().getTime();
                        var lockoout = new Date(data.lockoutEnd).getTime();
                        if (lockoout > today) {
                            //user is currently locked
                            return `<div class="w-75 btn-group" role="group"> 
                                    <a onClick=UnlocklLock('${data.id}') class="btn btn-danger mx-2"> <i class="fa-solid fa-lock-open"></i> Unlock</a>
                                </div>`;
                        }
                        else {
                            return `<div class="w-75 btn-group" role="group"> 
                                    <a onClick=UnlocklLock('${data.id}') class="btn btn-success mx-2"><i class="fa-solid fa-lock"></i> Lock</a>
                                </div>`;
                        }
                    }, "width": "15%"
                    //return `<div class="w-75 btn-group" role="group">
                    //<a href="/admin/product/addedit?id=${data}" class="btn btn-primary mx-2"><i class="fa-solid fa-pencil"></i>Edit</a>
                    //  <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="fa-regular fa-trash-can"></i>Delete</a>
                    //</div>`
                    //}, "width": "20%"

                }
            ],
            columnDefs: [
                { className: " text-nowrap", targets: "_all" },
                { className: "text-center", targets: [1] },
                { className: "text-sm-center", targets: [2] },
                { className: "text-md-center", targets: [3] },

            ],
            rowReorder: {
                selector: 'td:nth-child(2)'
            }
            
            
        });

        window.UnlocklLock = function(id) {
            $.ajax({
                url: '/api/Company/User/UnlockLock/' + id,
                type: 'POST',
                //data: JSON.stringify(id),
                //contentType: 'application/json',
                success: function (data) {
                    if (data.success === true) {
                        toastr.success(data.message);
                        userTbl.ajax.reload();
                    }
                    else
                    {
                        toastr.error(data.message);
                    }
                }
            });
        }
    };//end init



    return {
        init: init
    };
}, [jQuery]);

  
    
$(function () {
    App.CSP.Area.Company.User.Index.init();
});