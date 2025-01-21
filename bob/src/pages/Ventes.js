import React, { useState, useEffect } from 'react';
import { getBeers, putBeer, postVentes } from '../services/api';

const Ventes = ({ setPage, currentUser }) => {
    const [beers, setBeers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        setPage('Ventes');
        loadBeers();
    }, [setPage]);

    const loadBeers = async () => {
        try {
            const data = await getBeers();
            setBeers(data);
            setLoading(false);
        } catch (err) {
            setError('Erreur lors du chargement des bières');
            setLoading(false);
        }
    };

    const handleSell = async (beer) => {
        if (beer.quantity <= 0) {
            alert('Plus de stock disponible pour cette bière !');
            return;
        }

        try {
            const updatedBeer = {
                ...beer,
                quantity: beer.quantity - 1
            };

            // Mettre à jour le stock
            await putBeer(beer.id, updatedBeer);

            // Créer la vente
            const vente = {
                date: new Date(),
                idProduit: beer.id,
                // idUser: currentUser ? currentUser.id : null, -> plus tard Récupérer l'id de l'utilisateur connecté
                quantite: 1,
                montant: beer.price,
                name: beer.name,
                // Ajouter le nom de l'utilisateur, si disponible
                // userName: currentUser ? currentUser.name : null
            };

            // Enregistrer la vente
            await postVentes(vente);

            // Recharger les bières pour mettre à jour l'affichage
            loadBeers();
        } catch (err) {
            setError('Erreur lors de la vente');
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
                    <h1 className="text-3xl font-bold text-gray-900">Ventes</h1>
                </div>
            </header>

            <main className="max-w-7xl mx-auto py-6 px-4">
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {beers.map((beer) => (
                        <div
                            key={beer.id}
                            className="bg-white rounded-lg shadow p-6 hover:shadow-lg transition-shadow"
                        >
                            <h2 className="text-xl font-semibold text-gray-900">{beer.name}</h2>
                            <div className="mt-4">
                                <p className="text-gray-600">Prix: {beer.price} €</p>
                                <p className="text-gray-600">
                                    Stock: <span className={beer.quantity <= 5 ? 'text-red-500' : ''}>
                                        {beer.quantity}
                                    </span>
                                </p>
                            </div>
                            <button
                                onClick={() => handleSell(beer)}
                                disabled={beer.quantity <= 0}
                                className={`mt-4 w-full px-4 py-2 rounded-md text-white font-medium
                  ${beer.quantity <= 0
                                        ? 'bg-gray-400 cursor-not-allowed'
                                        : 'bg-blue-600 hover:bg-blue-700'}`}
                            >
                                Vendre
                            </button>
                        </div>
                    ))}
                </div>
            </main>

            <footer className="bg-white mt-8 py-4 text-center">
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
};

export default Ventes;
