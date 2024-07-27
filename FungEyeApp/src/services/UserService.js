import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import { isLoggedIn } from './AuthService';

const $http = axios.create({
    baseURL: "http://localhost:5268/api",
    headers: {
        "Content-type": "application/json",
        "Authorization": "Bearer " + localStorage.getItem('token')
    }
});

const login = async (user) => {
    try {
        const response = await $http.post('/Auth/loginUser', user);
        if (response.status === 200) {
            localStorage.setItem('token', response.data);
            isLoggedIn.value = true;
            console.log(isLoggedIn.value);
            console.log(response.data);
            setUser();
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
        const response = await $http.post('/Auth/registerUser', user);
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
    localStorage.removeItem('user');
    localStorage.removeItem('id');
    localStorage.removeItem('username');
    isLoggedIn.value = false;
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
        localStorage.setItem('id', userId);
        localStorage.setItem('username', username);
        // console.log(localStorage.getItem('user') + ' ' + localStorage.getItem('username'));
    }
    else
        console.log('token not found');
}

const getUserData = async () => {
    if (localStorage.getItem('id') != null) {
        const response = {
            id: 0,
            role: 1,
            username: "string",
            email: "string",
            password: "string",
            firstName: "string",
            lastName: "string",
            imageUrl: "string",
            createdAt: "2024-07-27T17:41:35.881Z",
            dateOfBirth: "2024-07-27T17:41:35.881Z"
        };
        return response;
    }
    // await axios.get('Auth/User/' + localStorage.getItem('id'))
        // .then(response => {

            // return response;
        // })
        // .catch(error => {
            // console.error('Error loading data:', error);
        // });
}



export default {
    login,
    register,
    logout,
    getUserData
};


