<template>
    <div class="input-group mb-3" id="searchBar">
        <input type="text" v-model="searchQuery" placeholder="Szukaj..." class="form-control search-bar"
            id="searchBar-input" aria-describedby="searchButton" v-on:keyup.enter="onSearch" />
        <button @click="onSearch" class="btn fungeye-default-button" type="button" id="searchButton">
            <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="search-icon" />
        </button>
    </div>
</template>

<script>
export default {
    name: "SearchBar",
    props: {
        initialQuery: {
            type: String,
            default: "",
        },
    },
    data() {
        return {
            searchQuery: this.initialQuery,
        };
    },
    watch: {
        searchQuery(newQuery) {
            if (newQuery === "") {
                this.onSearch();
            }
        },
    },
    methods: {
        onSearch() {
            this.$emit("search", this.searchQuery);
        },
    },
};
</script>

<style scoped>
#searchBar {
    width: 30%;
    margin: 0 auto;
    align-items: center;
    display: flex;
    justify-content: center;
    flex-direction: row;
    gap: 0.5em;
}

#searchBar-input {
    border-radius: 15px 0 0 15px;
    color: var(--black) !important;
}

#searchButton {
    border-radius: 15px;
}

.search-icon {
    position: absolute;
    padding: 5px;
    min-width: 50px;
    text-align: center;
}

@media screen and (max-width: 768px) {
    #searchBar {
        width: 95%;
    }
}

@media screen and (max-width: 576px) {
    #searchBar {
        flex-direction: column;
    }

    #searchBar-input {
        width: 100%;
    }

    #searchButton {
        width: 100%;
    }
}
</style>