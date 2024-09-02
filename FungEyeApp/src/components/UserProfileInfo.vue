<template>
    <div class="left-side">
      <ProfileImage :imgSrc="imgSrc" :width="170" :height="170" />
      <div id="user-text">
        <p id="username">{{ username }}</p>
        <p v-if="name_surname" id="name_surname">{{ name_surname }}</p>
        <p id="email">{{ email }}</p>
      </div>
    </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";
export default {
  components: {
    ProfileImage,
  },
  props: {
    imgSrc: String,
    username: String,
    name_surname: String,
    email: String,
  },
};
</script>

<style scoped>
#user-text {
  display: flex;
  flex-direction: column;
}

#username {
  font-size: 1.5em;
  font-weight: 500;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
}
</style>
