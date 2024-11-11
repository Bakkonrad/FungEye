<template>
  <div class="container-md">
    <div class="return">
      <RouterLink to="/atlas" class="btn fungeye-default-button">
        <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon"/>
        Powrót
      </RouterLink>
    </div>
    <div v-if="error === false" class="container-md">
      <!-- <div class="breadcrumbs">
      <RouterLink to="/recognize" class="r-link">Rozpoznawanie</RouterLink> / {{ name }}
    </div> -->
      <div class="mushroom-view">
        <div class="mushroom-view-header mb-3">
          <div class="header">
            <!-- <span class="placeholder main-image"></span> -->
            <img class="mushroom-view-header-image" :src="mainImg" alt="Mushroom" />
            <div class="mushroom-names">
              <h1>{{ polishName }}</h1>
              <h2 class="latin-name">{{ latinName }}</h2>
            </div>
          </div>
          <button v-if="isLoggedIn" type="button" class="btn fungeye-default-button" @click="saveMushroomToCollection">
            <font-awesome-icon v-if="savedByUser" icon="fa-regular fa-bookmark" />
            <font-awesome-icon v-else icon="fa-regular fa-bookmark" />
          </button>
        </div>
        <div class="mushroom-view-attributes">
          <ul class="attribute-list">
            <li v-for="attribute in attributes" :key="attribute" class="attribute"
              :class="changeAttributeClass(attribute)">
              {{ attribute }}
            </li>
          </ul>
        </div>
        <div class="mushroom-view-content">
          <div class="mushroom-view-description">
            <h3>Opis</h3>
            <p>{{ description }}</p>
          </div>
          <div class="mushroom-view-photos">
            <div v-if="myPhotos.length > 0" class="mushroom-view-photos-my">
              <h4>Moje zdjęcia</h4>
              <div class="mushroom-view-photos-my-images">
                <span class="placeholder" v-for="photo in myPhotos" :key="photo">
                </span>
                <!-- <img
                                v-for="photo in myPhotos"
                                :key="photo"
                                :src="photo.url"
                                alt="My Photo"
                                /> -->
              </div>
            </div>
            <div v-if="userPhotos.length > 0" class="mushroom-view-photos-user">
              <h4>Zdjęcia użytkowników</h4>
              <div class="mushroom-view-photos-user-images">
                <span class="placeholder" v-for="photo in userPhotos" :key="photo">
                </span>
                <!-- <img
                                v-for="photo in userPhotos"
                                :key="photo.id"
                                :src="photo.url"
                                alt="User Photo"
                                /> -->
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else class="container-md">
      <div class="error-message">
        {{ errorMessage }}
      </div>
    </div>
  </div>

</template>

<script>
import { checkAuth, isLoggedIn } from '@/services/AuthService';
import FungiService from '@/services/FungiService';
export default {
  props: {
    id: {
      type: Number,
      required: true,
    },
  },
  mounted() {
    checkAuth();
    this.isLoggedIn = isLoggedIn;
  },
  data() {
    return {
      isLoading: true,
      polishName: "",
      latinName: "",
      mainImg: "",
      attributes: [],
      description: "",
      savedByUser: false,
      myPhotos: [
      ],
      userPhotos: [],
      error: false,
      errorMessage: "",
      isLoggedIn: false,
    };
  },
  created() {
    this.fetchMushroomData();
  },
  methods: {
    async fetchMushroomData() {
      this.isLoading = true;
      try {
        const response = await FungiService.getFungi(this.id);
        // const response = { success: true };
        if (response.success === false) {
          this.error = true;
          this.errorMessage = "Błąd podczas pobierania danych grzyba";
          return;
        }
        console.log(response.data);
        this.polishName = response.data.polishName;
        this.latinName = response.data.latinName;
        if (response.data.imagesUrl > 0) {
          this.mainImg = response.data.imagesUrl[0].url;
        }
        this.attributes = response.data.attributes;
        this.description = response.data.description;
        this.savedByUser = response.data.savedByUser;
      } catch (error) {
        console.error("Failed to fetch mushroom data:", error);
      } finally {
        this.isLoading = false; // Ensure loading state is updated regardless of success or failure
      }
    },
    changeAttributeClass(attribute) {
      return {
        attribute: true,
        coniferous: attribute === "iglaste",
        deciduous: attribute === "liściaste",
        mixed: attribute === "mieszane",
        edible: attribute === "jadalny",
        inedible: attribute === "niejadalny",
        poisonous: attribute === "trujący",
      };
    },
    async saveMushroomToCollection(mushroomId) {
      const id = parseInt(mushroomId);
      // const response = await FungiService.saveFungiToCollection(id);
      const response = { success: true } // temporary
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas zapisywania grzyba.';
        return;
      }
      this.error = false;
      this.fetchMushroomData();
    },
    async deleteMushroomFromCollection(mushroomId) {
      const id = parseInt(mushroomId);
      // const response = await FungiService.deleteFungiFromCollection(id);
      const response = { success: true } // temporary
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas usuwania grzyba.';
        return;
      }
      this.error = false;
      this.fetchMushroomData();
    },
  },
};
</script>

<style scoped>
.return {
  margin-top: 20px;
}

.mushroom-view {
  margin-top: 20px;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.mushroom-view-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
}

.header {
  display: flex;
  align-items: center;
  gap: 20px;
}

.mushroom-view-header-image {
  width: 200px;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
}

.mushroom-names {
  display: flex;
  flex-direction: column;
}

.latin-name {
  font-size: 1.2em;
  font-style: italic;
}

.mushroom-view-content {
  margin-top: 20px;
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 20px;
}

.attribute-list {
  list-style-type: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  flex-direction: row;
  gap: 10px;
}

.mushroom-view-description {
  grid-column: 1 / 3;
}

.mushroom-view-photos {
  grid-column: 1 / 3;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.mushroom-view-photos-my {
  grid-column: 1 / 2;
}

.mushroom-view-photos-user {
  grid-column: 2 / 3;
}

.mushroom-view-photos-my-images,
.mushroom-view-photos-user-images {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
  gap: 10px;
}

.placeholder {
  width: 100px;
  height: 100px;
  background-color: #f0f0f0;
  display: inline-block;
}

.placeholder {
  width: 100px;
  height: 100px;
  background-image: url("@/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg");
  background-size: cover;
  background-position: center;
  display: inline-block;
}
</style>
