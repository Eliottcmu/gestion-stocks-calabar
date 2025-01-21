import React, { useState, useEffect } from 'react';
import { getVentes } from '../services/api';
import Loader from '../components/Loader/Loader';
import './Tresorerie.css';

const Tresorerie = ({ setPage }) => {
    const [ventes, setVentes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [total, setTotal] = useState(0);

    useEffect(() => {
        setPage('Tresorerie');
        loadVentes();
    }, [setPage]);

    const loadVentes = async () => {
        try {
            const data = await getVentes();
            setVentes(data);
            const totalVentes = data.reduce((acc, vente) => acc + vente.montant, 0);
            setTotal(totalVentes);
            setLoading(false);
        } catch (err) {
            setError('Erreur lors du chargement des ventes');
            setLoading(false);
        }
    };

    if (loading) {
        return <Loader message="Chargement de la trésorerie..." />;
    }

    if (error) {
        return (
            <div className="error-message" role="alert">
                {error}
            </div>
        );
    }

    return (
        <div className="tresorerie-container">
            <header className="header" role="banner">
                <div className="header-content">
                    <h1 tabIndex="0">Trésorerie</h1>
                </div>
            </header>

            <main className="main-content" role="main">
                <div className="total-card" tabIndex="0" aria-label="Résumé des ventes">
                    <h2>Total des ventes: <span aria-label="Montant">{total.toFixed(2)} €</span></h2>
                </div>

                <div className="table-container">
                    <div className="table-scroll" role="region" aria-label="Liste des ventes" tabIndex="0">
                        <table role="table">
                            <caption className="sr-only">Liste détaillée des ventes</caption>
                            <thead>
                                <tr>
                                    <th scope="col" role="columnheader">Date</th>
                                    <th scope="col" role="columnheader">Produit</th>
                                    <th scope="col" role="columnheader">Quantité</th>
                                    <th scope="col" role="columnheader">Montant</th>
                                </tr>
                            </thead>
                            <tbody>
                                {ventes.map((vente) => (
                                    <tr key={vente.id} role="row">
                                        <td role="cell" data-label="Date">
                                            {new Date(vente.date).toLocaleDateString()}
                                        </td>
                                        <td role="cell" data-label="Produit">
                                            {vente.name}
                                        </td>
                                        <td role="cell" data-label="Montant">
                                            {vente.montant.toFixed(2)} €
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
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