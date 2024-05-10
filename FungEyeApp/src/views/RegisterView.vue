<template>
  <Navbar></Navbar>
  <div class="container">
    <img
      class="register-bg"
      src="../assets/images/backgrounds/register-bg.jpg"
    />
    <div class="content">
      <div class="join-text">
        <h1>Dołącz do społeczności!</h1>
        <p id="join-p">Zarejestruj się, aby móc w pełni korzystać z możliwości FungEye</p>
      </div>
      <div class="form-content">
        <form>
          <div class="mb-3">
            <label for="exampleFormControlInput1" class="form-label"
              >Email*</label
            >
            <input
              autofocus
              type="email"
              class="form-control"
              id="exampleFormControlInput1"
              placeholder="name@example.com"
            />
          </div>
          <div class="row mb-3">
            <div class="col">
                <label for="exampleFormControlInput1" class="form-label"
                  >Imię</label
                >
                <input
                  autofocus
                  type="text"
                  class="form-control"
                  id="exampleFormControlInput1"
                />
            </div>
            <div class="col">
                <label for="exampleFormControlInput1" class="form-label"
                  >Nazwisko</label
                >
                <input
                  autofocus
                  type="text"
                  class="form-control"
                  id="exampleFormControlInput1"
                />
            </div>
          </div>
          <div class="mb-3">
            <label for="exampleFormControlInput1" class="form-label"
                  >Nazwa użytkownika*</label
                >
                <input
                  autofocus
                  type="text"
                  class="form-control"
                  id="exampleFormControlInput1"
                />
          </div>
          <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Hasło*</label>
            <input
              type="password"
              class="form-control"
              id="exampleInputPassword1"
            />
          </div>
          <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Powtórz hasło*</label>
            <input
              type="password"
              class="form-control"
              id="exampleInputPassword1"
            />
          </div>
        <div id="requiredFields" class="form-text">* Pola obowiązkowe</div>

          <button
            type="submit"
            class="btn fungeye-default-button submitFormButton"
          >
            Zarejestruj się
          </button>
        </form>
        <span id="registerLink">
          <p>Masz już konto?</p>
          <RouterLink to="/log-in" class="router-registerLink"
            ><p><b>Zaloguj się</b></p></RouterLink
          >
        </span>
      </div>
    </div>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";

export default {
  name: "LogInView",
  components: {
    Navbar,
  },
};
</script>

<style scoped>
.container {
  margin-top: 3em;
  display: flex;
  justify-content: center;
  align-items: center;
  position: relative;
}

.register-bg {
  width: 100%;
  height: auto;
  border-radius: 20px;
}

.content {
  position: absolute;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  color: white;
  width: 90%;
  height: 95%;
}

.join-text {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  color: white;
  width: 30vw;
  padding: 2em 2em 30em 1em;
}

h1 {
  font-size: 5em;
  padding-bottom: 0.2em;
}

#join-p {
    font-size: 1.8em;
}

p {
  font-size: 1.3em;
}

#requiredFields {
  font-size: 1.2em;
  color: white;
  padding-bottom: 0.3em;
}

.form-content {
  position: relative;

  width: 30vw;
  height: auto;

  background: rgba(0, 0, 0, 0.45);
  backdrop-filter: blur(9.5px);
  border-radius: 30px;

  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 2em 2em 3em 2em;
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
