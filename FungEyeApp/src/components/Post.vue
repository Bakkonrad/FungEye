<template>
  <div class="card">
    <div class="card-body">
      <img v-if="imgSrc" :src="imgSrc" alt="postImage" class="post-image" />
      <span class="username-data">
        <ProfileImage />
        <p class="username">{{ username }}</p>
      </span>
      <p class="card-text">{{ content }}</p>
      <div v-if="images.length > 0" class="image-preview">
        <div v-for="(image, index) in images" :key="index" class="image-box">
          <img :src="image.url" alt="uploaded image" />
        </div>
      </div>
      <div class="card-footer">
        <span class="likes">
          <button class="btn" @click.stop="toggleLike">{{ isLiked ? 'Nie lubię tego' : 'Lubię to' }}</button>
          <p class="num-of-likes">{{ numOfLikes }}</p>
        </span>
        <span class="comments">
          <button class="btn">Komentarze</button>
          <p class="num-of-comments">{{ numOfComments }}</p>
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "./ProfileImage.vue";

export default {
  components: {
    ProfileImage,
  },
  props: {
    imgSrc: {
      type: String,
      required: false,
    },
    username: {
      type: String,
      default: "Username",
    },
    content: {
      type: String,
      required: true,
    },
    images: {
      type: Array,
      default: () => [],
    },
    numOfLikes: {
      type: Number,
      default: 0,
    },
    numOfComments: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      isLiked: false,
    };
  },
  methods: {
    toggleLike() {
      if (this.isLiked) {
        this.$emit('update:numOfLikes', this.numOfLikes - 1);
      } else {
        this.$emit('update:numOfLikes', this.numOfLikes + 1);
      }
      this.isLiked = !this.isLiked;
    },
  },
};
</script>

<style scoped>
.card {
  margin: 1rem;
  border-radius: 0.5rem;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  background-color: var(--beige);
}

.card-body {
  display: flex;
  flex-direction: column;
  padding: 1rem;
}

.username-data {
  display: flex;
  align-items: last baseline;
  gap: 10px;
}

.post-image {
  width: auto;
  max-height: 30rem;
  border-radius: 0.5rem;
  margin-bottom: 1rem;
}

.card-text {
  margin: 1rem;
}

.card-footer {
  display: flex;
  justify-content: space-between;
  align-items: first baseline;
  margin-top: 1rem;
  background-color: var(--beige);
}

.likes,
.comments {
  display: flex;
  gap: 10px;
  flex-direction: row;
  align-items: baseline;
  justify-content: first baseline !important;
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
</style>