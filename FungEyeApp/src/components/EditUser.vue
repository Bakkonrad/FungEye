<template>
    <div class="edit-container">
      <div class="edit-form">
        <div class="form-group">
          <label for="username">Nazwa Użytkownika</label>
          <input id="username" v-model="user.Username" type="text" class="form-control" />
        </div>
        <div class="form-group">
          <label for="email">Email</label>
          <input id="email" v-model="user.Email" type="text" class="form-control" disabled />
        </div>
        <div class="form-group">
          <label for="firstName">Imię</label>
          <input id="firstName" v-model="user.FirstName" type="text" class="form-control" />
        </div>
        <div class="form-group">
          <label for="lastName">Nazwisko</label>
          <input id="lastName" v-model="user.LastName" type="text" class="form-control" />
        </div>
        <div class="button-group">
          <button class="btn btn-success rounded-button save-button" @click="save">Zapisz</button>
          <button class="btn btn-danger rounded-button cancel-button" @click="$emit('cancel-edit')">Anuluj</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: ['user'],
    methods: {
      save() {
        this.$emit('save-user', { ...this.user });
      }
    }
  };
  </script>
  
  <style scoped>
  .edit-container {
    display: flex;
    justify-content: center;
    align-items: center;

  }
  
  .edit-form {
    background-color: white;
    padding: 40px;
    border-radius: 12px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    width: 400px;
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  label {
    display: block;
    margin-bottom: 5px;
    color: black; /* Ustawiono kolor tekstu na czarny */
  }
  
  input[type="text"] {
    width: 100%;
    padding: 10px;
    border-radius: 4px;
    border: 1px solid #ccc;
    color: black !important; /* Ustawiono kolor tekstu na czarny */
  }
  
  .button-group {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
  }
  
  .btn.rounded-button {
    border-radius: 20px;
  }

  /* .save-button {
    background-color: #6a994e;
    color: white;
    border: none;
  }
  
  .cancel-button {
    background-color: #cc0101; 
    color: white;
    border: none;
  }
*/
  
  </style>