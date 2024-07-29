<template>
  <div>
    <div class="container-md">
      <div class="photo-upload">
        <div class="card">
          <input
            style="display: none"
            type="file"
            accept="image/*"
            @change="onFileChange"
            ref="fileInput"
            multiple
          />
          <div
            class="drag-area"
            @click="$refs.fileInput.click()"
            @dragover.prevent="onDragOver"
            @dragleave.prevent="onDragLeave"
            @drop.prevent="onDrop"
          >
            <span v-if="!isDragging">
              <header>Przeciągnij zdjęcie tutaj</header>
              <span class="select">lub kliknij, aby wybrać plik</span>
            </span>
            <span v-else>
              <header>Upuść zdjęcie tutaj</header>
              <br />
            </span>
          </div>
          <h3 v-if="images.length > 0" class="chosen-files-header">Wybrane zdjęcia:</h3>
          <div class="container">
            <div class="image" v-for="(image, index) in images" :key="index">
              <span class="delete" @click="deleteImage(index)">&times;</span>
              <img :src="image.url" alt="uploaded image" />
            </div>
          </div>
        </div>
        <div class="recognize-button">
          <button
            @click="onUpload"
            type="button"
            class="btn fungeye-default-button"
            id="upload-button"
          >
            Rozpoznaj
          </button>
        </div>
      </div>
      <RouterLink :to="'/mushroom/' + id" class="btn fungeye-default-button">Strona grzyba</RouterLink>
      <div v-if="showResult" class="result">
        <h2>Wynik</h2>
        <div class="card">
          <div class="result-image">
            <img :src="images[0]" alt="">
          </div>
          <div class="result-info">
            <h3>Nazwa</h3>
            <p>Grzyb</p>
            <h3>Opis</h3>
            <p>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed
              pulvinar, nunc nec ultricies.
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      images: [],
      isDragging: false,
      showResult: false,
      id: 2,
    };
  },
  methods: {
    onFileChange(event) {
      const files = event.target.files;
      if (files.length === 0) return;
      for (let i = 0; i < files.length; i++) {
        if (!this.images.some((image) => image.name === files[i].name)) {
          this.images.push({
            name: files[i].name,
            url: URL.createObjectURL(files[i]),
          });
        }
      }
      console.log(this.images);
    },
    deleteImage(index) {
      this.images.splice(index, 1);
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
      event.preventDefault();
      this.isDragging = false;
      const files = event.dataTransfer.files;
      if (files.length === 0) return;
      for (let i = 0; i < files.length; i++) {
        if (!this.images.some((image) => image.name === files[i].name)) {
          this.images.push({
            name: files[i].name,
            url: URL.createObjectURL(files[i]),
          });
        }
      }
      console.log(this.images);
    },
    onUpload() {
        this.showResult = true;
      console.log("Upload image");
    },
  },
};
</script>

<style scoped>
.card {
  background: radial-gradient(
      135.63% 132.41% at 149.88% 23.51%,
      var(--green) 50%,
      var(--dark-green) 100%
    )
    /* warning: gradient uses a rotation that is not supported by CSS and may not behave as expected */;

  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  padding: 20px;
  margin-top: 20px;
  color: white;
  overflow: hidden;
}

.drag-area {
  border: 2px dashed #ccc;
  border-radius: 10px;
  padding: 20px;
  text-align: center;
  cursor: pointer;
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
  height: auto;
  max-height: 200px;
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
  display: flex;
  justify-content: flex-end;
  margin-top: 20px;
}

</style>
