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
    const response = await $http.post("api/Model/predict", formData, {
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


export default { predict };