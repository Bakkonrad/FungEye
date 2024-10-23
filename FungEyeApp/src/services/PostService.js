import axios from "axios";
import ApiService from "./ApiService";

const $http = axios.create({
    baseURL: "http://localhost:5268/",
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

const getPosts = async () => {
    try {
        const response = await $http.get("api/Post/getPosts"); //get

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error getting posts:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getPost = async (postId) => {
    try {
        const response = await $http.get(`api/Post/getPost/${postId}`); //get

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error getting post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const addPost = async (post) => {
    try {
        const response = await $http.post("api/Post/addPost", post); //post

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error adding post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const editPost = async (post) => {
    try {
        const response = await $http.put("api/Post/editPost", post); //put

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error editing post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const deletePost = async (postId) => {
    try {
        const response = await $http.delete(`api/Post/deletePost/${postId}`); //delete

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error deleting post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const likePost = async (postId) => {
    try {
        const response = await $http.post(`api/Post/likePost/${postId}`); //post

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error liking post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const unlikePost = async (postId) => {
    try {
        const response = await $http.post(`api/Post/unlikePost/${postId}`); //post

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error unliking post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getLikes = async (postId) => {
    try {
        const response = await $http.post(`api/Post/getLikes/${postId}`); //get

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error getting likes:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getComments = async (postId) => {
    try {
        const response = await $http.post(`api/Post/getComments/${postId}`); //get

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error getting comments:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const addComment = async (comment) => {
    try {
        const response = await $http.post("api/Post/addComment", comment); //post

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error adding comment:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const editComment = async (comment) => {
    try {
        const response = await $http.put("api/Post/editComment", comment); //put

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error editing comment:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const deleteComment = async (commentId) => {
    try {
        const response = await $http.delete(`api/Post/deleteComment/${commentId}`); //delete

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error deleting comment:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const reportUser = async (user) => {
    try {
        const response = await $http.post("api/Report/reportUser", user); //post

        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        const errorMessage = ApiService.handleApiError(error);
        console.error("Error reporting user:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

export default {
    getPosts, 
    getPost, // postId w ścieżce
    addPost, // post w ciele
    editPost, // post w ciele
    deletePost, // postId w ścieżce
    likePost, // postId w ścieżce
    unlikePost, // postId w ścieżce
    getLikes, // postId w ścieżce
    getComments, // postId w ścieżce
    addComment, // comment w ciele
    editComment, // comment w ciele
    deleteComment, // commentId w ścieżce
    reportUser // user w ciele
};