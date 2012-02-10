(function ($) {
    $.fn.jGrid = function (options) {
        // Do stuff here! Cool!
        var settings = $.extend({
            'multiselect': false,
            'nonselectable': false,
            'allowsort': true,
            'pagesize': 0,
            'pageindex': 0,
            'pagecount': 0,
            'totalitems': 0,
            'next': 'Next',
            'previous': 'Previous',
            'first': 'First',
            'last': 'Last',
            'totalmsg': 'Showing _MIN_ to _MAX_ of _TOTAL_',
            'iconbuttons': true,
            'shownextprev': false,
            'showtotal': false,
            'checkboxindex': -1,
            'hidecheckboxes': false,
            'reorder': false
        }, options);

        if (settings.checkboxindex < 0) settings.checkboxindex = $(this).find('thead tr th').size() - 1;
        return this.each(function () {
            // Hide all checkboxes if this is a multiselect
            if (settings.hidecheckboxes && settings.multiselect) {
                // Find all checkbox columns and hide them, including the header one
                $($(this).find('thead tr th')[settings.checkboxindex]).hide();
                $(this).find('tbody tr').each(function () {
                    $($(this).find('td')[settings.checkboxindex]).hide();
                });
                $($(this).find('tfoot tr td')[settings.checkboxindex]).hide();
            }

            // Find all headers and add a click command
            // Also apply the correct classes on different mouse states
            if (settings.allowsort) {
                $(this).find('thead th').each(function () {
                    $(this).addClass('ui-state-default').mouseover(function () {
                        $(this).addClass('ui-state-hover');
                    }).mousedown(function () {
                        $(this).addClass('ui-state-active');
                    }).mouseout(function () {
                        $(this).removeClass('ui-state-active ui-state-hover');
                    });

                    // Extract link and href, and apply that to the th instead of the a
                    var a = $(this).children('a');
                    var href = a.attr('href');
                    var txt = a.html();
                    $(this).attr('onclick', href);

                    // Replace the link with a literal
                    a.remove();
                    if ($(this).find('span').size() > 0) {
                        $(this).find('span').before(txt);
                    } else {
                        $(this).append(txt);
                    }
                });
            } else {
                // If not allowsort, then apply the correct css class
                $(this).find('thead th').addClass('adfj-table-header-nonsortable');
            }

            // Apply the correct css classes on the rows in the tbody
            $(this).find('tbody > tr').addClass('ui-state-default');
            // but only if they are selectable
            if (!settings.nonselectable || settings.reorder) {
                $(this).find('tbody > tr').each(function () {
                    // Set every checked row to active
                    if (settings.multiselect && !settings.nonselectable) {
                        var chk = $(this).find('input[type=checkbox]:last');
                        if (chk.attr('checked')) {
                            $(this).addClass('ui-state-active');
                        }
                    }
                }).mouseover(function () {
                    $(this).addClass('ui-state-hover');
                }).click(function (e) {
                    if (e.which == 1 && !settings.reorder) { // Left button only
                        if (settings.multiselect) {
                            // Find the checkbox
                            var chk = $(this).find('input[type=checkbox]:last');
                            var prevChecked = chk.attr('checked');
                            chk.click();
                            if (prevChecked == chk.attr('checked')) { // Click had no effect
                                chk.attr('checked', !prevChecked);
                            }
                            if (!chk.attr('checked')) {
                                $(this).removeClass('ui-state-active');
                                return;
                            }
                        }
                        $(this).addClass('ui-state-active');
                    }
                }).mouseout(function () {
                    if (settings.multiselect) {
                        // Find the checkbox to select which classes to remove from the row
                        var chk = $(this).find('input[type=checkbox]:last');
                        $(this).removeClass(chk.attr('checked') ? 'ui-state-hover' : 'ui-state-active ui-state-hover');
                    } else {
                        // Remove the active and hover classes from the row
                        $(this).removeClass('ui-state-active ui-state-hover');
                    }
                });
            }

            // Create a nice shiny pager instead of the default .NET one.
            var $pager = $(this).find('tfoot > tr > td');
            var $pagerDiv = $('<div class="adfj-table-pager"></div>').appendTo($pager);

            function addButton($parent, $disabled, $label, $href, $icon) {

                var $first = "<button role=\"button\" value=\"" + settings.first + "\"";
                if ($href != null && $href.length > 0) {
                    $first += " onclick=\"" + $href + "\"";
                }
                $first += "></button>";
                var $b = $($first).appendTo($parent).button({ disabled: $disabled, label: $label, text: !settings.iconbuttons });
                if (settings.iconbuttons) $b.button('option', 'icons', { primary: $icon });
            }

            // If next/prev buttons are enabled, generate first and previous buttons
            if (settings.shownextprev) {
                addButton($pagerDiv, settings.pageindex == 0, settings.first, $pager.find('td:first > a').attr('href'), 'ui-icon-seek-first');
                addButton($pagerDiv, settings.pageindex == 0, settings.previous, $($pager.find('td > a')[settings.pageindex - 1]).attr('href'), 'ui-icon-seek-prev');
            }

            for (var $i = 0; $i < settings.pagecount; $i++) {
                var $button = "<button role=\"button\" value=\"" + ($i + 1) + "\"";
                if ($i == settings.pageindex) {
                    // No href for this button
                } else {
                    // Find the href for the button in the table
                    var a = $($pager.find('td')[$i]).find('a');
                    $button += " onclick=\"" + a.attr('href') + "\"";
                }
                $button += "></button>";
                $($button).appendTo($pagerDiv).button({ label: ($i + 1), disabled: ($i == settings.pageindex) });
            }
            // If next/prev buttons are enabled, generate next and last buttons
            if (settings.shownextprev) {
                addButton($pagerDiv, settings.pageindex == settings.pagecount - 1, settings.next, $($pager.find('td > a')[settings.pageindex]).attr('href'), 'ui-icon-seek-next');
                addButton($pagerDiv, settings.pageindex == settings.pagecount - 1, settings.last, $pager.find('td:last > a').attr('href'), 'ui-icon-seek-end');
            }

            // If totalmsg is enabled, add it
            if (settings.showtotal && settings.totalitems != -1) {
                var $msg = settings.totalmsg.replace('_MIN_', settings.pageindex * settings.pagesize + 1).replace('_MAX_', Math.min(settings.totalitems, (settings.pageindex + 1) * settings.pagesize)).replace('_TOTAL_', settings.totalitems);
                $('<div class="adfj-table-pager-totalmsg ui-state-default">' + $msg + '</div>').appendTo($pager);
            }

            $pager.find('table').remove();

            // Apply sortable if reorder is true

            // Return a helper with preserved width of cells
            var fixHelper = function (e, ui) {
                ui.children().each(function () {
                    $(this).width($(this).width());
                });
                return ui;
            };

            if (settings.reorder) {
                $(this).find('tbody > tr').each(function () {
                    $('<span class="adfj-reorder-icon ui-icon ui-icon-arrowthick-2-n-s">').prependTo($(this).find('td:first'));
                });

                $(this).sortable({
                    items: 'tbody > tr',
                    helper: fixHelper,
                    forceHelperSize: true,
                    axis: 'y',
                    appendTo: 'tbody',
                    forcePlaceHolderSize: true,
                    stop: function (event, ui) {
                        var newIndex = $(ui.item).parent().children().index(ui.item);
                        var oldIndex = $(ui.item).attr('onchanged').split('$')[4];
                        if (oldIndex != newIndex) {
                            var script = $(ui.item).attr('onchanged').replace('{newIndex}', newIndex);
                            eval(script);
                        }
                    }
                }).disableSelection();
            }
        });
    };
})(jQuery);