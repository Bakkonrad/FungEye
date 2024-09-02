<template>
    <div class="loading-spinner">
        <hollow-dots-spinner :animation-duration="1000" :dot-size="12" :dots-num="3" color="#333333" />
    </div>
</template>

<script>
import { HollowDotsSpinner } from 'epic-spinners'
export default {
    components: {
        HollowDotsSpinner
    }
}
</script>

<style>
.loading-spinner {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 20px;
    font-size: 16px;
    color: #333;
}
</style>