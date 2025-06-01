<template>
    <div class="left-side">
      <ProfileImage :imgSrc="imgSrc" :width="170" :height="170" />
      <div id="user-text">
        <p id="username">{{ username }}</p>
        <p v-if="name_surname" id="name_surname">{{ name_surname }}</p>
        <p v-if="email" id="email">{{ email }}</p>
        <p id="createdAt">Członek od: {{ formatDate(createdAt) }}</p>
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
    email: {
      type: String,
      Required: false,
    },
    createdAt: String,
  },
  methods: {
    formatDate(date) {
      return new Date(date).toLocaleDateString();
    },
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
  margin-top: 0.7em;
  margin-bottom: 0.7rem !important;
}

#name_surname {
  font-size: 1.2em;
  font-weight: 400;
  margin-bottom: 0.4rem !important;
}

#createdAt {
  font-size: 1em;
  font-weight: 300;
}

@media screen and (max-width: 400px) {
  .left-side {
    display: flex;
    flex-direction: column !important;
    align-items: center;
  }
}
</style>
