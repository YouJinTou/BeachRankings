var SiteManager = function () {
    var $loadingImageContainer = $('.loading');

    function init() {
        setEvents();
        setResponsive();
        setInterval(keepSessionAlive, 4 * 60 * 1000);
    }

    function showSpinner() {
        $loadingImageContainer.show();
    }

    function hideSpinner() {
        $loadingImageContainer.hide();
    }

    function setEvents() {
        setMiscEvents();        

        function setMiscEvents() {
            $('#results-container').on('click', '.info-icon', function () {
                var $popupContainer = $(this).siblings('.custom-popup-container');
                var $popup = $(this).siblings('.custom-popup');
                var $popupDt = $(this).siblings('.custom-popup-dt');

                $popupContainer.toggle();
                $popup.toggle();
                $popupDt.toggle();
            });

            $('#results-container').on('mouseenter mouseleave', '.info-icon-unclickable', function () {
                var $popup = $(this).siblings('.custom-popup');

                $popup.toggle();

                keepElementWithinViewport($popup);
            });
        }
    }

    function setResponsive() {
        ensureMarginBetweenHeaderAndMainContainer();

        function ensureMarginBetweenHeaderAndMainContainer() {
            var $headerWrapper = $('.header-wrapper');
            var headerWrapperBottom = $headerWrapper.height();
            var $resultsContainer = $('#results-container');
            var resultsContainerTop = $resultsContainer.offset().top;

            if (headerWrapperBottom >= resultsContainerTop) {
                var secureMargin = headerWrapperBottom + 10;

                $resultsContainer.css('margin-top', secureMargin + 'px');
            }
        }
    }

    function keepElementWithinViewport($elem) {
        if (!$elem || $elem.length === 0) {
            return;
        }

        var pos = $elem.offset();
        var height = $elem.height();
        var width = $elem.width();
        var bottom = pos.top + height;
        var right = pos.left + width;
        var viewportWidth = $(window).width();
        var viewportHeight = $(window).height();

        if (pos.left < 0) {
            $elem.css({ left: 10 + 'px' });
        }

        if (pos.top < 0) {
            $elem.css({ top: 10 + 'px' });
        }

        if (bottom > viewportHeight) {
            $elem.css({ top: pos.top - height + 'px' });
        }

        if (right > viewportWidth) {
            $elem.css({ top: pos.left - width + 'px' });
        }
    }

    function keepSessionAlive() {
        $.get('/Home/KeepAlive');
    };

    return {
        init: init,
        showSpinner: showSpinner,
        hideSpinner: hideSpinner
    }
};

var siteManager = new SiteManager();

siteManager.init();