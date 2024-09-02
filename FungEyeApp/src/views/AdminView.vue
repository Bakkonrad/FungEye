<template>
  <div v-if="isAdmin == true" class="admin-panel" @scroll="handleScroll">
    <h1>Panel administratora</h1>
    <div class="buttons my-3">
      <button class="btn category-btn" :class="getActiveTable('users')" @click="showUsers">
        Użytkownicy
      </button>
      <button class="btn category-btn disabled" :class="getActiveTable('mushrooms')"
        @click="showMushrooms">Grzyby</button>
    </div>
    <div v-if="error" class="error-loading-data">
      {{ errorMessage }}
    </div>
    <!-- search bar and table for users -->
    <div v-else>
      <div v-if="activeTable === 'users'">
        <div v-if="!isEditing">
          <button ref="goToTheTopButton" class="btn fungeye-default-button" type="button" id="goToTheTopButton" @click="goToTheTop" title="go to the top"><font-awesome-icon icon="fa-solid fa-arrow-up" /></button>
          <div class="input-group mb-3" id="searchUsers">
            <input type="text" v-model="searchQuery" placeholder="Szukaj użytkowników..." class="form-control"
              id="searchUsers-input" aria-describedby="button-addon2" />
            <button @click="filterUsers" class="btn fungeye-default-button" type="button" id="button-addon2">
              <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="search-icon" />
            </button>
          </div>
          <UserTable :users="users" @edit-user="startEditing" @ban-user="banUserPrompt" @delete-user="deleteUser" />
          <LoadingSpinner v-if="isLoading"></LoadingSpinner>
          <div v-if="noUsersFound">
            <p class="no-users">
              {{ noUsersMessage }}
            </p>
          </div>
        </div>
        <UserEdit v-else :user="selectedUser" @cancel-edit="cancelEditing" @save-user="saveUser" />
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
import AdminService from "@/services/AdminService";
import { checkAdmin, isAdmin } from "@/services/AuthService";
import LoadingSpinner from "@/components/LoadingSpinner.vue";

export default {
  components: {
    UserTable,
    UserEdit,
    LoadingSpinner,
  },
  data() {
    return {
      isEditing: false,
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
    };
  },
  setup() {
    checkAdmin();
    return {
      isAdmin: isAdmin,
    };
  },
  async mounted() {
    const goToTheTopButton = this.$refs.goToTheTopButton;
    if (localStorage.getItem("token") && this.isAdmin == true) {
      console.log(localStorage.getItem("role"));
      this.fetchUsers(this.currentPage);
    } else {
      console.log(localStorage.getItem("role"));
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
        const response = await AdminService.getAllUsers(this.currentPage, this.searchQuery);
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
        console.log(response.data);
        console.log(response.data.length);
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
      if (!this.isEditing && (window.innerHeight + window.scrollY >= document.body.offsetHeight)) {
        if (!this.noUsersFound) {
          this.currentPage++;
          this.fetchUsers();
        }
      }
      if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        goToTheTopButton.style.display = "block";
      } else {
        goToTheTopButton.style.display = "none";
      }
    },
    // When the user clicks on the button, scroll to the top of the document
    goToTheTop() {
      document.body.scrollTop = 0;
      document.documentElement.scrollTop = 0;
    },
    filterUsers() {
      this.users = [];
      this.currentPage = 1;
      this.fetchUsers();
    },
    startEditing(user) {
      this.selectedUser = { ...user };
      this.isEditing = true;
    },
    cancelEditing() {
      this.isEditing = false;
    },
    saveUser(updatedUser) {
      const index = this.users.findIndex(
        (user) => user.email === updatedUser.email
      );
      if (index !== -1) {
        this.users.splice(index, 1, updatedUser);
        this.users = this.users.slice(0, this.currentPage * this.usersPerPage);
      }
      this.isEditing = false;
    },
    banUserPrompt(email) {
      const duration = prompt(
        "Podaj czas bana w sekundach (30s do 2592000s):",
        30
      );
      if (!duration || isNaN(duration) || duration < 30 || duration > 2592000) {
        alert("Nieprawidłowy czas");
        return;
      }

      const durationInSeconds = parseInt(duration);
      const now = new Date();
      const bannedUntil = new Date(now.getTime() + durationInSeconds * 1000);

      this.banUser(email, bannedUntil);
    },
    banUser(email, bannedUntil) {
      const user = this.users.find((user) => user.email === email);
      if (user) {
        user.BannedUntil = bannedUntil;
        alert(`Użytkownik ${email} zbanowany do ${bannedUntil}`);
      } else {
        alert("Nie znaleziono użytkownika");
      }
    },
    deleteUser(email) {
      const confirmed = confirm(
        `Czy na pewno chcesz usunąć użytkownika ${email}?`
      );
      if (confirmed) {
        this.users = this.users.filter((user) => user.email !== email);
        this.users = this.users.filter((user) => user.email !== email);
        alert(`Użytkownik ${email} został usunięty.`);
      }
    },
    getActiveTable(table) {
      this.isEditing = false;
      this.selectedUser = null;
      let classString = "category-btn";
      if (this.activeTable === table) {
        classString += " active";
      }
      return classString;
    },
    showUsers() {
      this.activeTable = "users";
    },
    showMushrooms() {
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

#searchUsers {
  width: 30%;
  margin: 0 auto;
  align-items: center;
}

.search-icon {
  position: absolute;
  padding: 5px;
  min-width: 50px;
  text-align: center;
}

#searchUsers-input {
  border-radius: 35px 0 0 35px !important;
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

@media screen and (max-width: 768px) {
  #searchUsers {
    width: 80%;
  }
}
</style>
