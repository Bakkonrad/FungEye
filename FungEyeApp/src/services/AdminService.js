import axios from 'axios';

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
        const response = await $http.post('api/User/getUsers', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        if (response.status === 200) {
            return {success: true, data: response.data};
        }
        return {success: false, message: 'Nieznany błąd'};
    } catch (error) {
        const errorMessage = handleApiError(error);
        return {success: false, message: errorMessage};
    }
}

const deleteUser = async (id) => {
    try {
        const response = await $http.delete('api/Admin/deleteUser/' + id);
        if (response.status === 200) {
            return true;
        }
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return false;
    }
}

const updateUser = async (user) => {
    try {
        const response = await $http.put('api/Admin/updateUser', user);
        if (response.status === 200) {
            return true;
        }
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return false;
    }
}

const getUser = async (id) => {
    try {
        const response = await $http.get('api/Admin/getUser/' + id);
        if (response.status === 200) {
            return response.data;
        }
        return [];
    } catch (error) {
        console.error('Error loading data:', error);
        return [];
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
    getAllUsers,
    deleteUser,
    updateUser,
    getUser
}