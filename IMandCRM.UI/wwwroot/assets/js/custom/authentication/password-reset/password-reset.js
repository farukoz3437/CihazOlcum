"use strict";
var KTPasswordResetGeneral = function () {
    var t, e, i;
    return {
        init: function () {
            t = document.querySelector("#kt_password_reset_form"), e = document.querySelector("#kt_password_reset_submit"), i = FormValidation.formValidation(t, {
                fields: {
                    Email: {
                        validators: {
                            notEmpty: {
                                message: "Email alanı boş geçilemez."
                            },
                            emailAddress: {
                                message: "Geçerli bir mail adresi giriniz."
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger,
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: ".fv-row",
                        eleInvalidClass: "",
                        eleValidClass: ""
                    })
                }
            }), e.addEventListener("click", (function (o) {
                o.preventDefault(), i.validate().then((function (i) {
                    "Valid" == i ? 
                        Swal.fire({
                            title: "Göndermek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                t.submit();
                            }
                        }) : Swal.fire({
                            text: "Lütfen eksik alanları doldurup tekrar deneyiniz.",
                            icon: "error",
                            buttonsStyling: !1,
                            confirmButtonText: "Tamam",
                            customClass: {
                                confirmButton: "btn btn-primary"
                            }
                        })
                }))
            }))
        }
    }
}();
KTUtil.onDOMContentLoaded((function () {
    KTPasswordResetGeneral.init()
}));
