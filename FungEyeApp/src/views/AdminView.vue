<template>
  <div class="admin-panel">
    <Navbar/>
    <h1>Admin</h1>
    <div class="btn-group my-3">
      <button class="btn btn-success" @click="showUserTable">Użytkownicy</button>
      <button class="btn btn-success">Posty</button>
      <button class="btn btn-success">Grzyby</button>
    </div>
    <UserTable v-if="!isEditing" :users="users" @edit-user="startEditing"/>
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
      users: [
        { Username: 'JKowalski', Email: 'jkowalski@gmail.com', FirstName: 'Jan', LastName: 'Kowalski' },
        { Username: 'MKowalski', Email: 'mkowalski@gmail.com', FirstName: 'Marek', LastName: 'Kowalski' },
        { Username: 'AKowalski', Email: 'akowalski@gmail.com', FirstName: 'Anna', LastName: 'Kowalski' }
      ]
    };
  },
  methods: {
    showUserTable() {
      this.isEditing = false;
    },
    startEditing(user) {
      this.selectedUser = { ...user };
      this.isEditing = true;
    },
    cancelEditing() {
      this.isEditing = false;
    },
    saveUser(updatedUser) {
      const index = this.users.findIndex(user => user.Email === updatedUser.Email);
      if (index !== -1) {
        this.users.splice(index, 1, updatedUser);
      }
      this.isEditing = false;
    }
  }
};
</script>

<style scoped>
.admin-panel {
  text-align: center;
}
.btn-group {
  margin-bottom: 30px;
}
</style>
