<template>
  <div>
    <div v-if="errorFindingUser" class="not-loggedIn container-md">
      <h1>Profil użytkownika</h1>
      <p>Taki użytkownik nie istnieje!</p>
      <button @click="goBack" class="btn fungeye-default-button">Powrót do poprzedniej strony</button>
    </div>
    <div v-else class="container-md">
      <div id="user-info">
        <UserProfileInfo :imgSrc="imgSrc" :username="username" :name_surname="name_surname" :createdAt="createdAt" />
        <div class="buttons">
          <button v-if="!followed" @click="follow" type="button" class="btn fungeye-default-button">
            &plus; Obserwuj
          </button>
          <button v-else @click="unfollow" type="button" class="btn fungeye-red-button">
            Usuń z obserwowanych
          </button>
        </div>
      </div>
      <UserProfileCollections :mushrooms="mushrooms" :follows="follows" />
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";
import UserProfileCollections from "@/components/UserProfileCollections.vue";
import UserProfileInfo from "@/components/UserProfileInfo.vue";
import UserService from "@/services/UserService";

export default {
  components: {
    ProfileImage,
    UserProfileCollections,
    UserProfileInfo,
  },
  async created() {
    this.id = this.$route.params.id;
    if (this.id == localStorage.getItem("id")) {
      this.$router.push({ name: "myProfile" });
      return;
    }
    console.log(this.id);
    const response = await UserService.getUserData(this.id);
    if (response.success === false) {
      this.errorFindingUser = true; 
      console.log(response.message);
      return;
    }
    this.user = response.data;
    this.imgSrc = response.data.imageUrl;
    this.username = response.data.username;
    this.name_surname = response.data.firstName + " " + response.data.lastName;
    this.email = response.data.email;
    this.createdAt = response.data.createdAt;
    // this.mushrooms = response.data.mushrooms;
    this.follows = response.data.follows;
    // if (this.follows.length > 0 && this.follows.includes(localStorage.getItem("id"))) {
    //   this.followed = true;
    // } else {
    //   this.followed = false;
    // }
  },
  data() {
    return {
      id: null,
      imgSrc: "",
      user: null,
      followed: true,
      username: "",
      name_surname: "",
      email: "",
      mushrooms: [],
      follows: [],
      createdAt: "",
      errorFindingUser: false,
    };
  },
  methods: {
    follow() {
      this.followed = true;
      const response = UserService.followUser(this.id);
      console.log(response);
    },
    unfollow() {
      this.followed = false;
      const response = UserService.unfollowUser(this.id);
      console.log(response);
    },
    goBack() {
      this.$router.go(-1);
    },
  },
};
</script>

<style scoped>
.not-loggedIn {
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
</style>
