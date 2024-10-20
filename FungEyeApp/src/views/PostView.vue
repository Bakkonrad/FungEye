<template>
  <div class="container-md">
    <div class="button-container">
      <button class="btn fungeye-default-button" @click="goToPortal">
        <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon"></font-awesome-icon>
        Powrót do portalu</button>
    </div>
    
    <Post :id="id" :userId="userId" :content="content" :image="image" :numOfLikes="numOfLikes"
      :numOfComments="numOfComments" :detailsView="true"></Post>

    <div class="card-footer comment-section">
      <div class="add-comment">
        <ProfileImage :imgSrc="imgSrc" class="profile-image" />
        <textarea v-model="newCommentContent" class="form-control textarea" id="exampleFormControlTextarea1"
          placeholder="Dodaj komentarz do posta..."></textarea>
        <button class="btn fungeye-default-button publish-button" @click="publishComment">Dodaj</button>
      </div>
      <div v-if="error" class="error-message">
        {{ errorMessage }}
      </div>
      <div class="all-comments">
        <div class="comment" v-for="comment in comments" :key="comment.id">
          <span class="header">
            <div class="comment-author-info">
              <ProfileImage :imgSrc="comment.imgSrc" />
              <p class="username">{{ comment.username }}</p>
            </div>
            <button class="btn" @click="reportUser">
              <font-awesome-icon icon="fa-solid fa-flag" class="button-icon"></font-awesome-icon>
              Zgłoś
            </button>
          </span>
          <p class="comment-text">{{ comment.content }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "../components/ProfileImage.vue";
import UserService from "@/services/UserService";
import Post from "../components/Post.vue";

export default {
  components: {
    ProfileImage,
    Post,
  },
  data() {
    return {
      addComment: false,
      newCommentContent: "",
      isLiked: false,
      imgSrc: "",
      username: "username",
      userId: 2,
      id: 1,
      content: "content",
      image: {
        url: "https://picsum.photos/800/800",
      },
      numOfLikes: 0,
      numOfComments: 0,
      error: false,
      errorMessage: "",
      comments: [
        { id: 1, imgSrc: "", username: "username", content: "content" },
        { id: 2, imgSrc: "", username: "username", content: "content" }],
    };
  },
  mounted() {
    this.getAuthorData();
  },
  methods: {
    async getAuthorData() {
      const response = await UserService.getUserData(this.userId);
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
    toggleLike() {
      this.isLiked = !this.isLiked;
      if (this.isLiked) {
        this.numOfLikes++;

      } else {
        this.numOfLikes--;
      }
    },
    showAddComment() {
      this.addComment = true;
    },
    publishComment() {
      if (this.newCommentContent.trim() === "") {
        this.error = true;
        this.errorMessage = "Komentarz nie może być pusty";
        return;
      }
      this.error = false;
      this.comments.push({
        id: this.comments.length + 1,
        imgSrc: this.imgSrc,
        username: this.username,
        content: this.newCommentContent,
      });
      this.newCommentContent = "";
      this.numOfComments++;
    },
    reportUser() {
      alert("Zgłoszono użytkownika");
    },
  }
};
</script>

<style scoped>
.container-md {
  max-width: 800px;
  margin: 0 auto;
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

.comment-author-info {
  display: flex;
  align-items: last baseline;
  gap: 10px;
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
