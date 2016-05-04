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
            getEnrolments();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Enrolment Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getEnrolments() {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/EnrolmentAPI/RetrieveEnrolments',

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "LicenseID" },
            { "data": "CardSerialNumber" },
            { "data": "Lastname" },
            { "data": "FirstName" },
            { "data": "MiddleName" },
            { "data": "NameOnCard" },
            { "data": "DateOfBirth" },
            { "data": "Branch" },
            { "data": "PrintStatus" },
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
                "data": "Nationality",
                "visible": false
            },
            {
                "data": "UtilityBill",
                "visible": false
            },
            {
                "data": "IDNumber",
                "visible": false
            },
            {
                "data": "LocalGovernmentArea",
                "visible": false
            },
            {
                "data": "BloodGroup",
                "visible": false
            },
            {
                "data": "LicenseType",
                "visible": false
            },
            {
                "data": "IssueDate",
                "visible": false
            },
            {
                "data": "ValidTillDate",
                "visible": false
            },
            {
                "data": "FileNumber",
                "visible": false
            },
            {
                "data": "EmailAddress",
                "visible": false
            },
            {
                "data": "PhoneNumber",
                "visible": false
            },
            {
                "data": "Address",
                "visible": false
            },
            {
                "data": "UserPrinting",
                "visible": false
            },
            {
                "data": "DateEnroled",
                "visible": false
            },
            {
                "data": "DatePrinted",
                "visible": false
            }
        ],

        "order": [[1, "asc"]],

        dom: 'Bfrtip',

        buttons: [
            {
                extend: 'copyHtml5'
            },
            {
                extend: 'csvHtml5'
            },
            {
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A2'
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
    table += '<td style="color:navy;width:20%;font-family:Arial;">Nationality</td>';
    table += '<td>' + d.Nationality + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Utility Bill</td>';
    table += '<td>' + d.UtilityBill + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">ID Number</td>';
    table += '<td>' + d.IDNumber + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Local Government Area</td>';
    table += '<td>' + d.LocalGovernmentArea + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Blood Group</td>';
    table += '<td>' + d.BloodGroup + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">License Type</td>';
    table += '<td>' + d.LicenseType + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Issued Date</td>';
    table += '<td>' + d.IssueDate + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Valid Till Date</td>';
    table += '<td>' + d.ValidTillDate + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">File Number</td>';
    table += '<td>' + d.FileNumber + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Email Address</td>';
    table += '<td>' + d.EmailAddress + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Phone Number</td>';
    table += '<td>' + d.PhoneNumber + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Address</td>';
    table += '<td>' + d.Address + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">User Printing</td>';
    table += '<td>' + d.UserPrinting + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Date Enroled</td>';
    table += '<td>' + d.DateEnroled + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Date Printed</td>';
    table += '<td>' + d.DatePrinted + '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}