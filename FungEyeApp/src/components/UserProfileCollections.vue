<template>
  <div id="collections">
    <div class="collection upper-collection">
      <h3>Zebrane grzyby &rarr;</h3>
      <div class="hstack gap-3" id="mushroom-collection">
        <div class="p-2" v-for="mushroom in mushrooms" :key="mushroom">
          <img class="mushroom" :src="mushroom" alt="Mushroom" />
        </div>
      </div>
    </div>
    <div class="bottom-collections">
      <div class="collection">
        <h3>Trofea &rarr;</h3>
        <div class="hstack gap-3" id="trophy-collection">
          <div class="p-2" v-for="trophy in trophys" :key="trophy">
            <div class="trophy-content">
              <img class="mushroom" :src="trophy.img" alt="Trophy" />
              <p>{{ trophy.name }}</p>
            </div>
          </div>
        </div>
      </div>
      <div class="collection">
        <h3>Znajomi &rarr;</h3>
        <div class="hstack gap-3" id="friends-collection">
          <div class="p-2" v-for="friend in friends" :key="friend">
            <router-link
              :to="'/user-profile/' + friend.name"
              class="friend-content r-link"
            >
              <ProfileImage :imgSrc="friend.img" :width="100" :height="100" />
              <p>{{ friend.name }}</p>
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
    friends: Array,
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
#friends-collection {
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

.friend-content {
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
