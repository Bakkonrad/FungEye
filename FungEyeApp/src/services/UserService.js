import axios from 'axios';
import ApiService from './ApiService';
import { parse } from '@fortawesome/fontawesome-svg-core';

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

const getUserData = async () => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const userId = localStorage.getItem('id');
        const response = await $http.post(`api/User/getProfile/${userId}`);

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
        // console.log("get users: ", page, search);
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

        const response = await $http.post(`/api/User/removeAccount/${userId}`);
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

        // console.log("user: ", user);

        const formData = new FormData();
        formData.append('user', JSON.stringify(user));
        if (image) {
            formData.append('image', image);
        }

        // console.log("formData: ", formData.getAll('image'));

        const response = await $http.put('/api/User/updateUser', formData, {
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

// const updateImage = async (image) => {
//     try {
//         const isTokenValid = await validateToken();
//         if (isTokenValid.success == false) {
//             return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
//         }

//         const userId = localStorage.getItem('id');

//         const formData = new FormData();
//         formData.append('image', image);

//         const response = await $http.post(`/api/User/UpdateUserImage/${userId}`, formData, {
//             headers: {
//                 'Content-Type': 'multipart/form-data'
//             }
//         });
//         if (response.status === 200) {
//             return { success: true, data: response.data };
//         }
//         return { success: false, message: 'Nieznany błąd' };
//     } catch (error) {
//         const errorMessage = handleApiError(error);
//         console.error('Error updating image:', errorMessage);
//         return { success: false, message: errorMessage };
//     }
// }

const banUser = async (userId, ban) => {
    try {
        const isTokenValid = await ApiService.validateToken();
        if (isTokenValid.success == false) {
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        const banInt = parseInt(ban);
        const response = await $http.post(`/api/User/banUser/${userId}/${banInt}`);
        // const response = { status: 200 };
        if (response.status === 200) {
            return { success: true }
        }
        return { success: false, message: 'Nieznany błąd' };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error('Error banning user:', errorMessage);
        return { success: false, message: errorMessage };
    }
}

export default {
    getUserData,
    getAllUsers,
    // updateImage,
    updateUser,
    deleteAccount,
    banUser
};


