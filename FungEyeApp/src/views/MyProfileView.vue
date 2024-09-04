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
          <UserProfileInfo :imgSrc="imgSrc" :username="username" :name_surname="name_surname" :email="email" />
          <div class="buttons">
            <button @click="startEditing" type="button" class="btn fungeye-default-button">
              Ustawienia
            </button>
            <button @click="logOut" type="button" class="btn fungeye-red-button">
              Wyloguj się
            </button>
          </div>
        </div>
        <UserProfileCollections :mushrooms="mushrooms" :friends="friends" />
      </div>
    </div>
    <div class="settings container-md" v-if="isEditing">
      <div class="main-header">
        <h2>Ustawienia</h2>
        <button @click="cancelEditing" type="button" class="btn fungeye-default-button">
          Powrót do mojego profilu
        </button>
      </div>
      <div class="settings-content">
        <EditUser :user="user" @cancel-edit="cancelEditing" @save-user="saveUser" />
        <div class="settings-content-right">
          <!-- <div class="edit-container">
            <div class="edit-form">
              <h3>Zmiana hasła konta</h3>
              <div class="mb-3">
                <BaseInput v-model="registerFormData.password" type="password" label="Hasło (min. 8 znaków)" :class="{
                  'password-input': !submitted,
                  validInput: submitted && !v$.password.$invalid,
                  invalidInput: submitted && v$.password.$invalid,
                  }" />
                  <span class="error-message" v-for="error in v$.password.$errors" :key="error.$uid">
                    {{ error.$message }}
                  </span>
                </div>
                <div class="mb-3">
                  <BaseInput v-model="registerFormData.confirmPassword" type="password" label="Potwierdź hasło" :class="{
                    'confirmPassword-input': !submitted,
                    validInput: submitted && !v$.confirmPassword.$invalid,
                    invalidInput: submitted && v$.confirmPassword.$invalid,
                    }" />
                    <span class="error-message" v-for="error in v$.confirmPassword.$errors" :key="error.$uid">
                      {{ error.$message }}
                    </span>
                  </div>
                  <div class="mb-3">
                    <BaseInput v-model="registerFormData.dateOfBirth" type="date" label="Data urodzenia" :class="{
                      'dateOfBirth-input': !submitted,
                      validInput: submitted && !v$.dateOfBirth.$invalid,
                      invalidInput: submitted && v$.dateOfBirth.$invalid,
                      }" />
                      <span class="error-message" v-for="error in v$.dateOfBirth.$errors" :key="error.$uid">
                        {{ error.$message }}
                      </span>
              </div>
            </div> 
          </div> -->
          <div class="edit-container">
            <div class="edit-form">
              <h3>Usuwanie konta</h3>
              <button @click="deleteAccount" type="button" class="btn fungeye-red-button">
                <font-awesome-icon icon="fa-solid fa-trash" class=""/>
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
      friends: [
        {
          name: "Przyjaciel 1",
          img: "src/assets/images/profile-images/profile-img2.jpeg",
        },
        {
          name: "Przyjaciel 2",
          img: "src/assets/images/profile-images/profile-img3.jpeg",
        },
        {
          name: "Przyjaciel 3",
          img: "src/assets/images/profile-images/profile-img4.jpeg",
        },
      ],
      // errorLoadingData: false,
      // errorMessage: "",
      // isEditing: false,
      // isLoading: true,
    };
  },
  methods: {
    logOut() {
      UserService.logout();
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
          console.log("Account deleted");
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

    const fetchUser = async () => {
      try {
        isLoading.value = true;
        const userData = await UserService.getUserData();
        if (!userData.success) {
          errorLoadingData.value = true;
          errorMessage.value = userData.message;
          return;
        } else {
          if (userData.data.imageUrl && userData.data.imageUrl !== "string") {
            imgSrc.value = userData.data.imageUrl;
            localStorage.setItem("profileImg", userData.data.imageUrl);
            setProfileImage(userData.data.imageUrl);
          }
          else { // placeholder image
            const placeholderImg = "https://avatar.iran.liara.run/public";
            imgSrc.value = placeholderImg;
            localStorage.setItem("profileImg", placeholderImg);
            setProfileImage(placeholderImg);
          }
          user.value = userData.data;
          console.log(userData.data);
          username.value = userData.data.username;
          if (userData.data.firstName && userData.data.lastName) {
            name_surname.value = userData.data.firstName + " " + userData.data.lastName;
          }
          email.value = userData.data.email;
          // this.mushrooms = response.mushrooms;
          // this.friends = response.friends;
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
</style>
