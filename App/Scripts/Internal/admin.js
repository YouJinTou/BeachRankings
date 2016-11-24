(function ($) {
    var $ddlDivisions = $('[data-ddl-division]');
    var divisionHolder = '[data-division-holder]';

    setDivisionEvents();
    setCrudControlEvents();

    function setDivisionEvents() {
        $ddlDivisions.on('change', function () {
            var $this = $(this);
            var nextDivision = $this.data('next-division');
            var url = $this.data('ddl-division') + $this.val();
            var currentAddress = getCurrentAddress();

            updateAdminControls($this);

            if (!nextDivision) {
                return;
            }

            var $nextDivision = $($ddlDivisions[nextDivision]);

            if (!$this.val()) {
                setDivisionDropdownsVisibility(false);

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

        $('.restructure-button').on('click', function (event) {
            event.preventDefault();

            var $this = $(this);
            var $crudControls = $this.closest('.admin-crud-controls');
            var $editControl = $crudControls.find('.edit-control');
            var editControlReadOnly = $editControl.prop('readonly');
            var action = $this.data('action');

            resetCrudControls();

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

                if (editControlReadOnly) {
                    setUiCues();
                }
            }

            function onCancelClicked() {
                resetCrudControls();
            }

            function resetCrudControls() {
                var $ddlDivision = $crudControls.closest('.form-group').find('[data-ddl-division]');

                updateAdminControls($ddlDivision);
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
    }

    function updateAdminControls($ddlDivision) {
        var $editControls = $ddlDivision.closest('.form-group').find('.admin-crud-controls');
        var divisionName = getDivisionName($ddlDivision);
        var nothingSelected = (divisionName.indexOf('Choose') > -1);

        setEditControlsText();
        setCrudButtonsVisibility();

        function setEditControlsText() {
            divisionName = nothingSelected ? '' : divisionName;

            $editControls.find('.edit-control').val(divisionName);
            $ddlDivision.closest(('.form-group')).next('.form-group').find('.edit-control').val('');
        }

        function setCrudButtonsVisibility() {
            if (nothingSelected) {
                $editControls.find('.restructure-button').not('[data-action="Add"]').hide();
            } else {
                $editControls.find('.restructure-button').not('[data-action="Cancel"]').show();
                $ddlDivision.closest(('.form-group')).next('.form-group').find('.restructure-button').not('[data-action="Add"]').hide();
            }
        }
    }

    function getDivisionName($ddlDivision) {
        var selector = '#' + $ddlDivision.attr('id') + ' option:selected';
        var name = $(selector).text();

        return name;
    }
})(jQuery);