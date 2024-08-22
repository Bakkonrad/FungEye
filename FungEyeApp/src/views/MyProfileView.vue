<template>
  <div>
    <div
      v-if="errorLoadingData || isLoading"
      class="error-div container-md"
    >
      <h1>Profil użytkownika</h1>
      <div v-if="errorLoadingData" class="error-div">
        <p class="error-loading-data">{{ errorMessage }}</p>
        <router-link to="/log-in" class="btn fungeye-red-button" @click="logOut"
          >Zaloguj się ponownie</router-link
        >
      </div>
      <div v-if="isLoading" class="container-md">
        <p>Ładowanie danych...</p>
      </div>
    </div>
    <div v-else>
      <div v-if="!isEditing" class="container-md">
        <div id="user-info">
          <UserProfileInfo
            :imgSrc="imgSrc"
            :username="username"
            :name_surname="name_surname"
            :email="email"
          />
          <div class="buttons">
            <button
              @click="startEditing"
              type="button"
              class="btn fungeye-default-button"
            >
              Edytuj profil
            </button>
            <button
              @click="logOut"
              type="button"
              class="btn fungeye-red-button"
            >
              Wyloguj się
            </button>
          </div>
        </div>
        <UserProfileCollections
          :mushrooms="mushrooms"
          :trophys="trophys"
          :friends="friends"
        />
      </div>
    </div>
    <div class="edit-user" v-if="isEditing">
      <button
        @click="cancelEditing"
        type="button"
        class="btn fungeye-default-button"
      >
        Powrót do mojego profilu
      </button>
      <EditUser
        :user="user"
        @cancel-edit="cancelEditing"
        @save-user="saveUser"
      />
      <button
        @click="deleteAccount"
        type="button"
        class="btn fungeye-red-button"
      >
        Usuń konto
      </button>
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";
import UserService from "@/services/UserService";
import UserProfileCollections from "@/components/UserProfileCollections.vue";
import UserProfileInfo from "@/components/UserProfileInfo.vue";
import EditUser from "@/components/EditUser.vue";

export default {
  components: {
    ProfileImage,
    UserProfileCollections,
    UserProfileInfo,
    EditUser,
  },
  mounted() {
    if (localStorage.getItem("token")) {
      this.fetchUser();
    } else {
      this.$router.push("/log-in");
    }
  },
  data() {
    return {
      imgSrc: "src/assets/images/profile-images/profile-img1.jpeg",
      user: null,
      username: "",
      name_surname: "",
      email: "",
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
      errorLoadingData: false,
      errorMessage: "",
      isEditing: false,
      isLoading: true,
    };
  },
  methods: {
    async fetchUser() {
      try {
        this.isLoading = true;
        const userData = await UserService.getUserData();
        if (!userData.success) {
          this.errorLoadingData = true;
          this.errorMessage = userData.message;
          return;
        } else {
          this.user = userData.data;
          this.username = userData.data.username;
          if (userData.data.firstName && userData.data.lastName) {
            this.name_surname = userData.data.firstName + " " + userData.data.lastName;
          }
          this.email = userData.data.email;
          // this.imgSrc = response.imgSrc;
          // this.mushrooms = response.mushrooms;
          // this.trophys = response.trophys;
          // this.friends = response.friends;
        }
      } catch (error) {
        this.errorLoadingData = true;
        this.errorMessage = userData.message;
        this.$router.push("/log-in");
      } finally {
        this.isLoading = false;
      }
    },
    logOut() {
      UserService.logout();
      this.$router.push("/");
    },
    startEditing() {
      this.isEditing = true;
    },
    cancelEditing() {
      this.isEditing = false;
    },
    saveUser() {
      alert("Zapisano zmiany");
      this.isEditing = false;
    },
    deleteAccount() {
      // UserService.deleteAccount();
      alert("Konto usunięte");
      UserService.logout();
      this.$router.push("/");
    },
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

.edit-user {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1em;
}
</style>
