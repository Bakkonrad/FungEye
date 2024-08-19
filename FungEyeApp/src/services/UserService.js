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
            return true;
        }
        else
            // opis błędu - metoda handleApiError i wyświetlenie użytkownikowi

            return false;
    } catch (error) {

        console.error('Error loading data:', error);
        return [];
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
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return [];
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
    // if (localStorage.getItem('id') != null) {
    //     const response = {
    //         id: 0,
    //         role: 1,
    //         username: "string",
    //         email: "string",
    //         password: "string",
    //         firstName: "string",
    //         lastName: "string",
    //         imageUrl: "string",
    //         createdAt: "2024-07-27T17:41:35.881Z",
    //         dateOfBirth: "2024-07-27T17:41:35.881Z"
    //     };
    //     return response;
    // }
    try {
        const userId = localStorage.getItem('id');
        const response = await $http.post(`api/User/getProfile/${userId}`);

        if (response.status === 200) {
            // console.log(response.data);
            return response.data;
        }
        else if (response.status === 404) {
            alert('Nie znaleziono użytkownika');
            return false;
        }
        else if (response.status === 401) {
            alert('Session expired');
            this.$router.push("/log-in");
            return false;
        }
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return [];
    }
}



export default {
    login,
    register,
    logout,
    getUserData
};


