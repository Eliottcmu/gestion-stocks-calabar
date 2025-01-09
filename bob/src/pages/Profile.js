import React, { useState, useEffect } from 'react';
import { getUsers } from '../services/api';
import './Profile.css'; // Assurez-vous que votre fichier CSS est bien importé.

const Profile = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        getUsers()
            .then((data) => setUsers(data))
            .catch((error) => console.error('Erreur lors du chargement des utilisateurs :', error));
    }, []);

    if (!users.length) return <div className="loading">Chargement des utilisateurs...</div>;

    return (
        <div className="profile-page">
            <h1>Liste des utilisateurs</h1>
            <div className="users-list">
                {users.map((user) => (
                    <div className="user-card" key={user.id}>
                        <h2>{user.name}</h2>
                        <p>{user.email}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Profile;
