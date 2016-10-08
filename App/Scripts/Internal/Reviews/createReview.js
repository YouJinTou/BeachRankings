﻿(function ($) {
    var criteriaNames = [
        'water-quality',
        'seafloor-cleanliness',
        'coral-reef',
        'sea-life-diversity',
        'snorkeling-suitability',
        'beach-cleanliness',
        'crowd-free-factor',
        'sand-quality',
        'breathtaking-environment',
        'tent-suitability',
        'kayak-suitability',
        'long-stay-suitability'
    ];
    var rainbow = getRainbowGradient();
    var options = {
        disabled: 'true',
        steps: 100,
        x: 0.5,
        animationCallback: function (x) {
            if (this.disabled) {
                return;
            }

            var dragdealer = this;
            var resetButton = $(dragdealer.wrapper).parent().nextUntil('reset-button');

            if (!resetButton.is(':visible')) {
                resetButton.show();

                resetButton.on('click', function (event) {
                    event.preventDefault();

                    dragdealer.disable();
                    dragdealer.setValue(0.5, 0);
                    $(dragdealer.wrapper).css('background-color', '#EEE');
                    $(dragdealer.handle).text('');

                    $(this).hide();
                });
            }

            var step = Math.floor(x * 100);
            var wrapperColor = ('#' + rainbow.colourAt(step));
            var score = (Math.round(step * 10) / 100);

            $(dragdealer.wrapper).css('background-color', wrapperColor);
            $(dragdealer.handle).text(score);
        }
    };

    $('[data-btn-post-review]').on('click', function (event) {
        event.preventDefault();

        var reviewJsonData = {
            content: $('[data-review-content]').val(),
            waterQuality: $('[data-water-quality-handle]').text(),
            seafloorCleanliness: $('[data-seafloor-cleanliness-handle]').text(),
            coralReefFactor: $('[data-coral-reef-handle]').text(),
            seaLifeDiversity: $('[data-sea-life-diversity-handle]').text(),
            snorkelingSuitability: $('[data-snorkeling-suitability-handle]').text(),
            beachCleanliness: $('[data-beach-cleanliness-handle]').text(),
            crowdFreeFactor: $('[data-crowd-free-factor-handle]').text(),
            sandQuality: $('[data-sand-quality-handle]').text(),
            breathtakingEnvironment: $('[data-breathtaking-environment-handle]').text(),
            tentSuitability: $('[data-tent-suitability-handle]').text(),
            kayakSuitability: $('[data-kayak-suitability-handle]').text(),
            longStaySuitability: $('[data-long-stay-suitability-handle]').text()
        };
        var reviewForm = $('#submitReviewForm');
        var csrfToken = $('input[name="__RequestVerificationToken"]', reviewForm).val();

        if (reviewForm.valid()) {
            $.ajax({
                url: '/Reviews/Post/',
                type: 'POST',
                data: {
                    __RequestVerificationToken: csrfToken,
                    bindingModel: reviewJsonData
                },
                success: function (result) {
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                }
            });
        }
    })

    initializeDragdealers();

    function getRainbowGradient() {
        var rainbow = new Rainbow();

        rainbow.setSpectrum('#0074ff', '#00ffd0', '#FFd200', '#FF0a00');

        return rainbow;
    }

    function initializeDragdealers() {
        for (var i = 0; i < criteriaNames.length; i++) {
            var criterionId = (criteriaNames[i] + '-dragdealer');
            var dragdealer = new Dragdealer(criterionId, options);

            attachDragdealerClickEventListener(dragdealer, i);
        }
    }

    function attachDragdealerClickEventListener(dragdealer, i) {
        var handleDataAttribute = ('[data-' + criteriaNames[i] + '-handle]');

        $(handleDataAttribute).on('mousedown', function () {
            dragdealer.enable();

            $(dragdealer.wrapper).css('background-color', ('#' + rainbow.colourAt(50)));
            $(dragdealer.handle).text('5');
        });
    }
})(jQuery);