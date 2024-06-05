<template>
  <Navbar></Navbar>
  <div class="container-md">
    <img
      class="register-bg"
      src="../assets/images/backgrounds/register-bg.jpg"
    />
    <div class="content">
      <div class="join-text">
        <h1>Dołącz do społeczności!</h1>
        <p id="join-p">
          Zarejestruj się, aby móc w pełni korzystać z możliwości FungEye
        </p>
      </div>
      <div class="form-content">
        <form @submit.prevent="submitForm">
          <div class="mb-3">
            <BaseInput
              autofocus
              v-model="registerFormData.email"
              type="text"
              label="Email*"
              class="email-input"
            />
            <span
              class="error-message"
              v-for="error in v$.email.$errors"
              :key="error.$uid"
            >
              {{ error.$message }}
            </span>
          </div>
          <div class="row mb-3">
            <div class="col">
              <BaseInput
                v-model="registerFormData.firstName"
                type="text"
                label="Imię"
                class="firstName-input"
              />
              <span
                class="error-message"
                v-for="error in v$.firstName.$errors"
                :key="error.$uid"
              >
                {{ error.$message }}
              </span>
            </div>
            <div class="col">
              <BaseInput
                v-model="registerFormData.lastName"
                type="text"
                label="Nazwisko"
                class="lastName-input"
              />
              <span
                class="error-message"
                v-for="error in v$.lastName.$errors"
                :key="error.$uid"
              >
                {{ error.$message }}
              </span>
            </div>
          </div>
          <div class="mb-3">
            <BaseInput
              v-model="registerFormData.username"
              type="text"
              label="Nazwa użytkownika*"
              class="username-input"
            />
            <span
              class="error-message"
              v-for="error in v$.username.$errors"
              :key="error.$uid"
            >
              {{ error.$message }}
            </span>
          </div>
          <div class="mb-3">
            <BaseInput
              v-model="registerFormData.password"
              type="password"
              label="Hasło*"
              class="password-input"
            />
            <span
              class="error-message"
              v-for="error in v$.password.$errors"
              :key="error.$uid"
            >
              {{ error.$message }}
            </span>
          </div>
          <div class="mb-3">
            <BaseInput
              v-model="registerFormData.confirmPassword"
              type="password"
              label="Potwierdź hasło*"
              class="confirmPassword-input"
            />
            <span
              class="error-message"
              v-for="error in v$.confirmPassword.$errors"
              :key="error.$uid"
            >
              {{ error.$message }}
            </span>
          </div>
          <div id="requiredFields" class="form-text">* Pola obowiązkowe</div>
          <button
            type="submit"
            class="btn fungeye-default-button submitFormButton"
          >
            Zarejestruj się
          </button>
        </form>
        <span id="registerLink">
          <p>Masz już konto?</p>
          <RouterLink to="/log-in" class="router-registerLink"
            ><p><b>Zaloguj się</b></p></RouterLink
          >
        </span>
      </div>
    </div>
  </div>
  <div>
    <Footer></Footer>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Footer from "@/components/Footer.vue";
import BaseInput from "@/components/BaseInput.vue";
import { ref, reactive, computed } from "vue";
import { useVuelidate } from "@vuelidate/core";
import { required, email, minLength, sameAs } from "@vuelidate/validators";

export default {
  name: "RegisterView",
  components: {
    Navbar,
    Footer,
    BaseInput,
  },
  setup() {
    const registerFormData = reactive({
      email: "",
      firstName: "",
      lastName: "",
      username: "",
      password: "",
      confirmPassword: "",
    });

    const rules = computed(() => {
      return {
        email: {
          required,
          email,
        },
        firstName: {},
        lastName: {},
        username: {
          required,
          minLength: minLength(3),
        },
        password: {
          required,
          minLength: minLength(8),
        },
        confirmPassword: {
          required,
          sameAsPassword: sameAs(registerFormData.password),
        },
      };
    });

    const v$ = useVuelidate(rules, registerFormData);

    const submitForm = async () => {
      const result = await v$.value.$validate();
      if (result) {
        alert("Form submitted!");
      } else {
        alert("Form not submitted!");
      }
    };

    return {
      registerFormData,
      submitForm,
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
  position: relative;
}

.register-bg {
  width: 100%;
  height: auto;
  border-radius: 20px;
}

.content {
  position: absolute;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  color: white;
  width: 90%;
  height: 95%;
}

.join-text {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  color: white;
  width: 30vw;
  padding: 2em 2em 30em 1em;
}

h1 {
  font-size: 5em;
  padding-bottom: 0.2em;
}

#join-p {
  font-size: 1.8em;
}

p {
  font-size: 1.3em;
}

#requiredFields {
  font-size: 1.1em;
  color: white;
  padding-bottom: 0.3em;
}

.error-message {
  color: var(--red);
  font-size: 1em;
}

.form-content {
  position: relative;

  width: 30vw;
  height: auto;

  background: rgba(0, 0, 0, 0.45);
  backdrop-filter: blur(9.5px);
  border-radius: 30px;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 2em 2em 3em 2em;
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
