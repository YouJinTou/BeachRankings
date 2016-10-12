(function ($) {
    var mode = (window.location.href.indexOf('Rate/') > -1) ? "post" : "edit";
    var criteriaNames = [
        'sand-quality',
        'beach-cleanliness',
        'beautiful-scenery',
        'crowd-free',
        'water-purity',
        'wastefree-seabed',
        'feet-friendly-bottom',
        'sea-life-diversity',
        'coral-reef',
        'walking',
        'snorkeling',
        'kayaking',
        'camping',
        'infrastructure',
    ];
    var rainbow = getRainbowGradient();
    var options = {
        disabled: 'true',
        steps: 101,
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

                    disableDragdealer(dragdealer);

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
    var post = (mode === "post");
    var edit = (mode === "edit");

    initializeDragdealers();    

    $('[data-btn-submit-review]').on('click', function (event) {
        event.preventDefault();
               
        var actionUrl = post ? '/Reviews/Rate/' : '/Reviews/Edit/';
        var reviewJsonData = {
            content: $('[data-review-content]').val(),
            sandQuality: $('[data-sand-quality-handle]').text(),
            beachCleanliness: $('[data-beach-cleanliness-handle]').text(),
            beautifulScenery: $('[data-beautiful-scenery-handle]').text(),
            crowdFree: $('[data-crowd-free-handle]').text(),
            waterPurity: $('[data-water-purity-handle]').text(),
            wasteFreeSeabed: $('[data-wastefree-seabed-handle]').text(),
            feetFriendlyBottom: $('[data-feet-friendly-bottom-handle]').text(),
            coralReef: $('[data-coral-reef-handle]').text(),
            seaLifeDiversity: $('[data-sea-life-diversity-handle]').text(),
            walking: $('[data-walking-handle]').text(),
            snorkeling: $('[data-snorkeling-handle]').text(),
            kayaking: $('[data-kayaking-handle]').text(),
            camping: $('[data-camping-handle]').text(),
            infrastructure: $('[data-infrastructure-handle]').text()
        };        
        var reviewForm = $('#submitReviewForm');
        var csrfToken = $('input[name="__RequestVerificationToken"]', reviewForm).val();
        var modelData = getModelData();
        
        reviewJsonData[modelData.modelName] = modelData.modelId;
        
        if (reviewForm.valid()) {
            $.ajax({
                url: actionUrl,
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
    });

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

    function disableDragdealer(dragdealer) {
        dragdealer.disable();

        dragdealer.setValue(0.5, 0);
        $(dragdealer.wrapper).css('background-color', '#EEE');
        $(dragdealer.handle).text('');
    }

    function attachDragdealerClickEventListener(dragdealer, i) {
        var handleDataAttribute = ('[data-' + criteriaNames[i] + '-handle]');

        if (edit) {
            var criterionValue = $(handleDataAttribute).data(criteriaNames[i] + '-handle');
            var handleXPosition = (criterionValue / 10);

            if (criterionValue !== "") {
                dragdealer.enable();

                dragdealer.setValue(handleXPosition, 0);
                $(dragdealer.wrapper).css('background-color', ('#' + rainbow.colourAt(criterionValue * 10)));
                $(dragdealer.handle).text(criterionValue);
            } else {
                disableDragdealer(dragdealer);
            }
        }

        $(handleDataAttribute).on('mousedown', function () {
            dragdealer.enable();

            $(dragdealer.wrapper).css('background-color', ('#' + rainbow.colourAt(50)));
            $(dragdealer.handle).text('5');
        });
    }

    function getModelData() {
        var pattern = /^.*?(\d+)\D*$/;
        var modelId = pattern.exec(window.location.href)[1];
        var modelName = post ? "beachId" : "reviewId";

        return {
            modelId: modelId,
            modelName: modelName
        }
    }
})(jQuery);