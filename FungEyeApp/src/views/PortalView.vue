<template>
  <div class="container-md">
    <AddPost />
    <div class="posts">
      <Post :imgSrc="imgSrc" />
      <Post imgSrc="https://picsum.photos/id/237/200/300" />
      <Post :imgSrc="imgSrc" />
    </div>
  </div>
</template>

<script>
import AddPost from "../components/AddPost.vue";
import Post from "../components/Post.vue";

export default {
  components: {
    Post,
    AddPost,
  },
  data() {
    return {
      imgSrc: "https://picsum.photos/200/300",
    };
  },
};
</script>

<style scoped>
.container-md {
  display: flex;
    flex-direction: column;
  justify-content: center;
}

.posts {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

</style>
