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
      <div v-if="isLoading" class="container-md">
        <p>Ładowanie danych...</p>
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
        <UserProfileCollections :mushrooms="mushrooms" :trophys="trophys" :friends="friends" />
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

export default {
  components: {
    ProfileImage,
    UserProfileCollections,
    UserProfileInfo,
    BaseInput,
    EditUser,
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
        "src/assets/images/mushrooms/7a45d643-473a-417e-9f6d-6928440c0dc1.jpeg",
        "src/assets/images/mushrooms/db5c95b8-5596-4d91-8dcd-f1a9703e5a97.jpeg",
        "src/assets/images/mushrooms/a8eb8c5b-aaed-46e3-8798-a563d52ad54b.jpeg",
        "src/assets/images/mushrooms/8f54bebd-3afe-4c7c-93db-c1fd4ecc22fd.jpeg",
        "src/assets/images/mushrooms/bd692b45-3db3-4541-aaff-c40a12e08c12.jpeg",
        "src/assets/images/mushrooms/e4a1283f-27a2-467e-bd4a-d6ffc2ea0a5f.jpeg",
      ],
      trophys: [
        {
          name: "trofeum 1",
          img: "src/assets/images/trophys/2e0d2c9d-fc61-4346-afbc-6d55199d78cc.jpeg",
        },
        {
          name: "trofeum 2",
          img: "src/assets/images/trophys/7d5dbeac-5c83-4109-b3e7-ddf6f5777fcb.jpeg",
        },
        {
          name: "trofeum 3",
          img: "src/assets/images/trophys/71192d4e-9873-4951-a053-11954e2b6e06.jpeg",
        },
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
          if (userData.data.imageUrl) {
            imgSrc.value = userData.data.imageUrl;
            localStorage.setItem("profileImg", userData.data.imageUrl);
            setProfileImage(userData.data.imageUrl);
          }
          else { // placeholder image
            imgSrc.value = "https://avatar.iran.liara.run/public";
          }
          user.value = userData.data;
          console.log(userData.data);
          username.value = userData.data.username;
          if (userData.data.firstName && userData.data.lastName) {
            name_surname.value = userData.data.firstName + " " + userData.data.lastName;
          }
          email.value = userData.data.email;
          // this.mushrooms = response.mushrooms;
          // this.trophys = response.trophys;
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
