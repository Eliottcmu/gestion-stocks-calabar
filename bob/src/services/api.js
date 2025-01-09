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
