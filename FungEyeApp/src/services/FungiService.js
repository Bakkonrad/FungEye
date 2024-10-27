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

const predict = async (image) => {
  try {
    const formData = new FormData();
    formData.append("image", image);
    const response = await $http.get("api/Model/predict", formData, {
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
    console.error("Error predicting:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const getAllFungis = async (page, search) => {
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
        const response = await $http.post('api/Fungi/getFungis', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting all fungis:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const getFungiById = async (id) => {
  try {
    const response = await $http.get(`api/Fungi/${id}`);
    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting fungi by id:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const addFungi = async (fungi) => {
  try {
    const response = await $http.post("api/Fungi", fungi);
    if (response.status === 201) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error adding fungi:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const updateFungi = async (id, fungi) => { 
  try {
    const response = await $http.put(`api/Fungi/${id}`, fungi);
    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error updating fungi:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const deleteFungi = async (id) => {
  try {
    const response = await $http.delete(`api/Fungi/${id}`);
    if (response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error deleting fungi:", errorMessage);
    return { success: false, message: errorMessage };
  }
};


export default { predict,
  getAllFungis, // userId, page, search w formData
  getFungiById, // id w url
  addFungi, // userId, fungi w body
  updateFungi, // userId, id w url, fungi w body
  deleteFungi, // userId, id w url
 };