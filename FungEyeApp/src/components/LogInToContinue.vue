<template>
    <div class="container">
        <h2 class="page-title">Zaloguj się, aby kontynuować</h2>
        <p class="text-center">Aby korzystać z tej funkcji, musisz się zalogować.</p>
        <div class="text-center">
            <router-link to="/log-in" class="btn fungeye-default-button">Zaloguj się</router-link>
        </div>
    </div>
</template>

<script>
export default {
    name: "LogInToContinue",
};
</script>

<style scoped>
.container {
    background: rgba(0, 0, 0, 0.7);
    border-radius: 30px;
    color: white;
    padding: 4em 2em;
    margin: 2em 0;
}

.page-title {
    text-align: center;
}

.text-center {
    text-align: center;
    display: flex;
    justify-content: center;
}
</style>