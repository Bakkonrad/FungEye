<template>

    <div class="banUser-container container-md">
        <h2>Banowanie użytkownika {{ user.username }}</h2>
        <p>Wybierz okres czasu, na który chcesz zbanować użytkownika:</p>

        <div class="button-group bans">
            <button type="radio" class="btn fungeye-default-button ban-button" v-for="(ban, key) in bans" :key="ban" @click="chooseBan(key)">{{ ban }}</button>
        </div>

        <span class="error-message" v-if="error">{{ apiErrorMessage }}</span>
        <div class="button-group actions">
            <button type="submit" class="btn fungeye-default-button" @click="save">
                Zapisz zmiany
            </button>
            <button class="btn fungeye-red-button" @click="$emit('cancel-ban')">Anuluj</button>
        </div>
    </div>

</template>

<script>
export default {
    name: "BanUser",
    props: {
        user: Object,
    },
    data() {
        return {
            bans: {
                1: "Tydzień",
                2: "Miesiąc",
                3: "Rok",
                4: "Na zawsze",
            },
            chosenBan: null,
            error: false,
            apiErrorMessage: "",
        };
    },
    methods: {
        save() {
            console.log("chosenTime", this.chosenBan);
            console.log("User banned");
            this.$emit("ban-user", this.user, this.chosenBan);
        },
        chooseBan(ban) {
            this.chosenBan = ban;
            console.log("chosenBan", this.chosenBan);
        },
    },
};
</script>

<style scoped>
.banUser-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 20px;
    max-width: 600px;
    margin: 0 auto;
    padding: 2rem;
    background-color: var(--beige);
    border-radius: 5px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.button-group {
    margin-top: 2em;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 100%;
}

.bans {
    flex-wrap: wrap;
    gap: 1em;
}

.ban-button {
    padding: 10px 20px;
    border: 1px solid var(--black);
    background-color: var(--beige);
    color: var(--black);
}

.ban-button.active,
.ban-button:focus,
.ban-button:active {
    background-color: var(--black);
    color: var(--beige);
}

.ban-button:hover {
    background-color: var(--black);
    color: var(--beige);
}

.actions {
    margin-top: 2em;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 100%;
}
</style>