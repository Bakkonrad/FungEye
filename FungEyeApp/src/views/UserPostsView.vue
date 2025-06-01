<template>
  <div v-if="!isAdmin" class="unauthorized">
    <h1>Brak dostępu</h1>
    <p>Aby zobaczyć tę stronę, należy zalogować się jako administrator</p>
    <RouterLink to="/log-in" class="btn fungeye-default-button">Zaloguj się</RouterLink>
  </div>
  <div v-else class="container">
    <h1>Posty użytkownika: {{ id }}</h1>
    <button class="btn fungeye-default-button my-3" @click="goBackToAdmin">Powrót do zakładki admin</button>
    <div v-if="loading">
      <LoadingSpinner />
    </div>
    <div v-else>
      <div v-if="error === true" class="error">{{ errorMessage }}</div>
      <div v-else class="posts">
        <div v-for="post in posts" :key="post.id" class="post">
          <Post :id="post.id" :userId="post.userId" :content="post.content" :image="post.imageUrl"
            :num-of-likes="post.likeAmount" :num-of-comments="post.commentsAmount" :created-at="post.createdAt" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import PostService from '@/services/PostService';
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import Post from '@/components/Post.vue';
import { isAdmin } from '@/services/AuthService';

export default {
  components: {
    LoadingSpinner,
    Post
  },
  props: ['id'],
  data() {
    return {
      posts: [],
      loading: false,
      error: false,
      errorMessage: '',
      isAdmin: isAdmin
    };
  },
  mounted() {
    if (!this.isAdmin) return;
    this.fetchPosts();
  },
  methods: {
    async fetchPosts() {
      try {
        this.loading = true;
        const response = await PostService.getPosts();
        if (response.success === false) {
          this.error = true;
          this.errorMessage = response.message;
          this.loading = false;
          return;
        }
        if (response.data.length === 0 || response.data === null) {
          this.loading = false;
          this.error = true;
          this.errorMessage = 'Brak postów do wyświetlenia.';
          return;
        }
        this.posts = response.data.filter(post => post.userId === parseInt(this.id));
        if (this.posts.length === 0) {
          this.error = true;
          this.errorMessage = 'Brak postów użytkownika.';
          return;
        }
      } catch (error) {
        console.error('Błąd podczas pobierania postów:', error);
      } finally {
        this.loading = false;
      }
    },
    goBackToAdmin() {
      this.$router.push('/admin');
    }
  }
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
}

.unauthorized {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  margin-top: 50px;
}

h1 {
  margin-bottom: 20px;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  margin-bottom: 20px;
}

h2 {
  margin: 0;
}

p {
  margin: 5px 0 0;
}

.btn {
  margin-top: 20px;
}

.posts {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 20px;
}

.post {
  display: flex;
  justify-content: flex-start;
  width: 80%;
}

p.card-text {
  margin: 0;
}
</style>