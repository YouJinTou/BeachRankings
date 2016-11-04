(function ($) {
    $(document).ready(function () {
        $('#user-reviews-grid').DataTable({
            scrollX: 300,
            scrollY: 400,
            fixedColumns: {
                leftColumns: 3
            },
            fixedHeader: true
        });
    });
})(jQuery);