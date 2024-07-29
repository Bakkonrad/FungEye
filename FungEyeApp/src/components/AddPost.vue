<template>
    <div class="add-post">
      <ProfileImage
        imgSrc="https://fastly.picsum.photos/id/40/4106/2806.jpg?hmac=MY3ra98ut044LaWPEKwZowgydHZ_rZZUuOHrc3mL5mI"
      ></ProfileImage>
      <input
        type="text"
        class="form-control textarea"
        id="exampleFormControlTextarea1"
        rows="2"
        placeholder="Podziel się swoim koszykiem..."
      ></input>
      <input
        type="file"
        class="form-control file-input"
        id="inputGroupFile04"
        aria-describedby="inputGroupFileAddon04"
        aria-label="Upload"
      />
      <button class="btn fungeye-default-button">Post</button>
    </div>
</template>

<script>
import ProfileImage from "./ProfileImage.vue";

export default {
  components: {
    ProfileImage,
  },
};
</script>

<style scoped>

.add-post {
  display: flex;
  flex-direction: row;
  justify-content: center;
    align-items: center;
  margin: 1rem;
  padding: 1rem;
  border-radius: 0.5rem;
}

.add-post .textarea {
  width: 50rem;
  margin: 0.5rem;
  background-color: var(--beige) !important;
}

.add-post .textarea:focus {
  border-color: var(--green);
  background-color: var(--dark-beige) !important;
  color: var(--black) !important;
}

.add-post .file-input {
    display: none;
  width: 10rem;
  margin: 0.5rem;
}

.add-post button {
    display: none;
  margin: 0.5rem;
}

</style>