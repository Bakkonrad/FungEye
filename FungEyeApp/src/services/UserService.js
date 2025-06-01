import axios from 'axios';
import ApiService from './ApiService';

const apiUrl = import.meta.env.VITE_APP_API_URL;

const $http = axios.create({
    baseURL: apiUrl,
    headers: {
        "Content-type": "application/json",
    },
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

const getUserData = async (userId) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const response = await $http.get(`User/getProfile/${userId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error loading data:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getSmallUserData = async (userId) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const response = await $http.get(`User/getSmallProfile/${userId}`);

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error loading data:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getAllUsers = async (page, search) => {
    try {
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append('userId', userId);
        if (page) {
            formData.append('page', page);
        }
        if (search) {
            formData.append('search', search);
        }
        const response = await $http.post('User/getUsers', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        return { success: false, message: errorMessage };
    }
}

const deleteAccount = async (userId) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const response = await $http.delete(`/User/removeAccount/${userId}`);
        if (response.status === 200) {
            return { success: true }
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = handleApiError(error);
        console.error('Error deleting account:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const updateUser = async (user, image) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }

        const token = localStorage.getItem('token');

        const formData = new FormData();
        formData.append('userJson', JSON.stringify(user));
        if (image) {
            formData.append('image', image);
        }

        const response = await $http.put('/User/updateUser', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'Authorization': `Bearer ${token}`
            }
        });
        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error updating user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const banUser = async (userId, ban) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const banInt = parseInt(ban);
        const response = await $http.post(`/User/banUser/${userId}/${banInt}`);
        if (response.status === 200) {
            alert('Użytkownik zbanowany!');
            return { success: true, message: 'Użytkownik zbanowany', data: response.data };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error banning user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const retrieveAccount = async (userId) => {
    try {
        const response = await $http.get(`/User/retrieveAccount/${userId}`);
        if (response.status === 200) {
            return { success: true, message: 'Konto odzyskane' };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error retrieving account:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const followUser = async (follow) => {
    try {
        const userId = parseInt(localStorage.getItem('id'));
        const response = await $http.post(`/User/addFollow/${userId}/${follow}`);
        if (response.status === 200) {
            return { success: true, message: 'Użytkownik obserwowany' };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error following user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

const unfollowUser = async (follow) => {
    try {
        const userId = parseInt(localStorage.getItem('id'));
        const response = await $http.delete(`/User/removeFollow/${userId}/${follow}`);
        if (response.status === 200) {
            return { success: true, message: 'Użytkownik przestał być obserwowany' };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error following user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

export default {
    getUserData,
    getSmallUserData,
    getAllUsers,
    updateUser,
    deleteAccount,
    banUser,
    retrieveAccount,
    followUser,
    unfollowUser
};


