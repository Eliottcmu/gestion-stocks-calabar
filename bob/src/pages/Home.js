// import React, { useState } from 'react';

function Home({ setPage }) {
    // const [events] = useState([
    //     { title: 'Calabar', description: 'Les Ventes du Calabar' },
    // ]);

    // const handleAddEvent = (newEvent) => {
    //     setEvents([events, newEvent]);
    // };

    return (
        <div className="home-container">
            <header>
            </header>
            <main>
                <h1 > Bienvenue sur la page d'accueil</h1>
                <div className="home-main" >
                    <div className="button-container">
                        <div className="bouton-en-colonne">
                            {/* <button onClick={() => setPage('Stocks')} className="nav-button" id="stockage">Stockage</button>
                            <button onClick={() => setPage('Tresorerie')} className="nav-button" id="tresorerie">Trésorerie</button> */}
                        </div>
                        <div className="button-home">
                            {/* <button onClick={() => setPage('Statistiques')} className="nav-button" id="statistiques">Statistiques</button> */}
                        </div>
                    </div>
                    {/* <Evenement events={events} onAddEvent={handleAddEvent} setPage={setPage} /> */}
                </div>
            </main>
            <footer > All Rights Reserved - BDE ENSC © </footer>
        </div>
    );
}

export default Home;