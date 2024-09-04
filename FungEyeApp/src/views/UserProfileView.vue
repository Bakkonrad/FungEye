<template>
  <div>
    <div v-if="!user" class="not-loggedIn container-md">
      <h1>Profil użytkownika</h1>
      <p>Taki użytkownik nie istnieje!</p>
      <button @click="goBack" class="btn fungeye-default-button">Powrót do poprzedniej strony</button>
    </div>
    <div v-if="user" class="container-md">
      <div id="user-info">
        <UserProfileInfo
          :imgSrc="imgSrc"
          :username="username"
          :name_surname="name_surname"
          :email="email"
        />
        <div class="buttons">
          <button
            v-if="notMyFriend"
            @click="addFriend"
            type="button"
            class="btn fungeye-default-button"
          >
            &plus; Dodaj do znajomych
          </button>
          <button
            v-if="!notMyFriend"
            @click="deleteFriend"
            type="button"
            class="btn fungeye-red-button"
          >
            Usuń ze znajomych
          </button>
        </div>
      </div>
      <UserProfileCollections
        :mushrooms="mushrooms"
        :friends="friends"
      />
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
    //   const response = await UserService.getUserData();
    const response = {
      imgSrc: "https://picsum.photos/200/300",
      username: "username",
      firstName: "firstName",
      lastName: "lastName",
      email: "email",
      mushrooms: [
        "https://picsum.photos/200/300",
        "https://picsum.photos/200/300",
        "https://picsum.photos/200/300",
        "https://picsum.photos/200/300",
        "https://picsum.photos/200/300",
        "https://picsum.photos/200/300",
      ],
      friends: [
        {
          name: "Przyjaciel 1",
          img: "https://picsum.photos/200/300",
        },
        {
          name: "Przyjaciel 2",
          img: "https://picsum.photos/200/300",
        },
        {
          name: "Przyjaciel 3",
          img: "https://picsum.photos/200/300",
        },
      ],
    };
    console.log(response);

    if (response) {
      this.user = response;
      this.imgSrc = response.imgSrc;
      this.username = response.username;
      this.name_surname = response.firstName + " " + response.lastName;
      this.email = response.email;
      this.mushrooms = response.mushrooms;
      this.friends = response.friends;
    }
  },
  data() {
    return {
      imgSrc: "",
      user: null,
      notMyFriend: true,
      username: "",
      name_surname: "",
      email: "",
      mushrooms: [],
      friends: [],
    };
  },
  methods: {
    addFriend() {
      this.notMyFriend = false;
    },
    deleteFriend() {
      this.notMyFriend = true;
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
