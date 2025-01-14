import React, { useState, useEffect } from 'react';
import { getUsers, postUser, putUser, deleteUser } from '../services/api';
import './Profile.css';

const Profile = () => {
    const [users, setUsers] = useState([]);
    const [newUser, setNewUser] = useState({ name: '', email: '', password: '' });
    const [editingUser, setEditingUser] = useState(null);
    const [editForm, setEditForm] = useState({ name: '', email: '', password: '' });

    useEffect(() => {
        getUsers()
            .then((userData) => {
                setUsers(userData);
            })
            .catch(error => console.error('Erreur lors du chargement :', error));
    }, []);

    const handleDeleteUser = async (userId) => {
        if (window.confirm('Êtes-vous sûr de vouloir supprimer cet utilisateur ?')) {
            try {
                await deleteUser(userId);
                setUsers(prevUsers => prevUsers.filter(user => user.id !== userId));
            } catch (error) {
                console.error('Erreur lors de la suppression de l\'utilisateur :', error);
            }
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewUser((prevUser) => ({
            ...prevUser,
            [name]: value,
        }));
    };

    const handleEditInputChange = (e) => {
        const { name, value } = e.target;
        setEditForm((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const addedUser = await postUser(newUser);
            setUsers((prevUsers) => [...prevUsers, addedUser]);
            setNewUser({ name: '', email: '', password: '' });
        } catch (error) {
            console.error('Erreur lors de l\'ajout de l\'utilisateur :', error);
        }
    };

    const startEditing = (user) => {
        setEditingUser(user.id);
        setEditForm({
            name: user.name,
            email: user.email,
            password: user.password,
        });
    };

    const handleUpdateSubmit = async (userId) => {
        try {
            const updatedUser = await putUser(userId, editForm);
            setUsers((prevUsers) =>
                prevUsers.map((user) =>
                    user.id === userId ? updatedUser : user
                )
            );
            setEditingUser(null);
        } catch (error) {
            console.error('Erreur lors de la modification de l\'utilisateur :', error);
        }
    };

    if (!users.length) return <div className="loading">Chargement...</div>;

    return (
        <div className="profile-page">
            <div className="users-section">
                <h1>Liste des utilisateurs</h1>
                <div className="users-list">
                    {users.map((user) => (
                        <div className="user-card" key={user.id}>
                            {editingUser === user.id ? (
                                <form onSubmit={(e) => {
                                    e.preventDefault();
                                    handleUpdateSubmit(user.id);
                                }}>
                                    <input
                                        type="text"
                                        name="name"
                                        value={editForm.name}
                                        onChange={handleEditInputChange}
                                        required
                                    />
                                    <input
                                        type="email"
                                        name="email"
                                        value={editForm.email}
                                        onChange={handleEditInputChange}
                                        required
                                    />
                                    <input
                                        type="password"
                                        name="password"
                                        value={editForm.password}
                                        onChange={handleEditInputChange}
                                        required
                                    />
                                    <div className="button-group">
                                        <button type="submit">Sauvegarder</button>
                                        <button type="button" onClick={() => setEditingUser(null)}>Annuler</button>
                                    </div>
                                </form>
                            ) : (
                                <>
                                    <h2>Name : {user.name}</h2>
                                    <p>Email : {user.email}</p>
                                    <p>Password : {user.password}</p>
                                    <div className="button-group">
                                        <button onClick={() => startEditing(user)}>Modifier</button>
                                        <button onClick={() => handleDeleteUser(user.id)} className="delete-button">
                                            Supprimer
                                        </button>
                                    </div>
                                </>
                            )}
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
        </div>
    );
};

export default Profile;
