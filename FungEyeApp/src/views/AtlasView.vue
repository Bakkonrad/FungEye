<template>
  <div class="atlas-view">
    <h1>Atlas grzybów</h1>
    <SearchBar @search="handleSearch" />
    <div class="alphabet-filter">
      <span v-for="letter in alphabet" :key="letter" @click="filterByLetter(letter)"
        :class="{ 'active-letter': activeLetter === letter }">
        {{ letter }}
      </span>
    </div>

    <div class="attribute-filter">
      <span v-for="attribute in availableAttributes" :key="attribute" @click="toggleAttributeFilter(attribute)"
        :class="['attribute', attributeClass(attribute), { 'active-attribute': isActiveAttribute(attribute) }]">
        {{ attribute }}
      </span>
    </div>

    <div class="mushroom-list">
      <div v-for="mushroom in filteredMushrooms" :key="mushroom.id" class="mushroom-card"
        @click="openMushroomView(mushroom)">
        <img :src="mushroom.image" alt="Mushroom Image" class="mushroom-image" />
        <div class="mushroom-info">
          <h3>{{ mushroom.name }}</h3>
          <div class="attributes">
            <span v-for="attr in mushroom.attributes" :key="attr" @click.stop="toggleAttributeFilter(attr)"
              :class="['attribute', attributeClass(attr), { 'active-attribute': isActiveAttribute(attr) }]">
              {{ attr }}
            </span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import SearchBar from '@/components/SearchBar.vue';

export default {
  components: {
    SearchBar
  },
  data() {
    return {
      searchQuery: '',
      activeLetter: '',
      selectedAttributes: [],
      mushrooms: [
        {
          id: 1,
          name: 'Borowik szlachetny',
          image: 'src/assets/images/mushrooms/ATLAS-borowik.jpg',
          attributes: ['iglaste', 'liściaste', 'jadalny']
        },
        {
          id: 2,
          name: 'Muchomor czerwony',
          image: 'src/assets/images/mushrooms/ATLAS-muchomor.jpg',
          attributes: ['iglaste', 'liściaste', 'trujący', 'niejadalny']
        },
        // Testowe grzybki
      ],
      alphabet: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split(''),
      availableAttributes: ['iglaste', 'liściaste', 'jadalny', 'niejadalny', 'trujący']
    };
  },
  computed: {
    filteredMushrooms() {
      let filtered = this.mushrooms;

      if (this.activeLetter) {
        filtered = filtered.filter(mushroom => mushroom.name.startsWith(this.activeLetter));
      }

      if (this.searchQuery) {
        filtered = filtered.filter(mushroom =>
          mushroom.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        );
      }

      if (this.selectedAttributes.length) {
        filtered = filtered.filter(mushroom =>
          this.selectedAttributes.every(attr => mushroom.attributes.includes(attr))
        );
      }

      return filtered;
    }
  },
  methods: {
    handleSearch(query) {
      this.searchQuery = query;
    },
    filterByLetter(letter) {
      if (this.activeLetter === letter) {
        this.activeLetter = null;
      } else {
        this.activeLetter = letter;
      }
    },
    resetActiveLetter() {
      this.activeLetter = '';
    },
    openMushroomView(mushroom) {
      this.$router.push({ name: 'MushroomView', params: { id: mushroom.id } });
      console.log('Otwieram kartę grzyba:', mushroom);
    },
    toggleAttributeFilter(attribute) {
      const index = this.selectedAttributes.indexOf(attribute);
      if (index > -1) {
        this.selectedAttributes.splice(index, 1);
      } else {
        this.selectedAttributes.push(attribute);
      }
    },
    isActiveAttribute(attribute) {
      return this.selectedAttributes.includes(attribute);
    },
    attributeClass(attribute) {
      return {
        coniferous: attribute === 'iglaste',
        deciduous: attribute === 'liściaste',
        edible: attribute === 'jadalny',
        inedible: attribute === 'niejadalny',
        poisonous: attribute === 'trujący',
      };
    }
  }
};
</script>

<style scoped>
.atlas-view {
  padding: 0 20px;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.alphabet-filter {
  margin: 20px 0;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 5px;
}

.alphabet-filter span {
  margin: 0 10px;
  cursor: pointer;
  font-size: 20px;
  border-radius: 15px;
  transition: font-weight 0.3s;
  user-select: none;
}

.active-letter {
  font-weight: bold;
}

.attribute-filter {
  margin: 20px 0;
  text-align: center;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 5px;
}

/* css dla filtrów atrybutów */
.attribute-filter span {
  margin: 0 7px;
  cursor: pointer;
  font-size: 17px;
  padding: 1px 10px;
  border-radius: 15px;
  /* background-color: #f0f0f0; */
  transition: font-weight 0.3s, background-color 0.3s;
  user-select: none;
}

.active-attribute {
  font-weight: bold;
}

/* css dla atrybutów w kartach grzybów */
.attributes span {
  padding: 2px 15px;
  border-radius: 15px;
  font-weight: 300;
  transition: font-weight 0.3s;
  user-select: none;
}

/* pogrubienie dla aktywnych atrybutów */
.active-attribute.coniferous,
.attribute-filter span.coniferous.active-attribute,
.active-attribute.deciduous,
.attribute-filter span.deciduous.active-attribute,
.active-attribute.edible,
.attribute-filter span.edible.active-attribute,
.active-attribute.inedible,
.attribute-filter span.inedible.active-attribute,
.active-attribute.poisonous,
.attribute-filter span.poisonous.active-attribute {
  font-weight: bold;
}

.mushroom-list {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.mushroom-card {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  background-color: var(--beige);
  padding: 10px;
  margin: 10px 0;
  width: 70%;
  border-radius: 10px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: box-shadow 0.3s;
  user-select: none;
}

.mushroom-card:hover {
  /* background-color: #f0f0f0; */
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.4);
}

.mushroom-image {
  width: 80px;
  height: 80px;
  margin-right: 20px;
  border-radius: 50%;
}

.mushroom-info {
  flex-grow: 1;
}

.mushroom-info h3 {
  margin: 0;
  font-size: 22px;
}

.attributes {
  display: flex;
  margin-top: 5px;
  list-style-type: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  flex-direction: row;
  gap: 10px;
}

/* wyłączenie kliku gdy karty grzyba gdy najeżdża się na atrybut (nie działa) */
.attributes span:hover {
  cursor: pointer;
}

.attributes span:hover~.mushroom-card {
  pointer-events: none;
  background-color: #f0f0f0;
}

@media screen and (max-width: 768px) {
  .mushroom-card {
    align-items: flex-start;
    flex-direction: column;
    gap: 15px;
    width: 95%;
  }
}
</style>