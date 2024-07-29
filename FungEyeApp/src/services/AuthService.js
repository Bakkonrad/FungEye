import { ref } from 'vue';

export const isLoggedIn = ref(false);

export function checkAuth() {
    if (localStorage.getItem("token")) {
        isLoggedIn.value = true;
    } else {
        isLoggedIn.value = false;
    }
}