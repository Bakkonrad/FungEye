<template>
  <Navbar />
  <div class="container">
    <img class="log-in-bg" src="../assets/images/backgrounds/log-in-bg.jpeg" />
    <div class="log-in-content">
      <h1>Witaj ponownie!</h1>
      <p>Zaloguj się, aby móc w pełni korzystać z możliwości FungEye</p>
      <form>
        <div class="mb-3">
          <label for="exampleFormControlInput1" class="form-label">Email</label>
          <input
            type="email"
            class="form-control"
            id="exampleFormControlInput1"
            placeholder="name@example.com"
          />
        </div>
        <div class="mb-3">
          <label for="exampleInputPassword1" class="form-label">Hasło</label>
          <input
            type="password"
            class="form-control"
            id="exampleInputPassword1"
          />
        </div>
        <!-- <div id="forgotPassword" class="form-text">Zapomniałeś/aś hasła?</div> -->
        <button
          type="submit"
          class="btn fungeye-default-button submitFormButton"
        >
          Zaloguj się
        </button>
      </form>
      <span id="registerLink">
        <p>Nie masz jeszcze konta?</p>
        <RouterLink to="/register" class="router-registerLink"><p><b>Zarejestruj się</b></p></RouterLink>
      </span>
    </div>
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
export default {
  components: {
    Navbar,
  },
};
</script>

<style>
.container {
  margin-top: 3em;
  display: flex;
  justify-content: center;
  align-items: center;
}

.log-in-bg {
  width: 100%;
  height: 670px;
  border-radius: 20px;
  position: relative;
}

.log-in-content {
  position: absolute;
  height: auto;

  position: absolute;
  width: 25vw;

  background: rgba(0, 0, 0, 0.45);
  backdrop-filter: blur(9.5px);
  border-radius: 30px;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 2em 2em 3em 2em;
}

h1 {
  color: white;
  font-size: 3em;
}

p {
  color: white;
  font-size: 1.3em;
}

@media (max-width: 768px) {
  .log-in-content {
    width: 80vw;
  }
}

label {
  color: white;
}

#registerLink {
  color: white;
  display: flex;
  justify-content: center;
  gap: 0.5em;
  margin-top: 2em;
}

.router-registerLink {
  color: white;
  text-decoration: none;
}

.router-registerLink:hover {
  color: var(--light-green);
  text-decoration: underline;
}

</style>
