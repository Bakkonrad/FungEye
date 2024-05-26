<script setup>
import { RouterLink, RouterView } from "vue-router";

import "@fontsource/cormorant-garamond";
import "@fontsource/cormorant-garamond/500.css";
import "@fontsource/cormorant-garamond/600.css";
import "@fontsource/lobster-two";
</script>

<script>
import Navbar from "./components/Navbar.vue";

export default {
  components: {
    Navbar,
  },
};
</script>

<template>
  <div>
    <RouterView />
  </div>
</template>

<style>
/* body */
body {
  background-color: #f2e8cf !important;
  font-family: "Cormorant Garamond";
  /* zmienne css, przykładowe użycie: color: var(--dark-green); */
  --dark-green: #386641;
  --green: #6a994e;
  --light-green: #a7c957;
  --beige: #f2e8cf;
  --red: #bc4749;
}

/* fonts */
h1 {
  font-family: "Lobster Two";
  font-size: 3em;
}

h2 {
  font-family: "Lobster Two";
  font-size: 2em;
}

h3 {
  font-family: "Lobster Two";
  font-size: 1.5em;
}

p {
  font-family: "Cormorant Garamond";
  font-size: 1.2em;
}

/* buttons */
.fungeye-default-button {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  padding-left: 26px;
  padding-right: 26px;
  padding-top: 0;
  padding-bottom: 0;

  background: var(--green);
  color: white;
  border-radius: 35px;

  font-family: "Cormorant Garamond";
  font-weight: 500;
  font-size: 1.2em;
  height: 50px;
}

.fungeye-default-button:hover {
  background: var(--dark-green);
  color: white;
}

.fungeye-default-button:focus,
.fungeye-default-button:active,
.fungeye-default-button:active:focus {
  background: var(--dark-green);
  color: white;
  font-weight: 600;
}

/* forms */
.invalidInput {
  color: var(--red);
}

input {
  color: white !important;
  /* width: 90% !important; */

  box-sizing: border-box;
  height: 50px;

  background: rgba(255, 255, 255, 0.3) !important;
  /* input-stroke */
  border: 1px solid rgba(56, 102, 65, 0.2) !important;
  border-radius: 15px !important;
}

input::placeholder {
  color: rgba(0, 0, 0, 0.572) !important;
}

input:focus {
  background: rgba(255, 255, 255, 0.5) !important;
  border-color: var(--dark-green) !important;
  box-shadow: 0 0 0 0.15rem rgba(56, 102, 65, 0.25) !important;
}

#forgotPassword {
  color: white;
  display: flex;
  justify-content: flex-end;
}

.submitFormButton {
  width: 100%;
  margin-top: 1em;
}


</style>
