<template>
    <div class="container-md">
        <div class="goBack">
            <button @click="goBack" class="btn fungeye-default-button">
                <font-awesome-icon icon="fa-solid fa-left-long" class="button-icon"></font-awesome-icon>
                Powrót do panelu administratora</button>
        </div>
        <div class="page-title-container">
            <h1 class="page-title">Profil użytkownika</h1>
        </div>
        <div class="content">
            <div id="user-info">
                <UserProfileInfo :imgSrc="imgSrc" :username="username" :name_surname="name_surname" :email="email"
                    :createdAt="createdAt" />
                <button @click="goToProfile" class="btn fungeye-default-button">Zobacz profil</button>
            </div>
            <div class="admin-content">
                <div class="user-admin-info">
                    <h2>Informacje</h2>
                    <ul class="infos">
                        <li><strong>Administrator:</strong> {{ isAdmin ? "Tak" : "Nie" }}</li>
                        <li><strong>Konto zbanowane:</strong> {{ isBanned ? "Tak" : "Nie" }}</li>
                        <li v-if="isBanned"><strong>Konto zbanowane do dnia:</strong> {{ dateBanned }}</li>
                        <li><strong>Konto usunięte:</strong> {{ dateDeleted ? "Tak" : "Nie" }}</li>
                        <li v-if="isDeleted"><strong>Konto usunięte dnia:</strong> {{ dateDeleted }}</li>
                    </ul>
                </div>
                <div class="admin-actions">
                    <button class="btn fungeye-default-button action-button" id="btn-retrieveAccount"
                        @click="retrieveAccount()" v-if="isDeleted">
                        <font-awesome-icon icon="fa-solid fa-undo" class="button-icon"></font-awesome-icon>
                        Przywróć konto
                    </button>
                    <button class="btn fungeye-default-button action-button" id="btn-viewPosts" @click="viewPosts()"
                        :disabled="isDeleted">
                        <font-awesome-icon icon="fa-solid fa-list" class="button-icon"></font-awesome-icon>
                        Zobacz posty
                    </button>
                    <button class="btn fungeye-default-button action-button" id="btn-editUser" @click="startEditing()"
                        :disabled="isDeleted">
                        <font-awesome-icon icon="fa-solid fa-pen" class="button-icon"></font-awesome-icon>
                        Edytuj użytkownika
                    </button>
                    <button class="btn fungeye-default-button action-button" id="btn-banUser" @click="startBanning()"
                        :disabled="isDeleted">
                        <font-awesome-icon icon="fa-solid fa-ban" class="button-icon"></font-awesome-icon>
                        Zbanuj użytkownika
                    </button>
                    <button class="btn fungeye-red-button action-button" id="btn-deleteUser" @click="removeAccount()"
                        :disabled="isDeleted">
                        <font-awesome-icon icon="fa-solid fa-trash" class="button-icon"></font-awesome-icon>
                        Usuń użytkownika
                    </button>
                </div>
            </div>
        </div>
        <UserEdit v-if="isEditing" :user="user" @cancel-edit="cancelEditing" @save-user="saveUser" />
        <UserBan v-if="isBanning" :user="user" @cancel-ban="cancelBanning" @ban-successful="onBanSuccesful" />
    </div>
</template>

<script>
import UserService from "@/services/UserService";
import UserProfileInfo from "@/components/UserProfileInfo.vue";
import ProfileImage from "@/components/ProfileImage.vue";
import UserEdit from "@/components/EditUser.vue";
import UserBan from "@/components/BanUser.vue";

export default {
    components: {
        UserProfileInfo,
        ProfileImage,
        UserEdit,
        UserBan,
    },
    data() {
        return {
            id: null,
            role: null,
            user: null,
            imgSrc: '',
            username: null,
            name_surname: null,
            email: null,
            createdAt: null,
            isAdmin: false,
            isBanned: false,
            isDeleted: false,
            dateBanned: null,
            dateDeleted: null,
            errorFindingUser: false,
            isEditing: false,
            isBanning: false,
        };
    },
    methods: {
        goBack() {
            this.$router.push({ name: "admin" });
        },
        goToProfile() {
            this.$router.push({ name: "userProfile", params: { id: this.id } });
        },
        formatDate(date) {
            // console.log(date);
            if (date === null || new Date(date) < new Date()) {
                this.isBanned = false;
                return "";
            }
            this.isBanned = true;
            if (new Date(date).getFullYear() > 2100) {
                return "Na zawsze";
            }
            return new Date(date).toLocaleString();
        },
        formatDeletedDate(date) {
            if (date === null) {
                this.isDeleted = false;
                return "";
            }
            this.isDeleted = true;
            return new Date(date).toLocaleString();
        },
        viewPosts() {
            this.$router.push({ name: "UserPosts", params: { id: this.id } });
        },
        startEditing() {
            this.isBanning = false;
            this.isEditing = true;
        },
        cancelEditing() {
            this.isEditing = false;
        },
        saveUser() {
            this.isEditing = false;
            this.fetchUser();
        },
        startBanning() {
            this.isEditing = false;
            this.isBanning = true;
        },
        cancelBanning() {
            this.isBanning = false;
        },
        onBanSuccesful() {
            this.isBanning = false;
            this.fetchUser();
        },
        async removeAccount() {
            this.isEditing = false;
            this.isBanning = false;
            const confirmed = confirm(
                `Czy na pewno chcesz usunąć użytkownika ${this.username}?`
            );
            if (confirmed) {
                try {
                    const response = await UserService.deleteAccount(this.id);
                    if (response.success === false) {
                        console.log(response.message);
                        return;
                    }
                    alert("Konto użytkownika zostało usunięte.");
                    this.fetchUser();
                } catch (error) {
                    console.log(error);
                }
            }
        },
        async retrieveAccount() {
            try {
                const response = await UserService.retrieveAccount(this.id);
                if (response.success === false) {
                    console.log(response.message);
                    return;
                }
                alert("Konto użytkownika zostało przywrócone.");
                this.fetchUser();
            } catch (error) {
                console.log(error);
            }
        },
        async fetchUser() {
            this.id = this.$route.params.id;
            if (this.id == localStorage.getItem("id")) {
                this.$router.push({ name: "myProfile" });
                return;
            }
            // console.log(this.id);
            const response = await UserService.getUserData(this.id);
            if (response.success === false) {
                this.errorFindingUser = true;
                console.log(response.message);
                return;
            }
            this.user = response.data;
            console.log(this.user.imageUrl);
            this.role = response.data.role;
            if (this.role === 2) {
                this.isAdmin = true;
            }
            this.dateBanned = this.formatDate(response.data.banExpirationDate);
            this.dateDeleted = this.formatDeletedDate(response.data.dateDeleted);
            this.imgSrc = response.data.imageUrl;
            this.username = response.data.username;
            this.name_surname = response.data.firstName + " " + response.data.lastName;
            this.email = response.data.email;
            this.createdAt = response.data.createdAt;
        }
    },
    async created() {
        this.fetchUser();
    },
};
</script>

<style scoped>
.container-md {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
}

.goBack {
    display: flex;
    justify-content: flex-start;
    width: 100%;
}

.page-title-container {
    margin-bottom: 2em;
}

.content {
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    justify-content: center;
    flex-wrap: wrap;
    gap: 7em;
    margin-bottom: 2em;
}

#user-info {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 1em;
}

.admin-content {
    display: flex;
    flex-direction: row;
    gap: 4em;
}

.admin-actions {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.infos {
    font-size: 1.2em;
}

.action-button {
    width: 250px;
}

#btn-viewPosts {
    background-color: var(--light-green);
    color: var(--black);
}

#btn-editUser {
    background-color: var(--green);
}

#btn-banUser {
    background-color: var(--warning);
    color: var(--black);
}

#btn-deleteUser {
    background-color: var(--red);
}

@media screen and (max-width: 1200px) {
    .content {
        justify-content: flex-start;
        gap: 5em;
    }
}

@media screen and (max-width: 992px) {
    .admin-content {
        flex-direction: column;
    }
}

@media screen and (max-width: 768px) {
    .content {
        justify-content: center;
    }
}

@media screen and (max-width: 576px) {
    .action-button {
        width: 100%;
    }
}
</style>