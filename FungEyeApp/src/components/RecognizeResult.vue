<template>
  <div class="result" :style="{ backgroundImage: 'url(' + image + ')' }">
    <PossibleMushroom
      v-for="r in results"
      :key="r.id"
      :id="r.id"
      :polishName="r.polishName"
      :latinName="r.latinName"
      :probability="r.probability"
      :image="r.image"
    ></PossibleMushroom>
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
    results: {
      type: Array,
      required: true,
    },
  },
  data () {
    return {
      result: []
    }
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
