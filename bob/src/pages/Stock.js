import React, { useState, useEffect } from 'react';
import { getBeers, postBeer } from '../services/api';
import './Pages.css';

function Stock({ setPage }) {
    React.useEffect(() => {
        setPage('Stock');
    }, [setPage]);

    const [beers, setBeers] = useState([]);
    const [newBeer, setNewBeer] = useState({
        name: '',
        quantity: '',
        price: '',
    });

    useEffect(() => {
        getBeers()
            .then((data) => {
                if (Array.isArray(data)) setBeers(data);
                else console.error("Les données reçues ne sont pas un tableau :", data);
            })
            .catch((error) => console.error('Erreur lors du chargement des bières :', error));
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewBeer((prevBeer) => ({
            ...prevBeer,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const addedBeer = await postBeer(newBeer);
            setBeers((prevBeers) => [...prevBeers, addedBeer]);
            setNewBeer({ name: '', quantity: '', price: '' });
        } catch (error) {
            console.error('Erreur lors de l\'ajout de la bière :', error);
        }
    };

    return (
        <div>
            <header>
                <h1>Stock</h1>
            </header>
            <main>
                <div className="stock-main">
                    <p>Bienvenue sur la page Stock.</p>
                </div>
                <h1>Liste des bières</h1>
                <div className="beers-list">
                    {Array.isArray(beers) && beers.length > 0 ? (
                        beers.map((beer) => (
                            <div className="beer-card" key={beer.id}>
                                <h2>Name : {beer.name}</h2>
                                <p>Quantité : {beer.quantity}</p>
                                <p>Prix : {beer.price}</p>
                            </div>
                        ))
                    ) : (
                        <p>Chargement ou aucune bière disponible.</p>
                    )}
                </div>
                <div>
                    <h2>Ajouter une bière</h2>
                    <form onSubmit={handleSubmit}>
                        <input
                            type="text"
                            name="name"
                            placeholder="Nom"
                            value={newBeer.name}
                            onChange={handleInputChange}
                            required
                        />
                        <input
                            type="text"
                            name="quantity"
                            placeholder="Quantité"
                            value={newBeer.quantity}
                            onChange={handleInputChange}
                            required
                        />
                        <input
                            type="text"
                            name="price"
                            placeholder="Prix"
                            value={newBeer.price}
                            onChange={handleInputChange}
                            required
                        />
                        <button type="submit">Ajouter</button>
                    </form>
                </div>
            </main>
            <footer>
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
}

export default Stock;
