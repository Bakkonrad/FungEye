import { ref } from 'vue';

export const isLoggedIn = ref(false);
export const isAdmin = ref(false);

export function checkAuth() {
    if (localStorage.getItem("token")) {
        isLoggedIn.value = true;
    } else {
        isLoggedIn.value = false;
    }
}

export function checkAdmin() {
    if (localStorage.getItem("role") == "User") { // ! change to Admin
        isAdmin.value = true;
    } else {
        isAdmin.value = false;
    }
}