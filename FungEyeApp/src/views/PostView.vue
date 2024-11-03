<template>
  <div class="container-md">
    <div class="button-container">
      <button class="btn fungeye-default-button" @click="goToPortal">
        <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon"></font-awesome-icon>
        Powrót do portalu</button>
    </div>

    <Post :id="post.id" :userId="post.userId" :content="post.content" :image="post.image" :numOfLikes="post.numOfLikes" :is-liked="post.isLiked"
      :detailsView="true" @edit="editPost" @delete="deletePost"></Post>

    <div class="card-footer comment-section">
      <div class="add-comment">
        <ProfileImage :imgSrc="imgSrc" class="profile-image" />
        <textarea v-model="newCommentContent" class="form-control textarea" id="exampleFormControlTextarea1"
          placeholder="Dodaj komentarz do posta..."></textarea>
        <button class="btn fungeye-default-button publish-button" @click="addComment">Dodaj</button>
      </div>
      <div v-if="error" class="error-message">
        {{ errorMessage }}
      </div>
      <div v-if="comments.length > 0" class="all-comments">
        <div class="comment" v-for="comment in comments" :key="comment.id">
          <span class="header">
            <div class="comment-author-info" @click="goToProfile(comment.authorId)">
              <ProfileImage :imgSrc="comment.imgSrc" />
              <p class="username">{{ comment.username }}</p>
            </div>
            <span class="buttons">
              <button v-if="!isAuthor" class="btn" @click="reportUser">
                <font-awesome-icon icon="fa-solid fa-flag"></font-awesome-icon>
              </button>
              <button v-if="isAuthor || isAdmin" class="btn" @click="deleteComment">
                <font-awesome-icon icon="fa-solid fa-trash"></font-awesome-icon>
              </button>
              <button v-if="isAuthor" class="btn" @click="editCommentMode = true">
                <font-awesome-icon icon="fa-solid fa-pen"></font-awesome-icon>
              </button>
            </span>
          </span>
          <p class="comment-text">{{ comment.content }}</p>
          <input v-if="editCommentMode" v-model="comment.content" type="text">
          <button v-if="editCommentMode" class="btn fungeye-default-button" @click="editComment">Zapisz</button>
        </div>
      </div>
      <div v-else class="no-comments">
        <p>Brak komentarzy</p>
      </div>
    </div>
    <div v-if="showEditModal" class="modal">
      <div class="modal-content">
        <h2>Edytuj post</h2>
        <textarea v-model="post.content" placeholder="Zawartość posta" class="edit-input form-control"></textarea>

        <div class="photo-upload">
          <input style="display: none" type="file" accept="image/*" @change="onFileChange" ref="fileInput" />
          <div class="drag-area" @click="$refs.fileInput.click()" @dragover.prevent="onDragOver"
            @dragleave.prevent="onDragLeave" @drop.prevent="onDrop">
            <span v-if="!isDragging">
              <header v-if="!post.image">Kliknij, aby wybrać zdjęcie do posta</header>
              <header v-else>Kliknij, aby wybrać inne zdjęcie do posta</header>
              <span class="select">lub przeciągnij je tutaj</span>
            </span>
            <span v-else>
              <header>Upuść zdjęcie tutaj</header>
            </span>
          </div>
          <div>
            <img v-if="post.image" :src="post.image.url" alt="Zdjęcie posta" class="uploaded-image" />
          </div>
        </div>

        <hr>
        <span class="modal-buttons">
          <button class="btn fungeye-default-button" @click="savePost">Zapisz</button>
          <button class="btn fungeye-red-button" @click="closeModal">Anuluj</button>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "../components/ProfileImage.vue";
import UserService from "@/services/UserService";
import PostService from "@/services/PostService";
import Post from "../components/Post.vue";
import { checkAdmin, isAdmin } from "@/services/AuthService";

export default {
  components: {
    ProfileImage,
    Post,
  },
  data() {
    return {
      newCommentContent: "",
      imgSrc: "",
      username: "",
      userId: 0,
      id: 1,
      error: false,
      errorMessage: "",
      comments: [],
      post: {},
      isDragging: false,
      showEditModal: false,
      numOfComments: 0,
      isAuthor: false,
      isAdmin: false,
      editCommentMode: false,
    };
  },
  mounted() {
    this.getComments();
    this.getAuthorData();
    this.checkAuthor();
    checkAdmin();
    this.isAdmin = isAdmin;
  },
  created() {
    if (this.$route.query.post) {
      this.post = JSON.parse(this.$route.query.post);
    }
    console.log(this.post);
  },
  methods: {
    async getComments() {
      const response = await PostService.getComments(this.id);
      if (response.success === false) {
        console.error("Error while fetching comments data");
        return;
      }
      if (response.data.length === 0) {
        this.comments = [];
        return;
      }
      this.comments = response.data;
      this.numOfComments = this.comments.length;
    },
    async getAuthorData() {
      const response = await UserService.getUserData(this.post.userId);
      if (response.success === false) {
        console.error("Error while fetching user data");
        return;
      }
      this.imgSrc = response.data.imageUrl;
      this.username = response.data.username;
    },
    goToPortal() {
      this.$router.push({ name: 'portal' });
    },
    async addComment() {
      if (this.newCommentContent.trim() === "") {
        this.error = true;
        this.errorMessage = "Komentarz nie może być pusty";
        return;
      }
      const response = await PostService.addComment(this.id, this.newCommentContent);
      if (response.success === false) {
        console.error("Error while adding comment");
        return;
      }
      this.error = false;
      this.newCommentContent = "";
      this.getComments();
    },
    async deleteComment() {
      const response = await PostService.deleteComment(this.id);
      if (response.success === false) {
        alert("Nie udało się usunąć komentarza. Spróbuj ponownie później");
        return;
      }
      alert("Usunięto komentarz");
    },
    async editComment(comment) {
      console.log(comment);
      const response = await PostService.editComment(this.id, comment);
      if (response.success === false) {
        alert("Nie udało się edytować komentarza. Spróbuj ponownie później");
        return;
      }
      alert("Edytowano komentarz");
    },
    reportUser() {
      alert("Zgłoszono użytkownika");
    },
    editPost() {
      this.showEditModal = true;
    },
    async deletePost() {
      if (confirm("Czy na pewno chcesz usunąć ten post?")) {
        const response = await PostService.deletePost(this.id);
        if (response.success === false) {
          alert("Nie udało się usunąć posta. Spróbuj ponownie później");
          return;
        }
        alert("Usunięto post");
        this.$router.push({ name: 'portal' });
      }
    },
    async savePost() {
      if (this.post.content.trim() === "" || this.post.image === "") {
        alert("Post nie może być pusty");
        return;
      }
      const response = await PostService.editPost(this.id, this.post);
      if (response.success === false) {
        alert("Nie udało się zapisać zmian");
        return;
      }
      alert("Zapisano zmiany");
      this.showEditModal = false;
    },
    closeModal() {
      this.showEditModal = false;
    },
    onFileChange(e) {
      const file = e.target.files[0];
      this.post.image = URL.createObjectURL(file);
    },
    onDragOver(e) {
      e.preventDefault();
      this.isDragging = true;
    },
    onDragLeave(e) {
      e.preventDefault();
      this.isDragging = false;
    },
    onDrop(e) {
      e.preventDefault();
      this.isDragging = false;
      const file = e.dataTransfer.files[0];
      this.post.image = URL.createObjectURL(file);
    },
    goToProfile(id) {
      this.$router.push({ name: 'userProfile', params: { id: id } });
    },
    checkAuthor() {
      this.isAuthor = this.userId == localStorage.getItem("id");
    },
  }
};
</script>

<style scoped>
.container-md {
  max-width: 800px;
  margin: 0 auto;
}

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
  background-color: var(--beige);
  padding: 20px;
  border-radius: 10px;
  width: 450px;
}

.modal-content {
  background-color: var(--beige);
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
  margin-bottom: 15px;
}

.drag-area {
  width: 100%;
  border: 2px dashed #ccc;
  background: rgba(255, 255, 255, 0.3) !important;
  color: rgba(0, 0, 0, 0.572) !important;
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

.modal-buttons {
  display: flex;
  justify-content: space-between;
  gap: 10px;
}

.modal-buttons button {
  width: 100%;
}

.card {
  margin: 2rem 0;
  border-radius: 0.5rem;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  background-color: var(--beige);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.card-footer {
  display: flex;
  justify-content: center;
  background-color: var(--beige);
}

.comments {
  display: flex;
  flex-direction: row;
  align-items: baseline;
  justify-content: first baseline !important;
}

.comment-section {
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 1rem;
}

.add-comment {
  width: 100%;
  display: flex;
  gap: 0.7rem;
  margin: 1rem 0;
  justify-content: center;
  align-items: center;
}

.add-comment .textarea {
  background-color: var(--beige) !important;
  resize: none;
  overflow: hidden;
}

.add-comment .textarea:focus {
  border-color: var(--green);
  color: var(--black) !important;
}

.all-comments {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 100%;
}

.no-comments {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100px;
}

.comment-author-info {
  display: flex;
  align-items: last baseline;
  gap: 10px;
  cursor: pointer;
}

.comment {
  display: flex;
  border: 1px solid var(--dark-beige);
  border-radius: 15px;
  padding: 1rem;
  gap: 1rem;
  width: 100%;
  flex-direction: column;
}

.comment-text {
  margin: 0;
}

@media screen and (max-width: 556px) {
  .add-comment {
    flex-direction: column;
    gap: 0.5rem;
  }

  .publish-button {
    width: 100%;
  }

  .comment {
    padding: 0.5rem;
  }

}
</style>
