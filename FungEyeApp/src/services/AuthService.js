import { ref } from 'vue';

export const isLoggedIn = ref(false);
export const isAdmin = ref(true);

export function checkAuth() {
    if (localStorage.getItem("token")) {
        isLoggedIn.value = true;
    } else {
        isLoggedIn.value = false;
    }
}

export function checkAdmin() {
    // if (localStorage.getItem("userRole") === "admin") {
    //     isAdmin.value = true;
    // } else {
    //     isAdmin.value = false;
    // }
}