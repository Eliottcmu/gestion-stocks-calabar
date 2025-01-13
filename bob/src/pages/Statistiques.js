import React from 'react';
import './Pages.css';

function Statistiques({ setPage }) {
    // Appel de setPage pour mettre à jour l'état si nécessaire
    React.useEffect(() => {
        setPage('Statistiques');
    }, [setPage]);

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
