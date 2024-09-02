<template>
    <div>
      <UserTable :users="users" @delete-user="DeleteUser"/>
    </div>
  </template>
  
  <script>
  import UserTable from './UserTable.vue';
  
  export default {
    components: {
      UserTable
    },
    data() {
      return {
        users: [
        { Username: 'MPudzianowski', Email: 'mpudzianowski@gmail.com', FirstName: 'Mariusz', LastName: 'Pudzianowski' },
        { Username: 'RLewandowski', Email: 'rlewandowski@gmail.com', FirstName: 'Robert', LastName: 'Lewandowski' },
        { Username: 'AMałysz', Email: 'amalysz@gmail.com', FirstName: 'Adam', LastName: 'Małysz' },
        { Username: 'ATest', Email: 'atest@gmail.com', FirstName: 'Adam', LastName: 'Test' },
        ]
      };
    },
    methods: {
      DeleteUser(email) {
        this.users = this.users.filter(user => user.Email !== email);
      }
    }
  };
  </script>