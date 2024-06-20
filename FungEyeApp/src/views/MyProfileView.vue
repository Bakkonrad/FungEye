<template>
  <div>
    <div class="container-md">
      <div id="user-info">
        <div class="left-side">
          <ProfileImage :imgSrc="imgSrc" width="170" height="170" />
          <div id="user-text">
            <p id="login">{{ login }}</p>
            <p id="name_surname">{{ name_surname }}</p>
            <p id="email">{{ email }}</p>
          </div>
        </div>
        <div class="buttons">
          <button @click="editProfile" type="button" class="btn fungeye-default-button">
            Edytuj profil
          </button>
          <button @click="logOut" type="button" class="btn fungeye-red-button">
            Wyloguj się
          </button>
        </div>
      </div>
      <div class="collection">
        <h2>Moja kolekcja -></h2>
        <div class="hstack gap-3" id="mushroom-collection">
          <div class="p-2" v-for="mushroom in mushrooms" :key="mushroom">
            <img class="mushroom" :src="mushroom" alt="Mushroom" />
          </div>
        </div>
      </div>
      <div class="bottom-collections">
        <div class="collection">
          <h2>Trofea -></h2>
          <div class="hstack gap-3" id="trophy-collection">
            <div class="p-2" v-for="trophy in trophys" :key="trophy">
              <div class="trophy-content">
                <img class="mushroom" :src="trophy.img" alt="Trophy" />
                <p>{{ trophy.name }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="collection">
          <h2>Znajomi -></h2>
          <div class="hstack gap-3" id="friends-collection">
            <div class="p-2" v-for="friend in friends" :key="friend">
              <div class="friend-content">
                <ProfileImage :imgSrc="friend.img" width="100" height="100" />
                <p>{{ friend.name }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";

export default {
  components: {
    ProfileImage,
  },
  data() {
    return {
      imgSrc: "src/assets/images/profile-images/profile-img1.jpeg",
      login: "Nazwa użytkownika",
      name_surname: "Imię Nazwisko",
      email: "Adres e-mail",
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
    };
  },
  methods: {
    editProfile() {
      alert("Edytuj profil");
    },
    logOut() {
      alert("Wyloguj się");
    },
  }
};

</script>

<style scoped>
.container {
  margin-top: 2em;
}

.left-side {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 2em;
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

#user-text {
  display: flex;
  flex-direction: column;
}

#login {
  font-size: 1.5em;
  font-weight: 700;
  /* font-family: "Lobster Two"; */
  font-family: "Signika";
}

.collection {
  margin-top: 3em;
}

.bottom-collections {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}

#mushroom-collection {
  display: flex;
  justify-content: flex-start;
}

.mushroom {
  width: auto;
  height: 100px;
  border-radius: 15px;
}

.trophy-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>
