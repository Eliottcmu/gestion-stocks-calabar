import React from 'react';

function Stock({ setPage }) {
    React.useEffect(() => {
        setPage('Stock');
    }, [setPage]);

    return (
        <div>
            <header>
                <h1>Stock</h1>
            </header>
            <main>
                <div className="stock-main">
                    <p>Bienvenue sur la page Stock.</p>
                </div>
            </main>
            <footer>
                All Rights Reserved - BDE ENSC ©
            </footer>
        </div>
    );
}

export default Stock;
