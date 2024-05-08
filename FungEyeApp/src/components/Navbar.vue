<template>
  <nav class="navbar navbar-expand-lg bg-body-tertiary fungeye-navbar">
    <div class="container-fluid">
      <a class="navbar-brand" href="#"><Logo /></a>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarNav"
        aria-controls="navbarNav"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav">
          <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="#">Home</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Features</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Pricing</a>
          </li>
          <li class="nav-item">
            <a class="nav-link">Disabled</a>
          </li>
        </ul>
    </div>
    <MyProfileButton />
    </div>
  </nav>
</template>

<script>
// import ProfileImage from "./ProfileImage.vue";
import Logo from "./Logo.vue";
import MyProfileButton from "./MyProfileButton.vue";

export default {
  name: "Navbar",
  components: {
    Logo,
    MyProfileButton,
  },
};
</script>

<style>
.fungeye-navbar {
  background-color: var(--beige) !important;
}

#navbarNav {
  justify-content: center;
}

.nav-link {
  font-family: "Cormorant Garamond";
  font-style: normal;
  font-size: 1.5em;
  line-height: 29px;

  color: #000000;
  cursor: pointer;
}

.nav-link.active {
  color: var(--green) !important;
  font-weight: 500;
  text-decoration: underline;
}

</style>
