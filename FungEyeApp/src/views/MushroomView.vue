<template>
  <div class="container-md">
    <div class="breadcrumbs">
      <RouterLink to="/recognize" class="r-link">Rozpoznawanie</RouterLink> / {{ name }}
    </div>
    <div class="mushroom-view">
      <div class="mushroom-view-header mb-3">
        <div class="header">
          <span class="placeholder main-image"></span>
          <!-- <img class="mushroom-view-header-image" :src="mainImg" alt="Mushroom" /> -->
          <div class="mushroom-names">
            <h1>{{ name }}</h1>
            <h2 class="latin-name">{{ latinName }}</h2>
          </div>
        </div>
        <div class="buttons">
          <button type="button" class="btn fungeye-default-button">
            Dodaj do kolekcji
          </button>
          <button type="button" class="btn fungeye-default-button">
            Udostępnij
          </button>
        </div>
      </div>
      <div class="mushroom-view-attributes">
        <ul class="attribute-list">
          <li
            v-for="attribute in attributes"
            :key="attribute"
            class="attribute"
            :class="changeAttributeClass(attribute)"
          >
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
          <div class="mushroom-view-photos-user">
            <h4>Zdjęcia użytkowników</h4>
            <div class="mushroom-view-photos-user-images">
              <span
                class="placeholder"
                v-for="photo in userPhotos"
                :key="photo"
              >
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
</template>

<script>
export default {
    props: {
        id: {
            type: Number,
            required: true,
        },
    },
  data() {
    return {
      isLoading: true,
      id: null,
      name: "",
      latinName: "",
      mainImg: "",
      attributes: [],
      description: "",
      myPhotos: [
        {
          id: 1,
          url: "@/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
        },
        {
          id: 2,
          url: "@/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
        },
        {
          id: 3,
          url: "@/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
        },
      ],
      userPhotos: [],
    };
  },
  created() {
    this.id = this.$route.params.id;
    this.fetchMushroomData();
  },
  methods: {
    async fetchMushroomData() {
      this.isLoading = true;
      try {
        // Replace 'yourApiEndpoint' with the actual API endpoint and include the ID in the request
        // const response = await fetch(`yourApiEndpoint/${this.id}`);
        // const data = await response.json();
        // Update your component's data with the fetched data
        this.name = "Borowik szlachetny";
        this.latinName = "Boletus edulis";
        this.attributes = [
          "iglaste",
          "liściaste",
          "jadalny",
          "niejadalny",
          "trujący",
        ];
        this.description =
          "Gatunek grzybów z rodziny borowikowatych, potocznie nazywany prawdziwkiem. Występuje w Ameryce Północnej i w Europie. Ponadto został zawleczony do Nowej Zelandii i południowej Afryki. W Polsce często spotykany zwłaszcza w górach, rzadziej na niżu, zwykle rzadki w okolicach wielkich miast.";
        this.userPhotos = [
          {
            id: 1,
            url: "src/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
          },
          {
            id: 2,
            url: "src/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
          },
          {
            id: 3,
            url: "src/assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
          },
        ];
      } catch (error) {
        console.error("Failed to fetch mushroom data:", error);
        // Handle error appropriately
      } finally {
        this.isLoading = false; // Ensure loading state is updated regardless of success or failure
      }
    },
    changeAttributeClass(attribute) {
      return {
        attribute: true,
        coniferous: attribute === "iglaste",
        deciduous: attribute === "liściaste",
        edible: attribute === "jadalny",
        inedible: attribute === "niejadalny",
        poisonous: attribute === "trujący",
      };
    },
  },
};
</script>

<style scoped>
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

.buttons {
  display: flex;
  flex-direction: row;
  gap: 10px;
  margin-top: 20px;
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
