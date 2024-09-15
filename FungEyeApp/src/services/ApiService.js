import { jwtDecode } from 'jwt-decode';
import AuthService from './AuthService';

const handleApiError = (error) => {
    if (error.response) {
        console.log('Error response:', error.response);
        switch (error.response.status) {
            case 400:
                return 'Błąd: Nieprawidłowe dane.';
            case 401:
                return 'Błąd: Nieautoryzowany dostęp.';
            case 403:
                return 'Błąd: Brak dostępu.';
            case 404:
                return 'Błąd: Nie znaleziono zasobu.';
            case 409:
                return 'Błąd: Użytkownik zbanowany do dnia ' + error.response.data;
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

const validateToken = async () => {
    try {
        const token = localStorage.getItem('token');
        if (!token) {
            return { success: false, message: 'Brak tokenu.' };
        }
        const decodedToken = jwtDecode(token);
        const expirationTime = decodedToken.exp * 1000;
        const currentTime = Date.now();
        if (currentTime >= expirationTime) {
            AuthService.logout();
            return { success: false, message: 'Sesja wygasła, zaloguj się ponownie.' };
        }
        // console.log('Token is valid');
        return { success: true };
    } catch (error) {
        console.error('Error validating token:', error);
        return { success: false, message: 'Błąd weryfikacji tokenu.' };
    }
}

export default {
    handleApiError,
    validateToken
}