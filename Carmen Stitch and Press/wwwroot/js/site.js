"use strict"

App.createModule("App.CSP.Site", function () {
    function init() {
        $(function () {
            const msg = sessionStorage.getItem("toastMessage");
            if (msg) {
                toastr.success(msg);
                sessionStorage.removeItem("toastMessage");
            }
        });

        $('.menu').off("click").on("click", function () {
            $(this).toggleClass('active');
        });

    }//end init

    //delete modal
    function DeleteModal(url, name, reloadPage,jsSyntax) {
        Swal.fire({
            title: `Are you sure you want to remove, ${name}?`,
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.isConfirmed) {
                $.ajax({
                    url: url,
                    type: "DELETE",
                }).done(function (response) {
                    if (response.success === true) {

                        //if reaload page, set the toaster message to the session storage, or else it will not show
                        if (reloadPage) {
                            sessionStorage.setItem("toastMessage", response.message);
                        }
                        //else show toastr directly here if it is not reloading page
                        else {
                            toastr.success(response.message);
                        }

                        if (typeof jsSyntax === "function") {
                            jsSyntax();
                        }
                    }
                    else {
                        toastr.error(response.message);
                    }
                });
            }

        });


    }

    return {
        init: init,
        DeleteModal:DeleteModal
    }
}, [jQuery])

$(function () {
    App.CSP.Site.init();

});
