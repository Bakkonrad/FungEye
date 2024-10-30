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

const getPosts = async (postsFilter, page) => {
    try {
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append("userId", userId);
        if (postsFilter) {
            formData.append("postsFilter", postsFilter);
        }
        if (page) {
            formData.append("page", page);
        }
        const response = await $http.get("api/Post/getPosts", formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const response = await $http.get(`api/Post/getPost/${postId}`);

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
        const response = await $http.post("api/Post/addPost", post);

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
        const response = await $http.put("api/Post/editPost", post);

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
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("postId", postId);
        const response = await $http.delete(`api/Post/deletePost`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("postId", postId);
        const response = await $http.post(`api/Post/addPostReaction`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("postId", postId);
        const response = await $http.post(`api/Post/deletePostReaction`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            }
        });

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
        const response = await $http.post(`api/Post/getLikes/${postId}`);

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
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append("userId", userId);
        formData.append("postId", postId);
        const response = await $http.post(`api/Post/getComments`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append("userId", userId);
        formData.append("commentJson", comment);
        const response = await $http.post("api/Post/addComment", formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append("userId", userId);
        formData.append("commentJson", comment);
        const response = await $http.put("api/Post/editComment", formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
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
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("commentId", commentId);
        const response = await $http.delete(`api/Post/deleteComment`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });

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
        const response = await $http.post("api/Report/reportUser", user);

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
    getPost,
    addPost,
    editPost,
    deletePost,
    likePost,
    unlikePost,
    getLikes,
    getComments,
    addComment,
    editComment,
    deleteComment,
    reportUser
};