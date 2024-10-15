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
      <form @submit.prevent="submitForm">
        <div class="mb-3">
          <BaseInput v-model="loginFormData.email" type="text" label="Login lub email" class="email-input"
            :class="{ invalidInput: error }" @focus="resetValidation" />
        </div>
        <div class="mb-3">
          <BaseInput v-model="loginFormData.password" type="password" label="Hasło" class="password-input"
            @focus="resetValidation" :class="{ invalidInput: error }" />
        </div>
        <span v-if="error" class="error-message">{{ errorMessage }}</span>
        <button type="submit" class="btn fungeye-default-button submitFormButton">
          Zaloguj się
        </button>
      </form>
      <RouterLink to="/forgot-password" id="forgotPassword" class="btn fungeye-secondary-button submitFormButton">Nie
        pamiętasz hasła?
      </RouterLink>
      <span id="registerLink">
        <p id="noAccount">Nie masz jeszcze konta?</p>
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

import AuthService from "@/services/AuthService";

export default {
  components: {
    BaseInput,
  },
  data() {
    return {
      error: false,
      errorMessage: "",
      loggedIn: false,
    };
  },
  methods: {
    async submitForm() {
      const result = await this.v$.$validate();
      if (!result) {
        this.error = true;
        return;
      }
      const loginData = {
        email: this.loginFormData.email.includes("@") ? this.loginFormData.email : null,
        username: this.loginFormData.email.includes("@") ? null : this.loginFormData.email,
        password: this.loginFormData.password,
      };
      try {
        const result = await AuthService.login(loginData);
        if (!result.success) {
          this.error = true;
          this.errorMessage = result.message;
          return;
        }
        this.$router.push("/my-profile");
        this.resetValidation();
      }
      catch (error) {
        this.errorMessage = result.message;
        this.error = true;
      }
    },
    async logOut() {
      AuthService.logout();
      this.$router.push("/");
    },
    resetValidation() {
      this.error = false;
      this.errorMessage = "";
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
  padding: 2em 2em 2em 2em;
}

h1 {
  color: white;
  font-size: 3em;
}

p {
  color: white;
  font-size: 1.3em;
}

.submitFormButton {
  margin-top: 1.5em;
}

@media (max-width: 768px) {
  h1 {
    font-size: 2.5em;
  }

  .log-in-content {
    width: 80vw;
  }

  #noAccount {
    margin: 0 !important;
  }

  #registerLink {
    align-items: center;
    flex-direction: column;
    gap: 0 !important;
  }

  .router-registerLink {
    margin-top: 0;
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

@media screen and (max-width: 465px) {
  .log-in-bg {
    width: 97vw;
  }

  .log-in-content {
    width: 90vw;
    padding: 2em 0.5em 2em 0.5em;
  }

  .log-in-content p {
    font-size: 1.1em;
  }

  #noAccount {
    margin: 0 !important;
  }

  #registerLink {
    margin-top: 1.5em !important;
    gap: 0 !important;
  }
}

@media screen and (max-width: 375px) {
  .log-in-content {
    padding: 2em 0.5em 2em 0.5em;
  }

  h1 {
    font-size: 2.5em;
  }

  p {
    font-size: 1.1em;
  }

}

#forgotPassword {
  margin-top: 1em;
  justify-content: center;
  text-decoration: none;
}

#registerLink {
  color: white;
  display: flex;
  justify-content: center;
  gap: 0.5em;
  margin-top: 3em;
}

.router-registerLink {
  text-decoration: none;
  margin: 0 !important;
}

.router-registerLink p {
  color: white;
  transition: text-decoration 0.5s;
}

.router-registerLink:hover p {
  text-decoration: underline;
}
</style>
