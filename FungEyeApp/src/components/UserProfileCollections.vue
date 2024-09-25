<template>
  <div id="collections">
    <div class="bottom-collections">
      <div class="collection">
        <h3>Zebrane grzyby &rarr;</h3>
        <div class="hstack gap-3" id="mushroom-collection">
          <div class="p-2" v-for="mushroom in mushrooms" :key="mushroom">
            <img class="mushroom" :src="mushroom" alt="Mushroom" />
          </div>
        </div>
      </div>
      <div class="collection">
        <h3>Obserwowani &rarr;</h3>
        <div class="hstack gap-3" id="follows-collection">
          <div class="p-2" v-for="follow in follows" :key="follow">
            <router-link
              :to="'/user-profile/' + follow.id"
              class="follow-content r-link"
            >
              <ProfileImage :imgSrc="follow.img" :width="100" :height="100" />
              <p>{{ follow.name }}</p>
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileImage from "@/components/ProfileImage.vue";

export default {
  components: {
    ProfileImage,
  },
  props: {
    mushrooms: Array,
    trophys: Array,
    follows: Array,
  },
};
</script>

<style scoped>
#collections {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 2em;
}

.collection {
  margin-top: 3em;
  background-color: rgba(0, 0, 0, 0.063);
  padding: 10px 20px 10px 20px;
  border-radius: 15px;
}

.upper-collection {
  width: 50vw;
}

.bottom-collections {
  display: flex;
  flex-direction: row;
  gap: 2em;
}

#mushroom-collection,
#trophy-collection,
#follows-collection {
  display: flex;
  justify-content: flex-start;
  flex-wrap: wrap;
}

.mushroom {
  width: auto;
  height: 100px;
  border-radius: 15px;
}

.trophy-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.follow-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-decoration: none;
}

@media screen and (max-width: 1630px) {
  .upper-collection {
    width: 80vw;
  }
  /* .bottom-collections {
    width: 80vw;
  } */
}

@media screen and (max-width: 768px) {
  #collections {
    gap: 1em;
  }
  .collection {
    width: 90vw;
  }

  .bottom-collections {
    flex-direction: column;
    gap: 2em;
  }
  
}

</style>
