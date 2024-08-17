<template>
  <div class="table-container">
    <div class="custom-table">
      <div class="table-header">
        <div class="table-row">
          <div class="table-cell">Nazwa Użytkownika</div>
          <div class="table-cell">Email</div>
          <div class="table-cell">Imię</div>
          <div class="table-cell">Nazwisko</div>
          <div class="table-cell">Akcja</div>
        </div>
      </div>
      <div class="table-body">
        <div class="table-row" v-for="user in users" :key="user.Email">
          <div class="table-cell">{{ user.Username }}</div>
          <div class="table-cell">{{ user.Email }}</div>
          <div class="table-cell">{{ user.FirstName }}</div>
          <div class="table-cell">{{ user.LastName }}</div>
          <div class="table-cell">
            <button class="btn btn-info btn-sm rounded-button" @click="viewPosts(user.Email)">Posty</button>
            <button class="btn btn-success btn-sm rounded-button" @click="$emit('edit-user', user)">Edytuj</button>
            <button class="btn btn-warning btn-sm rounded-button" @click="$emit('ban-user', user.Email)">Banuj</button>
            <button class="btn btn-danger btn-sm rounded-button" @click="deleteUser(user.Email)">Usuń</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ['users'],
  methods: {
    viewPosts(email) {
      this.$router.push({ name: 'UserPosts', params: { email: email } });
    },
    deleteUser(email) {
      if (confirm(`Czy na pewno chcesz usunąć użytkownika o emailu ${email}?`)) {
        this.$emit('delete-user', email);
      }
    }
  }
};
</script>

<style scoped>
.table-container {
  margin: 20px auto;
  max-width: 80%;
}

.custom-table {
  background-color: rgba(106, 153, 78, 0.3);
  border-radius: 12px;
  overflow: hidden;
}

.table-header, .table-body {
  background-color: rgba(255, 255, 255, 0.8);
}

.table-header {
  background-color: rgba(106, 153, 78, 0.7);
}

.table-row {
  display: flex;
  justify-content: space-between;
  padding: 10px;
}

.table-cell {
  flex: 1;
  padding: 4px;
}

.btn-sm.rounded-button {
  border-radius: 20px;
  margin-right: 15px;
  padding: 5px 10px;
}
</style>