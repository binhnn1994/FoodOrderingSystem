function showItemList() {
    var request = new XMLHttpRequest();

    var url, content;

    var searchValue = document.getElementById("search-value");
    if (searchValue.length > 0) {
        url = "/api/AdminDashboard/ViewItemListBySearching";
        content = '{"SearchValue": "Rice", "CategoryName": "all", "Status": "all", "RowsOnPage": 100, "RequestPage": 1}';
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
            addFilter();
        });
    };
    request.send(content);
}

function renderItemList(itemList, callback) {
    var list = document.getElementById("item-list");

    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }

    for (let i = 0; i < itemList.length; i++) {
        var item = document.createElement('div');
        var box = document.createElement('div');
        var thumb = document.createElement('div');
        var info = document.createElement('div');
        var button = document.createElement('div');
        var image = document.createElement('img');
        var id = document.createElement('h3');
        var name = document.createElement('h4');
        var category = document.createElement('h5');
        var description = document.createElement('p');
        var status = document.createElement('h6');
        var price = document.createElement('span');
        var editBtn = document.createElement('a');

        list.appendChild(item);
        item.appendChild(box);
        box.appendChild(thumb);
        box.appendChild(info);
        box.appendChild(button);
        thumb.appendChild(image);
        info.appendChild(id);
        info.appendChild(name);
        info.appendChild(category);
        info.appendChild(description);
        info.appendChild(status);
        button.appendChild(price);
        button.appendChild(editBtn);

        item.classList = "col-md-12 col-sm-12 col-lg-6 filter-item filter-item1";
        box.classList = "featured-restaurant-box";
        thumb.classList = "featured-restaurant-thumb";
        info.classList = "featured-restaurant-info";
        button.classList = "ord-btn";
        image.classList = "brd-rd50";
        price.classList = "price money";
        editBtn.classList = "brd-rd2 edit-popup-btn";

        id.style = "display: none";
        box.style.height = "181px";

        editBtn.title = "Edit";
        editBtn.href = "#";

        image.src = "../images/resource/featured-restaurant-img1.jpg";
        image.alt = "featured-restaurant-img1.jpg";

        id.innerHTML = itemList[i].itemID;
        name.innerHTML = itemList[i].itemName;
        category.innerHTML = itemList[i].categoryName;
        description.innerHTML = itemList[i].description;
        status.innerHTML = itemList[i].status === "Active" ? "Available" : "Unavailable";
        price.innerHTML = itemList[i].unitPrice;
        editBtn.innerHTML = "Edit";
    }

    callback();

    //===== Edit Popup Script =====//
    $('.edit-popup-btn').on('click', function() {
        $('html').addClass('edit-popup-active');

        var $info = $(this).parent().parent().find('.featured-restaurant-info');
        var $button = $(this).parent().parent().find('.ord-btn');

        $('#dish-id-edit').val($info.find('h3').text());
        $('#dish-name-edit').val($info.find('h4').text());
        $('#dish-category-edit').val($info.find('h5').text());
        $('#dish-price-edit').val($button.find('.price').text().replace(/\D/g, ''));
        $('#dish-description-edit').val($info.find('p').text());

        getCategories();

        if ($info.find('h6').text() === "Available") {
            $('#dish-available').prop("checked", true);
        } else {
            $('#dish-unavailable').prop("checked", true);
        }

        return false;
    });

    $('.edit-close-btn, .edit-submit-btn').on('click', function() {
        $('html').removeClass('edit-popup-active');
        return false;
    });
}

function getCategories() {
    var request = new XMLHttpRequest();
    var url = "GetCategories";

    request.open('GET', url, true);
    request.onload = function() {
        var result = JSON.parse(this.responseText);
        var listCategories = document.getElementById("categories");
        for (let i = 0; i < result.length; i++) {
            var category = document.createElement("option");
            category.textContent = result[i];
            listCategories.appendChild(category);
        }
    };
    request.send();
}

function updateItem() {
    var inputs = document.forms["edit-form"].elements;

    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/UpdateItemInformation";
    var content = 'itemID=' + inputs[0].value;
    if (inputs[1].value.length > 0) {
        content += "&itemName=" + encodeURIComponent(inputs[1].value);
    }
    if (inputs[2].value.length > 0) {
        content += "&categoryName=" + encodeURIComponent(inputs[2].value);
    }
    if (inputs[3].value.length > 0) {
        content += "&unitPrice=" + inputs[3].value;
    }
    if (inputs[4].value.length > 0) {
        content += "&image=" + inputs[4].value;
    }
    if (inputs[5].value.length > 0) {
        content += "&description=" + encodeURIComponent(inputs[5].value);
    }

    if (inputs[6].checked === true) {
        activateItem(inputs[0].value);
    }
    if (inputs[6].checked === false) {
        deactivateItem(inputs[0].value);
    }

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.onload = function() {
        // showItemList();
        location.reload();
    };
    request.send(content);
}

function createItem() {
    var inputs = document.forms["add-form"].elements;

    var request = new XMLHttpRequest();
    var url = "/api/AdminDashboard/CreateItem";
    var content = "itemName=" + encodeURIComponent(inputs[0].value);
    content += "&categoryName=" + encodeURIComponent(inputs[1].value);
    content += "&unitPrice=" + inputs[2].value;
    content += "&image=" + inputs[3].value;
    content += "&description=" + encodeURIComponent(inputs[4].value);

    console.log(url);
    console.log(content);

    request.open('POST', url, true);
    request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    request.onload = function() {
        // showItemList();
        location.reload();
    };
    request.send(content);
}