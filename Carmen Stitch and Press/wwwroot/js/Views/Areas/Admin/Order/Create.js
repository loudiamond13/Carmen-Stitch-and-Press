"use strict";

App.createModule("App.CSP.Area.Admin.Order.Create", function () {

    function init() {



        let itemIndex = 1;

        $("#createOrderForm").validate({
            errorClass: "is-invalid", // Bootstrap class 
            validClass: "is-valid",
            errorPlacement: function (error) {
                console.log(error);
                toastr.error(error);
            },
            rules: {
                OrderName: "required"
                
            },
            messages: {
                OrderName: "Please enter an order name."
            },
            submitHandler: function (form) {
                let isValid = true;
                let formData = $(form);
                let forms = formData.serialize();

                const itemRows = $('#itemsContainer .item-row');

                itemRows.each(function () {
                    const desc = $(this).find('input[name$=".Description"]').val();
                    const price = $(this).find('input[name$=".Price"]').val();
                    const qty = $(this).find('input[name$=".Quantity"]').val();

                    if (!desc || !price || !qty || parseFloat(price) <= 0 || parseInt(qty) <= 0) {
                        isValid = false;
                        toastr.error("All item fields must be filled with valid values.");
                        return false; // break each loop
                    }
                });

                if (isValid) {
                    $.ajax({
                        url: `/api/Admin/Order/Create`,
                        data: formData.serialize(),
                        type: "POST",
                        beforeSend: function () {
                            $("#submitOrder").prop("disabled", true);
                            $("#remove-btn").prop("disabled", true);
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        console.log(jqXHR);
                        console.error("Error occurred:", textStatus, errorThrown);
                    })
                    .done(function (response) {
                        if (response.success === true) {
                            sessionStorage.setItem("toastMessage", response.message);
                            window.location.href = response.redirectUrl;
                            //toastr.success(response.message);
                            
                        }
                        else{
                            toastr.error(response.message)
                        }
                       
                    }).always(function(){
                        $("#submitOrder").prop("disabled", false);
                        $("#remove-btn").prop("disabled", false);
                    })
                }

                return false;
            }
        });

        //add Item Btn
        $("#addItemBtn").on("click", function () {
            const itemRow = `
                            <div class="item-row pt-1 pt-sm-0" style="display:none;">
                                <div class="row pt-md-2 g-1 ">
                                    <!-- Description -->
                                    <div class="col-6 form-floating">
                                        <input type="text" name="Items[${itemIndex}].Description" id="description-${itemIndex}" class="form-control" placeholder="Item Description" />
                                        <label for="description-${itemIndex}">Item Description</label>
                                    </div>

                                    <!-- Price -->
                                    <div class="col-2 form-floating">
                                        <input type="number" name="Items[${itemIndex}].Price" id="price-${itemIndex}" class="form-control" placeholder="Price" />
                                        <label for="price-${itemIndex}">Price</label>
                                    </div>

                                    <!-- Quantity -->
                                    <div class="col-2 form-floating">
                                        <input type="number" name="Items[${itemIndex}].Quantity" id="quantity-${itemIndex}" class="form-control" placeholder="Qty" />
                                        <label for="quantity-${itemIndex}">Qty</label>
                                    </div>

                                    <!-- Remove Button -->
                                    <div class="col-2 d-flex text-nowrap justify-content-center align-items-center">
                                        <button type="button" class="btn btn-danger  text-white remove-btn">
                                            <i class="bi bi-trash"></i>
                                            <span class="d-none d-md-inline">Remove</span>
                                        </button>
                                    </div>
                                </div>

                            </div>
            `;

            let $itemRow = $(itemRow)
            $("#itemsContainer").append($itemRow);
            $itemRow.slideDown("slow");
            itemIndex++;
        }); // end add item btn

        //remove item btn
        $("#itemsContainer").on("click", ".remove-btn", function () {

            if (itemIndex === 1) {
                toastr.error("Atleast one item is required.");
            }
            else {
                $(this).closest('.item-row').fadeOut("slow", function () {
                    $(this).remove();
                });
                itemIndex--;
            }
        });//end item btn remove

        //add payment button 
        $(document).on("click","#addPaymenBtn", function () {
            const paymentRow = `
                          <div class="item-row mb-2 paymentItemRow border-label-box p-1" style="display:none;">
                            <div class="border-label-text">Payment</div>
                            <div class="row pt-md-2 g-1 ">
                              <div class="col-4 form-floating">
                                <input type="text" name="PayerName" class="form-control" id="payerName" placeholder="Payer Name" required />
                                <label for="payerName">Payer Name</label>
                              </div>
                              <!-- Amount -->
                              <div class="col-3 form-floating">
                                <input type="number" name="PaymentAmount" class="form-control" id="paymentAmount" placeholder="Amount" required />
                                <label for="paymentAmount">Amount</label>
                              </div>
                              <div class="col-3 d-flex form-floating align-items-center">
                                <select class="form-select" name="PayTo" id="PayTo">
                                  <option></option>
                                  ${payToSelectOptions}
                                </select>
                                <label for="PayTo">Pay to</label>
                              </div>
                              <div class="col-2 d-flex justify-content-center  align-items-center ">
                                <button type="button" id="removePaymentBtn" class="text-white btn btn-danger  remove-btn">
                                  <i class="bi bi-trash"></i>
                                  <span class="d-none d-md-inline">Remove</span>
                                </button>
                              </div>
                            </div>
                          </div>
                        `;
            $("#addPaymenBtn").slideUp("slow", function () {
                $(this).remove();
            });
            let $row = $(paymentRow)
            $(".paymentRow").append($row);
            $row.fadeIn("slow");
        }); //add payment button end

        //remove payment button
        $(document).on('click',"#removePaymentBtn" ,function (e) {
            e.preventDefault();
            const addPaymentBtn = `
                <button style="display:none;" type="button" id="addPaymenBtn" class=" btn btn-secondary text-nowrap my-1"><i class="bi bi-database-add"></i> Add Payment</button>
            `;

            $(".paymentItemRow").fadeOut("slow", function () {
                $(this).remove();
            });
            let $btn = $(addPaymentBtn);
            $(".paymentRowContainer").append($btn);
            $btn.fadeIn("slow");
        });//remove payment btn end

        //add discount btn
        $(document).on("click", "#addDiscountBtn", function (e) {
            e.preventDefault();
            const discountRow = `
                            <div class="item-row mb-2 discountRow border-label-box">
                                <div class="border-label-text">Discount</div>
                                <div class="row g-1">
                                    <div class="col-6 form-floating">
                                       <input type="text" id="DiscountDesc" name="DiscountDesc" class="form-control" placeholder="Discount Description" required />
                                        <label for="DiscountDesc">Description</label>
                                    </div>
                                    <div class="col-3 form-floating">
                                       <input type="number" id="DiscountAmt" name="DiscountAmt" class="form-control" placeholder="Discount Amount" required />
                                         <label for="DiscountAmt">Amount</label>
                                    </div>
                                    <div class="col-1">
                                         
                                    </div>
                                    <div class="col-2 d-flex text-nowrap justify-content-center   align-items-center">
                                        <button type="button" id="removeDiscountBtn" class="btn text-white btn-danger remove-btn">
                                            <i class="bi bi-trash"></i>
                                            <span class="d-none d-md-inline">Remove</span>
                                        </button>
                                    </div>
                                </div>
                            </div>
            `;

            $("#addDiscountBtn").slideUp("slow", function () {
                $(this).remove();
            });
            const $row = $(discountRow);
            $(".discountRow").append($row);
            $row.slideDown("slow");
        }); //end add disc btn

        $(document).on("click", "#removeDiscountBtn", function () {
            $(this).closest(".discountRow").slideUp("slow", function () {
                $(this).remove();
            });

            // If needed, re-add the "Add Discount" button
            if ($("#addDiscountBtn").length === 0) {
                const addDiscBtn = `
                     <button type="button" id="addDiscountBtn" class="btn btn-secondary text-nowrap  my-1"><i class="bi bi-tags"></i> Add Discount</button>
                `;

                const $btn = $(addDiscBtn);
                $(".discountRowContainer").append($btn);
                $btn.slideDown("slow");
            }
        });

    }; //end init

    return {
        init: init
    }

}, [jQuery]);

$(function () {
    App.CSP.Area.Admin.Order.Create.init();
});