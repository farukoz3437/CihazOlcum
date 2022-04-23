$(async function () {

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
    };


    Inputmask({
        "mask": "(999) 999-9999"
    }).mask(".PhoneMask");

    Inputmask({
        "mask": "9",
        "repeat": 10,
        "greedy": false
    }).mask(".TaxNumberMask");

    var msg;
    if ('@TempData["message"]' != undefined && '@TempData["message"]' != "") {
        msg = '@Html.Raw((string)TempData["message"])';
        if (msg != null) {
            msgArr = msg.split('|');
            if (msgArr[1] == "success") {
                toastr.success(msgArr[0]);
            } else if (msgArr[1] == "error") {
                toastr.error(msgArr[0]);
            } else if (msgArr[1] == "warning") {
                toastr.warning(msgArr[0]);
            } else if (msgArr[1] == "info") {
                toastr.info(msgArr[0]);
            }

        }
    }


    $("[name='IsWindy']").on("change", function () {
        if ($(this).prop("checked")) {
            $("#WindForce0").prop("checked", true);
            $("#WindValue").removeAttr("readonly");
        } else {
            $("[name='WindForce']").prop("checked", false);
            $("#WindValue").attr("readonly", true);
        }
    });

    $("[name='IsRainy']").on("change", function () {
        if ($(this).prop("checked")) {
            $("#RainForce0").prop("checked", true);
            $("#RainValue").removeAttr("readonly");
        } else {
            $("[name='RainForce']").prop("checked", false);
            $("#RainValue").attr("readonly", true);
        }
    });

    $("[name='GroundPlate']").on("change", function () {
        if ($(this).val() == "1") {
            $("#GroundPlateDescription").removeAttr("readonly");
        } else {
            $("#GroundPlateDescription").attr("readonly", true);
        }
    });

    $("[name='EnviromentBuilding']").on("change", function () {
        if ($(this).val() == "3") {
            $("#EnviromentBuildingDescription").removeAttr("readonly");
        } else {
            $("#EnviromentBuildingDescription").attr("readonly", true);
        }
    });

    $("[name='Files']").change(function () {
        debugger;
        var fileCount = this.files.length;
        if (fileCount != 3) {
            $(this).val("");
            $("#fileCount").val("En az 3 dosya seçiniz");
            toastr.warning("En az 3 dosya seçiniz");
            return;
        }
        var fileE = false;
        var fileN = false;
        var fileZ = false;

        if (this.files[0].name.indexOf("E") != -1 || this.files[1].name.indexOf("E") != -1 || this.files[2].name.indexOf("E") != -1) {
            fileE = true;
        }
        if (this.files[0].name.indexOf("N") != -1 || this.files[1].name.indexOf("N") != -1 || this.files[2].name.indexOf("N") != -1) {
            fileN = true;
        }
        if (this.files[0].name.indexOf("Z") != -1 || this.files[1].name.indexOf("Z") != -1 || this.files[2].name.indexOf("Z") != -1) {
            fileZ = true;
        }
        if (fileE && fileN && fileZ) {
            $("#fileCount").val(fileCount + " Dosya Seçildi");
        } else {
            $(this).val("");
            $("#fileCount").val("Hatalı dosya ismi.");
            toastr.warning("Hatalı dosya ismi.");
        }

    })
    $("#fileIcon").on("click", function () {
        $("[name='Files']").click();
    })

    $('#kt_edit_measurement_submit').on('click', function () {
        var isValid = FormValidation();
        if (isValid) {
            Swal.fire({
                title: "Ölçümü kaydetmek istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    $('#kt_measurement_edit_form').submit();
                    $("#overlay").css("display","block");
                }
            });
        }
    })

    function FormValidation() {
        var isValid = true;
        if ($('[name="ProjectName"]').val() == "") {
            toastr.warning("Proje adını giriniz...");
            isValid = false;
        }
        if ($('[name="PointName"]').val() == "") {
            toastr.warning("Nokta adını giriniz...");
            isValid = false;
        }
        if ($('[name="DeviceName"]').val() == "") {
            toastr.warning("Cihaz adını giriniz...");
            isValid = false;
        }
        if ($('[name="Sensor"]').val() == "") {
            toastr.warning("Teklif şartlarını giriniz...");
            isValid = false;
        }
        if ($('[name="SensorFrequency"]').val() == "") {
            toastr.warning("Sensör frekansını giriniz...");
            isValid = false;
        }
        if ($('[name="Location"]').val() == "") {
            toastr.warning("Mevkii-Yer adını giriniz...");
            isValid = false;
        }
        if ($('[name="Lat"]').val() == "") {
            toastr.warning("Enlem bilgisini giriniz...");
            isValid = false;
        }
        if ($('[name="Lon"]').val() == "") {
            toastr.warning("Boylam bilgisini giriniz...");
            isValid = false;
        }
        if ($('[name="Ele"]').val() == "") {
            toastr.warning("Yükseklik bilgisini giriniz...");
            isValid = false;
        }
        if ($('[name="RecordingTime"]').val() == "") {
            toastr.warning("Kayıt süresi bilgisini giriniz...");
            isValid = false;
        }
        return isValid;
    }


});