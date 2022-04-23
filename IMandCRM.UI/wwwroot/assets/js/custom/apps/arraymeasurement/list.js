
// Class definition
var KTArraymeasurementsList = function () {
    // Define shared variables
    var datatable;
    var table

    // Private functions
    var initArraymeasurementList = function () {

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

    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        debugger;
        const filterSearch = document.querySelector('[data-kt-arraymeasurement-table-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }


    // Public methods
    return {
        init: function () {
            table = document.querySelector('#kt_arraymeasurements_table');

            if (!table) {
                return;
            }

            initArraymeasurementList();
            handleSearchDatatable();
        }
    }
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTArraymeasurementsList.init();
});