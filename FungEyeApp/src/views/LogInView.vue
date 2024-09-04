<template>
  <div v-if="loggedIn" class="logged-in container-md">
    <h1 id="loggedIn-h1">Jesteś już zalogowany!</h1>
    <div class="buttons">
      <router-link to="/my-profile" class="btn fungeye-default-button">Przejdź do swojego profilu</router-link>
      <button @click="logOut" class="btn fungeye-red-button">
        Wyloguj się
      </button>
    </div>
  </div>
  <div v-else class="container-md">
    <img class="log-in-bg" src="../assets/images/backgrounds/LOGIN-form-background.jpg" />
    <div class="log-in-content">
      <h1>Witaj ponownie!</h1>
      <p>Zaloguj się, aby móc w pełni korzystać z możliwości FungEye</p>
      <form>
        <div class="mb-3">
          <BaseInput v-model="loginFormData.email" type="text" label="Login lub email" class="email-input"
            :class="{ invalidInput: error }" />
        </div>
        <div class="mb-3">
          <BaseInput v-model="loginFormData.password" type="password" label="Hasło" class="password-input"
            :class="{ invalidInput: error }" />
        </div>
        <!-- <div id="forgotPassword" class="form-text">Zapomniałeś/aś hasła?</div> -->
        <span v-if="error" class="error-message">{{ errorMessage }}</span>
        <button type="submit" class="btn fungeye-default-button submitFormButton" @click.prevent="submitForm">
          Zaloguj się
        </button>
      </form>
      <span id="registerLink">
        <p>Nie masz jeszcze konta?</p>
        <RouterLink to="/register" class="router-registerLink">
          <p><b>Zarejestruj się</b></p>
        </RouterLink>
      </span>
    </div>
  </div>
</template>

<script>
import BaseInput from "../components/BaseInput.vue";
import { useVuelidate } from "@vuelidate/core";
import { required } from "@vuelidate/validators";
import { ref } from "vue";
import { isLoggedIn, checkAuth } from "@/services/AuthService";

import UserService from "@/services/UserService";

export default {
  components: {
    BaseInput,
  },
  data() {
    return {
      error: false,
      errorMessage: "",
      loggedIn: false,
      // error: true,
    };
  },
  methods: {
    async submitForm() {
      const result = await this.v$.$validate();
      if (!result) {
        this.error = true;
        return;
      }
      if (!this.loginFormData.email.includes("@")) {
        this.loginFormData.username = this.loginFormData.email;
        this.loginFormData.email = null;
      } else {
        this.loginFormData.username = null;
      }
      try {
        const result = await UserService.login(this.loginFormData);
        if (!result.success) {
          this.error = true;
          this.errorMessage = result.message;
          this.loginFormData.password = null;
          return;
        }
        this.$router.push("/my-profile");
      }
      catch (error) {
        this.errorMessage = result.message;
        this.error = true;
      }
    },
    async logOut() {
      UserService.logout();
      this.$router.push("/");
    },
  },
  setup() {
    checkAuth();
    if (isLoggedIn === true) {
      return {
        loggedIn: isLoggedIn,
      };
    }

    const loginFormData = ref({
      username: null,
      email: null,
      password: null,
    });

    const rules = {
      email: { required },
      password: { required },
    };

    const v$ = useVuelidate(rules, loginFormData);

    return {
      loggedIn: isLoggedIn,
      loginFormData,
      v$,
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

.logged-in {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1em;
}

#loggedIn-h1 {
  color: var(--black);
}

.buttons {
  display: flex;
  flex-direction: row;
  gap: 1em;
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
  max-height: 670px;

  width: 60vw;
  max-width: 500px;
  min-width: 300px;

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

  #registerLink {
    align-items: center;
    flex-direction: column;
    gap: 0;
  }

  .buttons {
    flex-direction: column;
    gap: 1em;
  }

  .logged-in {
    width: 80vw;
    margin-left: 2em;
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
  color: var(--light-green) !important;
  text-decoration: underline;
}
</style>
