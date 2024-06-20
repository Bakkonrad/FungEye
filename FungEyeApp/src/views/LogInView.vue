<template>
  <div class="container-md">
    <img class="log-in-bg" src="../assets/images/backgrounds/log-in-bg.jpeg" />
    <div class="log-in-content">
      <h1>Witaj ponownie!</h1>
      <p>Zaloguj się, aby móc w pełni korzystać z możliwości FungEye</p>
      <form>
        <div class="mb-3">
          <BaseInput autofocus v-model="loginFormData.email" type="text" label="Email" class="email-input"/>
        </div>
        <div class="mb-3">
          <BaseInput v-model="loginFormData.password" type="password" label="Hasło" class="password-input"/>
        </div>
        <!-- <div id="forgotPassword" class="form-text">Zapomniałeś/aś hasła?</div> -->
        <span v-if="error" class="error-message">Email/Login lub hasło są nieprawidłowe.</span>
        <button
          type="submit"
          class="btn fungeye-default-button submitFormButton"
        >
          Zaloguj się
        </button>
      </form>
      <span id="registerLink">
        <p>Nie masz jeszcze konta?</p>
        <RouterLink to="/register" class="router-registerLink"><p><b>Zarejestruj się</b></p></RouterLink>
      </span>
    </div>
  </div>
</template>

<script>
import BaseInput from "../components/BaseInput.vue";
import {ref } from "vue";

export default {
  components: {
    BaseInput,
  },
  data() {
    return {
      error: false,
    };
  },
  setup() {
    const loginFormData = ref({
      email: "",
      password: "",
    });
    return {
      loginFormData,
    };
  },
};

</script>

<style scoped>
.container-md {
  margin-top: 3em;
  display: flex;
  justify-content: center;
  align-items: center;
}

.log-in-bg {
  width: 100%;
  height: 670px;
  border-radius: 20px;
  position: relative;
}

.log-in-content {
  position: absolute;
  height: auto;

  width: 25vw;

  background: rgba(0, 0, 0, 0.45);
  backdrop-filter: blur(9.5px);
  border-radius: 30px;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 2em 2em 3em 2em;
}

h1 {
  color: white;
  font-size: 3em;
}

p {
  color: white;
  font-size: 1.3em;
}

@media (max-width: 768px) {
  .log-in-content {
    width: 80vw;
  }
}

#registerLink {
  color: white;
  display: flex;
  justify-content: center;
  gap: 0.5em;
  margin-top: 2em;
}

.router-registerLink {
  color: white;
  text-decoration: none;
}

.router-registerLink:hover {
  color: var(--light-green);
  text-decoration: underline;
}

</style>
