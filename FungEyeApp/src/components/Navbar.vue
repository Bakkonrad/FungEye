<template>
  <nav class="navbar navbar-expand-lg bg-body-tertiary fungeye-navbar">
    <div class="container-fluid">
      <div class="nav-element navbar-brand-container">
        <RouterLink to="/" class="navbar-brand">
          <Logo />
        </RouterLink>
      </div>
      <button class="navbar-toggler" type="button" @click="toggleNavbar" aria-controls="navbarNav"
        :aria-expanded="isNavbarCollapsed ? 'true' : 'false'" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" :class="{ show: isNavbarCollapsed, 'd-flex': isNavbarCollapsed }"
        id="navbarNav">
        <ul class="navbar-nav">
          <li class="nav-item">
            <RouterLink to="/" :class="getActiveNavLink('home')" aria-current="page">Strona główna
            </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/recognize" :class="getActiveNavLink('recognize')">Rozpoznawanie grzybów
            </RouterLink>
          </li>
          <li v-if="loggedIn === true" class="nav-item">
            <RouterLink to="/portal" :class="getActiveNavLink('portal')">Portal</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/weather" :class="getActiveNavLink('weather')">Pogoda</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/map" :class="getActiveNavLink('map')">Mapa</RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink to="/atlas" :class="getActiveNavLink('atlas')">Atlas</RouterLink>
          </li>
          <li v-if="isAdmin === true" class="nav-item">
            <RouterLink to="/admin" :class="getActiveNavLink('admin')">Admin</RouterLink>
          </li>
        </ul>
        <div v-if="isNavbarCollapsed" class="nav-buttons nav-buttons-collapsed">
          <RouterLink to="/log-in" id="logInButton" v-if="!loggedIn">
            <LogInRegisterButton />
          </RouterLink>
          <RouterLink to="/my-profile" id="myProfileButton" v-else>
            <MyProfileButton />
          </RouterLink>
        </div>
      </div>
      <div v-if="!isNavbarCollapsed" class="nav-element nav-buttons nav-buttons-expanded">
        <RouterLink to="/log-in" id="logInButton" v-if="!loggedIn">
          <LogInRegisterButton />
        </RouterLink>
        <RouterLink to="/my-profile" id="myProfileButton" v-else>
          <MyProfileButton />
        </RouterLink>
      </div>
    </div>
  </nav>
</template>

<script>
import LogInRegisterButton from "./LogInRegisterButton.vue";
import {
  isLoggedIn,
  checkAuth,
  checkAdmin,
  isAdmin,
} from "@/services/AuthService";
import Logo from "./Logo.vue";
import MyProfileButton from "./MyProfileButton.vue";
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

export default {
  name: "Navbar",
  components: {
    Logo,
    MyProfileButton,
    LogInRegisterButton,
  },
  data() {
    return {
      loggedIn: false,
      isAdmin: false,
    };
  },
  mounted() {
    checkAuth();
    checkAdmin();
    this.isAdmin = isAdmin;
    this.loggedIn = isLoggedIn;
  },
  setup() {
    const isNavbarCollapsed = ref(false);

    onMounted(() => {
      checkAuth();
      checkAdmin();
      isAdmin.value = isAdmin;
    });

    const toggleNavbar = () => {
      isNavbarCollapsed.value = !isNavbarCollapsed.value;
    };

    const hideNavbar = () => {
      isNavbarCollapsed.value = false;
    };

    const router = useRouter();
    router.beforeEach((to, from, next) => {
      hideNavbar();  // Ukryj navbar przed zmianą trasy
      next();
    });

    return {
      loggedIn: isLoggedIn,
      isAdmin: isAdmin,
      isNavbarCollapsed,
      toggleNavbar,
    };
  },
  methods: {
    getActiveNavLink(viewName) {
      let classString = "nav-link";
      if (this.$route.name === viewName) {
        classString += " active";
      }
      return classString;
    },
  },
};
</script>

<style>
.fungeye-navbar {
  background-color: var(--beige) !important;
  -webkit-user-select: none;
  /* Safari */
  -ms-user-select: none;
  /* IE 10 and IE 11 */
  user-select: none;
  /* Standard syntax */
}

.container-fluid {
  display: flex;
}

.nav-element {
  flex: 1;
  display: flex;
  justify-content: space-evenly;
}

.navbar-brand-container {
  justify-self: flex-start !important;
}

#navbarNav {
  justify-content: center;
}

.navbar-nav {
  display: flex;
  justify-self: space-between;
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
  transition: 0.1s;
}

.nav-link:hover {
  font-weight: 500;
}

.nav-link.active {
  font-weight: 500;
  border-bottom: 2px solid var(--black);
}

.nav-buttons-expanded {
  margin-left: auto;
  justify-self: flex-end !important;
}

#logInButton {
  height: 50px;
}

#myProfileButton,
#logInButton {
  text-decoration: none;
}

@media screen and (max-width: 1158px) {
  .navbar-nav {
    flex-direction: column;
    justify-content: center;
    align-items: baseline;
    gap: 1em;
  }

  #navbarNav {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 1em;
  }

  .navbar-nav {
    flex-direction: column;
  }


  .navbar-brand {
    font-size: 1em;
  }

  .nav-link {
    font-size: 1.2em;
  }
}

@media (max-width: 991px) {
  .nav-element {
    flex: 1;
    display: flex;
    justify-content: space-between;
  }

  #navbarNav {
    display: none;
    align-items: flex-start;
    flex-direction: column;
    gap: 1em;
  }

  .navbar-nav {
    flex-direction: column;
  }

  .nav-buttons-expanded {
    display: none;
  }

  .navbar-brand {
    font-size: 1em;
  }

  .nav-link {
    font-size: 1.2em;
  }
}
</style>
