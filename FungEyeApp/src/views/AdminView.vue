<template>
  <div v-if="isAdmin == true" class="admin-panel" @scroll="handleScroll">
    <h1>Panel administratora</h1>
    <div v-if="error" class="error-loading-data">
      {{ errorMessage }}
    </div>
    <!-- search bar and table for users -->
    <div v-else>
      <div class="buttons my-3">
        <button class="btn category-btn" :class="getActiveTable('users')" @click="showUsers">
          Użytkownicy
        </button>
        <button class="btn category-btn disabled" :class="getActiveTable('mushrooms')"
          @click="showMushrooms">Grzyby</button>
      </div>
      <div v-if="activeTable === 'users'">
        <div v-if="!isEditing && !isBanning">
          <button ref="goToTheTopButton" class="btn fungeye-default-button" type="button" id="goToTheTopButton" @click="goToTheTop" title="go to the top"><font-awesome-icon icon="fa-solid fa-arrow-up" /></button>
          <SearchBar @search="handleSearch" />
          <UserTable :users="users" @edit-user="startEditing" @ban-user="startBanning" @delete-user="deleteUser" />
          <LoadingSpinner v-if="isLoading"></LoadingSpinner>
          <div v-if="noUsersFound">
            <p class="no-users">
              {{ noUsersMessage }}
            </p>
          </div>
        </div>
        <UserEdit v-if="isEditing" :user="selectedUser" @cancel-edit="cancelEditing" @save-user="saveUser" />
        <UserBan v-if="isBanning" :user="selectedUser" @cancel-ban="cancelBanning" @ban-user="banUser" />
      </div>
      <!-- search bar and table for mushrooms -->
      <div v-else>
        <h2>Grzyby</h2>
        <p>not implemented</p>
      </div>
    </div>
  </div>
  <!-- is not admin -->
  <div v-else style="
      display: flex;
      justify-content: center;
      flex-direction: column;
      align-items: center;
    ">
    <h1>Brak dostępu!</h1>
    <p>Aby zobaczyć tę stronę, należy zalogować się jako administrator</p>
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

export default {
  components: {
    UserTable,
    UserEdit,
    LoadingSpinner,
    SearchBar,
    UserBan,
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
      // console.log(localStorage.getItem("role"));
      this.fetchUsers(this.currentPage);
    } else {
      // console.log(localStorage.getItem("role"));
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
        }
        else if (response.data.length === 0) {
          this.noUsersFound = true;
          this.noUsersMessage = "To już wszystkie wyniki.";
          return;
        }
        this.noUsersFound = false;
        // console.log(response.data);
        // console.log(response.data.length);
        const newUsers = response.data;
        this.users = [...this.users, ...newUsers];
        console.log(this.users[0]);
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
    // When the user clicks on the button, scroll to the top of the document
    goToTheTop() {
      document.body.scrollTop = 0;
      document.documentElement.scrollTop = 0;
    },
    cleanUsers() {
      this.users = [];
      this.currentPage = 1;
      this.fetchUsers();
    },
    handleSearch(query) {
      this.searchQuery = query;
      this.cleanUsers();
    },
    startEditing(user) {
      if (user.id == localStorage.getItem("id")) {
        alert("Aby edytować swoje dane, przejdź do swojego profilu.");
        this.isEditing = false;
        return;
      }
      this.selectedUser = { ...user };
      this.isEditing = true;
    },
    cancelEditing() {
      this.isEditing = false;
    },
    saveUser() {
      // console.log("User updated");
      this.cleanUsers();
      this.isEditing = false;
    },
    startBanning(user) {
      if (user.id == localStorage.getItem("id")) {
        alert("Nie możesz zbanować samego siebie.");
        this.isBanning = false;
        return;
      }
      this.selectedUser = { ...user };
      this.isBanning = true;
    },
    banUser(user, ban) {
      console.log(user);
      console.log("chosenTime", ban);
      try {
        const response = UserService.banUser(user.id, ban);
        if (response.success === false) {
          console.log(response.message);
          return;
        }
        console.log("User banned");
      }
      catch (error) {
        console.log(error);
      }
      this.cleanUsers();
    },
    cancelBanning() {
      this.isBanning = false;
    },
    async deleteUser(user) {
      if (user.id == localStorage.getItem("id")) {
        alert("Aby usunąć swoje konto, przejdź do swojego profilu.");
        return;
      }
      const confirmed = confirm(
        `Czy na pewno chcesz usunąć użytkownika ${user.username}?`
      );
      if (confirmed) {
        await UserService.deleteAccount(user.id);
        this.cleanUsers();
      }
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
    showMushrooms() {
      this.isEditing = false;
      this.selectedUser = null;
      this.activeTable = "mushrooms";
    },
  },
};
</script>

<style scoped>
.admin-panel {
  text-align: center;
  color: var(--black) !important;
  height: auto;
  overflow-y: auto;
}

.buttons {
  display: flex;
  justify-content: space-between;
  width: 23%;
  margin: 0 auto;
  margin-bottom: 30px;
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
