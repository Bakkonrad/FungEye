<template>
  <div class="edit-container">
    <div class="edit-form">
      <h3>Edycja profilu</h3>
      <form @submit.prevent="submitForm" class="form-group">
        <div class="mb-3 file-input-container">
          <input style="display: none;" type="file" accept="image/*" @change="onFileChange" ref="fileInput"></input>
          <div class="fileInput" @click="$refs.fileInput.click()">
            <span class="edit-pencil">&#9998;</span>
            <ProfileImage class="image" :isPlaceholder="isPlaceholder" :imgSrc="fileChanged ? tempImgSrc : registerFormData.imgSrc" :width="100"
              :height="100" />
          </div>
          <button v-if="fileChanged" class="btn fungeye-red-button revert" @click="revertImage()">&#10554; Przywróć
            poprzednie zdjęcie</button>
          <button v-if="!fileChanged && isPlaceholderImage() == false" class="btn fungeye-red-button revert" @click="deleteImage()">
            <font-awesome-icon icon="fa-solid fa-trash" class="button-icon" />
            Usuń zdjęcie</button>
        </div>
        <div class="mb-3">
          <BaseInput v-model="registerFormData.email" type="text" label="Email" :class="{
            'email-input': !submitted,
            validInput: submitted && !v$.email.$invalid,
            invalidInput: submitted && v$.email.$invalid,
          }" />
          <span class="error-message" v-for="error in v$.email.$errors" :key="error.$uid">
            {{ error.$message }}
          </span>
        </div>
        <div class="row mb-3">
          <div class="col">
            <BaseInput v-model="registerFormData.firstName" type="text" label="Imię" :class="{
              'firstName-input':
                !submitted || registerFormData.firstName === null,
              validInput: submitted && !v$.firstName.$invalid,
              invalidInput: submitted && v$.firstName.$invalid,
            }" />
            <span class="error-message" v-for="error in v$.firstName.$errors" :key="error.$uid">
              {{ error.$message }}
            </span>
          </div>
          <div class="col">
            <BaseInput v-model="registerFormData.lastName" type="text" label="Nazwisko" :class="{
              'lastName-input': !submitted,
              validInput: submitted && !v$.lastName.$invalid,
              invalidInput: submitted && v$.lastName.$invalid,
            }" />
            <span class="error-message" v-for="error in v$.lastName.$errors" :key="error.$uid">
              {{ error.$message }}
            </span>
          </div>
        </div>
        <div class="mb-3">
          <BaseInput v-model="registerFormData.username" type="text" label="Nazwa użytkownika" :class="{
            'username-input': !submitted,
            validInput: submitted && !v$.username.$invalid,
            invalidInput: submitted && v$.username.$invalid,
          }" />
          <span class="error-message" v-for="error in v$.username.$errors" :key="error.$uid">
            {{ error.$message }}
          </span>
        </div>
        <!-- <div class="mb-3">
          <BaseInput v-model="registerFormData.password" type="password" label="Hasło (min. 8 znaków)" :class="{
            'password-input': !submitted,
            validInput: submitted && !v$.password.$invalid,
            invalidInput: submitted && v$.password.$invalid,
          }" />
          <span class="error-message" v-for="error in v$.password.$errors" :key="error.$uid">
            {{ error.$message }}
          </span>
        </div>
        <div class="mb-3">
          <BaseInput v-model="registerFormData.confirmPassword" type="password" label="Potwierdź hasło" :class="{
            'confirmPassword-input': !submitted,
            validInput: submitted && !v$.confirmPassword.$invalid,
            invalidInput: submitted && v$.confirmPassword.$invalid,
          }" />
          <span class="error-message" v-for="error in v$.confirmPassword.$errors" :key="error.$uid">
            {{ error.$message }}
          </span>
        </div> -->
        <!-- <div class="mb-3">
          <BaseInput v-model="registerFormData.dateOfBirth" type="date" label="Data urodzenia" :class="{
            'dateOfBirth-input': !submitted,
            validInput: submitted && !v$.dateOfBirth.$invalid,
            invalidInput: submitted && v$.dateOfBirth.$invalid,
          }" />
          <span class="error-message" v-for="error in v$.dateOfBirth.$errors" :key="error.$uid">
            {{ error.$message }}
          </span>
        </div> -->
        <span class="error-message" v-if="error">{{ apiErrorMessage }}</span>
        <div class="button-group">
          <button type="submit" class="btn fungeye-default-button" @click="save">
            Zapisz zmiany
          </button>
          <button class="btn fungeye-red-button" @click="$emit('cancel-edit')">Anuluj</button>
        </div>
      </form>
    </div>
  </div>

</template>

<script>
import BaseInput from './BaseInput.vue';
import { useVuelidate } from '@vuelidate/core';
import { email, minLength, sameAs, helpers } from '@vuelidate/validators';
import { ref, reactive, computed, onMounted } from 'vue';
import UserService from '@/services/UserService';
import ProfileImage from './ProfileImage.vue';

export default {
  props: {
    user: {
      type: Object,
      required: true,
    }
  },
  components: {
    BaseInput,
    ProfileImage,
  },
  data() {
    return {
      error: false,
      apiErrorMessage: "",
      submitted: false,
      fileChanged: false,
      placeholderImage: '../src/assets/images/profile-images/placeholder.png',
      isPlaceholder: false,

    };
  },
  methods: {
    async save() {
      const result = await this.v$.$validate();
      this.submitted = true;

      const exportedData = {
        id: this.registerFormData.id,
        role: this.registerFormData.role,
        email: this.registerFormData.email,
        firstName: this.registerFormData.firstName,
        lastName: this.registerFormData.lastName,
        username: this.registerFormData.username,
        // imageUrl: this.registerFormData.imgSrc,
        createdAt: this.registerFormData.createdAt,
        dateOfBirth: this.registerFormData.dateOfBirth,
      };
      const imageFile = this.registerFormData.imgFile;

      // 1. mam placeholder, zmieniam na zdjęcie --> api potrzebuje imageUrl = 'placeholder'
      // 2. mam zdjęcie, usuwam je - zmieniam na placeholder --> api potrzebuje imageUrl = 'changeToPlaceholder'
      // 3. mam zdjęcie, zmieniam na inne zdjęcie --> api potrzebuje imageUrl = stare zdjęcie
      // 4. mam placeholder, nie zmieniam --> api potrzebuje imageUrl = 'placeholder'
      // 5. mam zdjęcie, nie zmieniam --> api potrzebuje imageUrl = stare zdjęcie

      console.log("isPlaceholder", this.isPlaceholder);
      console.log("fileChanged", this.fileChanged);
      console.log("user.imageUrl", this.user.imageUrl);

      if (this.fileChanged) {
        if (this.user.imageUrl === 'placeholder') {
          console.log(1);
          exportedData.imageUrl = "placeholder";
        } else if (this.isPlaceholder) {
          console.log(2);
          exportedData.imageUrl = "changeToPlaceholder";
        } else {
          console.log(3);
          exportedData.imageUrl = this.user.imageUrl;
        }
      } else {
        if (this.isPlaceholder) {
          console.log(4);
          exportedData.imageUrl = "placeholder";
        } else {
          console.log(5);
          exportedData.imageUrl = this.user.imageUrl;
        }
      }

      if (result) {
        try {
          const response = await UserService.updateUser(exportedData, imageFile);
          if (response.success) {
            this.$emit('save-user', response.data);
          } else {
            this.error = true;
            this.apiErrorMessage = response.message;
          }
        } catch (error) {
          this.error = true;
          this.apiErrorMessage = "Wystąpił błąd podczas zapisywania danych. Spróbuj ponownie.";
        }
      }
      else {
        this.error = true;
        this.apiErrorMessage = this.v$.$errors;
        // this.apiErrorMessage = "Niepoprawnie wypełnione pola formularza.";
      }
    },

    onFileChange(event) {
      this.isPlaceholder = false;
      const file = event.target.files[0];
      if (!file) return;
      event.target.value = "";
      this.fileChanged = true;
      this.tempImgSrc = URL.createObjectURL(file);
      this.registerFormData.imgFile = file;
    },
    revertImage() {
      this.fileChanged = false;
      this.tempImgSrc = null;
      this.registerFormData.imgFile = null;
    },
    deleteImage() {
      this.fileChanged = true;
      this.tempImgSrc = this.placeholderImage;
      this.isPlaceholder = true;
    },
    isPlaceholderImage() {
      if (this.registerFormData.imgSrc === "placeholder" || this.registerFormData.imgSrc === this.placeholderImage) {
        this.isPlaceholder = true;
        return true;
      }
      else {
        this.isPlaceholder = false;
        return false;
      }
    },
  },
  setup(props) {
    const registerFormData = reactive({
      id: props.user.id,
      role: props.user.role,
      email: null,
      firstName: null,
      lastName: null,
      username: null,
      imgSrc: props.user.imageUrl || "",
      imgFile: null,
      password: null,
      confirmPassword: null,
      createdAt: props.user.createdAt,
      dateOfBirth: null,
    });

    onMounted(() => {
      if (props.user) {
        registerFormData.email = props.user.email || "";
        registerFormData.firstName = props.user.firstName || "";
        registerFormData.lastName = props.user.lastName || "";
        registerFormData.imgSrc = props.user.imageUrl || "";
        registerFormData.username = props.user.username || "";
        registerFormData.dateOfBirth = props.user.dateOfBirth || "";
      }
    });

    const tempImgSrc = ref(registerFormData.imgSrc);

    const today = new Date().toISOString().split("T")[0];

    function betweenDates(value) {
      const minDate = new Date("1900-01-01");
      const maxDate = new Date();
      const date = new Date(value);
      return date >= minDate && date <= maxDate;
    }

    const rules = computed(() => {
      return {
        tempImgSrc: {},
        email: {
          email: helpers.withMessage("Nieprawidłowy adres email. ", email),
        },
        firstName: {},
        lastName: {},
        username: {
          minLength: helpers.withMessage(
            "Nazwa użytkownika powinna zawierać conajmniej 3 znaki. ",
            minLength(3)
          ),
        },
        password: {
          minLength: helpers.withMessage(
            "Hasło powinno zawierać conajmniej 8 znaków. ",
            minLength(8)
          ),
        },
        confirmPassword: {
          // only when password is changed
          // required: helpers.withMessage(
          //   "Potwierdzenie hasła jest wymagane. ",
          //   required
          // ),
          sameAsPassword: helpers.withMessage(
            "Hasła powinny być identyczne. ",
            sameAs(registerFormData.password)
          ),
        }
      };
    });

    const v$ = useVuelidate(rules, registerFormData);

    return {
      registerFormData,
      tempImgSrc,
      v$,
    };
  },
};
</script>

<style scoped>
.edit-container {
  display: flex;
  justify-content: center;
  align-items: center;
}

.edit-form {
  background-color: rgba(0, 0, 0, 0.85);
  padding: 40px;
  border-radius: 12px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  width: 500px;
}

h3 {
  margin-bottom: 1em;
  color: white;
}

.form-group {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.file-input-container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  cursor: pointer;
}

.file-input-container .fileInput {
  position: relative;
  width: 100px;
  height: 100px;
  margin-right: 10px;
  margin-bottom: 10px;
  border-radius: 10px;
  overflow: hidden;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.edit-pencil {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 999;
  font-size: 1.5em;
  color: white;
  display: none;
}

.file-input-container .fileInput:hover .image {
  opacity: 0.6;
  z-index: 1000;
}

.file-input-container .fileInput:hover .edit-pencil {
  display: block;
}

.revert {
  height: 35px;
  padding: 0 9px 0 9px;
  font-size: 1em;
}

.error-message {
  margin-bottom: 1em;
}

.button-group {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

@media screen and (max-width: 500px) {
  .edit-form {
    width: 90vw;
  }
  
}
</style>