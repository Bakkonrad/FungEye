<template>
  <div id="collections">
    <div class="bottom-collections">
      <div class="collection">
        <h3>Ostatnio zapisane grzyby</h3>
        <div v-if="mushrooms.length > 0" class="mushrooms">
          <div class="hstack gap-3 mushroom-collection">
            <div class="p-2" v-for="mushroom in mushrooms" :key="mushroom">
              <img class="mushroom" :src="mushroom.imagesUrl[0]" :alt="mushroom.polishName"
                @click="goToMushroom(mushroom.id)" />
            </div>
          </div>
        </div>
        <div v-else>
          <p>Brak zapisanych grzybów</p>
        </div>
      </div>
      <div class="collection">
        <h3>Obserwowani</h3>
        <div v-if="follows.length > 0" class="hstack gap-3 follows-collection">
          <div class="p-2" v-for="follow in follows" :key="follow">
            <router-link :to="'/user-profile/' + follow.id" class="follow-content r-link">
              <ProfileImage :imgSrc="follow.imageUrl" :width="100" :height="100" />
              <p>{{ follow.username }}</p>
            </router-link>
          </div>
        </div>
        <div v-else>
          <p>Brak obserwowanych</p>
        </div>
      </div>
      <div class="collection">
        <h3>Obserwatorzy</h3>
        <div v-if="followers.length > 0" class="hstack gap-3 follows-collection">
          <div class="p-2" v-for="follower in followers" :key="follower">
            <router-link :to="'/user-profile/' + follower.id" class="follow-content r-link">
              <ProfileImage :imgSrc="follower.imageUrl" :width="100" :height="100" />
              <p>{{ follower.username }}</p>
            </router-link>
          </div>
        </div>
        <div v-else>
          <p>Brak obserwatorów</p>
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
    followers: Array,
    showMoreMushrooms: Boolean,
  },
  methods: {
    goToMushroom(id) {
      this.$router.push("/mushroom/" + id);
    },
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

.mushrooms {
  display: flex;
  flex-direction: column;
  gap: 1em;
}

.upper-collection {
  width: 50vw;
}

.bottom-collections {
  display: flex;
  flex-direction: row;
  gap: 2em;
}

.mushroom-collection,
.follows-collection {
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
