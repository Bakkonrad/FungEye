<template>
  <nav class="navbar navbar-expand-lg bg-body-tertiary fungeye-navbar">
    <div class="container-fluid">
      <RouterLink  to="/" class="navbar-brand"><Logo /></RouterLink>
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
            <RouterLink to="/" :class="getActiveNavLink('home')" aria-current="page">Strona główna</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/recognize" :class="getActiveNavLink('recognize')">Rozpoznawanie grzybów</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/portal" :class="getActiveNavLink('portal')">Portal</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/weather" :class="getActiveNavLink('weather')">Pogoda</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/atlas" :class="getActiveNavLink('atlas')">Atlas</RouterLink>
          </li>
        </ul>
    </div>
    <RouterLink to="/log-in" class="btn fungeye-default-button" id="logInButton" v-if="!loggedIn">Zaloguj / Zarejestruj się</RouterLink>
    <RouterLink to="/my-profile" id="myProfileButton" v-else><MyProfileButton /></RouterLink>
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
  data() {
    return {
      loggedIn: false,
    }
  },
  methods: {
    getActiveNavLink(viewName) {
      let classString = "nav-link";
      if (this.$route.name === viewName) {
        classString += " active";
      }
      return classString;
    }
  }
};
</script>

<style>
.fungeye-navbar {
  background-color: var(--beige) !important;
}

#navbarNav {
  justify-content: center;
}

.navbar-nav {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 2em;
}

.nav-link {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-style: normal;
  font-size: 1.3em;
  line-height: 29px;

  color: var(--black);
  cursor: pointer;
  transition: 0.05s;
}

.nav-link.active {
  color: var(--dark-green) !important;
  font-weight: 400;
  background-color: var(--dark-beige);
  border-radius: 15px;
  /* text-decoration: underline; */
}

#logInButton {
    height: 50px;
}

#myProfileButton {
  text-decoration: none;
}

</style>
