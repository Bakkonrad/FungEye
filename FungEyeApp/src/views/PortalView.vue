<template>
  <div class="container-md">
    <h1 class="page-title">Portal</h1>
    <div v-if="!loggedIn" class="unauthorized">
      <h2>Brak dostępu!</h2>
      <p>Aby zobaczyć tę stronę, należy się zalogować</p>
      <RouterLink to="/log-in" class="btn fungeye-default-button">Zaloguj się</RouterLink>
    </div>
    <div v-else class="content">
      <!-- Formularz do dodania nowego posta -->
      <div class="buttons">
        <button class="btn fungeye-default-button" @click="toggleTab('post')">Wyświetl posty</button>
        <button class="btn fungeye-default-button" @click="toggleTab('search')">Szukaj użytkowników</button>
      </div>
      <div v-if="showPosts" class="posts-content">
        <AddPost @add-post="addPost" />
        <div class="posts">
          <div v-for="post in posts" :key="post.id" class="post-item" @click="viewPost(post.id)">
            <Post :content="post.content" :images="post.images" :username="post.username" />
          </div>
        </div>
      </div>
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
      posts: [],  // Początkowo brak postów
      nextId: 1,  // Inicjalizacja id dla nowych postów
      loggedIn: false,
      showPosts: true,
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
    viewPost(postId) {
      // Przekierowanie do widoku posta z odpowiednim id
      this.$router.push({ name: 'post', params: { id: postId } });
    },
    addPost(newPost) {
      // Dodajemy id do nowego posta i zwiększamy licznik id
      newPost.id = this.nextId++;
      this.posts.push(newPost);  // Dodanie nowego posta do listy
    },
    toggleTab(tab) {
      if (tab === 'post') {
        this.showPosts = true;
        this.searchUsers = false;
      } else if (tab === 'search') {
        this.searchUsers = true;
        this.showPosts = false;
      }
    },
    handleSearch(query) {
      // Wyszukiwanie użytkowników
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
  gap: 1rem;
  width: 100%;
  max-width: 40rem;
}

.post-item {
  width: 100%;
  cursor: pointer;
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