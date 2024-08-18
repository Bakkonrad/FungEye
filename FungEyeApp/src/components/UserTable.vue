<template>
  <div class="table-container table-responsive">
    <table class="table custom-table table-striped">
      <thead class="table-header">
        <tr class="table-row">
          <th scope="col" class="table-cell">Nazwa Użytkownika</th>
          <th scope="col" class="table-cell">Email</th>
          <th scope="col" class="table-cell">Imię</th>
          <th scope="col" class="table-cell">Nazwisko</th>
          <th scope="col" class="table-cell">Akcja</th>
        </tr>
      </thead>
      <tbody class="table-body">
        <tr class="table-row" v-for="user in users" :key="user.email">
          <td class="table-cell">{{ user.username }}</td>
          <td class="table-cell">{{ user.email }}</td>
          <td class="table-cell">{{ user.firstName }}</td>
          <td class="table-cell">{{ user.lastName }}</td>
          <td class="table-cell buttons">
            <button
              class="btn fungeye-default-button"
              id="btn-viewPosts"
              @click="viewPosts(user.email)"
            >
              Wyświetl posty
            </button>
            <button
              class="btn fungeye-default-button"
              id="btn-editUser"
              @click="$emit('edit-user', user)"
            >
              Edytuj
            </button>
            <button
              class="btn fungeye-default-button"
              id="btn-banUser"
              @click="$emit('ban-user', user.email)"
            >
              Banuj
            </button>
            <button
              class="btn fungeye-red-button"
              id="btn-deleteUser"
              @click="deleteUser(user.email)"
            >
              Usuń
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  props: ["users"],
  methods: {
    viewPosts(email) {
      this.$router.push({ name: "UserPosts", params: { email: email } });
    },
    deleteUser(email) {
      if (
        confirm(`Czy na pewno chcesz usunąć użytkownika o emailu ${email}?`)
      ) {
        this.$emit("delete-user", email);
      }
    },
  },
};
</script>

<style scoped>
.table-container {
  margin: 20px auto;
  max-width: 80%;
  border-radius: 12px;
}

/* table */
.table-striped {
  color: var(--black) !important;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  background-color: var(--beige) !important;
}

/* table header */
.table-striped > thead > tr > th {
  font-weight: 500;
  background-color: rgba(83, 130, 55, 0.7);
}

/* table body */
.table-striped > tbody > tr > td {
  background-color: var(--beige);
}

.table-striped > tbody > tr {
  border: 1px solid var(--beige);
}

.buttons {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 5px;
}

.fungeye-default-button,
.fungeye-red-button {
  height: 30px;
  padding: 0 15px;
  font-size: 1em;
}

#btn-viewPosts {
  background-color: var(--light-green);
  color: var(--black);
  height: auto;
}

#btn-editUser {
  background-color: var(--green);
}

#btn-banUser {
  background-color: var(--warning);
  color: var(--black);
}
</style>
