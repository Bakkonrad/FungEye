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
        <router-link :to="'/portal/all-posts'" class="btn fungeye-default-button" @click="toggleTab('all-posts')">Wyświetl wszystkie posty</router-link>
        <router-link :to="'/portal/followed-posts'" class="btn fungeye-default-button" @click="toggleTab('posts')">Wyświetl posty obserwowanych</router-link>
        <router-link :to="'/portal/search-users?q=' + searchQuery" class="btn fungeye-default-button" @click="toggleTab('search')">Szukaj użytkowników</router-link>
      </div>
      <!-- Posty -->
      <div v-if="showPosts || showAllPosts" class="posts-content">
        <AddPost @post-added="getPosts" />
        <div class="posts">
          <div v-for="post in posts" :key="post.id" class="post-item">
            <Post :id="post.id" :userId="post.userId" :content="post.content" :image="post.imageUrl" :num-of-likes="post.likeAmount" :isLiked="post.loggedUserReacted" />
          </div>
        </div>
      </div>
      <!-- Wyszukiwanie użytkowników -->
      <div v-if="searchUsers" class="searching">
        <SearchBar @search="handleSearch" :initialQuery="searchQuery" />
        <div class="search-content">
          <LoadingSpinner v-if="isLoading"></LoadingSpinner>
          <div v-if="error">
            <p class="error-message">
              {{ errorMessage }}
            </p>
          </div>
          <div class="users">
            <div v-if="users.length > 0" class="users-collection">
              <div class="p-2" v-for="user in users" :key="user.id">
                <router-link :to="'/user-profile/' + user.id" class="users-content r-link">
                  <ProfileImage :imgSrc="user.imageUrl" :width="100" :height="100" />
                  <p>{{ user.username }}</p>
                </router-link>
              </div>
            </div>
          </div>
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
import UserService from "@/services/UserService";
import PostService from "@/services/PostService";
import ProfileImage from "@/components/ProfileImage.vue";

export default {
  components: {
    Post,
    AddPost,
    SearchBar,
    LoadingSpinner,
    ProfileImage,
  },
  props: {
    defaultTab: {
      type: String,
      default: 'search',
    },
    query: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      posts: [],
      loggedIn: false,
      showAllPosts: true,
      showPosts: false,
      searchUsers: false,
      searchQuery: this.query,
      isLoading: false,
      error: false,
      errorMessage: "",
      users: [],
    };
  },
  setup() {
    checkAuth();
    return {
      loggedIn: isLoggedIn,
    };
  },
  mounted() {
    this.toggleTab(this.defaultTab);
    this.searchQuery = this.query;
    if (this.defaultTab === 'search' && this.searchQuery !== '') {
      this.handleSearch(this.searchQuery);
    }
    if (this.defaultTab === 'all-posts' || this.defaultTab === 'followed-posts') {
      this.getPosts();
    }
  },
  methods: {
    async getPosts() {
      const filter = this.showAllPosts ? 1 : 2;
      const page = 1;
      const response = await PostService.getPosts(filter, page);
      if (response.success === false) {
        console.error("Error while fetching posts data");
        return;
      }
      this.posts = response.data;
      console.log(this.posts);
    },
    toggleTab(tab) {
      this.showPosts = false;
      this.showAllPosts = false;
      this.searchUsers = false;

      if (tab === 'all-posts') {
        this.showAllPosts = true;
      } else if (tab === 'posts') {
        this.showPosts = true;
      } else if (tab === 'search') {
        this.searchUsers = true;
      }
    },
    async handleSearch(query) {
      if (query.trim() === '' || query.length < 3) {
        this.users = [];
        this.error = true;
        this.errorMessage = "Wpisz conajmniej 3 znaki aby rozpocząć wyszukiwanie";
        return;
      }
      this.searchQuery = query;
      this.$router.push({ name: 'searchUsers', query: { q: query } });
      this.isLoading = true;
      this.error = false;
      this.errorMessage = "";
      const response = await UserService.getAllUsers(0, query);
      if (response.success === false) {
        this.isLoading = false;
        this.error = true;
        this.errorMessage = "Wystąpił błąd podczas wyszukiwania użytkowników";
        return;
      }
      this.users = response.data;
      this.isLoading = false;
      if (this.users.length === 0) {
        this.error = true;
        this.errorMessage = "Nie znaleziono użytkowników spełniających kryteria wyszukiwania";
        return;
      }
      this.error = false;
      this.errorMessage = "";
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
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  width: 100%;
  margin-top: 2rem;
}

.search-content {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  width: 100%;
}

.users-collection {
  display: flex;
  justify-content: flex-start;
  flex-wrap: wrap;
  gap: 1rem;
}

.users-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-decoration: none;
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