var KTSignIn = function () {
    var submitButton;
    var form;
    var validator;
    var handleForm = function () {

        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'email': {
                        validators: {
                            notEmpty: {
                                message: 'E-Mail alanının doldurulması zorunludur.'
                            },
                            emailAddress: {
                                message: "Lütfen geçerli bir E-Mail adresi giriniz."
                            }
                        }
                    },
                    'password': {
                        validators: {
                            notEmpty: {
                                message: 'Şifre alanının doldurulması zorunludur.'
                            }
                        }
                    }

                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleInvalidClass: '',
                        eleValidClass: ''
                    })
                }
            }
        );

        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        form.submit();
                    }
                });
            }
        });

    };


    return {
        // Public functions
        init: function () {
            // Elements
            form = document.querySelector('#kt_sign_in_form');
            submitButton = form.querySelector('#kt_sign_in_submit');

            handleForm();
        }
    };


}();
// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTSignIn.init();
});