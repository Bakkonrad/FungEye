<template>
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
          <div class="mb-3">
            <BaseInput
              v-model="registerFormData.dateOfBirth"
              type="date"
              label="Data urodzenia*"
              class="date-input"
            />
            <span
              class="error-message"
              v-for="error in v$.dateOfBirth.$errors"
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
import Footer from "@/components/Footer.vue";
import BaseInput from "@/components/BaseInput.vue";
import { reactive, computed } from "vue";
import { useVuelidate } from "@vuelidate/core";
import {
  required,
  email,
  minLength,
  sameAs,
  between,
  helpers,
} from "@vuelidate/validators";

import UserService from "@/services/UserService";

export default {
  name: "RegisterView",
  components: {
    Footer,
    BaseInput,
  },
  methods: {
    async submitForm() {
      const result = await this.v$.$validate();

      const exportedData = {
        email: this.registerFormData.email,
        firstName: this.registerFormData.firstName,
        lastName: this.registerFormData.lastName,
        username: this.registerFormData.username,
        password: this.registerFormData.password,
        dateOfBirth: this.registerFormData.dateOfBirth,
      };

      if (result) {
        const response = await UserService.register(exportedData);
        // console.log(exportedData);
        // const response = true;
        if (response === true) {
          alert("Form submitted!");
        } else {
          alert("Form not submitted!");
        }
      }
    },
  },
  setup() {
    const registerFormData = reactive({
      email: null,
      firstName: null,
      lastName: null,
      username: null,
      password: null,
      confirmPassword: null,
      dateOfBirth: null,
    });
    
    const today = new Date().toISOString().split("T")[0];

    function betweenDates(value) {
      const minDate = new Date("1900-01-01");
      const maxDate = new Date();
      const date = new Date(value);
      return date >= minDate && date <= maxDate;
    };

    const rules = computed(() => {
      return {
        email: {
          required: helpers.withMessage("Email jest wymagany", required),
          email: helpers.withMessage("Nieprawidłowy adres email", email),
        },
        firstName: {},
        lastName: {},
        username: {
          required: helpers.withMessage("Nazwa użytkownika jest wymagana", required),
          minLength: helpers.withMessage("Nazwa użytkownika powinna zawierać conajmniej 3 znaki", minLength(3)),
        },
        password: {
          required: helpers.withMessage("Hasło jest wymagane", required),
          minLength: helpers.withMessage("Hasło powinno zawierać conajmniej 8 znaków", minLength(8)),
        },
        confirmPassword: {
          required: helpers.withMessage("Potwierdzenie hasła jest wymagane", required),
          sameAsPassword: helpers.withMessage("Hasła powinny być identyczne", sameAs(registerFormData.password)),
        },
        dateOfBirth: {
          validDate: helpers.withMessage("Nieprawidłowa data", (value) => {
            return !isNaN(new Date(value).getTime());
          }),
          betweenDates: helpers.withMessage(
            "Data urodzenia musi być pomiędzy 1900-01-01 a " + today,
            (value) => {
              // First, check if the date is valid. If not, skip this validation.
              if (isNaN(new Date(value).getTime())) {
                return true; // Return true to not trigger this validation message when the date is invalid.
              }
              // If the date is valid, proceed with the betweenDates validation.
              return betweenDates(value);
            }
          ),
        },
      };
    });

    const v$ = useVuelidate(rules, registerFormData);

    return {
      registerFormData,
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
