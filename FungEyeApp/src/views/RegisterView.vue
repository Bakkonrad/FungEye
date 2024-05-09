<template>
    <Navbar></Navbar>
    <div class="container">
        <div class="log-in-bg">
        <div class="log-in-content">
            <h1>Log in</h1>
            <p>Log in to your account</p>
            <router-link to="/register">
            <button class="fungeye-default-button">Register</button>
            </router-link>
        </div>
        </div>
    </div>
</template>

<script>
import Navbar from '@/components/Navbar.vue';

export default {
    name: "LogInView",
    components: {
        Navbar,
    },
};
</script>