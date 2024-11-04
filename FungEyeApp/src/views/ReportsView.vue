<template>
    <div class="container-md reports">
        <div v-for="report in reports" class="report">
            <Report :report="report" @delete-report="deleteReport"/>
        </div>
    </div>
</template>

<script>
import Report from "@/components/Report.vue";
import PostService from "@/services/PostService";

export default {
    name: "ReportsView",
    components: {
        Report
    },
    data() {
        return {
            reports: []
        };
    },
    created() {
        this.getReports();
    },
    methods: {
        async getReports() {
            try {
                const response = await PostService.getReports();
                // const response = { success: true, data: [{ id: 1, reportedUser: "test", author: "author", postId: 1, commentId: null, createdAt: "01.01.2021" }, { id: 2, reportedUser: "reportedUser", author: "author", postId: 2, commentId: 2, createdAt: "30.05.2024" }] };
                if (!response.success) {
                    console.error("Error while fetching reports");
                    return;
                }
                this.reports = response.data;
                console.log(this.reports);
            } catch (error) {
                console.error(error);
            }
        },
        async deleteReport(reportId) {
            try {
                await PostService.deleteReport(reportId);
                console.log("Report deleted");
                this.getReports();
            } catch (error) {
                console.error(error);
            }
        }
    }
}
</script>

<style scoped>
.reports {
    display: flex;
    justify-content: center;
    flex-direction: column-reverse;
    width: 80%;
}

@media screen and (max-width: 576px) {
    .reports {
        width: 100%;
    }
    
}
</style>