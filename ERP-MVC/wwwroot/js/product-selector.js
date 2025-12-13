document.addEventListener("DOMContentLoaded", function () {

    loadPackages();
    enableDynamicRows();
    enableTotals();

    // =============================
    // LOAD PRODUCT PACKAGES
    // =============================
    async function loadPackages() {
        const url = "/PurchaseInvoice/GetProductPackages";

        const res = await fetch(url);
        const packages = await res.json();

        document.querySelectorAll(".product-package-select").forEach(select => {
            fillPackageDropdown(select, packages);
        });
    }

    function fillPackageDropdown(select, packages) {
        select.innerHTML = '<option value="">Select Package</option>';
        packages.forEach(p => {
            select.innerHTML += `
                <option value="${p.id}"
                        data-price="${p.purchasePrice}">
                    ${p.productName} - ${p.packageTypeName} (Buy: ${p.purchasePrice})
                </option>`;
        });
    }

    // =============================
    // ADD / REMOVE PRODUCT ROWS
    // =============================
    function enableDynamicRows() {
        const addBtn = document.getElementById("add-product");

        if (!addBtn) return;

        addBtn.addEventListener("click", function () {

            const tbody = document.getElementById("products-body");
            const firstRow = tbody.querySelector(".product-row");
            const newRow = firstRow.cloneNode(true);

            // Reset input values
            newRow.querySelectorAll("input").forEach(input => {
                if (input.classList.contains("quantity")) input.value = 1;
                if (input.classList.contains("price")) input.value = 0;
                if (input.classList.contains("total")) input.value = "0.00";
            });

            // Clear dropdown selection
            newRow.querySelector(".product-package-select").value = "";

            tbody.appendChild(newRow);
            fillPackageDropdown(select, window.allPackages);
        });

        // REMOVE ROW
        document.addEventListener("click", function (e) {
            if (e.target.closest(".remove-row")) {
                const row = e.target.closest("tr");
                const tbody = document.getElementById("products-body");
                if (tbody.querySelectorAll(".product-row").length > 1) {
                    row.remove();
                    updateTotals();
                } else {
                    alert("At least one product is required.");
                }
            }
        });
    }

    // =============================
    // PRICE & QUANTITY HANDLING
    // =============================
    function enableTotals() {
        document.addEventListener("change", function (e) {
            if (e.target.classList.contains("product-package-select")) {
                const row = e.target.closest("tr");
                const selected = e.target.selectedOptions[0];
                const price = selected?.dataset.price || 0;
                row.querySelector(".price").value = price;
                updateRowTotal(row);
            }
        });

        document.addEventListener("input", function (e) {
            if (e.target.classList.contains("quantity") ||
                e.target.classList.contains("price")) {
                const row = e.target.closest("tr");
                updateRowTotal(row);
            }
        });
    }

    function updateRowTotal(row) {
        const qty = parseFloat(row.querySelector(".quantity").value) || 0;
        const price = parseFloat(row.querySelector(".price").value) || 0;
        row.querySelector(".total").value = (qty * price).toFixed(2);
        updateTotals();
    }

    function updateTotals() {
        let total = 0;

        document.querySelectorAll(".total").forEach(t => {
            total += parseFloat(t.value) || 0;
        });

        const discountInput = document.getElementById("discount");
        const discount = discountInput ? parseFloat(discountInput.value) || 0 : 0;

        // TOTAL
        const totalBox = document.getElementById("total-amount-display");
        if (totalBox) totalBox.value = total.toFixed(2);

        // NET
        const netBox = document.getElementById("net-amount-display");
        if (netBox) netBox.value = (total - discount).toFixed(2);
    }

});