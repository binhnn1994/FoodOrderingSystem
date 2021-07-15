function showReport() {
    $('#date-not-exist-error').hide();
    $('#end-start-error').hide();

    var dateFrom = document.getElementById("date-from").value;
    var dateTo = document.getElementById("date-to").value;
    console.log(dateFrom + " - " + dateTo);

    if (dateFrom.length === 0 || dateTo.length === 0) {
        $('#date-not-exist-error').show();
    } else if (dateTo < dateFrom) {
        $('#end-start-error').show();
    } else {
        var request = new XMLHttpRequest();
        var url = "/api/AdminDashboard/SaleReport";
        var content = '{"FromDate": "' + dateFrom + 'T00:00:00", "ToDate": "' + dateTo + 'T23:59:59"}';

        request.open('POST', url, true);
        request.setRequestHeader("Content-Type", "text/json");
        request.onload = function() {
            var result = JSON.parse(this.responseText);
            renderReport(result);
        };
        request.send(content);
    }
}

function renderReport(reportContent, callback) {
    $(document).ready(function() {
        statTable = $('#report-table-area').DataTable({
            destroy: true,
            processing: true,
            data: reportContent,
            columns: [{
                data: null
            }, {
                data: 'itemName'
            }, {
                data: 'categoryName'
            }, {
                data: 'totleQuantity'
            }, {
                data: 'totalSales',
                render: $.fn.dataTable.render.number(',', '.', 0, '', '')
            }],
            columnDefs: [{
                    "targets": [3],
                    "className": "dt-body-right"
                },
                {
                    "targets": [4],
                    "className": "dt-body-right money"
                },
                {
                    "targets": [1],
                    "className": "dt-body-left"
                }, {
                    "orderable": false,
                    "targets": 0
                },
                {
                    "searchable": false,
                    "targets": [0, 3, 4]
                }, {
                    "targets": "_all"
                }
            ],
            order: [
                [4, 'desc']
            ],
            language: {
                "lengthMenu": "_MENU_ &nbsp; items per page",
                "zeroRecords": "No matching items found",
                "info": "Page _PAGE_ of _PAGES_ pages",
                "infoEmpty": "No information",
                "infoFiltered": "(filter from _MAX_ items)",
                "search": "Item name: "
            }
        });

        statTable.on('order.dt', function() {
            statTable.column(0, { order: 'applied' }).nodes().each(function(cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });
}