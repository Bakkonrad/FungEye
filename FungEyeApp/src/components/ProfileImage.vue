<template>
  <div class="profile-img-container" :class="showBorderClass()" :style="{ width: `${width}px`, height: `${height}px` }">
    <img class="profile-img" :src="getProfileImage()" alt="Profile Image" :key="imgSrc" />
  </div>
</template>

<script>
import placeholder from "@/assets/images/profile-images/placeholder.png"

export default {
  name: "ProfileImage",
  props: {
    isPlaceholder: {
      type: Boolean
    },
    imgSrc: {
      type: String,
      required: true,
    },
    width: {
      type: Number,
      default: 50,
    },
    height: {
      type: Number,
      default: 50,
    },
    showBorder: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      placeholderPath: placeholder
    }
  },
  mounted() {
    this.showBorderClass();
  },
  methods: {
    getProfileImage() {
      // console.log('isPlaceholder:', this.isPlaceholder);
      // console.log('imgSrc:', this.imgSrc);

      if (this.isPlaceholder || !this.imgSrc || this.imgSrc === 'placeholder') {
        // console.log('Returning placeholder:', this.placeholderPath);
        return this.placeholderPath;
      }

      // console.log('Returning imgSrc:', this.imgSrc);
      return this.imgSrc;
    },
    showBorderClass() {
      if (this.showBorder == true) {
        return "profile-image-stroke";
      }
    },
  },
};
</script>

<style>
.profile-img-container {
  width: 4em;
  height: 4em;
  min-width: 50px;
  border-radius: 50%;
}

.profile-image-stroke {
  border: 2px solid var(--green);
  filter: drop-shadow(0px 4px 4px rgba(56, 102, 65, 0.2));
}

.profile-img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
}
</style>
