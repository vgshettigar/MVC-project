$(function () {
    var registerUserCheckBox = $('#AcceptUSerAgreement').click(
        onToggleRegisterUserDisabledClick);

    function onToggleRegisterUserDisabledClick()
    {
        $('.register-user-panel button').toggleClass('disabled');
    }

    var registerUserButton = $('.register-user-panel button').click(
    onRegisterUserClick);
    
    function onRegisterUserClick()
    {
        var url = '/Account/RegisterUserAsync';
        var antiForgery = $('[name="__RequestVerificationToken"]').val();
        var name = $('.register-user-panel .first-name').val();
        var email = $('.register-user-panel .email').val();
        var pwd = $('.register-user-panel .password').val();

        $.post(url, {
            __RequestVerificationToken: antiForgery, Email: email, Name: name, Password: pwd,
            AcceptUSerAgreement: true},
        function(data){
            var parsed = $.parseHTML(data);
            var hasErros = $(parsed).find('[data-valmsg-summary]').text().replace(/\n|\r/g, "").length > 0;
            if (hasErros == true)
            {
                $('.register-user-panel').html(data);

                var registerUserCheckBox = $('#AcceptUSerAgreement').click(
                    onToggleRegisterUserDisabledClick);

                var registerUserButton = $('.register-user-panel button').click(
                    onRegisterUserClick);

                $('.register-user-panel button').toggleClass('disabled');

            }
            else
            {
                var registerUserCheckBox = $('#AcceptUSerAgreement').click(
                    onToggleRegisterUserDisabledClick);

                var registerUserButton = $('.register-user-panel button').click(
                    onRegisterUserClick);

                location.href = '/Home/Index';

            }
        }).fail(function(xhr, status, error) { alert('Post unsuccesful'); })
    }
});