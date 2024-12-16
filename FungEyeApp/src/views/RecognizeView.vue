<template>
  <div class="container-md">
    <h1 class="page-title">Rozpoznawanie grzybów</h1>
    <div class="button-container">
      <button type="button" class="btn fungeye-default-button" @click="toggleDirections">{{ directionsState }}</button>
    </div>
    <div v-if="directionsState === 'Zwiń instrukcję'" class="directions-column">
      <div class="directions">
        <h2>Instrukcja</h2>
        <div class="directions-text">
          <p>
            1. Aby rozpocząć proces rozpoznawania grzybów, wybierz zdjęcie grzyba z dysku lub zrób zdjęcie za pomocą
            kamery.
          </p>
          <p>
            <b>Uwaga!</b> Do jednej analizy należy wybrać zdjęcia tylko jednego grzyba. Inaczej wyniki mogą być
            nieprawidłowe.
          </p>
          <p>
            2. Po wybraniu zdjęcia, kliknij przycisk "Rozpoznaj". Po chwili dostaniesz prawdopodobne wyniki
            rozpoznania.
            Liczba oznacza procent na ile algorytm jest pewny, że to dany gatunek grzyba.
          </p>
          <ul class="list-of-colors">
            <li class="green">kolor zielony: powyżej 80% pewności</li>
            <li class="orange">kolor żółty: powyżej 50% pewności</li>
            <li class="red">kolor czerwony: <u>poniżej</u> 50% pewności </li>
          </ul>
          <p>
            <b>Uwaga!</b> Wyniki mogą być niedokładne. W celu uzyskania pewniejszych wyników, skonsultuj się z
            ekspertem.
          </p>
          <p>
            3. Jeśli chcesz dowiedzieć się więcej o danym gatunku grzyba, kliknij na nazwę gatunku w wynikach
            rozpoznania.
          </p>
        </div>
      </div>
    </div>
    <div class="container-md content">
      <div class="photo-upload" :class="showResult ? 'result-chosen-files' : ''">
        <div class="card">
          <input style="display: none" type="file" @change="onFileChange" ref="fileInput" />
          <div v-if="showResult" class="back-to-file-upload">
            <button type="button" class="btn fungeye-secondary-button" @click="clearImages">Wybierz inne
              zdjęcie</button>
          </div>
          <div v-if="!showResult" class="drag-area" @click="$refs.fileInput.click()" @dragover.prevent="onDragOver"
            @dragleave.prevent="onDragLeave" @drop.prevent="onDrop">
            <span class="mobile-photo-upload">
              <span class="select">Kliknij aby zrobić lub dodać zdjęcie</span>
            </span>
            <span class="desktop-photo-upload">
              <span v-if="!isDragging">
                <header>Przeciągnij zdjęcie tutaj</header>
                <span class="select">lub kliknij, aby wybrać plik</span>
              </span>
              <span v-else>
                <header>Upuść zdjęcie tutaj</header>
                <br />
              </span>
            </span>
          </div>
          <h3 v-if="images.length > 0" class="chosen-files-header">
            Wybrane zdjęcie:
          </h3>
          <div class="container uploaded-images" :class="images.length > 0 ? 'images-uploaded' : ''">
            <div class="image" v-for="(image, index) in images" :key="index">
              <span class="delete" @click="deleteImage(index)">&times;</span>
              <img :src="image.url" alt="uploaded image" />
            </div>
          </div>
        </div>
        <div class="error mt-2">
          <span v-if="imagesUploaded === false" class="error-message">Nie wybrano zdjęcia.</span>
          <span v-if="fileTypeError === true" class="error-message">Wybrano plik, który nie jest zdjęciem.</span>
          <span v-if="errorRecognizing === true" class="error-message">Błąd podczas rozpoznawania. Spróbuj
            ponownie.</span>
          <span v-if="fileSizeError === true" class="error-message">Przesłano za duży plik. Maksymalny rozmiar to
            10MB.</span>
        </div>
        <div v-if="!showResult" class="recognize-button">
          <button @click="recognize" class="btn fungeye-default-button" id="upload-button">
            Rozpoznaj
          </button>
        </div>
      </div>
      <LoadingSpinner v-if="isLoading"></LoadingSpinner>
      <RecognizeResult v-if="showResult && images.length > 0" :image="images[0].url" :results="predictingResults">
      </RecognizeResult>
    </div>
  </div>
</template>

<script>
import RecognizeResult from "@/components/RecognizeResult.vue";
import LoadingSpinner from "@/components/LoadingSpinner.vue";
import FungiService from "@/services/FungiService";

export default {
  components: {
    RecognizeResult,
    LoadingSpinner,
  },
  data() {
    return {
      images: [],
      file: null,
      isDragging: false,
      showResult: false,
      imagesUploaded: null,
      id: 2,
      isLoading: false,
      predictingResults: [],
      directionsState: "Pokaż instrukcję",
      maxFileSize: 20 * 1024 * 1024, // 20MB
      fileSizeError: false,
      fileTypeError: false,
      errorRecognizing: false,
    };
  },
  methods: {
    checkIfImage(file) {
      return file.type.startsWith("image/");
    },
    onFileChange(event) {
      this.fileTypeError = false;
      const files = event.target.files;
      if (files.length === 0) return;
      for (let i = 0; i < files.length; i++) {
        if (this.checkFileSize(files[i]) === false) {
          return;
        };
        if (!this.checkIfImage(files[i])) {
          this.fileTypeError = true;
          return;
        }
        if (!this.images.some((image) => image.name === files[i].name)) {
          this.images.push({
            name: files[i].name,
            url: URL.createObjectURL(files[i]),
          });
        }
      }
      this.file = files[0];
      this.imagesUploaded = true;
    },
    checkFileSize(file) {
      if (file.size > this.maxFileSize) {
        this.fileSizeError = true;
        return false;
      }
      else {
        this.fileSizeError = false;
      }
    },
    deleteImage(index) {
      this.images.splice(index, 1);
      if (this.images.length === 0) {
        this.showResult = false;
        this.errorRecognizing = false;
      }
    },
    onDragOver(event) {
      event.preventDefault();
      this.isDragging = true;
      event.dataTransfer.dropEffect = "copy";
    },
    onDragLeave(event) {
      event.preventDefault();
      this.isDragging = false;
    },
    onDrop(event) {
      this.fileTypeError = false;
      event.preventDefault();
      this.isDragging = false;
      const files = event.dataTransfer.files;
      if (files.length === 0) return;
      for (let i = 0; i < files.length; i++) {
        if (this.checkFileSize(files[i]) === false) {
          return;
        };
        if (!this.checkIfImage(files[i])) {
          this.fileTypeError = true;
          return;
        }
        if (!this.images.some((image) => image.name === files[i].name)) {
          this.images.push({
            name: files[i].name,
            url: URL.createObjectURL(files[i]),
          });
        }
      }
      this.file = files[0];
    },
    async recognize() {
      if (this.images.length === 0) {
        this.imagesUploaded = false;
        return;
      }
      this.isLoading = true;
      const response = await FungiService.predict(this.file);
      if (response.success == false || response.data.length === 0) {
        this.errorRecognizing = true;
        this.isLoading = false;
        return;
      }
      this.predictingResults = response.data.slice(0, 3);
      this.showResult = true;
      this.isLoading = false;

    },
    toggleDirections() {
      this.directionsState = this.directionsState === "Pokaż instrukcję" ? "Zwiń instrukcję" : "Pokaż instrukcję";
    },
    clearImages() {
      this.images = [];
      this.imagesUploaded = null;
      this.predictingResults = [];
      this.showResult = false;
    },
  },
};
</script>

<style scoped>
.button-container {
  display: flex;
  justify-content: center;
  margin: 20px 0;
}

.recognize-content {
  display: flex;
  justify-content: center !important;
  align-items: center;
  flex-direction: row;
  gap: 2em;
  width: 100%;
}

.content {
  display: flex;
  justify-content: center;
  gap: 2em;
}

.directions-column {
  display: flex;
  background-color: rgba(0, 0, 0, 0.9);
  color: white;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  gap: 2em;
  width: 100%;
  border: #ccc 1px solid;
  border-radius: 10px;
  padding: 20px;
  margin: 20px 0;
}

.directions {
  margin-top: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.list-of-colors {
  font-weight: 500;
}

.red {
  color: var(--light-red);
}

.orange {
  color: var(--warning);
}

.green {
  color: var(--lighter-green);
}

.photo-upload {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 60%;
}

.card {
  background: radial-gradient(135.63% 132.41% at 149.88% 23.51%,
      var(--green) 50%,
      var(--dark-green) 100%);
  width: 100%;
  height: 400px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  padding: 20px;
  margin-top: 20px;
  color: white;
  overflow: hidden;
}

.result-chosen-files {
  height: 25rem;
  width: 20rem;
}

.mobile-photo-upload {
  display: none;
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
  height: 300px;
}

.drag-area header {
  font-size: 1.5em;
  font-weight: bold;
}

.select {
  margin-left: 5px;
  transition: 0.4s;
}

.select:hover {
  opacity: 0.6;
}

.chosen-files-header {
  margin-top: 20px;
}

.container {
  display: flex;
  justify-content: flex-start;
  align-items: flex-start;
  flex-wrap: wrap;
  margin-bottom: 8px;
  margin-top: 20px;
  width: 100%;
}

.images-uploaded {
  height: 250px;
}

.container .image {
  position: relative;
  width: 100px;
  height: 100px;
  margin-right: 10px;
  margin-bottom: 10px;
  border-radius: 10px;
  overflow: hidden;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.uploaded-images {
  display: flex;
  flex-wrap: wrap;
  overflow: auto;
}

.container .image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 10px;
}

.container .image .delete {
  z-index: 999;
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

.container .image .delete:hover {
  background-color: rgba(0, 0, 0, 0.8);
}

.card input,
.card .drag-area .on-drop,
.card .drag-area.dragover .visible {
  display: none;
}

.recognize-button {
  margin-top: 20px;
  width: 100%;
}

.recognize-button button {
  width: 100% !important;
}

@media screen and (max-width: 1400px) {
  .photo-upload {
    width: 70%;
  }

  .result-chosen-files {
    height: 20rem;
    width: 15rem;
  }
}

@media screen and (max-width: 992px) {

  .photo-upload {
    width: 90%;
  }

  .result-chosen-files {
    height: 20rem;
    width: 15rem;
  }

}

@media screen and (max-width: 768px) {
  .page-title {
    font-size: 2em;
  }

  .container-md {
    flex-direction: column;
    gap: 1em;
  }

  .photo-upload {
    width: 100%;
  }

  .mobile-photo-upload {
    display: block;
  }

  .desktop-photo-upload {
    display: none;
  }

}
</style>
