<template>
  <div class="card">
    <div class="card-body">
      <span class="header">
        <span class="username-data" @click="goToProfile">
          <ProfileImage :imgSrc="imgSrc" />
          <p class="username">{{ username }}</p>
        </span>
        <span class="buttons">
          <button v-if="!isAuthor" class="btn" @click="report">
            <font-awesome-icon icon="fa-solid fa-flag"
              :class="reportedPostId == id ? 'reported' : ''"></font-awesome-icon>
          </button>
          <button v-if="detailsView && isAuthor" class="btn" @click="$emit('edit')">
            <font-awesome-icon icon="fa-solid fa-pen"></font-awesome-icon>
          </button>
          <button v-if="detailsView && (isAuthor || isAdmin)" class="btn" @click="$emit('delete')">
            <font-awesome-icon icon="fa-solid fa-trash"></font-awesome-icon>
          </button>
        </span>
      </span>
      <p class="card-date">{{ formatDate(createdAt) }}</p>
      <p class="card-text">{{ content }}</p>
      <div v-if="image" :class="detailsView ? 'post-image' : 'uploaded-image'" @click="viewPost">
        <img :src="image" alt="uploaded image" />
      </div>
      <div class="likes-and-comments">
        <span class="likes">
          <span class="num-of-likes">
            <p class="likes-header">Polubienia: </p>
            <p class="number-of-likes">{{ localNumOfLikes }}</p>
            <font-awesome-icon icon="fa-solid fa-thumbs-up"></font-awesome-icon>
          </span>
        </span>
        <span class="comments">
          <span class="num-of-comments" :style="!detailsView ? 'cursor: pointer' : ''" @click="viewPost">
            <p class="comments-header">Komentarze: </p>
            <p class="num-of-comments">{{ numOfComments }}</p>
            <font-awesome-icon icon="fa-solid fa-comment"></font-awesome-icon>
          </span>
        </span>
      </div>
      <div class="card-footer">
        <button class="btn like-button" :class="localIsLiked ? 'liked' : ''"
          @click.stop="localIsLiked ? deleteLike() : addLike()">
          <div v-if="!localIsLiked">
            <font-awesome-icon icon="fa-solid fa-thumbs-up" class="button-icon"></font-awesome-icon>
            Lubię to
          </div>
          <div v-else>
            <font-awesome-icon icon="fa-solid fa-thumbs-up" class="button-icon liked-button"></font-awesome-icon>
            Polubiono
          </div>
        </button>
        <button v-if="!detailsView" class="btn" @click="viewPost">
          <font-awesome-icon icon="fa-solid fa-angles-right" class="button-icon"></font-awesome-icon>
          Zobacz post
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import UserService from "@/services/UserService";
import ProfileImage from "./ProfileImage.vue";
import { checkAdmin, isAdmin } from "@/services/AuthService";
import BaseInput from "./BaseInput.vue";
import PostService from "@/services/PostService";

export default {
  components: {
    ProfileImage,
    BaseInput,
  },
  props: {
    id: {
      type: Number,
      required: true,
    },
    userId: {
      type: Number,
      required: true,
    },
    content: {
      type: String,
      required: true,
    },
    image: {
    },
    numOfLikes: {
      type: Number,
      required: true,
    },
    numOfComments: {
      type: Number,
      default: 0,
    },
    createdAt: {
      type: String,
      required: true,
    },
    isLiked: {
      type: Boolean,
      default: false,
    },
    detailsView: {
      type: Boolean,
      default: false,
    },
    reportedUserId: {
      type: Number,
    },
  },
  data() {
    return {
      imgSrc: "",
      username: "",
      isImageVisible: false,
      localNumOfLikes: this.numOfLikes,
      isAuthor: false,
      isAdmin: false,
      localIsLiked: this.isLiked,
      reportedPostId: this.reportedUserId,
    };
  },
  setup() {
    checkAdmin();
    return {
      isAdmin: isAdmin
    };
  },
  mounted() {
    this.localNumOfLikes = this.numOfLikes;
    this.localIsLiked = this.isLiked;
    this.getAuthorData();
    this.checkAuthor();
  },
  methods: {
    async getAuthorData() {
      if (!this.userId) {
        return;
      }
      const response = await UserService.getSmallUserData(this.userId);
      if (response.success === false) {
        console.error("Error while fetching user data");
        return;
      }
      this.imgSrc = response.data.imageUrl;
      this.username = response.data.username;
    },
    viewPost() {
      this.$router.push({ name: 'post', params: { id: this.id } });
    },
    async addLike() {
      const response = await PostService.likePost(this.id);
      if (response.success === false) {
        console.error("Error while adding like");
        return;
      }
      this.localNumOfLikes++;
      this.localIsLiked = true;
    },
    async deleteLike() {
      const response = await PostService.unlikePost(this.id);
      if (response.success === false) {
        console.error("Error while deleting like");
        return;
      }
      this.localNumOfLikes--;
      this.localIsLiked = false;
    },
    // report the post
    async report() {
      try {
        const commentId = null;
        const response = await PostService.report(this.id, commentId);
        if (response.success === false) {
          console.error("Error while reporting user");
          return;
        }
        this.reportedPostId = this.id;
        alert("Zgłoszono post");
      }
      catch (error) {
        console.error(error);
      }
    },
    // check if the logged in user is the author of the post
    checkAuthor() {
      this.isAuthor = this.userId == localStorage.getItem("id");
    },
    goToProfile() {
      this.$router.push({ name: 'userProfile', params: { id: this.userId } });
    },
    formatDate(date) {
      if (!date) return "";
      // minus miliseconds
      return new Date(date).toLocaleString().slice(0, -3);
    },
  },
};
</script>

<style scoped>
.card {
  border-radius: 0.5rem;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  background-color: var(--beige);
}

.card-body {
  display: flex;
  flex-direction: column;
  padding: 1rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.username-data {
  display: flex;
  align-items: last baseline;
  gap: 10px;
  cursor: pointer;
}

.post-image {
  width: auto;
  max-height: 30rem;
  border-radius: 0.5rem;
  margin-bottom: 1rem;
}

.card-date {
  font-size: 0.9rem;
  color: var(--gray);
  margin-top: 0.5rem;
  display: flex;
  justify-content: flex-start;
}

.card-text {
  display: flex;
  justify-content: flex-start;
}

.likes,
.comments {
  display: flex;
  gap: 10px;
  flex-direction: row;
  align-items: baseline;
  justify-content: first baseline !important;
}

.uploaded-image {
  margin-bottom: 1rem;
  border-radius: 10px;
  width: 100%;
  max-height: 50rem;
  overflow: hidden;
  cursor: pointer;
  object-fit: cover;
  object-position: bottom;
}

.uploaded-image img {
  width: 100%;
  object-fit: cover;
  object-position: bottom;
}

.post-image {
  width: 100%;
  height: 100%;
  max-height: 90rem;
  border-radius: 10px;
  margin-bottom: 1rem;
  overflow: hidden;
}

.post-image img {
  width: 100%;
  max-width: 750px;
  object-fit: cover;
}

.likes {
  flex-direction: column;
  justify-content: flex-start;
}

.num-of-likes,
.num-of-comments {
  display: flex;
  justify-content: flex-start;
  flex-direction: row;
  align-items: first baseline;
  gap: 0.5rem;
}

.like-button.liked {
  color: var(--dark-green);
  font-weight: bold;
}

.liked-button {
  color: var(--dark-green);
}

.card-footer {
  display: flex;
  justify-content: space-around;
  gap: 1rem;
  background-color: var(--beige);
}

.likes-and-comments {
  display: flex;
  justify-content: space-around;
  align-items: center;
  gap: 1rem;
}

.reported {
  color: var(--red);
}

@media screen and (max-width: 556px) {
  .header {
    flex-direction: column-reverse;
    gap: 1rem;
    align-items: flex-start;
  }

  .card-footer {
    flex-direction: column;
    align-items: center;
  }

  .likes-header,
  .comments-header {
    display: none;
  }
}
</style>