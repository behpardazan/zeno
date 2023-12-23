$(".mobile-menu-dropdown-item1 > .mobile-menu-dropdown-item-title svg").click(function () {
	if( $(this).hasClass("rote180") ) {
		$(".mobile-menu-dropdown-content1").slideUp();
  		$(".mobile-menu-dropdown-item-title svg").removeClass("rote180");
		console.log("open");
	   //$(this).removeClass("rote180");
  		//$(this).parent(".mobile-menu-dropdown-item-title").next(".mobile-menu-dropdown-content1").slideUp();
	} else {
		$(".mobile-menu-dropdown-content1").slideUp();
  		$(".mobile-menu-dropdown-item-title svg").removeClass("rote180");
		console.log("close");
		$(this).addClass("rote180");
  		$(this).parent(".mobile-menu-dropdown-item-title").next(".mobile-menu-dropdown-content1").slideDown();
	}
})
$(".mobile-menu-dropdown-item2 > .mobile-menu-dropdown-item-title svg").click(function () {
	if( $(this).hasClass("rote180") ) {
		$(".mobile-menu-dropdown-content2").slideUp();
  		$(".mobile-menu-dropdown-item2 > .mobile-menu-dropdown-item-title svg").removeClass("rote180");
	   	//$(this).removeClass("rote180");
  		//$(this).parent(".mobile-menu-dropdown-item-title").next(".mobile-menu-dropdown-content2").slideUp();
	} else {
		$(".mobile-menu-dropdown-content2").slideUp();
  		$(".mobile-menu-dropdown-item2 > .mobile-menu-dropdown-item-title svg").removeClass("rote180");
		$(this).addClass("rote180");
  		$(this).parent(".mobile-menu-dropdown-item-title").next(".mobile-menu-dropdown-content2").slideDown();
	}
})

$(document).ready(function () {
    var homeIconNumber = $('.HomeServices .row .col-lg-2.col-md-4.col-sm-6.col-6.Top').length;
    // console.log(homeIconNumber);
    if (homeIconNumber % 2 == 1) {
        $(".HomeServices .row .col-lg-2.col-md-4.col-sm-6.col-6.Top:last-child").removeClass('col-6').addClass('col-12')
    }
})
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

/*============================ drop down 3 level ===========================*/
const screenWidth = $(window).width();
console.log(screenWidth);

if (screenWidth < 1200) {
    // alert("yes");
    $(".dropdown > .btn").click(function (event) {
        event.preventDefault();
        const dropMenu = $(this).closest(".dropdown").find(".dropdown-menu").first();
        // console.log(dropMenu)
        let toggleState = dropMenu.attr("toggle-state");
        console.log(toggleState);
        if (toggleState == "close") {
            // alert("yes");
            dropMenu.addClass("showSubMenu");
            $(this).addClass("rote180");
            dropMenu.attr("toggle-state", "open");
        } else {
            dropMenu.removeClass("showSubMenu");
            $(this).removeClass("rote180");
            dropMenu.attr("toggle-state", "close");
        }
    });
    $(".subDropDown > .btn").click(function () {
        const dropMenu = $(this).closest(".subDropDown").first().find(".dropdown-menu").first();
        // console.log(dropMenu)
        let toggleState = dropMenu.attr("toggle-state");
        // console.log(toggleState);
        if (toggleState == "close") {
            // alert("yes");
            dropMenu.addClass("showSubMenu");
            dropMenu.attr("toggle-state", "open");
        } else {
            dropMenu.removeClass("showSubMenu");
            dropMenu.attr("toggle-state", "close")
        }
    })
}



/*======================= see more ========================*/
const text = $(".totlalText p").text();
console.log(text.length);
if (text.length > 350)
    $(".limitedText p").text(text.substring(0, 350) + ' . . . ')
else {
    $(".limitedText p").text()
}

$(".seeMoreButton").click(function () {
    $(".limitedText").slideUp();
    $(".totlalText").slideDown();
});

$(".hideMoreTextBtn").click(function () {
    $(".totlalText").slideUp();
    $(".limitedText").slideDown();
});


/*=====================================mega menu==============================*/
if ($(window).width() > 990) {
    $(".dropdown").hover(function () {
        $(this).find(".dropDownContent").toggleClass("show animated fadeInDown")
    });
}
if ($(window).width() < 991) {
    $(".category_link").click(function () {
        $("body").addClass("body_filter");
        $(this).next(".dropDownContent").addClass("showsidebar");
    });
    $(".closeSide").click(function () {
        $("body").removeClass("body_filter");
        $(".dropDownContent").removeClass("showsidebar")
    });
}
$(".dropItem").hover(function (e) {
    $(".dropItem").removeClass("active");
    $(this).addClass("active");
    var itemName = $(this).find("a").attr('dtarget');
    var targetName = $(".dropContentBox[name=" + itemName + "]").attr("name");
    $('.dropContentBox').removeClass("d-block");
    $(".dropContentBox[name=" + itemName + "]").addClass("d-block");

});

$(".dropItem").click(function (e) {
    var itemName = $(this).find("a").attr('dtarget');
    var targetName = $(".dropContentBox[name=" + itemName + "]").attr("name");
    $('.dropContentBox').removeClass("d-block");
    $(".dropContentBox[name=" + itemName + "]").addClass("d-block");

});

$(".closeSide").click(function () {
    $("body").removeClass("body_filter");
    $(".dropDownContent").removeClass("showsidebar")
});
/*=====================================end mega menu==============================*/

/*====================sliderNavigation=====================*/
$(".next").click(function () {
    var currentowl = $(this).closest(".sliderNavigation").prev(".owl-carousel");
    currentowl.trigger('next.owl.carousel');
})
$(".prev").click(function () {
    var currentowl = $(this).closest(".sliderNavigation").prev(".owl-carousel");
    currentowl.trigger('prev.owl.carousel');
});
/*====================End sliderNavigation=====================*/
/*==================================range slider======================================*/
$(function () {
    $('.slider').on('input change', function () {
        $(this).next($('.slider_label')).html(this.value);


    });
    $('.slider_label').each(function () {
        var value = $(this).prev().attr('value');
        $(this).html(value);
    });


})
$(function () {

    var $range = $(".js-range-slider"),
        $inputFrom = $(".js-input-from"),
        $inputTo = $(".js-input-to"),
        instance,
        min = 0,
        max = 10000000,
        from = 0,
        to = 0;

    $range.ionRangeSlider({
        type: "double",
        min: min,
        max: max,
        from: 0,
        to: 5000000,
        prefix: 'تومان ',
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
});
/*=============================================end range slider======================================*/
/*============================================blog============================================*/
$(".blogItem").find(".blogItemText").text(function (index, currentText) {
    return (currentText.substr(0, 200) + '...');
});
$('.relatedArticles_slider').owlCarousel({
    loop: true,
    margin: 0,
    dots: false,
    rtl: true,
    autoplay: false,
    nav: false,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 4
        }
    }
});
/*============================================profile==========================================*/
$(".itemFooter a").click(function () {
    var parent = $(this).closest(".orderParent");

    parent.find(".orderItemContent ").slideToggle('slow');

});

/*============================================ end profile==========================================*/
/*===========================================slider=======================================*/
$('.mainSlider .owl-carousel').owlCarousel({
    loop: true,
    margin: 0,
    dots: false,
    rtl: true,
    autoplay: false,
    nav: false,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
});
/*=================================total products=========================================*/


$(".products_filter_slide").click(function () {
    if ($(this).next().find(".filter_slide_body").is(':visible')) {
        $(this).next().find(".filter_slide_body").slideUp();
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(90deg)");
        $(".products_filter_slide").removeClass("categoryExoand")
    } else {
        $(".products_filter_slide").removeClass("categoryExpand");
        $(".filter_slide_body").slideUp();
        $(".fI_line2").css("transform", "rotate(90deg)")
        $(".products_filter_slide").removeClass("categoryExoand")
        $(this).next().find(".filter_slide_body").slideToggle("slow");
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(0deg)");
        $(this).addClass("categoryExoand")
    }
});

$(".subproducts_filter_slide").click(function () {
    if ($(this).next().find(".filter_subslide_body").is(':visible')) {
        $(this).next().find(".filter_subslide_body").slideUp();
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(90deg)");
    } else {
        $(".subproducts_filter_slide").removeClass("categoryExpand");
        $(".filter_subslide_body").slideUp();
        $(".fI_line2").css("transform", "rotate(90deg)")
        $(this).next().find(".filter_subslide_body").slideToggle("slow");
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(0deg)")
    }
});

$(".subCat_filter_slide").click(function () {
    if ($(this).next().find(".subCat_slide_body").is(':visible')) {
        $(this).next().find(".subCat_slide_body").removeClass("d-block");
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(90deg)");
    } else {
        $(".subCat_slide_body").slideUp();
        $(".subCat_filter_slide").find(".fI_line2").css("transform", "rotate(90deg)")
        $(this).next().find(".subCat_slide_body").toggleClass("d-block");
        $(this).find(".filter_item_icon").find(".fI_line2").css("transform", "rotate(0deg)")
    }
});

$(".showFilter").click(function () {
    $("body").addClass("filter_body");
    $(".Filter_Product").addClass("slideFilter")

});

$(".closeFilter").click(function () {
    $("body").removeClass("filter_body");
    $(".Filter_Product").removeClass("slideFilter")

});


$(document).click(function (e) {
    if ($(e.target).closest('.filter_box').length === 0 && $(e.target).closest('.showFilter').length === 0) {
        $("body").removeClass("filter_body");
        $(".Filter_Product").removeClass("slideFilter")
    }
});

/*=======================header==================================*/
$(".userProfile_icon").click(function () {
    $(this).closest(".userProfile").find(".profile_items").slideToggle();
});
//if ($(".profile_items").css('dispaly') == 'block') {
//	$(document).click(function () {
//    	$(".profile_items").slideUp();
//    });
//}
$(window).click(function() {
  $(".profile_items").slideUp();
});
$('.userProfile').click(function(event){
  event.stopPropagation();
});

$(".navbar-toggler").click(function () {
    $(this).toggleClass("collapsrBar")
});

$('.down2TopButton').click(function () {
    $([document.documentElement, document.body]).animate({
        scrollTop: 0
    }, 1000);
    return false;
});




/*-------------------------------------------------------------------------New------------------------------------------------------------------------*/
$(".first-banner .first-banner-btn .btn").click(function () {
    $(".first-banner").addClass("dnone");
})

$(document).ready(function () {
    $('#mainSlider').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 0,
        nav: false,
        rtl: true,
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            800: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider1').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 20,
        nav: false,
        rtl: true,
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            800: {
                items: 3
            },
            1000: {
                items: 4
            },
            1400: {
                items: 5
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider2').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 20,
        nav: false,
        rtl: true,
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            800: {
                items: 3
            },
            1000: {
                items: 4
            },
            1400: {
                items: 5
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider3').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 20,
        nav: false,
        rtl: true,
        dots: false,
        responsive: {
            0: {
                items: 1
            },
            500: {
                items: 1
            },
            800: {
                items: 2
            },
            1000: {
                items: 3
            },
            1400: {
                items: 4
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider4').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 20,
        nav: false,
        rtl: true,
        dots: false,
        responsive: {
            0: {
                items: 1
            },
            500: {
                items: 1
            },
            800: {
                items: 2
            },
            1000: {
                items: 3
            },
            1400: {
                items: 4
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider5').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 20,
        nav: false,
        rtl: true,
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            500: {
                items: 2
            },
            800: {
                items: 2
            },
            1000: {
                items: 3
            },
            1400: {
                items: 3
            }
        }
    })
});
$(document).ready(function () {
    $('#Slider6').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        margin: 10,
        nav: false,
        rtl: true,
        dots: false,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            800: {
                items: 3
            },
            1000: {
                items: 4
            },
            1400: {
                items: 5
            }
        }
    })
});
$('.product-details-slider .owl-carousel').owlCarousel({
    loop: true,
    margin: 0,
    dots: false,
    rtl: true,
    autoplay: true,
    nav: false,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 3
        },
        1000: {
            items: 5
        }
    }
});
$(document).on('keypress',function(e) {
    if(e.which == 13) {
        $(".loginFooter .primary_btn").click();
    }
});
$(".HomeBlog").find(".next").click(function () {
    var currentowl = $(this).parents("section").find(".owl-carousel");
    currentowl.trigger('next.owl.carousel');
})
$(".HomeBlog").find(".prev").click(function () {
    var currentowl = $(this).parents("section").find(".owl-carousel");
    currentowl.trigger('prev.owl.carousel');
});
$(".eye-icon").click(function () {
	$(this).find(".eye-icon-inner").toggleClass("active");
});
$('html, body').animate({
         scrollTop: $(".product_details").offset().top
}, 400);