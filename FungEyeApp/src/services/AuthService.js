import { ref } from 'vue';
import axios from 'axios';
import { jwtDecode } from "jwt-decode";
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
// register, register admin, login, logout
const login = async (user) => {
    try {
        // console.log(user);
        const response = await $http.post('api/Auth/loginUser', user);
        if (response.status === 200) {
            localStorage.setItem('token', response.data);
            isLoggedIn.value = true;
            // console.log(isLoggedIn.value);
            // console.log(response.data);
            setUser();
            $http.defaults.headers.common['Authorization'] = `Bearer ${response.data}`;
            // alert('Zalogowano!');
            return { success: true };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        return { success: false, message: errorMessage };
    }
};

const register = async (admin, user) => {
    try {
        console.log(admin);
        // const response = await $http.post('api/Auth/registerUser', user);
        const response = await $http.post(admin ? 'api/Auth/registerAdmin' : 'api/Auth/registerUser', user);
        
        if (response.status === 200) {
            if (admin) {
                alert('Rejestracja administratora przebiegła pomyślnie!');
            } else {
                alert('Rejestracja przebiegła pomyślnie! Teraz możesz się zalogować.');
            }
            return true;
        }
        return { message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        return { message: errorMessage };
    }
}

const logout = () => {
    localStorage.removeItem('token');
    // localStorage.removeItem('user');
    localStorage.removeItem('id');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
    localStorage.removeItem('profileImg');
    isLoggedIn.value = false;
    isAdmin.value = false;
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
    }
    else
        console.log('token not found');
}

const sendResetPasswordEmail = async (email) => {
    try {
        console.log(email);
        const response = await $http.post(`api/Auth/sendResetPasswordEmail`, {email: email});
        if (response.status === 200) {
            // alert('Sprawdź swoją skrzynkę mailową!');
            console.log('Sprawdź swoją skrzynkę mailową!');
            return { success: true };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        return { success: false, message: errorMessage };
    }
}

const resetPassword = async (token, password) => {
    try {
        var decodedToken = jwtDecode(token);
        // console.log(decodedToken);
        var userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        console.log(userId);
        const response = await $http.post(`api/Auth/resetPassword/${parseInt(userId)}`, {password: password}, {headers: {Authorization: `Bearer ${token}`}});
        if (response.status === 200) {
            alert('Hasło zostało zresetowane! Zaloguj się.');
            return { success: true };
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        return { success: false, message: errorMessage };
    }

}

export default {
    login,
    register,
    logout,
    setUser,
    sendResetPasswordEmail,
    resetPassword
}

export const isLoggedIn = ref(false);
export const isAdmin = ref(false);
export const profileImage = ref(localStorage.getItem("profileImg"));

export function checkAuth() {
    if (localStorage.getItem("token")) {
        isLoggedIn.value = true;
    } else {
        isLoggedIn.value = false;
    }
}

export function checkAdmin() {
    if (localStorage.getItem("token") && localStorage.getItem("role") == "Admin" ) { 
        isAdmin.value = true;
    } else {
        isAdmin.value = false;
    }
}

export function setProfileImage(img) {
    profileImage.value = img;
}
