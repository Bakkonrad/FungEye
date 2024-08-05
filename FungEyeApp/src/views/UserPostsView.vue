<template>
    <div class="container">
      <h1>Posty użytkownika: {{ email }}</h1>
      <div v-if="loading">Ładowanie postów...</div>
      <div v-else>
        <div v-if="posts.length === 0">Brak postów do wyświetlenia.</div>
        <ul>
          <li v-for="post in posts" :key="post.id">
            <h2>{{ post.title }}</h2>
            <p>{{ post.body }}</p>
          </li>
        </ul>
      </div>
      <button class="btn btn-primary my-3" @click="goBackToAdmin">Powrót do zakładki admin</button>
    </div>
  </template>
  
  <script>
  export default {
    props: ['email'],
    data() {
      return {
        posts: [],
        loading: true
      };
    },
    created() {
      this.fetchPosts();
    },
    methods: {
      async fetchPosts() {
        try {
          // Example URL do zastąpienia odpowiednim endpointem API
          const response = await fetch(`https://api.example.com/posts?email=${this.email}`);
          const data = await response.json();
          this.posts = data;
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
  </style>