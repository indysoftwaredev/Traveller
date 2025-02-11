// DOM Selectors - Single source of truth for element selection
const Selectors = {
    hull: {
        form: '#hullSelectionSection form',
        inputs: '#hullSelectionSection form input, #hullSelectionSection form select'
    },
    armor: {
        form: '#armorSelectionSection form',
        type: '#armorType',
        protection: '#protectionLevel',
        cost: '#costMCr',
        tonnage: '#tonsDisplacement',
        techLevel: '#techLevel'
    },
    ship: {
        summary: '#shipSummary',
        container: '#shipSummaryContainer',
        cargoSpace: '#shipSummary .card-header div:nth-child(5)',
        components: '#shipSummary tbody tr'
    }
};

// API endpoints
const Endpoints = {
    calculateHull: '/ShipBuilder/CalculateHull',
    calculateArmor: '/ShipBuilder/CalculateArmor',
    deleteComponent: '/ShipBuilder/DeleteComponent',
    recalculateComponent: '/ShipBuilder/RecalculateComponent'
};

// HTTP client with consistent error handling
const HttpClient = {
    async post(url, data) {
        const response = await fetch(url, {
            method: 'POST',
            body: data,
            headers: data instanceof FormData ? {} : {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const contentType = response.headers.get('content-type');
        if (contentType?.includes('application/json')) {
            return response.json();
        }
        return response.text();
    },

    async get(url) {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    }
};

// UI Component for managing validation errors
class ValidationUI {
    static showError(container, message, id = 'validationError') {
        this.clearError(id);
        const errorDiv = document.createElement('div');
        errorDiv.className = 'alert alert-danger mt-2';
        errorDiv.id = id;
        errorDiv.textContent = message;
        container.appendChild(errorDiv);
    }

    static clearError(id) {
        const existingError = document.getElementById(id);
        if (existingError) {
            existingError.remove();
        }
    }
}

// Hull Management
class HullManager {
    constructor() {
        this.form = document.querySelector(Selectors.hull.form);
        this.bindEvents();
    }

    bindEvents() {
        if (!this.form) return;

        const inputs = this.form.querySelectorAll(Selectors.hull.inputs);
        inputs.forEach(input => {
            input.addEventListener('change', () => this.updateHull());
            if (input.type === 'number') {
                input.addEventListener('input', () => this.updateHull());
            }
        });
    }

    async updateHull() {
        try {
            const html = await HttpClient.post(Endpoints.calculateHull, new FormData(this.form));
            document.querySelector(Selectors.ship.container).innerHTML = html;
            await this.recalculateComponents();
        } catch (error) {
            console.error('Hull update error:', error);
        }
    }

    async recalculateComponents() {
        const componentRows = document.querySelectorAll(Selectors.ship.components);
        const updates = Array.from(componentRows).map(async (row, index) => {
            try {
                const data = await HttpClient.get(
                    `${Endpoints.recalculateComponent}?componentType=${row.dataset.componentType}&index=${index}`
                );
                this.updateComponentRow(row, data);
            } catch (error) {
                console.error('Component recalculation error:', error);
            }
        });
        await Promise.all(updates);
    }

    updateComponentRow(row, data) {
        const cells = row.cells;
        cells[1].textContent = data.techLevel;
        cells[2].textContent = data.tonsDisplacement;
        cells[3].textContent = data.powerRequired;
        cells[4].textContent = data.costMCr;
    }
}

// Armor Management
class ArmorManager {
    constructor() {
        this.form = document.querySelector(Selectors.armor.form);
        this.bindEvents();
    }

    bindEvents() {
        if (!this.form) return;

        const typeSelect = document.querySelector(Selectors.armor.type);
        const protectionInput = document.querySelector(Selectors.armor.protection);

        if (typeSelect && protectionInput) {
            typeSelect.addEventListener('change', () => this.updateCalculations());
            protectionInput.addEventListener('input', () => this.updateCalculations());
            this.form.addEventListener('submit', (e) => this.handleSubmit(e));
        }

        this.updateCalculations();
    }

    async updateCalculations() {
        try {
            const type = document.querySelector(Selectors.armor.type).value;
            const protection = document.querySelector(Selectors.armor.protection).value;

            const data = await HttpClient.get(
                `${Endpoints.calculateArmor}?armorType=${type}&protectionLevel=${protection}`
            );

            this.updateDisplays(data);
        } catch (error) {
            console.error('Armor calculation error:', error);
        }
    }

    updateDisplays(data) {
        document.querySelector(Selectors.armor.cost).value = data.costMCr.toFixed(0);
        document.querySelector(Selectors.armor.tonnage).value = data.tonsDisplacement.toFixed(2);
        document.querySelector(Selectors.armor.techLevel).value = data.techLevel;
    }

    handleSubmit(e) {
        if (!this.validateTonnage()) {
            e.preventDefault();
        }
    }

    validateTonnage() {
        const tonnage = parseFloat(document.querySelector(Selectors.armor.tonnage).value);
        const cargoElement = document.querySelector(Selectors.ship.cargoSpace);

        if (!cargoElement) {
            console.error('Cargo space element not found');
            return false;
        }

        const cargoMatch = cargoElement.textContent.match(/Cargo: ([\d.]+)t/);
        if (!cargoMatch) {
            console.error('Could not parse cargo space value');
            return false;
        }

        const availableCargo = parseFloat(cargoMatch[1]);

        if (tonnage > availableCargo) {
            ValidationUI.showError(
                this.form.querySelector('.card-body'),
                `Cannot add armor: Required space (${tonnage}t) exceeds available cargo space (${availableCargo}t)`,
                'armorValidationError'
            );
            return false;
        }

        ValidationUI.clearError('armorValidationError');
        return true;
    }
}

// Component Deletion Management
class ComponentManager {
    constructor() {
        this.bindEvents();
    }

    bindEvents() {
        document.addEventListener('click', async (e) => {
            if (e.target.closest('.delete-component')) {
                await this.handleDelete(e);
            }
        });
    }

    async handleDelete(e) {
        const row = e.target.closest('tr');
        const componentId = row.dataset.componentId;

        try {
            const html = await HttpClient.post(
                Endpoints.deleteComponent,
                JSON.stringify({ componentId })
            );
            document.querySelector(Selectors.ship.container).innerHTML = html;
        } catch (error) {
            console.error('Component deletion error:', error);
        }
    }
}

// State management for collapsible sections
class CollapseStateManager {
    constructor() {
        this.STORAGE_KEY = 'shipBuilder_collapseStates';
        this.bindEvents();
        this.restoreStates();
        this.bindFormSubmits();
    }

    bindFormSubmits() {
        document.querySelectorAll('form').forEach(form => {
            form.addEventListener('submit', () => this.saveAllStates());
        });
    }

    saveAllStates() {
        document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(trigger => {
            const targetId = trigger.getAttribute('data-bs-target').substring(1);
            this.saveState(targetId);
        });
    }

    bindEvents() {
        document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(trigger => {
            const targetId = trigger.getAttribute('data-bs-target').substring(1);
            const section = document.getElementById(targetId);

            section.addEventListener('shown.bs.collapse', () => {
                this.saveState(targetId, true);
            });

            section.addEventListener('hidden.bs.collapse', () => {
                this.saveState(targetId, false);
            });
        });
    }

    saveState(sectionId, isExpanded) {
        const states = this.getStoredStates();
        states[sectionId] = isExpanded;
        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(states));
    }

    restoreStates() {
        const states = this.getStoredStates();
        Object.entries(states).forEach(([id, isExpanded]) => {
            const section = document.getElementById(id);
            if (section) {
                if (isExpanded) {
                    section.classList.add('show');
                } else {
                    section.classList.remove('show');
                }
            }
        });
    }

    getStoredStates() {
        const stored = localStorage.getItem(this.STORAGE_KEY);
        return stored ? JSON.parse(stored) : {};
    }
}

// Main ShipBuilder Application
class ShipBuilder {
    static init() {
        this.hullManager = new HullManager();
        this.armorManager = new ArmorManager();
        this.componentManager = new ComponentManager();
        this.collapseManager = new CollapseStateManager();
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => ShipBuilder.init());

export default ShipBuilder;