import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import { isLoggedIn, isAdmin, checkAdmin } from './AuthService';

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

const login = async (user) => {
    try {
        const response = await $http.post('api/Auth/loginUser', user);
        if (response.status === 200) {
            localStorage.setItem('token', response.data);
            isLoggedIn.value = true;
            // console.log(isLoggedIn.value);
            console.log(response.data);
            setUser();
            $http.defaults.headers.common['Authorization'] = `Bearer ${response.data}`;
            alert('Zalogowano!');
            return { success: true };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = handleApiError(error);
        return { success: false, message: errorMessage };
    }
};

const register = async (user) => {
    try {
        console.log(user);
        const response = await $http.post('api/Auth/registerUser', user);
        if (response.status === 200) {
            alert('Rejestracja przebiegła pomyślnie! Teraz możesz się zalogować.');
            return true;
        }
        return {message: 'Nieznany błąd'};
    } catch (error) {
        const errorMessage = handleApiError(error);
        return {message: errorMessage};
    }
}

const logout = async () => {
    localStorage.removeItem('token');
    // localStorage.removeItem('user');
    localStorage.removeItem('id');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
    isLoggedIn.value = false;
    isAdmin.value = false;
    alert('Wylogowano!');
    return true;
}

const setUser = () => {
    var token = localStorage.getItem('token');
    if (token) {
        var decodedToken = jwtDecode(token);
        // console.log(decodedToken);
        var userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        var username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        var role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        localStorage.setItem('id', userId);
        localStorage.setItem('username', username);
        localStorage.setItem('role', role);
        checkAdmin();
        // console.log(localStorage.getItem('token'));
        // console.log(localStorage.getItem('id') + ' ' + localStorage.getItem('role'));
        // console.log(localStorage.getItem('user') + ' ' + localStorage.getItem('username'));
    }
    else
        console.log('token not found');
}

const getUserData = async () => {
    try {
        const userId = localStorage.getItem('id');
        const response = await $http.post(`api/User/getProfile/${userId}`);

        if (response.status === 200) {
            return {success: true, data: response.data};
        } 
        return {success: false, message: 'Nieznany błąd'};
    } catch (error) {
        const errorMessage = handleApiError(error);
        console.error('Error loading data:', errorMessage);
        return {success: false, message: errorMessage};
    }
}

const handleApiError = (error) => {
    if (error.response) {
        switch (error.response.status) {
            case 400:
                return 'Błąd: Nieprawidłowe dane.';
            case 401:
                return 'Błąd: Nieautoryzowany dostęp.';
            case 403:
                return 'Błąd: Brak dostępu.';
            case 404:
                return 'Błąd: Nie znaleziono zasobu.';
            case 500:
                return 'Błąd: Wewnętrzny błąd serwera.';
            default:
                return `Błąd: ${error.response.statusText}`;
        }
    } else if (error.request) {
        // Request was made but no response received
        return 'Błąd: Brak odpowiedzi z serwera.';
    } else {
        // Something happened in setting up the request
        return `Błąd: ${error.message}`;
    }
};

export default {
    login,
    register,
    logout,
    getUserData
};


