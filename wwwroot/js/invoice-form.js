document.addEventListener('DOMContentLoaded', function () {
    let itemIndex = document.querySelectorAll('.item-row').length;

    initializeForm();

    document.getElementById('addItemBtn').addEventListener('click', function () {
        addNewItem();
    });

    document.addEventListener('click', function (e) {
        if (e.target.closest('.remove-item')) {
            e.preventDefault();
            removeItem(e.target.closest('.item-row'));
        }
    });

    document.addEventListener('input', function (e) {
        if (e.target.classList.contains('item-count') || e.target.classList.contains('item-price')) {
            updateRowTotal(e.target.closest('.item-row'));
            updateGrandTotal();
        }
    });

    document.getElementById('branchSelect').addEventListener('change', function () {
        loadCashiersByBranch(this.value);
    });

    document.getElementById('invoiceForm').addEventListener('submit', function (e) {
        if (!validateForm()) {
            e.preventDefault();
        }
    });

    function initializeForm() {
        document.querySelectorAll('.item-row').forEach(row => {
            updateRowTotal(row);
        });
        updateGrandTotal();

        if (document.querySelectorAll('.item-row').length === 0) {
            addNewItem();
        }

        const branchSelect = document.getElementById('branchSelect');
        if (branchSelect.value) {
            loadCashiersByBranch(branchSelect.value);
        }
    }

    function addNewItem() {
        const template = document.getElementById('itemRowTemplate');
        const clone = template.content.cloneNode(true);

        updateCloneIndices(clone, itemIndex);

        const row = clone.querySelector('.item-row');
        row.setAttribute('data-index', itemIndex);

        document.getElementById('itemsTableBody').appendChild(clone);

        const newRow = document.querySelector(`[data-index="${itemIndex}"]`);
        const firstInput = newRow.querySelector('.item-name');
        if (firstInput) {
            firstInput.focus();
        }

        itemIndex++;
        updateGrandTotal();
    }

    function removeItem(row) {
        if (document.querySelectorAll('.item-row').length <= 1) {
            showAlert('يجب أن تحتوي الفاتورة على صنف واحد على الأقل', 'warning');
            return;
        }

        row.classList.add('removing');

        setTimeout(() => {
            row.remove();
            updateItemIndices();
            updateGrandTotal();
        }, 300);
    }

    function updateCloneIndices(clone, index) {
        const inputs = clone.querySelectorAll('input');
        inputs.forEach(input => {
            if (input.name) {
                input.name = input.name.replace('INDEX', index);
            }
        });
    }

    function updateItemIndices() {
        document.querySelectorAll('.item-row').forEach((row, index) => {
            row.setAttribute('data-index', index);

            const inputs = row.querySelectorAll('input');
            inputs.forEach(input => {
                if (input.name && input.name.includes('InvoiceDetails[')) {
                    const namePattern = /InvoiceDetails\[\d+\]/;
                    input.name = input.name.replace(namePattern, `InvoiceDetails[${index}]`);
                }
            });
        });

        itemIndex = document.querySelectorAll('.item-row').length;
    }

    function updateRowTotal(row) {
        const countInput = row.querySelector('.item-count');
        const priceInput = row.querySelector('.item-price');
        const totalInput = row.querySelector('.item-total');

        const count = parseFloat(countInput.value) || 0;
        const price = parseFloat(priceInput.value) || 0;
        const total = count * price;

        totalInput.value = formatNumber(total);

        totalInput.classList.add('highlight');
        setTimeout(() => {
            totalInput.classList.remove('highlight');
        }, 500);
    }

    function updateGrandTotal() {
        let grandTotal = 0;

        document.querySelectorAll('.item-row').forEach(row => {
            const totalInput = row.querySelector('.item-total');
            const total = parseFloat(totalInput.value.replace(/,/g, '')) || 0;
            grandTotal += total;
        });

        document.getElementById('grandTotal').textContent = formatNumber(grandTotal);
        document.getElementById('totalDisplay').textContent = formatNumber(grandTotal);
    }

    function loadCashiersByBranch(branchId) {
        const cashierSelect = document.getElementById('cashierSelect');

        cashierSelect.innerHTML = '<option value="">جاري التحميل...</option>';
        cashierSelect.disabled = true;

        if (!branchId) {
            cashierSelect.innerHTML = '<option value="">اختر الكاشير</option>';
            cashierSelect.disabled = false;
            return;
        }

        fetch(`/Invoice/GetCashiersByBranch?branchId=${branchId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(cashiers => {
                cashierSelect.innerHTML = '<option value="">اختر الكاشير</option>';

                cashiers.forEach(cashier => {
                    const option = document.createElement('option');
                    option.value = cashier.ID;
                    option.textContent = cashier.Name;
                    cashierSelect.appendChild(option);
                });

                cashierSelect.disabled = false;
            })
            .catch(error => {
                console.error('Error fetching cashiers:', error);
                cashierSelect.innerHTML = '<option value="">خطأ في تحميل البيانات</option>';
                cashierSelect.disabled = false;
                showAlert('حدث خطأ في تحميل بيانات الكاشيرين', 'error');
            });
    }

    function validateForm() {
        let isValid = true;
        const errors = [];

        const customerName = document.querySelector('[name="CustomerName"]');
        if (!customerName.value.trim()) {
            errors.push('اسم العميل مطلوب');
            customerName.classList.add('is-invalid');
            isValid = false;
        } else {
            customerName.classList.remove('is-invalid');
        }

        const branchId = document.querySelector('[name="BranchID"]');
        if (!branchId.value) {
            errors.push('الفرع مطلوب');
            branchId.classList.add('is-invalid');
            isValid = false;
        } else {
            branchId.classList.remove('is-invalid');
        }

        const itemRows = document.querySelectorAll('.item-row');
        let hasValidItems = false;

        itemRows.forEach(row => {
            const itemName = row.querySelector('.item-name');
            const itemCount = row.querySelector('.item-count');
            const itemPrice = row.querySelector('.item-price');

            let rowValid = true;

            if (!itemName.value.trim()) {
                itemName.classList.add('is-invalid');
                rowValid = false;
            } else {
                itemName.classList.remove('is-invalid');
            }

            if (!itemCount.value || parseFloat(itemCount.value) <= 0) {
                itemCount.classList.add('is-invalid');
                rowValid = false;
            } else {
                itemCount.classList.remove('is-invalid');
            }

            if (!itemPrice.value || parseFloat(itemPrice.value) <= 0) {
                itemPrice.classList.add('is-invalid');
                rowValid = false;
            } else {
                itemPrice.classList.remove('is-invalid');
            }

            if (rowValid) {
                hasValidItems = true;
            }
        });

        if (!hasValidItems) {
            errors.push('يجب إدخال صنف واحد صحيح على الأقل');
            isValid = false;
        }

        if (errors.length > 0) {
            showAlert(errors.join('<br>'), 'error');
        }

        return isValid;
    }

    function formatNumber(number) {
        return new Intl.NumberFormat('ar-EG', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        }).format(number);
    }

    function showAlert(message, type = 'info') {
        const alert = document.createElement('div');
        alert.className = `alert alert-${type === 'error' ? 'danger' : type} alert-dismissible fade show`;
        alert.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        const form = document.getElementById('invoiceForm');
        form.insertBefore(alert, form.firstChild);

        setTimeout(() => {
            if (alert.parentNode) {
                alert.remove();
            }
        }, 5000);
    }

    const style = document.createElement('style');
    style.textContent = `
        .highlight {
            background-color: #fff3cd !important;
            transition: background-color 0.5s ease;
        }
    `;
    document.head.appendChild(style);
});