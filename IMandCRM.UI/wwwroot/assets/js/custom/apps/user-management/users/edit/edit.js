// Class definition
var KTUsersEdit = function () {
    var submitButton;
    var validator;
    var form;


    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'FirstName': {
                        validators: {
                            notEmpty: {
                                message: 'Ad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'LastName': {
                        validators: {
                            notEmpty: {
                                message: 'Soyad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'UserName': {
                        validators: {
                            notEmpty: {
                                message: 'Kullanıcı adı alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'Email': {
                        validators: {
                            notEmpty: {
                                message: 'Mail alanının doldurulması zorunludur.'
                            },
                            emailAddress: {
                                message: "Lütfen geçerli bir E-Mail adresi giriniz."
                            }
                        }
                    },
                    'PhoneNumber': {
                        validators: {
                            notEmpty: {
                                message: 'Telefon numarası alanının doldurulması zorunludur.'
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

        // Action buttons
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            debugger;
            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        Swal.fire({
                            title: "Kaydetmek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                form.submit();
                            }
                        });

                    } else {

                    }
                });
            }
        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            form = document.querySelector('#kt_users_edit_form');
            submitButton = form.querySelector('#kt_edit_user_submit');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTUsersEdit.init();
});