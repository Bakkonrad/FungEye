<template>
  <div class="profile-button">
    <button type="button" class="btn fungeye-default-button">
      <ProfileImage
        :imgSrc="imgSrc"
        :width="50"
        :height="50"
      />
      Mój profil
    </button>
  </div>
</template>

<script>
import ProfileImage from "./ProfileImage.vue";

export default {
  name: "MyProfileButton",
  components: {
    ProfileImage,
  },
  data() {
    return {
        imgSrc: "https://picsum.photos/200/300"
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
</style>
