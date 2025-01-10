import React from 'react';
import './Home.css';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { FaUser } from 'react-icons/fa';

function Home({ setPage }) {
    const navigate = useNavigate();
    return (
        <div className="home-container">
            <header className="header">
                <h1>Accueil</h1>
                <Link to="/profile" className="profile-link">
                    <FaUser />
                </Link>
                {/* <button className="back-button">🔙</button> */}
                {/*<button className="notification-button">🔔</button> */}
            </header>
            <main>
                <div className="sections-container">
                    <button
                        className="section-button"
                        onClick={() => navigate('/statistiques')}
                    >
                        <div className="section-image statistiques"></div>
                        <span>STATISTIQUES</span>
                    </button>
                    <button
                        className="section-button"
                        onClick={() => navigate('/stock')}
                    >
                        <div className="section-image stockage"></div>
                        <span>STOCKAGE</span>
                    </button>
                    <button
                        className="section-button"
                        onClick={() => navigate('/tresorerie')}
                    >
                        <div className="section-image tresorerie"></div>
                        <span>TRÉSORERIE</span>
                    </button>
                </div>
            </main>
            <footer>All Rights Reserved - BDE ENSC ©</footer>
        </div>
    );
}

export default Home;
