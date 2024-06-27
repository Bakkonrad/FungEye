<template>
<div class="logo">
    <img id="logo-img" src="../assets/images/ikona-aplikacji.svg" alt="" />
    <h1 id="logo-text">FungEye</h1>
</div>
</template>

<script>
export default {
    name: "Logo",
};
</script>

<style>
.logo {
    display: flex;
    align-items:end;
    justify-content: center;
    line-height: normal;
    margin-left: 1em;
    gap: 0.3em;
}

#logo-img {
    width: 2.5em;
    height: 2.5em;
}

#logo-text {
  /* font-family: "Helvetica Neue", Helvetica, Arial, sans-serif; */
  font-family: 'Avenir Next', sans-serif;
    font-weight: 700;
    font-size: 1.7em;
    /* color: var(--black); */
    color: #28492e;
    line-height: 1;
}

</style>