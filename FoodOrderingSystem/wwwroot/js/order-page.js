var categoryList;

function loadCategories() {
    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/GetCategories";

    request.open('GET', url, true);
    request.onload = function() {
        categoryList = JSON.parse(this.responseText);
    };
    request.send();
}

function showItemList() {
    var searchValue = document.getElementById("search-value").value;
    var request = new XMLHttpRequest();
    var url, content;

    if (searchValue.length > 0) {
        url = "/api/AdminDashboard/ViewItemListBySearching";
        content = '{"SearchValue": "' + searchValue + '", "CategoryName": "all", "Status": "all", "RowsOnPage": 100, "RequestPage": 1}';
    } else {
        url = "/api/AdminDashboard/ViewItemList";
        content = '{"CategoryName": "all", "Status": "all", "RowsOnPage": 100, "RequestPage": 1}';
    }

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "text/json");
    request.onload = function() {
        var result = JSON.parse(this.responseText).data;
        renderItemList(result, function() {
            formatMoneyString();

            //===== Touch Spin =====//
            if ($.isFunction($.fn.TouchSpin)) {
                $('input.qty').TouchSpin({});
            }
        });
    };
    request.send(content);
}

function renderItemList(itemList, callback) {
    var list = document.getElementById("dishes-list");

    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }

    for (let i = 0; i < categoryList.length; i++) {
        var wrapper = document.createElement('div');
        var category = document.createElement('h4');
        var items = document.createElement('ul');
        var categoryValue = document.createElement('span');

        wrapper.classList = "dishes-list-wrapper";
        category.classList = "title3";
        items.classList = "dishes-list";
        categoryValue.classList = "sudo-bottom sudo-bg-red";

        categoryValue.innerHTML = categoryList[i].categoryName;

        for (let j = 0; j < itemList.length; j++) {
            if (itemList[j].categoryName === categoryList[i].categoryName && itemList[j].status === "Active") {
                var item = document.createElement('li');
                var box = document.createElement('div');
                var thumb = document.createElement('div');
                var image = document.createElement('img');
                var info = document.createElement('div');
                var name = document.createElement('h4');
                var price = document.createElement('span');
                var description = document.createElement('p');
                var qtyWrap = document.createElement('div');
                var touchspin = document.createElement('div');
                var qtyInput = document.createElement('input');
                var btnWrap = document.createElement('div');
                var button = document.createElement('a');

                items.appendChild(item);
                item.appendChild(box);
                box.appendChild(thumb);
                box.appendChild(info);
                box.appendChild(qtyWrap);
                box.appendChild(btnWrap);
                thumb.appendChild(image);
                info.appendChild(name);
                info.appendChild(price);
                info.appendChild(description);
                qtyWrap.appendChild(touchspin);
                touchspin.appendChild(qtyInput);
                btnWrap.appendChild(button);

                box.classList = "featured-restaurant-box";
                thumb.classList = "featured-restaurant-thumb";
                info.classList = "featured-restaurant-info";
                btnWrap.classList = "ord-btn";
                price.classList = "price money";
                qtyWrap.classList = "qty-wrap";
                touchspin.classList = "input-group bootstrap-touchspin";
                qtyInput.classList = "qty form-control";
                button.classList = "brd-rd2 add-cart";

                qtyInput.type = "text";
                qtyInput.value = 1;
                qtyInput.style.display = "block";
                image.src = "../images/resource/dish-img1-" + (j + 1) + ".jpg";
                image.alt = "dish-img1-" + (j + 1) + ".jpg";

                button.title = "Order Now";
                button.href = "/CustomerDashboard/addcart/" + itemList[j].itemID;

                name.innerHTML = itemList[j].itemName;
                price.innerHTML = itemList[j].unitPrice;
                description.innerHTML = itemList[j].description;
                button.innerHTML = "Add to order";
            }
        }

        if (items.firstChild) {
            list.appendChild(wrapper);
            wrapper.appendChild(category);
            wrapper.appendChild(items);
            category.appendChild(categoryValue);
        }
    }

    $('.add-cart').click(function(event) {
        event.preventDefault();
        var url = $(this).attr('href');

        var $quantity = $(this).parent().parent().find('.input-group input');
        var price = $(this).parent().parent().find('span.price').text().replace(/\D/g, '');
        var name = $(this).parent().parent().find('h4').text();

        for (let i = 0; i < $quantity.val(); i++) {
            $.get(url, function() {});
            addToCart(url.split("/").pop(), price, name, function() {
                formatMoneyString();

                $('.item-remove-btn').prop("onclick", null).off("click");
                $('.item-remove-btn').click(function(event) {
                    event.preventDefault();
                    var url = $(this).attr('href');
                    $(this).closest('li').remove();
                    $.get(url, function() {});
                    var removePrice = parseInt($(this).parent().find('.price').text().replace(/\D/g, ''));
                    subtotalBill = $("#subtotal-bill");
                    subtotalBill.html(parseInt(subtotalBill.html().replace(/\D/g, ''), 10) - removePrice);
                    formatMoneyString();
                });
            });
        }
    });

    callback();
}

function addToCart(itemID, itemPrice, itemName, callback) {
    var list = document.getElementById("order-list");
    var added = false;
    var listEmpty = ($("#order-list").length === 0);

    if (listEmpty === false) {
        for (let i = 1; i <= list.childElementCount; i++) {
            if ($("#order-list li:nth-child(" + i + ")").find('h5').text() === itemID) {
                var quantity = $("#order-list li:nth-child(" + i + ")").find('h6').find('span');
                quantity.html(parseInt(quantity.html(), 10) + 1);
                var subtotal = $("#order-list li:nth-child(" + i + ")").find('span.price');
                subtotal.html(parseInt(subtotal.html().replace(/\D/g, ''), 10) + parseInt(itemPrice));
                added = true;
            }
        }
    } else {
        var orderWrapper = document.getElementById("order-list-wrapper");
        list = document.createElement('ul');
        list.classList = "order-list-inner";
        orderWrapper.appendChild(list);
    }

    if (added === false) {
        var li = document.createElement('li');
        var wrapper = document.createElement('div');
        var id = document.createElement('h5');
        var name = document.createElement('h6');
        var quantity = document.createElement('span');
        var price = document.createElement('span');
        var icon = document.createElement('i');
        var button = document.createElement('a');

        list.appendChild(li);
        li.appendChild(wrapper);
        wrapper.appendChild(id);
        wrapper.appendChild(name);
        wrapper.appendChild(button);
        wrapper.appendChild(price);
        button.appendChild(icon);
        name.appendChild(quantity);

        wrapper.classList = "dish-name";
        button.classList = "item-remove-btn";
        price.classList = "price money";
        icon.classList = "fa fa-close";
        quantity.classList = "quantity-buy";

        quantity.style.color = "#00cf2f";
        id.style.display = "none";
        button.href = "/removecart/" + itemID;

        id.innerHTML = itemID;
        quantity.innerHTML = "1";
        name.innerHTML += " x " + itemName;
        price.innerHTML = itemPrice;
    }

    console.log("calbakchihi");

    if (listEmpty === true) {
        setTimeout(function() { location.reload(); }, 500);
    } else {
        subtotalBill = $("#subtotal-bill");
        subtotalBill.html(parseInt(subtotalBill.html().replace(/\D/g, ''), 10) + parseInt(itemPrice));
    }

    callback();
}