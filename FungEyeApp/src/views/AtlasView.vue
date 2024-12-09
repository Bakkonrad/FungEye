<template>
  <div class="atlas-view">
    <h1 class="page-title">Atlas grzybów</h1>

    <div class="upper-buttons">
      <div v-if="isAdmin" class="button-center">
        <button @click="showAddMushroomModal = true" class="btn fungeye-default-button">
          <font-awesome-icon icon="fa-solid fa-plus" class="button-icon" />
          <p class="button-text">
            Nowy grzyb
          </p>
        </button>
      </div>
      <div class="button-center">
        <button v-if="isLoggedIn" class="btn fungeye-default-button" @click="toggleSavedTab">
          <font-awesome-icon v-if="!showSaved" icon="fa-solid fa-bookmark" class="button-icon" />
          <p class="button-text">
            {{ showSaved === true ? 'Pokaż wszystkie' : 'Pokaż tylko zapisane' }}
          </p>
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
      <div v-for="category in availableAttributes" class="attributes-categories">
        {{ category[0] === 'iglaste' ? 'Siedlisko:' : 'Jadalność:' }}
        <span v-for="attribute in category" :key="attribute" @click="toggleAttributeFilter(attribute)"
          :class="['attribute', attributeClass(attribute), { 'active-attribute': isActiveAttribute(attribute) }]">
          {{ attribute }}
        </span>
        <span v-if="category[0] === 'iglaste'" class="separator">|</span>
      </div>
    </div>

    <div class="mushroom-list">
      <div v-for="mushroom in filteredMushrooms" :key="mushroom.id" class="mushroom-card"
        @click="openMushroomView(mushroom.id)">
        <span class="left">
          <img v-if="mushroom.imagesUrl" :src="setMushroomImage(mushroom.id)" alt="Mushroom Image"
            class="mushroom-image" @error="handleImageError(mushroom.id)" />
          <div class="mushroom-info">
            <h3 class="polish-name">{{ mushroom.polishName }}</h3>
            <h4 class="latin-name">{{ mushroom.latinName }}</h4>
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
            @click.stop="mushroom.savedByUser ? deleteMushroomFromCollection(mushroom.id) : saveMushroomToCollection(mushroom.id)">
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

    <div class="feedback mt-5">
      <LoadingSpinner v-if="isLoading"></LoadingSpinner>
      <div v-if="fetchMushroomsErrorMessage">
        <p class="error-message">{{ fetchMushroomsErrorMessage }}</p>
      </div>
      <div v-if="noMoreResults">
        <p class="no-more-results">To już wszystkie wyniki.</p>
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
      filteredMushrooms: [],
      alphabet: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split(''),
      availableAttributes: [['iglaste', 'mieszane', 'liściaste'], ['jadalny', 'niejadalny', 'trujący']],
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
      fetchMushroomsErrorMessage: '',
      noMoreResults: false,
      filters: {
        edibility: '',
        habitat: '',
        toxicity: '',
        letter: '',
        savedByUser: '',
      },
    };
  },
  async mounted() {
    checkAuth();
    checkAdmin();
    this.isAdmin = isAdmin;
    this.isLoggedIn = isLoggedIn;
    this.goToTheTopButton = this.$refs.goToTheTopButton;
    this.getFungies();
    window.addEventListener("scroll", this.handleScroll);
  },
  beforeDestroy() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    async getFungies(page) {
      try {
        if (this.noMoreResults || this.isLoading) {
          return;
        }
        if (page === undefined || this.currentPage === undefined) {
          page = 1;
        }
        this.isLoading = true;
        this.setFilters();
        const response = await FungiService.getAllFungies(page, this.searchQuery, this.filters);
        if (response.success === false) {
          this.error = true;
          this.fetchMushroomsErrorMessage = 'Wystąpił błąd podczas pobierania grzybów.';
          return;
        }
        if (response.data.length === 0 && this.mushrooms.length > 0) {
          this.error = true;
          this.noMoreResults = true;
          return;
        }
        else if (response.data.length === 0) {
          this.error = true;
          this.fetchMushroomsErrorMessage = "Nie znaleziono grzybów spełniających kryteria.";
          return;
        }
        this.fetchMushroomsErrorMessage = '';
        this.noMoreResults = false;
        this.error = false;
        const newFungies = response.data;
        this.mushrooms = [...this.mushrooms, ...newFungies];
        this.filterMushrooms();
        return newFungies;
      }
      catch (error) {
        this.error = true;
        this.fetchMushroomsErrorMessage = 'Wystąpił błąd podczas pobierania grzybów.';
      }
      finally {
        this.isLoading = false;
      }
    },
    handleScroll() {
      if (this.goToTheTopButton === null) {
        return;
      }
      if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
        if (!this.error && !this.noMoreResults && !this.isLoading) {
          this.currentPage++;
          this.getFungies(this.currentPage);
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
    async filterMushrooms() {
      let filtered = [...this.mushrooms];

      this.filteredMushrooms = filtered;
    },
    setFilters() {
      this.filters = {
        edibility: '',
        habitat: '',
        toxicity: '',
        letter: '',
        savedByUser: '',
      };

      if (this.selectedAttributes.length > 0) {
        if (this.selectedAttributes.includes('jadalny')) {
          this.filters.edibility = 'jadalny';
        } else if (this.selectedAttributes.includes('trujący')) {
          this.filters.toxicity = 'trujący';
        } else if (this.selectedAttributes.includes('niejadalny')) {
          this.filters.edibility = 'niejadalny';
        }
        if (this.selectedAttributes.includes('iglaste')) {
          this.filters.habitat = 'iglasty';
        } else if (this.selectedAttributes.includes('liściaste')) {
          this.filters.habitat = 'liściasty';
        } else if (this.selectedAttributes.includes('mieszane')) {
          this.filters.habitat = 'mieszany';
        }
      }
      if (this.activeLetter) {
        this.filters.letter = this.activeLetter;
      }
      if (this.showSaved) {
        this.filters.savedByUser = true;
      }
    },
    handleImageError(id) {
      const mushroom = this.mushrooms.find((mushroom) => mushroom.id === id);
      if (mushroom.imagesUrl.length > 0) {
        mushroom.imagesUrl.shift();
      } else {
        mushroom.imagesUrl.push('src/assets/images/no-image.svg');
      }
    },
    setMushroomImage(id) {
      const mushroom = this.mushrooms.find((mushroom) => mushroom.id === id);
      if (!mushroom) {
        return;
      }
      if (mushroom.imagesUrl.length === 0) {
        return 'src/assets/images/no-image.svg';
      }
      return mushroom.imagesUrl[0];
    },
    handleSearch(query) {
      this.searchQuery = query;
      this.currentPage = 1;
      this.mushrooms = [];
      this.noMoreResults = false;
      this.getFungies(this.currentPage);
    },
    clearMusrooms() {
      this.noMoreResults = false;
      this.mushrooms = [];
      this.filteredMushrooms = [];
      this.currentPage = 1;
      this.getFungies();
    },
    filterByLetter(letter) {
      if (this.activeLetter === letter) {
        this.activeLetter = '';
      } else {
        this.activeLetter = letter;
      }
      this.clearMusrooms();
    },
    openMushroomView(id) {
      const mushroomId = parseInt(id);
      this.$router.push({ name: 'MushroomView', params: { id: mushroomId } });
    },
    // handling the attribute filter
    toggleAttributeFilter(attribute) {
      if (this.selectedAttributes.includes(attribute)) {
        this.selectedAttributes = this.selectedAttributes.filter((a) => a !== attribute);
      } else {
        const mutuallyExclusiveGroups = [
          ['jadalny', 'niejadalny', 'trujący'],
          ['iglaste', 'liściaste', 'mieszane']
        ];

        mutuallyExclusiveGroups.forEach(group => {
          if (group.includes(attribute)) {
            this.selectedAttributes = this.selectedAttributes.filter(a => !group.includes(a) || a === attribute);
          }
        });
        this.selectedAttributes.push(attribute);
      }
      this.clearMusrooms();
    },
    isActiveAttribute(attribute) {
      return this.selectedAttributes.includes(attribute);
    },
    attributeClass(attribute) {
      return {
        coniferous: attribute === 'iglaste',
        deciduous: attribute === 'liściaste',
        mixed: attribute === 'mieszane',
        edible: attribute === 'jadalny',
        inedible: attribute === 'niejadalny',
        poisonous: attribute === 'trujący',
      };
    },
    // actions on mushrooms
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
      const response = await FungiService.deleteFungi(this.mushroomToDelete.id);
      this.closeModal();
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas usuwania grzyba.';
        return;
      }
      this.error = false;
      this.successMessage = 'Grzyb został usunięty.';
      this.getFungies(1);
    },
    async saveMushroomToCollection(mushroomId) {
      const id = parseInt(mushroomId);
      const response = await FungiService.saveFungiToCollection(id);
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas zapisywania grzyba.';
        return;
      }
      this.error = false;
      this.mushrooms = [];
      this.getFungies(1);
    },
    async deleteMushroomFromCollection(mushroomId) {
      const id = parseInt(mushroomId);
      const response = await FungiService.deleteFungiFromCollection(id);
      if (response.success === false) {
        this.error = true;
        this.errorMessage = 'Wystąpił błąd podczas usuwania grzyba.';
        return;
      }
      this.error = false;
      this.mushrooms = [];
      this.getFungies(1);
    },
    closeModal() {
      this.showAddMushroomModal = false;
      this.showEditMushroomModal = false;
      this.showDeleteMushroomModal = false;
      this.mushroomForm = {
        polishName: '',
        latinName: '',
        attributes: [],
        description: ''
      };
      this.mushroomToDelete = null;
      this.mushrooms = [];
      this.getFungies(1);
    },
    toggleSavedTab() {
      this.showSaved = !this.showSaved;
      this.clearMusrooms();
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

.button-text {
  margin: 0;
  font-size: 1em;
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

.attribute-filter span {
  margin: 0 7px;
  cursor: pointer;
  font-size: 17px;
  padding: 1px 10px;
  border-radius: 15px;
  transition: font-weight 0.3s, background-color 0.3s;
  user-select: none;
}

.active-attribute {
  font-weight: bold;
}

.attributes span {
  padding: 2px 15px;
  border-radius: 15px;
  font-weight: 300;
  transition: font-weight 0.3s;
  user-select: none;
}

.active-attribute.coniferous,
.attribute-filter span.coniferous.active-attribute,
.active-attribute.deciduous,
.attribute-filter span.deciduous.active-attribute,
.attribute-filter span.mixed.active-attribute,
.active-attribute.mixed,
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
  object-fit: cover;
  object-position: center;
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

.polish-name {
  font-size: 1.2em;
}

.latin-name {
  font-size: 1em;
  font-style: italic;
  font-weight: 300;
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

@media screen and (max-width: 794px) {
  .attribute-filter {
    display: flex;
    flex-direction: column;
    gap: 5px;
  }

  .attribute-filter .attributes-categories {
    display: flex;
    gap: 5px;
  }

  .separator {
    display: none;
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

@media screen and (max-width: 405px) {
  .page-title {
    font-size: 2.5em;
  }

  .attribute-filter .attributes-categories {
    flex-direction: column;
    gap: 5px;
  }

  .button-text {
    display: none;
  }

  .button-icon {
    margin-right: 0;
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