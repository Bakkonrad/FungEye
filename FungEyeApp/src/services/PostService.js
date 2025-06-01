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
        const response = await $http.post("Post/getPosts", formData, {
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
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append("userId", userId);
        formData.append("postId", postId);
        const response = await $http.post("Post/getPost", formData, {
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
        console.error("Error getting post:", errorMessage);
        return { success: false, message: errorMessage, status: error.response.status };
    }
}

const addPost = async (post, image) => {
    try {
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("postJson", JSON.stringify(post));
        if (image) {
            formData.append("image", image);
        }
        const response = await $http.post("Post/addPost", formData, {
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
        console.error("Error adding post:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const editPost = async (post, image) => {
    try {
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("postJson", JSON.stringify(post));
        if (image) {
            formData.append("image", image);
        }
        const response = await $http.put("Post/editPost", formData, {
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
        const response = await $http.post(`Post/deletePost`, formData, {
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
        const response = await $http.post(`Post/addPostReaction`, formData, {
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
        const response = await $http.post(`Post/deletePostReaction`, formData, {
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
        const response = await $http.post(`Post/getLikes/${postId}`);

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
        const response = await $http.post(`Post/getComments`, formData, {
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
        formData.append("commentJson", JSON.stringify(comment));
        const response = await $http.post("Post/addComment", formData, {
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
        formData.append("commentJson", JSON.stringify(comment));
        const response = await $http.put("Post/editComment", formData, {
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
        const response = await $http.post(`Post/deleteComment`, formData, {
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

const report = async (postId, commentId) => {
    try {
        const formData = new FormData();
        const reporterId = parseInt(localStorage.getItem("id"));
        formData.append("reporterId", reporterId);
        if (postId) {
            formData.append("postId", postId);
        }
        if (commentId) {
            formData.append("commentId", commentId);
        }
        const response = await $http.post("Post/report", formData, {
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
        console.error("Error reporting user:", errorMessage);
        return { success: false, message: errorMessage };
    }
}

const getReports = async () => {
    try {
        const response = await $http.get("Post/getReports");
        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        console.error("Error getting reports:", error);
        return { success: false, message: "Nieznany błąd" };
    }
}

const deleteReport = async (reportId) => {
    try {
        const formData = new FormData();
        const userId = parseInt(localStorage.getItem("id"));
        formData.append("userId", userId);
        formData.append("reportId", reportId);
        const response = await $http.post("Post/markReportAsCompleted", formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
        if (response.status === 200) {
            return { success: true, data: response.data };
        }
        return { success: false, message: "Nieznany błąd" };
    } catch (error) {
        console.error("Error deleting report:", error);
        return { success: false, message: "Nieznany błąd" };
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
    report,
    getReports,
    deleteReport,
};