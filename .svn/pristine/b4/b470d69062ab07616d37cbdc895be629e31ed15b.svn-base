
jQuery(document).ready(function () {

    /*
        Fullscreen background
    */
    $.backstretch("Content/bootstrap/pgasdark.jpg");
    /*
        Form validation
    */
    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on('focus', function () {
        $(this).removeClass('input-error');
    });

    $('.login-form').on('submit', function (e) {
             
        $(this).find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                e.preventDefault();
                $(this).addClass('input-error');
            }
            else {
                var username = $('#username').value();
                alert(username)
            }
        });
        //$(this).find('input[type="text"], input[type="password"], textarea').each(function () {
        //    if ($(this).val() == "") {
        //        e.preventDefault();
        //        $(this).addClass('input-error');
        //    }
        //    else {
        //        $(this).removeClass('input-error');
        //    }
        //});

        //$("#form").validate({
        //    // Specify the validation rules
        //    rules: {
        //        username: {
        //            required: true,
        //            email: true
        //        },
        //        password: {
        //            required: true,
        //            minlength: 1
        //        }
        //    },

        //    // Specify the validation error messages
        //    messages: {
        //        username: "Please enter a valid username",
        //        password: {
        //            required: "Please provide a password",
        //            minlength: "Your password must be at least 1 characters long"
        //        },
        //        submitHandler: function (form) { // for demo
        //            $('#username').focus();
        //            $('#submit').click(function () {
        //                event.preventDefault(); // prevent PageReLoad                                
        //                var ValEmail = $('#username').val() === 'admin@admin.com'; // Email Value
        //                alert("Email" + ValEmail);
        //                var ValPassword = $('#password').val() === 'admin1234'; // Password Value
        //                if (ValEmail === true && ValPassword === true) { // if ValEmail & Val ValPass are as above
        //                    alert('valid!'); // alert valid!
        //                    window.location.href = "http://java67.blogspot.com";
        //                    // go to home.html
        //                }
        //                else {
        //                    alert('not valid!'); // alert not valid!
        //                }
        //            });
        //        }
        //    }
        //});



    
    });
});
