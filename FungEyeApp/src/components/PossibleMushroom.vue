<template>
  <div v-if="probability > 0" class="possible-mushroom" >
    <RouterLink :to="'/mushroom/' + id" class="possible-mushroom">
    <div class="left-side">
      <img :src="image" alt="" class="album-photo" />
      <div class="mushroom-names">
        <h2 class="mushroom-name">{{ polishName }}</h2>
        <p class="mushroom-latin">{{ latinName }}</p>
      </div>
    </div>
    <div class="right-side">
      <div class="probability" :class="probabilityColor()">
        <p>{{ probability }}</p>
        <p>%</p>
      </div>
      <div class="arrow">
        &#10095;
      </div>
    </div>
  </RouterLink>
  </div>
</template>

<script>
export default {
  props: {
    id: {
      type: Number,
      default: 0,
    },
    polishName: {
      type: String,
      default: "Nieznany gatunek",
    },
    latinName: {
      type: String,
      default: "Nieznana łacińska nazwa",
    },
    probability: {
      type: Number,
      default: 0,
    },
    image: {
      type: String,
      default: "../assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg",
    },
  },
  // change color of probability depending on its value
  methods: {
    probabilityColor() {
      if (this.probability < 50) {
        return "red";
      } else if (this.probability < 80) {
        return "orange";
      } else {
        return "green";
      }
    },
  }
};
</script>

<style scoped>
.possible-mushroom {
  background: rgba(0,
      0,
      0,
      0.6);
  padding: 0.5em 1em 0.5em 1em;
  border-radius: 1rem;
  color: white;
  flex-direction: row;
  justify-content: space-between;
  display: flex;
  align-items: center;
  gap: 1.5em;
  width: 100%;
  text-decoration: none;
  cursor: pointer;
}

.probability {
  font-size: 0.8em;
  width: 4rem;
  border-radius: 30px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.left-side {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  gap: 1em;
}

.right-side {
  display: flex;
  flex-direction: column;
  justify-content: flex-start !important;
  align-items: center;
  gap: 0.3em;
  margin: 0;
}

.red {
  background-color: var(--red);
}

.orange {
  background-color: var(--warning);
}

.green {
  background-color: var(--green);
}

.album-photo {
  width: 4rem;
  height: 4rem;
  object-fit: cover;
  border-radius: 0.5rem;
}

.possible-mushroom h2,
.possible-mushroom p {
  margin: 0;
}

.mushroom-name {
  font-size: 1.2em;
}

.mushroom-latin {
  font-size: 1em;
  font-style: italic;
}

@media screen and (max-width: 768px) {
  .possible-mushroom {
    flex-direction: column;
    align-items: flex-start;
    gap: 1em;
  }

  .mushroom-names {
    flex-wrap: wrap;
  }

  .right-side {
    flex-direction: row;
    align-items: center;
    gap: 0.5em;
  }

  .album-photo {
    display: none;
  }
}
</style>