// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Wire up all of the checkboxes to run markCompleted()
    $('.done-checkbox').on('click', function (e) {
        markCompleted(e.target);
    });

    $('.save-btn').on('click', function (e) {
        updateItem(e.target);
    });

});

function markCompleted(checkbox) {
    checkbox.disabled = true;

    var row = checkbox.closest('tr'); //get this table row
    $(row).addClass('done');

    var form = checkbox.closest('form');
    form.submit();
}

function updateItem(btn) {
    var form = btn.closest('form');
    form.submit();
}