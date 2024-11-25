<template>
  <div class="container-md">
    <div class="return">
      <RouterLink to="/atlas" class="btn fungeye-default-button">
        <font-awesome-icon icon="fa-solid fa-book-atlas" class="button-icon" />
        Atlas
      </RouterLink>
      <RouterLink to="/recognize" class="btn fungeye-default-button">
        <font-awesome-icon icon="fa-solid fa-eye" class="button-icon" />
        Rozpoznawanie
      </RouterLink>
    </div>
    <div v-if="error === false" class="container-md">
      <div class="mushroom-view">
        <div class="mushroom-view-header mb-3">
          <div class="header">
            <img class="mushroom-view-header-image" :src="setMainImage()" alt="Mushroom" @error="handleMainImageError()" />
            <div class="mushroom-names">
              <h1>{{ polishName }}</h1>
              <h2 class="latin-name">{{ latinName }}</h2>
            </div>
          </div>
          <button v-if="isLoggedIn" type="button" class="btn fungeye-default-button"
            @click="savedByUser ? deleteMushroomFromCollection() : saveMushroomToCollection()">
            <font-awesome-icon v-if="savedByUser" icon="fa-solid fa-bookmark" />
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
          <div v-if="imagesUrl" class="mushroom-view-photos-container">
            <h4>Inne zdjęcia</h4>
            <div class="mushroom-view-photos">
              <img v-for="photo in imagesUrl" :key="photo.id" :src="photo" class="mushroom-images" alt="User Photo" @error="handleImageError(photo)"/>
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
import noImage from '@/assets/images/no-image.svg';

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
      imagesUrl: [],
      attributes: [],
      description: "",
      savedByUser: '',
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
        if (response.success === false) {
          this.error = true;
          this.errorMessage = "Błąd podczas pobierania danych grzyba";
          return;
        }
        this.polishName = response.data.polishName;
        this.latinName = response.data.latinName;
        if (response.data.imagesUrl.length > 0) {
          this.mainImg = response.data.imagesUrl[0];
          this.imagesUrl = response.data.imagesUrl;
        }
        this.attributes = response.data.attributes;
        this.description = response.data.description;
        this.savedByUser = response.data.savedByUser;
      } catch (error) {
        console.error("Failed to fetch mushroom data:", error);
      } finally {
        this.isLoading = false;
      }
    },
    handleImageError(image) {
      if (this.imagesUrl.length > 0) {
        this.imagesUrl = this.imagesUrl.filter((photo) => photo !== image);
        return;
      }
      this.imagesUrl = [];
    },
    handleMainImageError() {
      if (this.imagesUrl.length > 0) {
        this.mainImg = this.imagesUrl[0];
        this.imagesUrl = this.imagesUrl.slice(1);
        return;
      }
      this.mainImg = noImage;
    },
    setMainImage() {
      if (this.imagesUrl.length > 0) {
        return this.imagesUrl[0];
      }
      return noImage;
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
    async saveMushroomToCollection() {
      const response = await FungiService.saveFungiToCollection(this.id);
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas zapisywania grzyba.';
        return;
      }
      this.error = false;
      this.fetchMushroomData();
    },
    async deleteMushroomFromCollection() {
      const response = await FungiService.deleteFungiFromCollection(this.id);
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
  display: flex;
  justify-content: flex-start;
  /* flex-direction: column; */
  gap: 10px;
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
  width: 150px;
  height: 150px;
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
  width: 100%;
}

.mushroom-view-photos-container {
  width: 100%;
}

.mushroom-view-photos {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 20px;
}

.mushroom-images {
  width: 250px;
  height: auto;
  object-fit: cover;
  border-radius: 10px;
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

@media screen and (max-width: 768px) {
  .mushroom-view-header {
    flex-direction: column;
    gap: 20px;
  }

  .header {
    flex-direction: column;
    gap: 0;
    width: 100%;
  }

  .mushroom-view-header-image {
    width: 90%;
    height: auto;
    margin: 0;
  }

  .mushroom-names {
    margin-top: 20px;
    align-items: center;
  }

  .mushroom-names h1 {
    text-align: center;
    font-size: 2.5em;
  }

  .attribute-list {
    justify-content: center;
  }

  .mushroom-view-photos {
    justify-content: center;
  }

  .mushroom-images {
    width: 100%;
    height: auto;
  }
  
}
</style>
