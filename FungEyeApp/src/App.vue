<script setup>
import { RouterLink, RouterView } from "vue-router";
</script>

<script>
import Navbar from "./components/Navbar.vue";
import Footer from "./components/Footer.vue";

export default {
  components: {
    Navbar,
    Footer,
  },
};
</script>

<template>
  <Navbar />
  <div class="r-view">
    <RouterView />
  </div>
  <Footer />
</template>

<style>
/* body */
body {
  /* zmienne css, przykładowe użycie: color: var(--dark-green); */
  /* --dark-green: #386641; */
  --dark-green: #355f3d;
  /* --green: #6a994e; */
  --green: #538237;
  --light-green: #a7c957;
  --beige: #f1ebdd;
  --dark-beige: #dfd1a0;
  --red: #bc4749;
  --dark-red: #712b2c;
  --warning: #eebc3c;
  --black: #333333;
  background-color: var(--beige) !important;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  color: var(--black);
}

h1,
h2,
h3,
.r-link,
button,
nav,
label,
img,
input {
  -webkit-user-select: none;
  /* Safari */
  -ms-user-select: none;
  /* IE 10 and IE 11 */
  user-select: none;
  /* Standard syntax */
}

/* fonts */
h1 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 700;
  font-size: 3em;
}

h2 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 500;
  font-size: 2em;
}

h3 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 300;
  font-size: 1.5em;
}

p,
p1 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 300;
  font-size: 1.2em;
}

ul {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 300;
}

.page-title {
  text-align: center;
  margin-top: 20px;
}

::-webkit-scrollbar {
  width: 10px;
}

::-webkit-scrollbar-track {
  background: var(--dark-beige);
}

::-webkit-scrollbar-thumb {
  border-radius: 10px;
  background: var(--dark-green);
}

::-webkit-scrollbar-thumb:hover {
  background: var(--green);
}

/* buttons */
.fungeye-default-button,
.fungeye-red-button,
.fungeye-secondary-button {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  padding-left: 26px;
  padding-right: 26px;
  padding-top: 0;
  padding-bottom: 0;
  width: fit-content;

  background: var(--green);
  color: white;
  border-radius: 35px;

  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 300;
  font-size: 1.2em;
  height: 50px;
  text-decoration: none;
}

.fungeye-default-button:hover {
  background: var(--dark-green);
  color: white;
}

.fungeye-default-button:focus,
.fungeye-default-button:active,
.fungeye-default-button:active:focus,
.fungeye-default-button.active {
  background: var(--dark-green);
  color: white;
}

.fungeye-default-button:disabled {
  background: var(--light-green);
  color: white;
}

.fungeye-red-button {
  background: var(--red);
  color: white;
}

.fungeye-red-button:hover {
  background: var(--dark-red);
  color: white;
}

.fungeye-red-button:focus,
.fungeye-red-button:active,
.fungeye-red-button:active:focus {
  background: var(--dark-red);
  color: white;
}

.fungeye-secondary-button {
  /* transparent background */
  background: transparent;
  border: 1px solid white;
  color: white;
}

.fungeye-secondary-button:hover {
  background: #f1ebddb5;
  color: var(--black) !important;
}

.fungeye-secondary-button:focus,
.fungeye-secondary-button:active,
.fungeye-secondary-button:active:focus {
  background: var(--beige);
  color: var(--black) !important;
}

.button-icon {
  margin-right: 5px;
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

input.invalidInput {
  border-color: var(--red) !important;
  box-shadow: 0px 0px 13.1px #bc4749;
}

input.validInput {
  border-color: var(--green) !important;
  box-shadow: 0px 0px 13.1px #6a994e;
}

.error-message {
  color: var(--red);
  font-size: 1em;
}

.error-loading-data {
  color: var(--red);
  text-align: center;
  font-size: x-large;
  font-weight: 600;
  margin-top: 30px;
}

.r-link {
  color: var(--dark-green) !important;
}

.r-link:hover {
  color: var(--green) !important;
}

.r-link:active {
  color: var(--dark-green) !important;
  font-weight: 500;
}

.form-label {
  color: white;
}

.submitFormButton {
  width: 100%;
  margin-top: 1em;
}

.r-view {
  margin-top: 2em;
  min-height: calc(100vh - 70px);
}

/* mushroom attributes */
.attribute {
  background-color: var(--green);
  padding: 1px 25px;
  color: white;
  border-radius: 15px;
  font-weight: 300;
}

/* iglaste */
.attribute.coniferous {
  background-color: var(--dark-green);
}

/* liściaste */
.attribute.deciduous {
  background-color: var(--light-green);
  color: var(--black);
}

/* jadalny */
.attribute.edible {
  background-color: var(--green);
}

/* niejadalny */
.attribute.inedible {
  background-color: var(--beige);
  border: 1px solid var(--dark-red);
  color: var(--dark-red);
}

/* trujący */
.attribute.poisonous {
  background-color: var(--red);
}
</style>
