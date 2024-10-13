<template>
  <div class="container">
    <div class="content">
      <div class="picture-ad">
        <div class="img-container">
          <img
            class="register-bg"
            src="../assets/images/backgrounds/REGISTER-form-background.jpg"
          />
          <div class="black-curtain"></div>
        </div>
        <div v-if="!admin" class="join-text">
          <h1>Dołącz do społeczności!</h1>
          <p id="join-p">
            Zarejestruj się, aby móc w pełni korzystać z możliwości FungEye
          </p>
        </div>
        <div v-else class="join-text">
          <h1>Zarejestruj nowego administratora</h1>
        </div>
      </div>
      <div class="form-content">
        <form @submit.prevent="submitForm">
          <div class="mb-3">
            <BaseInput
              autofocus
              v-model="registerFormData.email"
              type="text"
              label="Email*"
              :class="{
                'email-input': !submitted,
                validInput: submitted && !v$.email.$invalid,
                invalidInput: submitted && v$.email.$invalid,
              }"
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
                :class="{
                  'firstName-input':
                    !submitted || registerFormData.firstName === null,
                  validInput: submitted && !v$.firstName.$invalid,
                  invalidInput: submitted && v$.firstName.$invalid,
                }"
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
                :class="{
                  'lastName-input': !submitted,
                  validInput: submitted && !v$.lastName.$invalid,
                  invalidInput: submitted && v$.lastName.$invalid,
                }"
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
              :class="{
                'username-input': !submitted,
                validInput: submitted && !v$.username.$invalid,
                invalidInput: submitted && v$.username.$invalid,
              }"
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
              label="Hasło* (min. 8 znaków)"
              :class="{
                'password-input': !submitted,
                validInput: submitted && !v$.password.$invalid,
                invalidInput: submitted && v$.password.$invalid,
              }"
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
              :class="{
                'confirmPassword-input': !submitted,
                validInput: submitted && !v$.confirmPassword.$invalid,
                invalidInput: submitted && v$.confirmPassword.$invalid,
              }"
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
              label="Data urodzenia"
              :class="{
                'dateOfBirth-input': !submitted,
                validInput: submitted && !v$.dateOfBirth.$invalid,
                invalidInput: submitted && v$.dateOfBirth.$invalid,
              }"
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
          <span class="error-message" v-if="error">{{ apiErrorMessage }}</span>
          <button
            v-if="!admin"
            type="submit"
            class="btn fungeye-default-button submitFormButton"
          >
            Zarejestruj się
          </button>
          <button
            v-else
            type="submit"
            class="btn fungeye-default-button submitFormButton"
          >
            Zarejestruj 
          </button>
        </form>
        <span v-if="!admin" id="registerLink">
          <p>Masz już konto?</p>
          <RouterLink to="/log-in" class="router-registerLink"
            ><p><b>Zaloguj się</b></p></RouterLink
          >
        </span>
      </div>
    </div>
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
  helpers,
} from "@vuelidate/validators";

import AuthService from "@/services/AuthService";

export default {
  name: "RegisterView",
  components: {
    Footer,
    BaseInput,
  },
  props: {
    admin: Boolean,
  },
  data() {
    return {
      error: false,
      apiErrorMessage: "",
      submitted: false,
    };
  },
  methods: {
    // randomNumber generator from 1 to 99
    randomNumber() {
      return Math.floor(Math.random() * 99) + 1;
    },
    async submitForm() {
      const result = await this.v$.$validate();
      this.submitted = true;

      const exportedData = {
        email: this.registerFormData.email,
        firstName: this.registerFormData.firstName,
        lastName: this.registerFormData.lastName,
        imageUrl: "placeholder",
        username: this.registerFormData.username,
        password: this.registerFormData.password,
        dateOfBirth: this.registerFormData.dateOfBirth,
      };

      if (result) {
        try {
          const response = await AuthService.register(this.admin, exportedData);
          if (response === true) {
            console.log("Form submitted!");
            if (this.admin) {
              this.$router.push("/admin");
            } else {
              this.$router.push("/log-in");
            }
            // this.$router.push("/log-in");
          } else {
            console.log("Form not submitted!");
            this.error = true;
            this.apiErrorMessage = response.message;
          }
        } catch (error) {
          console.error("Error submitting form: ", error);
          this.error = true;
          this.apiErrorMessage = response.message;
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
    }

    const rules = computed(() => {
      return {
        email: {
          required: helpers.withMessage("Email jest wymagany. ", required),
          email: helpers.withMessage("Nieprawidłowy adres email. ", email),
        },
        firstName: {},
        lastName: {},
        username: {
          required: helpers.withMessage(
            "Nazwa użytkownika jest wymagana. ",
            required
          ),
          minLength: helpers.withMessage(
            "Nazwa użytkownika powinna zawierać conajmniej 3 znaki. ",
            minLength(3)
          ),
        },
        password: {
          required: helpers.withMessage("Hasło jest wymagane. ", required),
          minLength: helpers.withMessage(
            "Hasło powinno zawierać conajmniej 8 znaków. ",
            minLength(8)
          ),
        },
        confirmPassword: {
          required: helpers.withMessage(
            "Potwierdzenie hasła jest wymagane. ",
            required
          ),
          sameAsPassword: helpers.withMessage(
            "Hasła powinny być identyczne. ",
            sameAs(registerFormData.password)
          ),
        },
        dateOfBirth: {
          validDate: helpers.withMessage("Nieprawidłowa data. ", (value) => {
            return !isNaN(new Date(value).getTime());
          }),
          betweenDates: helpers.withMessage(
            "Data urodzenia musi być pomiędzy 1900-01-01 a " + today + ". ",
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
.container {
  margin-top: 3em;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
}

.content {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  gap: 8em;
  width: 100%;
  height: auto;
}

.picture-ad {
  position: relative;
  height: 92vh;
  min-width: 100px;
}

.img-container {
  display: flex;
  justify-content: center;
  align-items: center;
  position: absolute;
}

.black-curtain,
.register-bg {
  width: 35vw;
  height: 92vh;
  border-radius: 20px;
  min-width: 100px;
}

.black-curtain {
  background: rgb(0, 0, 0, 0.35);
  backdrop-filter: blur(1.5px);
  position: absolute;
}

.register-bg {
  object-fit: cover;
  position: relative;
}

.join-text {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  color: white;
  width: 30vw;
  padding: 2em 3em 0 2em;
}

h1 {
  font-size: 4em;
  padding-bottom: 0.2em;
}

#join-p {
  font-size: 1.8em;
}

p {
  font-size: 1.3em;
}

input.firstName-input {
  background: rgba(255, 255, 255, 0.3) !important;
  border: 1px solid rgba(56, 102, 65, 0.2) !important;
}

#requiredFields {
  font-size: 1.1em;
  color: white;
  padding-bottom: 0.3em;
}

.form-content {
  position: relative;

  width: 30vw;
  height: auto;

  background: rgba(0, 0, 0, 0.85);
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
  text-decoration: none;
}

.router-registerLink p {
  color: white;
  transition: text-decoration 0.5s;
}

.router-registerLink:hover p{
  text-decoration: underline;
}

@media screen and (max-width: 1400px) {
  .container {
    margin-left: 3em;
  }
  .content {
    flex-direction: column;
    gap: 2em;
  }
  .picture-ad {
    height: 62vh;
  }
  .black-curtain,
  .register-bg {
    width: 85vw;
    height: 62vh;
  }
  .join-text {
    width: 80vw;
  }
  .form-content {
    width: 85vw;
  }
}

@media screen and (max-width: 768px) {
  .container {
    margin-left: 0;
  }
  .black-curtain,
  .register-bg {
    width: 95vw;
  }
  .form-content {
    width: 95vw;
  }
}

@media screen and (max-width: 600px) {
  .container {
    margin-left: 0;
  }
  .content {
    gap: 2em;
  }
  .picture-ad {
    height: 25vh;
  }
  .black-curtain,
  .register-bg {
    width: 90vw;
    height: 25vh;
  }
  .join-text {
    width: 80vw;
  }
  h1 {
    font-size: 2.5em;
  }
  #join-p {
    font-size: 1.1em;
  }
  .form-content {
    width: 90vw;
  }
}
</style>
