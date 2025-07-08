/**
* @author Ant1j
*
* Code inspired from jquery.suggest
* Concept from Django Admin filter list : http://simonwillison.net/static/2008/xtech/ (slide 18)
*
* Usage: $('select.filter').filterselect();
* Options: minChars => minimum characters needed to filter
*/

(function ($) {
    $.filterselect = function (select, input, options) {
        var originalrows = select.find('option');
        var prevLength = 0;
        var timeout = true;
        var $input = input;

        //if (navigator.userAgent.toUpperCase().indexOf('mozilla') )
        //    $input.keypress(processKey); // onkeypress repeats arrow keys in Mozilla/Opera
        //else
        $input.keyup(processKey); 	// onkeydown repeats arrow keys in IE/Safari

        function processKey(e) {
            if ($input.val().length != prevLength) {
                $input.addClass('ui-autocomplete-loading');
                if (timeout)
                    clearTimeout(timeout);
                timeout = setTimeout(filtre, 2000);
                prevLength = $input.val().length;
            }

            function filtre() {
                var q = $.trim($input.val());
                var rows = originalrows;
                if (q.length >= options.minChars) {
                    var exp = new RegExp(regExpEscape(q), 'i');
                    rows = parseRows(rows, exp);
                }
                select.html('');
                for (var row in rows) {
                    if (rows[row] && rows[row].value && rows[row].text) {
                        select.append($('<option></option>').val(rows[row].value).html(rows[row].text));
                    }
                }
                $input.removeAttr('disabled').focus().delay(5000).removeClass('ui-autocomplete-loading');
            }

            function regExpEscape(text) {
                return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
            }

            function parseRows(or, exp) {
                return or.filter(function (i) {
                    var txt = this.text;
                    if (exp.test(txt)) return this;
                });
            }
        }
    };

    $.fn.filterselect = function (input, options) {
        var defaults = { minChars: 2, loadIconPath: "" };
        var $this = $(this);
        var $options = $.extend({}, defaults, options);
        var $input = input;

        this.each(function () {
            new $.filterselect($this, $input, $options);
        });
        return this;
    };
})(jQuery);