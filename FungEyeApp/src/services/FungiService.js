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

const predict = async (image) => {
  try {
    const formData = new FormData();
    formData.append("image", image);
    const predictResponse = await $http.post("Model/predict", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    // const predictResponse = { status: 200, data: [{ "Item1": "Boletus_edulis", "Item2": 0.864475548 }, { "Item1": "Imleria_badia", "Item2": 0.0371585973 }, { "Item1": "Boletus_pinophilus", "Item2": 0.0329410769 }, { "Item1": "Boletus_reticulatus", "Item2": 0.0282462705 }, { "Item1": "Suillus_variegatus", "Item2": 0.0259523895 }] };
    if (predictResponse.status === 200) {
      let fungiData = [];
      const fungiResults = await Promise.all(predictResponse.data.map(async (result) => {
        const latinName = formatName(result.Item1);
        const probability = parseFloat((result.Item2 * 100).toFixed(0));
        const getFungiByNameResponse = await getFungiByName(latinName);
        if (getFungiByNameResponse.success === true) {
          const id = getFungiByNameResponse.data.id;
          const polishName = getFungiByNameResponse.data.polishName;
          const image = getFungiByNameResponse.data.imagesUrl[0];
          const fungi = {
            id: id,
            polishName: polishName,
            latinName: latinName,
            image: image,
            probability: probability,
          };
          fungiData.push(fungi);
          return fungi;
        }
        return null;
      }));
      if (fungiResults.length === fungiData.length) {
        fungiData = fungiData.sort((a, b) => b.probability - a.probability);
        return { success: true, data: fungiData };
      }
      return { success: false, message: "Nieznany błąd" };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error predicting:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const formatName = (name) => {
  const formattedName = name.split("_")[0] + " " + name.split("_")[1];
  return formattedName;
};

const getAllFungies = async (page, search, filters) => {
  try {
    let userId = null;
    if (localStorage.getItem("id")) {
      userId = parseInt(localStorage.getItem("id"));
    }
    const data = {
      "userId": userId,
      "search": search,
      "page": page,
      pageSize: null,
      edibility: filters ? filters.edibility : null,
      toxicity: filters ? filters.toxicity : null,
      habitat: filters ? filters.habitat : null,
      letter: filters ? filters.letter : null,
      savedByUser: filters ? filters.savedByUser : null,
    }
    const formData = new FormData();
    for (const key in data) {
      if (data[key] !== null && data[key] !== undefined) {
        formData.append(key, data[key]);
      }
    }
    const response = await $http.post("FungiAtlas/getFungies", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    if (response.status === 200) {
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

const getFilteredFungies = async (
  edibility,
  toxicity,
  habitat,
  letter,
  savedByUser
) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    if (edibility) {
      formData.append("edibility", edibility);
    }
    if (toxicity) {
      formData.append("toxicity", toxicity);
    }
    if (habitat) {
      formData.append("habitat", habitat);
    }
    if (letter) {
      formData.append("letter", letter);
    }
    if (savedByUser) {
      formData.append("savedByUser", savedByUser);
    }
    const response = await $http.post(
      "FungiAtlas/getFilteredFungies",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );
    if (response.status === 200) {
      const data = addAttributes(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting fungis by attributes:", errorMessage);
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
    } else if (fungi.toxicity === "trujący") {
      newFungi.attributes.push("trujący");
    }
    if (fungi.edibility === "niejadalny") {
      newFungi.attributes.push("niejadalny");
    }
    if (fungi.habitat === "iglasty") {
      newFungi.attributes.push("iglaste");
    } else if (fungi.habitat === "liściasty") {
      newFungi.attributes.push("liściaste");
    } else if (fungi.habitat === "mieszany") {
      newFungi.attributes.push("mieszane");
    }
    return newFungi;
  });
  return newFungies;
};

const addAttributesToSingle = (fungi) => {
  const newFungi = {
    ...fungi,
    attributes: [],
  };
  if (fungi.edibility === "jadalny") {
    newFungi.attributes.push("jadalny");
  } else if (fungi.toxicity === "trujący") {
    newFungi.attributes.push("trujący");
  }
  if (fungi.edibility === "niejadalny") {
    newFungi.attributes.push("niejadalny");
  }
  if (fungi.habitat === "iglasty") {
    newFungi.attributes.push("iglaste");
  } else if (fungi.habitat === "liściasty") {
    newFungi.attributes.push("liściaste");
  } else if (fungi.habitat === "mieszany") {
    newFungi.attributes.push("mieszane");
  }
  return newFungi;
};

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
};

const getFungi = async (id) => {
  try {
    const fungiId = parseInt(id);
    const userId = JSON.stringify(parseInt(localStorage.getItem("id")));
    const response = await $http.post(
      `FungiAtlas/getFungi/${fungiId}`,
      userId
    );
    if (response.status === 200) {
      const data = addAttributesToSingle(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting fungi by id:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const getFungiByName = async (fungiName) => {
  try {
    let userId = null;
    const data = new FormData();
    if (localStorage.getItem("id")) {
      userId = parseInt(localStorage.getItem("id"));
      data.append("userId", userId);
    }
    data.append("fungiName", fungiName);
    const response = await $http.post(`FungiAtlas/getFungiByName`, data, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    if (response.status === 200) {
      const data = addAttributesToSingle(response.data);
      return { success: true, data: data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error getting fungi by name:", errorMessage);
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
    if (images.length > 0) {
      for (let i = 0; i < images.length; i++) {
        formData.append("images", images[i]);
      }
    }
    const response = await $http.post("FungiAtlas/addFungi", formData, {
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
    const fungiJson = JSON.stringify(reconvertAttributes(fungi));
    formData.append("fungiJson", fungiJson);
    if (images.length > 0) {
      for (let i = 0; i < images.length; i++) {
        formData.append("images", images[i]);
      }
    }
    const response = await $http.post(`FungiAtlas/editFungi`, formData, {
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
    const response = await $http.post(`FungiAtlas/deleteFungi/`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    if (response.status === 200 || response.status === 201) {
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
    const response = await $http.post(
      "FungiAtlas/addFungiToCollection",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );
    if (response.status === 201 || response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error saving fungi to collection:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

const deleteFungiFromCollection = async (fungiId) => {
  try {
    const userId = parseInt(localStorage.getItem("id"));
    const formData = new FormData();
    formData.append("userId", userId);
    formData.append("fungiId", fungiId);
    const response = await $http.post(
      "FungiAtlas/deleteFungiFromCollection",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );
    if (response.status === 201 || response.status === 200) {
      return { success: true, data: response.data };
    }
    return { success: false, message: "Nieznany błąd" };
  } catch (error) {
    const errorMessage = ApiService.handleApiError(error);
    console.error("Error saving fungi to collection:", errorMessage);
    return { success: false, message: errorMessage };
  }
};

export default {
  predict,
  getAllFungies,
  getFilteredFungies,
  getFungi,
  getFungiByName,
  addFungi,
  editFungi,
  deleteFungi,
  saveFungiToCollection,
  deleteFungiFromCollection,
};
