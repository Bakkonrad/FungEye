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
  <Navbar />
</template>

<style>
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
</style>
