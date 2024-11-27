<template>
  <div class="container-md">
    <h1 class="page-title">Portal</h1>
    <div v-if="!loggedIn" class="unauthorized">
      <LogInToContinue />
    </div>
    <div v-else class="content">
      <!-- Przyciski -->
      <div class="buttons">
        <router-link :to="'/portal/all-posts'" class="btn fungeye-default-button"
          @click="toggleTab('all-posts')">Wyświetl wszystkie posty</router-link>
        <router-link :to="'/portal/followed-posts'" class="btn fungeye-default-button"
          @click="toggleTab('followed-posts')">Wyświetl posty obserwowanych</router-link>
        <router-link :to="'/portal/search-users?q=' + searchQuery" class="btn fungeye-default-button"
          @click="toggleTab('search')">Szukaj użytkowników</router-link>
      </div>
      <!-- Posty -->
      <div v-if="showFollowedPosts || showAllPosts" class="posts-content">
        <AddPost @post-added="addedNewPost" />
        <button ref="goToTheTopButton" class="btn fungeye-default-button" type="button" id="goToTheTopButton"
          @click="goToTheTop" title="go to the top"><font-awesome-icon icon="fa-solid fa-arrow-up" /></button>
        <div class="posts">
          <div v-for="post in posts" :key="post.id" class="post-item">
            <Post :id="post.id" :userId="post.userId" :content="post.content" :image="post.imageUrl"
              :num-of-likes="post.likeAmount" :num-of-comments="post.commentsAmount" :created-at="post.createdAt" :is-liked="post.loggedUserReacted" />
          </div>
        </div>
        <LoadingSpinner v-if="isLoading"></LoadingSpinner>
        <div v-if="error || noPostsFound" class="message-container">
          <p class="error-message">
            {{ errorMessage }}
          </p>
          <p>
            {{ noPostsMessage }}
          </p>
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
import LogInToContinue from "@/components/LogInToContinue.vue";

export default {
  components: {
    Post,
    AddPost,
    SearchBar,
    LoadingSpinner,
    ProfileImage,
    LogInToContinue,
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
      showFollowedPosts: false,
      searchUsers: false,
      searchQuery: this.query,
      isLoading: false,
      error: false,
      errorMessage: "",
      users: [],
      currentPage: 1,
      currentFilter: 1,
      goToTheTopButton: null,
      noPostsFound: false,
      noPostsMessage: "",
    };
  },
  setup() {
    checkAuth();
    return {
      loggedIn: isLoggedIn,
    };
  },
  mounted() {
    if (!this.loggedIn) {
      return;
    }
    this.toggleTab(this.defaultTab);
    this.searchQuery = this.query;
    if (this.defaultTab === 'search' && this.searchQuery !== '') {
      this.handleSearch(this.searchQuery);
    }
    this.goToTheTopButton = this.$refs.goToTheTopButton;
    window.addEventListener("scroll", this.debounce(this.handleScroll, 200)); 
  },
  beforeDestroy() {
    window.removeEventListener("scroll", this.debounce(this.handleScroll, 200));
  },
  methods: {
    // function to limit the rate of handleScroll calls
    debounce(func, wait) {
      let timeout;
      return function (...args) {
        const context = this;
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(context, args), wait);
      };
    },
    async getPosts() {
      if (this.isLoading) {
        return;
      }
      this.isLoading = true;
      try {
        const response = await PostService.getPosts(this.currentFilter, this.currentPage);
        if (response.success === false) {
          console.error("Error while fetching posts data");
          return;
        }
        if (response.data.length === 0) {
          this.noPostsFound = true;
          if (this.posts.length === 0) {
            this.noPostsMessage = "Nie znaleziono postów.";
          }
          else {
            this.noPostsMessage = "To już wszystkie wyniki.";
          }
        }
        else {
          this.noPostsFound = false;
          const newPosts = response.data;
          this.posts = [...this.posts, ...newPosts];
        }
      } catch (error) {
        console.error("Error while fetching posts data");
      }
      finally {
        this.isLoading = false;
      }
    },
    toggleTab(tab) {
      this.showFollowedPosts = false;
      this.showAllPosts = false;
      this.searchUsers = false;

      if (tab === 'all-posts') {
        this.showAllPosts = true;
        this.posts = [];
        this.currentFilter = 1;
        this.currentPage = 1;
        this.getPosts();
      } else if (tab === 'followed-posts') {
        this.showFollowedPosts = true;
        this.posts = [];
        this.currentFilter = 2;
        this.currentPage = 1;
        this.getPosts();
      } else if (tab === 'search') {
        this.searchUsers = true;
      }
    },
    addedNewPost() {
      this.posts = [];
      this.currentPage = 1;
      this.getPosts();
    },
    handleScroll() {
      if (this.goToTheTopButton === null) {
        return;
      }
      if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
        if (!this.noPostsFound) {
          this.currentPage++;
          this.getPosts();
        }
      }
      if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        this.goToTheTopButton.style.display = "block";
      } else {
        this.goToTheTopButton.style.display = "none";
      }
    },
    goToTheTop() {
      document.body.scrollTop = 0;
      document.documentElement.scrollTop = 0;
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

.content{
  margin-top: 2rem;
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

#goToTheTopButton {
  display: none;
  position: fixed;
  bottom: 20px;
  right: 30px;
  z-index: 99;
  border: none;
  outline: none;
  color: white;
  cursor: pointer;
  padding: 15px;
  height: auto;
}

.message-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 1rem;
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