<template>
  <div class="add-post">
    <!-- Obrazek profilowy użytkownika -->
    <ProfileImage
      imgSrc="https://fastly.picsum.photos/id/40/4106/2806.jpg?hmac=MY3ra98ut044LaWPEKwZowgydHZ_rZZUuOHrc3mL5mI"
    ></ProfileImage>

    <!-- Pole tekstowe do wpisania treści posta -->
    <textarea
      v-model="content"
      class="form-control textarea"
      id="exampleFormControlTextarea1"
      placeholder="Podziel się swoim koszykiem..."
      @input="autoResize"
    ></textarea>

    <!-- Sekcja przeciągania/upuszczania lub wyboru zdjęć -->
    <div class="drag-area"
         @click="$refs.fileInput.click()"
         @dragover.prevent="onDragOver"
         @dragleave.prevent="onDragLeave"
         @drop.prevent="onDrop">
      <span v-if="!isDragging">
        <header>Przeciągnij zdjęcie tutaj</header>
        <span class="select">lub kliknij, aby wybrać plik</span>
      </span>
      <span v-else>
        <header>Upuść zdjęcie tutaj</header>
      </span>
      <input
        ref="fileInput"
        type="file"
        class="form-control file-input"
        @change="onFileChange"
        style="display: none;"
        accept="image/*"
        multiple
      />
    </div>

    <!-- Podgląd wybranych zdjęć -->
    <div v-if="images.length > 0" class="image-preview">
      <div v-for="(image, index) in images" :key="index" class="image-box">
        <span class="delete" @click="deleteImage(index)">&times;</span>
        <img :src="image.url" alt="uploaded image" />
      </div>
    </div>

    <!-- Sekcja przycisków -->
    <div class="post-actions">
      <button class="btn fungeye-default-button" @click="publishPost">Opublikuj</button>
    </div>
  </div>
</template>

<script>
import ProfileImage from "./ProfileImage.vue";

export default {
  components: {
    ProfileImage,
  },
  data() {
    return {
      content: '', // Przechowywanie treści posta
      images: [],  // Przechowywanie przesłanych zdjęć
      isDragging: false,  // Status przeciągania plików
    };
  },
  methods: {
    // Obsługa przeciągania plików
    onDragOver(event) {
      this.isDragging = true;
      event.dataTransfer.dropEffect = "copy";
    },
    onDragLeave() {
      this.isDragging = false;
    },
    onDrop(event) {
      this.isDragging = false;
      const files = event.dataTransfer.files;
      this.handleFileSelection(files);
    },
    onFileChange(event) {
      const files = event.target.files;
      this.handleFileSelection(files);
    },
    handleFileSelection(files) {
      for (let i = 0; i < files.length; i++) {
        if (!this.images.some((image) => image.name === files[i].name)) {
          this.images.push({
            name: files[i].name,
            url: URL.createObjectURL(files[i]),
          });
        }
      }
    },
    deleteImage(index) {
      this.images.splice(index, 1);
    },
    publishPost() {
      if (this.content || this.images.length) {
        this.$emit('add-post', { content: this.content, images: this.images });
        this.content = '';
        this.images = [];
        this.$refs.fileInput.value = ''; // Czyszczenie inputu do pliku
      } else {
        alert('Nie można opublikować pustego posta');
      }
    },
    autoResize(event) {
      const textarea = event.target;
      textarea.style.height = 'auto';
      textarea.style.height = textarea.scrollHeight + 'px';
    }
  }
};
</script>

<style scoped>
.add-post {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin: 1rem;
  padding: 1rem;
  border-radius: 0.5rem;
}

.add-post .textarea {
  width: 50rem;
  margin: 0.5rem;
  background-color: var(--beige) !important;
  resize: none; /* Wyłączenie możliwości zmiany rozmiaru przez użytkownika */
  overflow: hidden; /* Ukrycie paska przewijania */
}

.add-post .textarea:focus {
  border-color: var(--green);
  background-color: var(--dark-beige) !important;
  color: var(--black) !important;
}

.drag-area {
  display: flex;
  border: 2px dashed #ccc;
  border-radius: 10px;
  padding: 20px;
  text-align: center;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: 0.4s;
  width: 100%;
  max-width: 600px;
  margin-top: 1rem;
}

.drag-area header {
  font-size: 1.2em;
  font-weight: bold;
}

.select {
  margin-left: 5px;
  transition: 0.4s;
}

.image-preview {
  display: flex;
  justify-content: flex-start;
  align-items: flex-start;
  flex-wrap: wrap;
  margin-top: 1rem;
}

.image-box {
  position: relative;
  width: 100px;
  height: 100px;
  margin-right: 10px;
  margin-bottom: 10px;
  border-radius: 10px;
  overflow: hidden;
}

.image-box img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 10px;
}

.image-box .delete {
  position: absolute;
  top: 0;
  right: 0;
  width: 20px;
  height: 20px;
  background-color: rgba(0, 0, 0, 0.5);
  color: white;
  font-size: 1.2em;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.image-box .delete:hover {
  background-color: rgba(0, 0, 0, 0.8);
}

.post-actions {
  display: flex;
  justify-content: flex-end;
  width: 100%;
  margin-top: 1rem;
}

.add-post button {
  width: auto;
  padding: 0.5rem 1rem;
}
</style>