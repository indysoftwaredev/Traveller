const ShipBuilder = {
    init: function () {
        this.initializeHullCalculation();
        this.initializeArmorCalculation();
        this.initializeComponentDeletion();
        this.initializeArmorValidation();
    },

    initializeHullCalculation: function () {
        const hullForm = document.querySelector('#hullSelectionSection form');
        if (!hullForm) return;

        const inputs = hullForm.querySelectorAll('input, select');
        inputs.forEach(input => {
            input.addEventListener('change', this.updateHull);
            if (input.type === 'number') {
                input.addEventListener('input', this.updateHull);
            }
        });
    },

    updateHull: function () {
        const hullForm = document.querySelector('#hullSelectionSection form');
        const formData = new FormData(hullForm);

        fetch('/ShipBuilder/CalculateHull', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.text();
            })
            .then(html => {
                if (html.includes("System.InvalidOperationException")) {
                    console.error("Server error:", html);
                    return;
                }
                document.getElementById('shipSummaryContainer').innerHTML = html;

                // After hull update, recalculate all components
                const componentRows = document.querySelectorAll('#shipSummary tbody tr');
                componentRows.forEach((row, index) => {
                    fetch(`/ShipBuilder/RecalculateComponent?componentType=${row.dataset.componentType}&index=${index}`)
                        .then(response => response.json())
                        .then(data => {
                            // Update the row with new values
                            const cells = row.cells;
                            cells[1].textContent = data.techLevel;
                            cells[2].textContent = data.tonsDisplacement;
                            cells[3].textContent = data.powerRequired;
                            cells[4].textContent = data.costMCr;
                        })
                        .catch(error => console.error('Error updating component:', error));
                });
            })
            .catch(error => {
                console.error('Error:', error);
            });
    },

    initializeArmorCalculation: function () {
        const armorTypeSelect = document.getElementById('armorType');
        const protectionLevelInput = document.getElementById('protectionLevel');

        if (!armorTypeSelect || !protectionLevelInput) return;

        armorTypeSelect.addEventListener('change', this.updateArmorCalculations);
        protectionLevelInput.addEventListener('input', this.updateArmorCalculations);

        // Run initial calculation
        this.updateArmorCalculations();
    },

    updateArmorCalculations: function () {
        const armorTypeSelect = document.getElementById('armorType');
        const protectionLevelInput = document.getElementById('protectionLevel');
        const costMcrDisplay = document.getElementById('costMCr');
        const tonnageDisplay = document.getElementById('tonsDisplacement');
        const techLevelDisplay = document.getElementById('techLevel');

        const armorType = armorTypeSelect.value;
        const protectionLevel = protectionLevelInput.value;

        fetch(`/ShipBuilder/CalculateArmor?armorType=${armorType}&protectionLevel=${protectionLevel}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                costMcrDisplay.value = data.costMCr.toFixed(0);
                tonnageDisplay.value = data.tonsDisplacement.toFixed(2);
                techLevelDisplay.value = data.techLevel;
            })
            .catch(error => {
                console.error('Error:', error);
            });
    },

    initializeArmorValidation: function () {
        const armorForm = document.querySelector('#armorSelectionSection form');
        if (!armorForm) return;

        armorForm.addEventListener('submit', (e) => {
            if (!this.validateArmorTonnage()) {
                e.preventDefault();
            }
        });
    },

    validateArmorTonnage: function () {
        // Get the tonnage value from the form
        const tonnageInput = document.getElementById('tonsDisplacement');
        const armorTonnage = parseFloat(tonnageInput.value);

        // Get the current cargo space from the ship summary
        const cargoSpaceElement = document.querySelector('#shipSummary .card-header div:nth-child(5)');
        if (!cargoSpaceElement) {
            console.error('Could not find cargo space element');
            return false;
        }

        // Extract the cargo space value (format: "Cargo: XXXt")
        const cargoMatch = cargoSpaceElement.textContent.match(/Cargo: ([\d.]+)t/);
        if (!cargoMatch) {
            console.error('Could not parse cargo space value');
            return false;
        }

        const availableCargo = parseFloat(cargoMatch[1]);

        // Check if armor tonnage exceeds available cargo space
        if (armorTonnage > availableCargo) {
            // Show error message
            this.showValidationError(`Cannot add armor: Required space (${armorTonnage}t) exceeds available cargo space (${availableCargo}t)`);
            return false;
        }

        // Clear any existing error messages
        this.clearValidationError();
        return true;
    },

    showValidationError: function (message) {
        // Clear any existing error messages
        this.clearValidationError();

        // Create and insert error message
        const errorDiv = document.createElement('div');
        errorDiv.className = 'alert alert-danger mt-2';
        errorDiv.id = 'armorValidationError';
        errorDiv.textContent = message;

        const armorForm = document.querySelector('#armorSelectionSection form');
        armorForm.querySelector('.card-body').appendChild(errorDiv);
    },

    clearValidationError: function () {
        const existingError = document.getElementById('armorValidationError');
        if (existingError) {
            existingError.remove();
        }
    },

    initializeArmorCalculation: function () {
        const armorTypeSelect = document.getElementById('armorType');
        const protectionLevelInput = document.getElementById('protectionLevel');

        if (!armorTypeSelect || !protectionLevelInput) return;

        armorTypeSelect.addEventListener('change', this.updateArmorCalculations);
        protectionLevelInput.addEventListener('input', this.updateArmorCalculations);

        // Run initial calculation
        this.updateArmorCalculations();
    },

    updateArmorCalculations: function () {
        const armorTypeSelect = document.getElementById('armorType');
        const protectionLevelInput = document.getElementById('protectionLevel');
        const costMcrDisplay = document.getElementById('costMCr');
        const tonnageDisplay = document.getElementById('tonsDisplacement');
        const techLevelDisplay = document.getElementById('techLevel');

        const armorType = armorTypeSelect.value;
        const protectionLevel = protectionLevelInput.value;

        fetch(`/ShipBuilder/CalculateArmor?armorType=${armorType}&protectionLevel=${protectionLevel}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                costMcrDisplay.value = data.costMCr.toFixed(0);
                tonnageDisplay.value = data.tonsDisplacement.toFixed(2);
                techLevelDisplay.value = data.techLevel;
            })
            .catch(error => {
                console.error('Error:', error);
            });
    },

    initializeComponentDeletion: function () {
        document.addEventListener('click', function (e) {
            if (e.target.closest('.delete-component')) {
                const row = e.target.closest('tr');
                const componentId = row.dataset.componentId;

                fetch('/ShipBuilder/DeleteComponent', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ componentId: componentId })
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.text();
                    })
                    .then(html => {
                        document.getElementById('shipSummaryContainer').innerHTML = html;
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            }
        });
    }
};

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    ShipBuilder.init();
});

export default ShipBuilder;