<template>
  <div class="atlas-view">
    <h1>Atlas grzybów</h1>

    <div class="upper-buttons">
      <div v-if="isAdmin" class="button-center">
        <button @click="showAddMushroomModal = true" class="btn fungeye-default-button">
          <font-awesome-icon icon="fa-solid fa-plus" class="button-icon" />
          Nowy grzyb</button>
      </div>
      <div class="button-center">
        <button v-if="isLoggedIn" class="btn fungeye-default-button" @click="toggleSavedTab">
          <font-awesome-icon v-if="!showSaved" icon="fa-solid fa-bookmark" class="button-icon" />
          {{ showSaved === true ? 'Pokaż wszystkie' : 'Pokaż tylko zapisane' }}
        </button>
      </div>
    </div>

    <div class="feedback">
      <div v-if="error" class="error-message">
        <p>{{ errorMessage }}</p>
      </div>
      <div v-else class="success-message">
        <p v-if="successMessage">{{ successMessage }}</p>
      </div>
    </div>

    <button ref="goToTheTopButton" class="btn fungeye-default-button" type="button" id="goToTheTopButton"
      @click="goToTheTop" title="go to the top"><font-awesome-icon icon="fa-solid fa-arrow-up" /></button>

    <SearchBar @search="handleSearch" />
    <div class="alphabet-filter">
      <span v-for="letter in alphabet" :key="letter" @click="filterByLetter(letter)"
        :class="{ 'active-letter': activeLetter === letter }">
        {{ letter }}
      </span>
    </div>

    <div class="attribute-filter">
      <span v-for="attribute in availableAttributes" :key="attribute" @click="toggleAttributeFilter(attribute)"
        :class="['attribute', attributeClass(attribute), { 'active-attribute': isActiveAttribute(attribute) }]">
        {{ attribute }}
      </span>
    </div>

    <div class="feedback mt-5">
      <LoadingSpinner v-if="isLoading"></LoadingSpinner>
      <div v-if="mushrooms.length === 0">
        <p class="error-message">Nie znaleziono grzybów spełniających kryteria wyszukiwania.</p>
      </div>
    </div>

    <div class="mushroom-list">
      <div v-for="mushroom in mushrooms" :key="mushroom.id" class="mushroom-card"
        @click="openMushroomView(mushroom.id)">
        <span class="left">
          <img :src="mushroom.image" alt="Mushroom Image" class="mushroom-image" />
          <div class="mushroom-info">
            <h3>{{ mushroom.name }}</h3>
            <div class="attributes">
              <span v-for="attr in mushroom.attributes" :key="attr" class="mushroom-attribute"
                @click.stop="toggleAttributeFilter(attr)"
                :class="['attribute', attributeClass(attr), { 'active-attribute': isActiveAttribute(attr) }]">
                {{ attr }}
              </span>
            </div>
          </div>
        </span>
        <!-- Przycisk edytowania i usuwania grzyba -->
        <div class="mushroom-actions">
          <button v-if="isLoggedIn" class="btn btn-mushroom fungeye-default-button"
            @click.stop="mushroom.savedByUser ? deleteMushroomFromCollection() : saveMushroomToCollection()">
            <font-awesome-icon v-if="!mushroom.savedByUser" icon="fa-regular fa-bookmark" />
            <font-awesome-icon v-else icon="fa-solid fa-bookmark" />
          </button>
          <button v-if="isAdmin" @click.stop="editMushroom(mushroom)" class="btn btn-mushroom fungeye-default-button">
            <font-awesome-icon icon="fa-solid fa-pen" />
          </button>
          <button v-if="isAdmin" @click.stop="confirmDeleteMushroom(mushroom)"
            class="btn btn-mushroom fungeye-red-button">
            <font-awesome-icon icon="fa-solid fa-trash" />
          </button>
        </div>
      </div>
    </div>

    <!-- Modal dodawania/edycji grzyba -->
    <div v-if="showAddMushroomModal || showEditMushroomModal" class="modal">
      <MushroomModal :showEditMushroomModal="showEditMushroomModal" :mushroomForm="mushroomForm"
        :availableAttributes="availableAttributes" @close="closeModal"></MushroomModal>
    </div>

    <!-- Modal potwierdzający usunięcie -->
    <div v-if="showDeleteMushroomModal" class="modal">
      <div class="modal-content">
        <h2>Potwierdź działanie</h2>
        <p>Czy na pewno chcesz usunąć grzyb <b>{{ mushroomToDelete.name }}</b> z bazy danych?</p>
        <p class="warning"><strong>Uwaga!</strong> To działanie jest nieodwracalne.</p>
        <span class="buttons">
          <button class="btn fungeye-red-button" @click="deleteMushroom">Usuń</button>
          <button class="btn fungeye-secondary-black-button" @click="closeModal">Anuluj</button>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import SearchBar from '@/components/SearchBar.vue';
import BaseInput from '@/components/BaseInput.vue';
import MushroomModal from '@/components/MushroomModal.vue';
import FungiService from '@/services/FungiService';
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import { checkAdmin, checkAuth, isAdmin, isLoggedIn } from '@/services/AuthService';

export default {
  components: {
    SearchBar,
    BaseInput,
    MushroomModal,
    LoadingSpinner,
  },
  data() {
    return {
      isAdmin: false,
      isLoggedIn: isLoggedIn,
      searchQuery: '',
      activeLetter: '',
      selectedAttributes: [],
      addMushroomSelectedAttributes: [],
      mushrooms: [],
      localMushrooms: [{
        id: 1,
        name: 'Borowik szlachetny',
        image: 'src/assets/images/mushrooms/ATLAS-borowik.jpg',
        attributes: ['iglaste', 'liściaste', 'jadalny'],
        description: 'Borowik szlachetny to jeden z najbardziej cenionych grzybów jadalnych.'
      },
      {
        id: 2,
        name: 'Muchomor czerwony',
        image: 'src/assets/images/mushrooms/ATLAS-muchomor.jpg',
        attributes: ['iglaste', 'liściaste', 'trujący', 'niejadalny'],
        description: 'Muchomor czerwony to grzyb trujący, znany ze swojego charakterystycznego wyglądu.'
      },
      {
        id: 3,
        name: 'Pieczarka polna',
        image: 'src/assets/images/mushrooms/ATLAS-pieczarka.jpg',
        attributes: ['jadalny', 'łąkowy'],
        description: 'Pieczarka polna to popularny grzyb jadalny, często spotykany na łąkach.'
      },
      {
        id: 4,
        name: 'Kurka',
        image: 'src/assets/images/mushrooms/ATLAS-kurka.jpg',
        attributes: ['jadalny', 'iglaste'],
        description: 'Kurka to smaczny grzyb jadalny, często spotykany w lasach iglastych.'
      },
      {
        id: 5,
        name: 'Maślak zwyczajny',
        image: 'src/assets/images/mushrooms/ATLAS-maslak.jpg',
        attributes: ['jadalny', 'iglaste'],
        description: 'Maślak zwyczajny to grzyb jadalny, często spotykany w lasach iglastych.'
      },
      {
        id: 6,
        name: 'Opieńka miodowa',
        image: 'src/assets/images/mushrooms/ATLAS-opienka.jpg',
        attributes: ['jadalny', 'liściaste'],
        description: 'Opieńka miodowa to grzyb jadalny, często spotykany w lasach liściastych.'
      },
      {
        id: 7,
        name: 'Gąska zielonka',
        image: 'src/assets/images/mushrooms/ATLAS-gaska.jpg',
        attributes: ['jadalny', 'iglaste'],
        description: 'Gąska zielonka to grzyb jadalny, często spotykany w lasach iglastych.'
      },
      {
        id: 8,
        name: 'Koźlarz babka',
        image: 'src/assets/images/mushrooms/ATLAS-kozlarz.jpg',
        attributes: ['jadalny', 'liściaste'],
        description: 'Koźlarz babka to grzyb jadalny, często spotykany w lasach liściastych.'
      },
      {
        id: 9,
        name: 'Rydz',
        image: 'src/assets/images/mushrooms/ATLAS-rydz.jpg',
        attributes: ['jadalny', 'iglaste'],
        description: 'Rydz to grzyb jadalny, często spotykany w lasach iglastych.'
      },
      {
        id: 10,
        name: 'Sromotnik bezwstydny',
        image: 'src/assets/images/mushrooms/ATLAS-sromotnik.jpg',
        attributes: ['niejadalny', 'liściaste'],
        description: 'Sromotnik bezwstydny to grzyb niejadalny, często spotykany w lasach liściastych.'
      },
      {
        id: 11,
        name: 'Trufla czarna',
        image: 'src/assets/images/mushrooms/ATLAS-trufla.jpg',
        attributes: ['jadalny', 'liściaste'],
        description: 'Trufla czarna to grzyb jadalny, ceniony za swój wyjątkowy smak.'
      },
      {
        id: 12,
        name: 'Mleczaj rydz',
        image: 'src/assets/images/mushrooms/ATLAS-mleczaj.jpg',
        attributes: ['jadalny', 'iglaste'],
        description: 'Mleczaj rydz to grzyb jadalny, często spotykany w lasach iglastych.'
      }],
      alphabet: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split(''),
      availableAttributes: ['iglaste', 'liściaste', 'jadalny', 'niejadalny', 'trujący'],
      showAddMushroomModal: false,
      showEditMushroomModal: false,
      showDeleteMushroomModal: false,

      mushroomForm: {
        polishName: '',
        latinName: '',
        images: [],
        attributes: [],
        description: '',
        savedByUser: false,
      },
      mushroomToDelete: null,
      editMushroomId: null,
      isDragging: false,
      error: false,
      errorMessage: '',
      successMessage: '',
      showSaved: false,
      isLoading: false,
      currentPage: 1,
      goToTheTopButton: null,
    };
  },
  async mounted() {
    checkAuth();
    checkAdmin();
    this.isAdmin = isAdmin;
    this.isLoggedIn = isLoggedIn;
    this.goToTheTopButton = this.$refs.goToTheTopButton;
    this.filterMushrooms(this.currentPage);
    window.addEventListener("scroll", this.handleScroll);
  },
  beforeDestroy() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    async getFungies(page, search) {
      // const response = await FungiService.getAllFungies(page, search);
      const response = { success: true, data: this.localMushrooms };
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas pobierania grzybów.';
        return;
      }
      if (response.data.length === 0 && this.users.length === 0) {
        this.error = true;
        this.errorMessage = "Nie znaleziono grzybów spełniających kryteria.";
        return;
      } else if (response.data.length === 0) {
        this.error = true;
        this.errorMessage = "To już wszystkie wyniki.";
        return;
      }
      this.error = false;
      const newFungies = response.data;
      this.mushrooms = [...this.mushrooms, ...newFungies];
      return newFungies;
    },
    handleScroll() {
      if (this.goToTheTopButton === null) {
        return;
      }
      if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
        if (!this.error) {
          this.currentPage++;
          this.filterMushrooms();
        }
      }
      if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        this.goToTheTopButton.style.display = "block";
      } else {
        this.goToTheTopButton.style.display = "none";
      }
    },
    goToTheTop() {
      document.body.scrollTop = 0;
      document.documentElement.scrollTop = 0;
    },
    async filterMushrooms(page, search) {
      this.isLoading = true;
      let filtered = await this.getFungies(page, search);
      this.isLoading = false;

      if (this.searchQuery) {
        filtered = filtered.filter((mushroom) =>
          mushroom.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        );
      }

      if (this.selectedAttributes.length) {
        filtered = filtered.filter((mushroom) =>
          this.selectedAttributes.every((attr) => mushroom.attributes.includes(attr))
        );
      }

      if (this.activeLetter) {
        filtered = filtered.filter((mushroom) =>
          mushroom.name.toLowerCase().startsWith(this.activeLetter.toLowerCase())
        );
      }

      if (this.showSaved) {
        filtered = filtered.filter((mushroom) => mushroom.savedByUser === true);
      }

      this.mushrooms = filtered;
      console.log(this.mushrooms);
    },
    handleSearch(query) {
      this.searchQuery = query;
      this.filterMushrooms(1, query);
    },
    filterByLetter(letter) {
      if (this.activeLetter === letter) {
        this.resetActiveLetter();
      } else {
        this.activeLetter = letter;
      }
      this.filterMushrooms();
    },
    resetActiveLetter() {
      this.activeLetter = '';
    },
    openMushroomView(id) {
      const mushroomId = parseInt(id);
      console.log('Otwieram kartę grzyba:', mushroomId);
      this.$router.push({ name: 'MushroomView', params: { id: mushroomId } });
    },
    toggleAttributeFilter(attribute) {
      const index = this.selectedAttributes.indexOf(attribute);
      if (index > -1) {
        this.selectedAttributes.splice(index, 1);
      } else {
        this.selectedAttributes.push(attribute);
      }
      this.filterMushrooms();
    },
    isActiveAttribute(attribute) {
      return this.selectedAttributes.includes(attribute);
    },
    attributeClass(attribute) {
      return {
        coniferous: attribute === 'iglaste',
        deciduous: attribute === 'liściaste',
        edible: attribute === 'jadalny',
        inedible: attribute === 'niejadalny',
        poisonous: attribute === 'trujący',
      };
    },
    editMushroom(mushroom) {
      this.editMushroomId = mushroom.id;
      this.mushroomForm = { ...mushroom };
      this.showEditMushroomModal = true;
    },
    confirmDeleteMushroom(mushroom) {
      this.mushroomToDelete = mushroom;
      this.showDeleteMushroomModal = true;
    },
    async deleteMushroom() {
      // this.mushrooms = this.mushrooms.filter((m) => m.id !== this.mushroomToDelete.id);
      // const response = await FungiService.deleteFungi(this.mushroomToDelete.id);
      const response = { success: true }
      this.closeModal();
      if (response.success === true) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas usuwania grzyba.';
        return;
      }
      this.error = false;
      this.successMessage = 'Grzyb został usunięty.';
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
      this.filterMushrooms();
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
      this.filterMushrooms();
    },
    closeModal() {
      this.showAddMushroomModal = false;
      this.showEditMushroomModal = false;
      this.showDeleteMushroomModal = false;
      this.mushroomForm = {
        polishName: '',
        latinName: '',
        images: [],
        attributes: [],
        description: ''
      };
      this.mushroomToDelete = null;
    },
    toggleSavedTab() {
      this.showSaved = !this.showSaved;
      this.filterMushrooms();
    },
  },
};
</script>

<style scoped>
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 100;
}

.modal-content {
  background-color: var(--beige);
  padding: 20px;
  border-radius: 10px;
  width: 450px;
}

.feedback {
  display: flex;
  justify-content: center;
}

.upper-buttons {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 1rem;
}

.success-message {
  color: var(--green);
}

h3 {
  font-weight: 500;
}

.btn-mushroom {
  padding: 0 1rem;
  height: 1.5em;
  width: 2.5em;
}

.button-center {
  display: flex;
  justify-content: center;
  margin-bottom: 1rem;
}

.attribute-selection {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 10px;
  margin: 1rem 0
}

.atlas-view {
  padding: 0 20px;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.alphabet-filter {
  margin: 20px 0;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 5px;
}

.alphabet-filter span {
  margin: 0 10px;
  cursor: pointer;
  font-size: 20px;
  border-radius: 15px;
  transition: font-weight 0.3s;
  user-select: none;
}

.active-letter {
  font-weight: bold;
}

.attribute-filter {
  margin: 20px 0;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 5px;
}

/* css dla filtrów atrybutów */
.attribute-filter span {
  margin: 0 7px;
  cursor: pointer;
  font-size: 17px;
  padding: 1px 10px;
  border-radius: 15px;
  /* background-color: #f0f0f0; */
  transition: font-weight 0.3s, background-color 0.3s;
  user-select: none;
}

.active-attribute {
  font-weight: bold;
}

/* css dla atrybutów w kartach grzybów */
.attributes span {
  padding: 2px 15px;
  border-radius: 15px;
  font-weight: 300;
  transition: font-weight 0.3s;
  user-select: none;
}

/* pogrubienie dla aktywnych atrybutów */
.active-attribute.coniferous,
.attribute-filter span.coniferous.active-attribute,
.active-attribute.deciduous,
.attribute-filter span.deciduous.active-attribute,
.active-attribute.edible,
.attribute-filter span.edible-active-attribute,
.active-attribute.inedible,
.attribute-filter span.inedible.active-attribute,
.active-attribute.poisonous,
.attribute-filter span.poisonous.active-attribute {
  font-weight: bold;
}

.mushroom-list {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.mushroom-card {
  position: relative;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: flex-start;
  background-color: var(--beige);
  padding: 10px;
  margin: 10px 0;
  width: 70%;
  border-radius: 10px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: box-shadow 0.3s;
  user-select: none;
}

.mushroom-card:hover {
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.4);
}

.mushroom-image {
  width: 80px;
  height: 80px;
  margin-right: 20px;
  border-radius: 50%;
}

.mushroom-card .left {
  display: flex;
  flex-direction: row;
}

.mushroom-info {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.mushroom-info h3 {
  margin: 0;
  font-size: 22px;
}

.mushroom-actions {
  display: flex;
  flex-direction: row;
  gap: 5px;
}

.attributes {
  display: flex;
  margin-top: 5px;
  list-style-type: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  flex-direction: row;
  gap: 10px;
}

/* wyłączenie kliku gdy karty grzyba gdy najeżdża się na atrybut (nie działa) */
.attributes span:hover {
  cursor: pointer;
}

.attributes span:hover~.mushroom-card {
  pointer-events: none;
  background-color: #f0f0f0;
}

.warning {
  color: var(--red);
}

@media screen and (max-width: 955px) {
  .mushroom-card {
    align-items: flex-start;
    flex-direction: column-reverse;
    gap: 15px;
    width: 95%;
  }
}

@media screen and (max-width: 576px) {
  .mushroom-card {
    width: 100%;
  }

  .mushroom-card .left {
    flex-direction: column;
    gap: 10px;
  }
}

.buttons {
  display: flex;
  justify-content: space-between;
  gap: 10px;
}

.buttons button {
  width: 100%;
}

#goToTheTopButton {
  display: none;
  position: fixed;
  bottom: 20px;
  right: 30px;
  z-index: 99;
  border: none;
  outline: none;
  color: white;
  cursor: pointer;
  padding: 15px;
  height: auto;
}
</style>