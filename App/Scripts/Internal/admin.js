﻿(function ($) {
    var $ddlDivisions = $('[data-ddl-division]');
    var divisionHolder = '[data-division-holder]';
    var movingBeaches = false;
    var lastDivisionReached = false;
    var lastDivisionSelected = false;
    var $currentDivision;

    setDivisionEvents();
    setCrudControlEvents();

    function setDivisionEvents() {
        $ddlDivisions.on('change', function () {
            var $this = $(this);
            var nextDivision = $this.data('next-division');
            var url = $this.data('ddl-division') + $this.val();
            var currentAddress = getCurrentAddress();
            lastDivisionReached = (nextDivision.length === 0);
            $currentDivision = $this;

            updateAdminControls($this);
            $('#move-beaches-container').show();

            if (lastDivisionReached) {
                lastDivisionSelected = ($this.val() !== '');

                tryClearCheckBoxesContainer();
                prepareBeachMovement();

                return;
            }

            var $nextDivision = $($ddlDivisions[nextDivision]);

            if (!$this.val()) {
                setDivisionDropdownsVisibility(false);
                tryHideBeachMovementContainer();
                tryClearCheckBoxesContainer();
                $('#btn-move-beaches-confirm').hide();

                return;
            }

            $.getJSON(url, function (result) {
                setDivisionDropdownsVisibility(true);

                gMapManager.setMapByAddress(currentAddress);

                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($nextDivision);
                });

                lastDivisionReached = (result.length === 0);
                lastDivisionSelected = ($this.val() !== '');

                tryClearCheckBoxesContainer();
                prepareBeachMovement();
            });

            function setDivisionDropdownsVisibility(showingClosest) {
                $nextDivision.closest(divisionHolder).show();

                for (var i = nextDivision; i < $ddlDivisions.length; i++) {
                    $($ddlDivisions[i]).empty();
                    createInitialOption($($ddlDivisions[i]), '-- Choose an area --');

                    if (!showingClosest) {
                        $($ddlDivisions[i]).closest(divisionHolder).hide();
                    }
                }

                for (var i = nextDivision + 1; i < $ddlDivisions.length; i++) {
                    $($ddlDivisions[i]).closest(divisionHolder).hide();
                }
            }

            function updateEditControl() {
                var divisionName = getDivisionName($this);
                var noName = (divisionName.indexOf('Choose') > -1);
                divisionName = noName ? '' : divisionName;

                $this.closest('.form-group').find('.edit-control').val(divisionName);
            }

            function createInitialOption(jQueryAppendee, text) {
                $(document.createElement('option'))
                            .attr('value', '')
                            .text(text)
                            .appendTo(jQueryAppendee);
            }

            function getCurrentAddress() {
                var currentAddress = '';

                for (var i = 0; i < $ddlDivisions.length; i++) {
                    var $division = $($ddlDivisions[i])
                    var text = getDivisionName($($ddlDivisions[i]));
                    var validText = (text && text.length > 0 && text.indexOf('-- Choose') === -1 && text.indexOf('undefined') === -1);

                    if (validText) {
                        currentAddress += text + ' ';
                    }
                }

                return currentAddress;
            }
        });
    }

    function setCrudControlEvents() {
        var form = '#restructure-form';
        var deleting = false;

        $('.restructure-button').on('click', function (event) {
            event.preventDefault();

            var $this = $(this);
            var $crudControls = $this.closest('.admin-crud-controls');
            var $editControl = $crudControls.find('.edit-control');
            var editControlReadOnly = $editControl.prop('readonly');
            var action = $this.data('action');

            switch (action) {
                case 'Add':
                    onAddClicked();

                    return;
                case 'Edit':
                    onEditClicked();

                    return;
                case 'Delete':
                    onDeleteClicked();

                    return;
                case 'Cancel':
                    onCancelClicked();
            }

            function onAddClicked() {
                if (editControlReadOnly) {
                    $editControl.val('');
                    setUiCues();
                } else {
                    $(form).attr('action', getControllerAction());
                    $(form).submit();
                }
            }

            function onEditClicked() {
                if (nothingSelected()) {
                    return;
                }

                if (editControlReadOnly) {
                    setUiCues();
                } else {
                    $(form).attr('action', getControllerAction());
                    $(form).submit();
                }
            }

            function onDeleteClicked() {
                if (nothingSelected()) {
                    return;
                }

                if (!deleting) {
                    deleting = true;
                    setUiCues();
                    $editControl.attr('readonly', true);
                } else {
                    var url = $currentDivision.data('ddl-division');
                    var secondSlashIndex = url.indexOf('/', 1);
                    var adminBeachesUrl = url.substr(0, secondSlashIndex) + '/AdminBeaches/' + $currentDivision.val();

                    $.getJSON(adminBeachesUrl, function (result) {
                        deleting = false;

                        if (result.length > 0) {
                            alert('You cannot delete a division that still has beaches in it. You must first move them to another division.');
                            resetCrudControls();
                        } else {
                            $(form).attr('action', getControllerAction());
                            $(form).submit();
                        }
                    });
                }
            }

            function onCancelClicked() {
                resetCrudControls();
            }

            function resetCrudControls() {
                var $ddlDivision = $crudControls.closest('.form-group').find('[data-ddl-division]');

                updateAdminControls($ddlDivision, true);
                $('#restructure-container').find('.admin-crud-controls').show();
                $crudControls.find('.restructure-button').each(function () {
                    var $button = $(this);

                    $button.removeClass('btn-active');
                    $button.text($button.data('action'));
                });
                $crudControls.find('[data-action="Cancel"]').hide();
                $editControl.attr('readonly', true);
            }

            function setUiCues() {
                var cueText = 'Press to save (' + $this.text().toLowerCase() + ')';

                $('#restructure-container').find('.admin-crud-controls').not($crudControls).hide();
                $crudControls.find('.restructure-button').not($this).hide();
                $crudControls.find('[data-action="Cancel"]').show();
                $this.addClass('btn-active');
                $this.text(cueText);
                $editControl.attr('readonly', false);
            }

            function nothingSelected() {
                var $activeDdlDivision = $crudControls.closest('.form-group').find('[data-ddl-division]');

                return (getDivisionName($activeDdlDivision).indexOf('Choose') > -1);
            }

            function getControllerAction() {
                var ddlDivision = $this.closest('.form-group').find('[data-ddl-division]').data('ddl-division');
                var secondSlashIndex = ddlDivision.indexOf('/', 1);
                var controller = ddlDivision.substr(1, secondSlashIndex);
                var controllerAction = '/' + controller + action;

                return controllerAction;
            }
        });

        $('#btn-move-beaches').on('click', function (event) {
            event.preventDefault();

            if (movingBeaches) {
                alert('Remove the currently checked beaches in order to be able to list new ones.');

                return;
            }

            var url = $currentDivision.data('ddl-division');
            var secondSlashIndex = url.indexOf('/', 1);
            var adminBeachesUrl = url.substr(0, secondSlashIndex) + '/AdminBeaches/' + $currentDivision.val();

            tryClearCheckBoxesContainer();

            $.getJSON(adminBeachesUrl, function (result) {
                $(result).each(function () {
                    var div = $('#beaches-result').find('.beach-result-template').first().clone();
                    var a = $('<a>', { href: '/Beaches/Details/' + this.Value, text: this.Text, target: '_blank' })
                    var beachCheckBox = $('<input />',
                        {
                            type: 'checkbox',
                            value: this.Value
                        });

                    div.removeClass('beach-result-template');
                    div.append(beachCheckBox);
                    div.append(a);
                    $('#beaches-result-container').append(div);
                });
            });
        });

        $('#btn-move-beaches').on('click', function (event) {
            event.preventDefault();

            if (movingBeaches) {
                alert('Remove the currently checked beaches in order to be able to list new ones.');

                return;
            }

            var url = $currentDivision.data('ddl-division');
            var secondSlashIndex = url.indexOf('/', 1);
            var adminBeachesUrl = url.substr(0, secondSlashIndex) + '/AdminBeaches/' + $currentDivision.val();

            tryClearCheckBoxesContainer();

            $.getJSON(adminBeachesUrl, function (result) {
                $(result).each(function () {
                    var div = $('#beaches-result').find('.beach-result-template').first().clone();
                    var a = $('<a>', { href: '/Beaches/Details/' + this.Value, text: this.Text, target: '_blank' })
                    var beachCheckBox = $('<input />',
                        {
                            type: 'checkbox',
                            value: this.Value
                        });

                    div.removeClass('beach-result-template');
                    div.append(beachCheckBox);
                    div.append(a);
                    $('#beaches-result-container').append(div);
                });

                if (result.length > 0) {
                    $('#btn-beaches-toggle-selected').removeClass('btn-reset').addClass('btn-info').text('Select all').show();
                }
            });
        });

        $('#beaches-result').on('click', 'input:checkbox', function () {
            var checkedCheckboxes = $('#beaches-result').find('input:checkbox').filter(function () {
                return $(this).is(':checked');
            });

            movingBeaches = (checkedCheckboxes.length > 0);
        });

        $('#btn-move-beaches-confirm').on('click', function (event) {
            event.preventDefault();

            setSelectedBeachIds();

            $(form).attr('action', '/Beaches/MoveBeaches/');
            $(form).submit();

            function setSelectedBeachIds() {
                var beachIds = [];
                var beachIdsResult;

                $('#beaches-result').find('input:checkbox').filter(function () {
                    return ($(this).is(':checked'));
                }).each(function () {
                    beachIds.push($(this).val());
                });

                beachIdsResult = beachIds.join();

                $('#hdn-beach-ids').val(beachIdsResult);
            }
        });

        $('#btn-beaches-toggle-selected').on('click', function (event) {
            event.preventDefault();

            var $this = $(this);
            var $checkBoxes = $this.closest('#move-beaches-container').find('input:checkbox');

            if ($checkBoxes.length === 0) {
                return;
            }

            if ($this.hasClass('btn-info')) {
                $this.removeClass('btn-info').addClass('btn-reset').text('Deselect all');
                $checkBoxes.prop('checked', true);

                movingBeaches = true;
            } else {
                $this.removeClass('btn-reset').addClass('btn-info').text('Select all');
                $checkBoxes.prop('checked', false);

                movingBeaches = false;
            }
        });
    }

    function updateAdminControls($ddlDivision, isCancel) {
        var $editControls = $ddlDivision.closest('.form-group').find('.admin-crud-controls');
        var divisionName = getDivisionName($ddlDivision);
        var nothingSelected = (divisionName.indexOf('Choose') > -1);

        setEditControlsText();
        setCrudButtonsVisibility();

        function setEditControlsText() {
            divisionName = nothingSelected ? '' : divisionName;

            $editControls.find('.edit-control').val(divisionName);

            if (!isCancel) {
                $ddlDivision.closest(('.form-group')).next('.form-group').find('.edit-control').val('');
            }
        }

        function setCrudButtonsVisibility() {
            if (nothingSelected) {
                $editControls.find('.restructure-button').not('[data-action="Add"]').hide();
                $ddlDivision.closest(('.form-group')).next('.form-group').find('.restructure-button').not('[data-action="Add"]').hide();
            } else {
                $editControls.find('.restructure-button').not('[data-action="Cancel"]').show();

                if (!isCancel) {
                    $ddlDivision.closest(('.form-group')).next('.form-group').find('.restructure-button').not('[data-action="Add"]').hide();
                }
            }

            if (!movingBeaches) {
                $('#btn-beaches-toggle-selected').removeClass('btn-reset').addClass('btn-info').text('Select all').hide();
            }
        }
    }

    function getDivisionName($ddlDivision) {
        var selector = '#' + $ddlDivision.attr('id') + ' option:selected';
        var name = $(selector).text();

        return name;
    }

    function tryClearCheckBoxesContainer() {
        if (!movingBeaches) {
            $('#beaches-result-container').empty();
        }
    }

    function tryHideBeachMovementContainer() {
        if (!movingBeaches) {
            $('#move-beaches-container').hide();
        }
    }

    function prepareBeachMovement() {
        if (movingBeaches && lastDivisionReached && lastDivisionSelected) {
            $('#btn-move-beaches-confirm').show();
        } else {
            $('#btn-move-beaches-confirm').hide();
        }
    }
})(jQuery);