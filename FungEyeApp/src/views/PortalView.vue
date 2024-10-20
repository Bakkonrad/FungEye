<template>
  <div class="container-md">
    <h1 class="page-title">Portal</h1>
    <div v-if="!loggedIn" class="unauthorized">
      <h2>Brak dostępu!</h2>
      <p>Aby zobaczyć tę stronę, należy się zalogować</p>
      <RouterLink to="/log-in" class="btn fungeye-default-button">Zaloguj się</RouterLink>
    </div>
    <div v-else class="content">
      <!-- Przyciski -->
      <div class="buttons">
        <button class="btn fungeye-default-button" @click="toggleTab('all-posts')">Wyświetl wszystkie posty</button>
        <button class="btn fungeye-default-button" @click="toggleTab('post')">Wyświetl posty obserwowanych</button>
        <button class="btn fungeye-default-button" @click="toggleTab('search')">Szukaj użytkowników</button>
      </div>
      <!-- Posty -->
      <div v-if="showPosts || showAllPosts" class="posts-content">
        <AddPost @post-added="getPosts"/>
        <div class="posts">
          <div v-for="post in posts" :key="post.id" class="post-item">
            <Post :id="post.id" :content="post.content" :image="post.image" :userId="post.userId" />
          </div>
        </div>
      </div>
      <!-- Wyszukiwanie użytkowników -->
      <div v-if="searchUsers" class="searching">
        <SearchBar @search="handleSearch" />
        <LoadingSpinner v-if="isLoading"></LoadingSpinner>
        <div v-if="noUsersFound">
          <p class="no-users">
            {{ noUsersMessage }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AddPost from "../components/AddPost.vue";
import Post from "../components/Post.vue";
import SearchBar from "../components/SearchBar.vue";
import { isLoggedIn, checkAuth } from "@/services/AuthService";
import LoadingSpinner from "@/components/LoadingSpinner.vue";

export default {
  components: {
    Post,
    AddPost,
    SearchBar,
    LoadingSpinner,
  },
  data() {
    return {
      posts: [], 
      loggedIn: false,
      showAllPosts: true,
      showPosts: false,
      searchUsers: false,
      isLoading: false,
      noUsersFound: false,
      noUsersMessage: "",
    };
  },
  setup() {
    checkAuth();
    return {
      loggedIn: isLoggedIn,
    };
  },
  methods: {
    getPosts(post) {
      this.posts.push(post);
      
    },
    toggleTab(tab) {
      if (tab === 'all-posts') {
        this.showAllPosts = true;
        this.showPosts = false;
        this.searchUsers = false;
      } 
      else if (tab === 'post') {
        this.showAllPosts = false;
        this.showPosts = true;
        this.searchUsers = false;
      }
      else if (tab === 'search') {
        this.showAllPosts = false;
        this.showPosts = false;
        this.searchUsers = true;
      }
    },
    handleSearch(query) {
      console.log('Search query:', query);
    },
  },
};
</script>

<style scoped>
.container-md,
.content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.page-title {
  margin-bottom: 2rem;
}

.buttons {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.posts-content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 1rem;
  width: 100%;
  max-width: 50rem;
}

.posts {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  width: 100%;
  max-width: 40rem;
  margin: auto;
}

.post-item {
  width: 100%;
}

.searching {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  width: 100%;
  margin-top: 2rem;
}

#searchBar {
  width: 100%;
  max-width: 600px;
}

@media screen and (max-width: 768px) {

  .buttons {
    width: 90%;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 1rem;
  }
  
}

@media screen and (max-width: 556px) {

  .page-title {
    font-size: 2em;
  }

  .buttons {
    width: 90%;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 1rem;
  }

  .posts-content {
    width: 100%;
  }

}
</style>