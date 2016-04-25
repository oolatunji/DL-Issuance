$(document).ready(function () {
    try {
        var currentUrl = window.location.href;
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        var userFunctions = user.Function;

        var exist = false;
        $.each(userFunctions, function (key, userfunction) {
            var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
            if (currentUrl == link) {
                exist = true;
            }
        });

        if (!exist)
            window.location.href = '../System/UnAuthorized';
        else
            getFines();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Fine Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getFines() {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/FineAPI/RetrieveFines',

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "LicenseID" },
            { "data": "Lastname" },
            { "data": "Othernames" },
            { "data": "DateOfBirth" },
            { "data": "PhoneNumber" },
            { "data": "EmailAddress" },
            {
                "data": "MaritalStatus",
                "visible": false
            },
            {
                "data": "Sex",
                "visible": false
            },
            {
                "data": "Religion",
                "visible": false
            },
            {
                "data": "MothersMaidenName",
                "visible": false
            },
            {
                "data": "Address",
                "visible": false
            },
            {
                "data": "Nationality",
                "visible": false
            },
            {
                "data": "Details",
                "visible": false
            },
            {
                "data": "Date",
                "visible": false
            }
        ],

        "order": [[1, "asc"]],

        dom: 'Bfrtip',

        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                }
            },
            {
                extend: 'csvHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                },
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ]
    });

    $('#example tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        function closeAll() {
            var e = $('#example tbody tr.shown');
            var rows = table.row(e);
            if (tr != e) {
                e.removeClass('shown');
                rows.child.hide();
            }
        }

        if (row.child.isShown()) {
            closeAll();
        }
        else {
            closeAll();

            row.child(formatDetails(row.data())).show();
            tr.addClass('shown');
        }
    });

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
};


$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function formatDetails(d) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Marital Status</td>';
    table += '<td>' + d.MaritalStatus + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Sex</td>';
    table += '<td>' + d.Sex + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Religion</td>';
    table += '<td>' + d.Religion + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Mothers Maiden Name</td>';
    table += '<td>' + d.MothersMaidenName + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Address</td>';
    table += '<td>' + d.Address + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Nationality</td>';
    table += '<td>' + d.Nationality + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Details</td>';
    table += '<td>' + d.Details + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Date</td>';
    table += '<td>' + d.Date + '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}