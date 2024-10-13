<template>
    <div class="post-view">
      <!-- Sekcja obrazka (opcjonalna) -->
      <div v-if="post.image" class="post-image">
        <img :src="post.image" alt="Zdjęcie posta" />
      </div>
  
      <!-- Treść posta -->
      <div class="post-content">
        <div class="post-text">
          <!-- Nazwa użytkownika i treść posta -->
          <h3 class="username">{{ post.username }}</h3>
          <p>{{ post.content }}</p>
        </div>
  
        <!-- Informacje o like'ach i komentarzach -->
        <div class="post-stats">
          <span class="likes">{{ post.likes }} 👍</span>
          <span class="comments">{{ post.comments.length }} 💬</span>
        </div>
  
        <!-- Przycisk polubienia -->
        <button @click.stop="toggleLike" :class="{ liked: isLiked }">
          {{ isLiked ? 'Lubię to 👍' : 'Lubię to 👍' }}
        </button>
      </div>
  
      <!-- Sekcja komentarzy -->
      <div class="comments-section">
        <!-- Pole do dodawania komentarza -->
        <div class="add-comment">
          <input
            v-model="newComment"
            @keyup.enter="addComment"
            type="text"
            placeholder="Dodaj komentarz..."
          />
          <button @click="addComment">Dodaj</button>
        </div>
  
        <!-- Lista istniejących komentarzy -->
        <div class="existing-comments" v-if="post.comments.length">
          <div v-for="(comment, index) in post.comments" :key="index" class="comment-box">
            <!-- Nazwa użytkownika przy komentarzu -->
            <h4 class="comment-username">{{ comment.username }}</h4>
            <p>{{ comment.text }}</p>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        post: {
          username: 'Mariusz Pudzianowski',
          image: 'src/assets/images/mushrooms/a8eb8c5b-aaed-46e3-8798-a563d52ad54b.jpeg',
          content: 'Patrzcie jakiego super grzyba znalazłem! Dzięki aplikacji FungEye niestety wiem, że jest on trujący 🍄',
          likes: 100,
          comments: [
            { username: 'Robert Kubica', text: 'Ale trujak!' },
            { username: 'Adam Małysz', text: 'Wygląda smacznie!' },
            { username: 'Magda Gessler', text: 'Gdzie znalazłeś tak wspaniałego grzyba!?' }
          ]
        },
        isLiked: false,
        newComment: '',
        currentUser: 'Gal Anonim'
      };
    },
    methods: {
      toggleLike() {
        if (this.isLiked) {
          this.post.likes--;
        } else {
          this.post.likes++;
        }
        this.isLiked = !this.isLiked;
      },
      addComment() {
        if (this.newComment.trim()) {
          this.post.comments.push({ username: this.currentUser, text: this.newComment });
          this.newComment = '';
        }
      },
      goToPortal() {
        // Przeniesienie do portalu
        this.$router.push({ name: 'portal' });
      }
    }
  };
  </script>
  
  <style scoped>
  .post-view {
    width: 100%;
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
    background-color: #fff7e6;
    border-radius: 15px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
   
  .post-image img {
    width: 100%;
    max-height: 400px;
    object-fit: cover;
    border-radius: 15px;
    cursor: pointer;
  }
  
  .post-content {
    padding: 15px;
    margin-top: 20px;
    background-color: #fff7e6;
    border-radius: 15px;
    cursor: pointer;
  }
  
  .username {
    font-size: 18px;
    font-weight: bold;
    color: #333;
    margin-bottom: 10px;
  }
  
  .post-text {
    font-size: 18px;
    line-height: 1.6;
  }
  
  .post-stats {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
  }
  
  .post-stats span {
    font-size: 16px;
    color: #666;
  }
  
  button {
    padding: 10px 15px;
    margin-top: 10px;
    background-color: #7ea24b;
    color: white;
    border: none;
    border-radius: 10px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  button.liked {
    background-color: #ff4b4b;
  }
  
  button:hover {
    background-color: #688d39;
  }
  
  .comments-section {
    margin-top: 20px;
  }
  
  .add-comment {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 20px;
  }
  
  .add-comment input {
    flex-grow: 1;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid #ccc;
    color: black !important;
  }
  
  .add-comment button {
    padding: 10px 15px;
    background-color: #7ea24b;
    color: white;
    border: none;
    border-radius: 10px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .add-comment button:hover {
    background-color: #688d39;
  }
  
  .existing-comments {
    margin-top: 10px;
  }
  
  .comment-box {
    padding: 10px;
    margin-bottom: 10px;
    background-color: #f0f0f0;
    border-radius: 10px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }
  
  .comment-username {
    font-size: 16px;
    font-weight: bold;
    margin-bottom: 5px;
  }
  </style>
