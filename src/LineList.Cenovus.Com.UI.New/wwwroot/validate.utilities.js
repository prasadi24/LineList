$(document).ready(function () {
    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var check = false;
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        },
        "Please check your input value."
    );

    $.validator.addMethod(
        "notEqualTo",
        function (value, element, param) {
            return this.optional(element) || value != $(param).val();
        },
        "This has to be different."
    );
});