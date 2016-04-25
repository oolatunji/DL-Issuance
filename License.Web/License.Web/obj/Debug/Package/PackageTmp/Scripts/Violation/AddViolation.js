$(document).ready(function () {

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
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function addViolation() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');
        $("#addBtn").attr("disabled", "disabled");

        var licenseID = $('#licenseID').val();
        var details = $('#details').val();

        if (_.isEmpty(licenseID) || _.isEmpty(details)) {
            displayMessage("error", 'License ID and Details fields are required.', "Violation Management");
        } else {
            var data = { LicenseID: licenseID, Details: details };
            $.ajax({
                url: settingsManager.websiteURL + 'api/ViolationAPI/SaveViolation',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (response) {
                    displayMessage("success", response, "Violation Management");
                    $('#licenseID').val('');
                    $('#details').val('');
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-cog"></i> Add');
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText, "Violation Management");
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-cog"></i> Add');
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Violation Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}