<!-- TODO responsywność -->
<!-- TODO czy chcemy żeby to były prawdziwe posty - jeśli tak to będzie trzeba zrobić je jako zmienne, slider -->
<!-- TODO kolory dodać jako zmienne (nazwy w App) -->

<template>
  <div>
    <Navbar />
  </div>
  <div class="container">
    <section class="header">
      <div class="header-content">
        <!-- <h1>miej oko na grzyby z</h1>
        <h2>FungEye</h2> -->
        <h3>miej oko na grzyby z</h3>
        <h1>FungEye</h1>
        <p>
          jakiś tekst zachęcający do naszej super aplikacji<br />
          taki dłuższy trochę<br />
          żeby wiatr w nie hulał<br />
          można wpisać co się da tu zrobić:<br />
          <ul>
            <li>przeglądaj grzyby</li>
            <li>obserwuj pogodę</li>
            <li>połącz się ze swoimi znajomymi grzybiarzami i konkuruj z nimi</li>
            <li>zdobywaj trofea</li>
            <li>a co najważniejsze - rozpoznawaj gatunki grzybów!</li>
          </ul>
        </p>
        <!-- <button>Rozpoznaj grzyba</button> -->
        <RouterLink to="/recognize" class="btn fungeye-default-button">Rozpoznaj grzyba</RouterLink>
      </div>
      <div class="header-image">
        <img src="..\assets\images\backgrounds\Rectangle 11.png" alt="Mushrooms in forest" />
      </div>
    </section>
    <section class="main">
      <h2>Dołącz do grona grzybiarzy</h2>
      <p1>Dziel się swoimi zdobyczami z innymi miłośnikami zbierania grzybów.</p1>
      <div class="cards">
        <div class="card left-card">
          <img src="..\assets\images\backgrounds\left-post.png" alt="Mushroom 1" class="first-image" />
          <div class="card-content">
            <h3>FanGrzybow</h3>
            <p>Dzisiaj w lesie wypatrzyłem takiego pięknego grzybka. Myślałem że to prawdziwek, ale coś mi podpowiedziało żeby sprawdzić.. wziąłem telefon i go zeskanowałem aplikacją FungEye. Okazało się że to Muchomor Zwyczajny - trujący!!!!!! Uważajcie na to co zbieracie!!!!!1111!</p>
          </div>
        </div>
        <div class="card middle-card">
          <img src="..\assets\images\backgrounds\middle-post.png" alt="Mushroom 2" />
          <div class="card-content">
            <h3>janinka_0234</h3>
            <p>Uwielbiam jeść grzyby na obiad, ale największą przyjemność sprawia mi zbieranie ich w lesie. Jednakowoż ja jestem samoukiem i nie umiem rozpoznawać gatunków. W zeszłym roku dowiedziałam się o aplikacji FungEye i moje życie odmieniło się o 360 stopni... Już nie wyobrażam sobie grzybobrania bez tego narzędzia!!</p>
          </div>
        </div>
        <div class="card right-card">
          <img src="..\assets\images\backgrounds\right-post.png" alt="Mushroom 3" />
          <div class="card-content">
            <h3>muhomorex</h3>
            <p>Taki piękny muchomorek! Po deszczu rosną jak grzyby...</p>
          </div>
        </div>
      </div>
      <!-- <button class="portal-button">Przejdź do portalu</button> -->
      <div class="button-container">
        <RouterLink to="/portal" class="btn fungeye-default-button" id="portal-button" >Przejdź do portalu</RouterLink>
      </div>
    </section>
  </div>
  <div>
    <Footer></Footer>
  </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";
import Footer from "../components/Footer.vue";
export default {
  components: {
    Navbar,
    Footer,
  },
};
</script>

<!-- scoped? -->
<style scoped>
.container {
  color: #333;
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  background: linear-gradient(to top, #386641, #6A994E);
  border-radius: 20px;
  color: white;
  max-width: 1520px; /* Maksymalna szerokość */
  margin: 0 auto; /* Wyśrodkowanie */

}

.header-content {
  max-width: 50%;
  padding: 20px; /* Padding dla tekstu */
}

.header-image img {
  max-width: 100%;
  height: auto;
  padding: 0; /* Usunięcie paddingu z obrazu */
  border-top-right-radius: 20px;
  border-bottom-right-radius: 20px;
}
/* można by było dodać klasy/idiki do tekstu (np. .header h1 na header-title albo coś w tym stylu) */
.header h1, .header h2 {
  margin: 0;
}

.header p1 {
  
  margin-top: 20px;
}

.header p {
  margin-top: 20px;
}

.header ul {
  font-size: 20px;
  padding-left: 20px;
}

/* .header button {
  margin-top: 20px;
  padding: 10px 25px;
  background-color: #8fb339;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 20px;
} */

.header-image img {
  max-width: 100%;
  height: auto;
  padding: 0; /* Usunięcie paddingu z obrazu */
}

.main {
  text-align: center;
  margin-top: 40px;
}

.main h2 {
  font-size: 50px;
  margin-bottom: 10px;
}

.main p1 {
  font-size: 30px;
  margin-bottom: 30px;
}

.main p {
  margin-bottom: 20px;
}

.cards {
  display: flex;
  justify-content: space-around;
  margin-top: 130px; /* Dodanie marginesu górnego dla sekcji kart */
  align-items: flex-start; 
  margin-bottom: 50px;
}

.card {
  border-radius: 20px; /* Zaokrąglenie ramek */
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  color: white;
}

.first-image {
  padding: 0;
}

.left-card {
  width: 22%; /* Ustawienie szerokości left-card i right-card na 25% */
  height: 600px;
  background: linear-gradient(to left, #386641, #6A994E);
  margin-left: 150px;
}

.right-card {
  width: 22%; /* Ustawienie szerokości left-card i right-card na 25% */
  height: 600px;
  background: linear-gradient(to left, #386641, #6A994E);
  margin-right: 150px;
}

.middle-card {
  width: 28%;
  height: 700px; /* Zwiększona wysokość środkowej karty */
  background: linear-gradient(to right, #386641, #6A994E);
  margin-top: -50px;
}

.card img {
  width: 100%;
  height: auto;
}

.card-content {
  padding: 15px;
}

.card-content h3 {
  margin-top: 0;
}

/* .portal-button {
  margin-top: 40px;
  padding: 10px 20px;
  background-color: #6A994E;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 20px; /* Zaokrąglenie ramek */
/* } */

.button-container {
  display: flex;
  justify-content: center;
}

</style>
