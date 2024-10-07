<template>
    <h1 class="page-title">Odzyskiwanie hasła</h1>
    <div class="container-md">
        <RouterLink to="/log-in" class="back-to-login">
            <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon" />
            Powrót do logowania
        </RouterLink>
        <div v-if="!resetConfirmed" class="find-account">
            <div v-if="!emailSent">
                <h2>Znajdź swoje konto</h2>
                <p>Wprowadź adres e-mail użyty do tego konta, aby odzyskać hasło</p>
                <div class="mb-3">
                    <BaseInput v-model="formData.email" type="text" class="email-input" color="black"
                        :class="{ invalidInput: error }" />
                </div>
                <p v-for="error in v$.email.$errors" :key="error.$uid" class="error-message">{{ error.$message }}</p>
                <button @click="searchForEmail" class="btn fungeye-default-button submitFormButton">Dalej</button>
            </div>
            <!-- Email found -->
            <div v-if="emailSent" class="found-account">
                <h2>Wysłano e-mail</h2>
                <p>Na adres e-mail <b>{{ formData.email }}</b> został wysłany link do zresetowania hasła.</p>
                <br>
                <h3>Email nie dotarł?</h3>
                <p>Sprawdź w folderze Spam lub</p>
                <button @click="sendEmail" class="btn fungeye-default-button submitFormButton">Wyślij ponownie</button>
            </div>
        </div>
        <div v-if="resetConfirmed" class="reset-password-container">
            <h2>Resetuj hasło</h2>
            <p>Wprowadź nowe hasło</p>
            <form @submit.prevent="resetPassword">
                <div class="mb-3">
                    <BaseInput v-model="formData.password" type="password" label="Hasło* (min. 8 znaków)" :class="{
                        'password-input': !submitted,
                        validInput: submitted && !v$.password.$invalid,
                        invalidInput: submitted && v$.password.$invalid,
                    }" color="black" />
                    <span class="error-message" v-for="error in v$.password.$errors" :key="error.$uid">
                        {{ error.$message }}
                    </span>
                </div>
                <div class="mb-3">
                    <BaseInput v-model="formData.confirmPassword" type="password" label="Potwierdź hasło*" :class="{
                        'confirmPassword-input': !submitted,
                        validInput: submitted && !v$.confirmPassword.$invalid,
                        invalidInput: submitted && v$.confirmPassword.$invalid,
                    }" color="black" />
                    <span class="error-message" v-for="error in v$.confirmPassword.$errors" :key="error.$uid">
                        {{ error.$message }}
                    </span>
                </div>
                <p v-if="error" class="error-message">{{ errorMessage }}</p>
                <button type="submit" class="btn fungeye-default-button submitFormButton">Zresetuj
                    hasło</button>
            </form>
        </div>
    </div>
</template>

<script>
import { RouterLink } from "vue-router";
import BaseInput from "../components/BaseInput.vue";
import UserService from "@/services/UserService";
import { useVuelidate } from "@vuelidate/core";
import {
    required,
    email,
    minLength,
    sameAs,
    helpers,
} from "@vuelidate/validators";
import { computed, reactive } from "vue";
import AuthService from "@/services/AuthService";

export default {
    components: {
        BaseInput,
    },
    data() {
        return {
            email: null,
            error: false,
            errorMessage: "",
            emailSent: false,
            resetConfirmed: false,
            submitted: false,
        };
    },
    setup() {
        const formData = reactive({
            password: null,
            confirmPassword: null,
            email: null,
        });

        const rules = computed(() => {
            return {
                email: {
                    required: helpers.withMessage("Adres e-mail jest wymagany. ", required),
                    email: helpers.withMessage("Niepoprawny adres e-mail. ", email),
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
                    minLength: helpers.withMessage(
                        "Hasło powinno zawierać conajmniej 8 znaków. ",
                        minLength(8)
                    ),
                    sameAsPassword: helpers.withMessage(
                        "Hasła powinny być identyczne. ",
                        sameAs(formData.password)
                    ),
                },
            }
        });

        const v$ = useVuelidate(rules, formData);

        return {
            formData,
            v$
        };
    },
    methods: {
        async searchForEmail() {
            const result = await this.v$.email.$validate();
            if (!result) {
                this.error = true;
                return;
            }
            // const response = UserService.userExists(this.email);
            console.log(this.email);
            const response = true; // user exists
            // const response = false; // user does not exist
            if (response) {
                this.error = false;
                this.sendEmail();
            } else {
                this.error = true;
                this.errorMessage = "Nie znaleziono konta o podanym adresie e-mail";
            }
        },
        sendEmail() {
            const response = AuthService.sendResetPasswordEmail(this.email);
            if (response.success === false) {
                this.error = true;
                this.errorMessage = response.message;
                return;
            }
            this.emailSent = true;
            console.log("Email sent");
        },
        async resetPassword() {
            const validPassword = await this.v$.password.$validate();
            const validConfirmPassword = await this.v$.confirmPassword.$validate();
            if (!validPassword || !validConfirmPassword) {
                this.error = true;
                return;
            }
            this.submitted = true;
            this.error = false;
            console.log("Resetting password");
        },
    },
};

</script>

<style scoped>
b {
    font-weight: 600;
}

.page-title {
    text-align: center;
    margin-bottom: 2rem;
}

.container-md {
    max-width: 600px;
}

.find-account,
.reset-password-container {
    max-width: 500px;
    margin: 0 auto;
    padding: 2rem;
    background-color: var(--beige);
    border-radius: 5px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.back-to-login {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    margin-bottom: 1rem;
    color: var(--black);
    text-decoration: none;
}

.back-to-login:hover {
    text-decoration: underline;
}

.find-account h2 {
    margin-bottom: 1rem;
}

.find-account p {
    margin-bottom: 1rem;
}

.find-account .email-input,
.reset-password-container .password-input,
.reset-password-container .confirmPassword-input {
    width: 100%;
    color: var(--black);
}

input {
    width: 100%;
    color: var(--black) !important;
}

.form-label {
    color: var(--black) !important;
}

.error-message {
    font-weight: 500;
}
</style>