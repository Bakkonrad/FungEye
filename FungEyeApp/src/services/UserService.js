import axios from 'axios';
import {jwtDecode} from "jwt-decode";

const $http = axios.create({
    baseURL: "http://localhost:5268/api",
    headers: {
        "Content-type": "application/json"
    }
});

const login = async (user) => {
    try {
        const response = await $http.post('/Auth/loginUser', user);
        if(response.status === 200)
        {
            localStorage.setItem('token', response.data);
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
        if(response.status === 200){
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
    localStorage.removeItem('username');
    alert('Wylogowano!');
}

const setUser = () => {
    var token = localStorage.getItem('token');
    if (token) {
        var decodedToken = jwtDecode(token);
        // console.log(decodedToken);
        var nameidentifier = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        var username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        localStorage.setItem('user', nameidentifier);
        localStorage.setItem('username', username);
        // console.log(localStorage.getItem('user') + ' ' + localStorage.getItem('username'));
    }
    else
    console.log('token not found');
}



export default {
    login,
    register,
    logout
};


