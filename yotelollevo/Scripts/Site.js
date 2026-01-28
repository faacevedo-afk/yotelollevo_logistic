// Scripts globales del sitio
(function () {

    // Sidebar toggle (si usas clase .sgi-collapsed)
    function wireSidebarToggle() {
        const btn = document.getElementById("sidebarToggle");
        if (!btn) return;

        btn.addEventListener("click", function (e) {
            e.preventDefault();
            document.body.classList.toggle("sgi-collapsed");
        });
    }

    // Helper para inicializar DataTables (jQuery)
    window.initDataTable = function (tableSelector, searchInputSelector) {
        if (!window.jQuery) {
            console.error("jQuery no cargó.");
            return;
        }
        if (!jQuery.fn || !jQuery.fn.DataTable) {
            console.error("DataTables no cargó.");
            return;
        }

        const $table = jQuery(tableSelector);
        if ($table.length === 0) {
            console.warn("No existe la tabla:", tableSelector);
            return;
        }

        const dt = $table.DataTable({
            dom: "rtip",       // sin buscador interno ni length
            paging: true,
            searching: true,   // lo usamos vía input externo
            info: true,
            scrollX: true,
            pageLength: 10,
            language: {
                info: "Mostrando _START_ a _END_ de _TOTAL_ filas",
                infoEmpty: "Mostrando 0 a 0 de 0 filas",
                zeroRecords: "No se encontraron resultados",
                paginate: { first: "Primero", last: "Último", next: ">", previous: "<" }
            }
        });

        // input externo buscar
        if (searchInputSelector) {
            const $input = jQuery(searchInputSelector);
            if ($input.length) {
                $input.on("keyup", function () {
                    dt.search(this.value).draw();
                });
            }
        }

        return dt;
    };

    document.addEventListener("DOMContentLoaded", function () {
        wireSidebarToggle();
    });

})();
