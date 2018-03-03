$(document).ready(function () {
    var table = $('#table-games').DataTable({
        'responsive': true,
        'processing': true,
        'serverSide': true,
        'ajax': {
            'url': sourceLocation,
            'type': 'POST'
        },
        'columns': [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<button>Expand</button>'
            },
            { 'data': "id" },
            { 'data': "type" },
            { 'data': "name" },
            { 'data': "state" },
            { 'data': "playState" },
            { 'data': "mapTemplateName" },
            { 'data': "createdBy" },
            { 'data': "currentPlayer" },
            {
                'data': "id",
                'render': function (data, type, row, meta) {
                    return '<button onclick="deleteUser(\'' + data + '\')">Test</button>';
                }
            }
        ],
        'ordering': false,
        'info': false,
        'searchDelay': 1000
    });

    function deleteUser(id) {
        /*$.ajax({
        url: '@Url.Action("Delete", "Users")' + '?userId=' + id,
        type: 'POST',
        contentType: 'application/json',
        success: function(result) {
        alert("Removed");
        }
        });*/
    }

    function format(data) {
        var $content = $("<div></div>");
        $.get(viewLocation, { gameId: data.id }, result => {
            $content.html(result);
        });

        return $content;
    }

    // Add event listener for opening and closing details
    $('#table-games tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
});