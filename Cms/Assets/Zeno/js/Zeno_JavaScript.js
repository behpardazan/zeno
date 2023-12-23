


function gotoProfile() {

    document.location = "/profile";
}
function gotoDashboard() {

    document.location = "/Account/Dashboard";
}
function gotoDashboardAfterReg() {
    $("#Successregister").modal("hide");
    document.location = "/Account/Dashboard";
}
function gotoPaymentSubscription() {

    //$("#Shipping-Subscription").modal("hide");
    //alert("ffio");
    location.replace("/PaymentSubscriptiondd");
}
function showSuccsessRegister() {
    $("#Successregister").modal("show");

    document.location = "/Account/Dashboard";
}
function gotocurrentUrl() {

    //var x = window.location.href.includes("Basket");
    $(".modal-add-address").modal("hide");
    //if (!x) {
    document.location = window.location.href;
    //}

}
function gotoAddress() {
    if (viewModel.Basket.HaveAddress() == true) {
        document.location = "/basket/address";

    }
    else {
        document.location = "/basket/confirm";
    }

}
function initializeCommonScript() {
    $(document).ready(function () {
        initializeHeaderSearchScripts();
        initializeHeaderStoreSearchScripts();
        //doGetCountryLocation();
        doGetCurrentAccount();
        doGetStoreBasket();
        doGetProductLikes();
        $("#btnAddNewAddress").click(function () {





        });
        $(".show-register-modal").click(function () {
            $("#modal-login").modal("hide");
            $("#modal-register").modal("show");
            $("#modal-prelogin").modal("hide");
        });
        $(".show-login-modal").click(function () {
            $("#modal-login").modal("show");
            $("#modal-register").modal("hide");
            $("#modal-prelogin").modal("hide");
        });
    });
}
function initializeHeaderStoreSearchScripts() {

    $("#txtStoreSearch").keyup(function () {
        var searchValue = $(this).val().trim();

        if (searchValue.length > 0) {

            viewModel.StoreSearchResult.Name(searchValue);
            viewModel.StoreSearchResult.PageSize(7);
            doSearchStore();

        } else {
            viewModel.StoreSearchResult.Name("");
            viewModel.StoreSearchResult.Results.removeAll();
        }
    });
}
function showEditProfile() {
    $('#edit_profile_modal').modal('show');
    doGetCountryProfile();

    // GetStateByCountryIdShipping(viewModel.Account.CountryId());
    //$("#selectedState").val(viewModel.Account.StateId());
    //GetCityByStateId(viewModel.Account.StateId());
    //$("#selectedCity").val(viewModel.Account.CityId());
    //$("#addressCity").val(viewModel.Account.Addresses.Item.CityId());
    //viewModel.State.StateId = viewModel.Account.StateId();
    //viewModel.City.CityId = viewModel.Account.CityId();

}

function showImageCropper() {
    $('.modalImageCropper').modal('show');
}

function stateDropProfile() {
    var selectedstateID = $("#selectedStateProfile").val();
    viewModel.Account.StateId(selectedstateID);
    GetCityByStateIdProfile(selectedstateID);
}
function CityDropProfile() {
    var selectedCityID = $("#selectedCityProfile").val();
    viewModel.Account.CityId(selectedCityID);
}
function closeAddressModal() {
    $(".modal-add-address").modal("hide");
}
function showSubscriptionModal() {
    $('#Shipping-Subscription').modal('show');
}
function showAddress() {

    viewModel.Account.Addresses.Item.NameFamily(viewModel.Account.FullName());

    viewModel.Account.Addresses.Item.Mobile(viewModel.Account.Mobile());
    viewModel.Account.Addresses.Item.Phone(viewModel.Account.Mobile());
    viewModel.Account.Addresses.Item.PostalCode("");
    viewModel.Account.Addresses.Item.AddressValue("");
    doGetCountryAddress();
    GetStateByCountryIdAdress(118);
    $("#addressState").val(0);
    $("#addressCity").val(0);
    viewModel.Account.Addresses.Item.CountryId(118);
    viewModel.Account.Addresses.Item.StateId(0);
    viewModel.Account.Addresses.Item.CityId(0);
    GetCityByStateIdAdress(0);
    $('#addaddress_modal').modal('show');

}
function showInvitation() {
    $("#txtInvitation").val("");
    $('#modalInvitation').modal('show');
}
function ShowAnswerComment(id) {
    GetComment(id);
    $('#mymodal').modal('show');
}
function ShowAnswerCommentStore(id) {
    GetStoreComment(id);
    $('#mymodalstore').modal('show');
}
function initializeHeaderSearchScripts() {

    $("#txtHeaderSearch").keyup(function () {
        var searchValue = $(this).val().trim();
        $(".quick-no-results").removeClass("d-none");
        $(".search_with_result").removeClass("d-none");
        if (searchValue.length > 2) {

            viewModel.SearchResult.Name(searchValue);
            viewModel.SearchResult.isService(false);
            viewModel.SearchResult.PageSize(5);

            doSearchProduct();

        } else {
            viewModel.SearchResult.Name("");
            viewModel.SearchResult.Results.removeAll();
        }
    });
}

function initializeCompareSearchScripts() {

    $("#txtComapreSearch").keyup(function () {

        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            viewModel.CompareSearchResult.Name(searchValue);
            viewModel.CompareSearchResult.isService(false);
            viewModel.CompareSearchResult.PageSize(5);
            doCompareSearchProduct();
        }
        else {

            viewModel.CompareSearchResult.Name("");
            viewModel.CompareSearchResult.Results.removeAll();

        }
    });

    $(".compare-form .btn-remove-pr").on("click", function () {
        $(this).parent().parent().remove();
        $('.compare-form').submit();
    })

}


function addToCompareList(data, event) {
    $('<input>').attr({
        type: 'hidden',
        name: 'productId',
        value: data.Id
    }).appendTo('.compare-form');
    $('.compare-form').submit();
}


function closeModal() {
    $('.modal').modal('hide');
}
function basketPayment() {
    if (viewModel.Account.IsOnline() == false) {
        $("#modal-prelogin").modal("show");
    }
    else {
        location.replace("/basket/confirm?onlineProduct=true");

    }
}
function ShowModalFinishBasket() {
    $("#modal-finishBasket").modal("show");
}

function showAddressInBaket() {
    if (viewModel.Account.IsOnline() == false) {
        $("#modal-prelogin").modal("show");
    }
    else if (viewModel.Basket.StoreItems.Count() == 0) {
        GetNotifyMsg("error", lang.notify.BasketIsEmpty);
    }

    else {
        //$(".shoping_cart").removeClass("active");
        //$(".shoping_address").addClass("active");
        //$(".shoping_confirm").removeClass("active");
        //$(".div-confirm").removeClass("activeT");
        //$(".div-cart").addClass("activeT");
        //$(".div-address").addClass("activeT");
        RemoveCookieOuOfSendStore();

    }
}
function initializeBasketPageScript() {
    //Cart Partial Scripts
    //doGetCountryAddress();

    $(document).ready(function () {
        doGetCountryLocation();

    });
    $(".shoping_address").removeClass("active");
    $(".shoping_confirm").removeClass("active");

    $(".shoping_cart .next-page").click(function () {
        if (viewModel.Account.IsOnline() == false) {
            $("#modal-prelogin").modal("show");
        }
        else if (viewModel.Basket.StoreItems.Count() == 0) {
            GetNotifyMsg("error", lang.notify.BasketIsEmpty);
        }

        else {
            //$(".shoping_cart").removeClass("active");
            //$(".shoping_address").addClass("active");
            //$(".shoping_confirm").removeClass("active");
            //$(".div-confirm").removeClass("activeT");
            //$(".div-cart").addClass("activeT");
            //$(".div-address").addClass("activeT");
            RemoveCookieOuOfSendStore();

        }
        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });

    $(".shoping_address .pre-page").click(function () {
        location.replace("/basket/");
        //$(".shoping_cart").addClass("active");
        //$(".shoping_confirm").removeClass("active");
        //$(".shoping_address").removeClass("active");

        //$(".div-cart").addClass("activeT");
        //$(".div-address").removeClass("activeT");
        //$(".div-confirm").removeClass("activeT");

        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });
    $(".shoping_address .next-page").click(function () {
        if (viewModel.Basket.Address.AddressId() < 1) {
            GetNotifyMsg("error", lang.notify.PleaseSelectAddress);
        }
        else {
            location.replace("/basket/confirm");
        }

    });

    $(".shoping_confirm .pre-page").click(function () {
        location.replace("/basket/address");
        //$(".shoping_cart").removeClass("active");
        //$(".shoping_address").addClass("active");
        //$(".shoping_confirm").removeClass("active");

        //$(".div-confirm").removeClass("activeT");
        //$(".div-cart").addClass("activeT");
        //$(".div-address").addClass("activeT");
        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });
    $(".shoping_confirm .next-page").click(function () {
        if (viewModel.Basket.HaveAddress() != true) {
            viewModel.Basket.Address.AddressId(0);
        }
        if (viewModel.Basket.Address.AddressId() < 1 && viewModel.Basket.HaveAddress() == true) {
            if (viewModel.Basket.HaveAddress() == true) {
                createMessage("error", lang.notify.PleaseSelectAddress);
            }
            //$(".shoping_cart").removeClass("active");
            //$(".shoping_confirm").removeClass("active");
            //$(".shoping_address").addClass("active");
            //$(".div-confirm").removeClass("activeT");
            //$(".div-cart").addClass("activeT");
            //$(".div-address").addClass("activeT");

        }
        else if ($('input[name=merchantId]:checked').val() == undefined) {
            GetNotifyMsg("error", lang.notify.PleaseSelectBank);
        }
        else {
            var merchantId = $('input[name=merchantId]:checked').val();
            var IsCreditShopingVal = $('input[name=IsCreditShoping]:checked').val();
            var IsCreditShoping = false;
            if (IsCreditShopingVal === "1") {
                IsCreditShoping = true;

            }
            else {
                IsCreditShoping = false;

            }
            var rulCreditShoping = $("#rulCreditShoping").is(':checked');

            if (IsCreditShoping == true && rulCreditShoping == false) {
                GetNotifyMsg("error", "لطفا قوانین و مقررات را تایید نمایید");
            }
            else {
                var rebate = viewModel.Basket.Rebate.Value();
                rebate = rebate.toLowerCase();
                window.location.href = "/basket/NewOrder?addressId=" + viewModel.Basket.Address.AddressId() + "&rebateValue=" + rebate + "&merchantId=" + merchantId + "&store=true&IsCreditShoping=" + IsCreditShoping;
            }
        }


    });
    viewModel.Basket.Address.AddressId.subscribe(() => {

        if (viewModel.Account.Addresses.List().length > 0) {
            var match = ko.utils.arrayFirst(viewModel.Account.Addresses.List(), function (item) {
                return item.Id == viewModel.Basket.Address.AddressId();
            });
            if (match) {
                viewModel.Basket.Address.CurrentAddress(match);
            }
            else {
                viewModel.Basket.Address.CurrentAddress("");
            }

        }

    });
    //---Cart Partial Scripts---

    //Address Partial Scripts

    $(".shoping_address .add-new-address").click(function () {
        clearViewModelAccountAddressItem();
        $(".modal-add-address").modal("show");
    });
    //---Address Partial Scripts---

}
function PaymentSubscription() {
    var merchantId = $('input[name=merchantId]:checked').val();
    var pymentId = $('#pymentId').val();

    window.location.href = "/PaymentSubscription/Start?id=" + pymentId + "&merchantId=" + merchantId;

}
function gotoCartPage() {
    location.replace("/basket");
}
function gotoPaymentPage() {
    $(".shoping_address .next-page").trigger("click");
}
function initializeSearchPageScript() {

    var searchForm = $("#searchForm");
    $('.update-stock').change(function () {
        $("input[name='prstockId']").val($("input[name='prstock']:checked").val());
        pageSubmit();
    });

    $('.update-subcategory').change(function () {

        var ids = [];
        $('.brand_filter input:checked').each(function () {
            ids.push($(this).val());
        });
        $("input[name='prbrandId']").val(ids.join("-"));
        pageSubmit();
    });

    $('.update-store').change(function () {

        var ids = [];
        $('.store_filter input:checked').each(function () {
            ids.push($(this).val());
        });
        $("input[name='prstoreId']").val(ids.join("-"));
        pageSubmit();
    });

    $(".option-check-filter").click(function () {
        var selectedOptionItem = $("[option-value]:checked");

        var optionString = "";
        for (var i = 0; i < selectedOptionItem.length; i++) {
            if (i > 0) {
                optionString += "*";
            }

            var itemId = $(selectedOptionItem[i]).attr("option-id");
            optionString += itemId;
        }

        $("[name='proptions']").val(optionString);
        $(searchForm).submit();
    });
    $('.update-storecompetitive').change(function () {

        var ids = [];
        $('.storecompetitive_filter input:checked').each(function () {
            ids.push($(this).val());
        });
        $("input[name='prcompetitiveId']").val(ids.join("-"));
        pageSubmit();
    });
    $('.product_filter_item .products_filter_slide').click(function () {
        $('input[name=prtypeId]').val($(this).attr("value"));
        $('input[name=prcategoryId]').val("");
        $('input[name=prsubcategoryId]').val("");
        $("input[name=prcustomLabel]").val($(this).attr("category-label"));
        pageSubmit();
    });
    $('.product_filter_item .category_id').click(function () {
        $('input[name=prcategoryId]').val($(this).attr("value"));
        $('input[name=prsubcategoryId]').val("");
        $("input[name=prcustomLabel]").val($(this).attr("category-label"));

        pageSubmit();
    });

    $(".custom-check-filter").click(function () {
        var selectedCustomItem = $("[custom-value]:checked");
        var customString = "";
        for (var i = 0; i < selectedCustomItem.length; i++) {
            if (i > 0) {
                customString += "*";
            }

            var fieldId = $(selectedCustomItem[i]).attr("custom-field-id");
            var itemId = $(selectedCustomItem[i]).attr("custom-value");
            customString += fieldId + "@" + itemId;
        }

        $("[name='prcustom']").val(customString);
        $(searchForm).submit();
    });

    $("input[name='searchorder']").change(function () {
        $("input[name='prorder']").val($("input[name='searchorder']:checked").val());
        $(searchForm).submit();
    });

    $(".pagination .page-no").click(function () {
        $("[name=prindex]").val($(this).attr("page-no"));
        pageSubmit();
    });


    var $range = $(".js-range-slider"),
        $inputFrom = $(".js-input-from"),
        $inputTo = $(".js-input-to"),
        instance,
        min = 0,
        max = $("[name=prpriceto]").val() * 3,
        from = 0,
        to = 0;

    $range.ionRangeSlider({
        type: "double",
        min: min,
        max: max,
        from: $("[name=prpricefrom]").val(),
        to: $("[name=prpriceto]").val(),
        prefix: 'تومان',
        onStart: updateInputs,
        onChange: updateInputs,
        step: 50000,
        prettify_enabled: true,
        prettify_separator: ".",
        values_separator: " - ",
        force_edges: true
    });

    instance = $range.data("ionRangeSlider");

    function updateInputs(data) {
        from = data.from;
        to = data.to;
        $("[name=prpricefrom]").val(from);
        $("[name=prpriceto]").val(to);
        $inputFrom.prop("value", from);
        $inputTo.prop("value", to);
    }

    $inputFrom.on("input", function () {
        var val = $(this).prop("value");

        // validate
        if (val < min) {
            val = min;
        } else if (val > to) {
            val = to;
        }

        instance.update({
            from: val
        });
    });

    $inputTo.on("input", function () {
        var val = $(this).prop("value");

        // validate
        if (val < from) {
            val = from;
        } else if (val > max) {
            val = max;
        }

        instance.update({
            to: val
        });
    });

    function pageSubmit() {
        ajaxLoader(true);
        customLabel = $("input[name=prcustomLabel]").val();
        //console.log("/search/" + customLabel + "/?" + $("#searchForm").serialize());
        document.location = "/search/" + customLabel + "/?" + $("#searchForm").serialize();

        //$(searchForm).submit();

    }
}
function initializeSearchStoreScript() {
    var searchForm = $("#searchForm");

    $("input[name='searchorder']").change(function () {
        $("input[name='prorder']").val($("input[name='searchorder']:checked").val());
        $(searchForm).submit();
    });
    $(".pagination .page-no").click(function () {
        $("[name=prindex]").val($(this).attr("page-no"));
        pageSubmit();
    });

    function pageSubmit() {

        ajaxLoader(true);
        $(searchForm).submit();
    }

}

//function GetPriceOption() {
//    $('#prpriceto').val($('.irs-to').text().replace(".", "").replace(".", ""));
//    $('#prpricefrom').val($('.irs-from').text().replace(".", "").replace(".", ""));
//}

function initializeProductPageScript() {

    function getVariety() {
        ajaxLoader(true);
        var productId = $(".variety input[name=productid]").val();
        viewModel.Variety.ProductId(productId.toString());
        var sizeId = viewModel.Variety.SizeId();
        var colorId = viewModel.Variety.ColorId();
        var optionItemId = viewModel.Variety.OptionItemId();
        var request = createRequest();
        request.type = apirequest.type.Get;
        request.url = request.url + codeprocess.api.StoreProductQuantity.name + "?productId=" + productId + "&colorid=" + colorId + "&sizeid=" + sizeId + "&optionItemId=" + optionItemId;
        request.success = function (result) {

            if (result.Code == apirequest.status.Success) {

                var res = result.Value;

                viewModel.Variety.SizeId(res.SizeId);
                viewModel.Variety.Sizes(res.Sizes);
                viewModel.Variety.ColorId(res.ColorId);
                viewModel.Variety.Colors(res.Colors);
                if (res.ColorName) {
                    viewModel.Variety.ColorName(res.ColorName);
                    viewModel.Variety.ColorHex(res.ColorHex);
                }
                if (res.SizeName) {
                    viewModel.Variety.SizeName(res.SizeName);
                }

                viewModel.Variety.OptionName(res.OptionName);
                viewModel.Variety.OptionItemId(res.OptionItemId);
                viewModel.Variety.OptionItemValue(res.OptionItemValue);
                viewModel.Variety.OptionItems(res.OptionItems);
                viewModel.Variety.ProductQuantity(res.ProductQuantity);



            }
            var count = viewModel.Variety.ProductQuantity().length;
            console.log(count);
            if (count == 0) {
                $(".noAvaibleProduct").removeClass("d-none");

            }
            ajaxLoader(false);
        };
        $.ajax(request);
    }
    $(document).on('click', '.variety .btn-color', function () {

        var colorId = $(this).attr("color-id");
        var colrName = $(this).attr("color-name");
        viewModel.Variety.ColorId(colorId);
        viewModel.Variety.SizeId(null);
        viewModel.Variety.OptionItemId(null);
        viewModel.Variety.ColorName(colrName);
        viewModel.Variety.ColorName($(this).attr("color-name"));
        getVariety();
    });
    $(document).on('click', '.variety .btn-size', function () {

        var sizeId = $(this).attr("size-id");
        var sizeName = $(this).attr("size-name");

        viewModel.Variety.SizeId(sizeId);
        viewModel.Variety.SizeName(sizeName);
        viewModel.Variety.OptionItemId(null);
        getVariety();
    });
    $(document).on('click', '.variety .btn-optionitem', function () {
        var optionItemId = $(this).attr("optionitem-id");
        var optionItemName = $(this).attr("optionitem-name");
        viewModel.Variety.OptionItemId(optionItemId);
        viewModel.Variety.OptionItemValue(optionItemName);

        getVariety();
    });
    getVariety();
}


//Address Partial Scripts

doGetAccountAddresses();
doGetStates();
$(".shoping_address .add-new-address").click(function () {
    clearViewModelAccountAddressItem();
    $(".modal-add-address").modal("show");
});
//---Address Partial Scripts---


function showModalAddressForEdit(data, target) {

    updateViewModelAccountAddressItem(data);
    //$("#addressCountry").val("118");
    //$("#addressCountry").val("8");
    //$("#addressCountry").val("87");
    GetStateByCountryIdAdress(viewModel.Account.Addresses.Item.CountryId());
    $(".modal-add-address").modal("show");
}

function showModalAddressForDelete(data, target) {
    $(".btnRemoveAddressItem").attr("address-id", data.Id);
    $(".modal-address-delete").modal("show");
}
function initializeProfilePageScript() {
    doGetAccountAddresses();
    doGetStates();
    doGetProductLikes();

}


function initializeQuestionSearchScripts() {

    $("#txtQuestionSearch").keyup(function () {
        //$("#dv_QuestionSearch").css({ 'display': 'none' });
        $(".dv_QuestionSearch").removeClass("dis-block");
        $(".dv_QuestionSearch").addClass("dis-none");
        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            if (searchValue == "") {
                $(".header-search .quick-results.show-result").hide();
                $(".header-search .quick-no-results").hide();
            } else {
                $(".dv_QuestionSearch").removeClass("dis-none");
                $(".dv_QuestionSearch").addClass("dis-block");
                //$(".header-search .quick-results.show-result").show();
                //$(".header-search .quick-no-results").show();

                //viewModel.SearchResult.Name(searchValue);
                //viewModel.SearchResult.PageSize(5);
                //doSearchProduct();

                viewModel.PostSearchResult.Name(searchValue);
                viewModel.PostSearchResult.Category("Questions");
                viewModel.PostSearchResult.PageSize(5);
                doSearchPost();

            }
        } else {
            $(".header-search .quick-results.show-result").hide();
            $(".header-search.quick-no-results").hide();
        }
    });

    //------------

    var searchForm = $("#searchForm");

    $("input[name='searchorder']").change(function () {
        $("input[name='poorder']").val($("input[name='searchorder']:checked").val());
        pageSubmit();
    });

    function pageSubmit() {

        ajaxLoader(true);
        $(searchForm).submit();
    }
}

function initializeBlogSearchScripts() {

    $("#txtBlogSearch").keyup(function () {
        //$("#dv_‌BlogSearch").css({ 'display': 'none' });
        $(".dv_BlogSearch").removeClass("d-block");
        $(".dv_BlogSearch").addClass("d-none");
        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            if (searchValue == "") {
                $(".header-search .quick-results.show-result").hide();
                $(".header-search .quick-no-results").hide();
            } else {
                $(".dv_BlogSearch").removeClass("d-none");
                $(".dv_BlogSearch").addClass("d-block");

                //$(".header-search .quick-results.show-result").show();
                //$(".header-search .quick-no-results").show();
                //viewModel.SearchResult.Name(searchValue);
                //viewModel.SearchResult.PageSize(5);
                //doSearchProduct();

                viewModel.PostSearchResult.Name(searchValue);
                viewModel.PostSearchResult.Category("Blog");
                viewModel.PostSearchResult.PageSize(5);
                doSearchPost();

            }
        } else {
            $(".header-search .quick-results.show-result").hide();
            $(".header-search.quick-no-results").hide();
        }
    });
}

function initialArticleInfinity() {
    var infinityDiv = $(".infinity-div #infinity-content");
    var pageIndex = parseInt(infinityDiv.attr('page-index'));
    var pageSize = parseInt(infinityDiv.attr('page-size'));
    var loading = infinityDiv.attr("loading");
    $(window).scroll(function () {
        pageIndex = parseInt(infinityDiv.attr('page-index'));
        pageSize = parseInt(infinityDiv.attr('page-size'));
        loading = infinityDiv.attr("loading");
        if (loading == "false") {
            if ((infinityDiv.offset().top + infinityDiv.height()) <= $(window).scrollTop() + $(window).height()) {
                getItems();
            }
        }
    });
    function getItems() {
        if (pageIndex != 0) {

            infinityDiv.attr("loading", "true");
            $.ajax({
                url: "/Post/Search?pocategory=Blog",
                method: "GET",
                datatype: "Html",
                data: {
                    poviewName: "PostList",
                    poIndex: pageIndex,
                    poSize: pageSize
                },
                success: function (result) {
                    //console.log(result);
                    infinityDiv.append(result);
                    if (result.length < 5) {
                        infinityDiv.attr('page-index', "0");

                    }
                    else {
                        infinityDiv.attr('page-index', pageIndex + 1);
                        infinityDiv.attr("loading", "false");
                    }
                    $(".infinity-div .loader").hide();
                }

            });
        }
    }
    getItems();
}

function qrModalOpen() {

}
function GetQuestionSort() {

    $(".sortnew").removeClass("dis-none");
    $(".sortmostsale").removeClass("dis-none");
    if (document.getElementById("sort-2").checked) {
        //$(".sortmostsale").removeClass("dis-none");
        $(".sortmostsale").removeClass("dis-block");
        $(".sortnew").removeClass("dis-none");

        $(".sortmostsale").addClass("dis-none");
        $(".sortnew").addClass("dis-block");
    }
    else {
        $(".sortmostsale").removeClass("dis-none");
        $(".sortnew").removeClass("dis-block");

        $(".sortmostsale").addClass("dis-block");
        $(".sortnew").addClass("dis-none");
    }
}

function initializeOrderSearchScripts() {

    var searchForm = $("#searchForm");

    $("input[name='searchorder']").change(function () {
        $("input[name='aoStatus']").val($("input[name='searchorder']:checked").val());
        $(searchForm).submit();
    });
}
function getSimilarProduct(productId, categoryId, price, name, picture) {
    viewModel.FavoriteSimilarProduct.NameProduct(name);
    viewModel.FavoriteSimilarProduct.Price(price);
    viewModel.FavoriteSimilarProduct.PictureProduct(picture);
    $("#exampleModal").modal("show");
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.product.name + "?notId=" + productId + "&categoryId=" + categoryId + "&pageSize=4";
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            var res = result.Value;
            viewModel.FavoriteSimilarProduct.Items(res);
        }
    };
    $.ajax(request);
}
function initializeShippingPageScript() {
    doGetCountryShipping();
    doGetListShippingCity();
}

function ProfileCountryDropShipping() {
    var countryId = $("#profileSelectedCountryShipping").val();
    viewModel.Account.CountryId(countryId);
    GetStateByCountryIdShipping(countryId);
}
function AddressCountryDropShipping() {
    var countryId = $("#selectedCountryShipping").val();
    viewModel.Account.Addresses.Item.CountryId(countryId);
    GetStateByCountryIdShipping(countryId);
}
function stateDropShipping() {
    var selectedstateID = $("#selectedStateShipping").val();
    GetCityByStateIdShipping(selectedstateID);
}
function stateDropAddress() {
    var selectedstateID = $("#addressState").val();
    viewModel.Account.Addresses.Item.StateId(selectedstateID);
    GetCityByStateIdAdress(selectedstateID);
}
function CountryDropShipping() {
    var countryId = $("#selectedCountryShipping").val();
    GetStateByCountryIdShipping(countryId);
}
function AddShippingCity() {
    var isvalid = true;
    var stateId = $("#selectedStateShipping").val();
    var cityId = $("#selectedCityShipping").val();
    var countryId = $("#selectedCountryShipping").val();
    var sendTime = $("#sendTime").val();
    var sendPrice = $("#sendPrice").val();

    if (countryId == "-1") {
        alert("لطفا یک کشور را انتخاب نمایید");
        isvalid = false;

    }
    if (sendTime == "" || sendPrice == "") {
        alert("ورود هزینه و زمان ارسال اجباری می باشد");
        isvalid = false;

    }
    if (stateId == "-1") {
        alert("لطفا یک استان را انتخاب نمایید");
        isvalid = false;

    }

    if (isvalid == true) {
        ajaxLoader(true);

        var request = createRequest();
        request.type = apirequest.type.get;
        request.url = request.url + codeprocess.api.shippingCity.name + "?stateId=" + stateId + "&cityId=" + cityId + "&countryId=" + countryId + "&sendPrice=" + sendPrice + "&sendTime=" + sendTime;
        request.success = function (result) {
            if (result.Code === apirequest.status.Success) {

                $("#sendTime").val("");
                $("#sendPrice").val("");
                $("#selectedStateShipping").val("-1");
                $("#selectedCityShipping").val("-1");
                doGetListShippingCity();
                doGetCountryShipping();
                ajaxLoader(false);
            }
        };
        $.ajax(request);
    }

}
function RemoveShippingCity(shippingid) {
    var request = createRequest();
    request.type = apirequest.type.get;

    request.url = request.url + codeprocess.api.shippingCity.name + "?id=" + shippingid;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            doGetListShippingCity();
        }
    };
    $.ajax(request);
}
function GetSelectCityIdAddress() {
    var selectedCityID = $("#addressCity").val();
    viewModel.Account.Addresses.Item.CityId(selectedCityID);

}

function CountryDropAddress() {
    var countryId = $("#addressCountry").val();
    viewModel.Account.Addresses.Item.CountryId(countryId);
    GetStateByCountryIdAdress(countryId);
}
function doGetCountryShipping(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name + "?isShipping=true";
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            viewModel.Country.CountryId("118");
            $("#selectedCountryShipping").val("118");
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCountryProfile(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            viewModel.Country.CountryId("118");
            $("#profileSelectedCountryShipping").val("118");
            GetStateByCountryIdProfile(viewModel.Country.CountryId());
            //CountryDropShipping();
            //ProfileCountryDropShipping()
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCountryAddress(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            $("#addressCountry").val("118");
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetListShippingCity(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shippingCity.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Shipping.List(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCountryLocation(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;

    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            viewModel.Country.CountryId(118);
            $("#locationCountry").val("118");
            GetStateByCountryIdLocation("118");
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function GetStateByCountryIdShipping(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;

    request.success = function (result) {
        viewModel.State.StateList(result.Value);

    };
    $.ajax(request);
}
function GetStateByCountryIdLocation(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;
    request.success = function (result) {
        viewModel.State.StateList(result.Value);
        GetCurrentLocation();

    };
    $.ajax(request);
}
function GetStateByCountryIdAdress(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;

    request.success = function (result) {
        viewModel.State.StateList(result.Value);
        $("#addressState").val(viewModel.Account.Addresses.Item.StateId());
        GetCityByStateIdAdress(viewModel.Account.Addresses.Item.StateId());
    };
    $.ajax(request);
}
function GetStateByCountryIdProfile(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;
    ajaxLoader(true);
    request.success = function (result) {

        viewModel.State.StateList(result.Value);

        viewModel.State.StateId(viewModel.Account.StateId());
        $("#selectedStateProfile").val(viewModel.State.StateId());
        GetCityByStateIdProfile(viewModel.State.StateId());
    };
    $.ajax(request);
    ajaxLoader(false);
}
function GetCityByStateIdShipping(stateId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.city.name + "?isShipping=true&id=" + stateId;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
    };
    $.ajax(request);
}
function GetCityByStateIdProfile(stateId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.city.name + "?isShipping=true&id=" + stateId;
    ajaxLoader(true);

    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        $("#selectedCityProfile").val(viewModel.Account.CityId());
    };
    $.ajax(request);
    ajaxLoader(false);
}
function GetCityByStateIdAdress(stateId) {
    var request = createRequest();
    request.data = ko.toJSON(stateId);
    request.url = request.url + codeprocess.api.city.name + "?id=" + stateId;
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        $("#addressCity").val(viewModel.Account.Addresses.Item.CityId());
    };
    $.ajax(request);
}
function GetCityByStateIdLocation(stateId, stateName) {
    ajaxLoader(true);
    var request = createRequest();
    request.data = ko.toJSON(stateId);
    request.url = request.url + codeprocess.api.city.name + "?id=" + stateId;
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        viewModel.City.CityList.remove("0");
        viewModel.City.CityName("");
        viewModel.City.CityId("");
        viewModel.State.StateName(stateName);
        viewModel.State.StateId(stateId);
        ajaxLoader(false);
    };
    $.ajax(request);

}
function GetSelectCityIdlocation(cityId, cityName) {
    viewModel.City.CityId(cityId);
    viewModel.City.CityName(cityName);
    doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), viewModel.Basket.Address.AddressId());

    $("#exampleModalCenter").modal("hide");
}



//function loadImage() {
//    debugger;
//    var fileUpload = $("#storeimg").get(0);
//    var files = fileUpload.files;
//    var uploadType = $(fileUpload).attr("uploadtype");
//    var data = new FormData();
//    for (var i = 0; i < files.length; i++) {
//        data.append(files[i].name, files[i]);
//    }
//    var request = createRequest();
//    request.url = base_admin_url + "/Upload/UploadPhoto";
//    request.type = "POST";
//    request.data = data,
//        request.contentType = false;
//    request.processData = false;
//    request.beforeSend = function () {
//        $("#modalLoading").modal("show");
//    }
//    request.error = function () {
//        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
//    };
//    request.success = function (entity) {
//        if (uploadType == 2) {
//            uploadIconId = entity.Id;
//        }
//        else if (uploadType == 3) {
//            uploadWriterPictureId = entity.Id;
//        }
//        else {
//            uploadPictureId = entity.Id;

//        }
//        $("#modalLoading").modal("hide");
//    };
//    $.ajax(request);
//    $("#storeimg").change(function () {

//        //var fileUpload = $("#" + Id).get(0);
//        //var files = fileUpload.files;

//        //var data = new FormData();
//        //for (var i = 0; i < files.length; i++) {
//        //    data.append(files[i].name, files[i]);
//        //}
//        //var request = createRequest();
//        //request.url = base_admin_url + "/Upload/UploadDocument";
//        //request.type = "POST";
//        //request.data = data,
//        //    request.contentType = false;
//        //request.processData = false;
//        //request.beforeSend = function () {
//        //    $("#modalLoading").modal("show");
//        //}
//        //request.error = function () {
//        //    createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
//        //};
//        //request.success = function (entity) {
//        //    uploadDocumentId = entity.Id;
//        //    $("#modalLoading").modal("hide");
//        //};
//        //$.ajax(request);
//    });
//}

function addImage(id) {

    var fileUpload = $("#storeimg").get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    data.append("Id", id);
    var request = createRequest();
    request.type = "POST";
    request.url = "Areas/Store/Accounts/AddImage";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.success = function () {
        //alert("yes");
        //$("#divprority").html(result);
    }
    request.error = function () {
        //alert("no");
        //$("#divprority").html(result);
    }
    //alert(request);
    $.ajax(request);
}

$(".closeResult").click(function () {
    $(".quick-no-results").addClass("d-none");
    $(".search_with_result").addClass("d-none");
    viewModel.SearchResult.Results(null);
});

        function changeType() {
            var type = $("#float_password").attr("type");
           
            if (type == "password") {
                $("#float_password").attr("type", "text");
            }
            else {
                $("#float_password").attr("type", "password");
            }
            
        }
