<template>
<div class="logo">
    <img id="logo-img" src="../../public/ikona-aplikacji.svg" alt="" />
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
    align-items: center;
    margin-left: 1em;
    gap: 0.3em;
}

#logo-img {
    width: 3em;
    height: 3em;
}

#logo-text {
    font-family: "Lobster Two";
    font-size: 1.5em;
    color: var(--dark-green);
}

</style>