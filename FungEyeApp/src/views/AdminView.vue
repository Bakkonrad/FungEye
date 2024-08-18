<template>
  <div class="admin-panel" @scroll="handleScroll">
    <h1>Panel administratora</h1>
    <div class="buttons my-3">
      <button
        class="btn fungeye-default-button category-btn active"
        @click="showUserTable"
      >
        Użytkownicy
      </button>
      <button class="btn fungeye-default-button category-btn">Grzyby</button>
    </div>
    <div class="input-group mb-3" id="searchUsers">
      <input
        type="text"
        v-model="searchQuery"
        placeholder="Szukaj użytkownika"
        class="form-control"
        id="searchUsers-input"
        aria-describedby="button-addon2"
      />
      <button
        @click="filterUsers"
        class="btn fungeye-default-button"
        type="button"
        id="button-addon2"
      >
        Szukaj
      </button>
    </div>
    <UserTable
      v-if="!isEditing"
      :users="users"
      @edit-user="startEditing"
      @ban-user="banUserPrompt"
      @delete-user="deleteUser"
    />
    <UserEdit
    v-else
    :user="selectedUser"
    @cancel-edit="cancelEditing"
    @save-user="saveUser"
    />
  </div>
</template>

<script>
import UserTable from "@/components/UserTable.vue";
import UserEdit from "@/components/EditUser.vue";
import AdminService from "@/services/AdminService";

export default {
  components: {
    UserTable,
    UserEdit,
  },
  data() {
    return {
      isEditing: false,
      selectedUser: null,
      searchQuery: "",
      users: [],
      loadedUsers: [],
      filteredUsers: [],
      usersPerPage: 2,
      currentPage: 1,
      isLoading: false,
    };
  },
  mounted() {
    this.fetchUsers(this.currentPage);
    window.addEventListener("scroll", this.handleScroll);
  },
  beforeDestroy() {
    window.removeEventListener("scroll", this.handleScroll);
  },
  methods: {
    // async loadUsers() {
    //   const response = await AdminService.getAllUsers();
    //   this.users = response;
    //   const start = (this.currentPage - 1) * this.usersPerPage;
    //   const end = start + this.usersPerPage;
    //   const newUsers = this.users.slice(start, end);
    //   this.users = [...this.users.slice(0, start), ...newUsers];
    //   this.currentPage++;
    // },
    // handleScroll() {
    //   if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
    //     this.loadUsers();
    //   }
    // },
    async fetchUsers(page) {
      this.isLoading = true;
      const response = await AdminService.getAllUsers(page, this.usersPerPage);
      const newUsers = response || [];
      this.users = [...this.users, ...newUsers];
      this.isLoading = false;
    },
    handleScroll() {
      if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
        this.currentPage++;
        this.fetchUsers(this.currentPage);
      }
      // const bottom = event.target.scrollHeight - event.target.scrollTop === event.target.clientHeight;
      // if (bottom && !this.isLoading) {
      //   this.currentPage++;
      //   this.fetchUsers(this.currentPage);
      // }
    },
    filterUsers() {
      this.users = this.users.filter(
        (user) =>
          user.username
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase()) ||
          user.email.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          user.firstName
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase()) ||
          user.lastName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
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
  },
};
</script>

<style scoped>
.admin-panel {
  text-align: center;
  color: var(--black) !important;
  height: 100vh;
  overflow-y: auto;
}

#searchUsers {
  width: 30%;
  margin: 0 auto;
}

#searchUsers-input {
  border-radius: 35px 0 0 35px !important;
}

.buttons {
  display: flex;
  justify-content: center;
  gap: 1em;
  margin-bottom: 30px;
}

.category-btn {
  height: 35px;
  padding: 5px 40px !important;
}

.loading-spinner {
  text-align: center;
  padding: 20px;
  font-size: 16px;
  color: #333;
}

.form-control {
  color: var(--black) !important;
  width: 50%;
  margin: 0 auto;
}
</style>
