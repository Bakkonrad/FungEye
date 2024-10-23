<template>
  <div class="atlas-view">
    <h1>Atlas grzybów</h1>
    <SearchBar @search="handleSearch" />

    <!-- Przycisk dodania nowego grzyba -->
    <div class="button-center">
      <button @click="showAddMushroomModal = true" class="btn-mushroom">Dodaj nowego grzyba</button>
    </div>

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

    <div class="mushroom-list">
      <div v-for="mushroom in filteredMushrooms" :key="mushroom.id" class="mushroom-card">
        <img :src="mushroom.image" alt="Mushroom Image" class="mushroom-image" />
        <div class="mushroom-info">
          <h3>{{ mushroom.name }}</h3>
          <p>{{ mushroom.description }}</p>
          <div class="attributes">
            <span v-for="attr in mushroom.attributes" :key="attr"
              :class="['attribute', attributeClass(attr), { 'active-attribute': isActiveAttribute(attr) }]">
              {{ attr }}
            </span>
          </div>

          <!-- Przycisk edytowania i usuwania grzyba -->
          <div class="mushroom-actions">
            <button @click="editMushroom(mushroom)" class="btn-mushroom">Edytuj</button>
            <button @click="confirmDeleteMushroom(mushroom)" class="btn-mushroom">Usuń</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal dodawania/edycji grzyba -->
    <div v-if="showAddMushroomModal || showEditMushroomModal" class="modal">
      <div class="modal-content">
        <h2>{{ showEditMushroomModal ? 'Edytuj grzyba' : 'Dodaj nowego grzyba' }}</h2>
        <input v-model="mushroomForm.name" type="text" placeholder="Nazwa grzyba" class="edit-input" />
        
        <!-- Dodaj zdjęcie grzyba -->
        <div class="photo-upload">
          <input
            style="display: none"
            type="file"
            accept="image/*"
            @change="onFileChange"
            ref="fileInput"
          />
          <div class="drag-area" @click="$refs.fileInput.click()" @dragover.prevent="onDragOver"
            @dragleave.prevent="onDragLeave" @drop.prevent="onDrop">
            <span v-if="!isDragging">
              <header>Kliknij, aby wybrać zdjęcie grzyba</header>
              <span class="select">lub przeciągnij tutaj</span>
            </span>
            <span v-else>
              <header>Upuść zdjęcie tutaj</header>
            </span>
          </div>
          <div v-if="mushroomForm.image">
            <img :src="mushroomForm.image" alt="Zdjęcie grzyba" class="uploaded-image" />
          </div>
        </div>

        <textarea v-model="mushroomForm.description" placeholder="Opis grzyba" class="edit-input"></textarea>
        <div class="attribute-selection">
          <label v-for="attr in availableAttributes" :key="attr">
            <input type="checkbox" :value="attr" v-model="mushroomForm.attributes" /> {{ attr }}
          </label>
        </div>
        <button @click="saveMushroom">{{ showEditMushroomModal ? 'Zapisz zmiany' : 'Dodaj grzyb' }}</button>
        <button @click="closeModal">Anuluj</button>
      </div>
    </div>

    <!-- Modal potwierdzający usunięcie -->
    <div v-if="showDeleteMushroomModal" class="modal">
      <div class="modal-content">
        <h2>Czy na pewno chcesz usunąć grzyba {{ mushroomToDelete.name }}?</h2>
        <button @click="deleteMushroom">Usuń</button>
        <button @click="closeModal">Anuluj</button>
      </div>
    </div>
  </div>
</template>

<script>
import SearchBar from '@/components/SearchBar.vue';

export default {
  components: {
    SearchBar
  },
  data() {
    return {
      searchQuery: '',
      activeLetter: '',
      selectedAttributes: [],
      mushrooms: [
        {
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
      ],
      alphabet: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split(''),
      availableAttributes: ['iglaste', 'liściaste', 'jadalny', 'niejadalny', 'trujący'],
      showAddMushroomModal: false,
      showEditMushroomModal: false,
      showDeleteMushroomModal: false,

      mushroomForm: {
        name: '',
        image: '',
        attributes: [],
        description: ''
      },
      mushroomToDelete: null,
      editMushroomId: null,
      isDragging: false
    };
  },
  computed: {
    filteredMushrooms() {
      let filtered = this.mushrooms;

      if (this.activeLetter) {
        filtered = filtered.filter((mushroom) => mushroom.name.startsWith(this.activeLetter));
      }

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

      return filtered;
    },
  },
  methods: {
    handleSearch(query) {
      this.searchQuery = query;
    },
    filterByLetter(letter) {
      if (this.activeLetter === letter) {
        this.activeLetter = null;
      } else {
        this.activeLetter = letter;
      }
    },
    resetActiveLetter() {
      this.activeLetter = '';
    },
    openMushroomView(mushroom) {
      this.$router.push({ name: 'MushroomView', params: { id: mushroom.id } });
      console.log('Otwieram kartę grzyba:', mushroom);
    },
    toggleAttributeFilter(attribute) {
      const index = this.selectedAttributes.indexOf(attribute);
      if (index > -1) {
        this.selectedAttributes.splice(index, 1);
      } else {
        this.selectedAttributes.push(attribute);
      }
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
    openMushroomView(mushroom) {
      this.$router.push({ name: 'MushroomView', params: { id: mushroom.id } });
    },
    addMushroom() {
      this.showAddMushroomModal = true;
    },
    editMushroom(mushroom) {
      this.editMushroomId = mushroom.id;
      this.mushroomForm = { ...mushroom };
      this.showEditMushroomModal = true;
    },
    saveMushroom() {
      if (this.showEditMushroomModal) {
        const mushroomIndex = this.mushrooms.findIndex((m) => m.id === this.editMushroomId);
        this.mushrooms.splice(mushroomIndex, 1, { ...this.mushroomForm });
      } else {
        this.mushrooms.push({
          ...this.mushroomForm,
          id: this.mushrooms.length + 1,
        });
      }
      this.closeModal();
    },
    confirmDeleteMushroom(mushroom) {
      this.mushroomToDelete = mushroom;
      this.showDeleteMushroomModal = true;
    },
    deleteMushroom() {
      this.mushrooms = this.mushrooms.filter((m) => m.id !== this.mushroomToDelete.id);
      this.closeModal();
    },
    closeModal() {
      this.showAddMushroomModal = false;
      this.showEditMushroomModal = false;
      this.showDeleteMushroomModal = false;
      this.mushroomForm = {
        name: '',
        image: '',
        attributes: [],
        description: ''
      };
      this.mushroomToDelete = null;
    },
    // Obsługa przesyłania zdjęcia
    onFileChange(event) {
      const file = event.target.files[0];
      if (file) {
        this.mushroomForm.image = URL.createObjectURL(file);
      }
    },
    onDragOver(event) {
      event.preventDefault();
      this.isDragging = true;
    },
    onDragLeave(event) {
      event.preventDefault();
      this.isDragging = false;
    },
    onDrop(event) {
      event.preventDefault();
      this.isDragging = false;
      const file = event.dataTransfer.files[0];
      if (file) {
        this.mushroomForm.image = URL.createObjectURL(file);
      }
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
}

.modal-content {
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  width: 450px;
}

.edit-input {
  color: black !important;
  margin-bottom: 10px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.photo-upload {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  margin-bottom: 10px;
}

.drag-area {
  width: 100%;
  border: 2px dashed #ccc;
  border-radius: 10px;
  padding: 20px;
  text-align: center;
  cursor: pointer;
  transition: 0.4s;
}

.uploaded-image {
  margin-top: 10px;
  width: 200px;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
}

.mushroom-actions {
  position: absolute;
  top: 10px;
  right: 10px;
  display: flex;
  gap: 10px;
}

.btn-mushroom {
  margin: 0;
  padding: 10px;
  background-color: var(--green);
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.button-center {
  display: flex;
  justify-content: center;
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
  flex-wrap: wrap;
  align-items: center;
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
  /* background-color: #f0f0f0; */
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.4);
}

.mushroom-image {
  width: 80px;
  height: 80px;
  margin-right: 20px;
  border-radius: 50%;
}

.mushroom-info {
  flex-grow: 1;
}

.mushroom-info h3 {
  margin: 0;
  font-size: 22px;
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

@media screen and (max-width: 768px) {
  .mushroom-card {
    align-items: flex-start;
    flex-direction: column;
    gap: 15px;
    width: 95%;
  }
}
</style>