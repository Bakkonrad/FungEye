<template>
  <div class="result" :style="{ backgroundImage: 'url(' + image + ')' }">
    <PossibleMushroom :image="image" mushroomName="Grzybek" mushroomLatin="Jakiś" :probability="50">
    </PossibleMushroom>
    <PossibleMushroom :image="image" mushroomName="Grzybek" mushroomLatin="Jakiś" :probability="34">
    </PossibleMushroom>
  </div>
</template>

<script>
import PossibleMushroom from "./PossibleMushroom.vue";

export default {
  components: {
    PossibleMushroom,
  },
  props: {
    image: {
      type: String,
      required: true,
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
.result {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-end;
  position: relative;
  height: auto;
  max-width: 100em;
  max-height: 150em;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  border-radius: 0.5rem;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  padding: 2em;
  gap: 1em;
}

@media screen and (max-width: 768px) {
  .result {

    height: 25em;
  }

}
</style>
