import axios from "axios";
import ApiService from "./ApiService";

const apiUrl = import.meta.env.VITE_APP_API_URL;

const $http = axios.create({
    baseURL: apiUrl,
    headers: {
        "Content-type": "application/json",
    },
});

$http.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
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
      return {
        success: false,
        message: "Sesja wygasła, zaloguj się ponownie.",
      };
    }

    const userId = parseInt(localStorage.getItem("id"));
    followId = parseInt(followId);
    const response = await $http.post(
      `Follow/addFollow/${userId}/${followId}`
    );

    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error following user:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const unfollowUser = async (followId) => {
  try {
    const isTokenValid = await ApiService.validateToken();
    if (isTokenValid.success == false) {
      return {
        success: false,
        message: "Sesja wygasła, zaloguj się ponownie.",
      };
    }

    const userId = parseInt(localStorage.getItem("id"));
    followId = parseInt(followId);
    const response = await $http.delete(
      `Follow/removeFollow/${userId}/${followId}`
    );

    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error unfollowing user:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

// obserwatorzy
const getFollowers = async (userId) => {
  try {
    const isTokenValid = await ApiService.validateToken();
    if (isTokenValid.success == false) {
      return {
        success: false,
        message: "Sesja wygasła, zaloguj się ponownie.",
      };
    }

    userId = parseInt(userId);
    const response = await $http.get(`Follow/getFollowers/${userId}`);

    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting followers:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

// obserwowani
const getFollowing = async (userId) => {
  try {
    const isTokenValid = await ApiService.validateToken();
    if (isTokenValid.success == false) {
      return {
        success: false,
        message: "Sesja wygasła, zaloguj się ponownie.",
      };
    }

    userId = parseInt(userId);
    const response = await $http.get(`Follow/getFollows/${userId}`);

    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting following:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const isFollowing = async (userId, followId) => {
  try {
    const isTokenValid = await ApiService.validateToken();
    if (isTokenValid.success == false) {
      return {
        success: false,
        message: "Sesja wygasła, zaloguj się ponownie.",
      };
    }
    
    userId = parseInt(userId);
    followId = parseInt(followId);
    const response = await $http.get(
      `Follow/isFollowing/${userId}/${followId}`
    );

    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error checking if following user:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

export default {
  followUser,
  unfollowUser,
  getFollowers,
  getFollowing,
  isFollowing,
};
