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

const getAllFungies = async (page, search) => {
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
        const response = await $http.post('api/Fungi/getFungies', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
    if (response.status === 200) {
      const data = convertToAttributes(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting all fungis:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const convertToAttributes = (fungi) => {
  const newFungi = {
    ...fungi,
    attributes: [],
  }
  if (fungi.edibility === "jadalny") {
    newFungi.attributes.push("jadalny");
  } else if (fungi.edibility === "niejadalny") {
    newFungi.attributes.push("niejadalny");
  }
  if (fungi.toxicity === "trujący") {
    newFungi.attributes.push("trujący");
  }
  if (fungi.habitat === "iglasty") {
    newFungi.attributes.push("iglaste");
  } else if (fungi.habitat === "liściasty") {
    newFungi.attributes.push("liściaste");
  }
  else if (fungi.habitat === "mieszany") {
    newFungi.attributes.push("mieszane");
  }
  return newFungi;
}

const getFungi = async (id) => {
  try {
    const response = await $http.get(`api/Fungi/getFungi/${id}`);
    if (response.status === 200) {
      const data = convertToAttributes(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting fungi by id:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const addFungi = async (fungi, images) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiJson", JSON.stringify(fungi));
    // for (let i = 0; i < images.length; i++) {
    //   formData.append("images", images[i]);
    // }
    formData.append("images", images);
    const response = await $http.post("api/Fungi/addFungi", formData, { 
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
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

const editFungi = async (fungi, images) => { 
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiJson", JSON.stringify(fungi));
    // for (let i = 0; i < images.length; i++) {
    //   formData.append("images", images[i]);
    // }
    formData.append("images", images);
    const response = await $http.post(`api/Fungi/editFungi`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      }
    }
    );
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
    const response = await $http.delete(`api/Fungi/deleteFungi/${id}`);
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

const saveFungiToCollection = async (fungiId) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiId", fungiId);
    const response = await $http.post("api/Fungi/saveFungiToCollection", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    if (response.status === 201) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error saving fungi to collection:", errorMessage);
    return { success: false, message: errorMessage };
  }
}

const deleteFungiFromCollecion = async (fungiId) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiId", fungiId);
    const response = await $http.delete("api/Fungi/deleteFungiFromCollection", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    if (response.status === 201) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error saving fungi to collection:", errorMessage);
    return { success: false, message: errorMessage };
  }
}


export default { 
  predict,
  getAllFungies,
  getFungi,
  addFungi,
  editFungi,
  deleteFungi,
  saveFungiToCollection,
  deleteFungiFromCollecion
};