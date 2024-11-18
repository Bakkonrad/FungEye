<template>
  <div v-if="isLoading" class="loading">
    <LoadingSpinner />
  </div>
  <div v-else class="container-md">
    <div class="button-container">
      <button class="btn fungeye-default-button" @click="goToPortal">
        <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon"></font-awesome-icon>
        Powrót do portalu</button>
    </div>

    <!-- Post -->
    <Post :id="post.id" :userId="post.userId" :content="post.content" :image="post.imageUrl"
      :num-of-likes="post.likeAmount" :num-of-comments="post.commentsAmount" :created-at="post.createdAt"
      :is-liked="post.loggedUserReacted" :detailsView="true" @edit="editPost" @delete="deletePost"></Post>

    <!-- Comments -->
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
            <div class="comment-author-info" @click="goToProfile(comment.user.id)">
              <ProfileImage :imgSrc="comment.user.imageUrl" />
              <p class="username">{{ comment.user.username }}</p>
            </div>
            <span class="buttons">
              <button v-if="!checkCommentAuthor(comment.user.id)" class="btn" @click="report(comment.id)">
                <font-awesome-icon icon="fa-solid fa-flag"
                  :class="reportedComment == comment.id ? 'reported' : ''"></font-awesome-icon>
              </button>
              <button v-if="checkCommentAuthor(comment.user.id) || isAdmin" class="btn"
                @click="deleteComment(comment.id)">
                <font-awesome-icon icon="fa-solid fa-trash"></font-awesome-icon>
              </button>
              <button v-if="checkCommentAuthor(comment.user.id)" class="btn"
                @click="commentToEdit = comment.id; commentContentToEdit = comment.content">
                <font-awesome-icon icon="fa-solid fa-pen"></font-awesome-icon>
              </button>
            </span>
          </span>
          <p class="comment-text">{{ comment.content }}</p>
          <input v-if="commentToEdit === comment.id" v-model="commentContentToEdit" type="text">
          <button v-if="commentToEdit === comment.id" class="btn fungeye-default-button"
            @click="editComment(comment.id)">Zapisz</button>
        </div>
      </div>
      <div v-else class="no-comments">
        <p>Brak komentarzy</p>
      </div>
    </div>

    <!-- Edit post modal -->
    <div v-if="showEditModal" class="modal">
      <div class="modal-content">
        <h2>Edytuj post</h2>
        <textarea v-model="postContentToEdit" placeholder="Zawartość posta" class="edit-input form-control"></textarea>

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
          <div v-if="postImageToEdit.name != ''" class="container uploaded-images">
            <div class="image">
              <span class="delete" @click="deleteImage()">&times;</span>
              <img :src="postImageToEdit.url" alt="Zdjęcie posta" class="uploaded-image" />
            </div>
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
import PostService from "@/services/PostService";
import Post from "../components/Post.vue";
import { checkAdmin, isAdmin, profileImage } from "@/services/AuthService";
import LoadingSpinner from "@/components/LoadingSpinner.vue";

export default {
  components: {
    ProfileImage,
    Post,
    LoadingSpinner,
  },
  props: {
    reportedCommentId: {
      type: Number,
      default: null,
    },
    id: {
      type: Number,
    },
  },
  data() {
    return {
      newCommentContent: "",
      error: false,
      errorMessage: "",
      comments: [],
      post: {
        id: 0,
        userId: 0,
        content: "",
        imageUrl: "",
        likeAmount: 0,
        commentsAmount: 0,
        createdAt: "",
        loggedUserReacted: false,
      },
      isDragging: false,
      showEditModal: false,
      isAuthor: false,
      isAdmin: false,
      commentToEdit: false,
      postContentToEdit: "",
      isAuthorOfComment: false,
      commentContentToEdit: "",
      imgSrc: profileImage,
      file: "",
      postImageToEdit: {
        name: "",
        url: "",
      },
      reportedComment: this.reportedCommentId,
      isLoading: false,
    };
  },
  mounted() {
    this.getPost();
    this.checkAuthor();
    checkAdmin();
    this.isAdmin = isAdmin;
  },
  methods: {
    async getPost() {
      this.isLoading = true;
      try {
        const response = await PostService.getPost(this.id);
        if (response.success === false) {
          console.error("Error while fetching post data");
          return;
        }
        this.post = response.data;
        if (response.data.comments) {
          this.comments = response.data.comments;
        }
      }
      catch (error) {
        console.error(error);
      }
      this.isLoading = false;
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
      const comment = {
        content: this.newCommentContent,
        postId: this.post.id,
      };
      const response = await PostService.addComment(comment);
      if (response.success === false) {
        console.error("Error while adding comment");
        return;
      }
      this.error = false;
      this.newCommentContent = "";
      this.getPost();
    },
    async deleteComment(commentId) {
      const response = await PostService.deleteComment(commentId);
      if (response.success === false) {
        alert("Nie udało się usunąć komentarza. Spróbuj ponownie później");
        return;
      }
      alert("Usunięto komentarz");
      this.getPost();
    },
    async editComment(commentId) {
      if (this.commentContentToEdit.trim() === "") {
        this.error = true;
        this.errorMessage = "Komentarz nie może być pusty";
        return;
      }
      const newComment = {
        content: this.commentContentToEdit,
        postId: this.post.id,
        id: commentId,
      };
      const response = await PostService.editComment(newComment);
      if (response.success === false) {
        alert("Nie udało się edytować komentarza. Spróbuj ponownie później");
        return;
      }
      alert("Edytowano komentarz");
      this.commentToEdit = '';
      this.getPost();
    },
    editPost() {
      this.showEditModal = true;
      this.postContentToEdit = this.post.content;
      if (this.post.image == "" || this.post.image == null) {
        this.postImageToEdit = {
          name: "",
          url: "",
        };
      }
      else {
        this.postImageToEdit = {
          name: this.post.image,
          url: this.post.image,
        };
      }
    },
    async deletePost() {
      if (confirm("Czy na pewno chcesz usunąć ten post?")) {
        const response = await PostService.deletePost(this.post.id);
        if (response.success === false) {
          alert("Nie udało się usunąć posta. Spróbuj ponownie później");
          return;
        }
        alert("Usunięto post");
        this.$router.push({ name: 'portal' });
      }
    },
    async savePost() {
      if (this.postContentToEdit.trim() === "" || this.post.image === "") {
        alert("Post nie może być pusty");
        return;
      }
      const editedPost = {
        id: this.post.id,
        content: this.postContentToEdit,
      };
      const response = await PostService.editPost(editedPost, this.file);
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
      this.file = file;
      this.postImageToEdit = {
        name: file.name,
        url: URL.createObjectURL(file),
      };
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
      this.file = file;
      this.postImageToEdit = {
        name: file.name,
        url: URL.createObjectURL(file),
      };
    },
    goToProfile(id) {
      this.$router.push({ name: 'userProfile', params: { id: id } });
    },
    checkAuthor() {
      this.isAuthor = this.userId == localStorage.getItem("id");
    },
    checkCommentAuthor(commentAuthorId) {
      return commentAuthorId == localStorage.getItem("id");
    },
    deleteImage() {
      this.file = "";
      this.postImageToEdit = {
        name: "",
        url: "",
      };
    },
    async report(commentId) {
      try {
        const response = await PostService.report(this.id, commentId);
        if (response.success === false) {
          console.error("Error while reporting comment");
          return;
        }
        this.reportedComment = commentId;
        alert("Zgłoszono komentarz");
      }
      catch (error) {
        console.error(error);
      }
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

.container .image {
  position: relative;
  width: 200px;
  height: 200px;
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

.uploaded-image {
  margin-top: 10px;
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 10px;
}

.container .image .delete {
  z-index: 999;
  position: absolute;
  top: 10px;
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

.reported {
  color: var(--red);
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
