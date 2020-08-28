
// var url =   "https://tsg-dvds.herokuapp.com/";
$(document).ready(function () {
    loadDvds();

    // Add Button onclick handler
    $('#add-button').click(function (event) {

        // check for errors and display any that we have
        // pass the input associated with the add form to the validation function
        var haveValidationErrors = checkAndDisplayValidationErrors($('#add-form').find('input'));

        // if we have errors, bail out by returning false
        if (haveValidationErrors) {
            return false;
        }

        var year = $('#add-releaseYear').val();
        if (isNaN(year) || year.length != 4) {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Please enter a 4-digit year.'));
            return false;
        }

        // if we made it here, there are no errors so make the ajax call
        $.ajax({
            type: 'POST',
         //    'https://tsg-dvds.herokuapp.com/dvd',
            url: 'https://localhost:44344/api/home/dvd',
            // localhost:44344
            data: JSON.stringify({
                Title: $('#add-title').val(),
                ReleaseYear: $('#add-releaseYear').val(),
                Director: $('#add-director').val(),
                Rating: $('#add-rating').val(),
                Notes: $('#add-notes').val()
            }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            success: function (data, status) {
                // clear errorMessages
                $('#errorMessages').empty();
                // // Clear the form and reload the table
                hideAddForm();
                loadDvds();
            },
            error: function () {
                $('#errorMessages')
                    .append($('<li>')
                        .attr({ class: 'list-group-item list-group-item-danger' })
                        .text('Error calling web service.  Please try again later.'));
            }
        });
    });

    // Update Button onclick handler
    $('#edit-button').click(function (event) {

        // check for errors and display any that we have
        // pass the input associated with the edit form to the validation function
        var haveValidationErrors = checkAndDisplayValidationErrors($('#edit-form').find('input'));

        // if we have errors, bail out by returning false
        if (haveValidationErrors) {
            return false;
        }

        var year = $('#edit-releaseYear').val();
        if (isNaN(year) || year.length != 4) {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Please enter a 4-digit year.'));
            return false;
        }

        // if we get to here, there were no errors, so make the Ajax call
        $.ajax({
            type: 'PUT',
         //   url: 'https://tsg-dvds.herokuapp.com/dvd/' + $('#edit-dvd-id').val(),
            url: 'https://localhost:44344/api/home/dvd/' + $('#edit-dvd-id').val(),
            data: JSON.stringify({
                DvdId: $('#edit-dvd-id').val(),
                Title: $('#edit-title').val(),
                ReleaseYear: $('#edit-releaseYear').val(),
                Director: $('#edit-director').val(),
                Rating: $('#edit-rating').val(),
                Notes: $('#edit-notes').val()
            }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            // 'dataType': 'json',
            success: function (data, status) {
                // clear errorMessages
                $('#errorMessages').empty();
                hideEditForm();
                loadDvds();
            },
            error: function (data, status) {
                // console.log(data, status);
                $('#errorMessages')
                    .append($('<li>')
                        .attr({ class: 'list-group-item list-group-item-danger' })
                        .text('Error calling web service.  Please try again later.'));
                //         hideEditForm();
                //         loadDvds();
            }
        });
    });

    $('#search-button').click(function (event) {

        var category = $('#search-category').val();
        var term = $('#search-term').val();
        if (category == '' || term == '') {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Both Search Category and Search Term are required.'));
            return false;
        }

        var contentRows = $('#contentRows');
        clearDvdTable();

        $.ajax({
            type: 'GET',
           // url: 'https://tsg-dvds.herokuapp.com/dvds/' + category + '/' + term,
            url: 'https://localhost:44344/api/home/dvds/' + category + '/' + term,
            success: function (data, status) {
                $('#errorMessages').empty();
                $.each(data, function (index, dvd) {
                    var title = dvd.Title;
                    var releaseYear = dvd.ReleaseYear;
                    var director = dvd.Director;
                    var rating = dvd.Rating;
                    var id = dvd.DvdID;

                    var row = '<tr>';
                    row += '<td><a onclick="showDvdDetails(' + id + ')">' + title + '</a></td>';
                    row += '<td>' + releaseYear + '</td>';
                    row += '<td>' + director + '</td>';
                    row += '<td>' + rating + '</td>';
                    row += '<td style="text-align: right;"><a onclick="showEditForm(' + id + ')">Edit</a></td>';
                    row += '<td>|</td>';
                    row += '<td style="text-align: left;"><a onclick="deleteDvd(' + id + ')">Delete</a></td>';
                    row += '</tr>';
                    contentRows.append(row);
                });
            },
            error: function () {
                $('#errorMessages')
                    .append($('<li>')
                        .attr({ class: 'list-group-item list-group-item-danger' })
                        .text('Error calling web service.  Please try again later.'));
            }
        });
    });
});

function loadDvds() {
    clearDvdTable();
    var contentRows = $('#contentRows');
    $.ajax({
        type: 'GET',
      //  url: 'https://tsg-dvds.herokuapp.com/dvds',
        url: 'https://localhost:44344/api/home/dvds',
        success: function (data, status) {
            $.each(data, function (index, dvd) {
                var title = dvd.Title;
                var releaseYear = dvd.ReleaseYear;
                var director = dvd.Director;
                var rating = dvd.Rating;
                var id = dvd.DvdID;

                var row = '<tr>';
                row += '<td><a onclick="showDvdDetails(' + id + ')">' + title + '</a></td>';
                row += '<td>' + releaseYear + '</td>';
                row += '<td>' + director + '</td>';
                row += '<td>' + rating + '</td>';
                row += '<td style="text-align: right;"><a onclick="showEditForm(' + id + ')">Edit</a></td>';
                row += '<td>|</td>';
                row += '<td style="text-align: left;"><a onclick="deleteDvd(' + id + ')">Delete</a></td>';
                row += '</tr>';
                contentRows.append(row);
            });
        },
        error: function () {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Error calling web service.  Please try again later.'));
        }
    });
}

function clearDvdTable() {
    $('#contentRows').empty();
}

function showEditForm(id) {
    // clear errorMessages
    $('#errorMessages').empty();
    $('#topRowForm').hide();
    // get the contact details from the server and then fill and show the
    // form on success
    $.ajax({
        type: 'GET',
      //  url: 'https://tsg-dvds.herokuapp.com/dvd/' + id,
        url: 'https://localhost:44344/api/home/dvd/' + id,
        success: function (dvd, status) {
            // console.log(dvd.rating);
            $('#edit-h2').append("Edit DVD: " + dvd.Title);
            $('#edit-title').val(dvd.Title);
            $('#edit-releaseYear').val(dvd.ReleaseYear);
            $('#edit-director').val(dvd.Director);
            $('#edit-rating').val(dvd.Rating);
            $('#edit-notes').val(dvd.Notes);
            $('#edit-dvd-id').val(dvd.DvdID);
        },
        error: function () {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Error calling web service.  Please try again later.'));
        }
    });
    $('#dvdTableDiv').hide();
    $('#editFormDiv').show();
}

function hideEditForm() {
    // clear errorMessages
    $('#errorMessages').empty();
    // clear the form and then hide it
    $('#edit-h2').empty();
    $('#edit-title').val('');
    $('#edit-releaseYear').val('');
    $('#edit-director').val('');
    $('#edit-rating').val('');
    $('#edit-notes').val('');
    $('#edit-dvd-id').val('');
    $('#editFormDiv').hide();
    //show the home screen with controls and dvd table
    $('#topRowForm').show();
    $('#dvdTableDiv').show();
}

function showAddForm() {
    $('#errorMessages').empty();
    $('#topRowForm').hide();
    $('#dvdTableDiv').hide();
    $('#addFormDiv').show();
}

function hideAddForm() {
    // clear errorMessages
    $('#errorMessages').empty();
    // clear the form and then hide it
    $('#add-title').val('');
    $('#add-releaseYear').val('');
    $('#add-director').val('');
    $('#add-rating').val('');
    $('#add-notes').val('');
    $('#addFormDiv').hide();
    $('#topRowForm').show();
    $('#dvdTableDiv').show();
}

function deleteDvd(id) {
    var areYouSure = confirm("Are you sure you want to delete this DVD from your collection?");
    if (!areYouSure) return false;
    $.ajax({
        type: 'DELETE',
      //  url: "https://tsg-dvds.herokuapp.com/dvd/" + id,
        url: 'https://localhost:44344/api/home/dvd/' + id,
        success: function (status) {
            loadDvds();
        }
    });

}

function showDvdDetails(id) {
    $('#errorMessages').empty();
    $("#topRowForm").hide();

    $.ajax({
        type: 'GET',
        //url: 'https://tsg-dvds.herokuapp.com/dvd/' + id,
        url: 'https://localhost:44344/api/home/dvd/' + id,
        success: function (dvd, status) {
            // clear errorMessages
            //$('#errorMessages').empty();
            $('#show-h2').append(" " + dvd.Title);
            $('#show-releaseYear').append("Release Year: " + dvd.ReleaseYear);
            $('#show-director').append("Director: " + dvd.Director);
            $('#show-rating').append("Rating: " + dvd.Rating);
            $('#show-notes').append("Notes: " + dvd.Notes);
            $('#dvdTableDiv').hide();
            $('#showDvdDiv').show();
        },
        error: function () {
            $('#errorMessages')
                .append($('<li>')
                    .attr({ class: 'list-group-item list-group-item-danger' })
                    .text('Error calling web service.  Please try again later.'));
        }
    });
}

function hideDvdDetails() {
    $('#errorMessages').empty();
    $('#show-h2').empty();
    $('#show-releaseYear').empty();
    $('#show-director').empty();
    $('#show-rating').empty();
    $('#show-notes').empty();
    $('#showDvdDiv').hide();
    $('#topRowForm').show();
    $('#dvdTableDiv').show();
}

function checkAndDisplayValidationErrors(input) {
    // clear displayed error message if there are any
    $('#errorMessages').empty();
    // check for HTML5 validation errors and process/display appropriately
    // a place to hold error messages
    var errorMessages = [];

    // loop through each input and check for validation errors
    input.each(function () {
        // Use the HTML5 validation API to find the validation errors
        if (!this.validity.valid) {
            var errorField = $('label[for=' + this.id + ']').text();
            errorMessages.push(errorField + ' ' + this.validationMessage);
        }
    });

    // put any error messages in the errorMessages div
    if (errorMessages.length > 0) {
        $.each(errorMessages, function (index, message) {
            $('#errorMessages').append($('<li>').attr({ class: 'list-group-item list-group-item-danger' }).text(message));
        });
        // return true, indicating that there were errors
        return true;
    } else {
        // return false, indicating that there were no errors
        return false;
    }
}
