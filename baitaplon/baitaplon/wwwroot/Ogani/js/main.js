/*  ---------------------------------------------------
    Template Name: Ogani
    Description:  Ogani eCommerce  HTML Template
    Author: Colorlib
    Author URI: https://colorlib.com
    Version: 1.0
    Created: Colorlib
---------------------------------------------------------  */

'use strict';

(function ($) {

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Gallery filter
        --------------------*/
        $('.featured__controls li').on('click', function () {
            $('.featured__controls li').removeClass('active');
            $(this).addClass('active');
        });
        if ($('.featured__filter').length > 0) {
            var containerEl = document.querySelector('.featured__filter');
            var mixer = mixitup(containerEl);
        }
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    //Humberger Menu
    $(".humberger__open").on('click', function () {
        $(".humberger__menu__wrapper").addClass("show__humberger__menu__wrapper");
        $(".humberger__menu__overlay").addClass("active");
        $("body").addClass("over_hid");
    });

    $(".humberger__menu__overlay").on('click', function () {
        $(".humberger__menu__wrapper").removeClass("show__humberger__menu__wrapper");
        $(".humberger__menu__overlay").removeClass("active");
        $("body").removeClass("over_hid");
    });

    /*------------------
		Navigation
	--------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*-----------------------
        Categories Slider
    ------------------------*/
    $(".categories__slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 4,
        dots: false,
        nav: true,
        navText: ["<span class='fa fa-angle-left'><span/>", "<span class='fa fa-angle-right'><span/>"],
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        responsive: {

            0: {
                items: 1,
            },

            480: {
                items: 2,
            },

            768: {
                items: 3,
            },

            992: {
                items: 4,
            }
        }
    });


    $('.hero__categories__all').on('click', function(){
        $('.hero__categories ul').slideToggle(400);
    });

    /*--------------------------
        Latest Product Slider
    ----------------------------*/
    $(".latest-product__slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ["<span class='fa fa-angle-left'><span/>", "<span class='fa fa-angle-right'><span/>"],
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*-----------------------------
        Product Discount Slider
    -------------------------------*/
    $(".product__discount__slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 3,
        dots: true,
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        responsive: {

            320: {
                items: 1,
            },

            480: {
                items: 2,
            },

            768: {
                items: 2,
            },

            992: {
                items: 3,
            }
        }
    });

    /*---------------------------------
        Product Details Pic Slider
    ----------------------------------*/
    $(".product__details__pic__slider").owlCarousel({
        loop: true,
        margin: 20,
        items: 4,
        dots: true,
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true
    });

    /*-----------------------
		Price Range Slider
	------------------------ */
    var rangeSlider = $(".price-range"),
        minamount = $("#minamount"),
        maxamount = $("#maxamount"),
        minPrice = rangeSlider.data('min'),
        maxPrice = rangeSlider.data('max');
    rangeSlider.slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [minPrice, maxPrice],
        slide: function (event, ui) {
            minamount.val('$' + ui.values[0]);
            maxamount.val('$' + ui.values[1]);
        }
    });
    minamount.val('$' + rangeSlider.slider("values", 0));
    maxamount.val('$' + rangeSlider.slider("values", 1));

    /*--------------------------
        Select
    ----------------------------*/
    $("select").niceSelect();

    /*------------------
		Single Product
	--------------------*/
    $('.product__details__pic__slider img').on('click', function () {

        var imgurl = $(this).data('imgbigurl');
        var bigImg = $('.product__details__pic__item--large').attr('src');
        if (imgurl != bigImg) {
            $('.product__details__pic__item--large').attr({
                src: imgurl
            });
        }
    });

    /*-------------------
		Quantity change
	--------------------- */
    var proQty = $('.pro-qty');
    proQty.prepend('<span class="dec qtybtn">-</span>');
    proQty.append('<span class="inc qtybtn">+</span>');
    proQty.on('click', '.qtybtn', function () {
        var $button = $(this);
        var $input = $button.parent().find('input');
        var oldValue = $input.val();
        var productId = $input.data('product-id');
        var $row = $button.closest('tr'); 
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
            updateCart('AddToCart', productId, $row);
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0 ) {
                var newVal = parseFloat(oldValue) - 1;
                updateCart('RemoveFromCart', productId, $row);
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });
    function updateCart(action, productId, $row) {
        $.ajax({
            url: '/Shop/' + action, // URL to the server-side function
            type: 'POST',
            data: {
                productId: productId,
            },
            success: function (response) {
                if (response.success) {
                    // Cập nhật tổng giá trị của sản phẩm
                    if (action === 'AddToCart') {
                        var price = parseFloat($row.find('.shoping__cart__price').text().replace(/[^0-9.-]+/g, ""));
                        var priceText = $row.find('.shoping__cart__price').text(); // Lấy giá trị văn bản
                        console.log('Giá trị văn bản của Product_Price:', priceText);
                        console.log('day la price', price)
                        var newTotal = price * (parseInt($row.find('input').val(), 10));
                        $row.find('.shoping__cart__total').text(`$${newTotal.toFixed(2)}`);
                        // Cập nhật tổng tiền toàn bộ giỏ hàng (totalAmount)
                        console.log("in fia tri nay ra", $('#subtotal-amount').text())
                        $('#subtotal-amount').text(`$${response.totalAmount.toFixed(2) }`);
                        $('#total-amount').text(`$${response.totalAmount.toFixed(2)}`);
                        console.log('total', response.totalItemsCount)
                        $('#cart-count').text(response.totalItemsCount)
                    } else if (action === 'RemoveFromCart') {
                 
                        var currentQuantity = parseInt($row.find('input').val(), 10);

                        if (currentQuantity > 0) {
                            var price = parseFloat($row.find('.shoping__cart__price').text().replace(/[^0-9.-]+/g, ""));
                            var newTotal = price * currentQuantity;
                            $row.find('.shoping__cart__total').text(`$${newTotal.toFixed(2)}`);
                            $('#subtotal-amount').text(`$${response.totalAmount.toFixed(2)}`);
                            $('#total-amount').text(`$${response.totalAmount.toFixed(2)}`);
                            console.log('total', response.totalItemsCount)
                            $('#cart-count').text(response.totalItemsCount)
                        } else {
                            $row.remove(); // Xóa dòng sản phẩm khỏi UI nếu số lượng là 0
                        }
                    }

                }
            },
            error: function (error) {
                console.log('Error updating:', error);
                // Có thể thêm thông báo lỗi cho người dùng
            }
        });

    }

    $(function () {
        // Sử dụng on thay cho click để bắt sự kiện
        $(document).on('click', '.addToCartBtn', function () {
            console.log('zo day ne')
            var productId = $(this).data('product-id'); // Lấy productId từ thuộc tính data

            $.ajax({
                url: '/Shop/AddToCart/', // URL của action xử lý
                type: 'POST',
                data: {
                    productId: productId  // Truyền productId tới server
                },
                success: function (response) {
                    // Xử lý khi thêm vào giỏ hàng thành công
                    alert('Sản phẩm đã được thêm vào giỏ hàng!');
                },
                error: function (xhr, status, error) {
                    // Xử lý khi có lỗi
                    console.error('Lỗi khi thêm vào giỏ hàng: ' + error);
                }
            });
        });
    });

    $(function () {
        $(document).on('click', '.icon_close', function () {
            var productId = $(this).data('product-id'); // Lấy productId từ thuộc tính data

            // Gửi yêu cầu Ajax để xóa sản phẩm khỏi giỏ hàng
            $.ajax({
                url: '/Shop/DeleteFromCart/',
                type: 'POST',
                data: {
                    productId: productId
                },
                success: function (response) {
                    // Xử lý khi xóa thành công (ví dụ: làm mới giỏ hàng hoặc xóa sản phẩm khỏi bảng)
                    alert('Sản phẩm đã được xóa khỏi giỏ hàng!');
                    location.reload(); // Làm mới trang để cập nhật lại giỏ hàng
                },
                error: function (xhr, status, error) {
                    // Xử lý lỗi
                    console.error('Lỗi khi xóa sản phẩm: ' + error);
                }
            });
        });
    });
    
    

})(jQuery);