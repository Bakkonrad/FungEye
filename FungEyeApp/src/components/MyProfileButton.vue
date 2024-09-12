<template>
  <div class="profile-button">
    <button type="button" class="btn fungeye-default-button">
      <ProfileImage :imgSrc="imgSrc" :width="50" :height="50" />
      <span class="profile-button-text">
        Mój profil
      </span>
    </button>
  </div>
</template>

<script>
import { profileImage } from "@/services/AuthService";
import ProfileImage from "./ProfileImage.vue";

export default {
  name: "MyProfileButton",
  components: {
    ProfileImage,
  },
  data() {
    return {
      imgSrc: profileImage,
    };
  },
  setup() {
    return {
      imgSrc: profileImage,
    };
  },
};
</script>

<style scoped>
.fungeye-default-button {
  display: flex;
  justify-content: flex-start !important;
  align-content: flex-start !important;
  padding-left: 0 !important;
  gap: 0.3em;
}

.profile-button {
  justify-content: flex-end;
}

@media screen and (max-width: 1400px) {
  .fungeye-default-button {
    background-color: var(--beige) !important;
    gap: 0;
  }
  .profile-button-text {
    display: none;
  }
  
}

@media screen and (max-width: 991px) {
  .fungeye-default-button {
    background-color: var(--green) !important;  
    gap: 0.3em;
  }
  .profile-button-text {
    display: flex;
  }

}
</style>