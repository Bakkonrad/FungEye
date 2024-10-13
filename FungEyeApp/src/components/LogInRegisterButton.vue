<template>
    <div class="profile-button">
        <button type="button" class="btn fungeye-default-button">
            <font-awesome-icon icon="fa-solid fa-right-to-bracket" class="button-icon" />
            <span class="profile-button-text">
                Logowanie i Rejestracja
            </span>
        </button>
    </div>
</template>

<script>
</script>

<style scoped>
.fungeye-default-button {
    display: flex;
    justify-content: center !important;
    align-content: center !important;
    gap: 0.3em;
}

.profile-button {
    justify-content: flex-end;
}

@media screen and (max-width: 1470px) {
    .fungeye-default-button {
        padding: 0 15px 0 15px;
        gap: 0;
    }

    .profile-button-text {
        display: none;
    }

    .button-icon {
        margin-right: 0px;
    }
}

@media screen and (max-width: 991px) {
    .fungeye-default-button {
        background-color: var(--green) !important;
    }

    .profile-button-text {
        display: flex;
    }
}
</style>
