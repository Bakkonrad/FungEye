<template>
    <div class="container-md reports">
        <div v-if="reports.length > 0" v-for="report in reports" class="report">
            <Report :report="report" @delete-report="deleteReport" />
        </div>
        <div v-else class="no-reports">
            <p>Brak zgłoszeń</p>
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
                if (!response.success) {
                    this.reports = [];
                    console.error("Error while fetching reports");
                    return;
                }
                this.reports = response.data;
            } catch (error) {
                this.reports = [];
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

.no-reports {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    margin-top: 2em;
}

@media screen and (max-width: 576px) {
    .reports {
        width: 100%;
    }

}
</style>