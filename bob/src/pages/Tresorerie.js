import React from 'react';
import './Pages.css';

function Tresorerie({ setPage }) {
    React.useEffect(() => {
        setPage('Tresorerie');
    }, [setPage]);

    return (
        <div>
            <header>
                <h1>Tresorerie</h1>
            </header>
            <main>
                <div className="tresorerie-main">
                    <p>Bienvenue sur la page de Tresorerie.</p>
                </div>
            </main>
            <footer>
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
}

export default Tresorerie;
