$(document).ready(function () {
    // a custom selector for jquery used by the tooltip utilities
    (function ($) {
        var _dataFn = $.fn.data;
        $.fn.data = function (key, val) {
            if (typeof val !== 'undefined') {
                $.expr.attrHandle[key] = function (elem) {
                    return $(elem).attr(key) || $(elem).data(key);
                };
            }
            return _dataFn.apply(this, arguments);
        };
    })(jQuery);
});

var tooltipClass = "tooltip";
var tipTimer = null;

TooltipUtilities = {
    apply: function () {
        if (tipTimer) {
            clearTimeout(tipTimer);
        }

        tipTimer = setTimeout(this._bind, 200);
    },

    getTooltipClass: function () {
        return tooltipClass;
    },

    _bind: function () {
        var hiConfig = {
            sensitivity: 3,
            interval: 200,
            timeout: 200,
            over: function (e) {
                $('body').append($(this).attr('tooltip'));
                var $tooltip = $('.tooltipContainer');
                $tooltip.position({
                    my: 'left top',
                    at: 'left bottom',
                    of: this
                });

                var tOffset = $tooltip.offset();
                var tWidth = tOffset.left + $tooltip.outerWidth(true);
                var wWidth = $(window).width();
                var adjustLeft = (wWidth - tWidth) < 0;
                var tHeight = tOffset.top + $tooltip.outerHeight(true);
                var wHeight = $(window).height();
                var adjustTop = (wHeight - tHeight) < 0;

                if (adjustLeft && !adjustTop) {
                    $tooltip.position({
                        my: 'right top',
                        at: 'right bottom',
                        of: this
                    });
                }

                if (adjustTop && !adjustLeft) {
                    $tooltip.position({
                        my: 'left bottom',
                        at: 'left top',
                        of: this
                    });
                }

                if (adjustTop && adjustLeft) {
                    $tooltip.position({
                        my: 'right bottom',
                        at: 'right top',
                        of: this
                    });
                }
            },
            out: function () {
                $('.tooltipContainer').empty().remove();
            }
        };
        $('.tooltip').hoverIntent(hiConfig);
    }
};