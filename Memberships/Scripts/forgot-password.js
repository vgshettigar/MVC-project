$(function () {
    var pwdLinkHover = $('#pwdLink').hover(onCloseLogin);
    var resetPwd = $('#resetPwd').click(onResetPassowrd);

    function onCloseLogin() {
        $("div[data-login-user-area]").removeClass('open');
    }

    function onResetPassowrd() {
        var email = $('.modal-dialog .reset-email').val();
        var antiforgery = $('[name="__RequestVerificationToken"]').val();
        var url = "/Account/ForgotPasswordConfirmation";

        $.post("/Account/ForgotPassword", {
            __RequestVerificationToken: antiforgery, email: email
        }, function (data) {
            location.href = url;
        }).fail(function (xhr, status, error) { location.href = url; });
    }
});
