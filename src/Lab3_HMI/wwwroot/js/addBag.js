$(function () {
    $(document).on('click', '.btn-add', function (e) {
        e.preventDefault();

        var controlForm = $('.controls div:first'),
            currentEntry = $(this).parents('.entry:first'),
            newEntry = $(currentEntry.clone()).appendTo(controlForm);

        newEntry.find('input').val('');
        controlForm.find('.entry:not(:last) .btn-add')
            .removeClass('btn-add add').addClass('btn-remove add')
            .removeClass('btn-success add').addClass('btn-danger add')
            .html('<span class="glyphicon glyphicon-minus"></span>');
    }).on('click', '.btn-remove add', function (e) {
        $(this).parents('.entry:first').remove();

        e.preventDefault();
        return false;
    });
});
