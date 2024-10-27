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
        <div v-if="!isLoggedUser" class="buttons">
          <button v-if="followed == false" @click="follow" type="button" class="btn fungeye-default-button">
            &plus; Obserwuj
          </button>
          <button v-if="followed == true" @click="unfollow" type="button" class="btn fungeye-red-button">
            Usuń z obserwowanych
          </button>
        </div>
        <div v-else>
          <button @click="goToMyProfile" class="btn fungeye-default-button">Przejdź do swojego profilu</button>
        </div>
      </div>
      <UserProfileCollections :mushrooms="mushrooms" :follows="follows" :followers="followers" @click="fetchUser" />
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";
import UserProfileCollections from "@/components/UserProfileCollections.vue";
import UserProfileInfo from "@/components/UserProfileInfo.vue";
import UserService from "@/services/UserService";
import FollowService from "@/services/FollowService";
import { ref } from "vue";

export default {
  components: {
    ProfileImage,
    UserProfileCollections,
    UserProfileInfo,
  },
  async created() {
    this.fetchUser();
    this.checkIfFollowed();
  },
  data() {
    return {
      id: null,
      imgSrc: "",
      user: null,
      username: "",
      name_surname: "",
      email: "",
      mushrooms: [],
      createdAt: "",
      errorFindingUser: false,
    };
  },
  setup() {
    return {
      followed: ref(null),
      follows: ref([]),
      followers: ref([]),
    };
  },
  methods: {
    async fetchUser() {
      this.id = this.$route.params.id;
      // if (this.id == localStorage.getItem("id")) {
      //   this.$router.push({ name: "myProfile" });
      //   return;
      // }
      const response = await UserService.getUserData(this.id);
      const followsResponse = await FollowService.getFollowing(this.id);
      const followersResponse = await FollowService.getFollowers(this.id);
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
      this.follows = followsResponse.data;
      this.followers = followersResponse.data;
    },
    async checkIfFollowed() {
      const response = await FollowService.isFollowing(localStorage.getItem("id"), this.id);
      if (response.success === false) {
        console.log(response.message);
        return;
      }
      this.followed = response.data;
    },
    follow() {
      const response = FollowService.followUser(this.id);
      if (response.success === false) {
        console.log(response.message);
        return;
      }
      this.followed = true;
      this.fetchUser();
    },
    unfollow() {
      const response = FollowService.unfollowUser(this.id);
      if (response.success === false) {
        console.log(response.message);
        return;
      }
      this.followed = false;
      this.fetchUser();
    },
    goBack() {
      this.$router.go(-1);
    },
    isLoggedUser() {
      return this.id != localStorage.getItem("id");
    },
    goToMyProfile() {
      this.$router.push({ name: "myProfile" });
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
