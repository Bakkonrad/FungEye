<template>
  <div class="add-post">
    <div class="centered">
      <ProfileImage :imgSrc="imgSrc">
      </ProfileImage>
      <textarea v-model="content" class="form-control textarea" id="exampleFormControlTextarea1"
        placeholder="Podziel się swoim koszykiem..." @input="autoResize"></textarea>
      <div v-if="!image || image.name == ''" class="drag-area" @click="$refs.fileInput.click()"
        @dragover.prevent="onDragOver" @dragleave.prevent="onDragLeave" @drop.prevent="onDrop">
        <span v-if="!isDragging" class="drag-text">
          <header>Przeciągnij zdjęcie tutaj</header>
          <span class="select">lub kliknij, aby wybrać plik</span>
        </span>
        <span v-else>
          <header>Upuść zdjęcie tutaj</header>
        </span>
        <input ref="fileInput" type="file" class="form-control file-input" @change="onFileChange" style="display: none;"
          accept="image/*" />
      </div>
    </div>
    <div v-if="image && image.name != ''" class="image-preview">
      <div class="image-box">
        <span class="delete" @click="deleteImage">&times;</span>
        <img :src="image.url" alt="uploaded image" />
      </div>
    </div>
  </div>
  <div class="post-actions">
    <button class="btn fungeye-default-button publish-button" @click="publishPost">Opublikuj</button>
  </div>
  <div v-if="error" class="error-message">
    {{ errorMessage }}
  </div>
</template>

<script>
import PostService from "@/services/PostService";
import ProfileImage from "./ProfileImage.vue";
import AuthService from "@/services/AuthService";
import { profileImage } from "@/services/AuthService";

export default {
  components: {
    ProfileImage,
  },
  emits: ['post-added'],
  data() {
    return {
      content: '',
      image: {
        name: '',
        url: '',
      },
      isDragging: false,
      error: null,
      errorMessage: '',
      imgSrc: profileImage,
    };
  },
  methods: {
    onDragOver(event) {
      this.isDragging = true;
      event.dataTransfer.dropEffect = "copy";
    },
    onDragLeave() {
      this.isDragging = false;
    },
    onDrop(event) {
      this.isDragging = false;
      const file = event.dataTransfer.files[0];
      this.handleFileSelection(file);
    },
    onFileChange(event) {
      const file = event.target.files[0];
      this.handleFileSelection(file);
    },
    handleFileSelection(file) {
      if (this.image.name !== file.name) {
        this.image = {
          name: file.name,
          url: URL.createObjectURL(file),
        };
      }
    },
    deleteImage() {
      this.image = '';
    },
    async publishPost() {
      if (this.content.trim() == '' && this.image.name == '') {
        alert('Nie można opublikować pustego posta');
        return;
      }

      const post = {
        id: 0,
        content: this.content,
        // image: this.image,
        userId: parseInt(localStorage.getItem("id")),
      };

      // console.log(post);
      const response = await PostService.addPost(post, this.image);
      // const response = { success: true };
      if (response.success == false) {
        this.error = true;
        this.errorMessage = response.message;
        console.log(response.message);
        return;
      }
      this.$emit('post-added', post);
      this.content = '';
      this.image = {
        name: '',
        url: '',
      };
    },
    autoResize(event) {
      const textarea = event.target;
      textarea.style.height = 'auto';
      textarea.style.height = textarea.scrollHeight + 'px';
    },
  }
};
</script>

<style scoped>
.add-post {
  display: flex;
  flex-direction: column;
  justify-content: center;
  margin-top: 1rem;
  padding: 1rem;
  border-radius: 0.5rem;
  border: 1px solid var(--black);
  background: radial-gradient(135.63% 132.41% at 149.88% 23.51%,
      var(--green) 50%,
      var(--dark-green) 100%);
}

.add-post .centered {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
}

.add-post .textarea {
  /* width: 50rem; */
  margin: 0.5rem;
  background-color: var(--beige) !important;
  resize: none;
  overflow: hidden;
}

.add-post .textarea:focus {
  border-color: var(--green);
  /* background-color: var(--dark-beige) !important; */
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
  margin: 1rem 0;
}

.drag-area .drag-text {
  color: white
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
}

.publish-button {
  margin: 0 0 1.5rem 0;
}

@media screen and (max-width: 710px) {
  .add-post {
    width: 100%;
  }
}
</style>