<template>
    <div class="container-md reset-password-container">
        <div v-if="!token">
            <h2>Brak dostępu!</h2>
            <p>Link do resetowania hasła jest niepoprawny</p>
            <RouterLink to="/log-in" class="btn fungeye-default-button">
                Przejdź do strony logowania
            </RouterLink>
        </div>
        <div v-else>
            <RouterLink to="/log-in" class="back-to-login">
                <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon" />
                Powrót do logowania
            </RouterLink>
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
import BaseInput from "../components/BaseInput.vue";
import { useVuelidate } from "@vuelidate/core";
import { required, minLength, sameAs, helpers } from "@vuelidate/validators";
import { reactive, computed } from "vue";
import AuthService from "@/services/AuthService";

export default {
    components: {
        BaseInput,
    },
    data() {
        return {
            error: false,
            errorMessage: "",
            submitted: null
        };
    },
    props: {
        token: {
            type: String,
            required: true,
            default: ''
        }
    },
    setup() {
        const formData = reactive({
            password: null,
            confirmPassword: null,
        });

        const rules = computed(() => {
            return {
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
        async resetPassword() {
            this.submitted = true;
            const result = await this.v$.$validate();
            if (!result) {
                return;
            }
            const response = await AuthService.resetPassword(this.token, this.formData.password);
            if (!response.success) {
                this.error = true;
                this.errorMessage = response.message;
                return;
            }
            this.$router.push("/log-in");
        },
    }
}
</script>

<style scoped>
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
</style>