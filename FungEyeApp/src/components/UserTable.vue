<template>
  <div class="table-container table-responsive">
    <table class="table custom-table table-hover">
      <thead class="table-header">
        <tr class="table-row">
          <th scope="col" class="table-cell"></th>
          <th scope="col" class="table-cell">Login</th>
          <th scope="col" class="table-cell">Email</th>
          <th scope="col" class="table-cell">Imię</th>
          <th scope="col" class="table-cell">Nazwisko</th>
          <th scope="col" class="table-cell">Szczegóły</th>
        </tr>
      </thead>
      <tbody class="table-body">
        <tr class="table-row" v-for="user in users" :key="user.id" @click="userDetails(user)">
          <td class="table-cell img-cell" :class="user.dateDeleted ? 'deleted-user' : ''">
            <ProfileImage :imgSrc="user.imageUrl" :width="25" :height="25" :showBorder="false" />
          </td>
          <td class="table-cell" :class="user.dateDeleted ? 'deleted-user' : ''">
            {{ user.username }}
          </td>
          <td class="table-cell" :class="user.dateDeleted ? 'deleted-user' : ''">{{ user.email }}</td>
          <td class="table-cell" :class="user.dateDeleted ? 'deleted-user' : ''">{{ user.firstName }}</td>
          <td class="table-cell" :class="user.dateDeleted ? 'deleted-user' : ''">{{ user.lastName }}</td>
          <td class="table-cell" :class="user.dateDeleted ? 'deleted-user' : ''">
            <button class="btn fungeye-default-button" id="btn-retrieveAccount" @click="userDetails(user)">
              <font-awesome-icon icon="fa-solid fa-angles-right"></font-awesome-icon>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import ProfileImage from './ProfileImage.vue';

export default {
  props: ["users"],
  components: {
    ProfileImage,
  },
  methods: {
    viewPosts(email) {
      this.$router.push({ name: "UserPosts", params: { email: email } });
    },
    formatDate(date) {
      // console.log(date);
      if (date === null || new Date(date) < new Date()) {
        return "";
      }
      if (new Date(date).getFullYear() > 2100) {
        return "Na zawsze";
      }
      return new Date(date).toLocaleString();
    },
    formatDeletedDate(date) {
      if (date === null) {
        return "";
      }
      return new Date(date).toLocaleString();
    },
    userDetails(user) {
      this.$router.push({ name: "UserProfileAdmin", params: { id: user.id } });
    },
  }
};
</script>

<style scoped>
.table-container {
  margin: 20px auto;
  max-width: 80%;
}

/* table */
.table-hover {
  color: var(--black) !important;
  box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
  border-collapse: separate;
  /* Ensure border-radius is applied */
  border-spacing: 0;
  /* Remove spacing between cells */
  background-color: var(--beige) !important;
  border-radius: 10px;
  overflow: hidden;
  /* Ensures content respects the border-radius */
}

thead {
  background-color: rgba(83, 130, 55, 0.7);
  border-radius: 10px;
}

/* Ensure header respects the border-radius */
thead tr:first-child th:first-child {
  border-top-left-radius: 10px;
}

thead tr:first-child th:last-child {
  border-top-right-radius: 10px;
}

thead tr:last-child th:first-child {
  border-bottom-left-radius: 10px;
}

thead tr:last-child th:last-child {
  border-bottom-right-radius: 10px;
}

/* table header */
.table-hover>thead>tr>th {
  font-weight: 500;
  text-align: start;
  border: none;
  padding: 10px;
  background-color: transparent !important;
}

/* table body */
.table-hover>tbody>tr>td {
  align-items: center;
  text-align: start;
  border: none;
  border-bottom: 1px solid #d9d3c7;
  background-color: var(--beige);
}

.table-hover>tbody>tr {
  /* border: 0.5px solid #d9d3c7; */
  /* border: hidden; */
  border: none;
}

.fungeye-default-button,
.fungeye-red-button {
  height: 30px;
  padding: 0 15px;
  font-size: 1em;
}

.img-cell {
  padding-right: 0px !important;
}

.profile-picture {
  width: 25px;
  height: 25px;
  border-radius: 50%;
}

#btn-viewPosts {
  background-color: var(--light-green);
  color: var(--black);
}

#btn-editUser {
  background-color: var(--green);
}

#btn-banUser {
  background-color: var(--warning);
  color: var(--black);
}

.deleted-user {
  color: grey !important;
}

button .deleted-user {
  cursor: not-allowed !important;
}

@media screen and (max-width: 992px) {
  .table-container {
    margin: 20px 20px;
    max-width: 95%;
  }

  .table-hover>tbody>tr>td {
    padding: 10px;
  }

  .fungeye-default-button,
  .fungeye-red-button {
    height: 25px;
    padding: 0 10px;
    font-size: 0.8em;
  }

  .profile-picture {
    width: 20px;
    height: 20px;
  }

}
</style>
