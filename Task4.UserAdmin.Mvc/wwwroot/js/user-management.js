document.addEventListener("DOMContentLoaded", function () {
    const selectAll = document.getElementById("selectAllRows");
    const rowCheckboxes = Array.from(document.querySelectorAll(".row-selector"));
    const bulkButtons = Array.from(document.querySelectorAll(".bulk-action"));
    const filterInput = document.getElementById("userFilter");
    const rows = Array.from(document.querySelectorAll(".user-row"));

    const syncToolbar = () => {
        const anyChecked = rowCheckboxes.some(cb => cb.checked);
        bulkButtons.forEach(button => {
            button.disabled = !anyChecked;
        });

        if (selectAll) {
            const visibleCheckboxes = rowCheckboxes.filter(cb => cb.closest("tr")?.style.display !== "none");
            selectAll.checked = visibleCheckboxes.length > 0 && visibleCheckboxes.every(cb => cb.checked);
            selectAll.indeterminate = visibleCheckboxes.some(cb => cb.checked) && !selectAll.checked;
        }
    };

    if (selectAll) {
        selectAll.addEventListener("change", function () {
            rowCheckboxes.forEach(cb => {
                if (cb.closest("tr")?.style.display !== "none") {
                    cb.checked = selectAll.checked;
                }
            });
            syncToolbar();
        });
    }

    rowCheckboxes.forEach(cb => cb.addEventListener("change", syncToolbar));

    if (filterInput) {
        filterInput.addEventListener("input", function () {
            const term = filterInput.value.trim().toLowerCase();
            rows.forEach(row => {
                const searchable = row.dataset.filter || "";
                row.style.display = searchable.includes(term) ? "" : "none";
            });
            syncToolbar();
        });
    }

    const managementForm = document.getElementById("managementForm");
    const hiddenSearch = document.querySelector('input[name="SearchTerm"]');
    if (managementForm && hiddenSearch && filterInput) {
        managementForm.addEventListener("submit", function () {
            hiddenSearch.value = filterInput.value || "";
        });
    }

    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });

    syncToolbar();
});
