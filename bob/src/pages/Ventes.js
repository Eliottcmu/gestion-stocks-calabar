import React from 'react';
import './Pages.css';

function Ventes({ setPage }) {
    React.useEffect(() => {
        setPage('Stock');
    }, [setPage]);

    return (
        <div>
            <header>
                <h1>Ventes</h1>
            </header>
            <main>
                <div className="ventes-main">
                    <p>Bienvenue sur la page Ventes.</p>
                </div>
            </main>
            <footer>
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
}

export default Ventes;
