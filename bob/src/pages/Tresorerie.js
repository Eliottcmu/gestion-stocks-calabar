import React, { useState, useEffect } from 'react';
import { getVentes } from '../services/api';

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

            // Calculer le total des ventes
            const totalVentes = data.reduce((acc, vente) => acc + vente.montant, 0);
            setTotal(totalVentes);

            setLoading(false);
        } catch (err) {
            setError('Erreur lors du chargement des ventes');
            setLoading(false);
        }
    };

    if (loading) {
        return <div className="flex justify-center items-center h-screen">Chargement...</div>;
    }

    if (error) {
        return <div className="text-red-500 text-center p-4">{error}</div>;
    }

    return (
        <div className="min-h-screen bg-gray-100">
            <header className="bg-white shadow">
                <div className="max-w-7xl mx-auto py-6 px-4">
                    <h1 className="text-3xl font-bold text-gray-900">Trésorerie</h1>
                </div>
            </header>

            <main className="max-w-7xl mx-auto py-6 px-4">
                <div className="bg-white rounded-lg shadow p-6 mb-6">
                    <h2 className="text-2xl font-semibold text-gray-900">
                        Total des ventes: {total.toFixed(2)} €
                    </h2>
                </div>

                <div className="bg-white rounded-lg shadow overflow-hidden">
                    <table className="min-w-full">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Date
                                </th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Produit
                                </th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Quantité
                                </th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Montant
                                </th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {ventes.map((vente) => (
                                <tr key={vente.id}>
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        {new Date(vente.date).toLocaleDateString()}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        {vente.name}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        {vente.quantite}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap">
                                        {vente.montant.toFixed(2)} €
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </main>

            <footer className="bg-white mt-8 py-4 text-center">
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
};

export default Tresorerie;