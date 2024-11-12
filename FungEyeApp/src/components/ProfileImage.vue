<template>
  <div class="profile-img-container" :class="showBorderClass()" :style="{ width: `${width}px`, height: `${height}px` }">
    <img class="profile-img" :src="imageSrc" alt="Profile Image" @error="handleImageError" />
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
      String
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
      imageSrc: "",
    }
  },
  watch: {
    imgSrc: {
      immediate: true,
      handler(newVal) {
        this.setImageSrc(newVal);
      }
    }
  },
  mounted() {
    this.showBorderClass();
    this.setImageSrc(this.imgSrc);
  },
  computed: {
    placeholderPath() {
      return placeholder;
    }
  },
  methods: {
    showBorderClass() {
      if (this.showBorder == true) {
        return "profile-image-stroke";
      }
    },
    setImageSrc(src) {
      if (src === "" || src === null || src === undefined || this.isPlaceholder || src == "placeholder" || src == "src/assets/images/profile-images/placeholder.png") {
        this.imageSrc = this.placeholderPath;
      } else {
        this.imageSrc = src;
      }
    },
    handleImageError() {
      console.error("Image failed to load; switching to placeholder.");
      this.imageSrc = this.placeholderPath; // Fallback to placeholder if error
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
  overflow: hidden;
}

.profile-image-stroke {
  border: 2px solid var(--green);
  filter: drop-shadow(0px 4px 4px rgba(56, 102, 65, 0.2));
}

.profile-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
  border-radius: 50%;
}
</style>
