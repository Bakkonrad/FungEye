<template>
  <div>
    <div v-if="errorLoadingData || isLoading" class="error-div container-md">
      <h2>Profil użytkownika</h2>
      <div v-if="errorLoadingData" class="error-div">
        <p class="error-loading-data">{{ errorMessage }}</p>
        <router-link v-if="!isLoggedIn" to="/log-in" class="btn fungeye-red-button" @click="logOut">Przejdź do
          logowania</router-link>
        <router-link v-else to="/log-in" class="btn fungeye-red-button" @click="logOut">Zaloguj się
          ponownie</router-link>
      </div>
      <div v-if="isLoading" class="container-md data-loading">
        <h3 class="data-loading">Trwa ładowanie danych. Proszę czekać</h3>
        <LoadingSpinner></LoadingSpinner>
      </div>
    </div>
    <div v-else>
      <div v-if="!isEditing" class="container-md">
        <div id="user-info">
          <UserProfileInfo :imgSrc="imgSrc" :username="username" :name_surname="name_surname" :createdAt="createdAt"/>
          <div class="buttons">
            <button @click="startEditing" type="button" class="btn fungeye-default-button">
              <font-awesome-icon icon="fa-solid fa-gear" class="button-icon" />
              Ustawienia
            </button>
            <button @click="logOut" type="button" class="btn fungeye-red-button">
              <font-awesome-icon icon="fa-solid fa-right-from-bracket" class="button-icon" />
              Wyloguj się
            </button>
          </div>
        </div>
        <UserProfileCollections :mushrooms="mushrooms" :follows="follows" />
      </div>
    </div>
    <div class="settings container-md" v-if="isEditing">
      <div class="main-header">
        <h2>Ustawienia</h2>
        <button @click="cancelEditing" type="button" class="btn fungeye-default-button">
          <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon" />
          Powrót do mojego profilu
        </button>
      </div>
      <div class="settings-content">
        <EditUser :user="user" @cancel-edit="cancelEditing" @save-user="saveUser" />
        <div class="settings-content-right">
          <div class="edit-container">
            <div class="edit-form">
              <h3>Usuwanie konta</h3>
              <button @click="deleteAccount" type="button" class="btn fungeye-red-button">
                <font-awesome-icon icon="fa-solid fa-trash" class="button-icon" />
                Usuń konto
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";
import UserService from "@/services/UserService";
import AuthService from "@/services/AuthService";
import UserProfileCollections from "@/components/UserProfileCollections.vue";
import UserProfileInfo from "@/components/UserProfileInfo.vue";
import EditUser from "@/components/EditUser.vue";
import BaseInput from "@/components/BaseInput.vue";
import { onMounted, reactive, ref } from "vue";
import { isLoggedIn, profileImage, setProfileImage } from "@/services/AuthService";
import LoadingSpinner from "@/components/LoadingSpinner.vue";

export default {
  components: {
    ProfileImage,
    UserProfileCollections,
    UserProfileInfo,
    BaseInput,
    EditUser,
    LoadingSpinner,
  },
  data() {
    return {
      // imgSrc: "",
      // imgFile: null,
      // user: null,
      // username: "",
      // name_surname: "",
      // email: "",
      mushrooms: [
        "src/assets/images/mushrooms/ATLAS-borowik.jpg",
        "src/assets/images/mushrooms/ATLAS-muchomor.jpg",
        "src/assets/images/mushrooms/ATLAS-kurka.jpg",
        "src/assets/images/mushrooms/ATLAS-podgrzybek.jpg",
        "src/assets/images/mushrooms/ATLAS-borowik.jpg",
        "src/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
      ],
      // follows: [
      //   {
      //     id: 1,
      //     name: "Przyjaciel 1",
      //     img: "src/assets/images/profile-images/profile-img2.jpeg",
      //   },
      //   {
      //     id: 2,
      //     name: "Przyjaciel 2",
      //     img: "src/assets/images/profile-images/profile-img3.jpeg",
      //   },
      //   {
      //     id: 3,
      //     name: "Przyjaciel 3",
      //     img: "src/assets/images/profile-images/profile-img4.jpeg",
      //   },
      // ],
      // errorLoadingData: false,
      // errorMessage: "",
      // isEditing: false,
      // isLoading: true,
    };
  },
  methods: {
    logOut() {
      AuthService.logout();
      this.$router.push("/log-in");
    },
    async deleteAccount() {
      const confirmDelete = confirm(
        "Czy na pewno chcesz usunąć swoje konto?"
      );
      if (!confirmDelete) {
        return;
      }
      try {
        const response = await UserService.deleteAccount(parseInt(localStorage.getItem('id')));
        if (response.success) {
          // console.log("Account deleted");
          this.logOut();
        }
        else {
          console.error("Error deleting account: ", response.message);
        }
      }
      catch (error) {
        console.error("Error deleting account: ", error);
      }
    },
  },
  setup() {
    const imgSrc = reactive(profileImage);
    const imgFile = ref(null);
    const isEditing = ref(false);
    const isLoading = ref(false);
    const errorLoadingData = ref(false);
    const errorMessage = ref('');
    const user = ref(null);
    const username = ref('');
    const name_surname = ref('');
    const email = ref('');
    const createdAt = ref('');
    const follows = ref([
      // {
      //   id: 1,
      //   name: "Przyjaciel 1",
      //   img: "src/assets/images/profile-images/profile-img2.jpeg",
      // },
      // {
      //   id: 2,
      //   name: "Przyjaciel 2",
      //   img: "src/assets/images/profile-images/profile-img3.jpeg",
      // },
      // {
      //   id: 3,
      //   name: "Przyjaciel 3",
      //   img: "src/assets/images/profile-images/profile-img4.jpeg",
      // },
    ]);

    const fetchUser = async () => {
      try {
        isLoading.value = true;
        const userData = await UserService.getUserData(localStorage.getItem("id"));
        if (!userData.success) {
          errorLoadingData.value = true;
          errorMessage.value = userData.message;
          return;
        } else {
          if (userData.data.imageUrl && userData.data.imageUrl !== "string" && userData.data.imageUrl !== "placeholder") {
            imgSrc.value = userData.data.imageUrl;
            localStorage.setItem("profileImg", userData.data.imageUrl);
            setProfileImage(userData.data.imageUrl);
          }
          else { // placeholder image
            const placeholderImg = "src/assets/images/profile-images/placeholder.png";
            imgSrc.value = placeholderImg;
            localStorage.setItem("profileImg", placeholderImg);
            setProfileImage(placeholderImg);
          }
          user.value = userData.data;
          // console.log(userData.data);
          username.value = userData.data.username;
          createdAt.value = userData.data.createdAt;
          if (userData.data.firstName && userData.data.lastName) {
            name_surname.value = userData.data.firstName + " " + userData.data.lastName;
          }
          email.value = userData.data.email;
          // this.mushrooms = response.mushrooms;
          follows.value = userData.data.follows;
          console.log(follows.value);
        }
      } catch (error) {
        errorLoadingData.value = true;
        errorMessage.value = userData.message;
        this.$router.push("/log-in");
      } finally {
        isLoading.value = false;
      }
    };

    const startEditing = () => {
      isEditing.value = true;
    };
    const cancelEditing = () => {
      imgSrc.value = profileImage.value;
      isEditing.value = false;
    };
    const saveUser = async () => {
      cancelEditing();
      fetchUser();
    };

    onMounted(() => {
      fetchUser();
    });

    return {
      imgSrc,
      imgFile,
      user,
      username,
      name_surname,
      email,
      createdAt,
      follows,
      errorLoadingData,
      errorMessage,
      isEditing,
      isLoading,
      fetchUser,
      startEditing,
      cancelEditing,
      saveUser,
    };
  },
};
</script>

<style scoped>
.error-div {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.container-md {
  margin-top: 2em;
}

.data-loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #333;
}

.left-side {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 1.5em;
}

.buttons {
  display: flex;
  flex-direction: row;
  gap: 1em;
}

#user-info {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}


.settings {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1em;
}

.main-header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  width: 90%;
  margin-bottom: 1em;
}

.settings-content {
  display: flex;
  flex-direction: row;
  align-items: start;
  flex-wrap: wrap;
  gap: 1em;
}

.settings-content-right {
  display: flex;
  flex-direction: column;
  gap: 1em;
}

.fileInput {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1em;
  cursor: pointer;
}

h3 {
  margin-bottom: 1em;
  color: white;
}

.edit-container {
  display: flex;
  justify-content: center;
  align-items: center;
}

.edit-form {
  background-color: rgba(0, 0, 0, 0.85);
  padding: 40px;
  border-radius: 12px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  width: 500px;
}

@media screen and (max-width: 1200px) {
  .settings-content {
    flex-direction: column;
    justify-content: center !important;
    align-items: center !important;
    gap: 1em;
  }
}

@media screen and (max-width: 768px) {
  .container-md {
    margin-top: 1em;
    justify-content: center;
    padding: 0;
    width: 80vw;
  }

  #user-info {
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: 1em;
  }

  .buttons {
    flex-direction: column;
    gap: 1em;
  }

  .main-header {
    flex-direction: column;
    gap: 1em;
  }

  h3 {
    font-size: 1.5em;
  }
}

@media screen and (max-width: 500px) {
  .edit-form {
    width: 90vw;
  }
}
</style>
