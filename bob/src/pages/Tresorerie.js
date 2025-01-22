import React, { useState, useEffect } from 'react';
import { getVentes } from '../services/api';
import Loader from '../components/Loader/Loader';

function formatDateTime(datetime) {
    const options = {
        day: '2-digit',
        month: 'long',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    };

    return new Intl.DateTimeFormat('fr-FR', options).format(new Date(datetime));
}


const CustomSelect = ({ value, onChange, options }) => (
    <select
        className="custom-select"
        value={value}
        onChange={(e) => onChange(e.target.value)}
    >
        {options.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
        ))}
    </select>
);

const CustomInput = ({ type, value, onChange, placeholder }) => (
    <input
        type={type}
        className="custom-input"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
    />
);

const CustomButton = ({ onClick, variant = 'primary', children }) => (
    <button
        onClick={onClick}
        className={`custom-button ${variant === 'primary' ? 'custom-button-primary' : 'custom-button-secondary'}`}
    >
        {children}
    </button>
);

const CustomDatePicker = ({ value, onChange, placeholder }) => (
    <input
        type="date"
        className="custom-date-picker"
        value={value || ''}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
    />
);

const Tresorerie = ({ setPage }) => {
    const [ventes, setVentes] = useState([]);
    const [filteredVentes, setFilteredVentes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [total, setTotal] = useState(0);

    const [dateStart, setDateStart] = useState('');
    const [dateEnd, setDateEnd] = useState('');
    const [produitFilter, setProduitFilter] = useState('');
    const [groupBy, setGroupBy] = useState('none');

    useEffect(() => {
        setPage('Tresorerie');
        loadVentes();
    }, [setPage]);

    const loadVentes = async () => {
        try {
            const data = await getVentes();
            setVentes(data);
            setFilteredVentes(data);
            updateTotal(data);
            setLoading(false);
        } catch (err) {
            setError('Erreur lors du chargement des ventes');
            setLoading(false);
        }
    };

    const updateTotal = (data) => {
        const totalVentes = data.reduce((acc, vente) => acc + vente.montant, 0);
        setTotal(totalVentes);
    };

    const applyFilters = () => {
        let filtered = [...ventes];

        if (dateStart) {
            filtered = filtered.filter(vente => vente.date >= dateStart);
        }
        if (dateEnd) {
            filtered = filtered.filter(vente => vente.date <= dateEnd);
        }
        if (produitFilter) {
            filtered = filtered.filter(vente =>
                vente.name.toLowerCase().includes(produitFilter.toLowerCase())
            );
        }

        setFilteredVentes(filtered);
        updateTotal(filtered);
    };

    const resetFilters = () => {
        setDateStart('');
        setDateEnd('');
        setProduitFilter('');
        setGroupBy('none');
        setFilteredVentes(ventes);
        updateTotal(ventes);
    };

    const groupVentes = (ventes) => {
        if (groupBy === 'none') return ventes;

        const grouped = {};
        ventes.forEach(vente => {
            let key = '';
            const date = new Date(vente.date);

            switch (groupBy) {
                case 'day':
                    key = date.toLocaleDateString();
                    break;
                case 'month':
                    key = `${date.getMonth() + 1}/${date.getFullYear()}`;
                    break;
                case 'year':
                    key = date.getFullYear().toString();
                    break;
                default:
                    return vente;
            }

            if (!grouped[key]) {
                grouped[key] = {
                    date: key,
                    name: `Total ${key}`,
                    montant: 0,
                    entries: []
                };
            }
            grouped[key].montant += vente.montant;
            grouped[key].entries.push(vente);
        });

        return Object.values(grouped);
    };

    if (loading) return <Loader message="Chargement de la trésorerie..." />;
    if (error) return <div className="error-message" role="alert">{error}</div>;

    const groupedVentes = groupVentes(filteredVentes);
    const groupOptions = [
        { value: 'none', label: 'Pas de groupement' },
        { value: 'day', label: 'Par jour' },
        { value: 'month', label: 'Par mois' },
        { value: 'year', label: 'Par année' }
    ];

    return (
        <div className="tresorerie-container">
            <header className="header" role="banner">
                <div className="header-content">
                    <h1 tabIndex="0">Trésorerie</h1>
                </div>
            </header>

            <main className="main-content" role="main">
                <div className="filters-container">
                    <div className="filters-grid">
                        <CustomDatePicker
                            value={dateStart}
                            onChange={setDateStart}
                            placeholder="Date début"
                        />
                        <CustomDatePicker
                            value={dateEnd}
                            onChange={setDateEnd}
                            placeholder="Date fin"
                        />
                        <CustomInput
                            type="text"
                            placeholder="Filtrer par produit"
                            value={produitFilter}
                            onChange={setProduitFilter}
                        />
                    </div>

                    <div className="controls-row">
                        <CustomSelect
                            value={groupBy}
                            onChange={setGroupBy}
                            options={groupOptions}
                        />
                        <CustomButton onClick={applyFilters}>
                            Appliquer les filtres
                        </CustomButton>
                        <CustomButton variant="secondary" onClick={resetFilters}>
                            Réinitialiser
                        </CustomButton>
                    </div>
                </div>

                <div className="total-card" tabIndex="0" aria-label="Résumé des ventes">
                    <h2>Total des ventes: <span aria-label="Montant">{total.toFixed(2)} €</span></h2>
                </div>

                <div className="table-container">
                    <div className="table-scroll" role="region" aria-label="Liste des ventes" tabIndex="0">
                        {groupedVentes.map((vente, index) => (
                            <div key={index} className="card">
                                <div className="card-header">
                                    <span>{formatDateTime(vente.date)}</span>
                                    <span>  {vente.montant.toFixed(2)} €</span>
                                </div>
                                <div className="card-body">
                                    <span>Produit : {vente.name}</span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

            </main>

            <footer className="footer" role="contentinfo">
                <p>All Rights Reserved - BDE ENSC © <span className="sr-only">Bureau des étudiants ENSC</span></p>
            </footer>
        </div>
    );
};

export default Tresorerie;
