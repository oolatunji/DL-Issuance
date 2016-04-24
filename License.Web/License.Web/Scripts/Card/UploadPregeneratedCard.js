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
        else {
            $('#branch').html('<option>Loading Branches...</option>');
            $('#branch').prop('disabled', 'disabled');

            //Get Branches
            $.ajax({
                url: settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    $('#branch').html('');
                    $('#branch').prop('disabled', false);
                    $('#branch').append('<option value="">Select Branch</option>');
                    var functions = response.data;
                    var html = '';
                    $.each(functions, function (key, value) {
                        $('#branch').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                    });
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText, "Card Management");
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Card Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function handleFiles(files) {
    // Check for the various File API support.
    if (window.FileReader) {
        // FileReader are supported.
        getAsText(files[0]);
    } else {
        displayMessage("error", 'FileReader are not supported in this browser.', "Card Management");
    }
}

function getAsText(fileToRead) {
    var reader = new FileReader();
    // Read file into memory as UTF-8      
    if (fileToRead.type != 'application/vnd.ms-excel') {
        displayMessage("error", 'Invalid file. File type expected is: .csv', "Card Management");
    } else {
        reader.readAsText(fileToRead);
        // Handle errors load
        reader.onload = loadHandler;
        reader.onerror = errorHandler;
    }
}

function loadHandler(event) {
    var csv = event.target.result;
    processData(csv);
}

function processData(csv) {
    var allTextLines = csv.split(/\r\n|\n/);
    var tarr = [];
    for (var i = 1; i < allTextLines.length; i++) {
        var data = allTextLines[i].split(';');
        for (var j = 0; j < data.length; j++) {
            var line = data[j].split(',');
            if (line.length == 1) {
                tarr.push(data[j]);
            }
        }
    }
    if (_.size(tarr) == 0) {
        displayMessage("error", 'Invalid file content!', "Card Management");
    } else {
        window.sessionStorage.setItem('cards', JSON.stringify(tarr));
    }
}

function errorHandler(evt) {
    if (evt.target.error.name == "NotReadableError") {
        displayMessage("error", 'Cannot read file!', "Card Management");
    }
}


function uploadPregeneratedCard() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');
        $("#addBtn").attr("disabled", "disabled");

        var cards = JSON.parse(window.sessionStorage.getItem('cards'));
        var branch = $('#branch').val();

        if (_.size(cards) == 0) {
            displayMessage("error", 'Invalid file content!', "Card Management");
        } else {
            if (branch == 0) {
                displayMessage("error", 'Branch is required!', "Card Management");
            } else {
                var data = { CardNumbers: cards, Branch: branch };
                $.ajax({
                    url: settingsManager.websiteURL + 'api/CardAPI/UploadPregeneratedCards',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {
                        displayMessage("success", response, "Card Management");
                        $('#branch').val('');

                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
                    },
                    error: function (xhr) {
                        displayMessage("error", 'Error experienced: ' + xhr.responseText, "Card Management");
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
                    }
                 });
            }
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Card Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}