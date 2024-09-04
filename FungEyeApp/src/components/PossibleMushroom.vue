<template>
    <RouterLink :to="'/mushroom/' + id" class="possible-mushroom">
        <p class="probability">{{ probability }}%</p>
      <img
        src="../assets/images/mushrooms/RECOGNIZE-example-mushroom.jpg"
        alt=""
        class="album-photo"
      />
      <div class="mushroom-names">
        <h2 class="mushroom-name">{{ mushroomName }}</h2>
        <p class="mushroom-latin">{{ mushroomLatin }}</p>
      </div>
      <div class="arrow">
        &#10095;
      </div>
    </RouterLink>
</template>

<script>
export default {
  props: {
    id: {
      type: String,
      default: "0",
    },
    mushroomName: {
      type: String,
      default: "Nieznany gatunek",
    },
    mushroomLatin: {
      type: String,
      default: "Nieznana łacińska nazwa",
    },
    probability: {
      type: Number,
      default: 0,
    },
  },
};
</script>

<style scoped>

.possible-mushroom {
  background: rgba(
    0,
    0,
    0,
    0.6
  ); /* Półprzezroczyste tło dla czytelności tekstu */
  padding: 0.5em 1em 0.5em 1em;
  border-radius: 1rem;
  color: white;
  flex-direction: row;
  display: flex;
  align-items: center;
  gap: 1.5em;
  text-decoration: none;
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
  font-size: 1.5em;
}

.mushroom-latin {
  font-size: 1em;
  font-style: italic;
}
</style>