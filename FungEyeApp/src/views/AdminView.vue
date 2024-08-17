<template>
  <div class="admin-panel" @scroll="handleScroll">
    <Navbar/>
    <h1>Admin</h1>
    <div class="btn-group my-3">
      <button class="btn btn-success" @click="showUserTable">Użytkownicy</button>
      <button class="btn btn-success">Grzyby</button>
    </div>
    <input type="text" v-model="searchQuery" placeholder="Szukaj użytkownika" class="form-control my-3"/>
    <UserTable v-if="!isEditing" :users="filteredUsers()" @edit-user="startEditing" @ban-user="banUserPrompt" @delete-user="deleteUser"/>
    <UserEdit v-else :user="selectedUser" @cancel-edit="cancelEditing" @save-user="saveUser"/>
  </div>
</template>

<script>
import UserTable from '@/components/UserTable.vue';
import UserEdit from '@/components/EditUser.vue';

export default {
  components: {
    UserTable,
    UserEdit
  },
  data() {
    return {
      isEditing: false,
      selectedUser: null,
      searchQuery: '',
      users: [
        { username: 'MPudzianowski', email: 'mpudzianowski@gmail.com', firstName: 'Mariusz', lastName: 'Pudzianowski' },
        { username: 'RLewandowski', email: 'rlewandowski@gmail.com', firstName: 'Robert', lastName: 'Lewandowski' },
        { username: 'AMałysz', email: 'amalysz@gmail.com', firstName: 'Adam', lastName: 'Małysz' },
        { username: 'ATest', email: 'atest@gmail.com', firstName: 'Adam', lastName: 'Test' },
        
      ],
      loadedUsers: [],
      usersPerPage: 2,
      currentPage: 1
    };
  },
  mounted() {
    this.loadUsers();
    window.addEventListener('scroll', this.handleScroll);
  },
  beforeDestroy() {
    window.removeEventListener('scroll', this.handleScroll);
  },
  methods: {
    loadUsers() {
      const start = (this.currentPage - 1) * this.usersPerPage;
      const end = start + this.usersPerPage;
      const newUsers = this.users.slice(start, end);
      this.loadedUsers = [...this.loadedUsers, ...newUsers];
      this.currentPage++;
    },
    handleScroll() {
      if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
        this.loadUsers();
      }
    },
    filteredUsers() {
      return this.loadedUsers.filter(user => 
        user.username.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.firstName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
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
      const index = this.users.findIndex(user => user.email === updatedUser.email);
      if (index !== -1) {
        this.users.splice(index, 1, updatedUser);
        this.loadedUsers = this.users.slice(0, this.currentPage * this.usersPerPage);
      }
      this.isEditing = false;
    },
    banUserPrompt(email) {
      const duration = prompt("Podaj czas bana w sekundach (30s do 2592000s):", 30);
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
      const user = this.users.find(user => user.email === email);
      if (user) {
        user.BannedUntil = bannedUntil;
        alert(`Użytkownik ${email} zbanowany do ${bannedUntil}`);
      } else {
        alert("Nie znaleziono użytkownika");
      }
    },
    deleteUser(email) {
      const confirmed = confirm(`Czy na pewno chcesz usunąć użytkownika ${email}?`);
      if (confirmed) {
        this.users = this.users.filter(user => user.email !== email);
        this.loadedUsers = this.loadedUsers.filter(user => user.email !== email);
        alert(`Użytkownik ${email} został usunięty.`);
      }
    }
  }
};
</script>

<style scoped>
.admin-panel {
  text-align: center;
  color: black !important;
  height: 100vh;
  overflow-y: auto;
}
.btn-group {
  margin-bottom: 30px;
}

.form-control {
  color: black !important;
  width: 50%;
  margin: 0 auto;
}
</style>