<template>
  <div class="card">
    <div class="card-body">
      <span class="header">
        <span class="username-data" @click="goToProfile">
          <ProfileImage :imgSrc="imgSrc" />
          <p class="username">{{ username }}</p>
        </span>
        <span class="buttons">
          <button v-if="!isAuthor" class="btn" @click="reportUser">
            <font-awesome-icon icon="fa-solid fa-flag" class="button-icon"></font-awesome-icon>
            Zgłoś
          </button>
          <button v-if="detailsView && isAuthor" class="btn" @click="$emit('edit')">
            <font-awesome-icon icon="fa-solid fa-pen" class="button-icon"></font-awesome-icon>
            Edytuj
          </button>
          <button v-if="detailsView && (isAuthor || isAdmin)" class="btn" @click="$emit('delete')">
            <font-awesome-icon icon="fa-solid fa-trash" class="button-icon"></font-awesome-icon>
            Usuń post
          </button>
        </span>
      </span>

      <p class="card-text">{{ content }}</p>
      <div v-if="image && image.name != ''" :class="detailsView ? 'post-image' : 'uploaded-image'">
        <img :src="image.url" alt="uploaded image" />
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
          <span class="num-of-comments">
            <p class="comments-header">Komentarze: </p>
            <p class="num-of-comments">{{ numOfComments }}</p>
            <font-awesome-icon icon="fa-solid fa-comment"></font-awesome-icon>
          </span>
        </span>
      </div>
      <div class="card-footer">
        <button class="btn like-button" :class="isLiked ? 'dislike' : ''" @click.stop="toggleLike">
          <div v-if="!isLiked">
            <font-awesome-icon icon="fa-solid fa-thumbs-up" class="button-icon"></font-awesome-icon>
            Lubię to
          </div>
          <div v-else>
            <font-awesome-icon icon="fa-solid fa-thumbs-down" class="button-icon"></font-awesome-icon>
            Nie lubię tego
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
      default: 0,
    },
    content: {
      type: String,
      required: true,
    },
    image: {
      type: Object,
      default: () => ({}),
    },
    numOfLikes: {
      type: Number,
      default: 0,
    },
    numOfComments: {
      type: Number,
      default: 0,
    },
    detailsView: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isLiked: false,
      imgSrc: "",
      username: "",
      isImageVisible: false,
      localNumOfLikes: this.numOfLikes,
      isAuthor: false,
      isAdmin: false,
    };
  },
  setup() {
    checkAdmin();
    return {
      isAdmin: isAdmin
    };
  },
  mounted() {
    this.getAuthorData();
    this.checkAuthor();
  },
  methods: {
    toggleLike() {
      this.isLiked = !this.isLiked;
    },
    async getPost() {
      // const response = await UserService.getPost(this.id);
      const response = {
        success: true,
        data: {
          userId: 5,
          content: "content",
          image: {
            url: "https://picsum.photos/800/800",
          },
          numOfLikes: 0,
          numOfComments: 0,
          comments: [
            { id: 1, imgSrc: "", username: "username", content: "content" },
            { id: 2, imgSrc: "", username: "username", content: "content" },
          ],
        },
      }
      if (response.success === false) {
        console.error("Error while fetching post data");
        return;
      }
      this.post = response.data;
    },
    async getAuthorData() {
      const response = await UserService.getUserData(this.userId);
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
    toggleLike() {
      this.isLiked = !this.isLiked;
      if (this.isLiked) {
        this.localNumOfLikes++;
      } else {
        this.localNumOfLikes--;
      }
    },
    reportUser() {
      alert("Zgłoszono użytkownika");
    },
    checkAuthor() {
      this.isAuthor = this.userId == localStorage.getItem("id");
    },
    goToProfile() {
      this.$router.push({ name: 'userProfile', params: { id: this.userId } });
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

.card-text {
  margin-top: 1rem;
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
  max-height: 20rem;
  overflow: hidden;
}

.uploaded-image img {
  width: 100%;
  object-fit: cover;
}

.post-image {
  width: 100%;
  height: 100%;
  max-height: 50rem;
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

.like-button.dislike {
  color: var(--red);
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