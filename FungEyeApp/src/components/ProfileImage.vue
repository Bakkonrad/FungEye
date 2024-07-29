<template>
  <div class="profile-img-container" :style="{ width: `${width}px`, height: `${height}px` }">
    <img
      class="profile-img"
      :src="imgSrc"
      alt="Profile Image"
      
    />
  </div>
</template>

<script>
export default {
  name: "ProfileImage",
  props: {
    imgSrc: {
      type: String,
      required: true,
      default: "https://picsum.photos/200",
    },
    width: {
      type: Number,
      default: 50,
    },
    height: {
      type: Number,
      default: 50,
    },
  }
};
</script>

<style>
.profile-img-container {
  width: 4em;
  height: 4em;
  border-radius: 50%;
  /* profile-image-stroke */
  border: 2px solid var(--green);
  /* profile-image-shadow */
  filter: drop-shadow(0px 4px 4px rgba(56, 102, 65, 0.2));
}

.profile-img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
}

</style>
