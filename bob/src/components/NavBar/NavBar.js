import React from 'react';
import NavLink from '../NavLink/NavLink';
import './NavBar.css';

function NavBar({ setPage }) {
    return (
        <nav className="navbar">
            {/* <img src='/img/Logo Blanc.png' id="logo"></img> */}
            <NavLink onClick={() => setPage('Home')} text="Accueil" />
            <NavLink onClick={() => setPage('Stock')} text="Stocks" />
            <NavLink onClick={() => setPage('Statistiques')} text="Statistiques" />
            <NavLink onClick={() => setPage('Tresorerie')} text="Trésorerie" />
            <NavLink onClick={() => setPage('Ventes')} text="Ventes" />

        </nav>
    );
}
export default NavBar;