import axios from 'axios';
import ApiService from './ApiService';

const $http = axios.create({
    baseURL: "http://localhost:5268/",
    headers: {
        "Content-type": "application/json"
    }
});

$http.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

const followUser = async (followId) => { 
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }

        const userId = parseInt(localStorage.getItem('id'));
        followId = parseInt(followId);
        const response = await $http.post(`api/Follow/addFollow/${userId}/${followId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error following user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const unfollowUser = async (followId) => { 
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }

        const userId = parseInt(localStorage.getItem('id'));
        followId = parseInt(followId);
        const response = await $http.post(`api/Follow/removeFollow/${userId}/${followId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error unfollowing user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

// obserwatorzy
const getFollowers = async (userId) => {
    try {
        userId = parseInt(userId);
        const response = await $http.post(`api/Follow/getFollowers/${userId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error getting followers:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

// obserwowani
const getFollowing = async (userId) => {
    try {
        userId = parseInt(userId);
        const response = await $http.post(`api/Follow/getFollows/${userId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error getting following:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

export default {
    followUser,
    unfollowUser,
    getFollowers,
    getFollowing
}