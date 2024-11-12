<template>
  <div v-if="isAdmin == true" class="admin-panel" @scroll="handleScroll">
    <h1 class="panel-admin">Panel administratora</h1>
    <div v-if="error" class="error-loading-data">
      {{ errorMessage }}
    </div>
    <!-- search bar and table for users -->
    <div v-else>
      <div class="buttons my-3">
        <button class="btn category-btn" :class="getActiveTable('users')" @click="showUsers">
          Użytkownicy
        </button>
        <button class="btn category-btn" :class="getActiveTable('reports')" @click="showReports">
          Zgłoszenia
        </button>
      </div>
      <div v-if="activeTable === 'users'">
        <div v-if="!isEditing && !isBanning">
          <button ref="goToTheTopButton" class="btn fungeye-default-button" type="button" id="goToTheTopButton"
            @click="goToTheTop" title="go to the top"><font-awesome-icon icon="fa-solid fa-arrow-up" /></button>
          <div class="admin-actions">
            <button class="btn fungeye-default-button" @click="addmin">
              <font-awesome-icon icon="fa-solid fa-plus" class="button-icon" />
              Zarejestruj nowego admina
            </button>
            <SearchBar @search="handleSearch" />
          </div>
          <UserTable :users="users" />
          <LoadingSpinner v-if="isLoading"></LoadingSpinner>
          <div v-if="noUsersFound">
            <p class="no-users">{{ noUsersMessage }}</p>
          </div>
        </div>
        <UserEdit v-if="isEditing" :user="selectedUser" @cancel-edit="cancelEditing" @save-user="saveUser" />
        <UserBan v-if="isBanning" :user="selectedUser" @cancel-ban="cancelBanning" @ban-user="banUser" />
      </div>

      <!-- Widok zgłoszeń -->
      <div v-else-if="activeTable === 'reports'">
        <ReportsView />
      </div>
      <div v-else style="display: flex; justify-content: center; flex-direction: column; align-items: center;">
      </div>
    </div>
  </div>

  <!-- is not admin -->
  <div v-else class="unauthorized">
    <h1>Brak dostępu!</h1>
    <p>Aby zobaczyć tę stronę, należy zalogować się jako administrator</p>
    <RouterLink to="/log-in" class="btn fungeye-default-button">Zaloguj się</RouterLink>
  </div>
</template>

<script>
import UserTable from "@/components/UserTable.vue";
import UserEdit from "@/components/EditUser.vue";
import UserService from "@/services/UserService";
import { checkAdmin, isAdmin } from "@/services/AuthService";
import LoadingSpinner from "@/components/LoadingSpinner.vue";
import SearchBar from "@/components/SearchBar.vue";
import UserBan from "@/components/BanUser.vue";
import { RouterLink } from "vue-router";
import ReportsView from "@/views/ReportsView.vue";

export default {
  components: {
    UserTable,
    UserEdit,
    LoadingSpinner,
    SearchBar,
    UserBan,
    ReportsView,
  },
  data() {
    return {
      isEditing: false,
      isBanning: false,
      selectedUser: null,
      searchQuery: "",
      users: [],
      filteredUsers: [],
      usersPerPage: 2,
      currentPage: 1,
      isLoading: false,
      isAdmin: false,
      error: false,
      errorMessage: "",
      noUsersFound: false,
      noUsersMessage: "",
      activeTable: "users",
      goToTheTopButton: null,
    };
  },
  setup() {
    checkAdmin();
    return {
      isAdmin: isAdmin,
    };
  },
  async mounted() {
    this.goToTheTopButton = this.$refs.goToTheTopButton;
    if (localStorage.getItem("token") && this.isAdmin == true) {
      this.fetchUsers(this.currentPage);
    } else {
      console.log("No token or no admin rights");
    }
    window.addEventListener("scroll", this.handleScroll);
  },
  beforeDestroy() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    async fetchUsers() {
      try {
        this.isLoading = true;
        const response = await UserService.getAllUsers(this.currentPage, this.searchQuery);
        if (!response.success) {
          this.error = true;
          this.errorMessage = response.message;
          return;
        }
        if (response.data.length === 0 && this.users.length === 0) {
          this.noUsersFound = true;
          this.noUsersMessage = "Nie znaleziono użytkowników spełniających kryteria.";
          return;
        } else if (response.data.length === 0) {
          this.noUsersFound = true;
          this.noUsersMessage = "To już wszystkie wyniki.";
          return;
        }
        this.noUsersFound = false;
        const newUsers = response.data;
        this.users = [...this.users, ...newUsers];
      } catch (error) {
        this.error = true;
        this.errorMessage = response.message;
      } finally {
        this.isLoading = false;
      }
    },
    handleScroll() {
      if (this.goToTheTopButton === null) {
        return;
      }
      if (!this.isEditing && (window.innerHeight + window.scrollY >= document.body.offsetHeight)) {
        if (!this.noUsersFound) {
          this.currentPage++;
          this.fetchUsers();
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
    cleanUsers() {
      this.users = [];
      this.currentPage = 1;
      this.fetchUsers();
    },
    addmin() {
      this.$router.push("/register/admin");
    },
    handleSearch(query) {
      this.searchQuery = query;
      this.cleanUsers();
    },
    getActiveTable(table) {
      let classString = "category-btn";
      if (this.activeTable === table) {
        classString += " active";
      }
      return classString;
    },
    showUsers() {
      this.isEditing = false;
      this.selectedUser = null;
      this.activeTable = "users";
    },
    showReports() {
      this.isEditing = false;
      this.selectedUser = null;
      this.activeTable = "reports";
    },
  },
};
</script>

<style scoped>
.admin-panel {
  color: var(--black) !important;
  height: auto;
  overflow-y: auto;
}

.buttons {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 23%;
  margin: 0 auto;
  margin-bottom: 30px;
  gap: 30px;
}

.category-btn {
  font-size: 1.2em;
  font-weight: 300;
  color: var(--black);
  text-decoration: none;
  height: 35px;
  padding: 5px 30px !important;
  border-radius: 0;
  border: none;
  border-bottom: 1px solid var(--black);
  cursor: pointer;
}

.category-btn:focus,
.category-btn.active,
.category-btn:active {
  font-weight: 500;
  border-bottom: 2px solid var(--black);
}

.category-btn:hover {
  font-weight: 500;
}

.category-btn.disabled {
  color: #ccc;
  border-bottom: 1px solid #ccc;
  cursor: not-allowed;
}

.form-control {
  color: var(--black) !important;
  width: 50%;
  margin: 0 auto;
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

.admin-actions {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 20px;
}

.panel-admin {
  text-align: center;
}

.no-users {
  text-align: center;
  font-weight: 600;
  margin-top: 20px;
}

@media screen and (max-width: 1400px) {
  .buttons {
    width: 30%;
  }
}

@media screen and (max-width: 1200px) {
  .buttons {
    width: 40%;
  }
}

@media screen and (max-width: 992px) {
  .buttons {
    width: 50%;
  }
}

@media screen and (max-width: 768px) {
  .buttons {
    width: 80%;
  }
}

@media screen and (max-width: 576px) {
  h1 {
    font-size: 2em;
  }

  .buttons {
    width: 90%;
  }

  .category-btn {
    font-size: 1em;
    padding: 5px 20px !important;
  }
}
</style>
