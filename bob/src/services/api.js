import axios from 'axios';

const BASE_URL = 'http://localhost:5000/api';

export const getUsers = async () => {
    try {
        const response = await axios.get(`${BASE_URL}/users`);
        return response.data;
    } catch (error) {
        console.error('Erreur lors de la récupération des utilisateurs :', error);
        throw error;
    }
};

export const getBeers = async () => {
    try {
        const response = await axios.get(`${BASE_URL}/stock`);
        return response.data;
    } catch (error) {
        console.error('Erreur lors de la récupération des bières :', error);
        throw error;
    }
}

export const postBeer = async (beer) => {
    try {
        const response = await axios.post(`${BASE_URL}/stock`, beer);
        return response.data;
    } catch (error) {
        console.error('Erreur lors de la création de la bière :', error);
        throw error;
    }
}

// export const putUser = async (user) => {
//     try {
//         const response = await axios.put(`${BASE_URL}/users/${user.id}`, user);
//         return response.data;
//     } catch (error) {
//         console.error('Erreur lors de la modification de l\'utilisateur :', error);
//         throw error;
//     }
// }

export const postUser = async (user) => {
    try {
        const response = await axios.post(`${BASE_URL}/users`, user);
        return response.data;
    } catch (error) {
        console.error('Erreur lors de la création de l\'utilisateur :', error);
        throw error;
    }
}