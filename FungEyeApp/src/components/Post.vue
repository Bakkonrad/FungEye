<template>
  <div class="card">
    <div class="card-body">
      <img :src="imgSrc" alt="postImage" class="post-image" />
      <span class="username-data">
        <ProfileImage />
        <p class="username">{{ username }}</p>
      </span>
      <p class="card-text">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam viverra,
        nisl nec ultricies ultricies, nunc nisl ultricies sapien, nec ultricies
        nisl nisl nec nisl. Nullam viverra, nisl nec ultricies ultricies, nunc
        nisl ultricies sapien, nec ultricies nisl nisl nec nisl.
      </p>
      <div class="card-footer">
        <span class="likes">
            <button class="btn">Like</button>
            <p class="num-of-likes">{{ numOfLikes }}</p>
        </span>
        <span class="comments">
            <button class="btn">Comment</button>
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
      required: true,
    },
    username: {
      type: String,
      default: "Username",
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
};
</script>

<style scoped>
.card {
  width: 45rem;
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


</style>
