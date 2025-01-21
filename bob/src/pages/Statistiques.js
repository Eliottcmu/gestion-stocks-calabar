import React, { useState, useEffect } from 'react';
import Loader from '../components/Loader/Loader';

function Statistiques({ setPage }) {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        setPage('Statistiques');
        // Simuler un chargement initial
        const timer = setTimeout(() => {
            setLoading(false);
        }, 1000);
        return () => clearTimeout(timer);
    }, [setPage]);

    if (loading) {
        return <Loader message="Chargement des statistiques..." />;
    }

    return (
        <div>
            <header>
                <h1>Statistiques</h1>
            </header>
            <main>
                <div className="statistiques-main">
                    <p>Bienvenue sur la page Statistiques.</p>
                </div>
            </main>
            <footer>
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
}

export default Statistiques;