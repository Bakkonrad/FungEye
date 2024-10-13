<template>
  <div class="container-md">
    <!-- Formularz do dodania nowego posta -->
    <AddPost @add-post="addPost" />

    <!-- Lista postów -->
    <div class="posts">
      <div 
        v-for="post in posts" 
        :key="post.id" 
        class="post-item"
        @click="viewPost(post.id)">
        <Post :content="post.content" :images="post.images" :username="post.username" />
      </div>
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
      posts: [],  // Początkowo brak postów
      nextId: 1,  // Inicjalizacja id dla nowych postów
    };
  },
  methods: {
    viewPost(postId) {
      // Przekierowanie do widoku posta z odpowiednim id
      this.$router.push({ name: 'post', params: { id: postId } });
    },
    addPost(newPost) {
      // Dodajemy id do nowego posta i zwiększamy licznik id
      newPost.id = this.nextId++;
      this.posts.push(newPost);  // Dodanie nowego posta do listy
    },
  },
};
</script>

<style scoped>
.container-md {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.posts {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 100%;
  max-width: 600px;
}

.post-item {
  width: 100%;
  cursor: pointer; /* Podświetlenie postu jako klikalnego */
}
</style>