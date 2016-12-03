var DragdealersManager = function () {
    var rainbow = getRainbowGradient();
    var criteriaNames = [
        'sand-quality',
        'beach-cleanliness',
        'beautiful-scenery',
        'crowd-free',
        'infrastructure',
        'water-visibility',
        'litter-free',
        'feet-friendly-bottom',
        'sea-life-diversity',
        'coral-reef',
        'snorkeling',
        'kayaking',
        'walking',
        'camping',
        'long-term-stay'
    ];

    function initializeDragdealers() {
        var options = {
            disabled: 'true',
            steps: 101,
            x: 0.5,
            animationCallback: function (x) {
                if (this.disabled) {
                    return;
                }

                var step = Math.floor(x * 100);
                var wrapperColor = ('#' + rainbow.colourAt(step));
                var score = (Math.round(step * 10) / 100);

                $(this.wrapper).css('background-color', wrapperColor);
                $(this.handle).text(score);
            }
        };

        for (var i = 0; i < criteriaNames.length; i++) {
            var criterionId = (criteriaNames[i] + '-dragdealer');
            var dragdealer = new Dragdealer(criterionId, options);

            attachDragdealerClickEventListener(dragdealer, i);
        }
        
        function attachDragdealerClickEventListener(dragdealer, i) {
            var handleDataAttribute = ('[data-' + criteriaNames[i] + '-handle]');
            var criterionValue = $(handleDataAttribute).data(criteriaNames[i] + '-handle');
            var handleXPosition = (criterionValue / 10);
                
            if (criterionValue !== '') {
                dragdealer.enable();

                dragdealer.setValue(handleXPosition, 0);
                $(dragdealer.wrapper).css('background-color', ('#' + rainbow.colourAt(criterionValue * 10)));
                $(dragdealer.handle).text(criterionValue);

                setResetButtonEvents();
            } else {
                disableDragdealer(dragdealer);
            }

            $(handleDataAttribute).on('mousedown', function () {
                setResetButtonEvents();
                dragdealer.enable();

                $(dragdealer.wrapper).css('background-color', ('#' + rainbow.colourAt(50)));
                $(dragdealer.handle).text('5');
            });

            function setResetButtonEvents() {
                var resetButton = $(dragdealer.wrapper).parent().next().find('.btn-reset');

                if (!resetButton.is(':visible')) {
                    resetButton.show();

                    resetButton.on('click', function (event) {
                        event.preventDefault();

                        disableDragdealer(dragdealer);

                        $(this).hide();
                    });
                }
            }

            function disableDragdealer(dragdealer) {
                dragdealer.disable();

                dragdealer.setValue(0.5, 0);
                $(dragdealer.wrapper).css('background-color', '#EEE');
                $(dragdealer.handle).text('');
            }
        }
    }

    function initializeMetersForDisplay($container) {
        for (var i = 0; i < criteriaNames.length; i++) {
            var criterionId = ('#' + criteriaNames[i] + '-meter');
            var $meter = $container.find(criterionId);
            var $scoreBox = $meter.parent().siblings('.criterion-score-box');
            var $scoreContainer = $scoreBox.find('div');
            var scoreOriginal = $scoreContainer.text();
            var score = (scoreOriginal.length === 1) ? (scoreOriginal + '.0') : scoreOriginal;
            var step = Math.floor(score * 10);
            var color = ('#' + rainbow.colourAt(step));

            if (!score.length) {
                $meter.closest('.form-group').remove();

                continue;
            }

            $scoreContainer.text(score);
            $scoreBox.css('background-color', color);
            $meter.css('background-color', color);
            $meter.width(step + '%');
            $meter.height('100%');
        }
    }

    function getRainbowGradient() {
        var rainbow = new Rainbow();

        rainbow.setSpectrum('#0074ff', '#00ffd0', '#FFd200', '#FF0a00');

        return rainbow;
    }

    return {
        initializeDragdealers: initializeDragdealers,
        initializeMeters: initializeMetersForDisplay
    }
};