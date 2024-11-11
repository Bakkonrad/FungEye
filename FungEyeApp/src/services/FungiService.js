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
    const response = await $http.post('api/FungiAtlas/getFungies', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    // const response = {
    //   status: 200, data: [{
    //     id: 1,
    //     name: 'Borowik szlachetny',
    //     image: 'src/assets/images/mushrooms/ATLAS-borowik.jpg',
    //     edibility: 'jadalny',
    //     toxicity: 'nietrujący',
    //     habitat: 'iglasty',
    //     description: 'Borowik szlachetny to jeden z najbardziej cenionych grzybów jadalnych.'
    //   }]
    // };
    if (response.status === 200) {
      // console.log("fungies: ", response.data);
      const data = addAttributes(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting all fungis:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const addAttributes = (fungies) => {
  const newFungies = fungies.map((fungi) => {
    const newFungi = {
      ...fungi,
      attributes: [],
    };
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
  });
  return newFungies;
}

const reconvertAttributes = (fungi) => {
  const newFungi = {
    ...fungi,
    edibility: "",
    toxicity: "",
    habitat: "",
  };
  if (fungi.attributes.includes("jadalny")) {
    newFungi.edibility = "jadalny";
  } else if (fungi.attributes.includes("niejadalny")) {
    newFungi.edibility = "niejadalny";
  }
  if (fungi.attributes.includes("trujący")) {
    newFungi.toxicity = "trujący";
  } else if (fungi.attributes.includes("nietrujący")) {
    newFungi.toxicity = "nietrujący";
  }
  if (fungi.attributes.includes("iglaste")) {
    newFungi.habitat = "iglasty";
  } else if (fungi.attributes.includes("liściaste")) {
    newFungi.habitat = "liściasty";
  } else if (fungi.attributes.includes("mieszane")) {
    newFungi.habitat = "mieszany";
  }
  return newFungi;
}

const getFungi = async (id) => {
  try {
    const response = await $http.get(`api/Fungi/getFungi/${id}`);
    if (response.status === 200) {
      const data = addAttributes(response.data);
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
    const fungiJson = JSON.stringify(reconvertAttributes(fungi));
    formData.append("fungiJson", fungiJson);
    // formData.append("images", images);
    console.log(images);
    console.log(images.length);
    if (images.length > 0) {
      for (let i = 0; i < images.length; i++) {
        formData.append("images", images[i]);
      }
    }
    console.log("addFungi: ", formData.get("fungiJson"));
    console.log(formData.get("images"));
    const response = await $http.post("api/FungiAtlas/addFungi", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    // const response = {status: 201, data: fungi};
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
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiId", id);
    console.log("deleteFungi: ", formData.get("userId"), formData.get("fungiId"));
    const response = await $http.post(`api/FungiAtlas/deleteFungi/`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    console.log(response)
    // const response = { status: 200, data: { id: id } };
    if (response.status === 200) {
      return { success: true };
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
    const response = await $http.post("api/FungiAtlas/addFungiToCollection", formData, {
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

const deleteFungiFromCollection = async (fungiId) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiId", fungiId);
    const response = await $http.delete("api/FungiAtlas/deleteFungiFromCollection", formData, {
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
  deleteFungiFromCollection
};