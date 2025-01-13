import React, { useState, useEffect } from 'react';
import { getUsers, postUser } from '../services/api';
import './Profile.css';

const Profile = () => {
    const [users, setUsers] = useState([]);
    const [newUser, setNewUser] = useState({
        name: '',
        email: '',
        password: '',
    });

    useEffect(() => {
        getUsers()
            .then((data) => setUsers(data))
            .catch((error) => console.error('Erreur lors du chargement des utilisateurs :', error));
    }, []);

    // useEffect(() => {
    //     putUser()
    //         .then((data) => setUsers(data))
    //         .catch((error) => console.error('Erreur lors de la modification de l\'utilisateur :', error));
    // }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewUser((prevUser) => ({
            ...prevUser,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const addedUser = await postUser(newUser);
            setUsers((prevUsers) => [prevUsers, addedUser]);
            setNewUser({ name: '', email: '', password: '' });
        } catch (error) {
            console.error('Erreur lors de l\'ajout de l\'utilisateur :', error);
        }
    };



    if (!users.length) return <div className="loading">Chargement des utilisateurs...</div>;

    return (
        <div className="profile-page">
            <h1>Liste des utilisateurs</h1>
            <div className="users-list">
                {users.map((user) => (
                    <div className="user-card" key={user.id}>
                        <h2>Name : {user.name}</h2>
                        <p>Email : {user.email}</p>
                        <p>Password : {user.password}</p>
                    </div>
                ))}
            </div>
            <div className="form-new-user">
                <h2>Ajouter un utilisateur</h2>
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        name="name"
                        placeholder="Nom"
                        value={newUser.name}
                        onChange={handleInputChange}
                        required
                    />
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={newUser.email}
                        onChange={handleInputChange}
                        required
                    />
                    <input
                        type="password"
                        name="password"
                        placeholder="Mot de passe"
                        value={newUser.password}
                        onChange={handleInputChange}
                        required
                    />
                    <button type="submit">Ajouter</button>
                </form>
            </div>
        </div>
    );
};

export default Profile;
