<template>
    <div class="container-md">
        <div class="card">
            <h5 class="card-header" :class="report.completed ? 'completed' : ''">Zgłoszenie nr {{ report.id }}
                <span v-if="report.completed" class="completed-mark">- ROZWIĄZANE</span>
            </h5>
            <div class="card-body">
                <h5 class="card-title">Zgłoszono {{ postOrComment() }}</h5>
                <p class="card-text">Autor zgłoszenia: <strong>{{ report.reportedBy.username }}</strong></p>
                <p class="card-text">Data zgłoszenia: <strong>{{ formatDate(report.createdAt) }}</strong></p>
                <div class="action-buttons">
                    <button class="btn fungeye-default-button" @click="goToPost">Zobacz zgłoszony {{
                        postOrComment()
                        }}</button>
                    <button v-if="report.completed === false" class="btn fungeye-secondary-black-button" @click="$emit('delete-report', report.id)">
                        Oznacz jako rozwiązane
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
                const reportedCommentId = parseInt(this.report.commentId);
                this.$router.push({ name: "post", params: { id: this.report.postId }, query: { reportedCommentId: reportedCommentId } });
            } else {
                this.$router.push({ name: "post", params: { id: this.report.postId } });
            }
        },
        formatDate(date) {
            return new Date(date).toLocaleString();
        }   
    }
};
</script>

<style scoped>
.card {
    margin-top: 2em;
    /* border-radius: 15px; */
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    background-color: var(--beige);
}

.card-header {
    background-color: var(--red);
    color: white;
}

.completed {
    background-color: var(--green);
} 

.completed-mark {
    padding: 0.5em;
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