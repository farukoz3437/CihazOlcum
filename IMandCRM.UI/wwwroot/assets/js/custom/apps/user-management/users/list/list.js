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

function Delete(id) {
    Swal.fire({
        title: "Silmek istediğinizden emin misiniz?",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Hayır!",
        confirmButtonText: "Evet!"
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: '/Admin/UserDelete?id=' + id,
                type: 'GET',
                success: function (response) {
                    console.log(response);
                    if (response.responseStatus) {
                        toastr.success(response.messageText);
                        setTimeout(function () {
                            location.reload(true);
                        }, 1000);
                    } else {
                        toastr.error(response.messageText);
                    }
                },
                error: function (error) {
                    toastr.error("Hata oluştu.");
                }
            });
        }
    });

}

// Class definition
var KTUsersList = function () {
    // Define shared variables
    var datatable;
    var table

    // Private functions
    var initUserList = function () {

        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'order': [],
            'columnDefs': [
                { orderable: false, targets: 0 }, // Disable ordering on column 0 (checkbox)
                { orderable: false, targets: 4 }, // Disable ordering on column 6 (actions)
            ],
            "oLanguage": {
                "sEmptyTable": "Kayıt bulunamadı..."
            }
        });

        // Re-init functions on every table re-draw -- more info: https://datatables.net/reference/event/draw
        datatable.on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        debugger;
        const filterSearch = document.querySelector('[data-kt-user-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }
    //-----------------------------------------------------

    // Init toggle toolbar
    var initToggleToolbar = () => {
        // Toggle selected action toolbar
        // Select all checkboxes
        const checkboxes = table.querySelectorAll('[type="checkbox"]');

        // Toggle delete selected toolbar
        checkboxes.forEach(c => {
            // Checkbox on click event
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });

        // Select elements
        const deleteSelected = document.querySelector('[data-kt-user-table-select="delete_selected"]');
        // Deleted selected rows
        deleteSelected.addEventListener('click', function () {
            // SweetAlert2 pop up --- official docs reference: https://sweetalert2.github.io/
            Swal.fire({
                text: "Seçili olan kullanıcıları silmek istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                buttonsStyling: false,
                confirmButtonText: "Evet!",
                cancelButtonText: "Hayır",
                customClass: {
                    confirmButton: "btn fw-bold btn-danger",
                    cancelButton: "btn fw-bold btn-active-light-primary"
                }
            }).then(function (result) {
                if (result.value) {

                    // Remove header checked box
                    const headerCheckbox = table.querySelectorAll('[type="checkbox"]')[0];
                    headerCheckbox.checked = false;

                    //Serialize the form datas.   
                    var valdata = $("#UserListForm").serialize();
                    $.ajax({
                        url: "/Admin/UsersDelete",
                        type: "POST",
                        dataType: 'json',
                        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                        data: valdata,
                        success: function (response) {
                            console.log(response);
                            if (response.responseStatus) {
                                toastr.success(response.messageText);
                                setTimeout(function () {
                                    location.reload(true);
                                }, 1000);
                            } else {
                                toastr.error(response.messageText);
                            }
                        },
                        error: function (error) {

                        }
                    });

                }
            });
        });

    }
    //------------------------------------------------------------
    // Toggle toolbars
    const toggleToolbars = () => {
        // Define variables
        const toolbarBase = document.querySelector('[data-kt-user-table-toolbar="base"]');
        const toolbarSelected = document.querySelector('[data-kt-user-table-toolbar="selected"]');
        const selectedCount = document.querySelector('[data-kt-user-table-select="selected_count"]');

        // Select refreshed checkbox DOM elements 
        const allCheckboxes = table.querySelectorAll('tbody [type="checkbox"]');

        // Detect checkboxes state & count
        let checkedState = false;
        let count = 0;

        // Count checked boxes
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
        });

        // Toggle toolbars
        if (checkedState) {
            selectedCount.innerHTML = count;
            toolbarBase.classList.add('d-none');
            toolbarSelected.classList.remove('d-none');
        } else {
            toolbarBase.classList.remove('d-none');
            toolbarSelected.classList.add('d-none');
        }
    }

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

    // Public methods
    return {
        init: function () {
            table = document.querySelector('#kt_users_table');

            if (!table) {
                return;
            }

            initUserList();
            initToggleToolbar();
            handleSearchDatatable();
        }
    }
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTUsersList.init();
});