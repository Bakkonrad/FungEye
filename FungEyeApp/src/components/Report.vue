<template>
    <div class="container-md">
        <div class="card">
            <h5 class="card-header">Zgłoszenie nr {{ report.id }}</h5>
            <div class="card-body">
                <h5 class="card-title">Zgłoszono {{ postOrComment() }} użytkownika {{ report.reportedUser }}</h5>
                <p class="card-text">Autor zgłoszenia: <strong>{{ report.author }}</strong></p>
                <p class="card-text">Data zgłoszenia: <strong>{{ report.createdAt }}</strong></p>
                <div class="action-buttons">
                    <button class="btn fungeye-default-button" @click="goToPost">Zobacz zgłoszony {{
                        postOrComment()
                        }}</button>
                    <button class="btn fungeye-red-button" @click="$emit('delete-report')">
                        Usuń zgłoszenie
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "ReportsView",
    props: {
        report: {
            type: Object,
            required: true
        }
    },
    methods: {
        postOrComment() {
            return this.report.commentId ? "komentarz" : "post";
        },
        goToPost() {
            if (this.report.commentId) {
                this.$router.push({ name: "PostView", params: { id: this.report.postId, reportedCommentId: this.report.commentId } });
            } else {
                this.$router.push({ name: "PostView", params: { id: this.report.postId } });
            }
        }
    }
};
</script>

<style scoped>
.card {
    margin-top: 2em;
    border-radius: 15px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    background-color: var(--beige);
}

.card-header {
    background-color: var(--red);
    color: white;
}

.card-body {
    display: flex;
    flex-direction: column;
    background-color: var(--beige);
}

.action-buttons {
    display: flex;
    justify-content: space-between;
    margin-top: 1em;
    gap: 1em;
}

@media screen and (max-width: 576px) {

    .action-buttons {
        flex-direction: column;
        gap: 1em;
    }
    
}
</style>