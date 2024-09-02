<template>
  <div class="container">
      <section class="header">
        <!-- <div class="header-image">
          <img src="../assets/images/backgrounds/pexels-fabianwiktor-3466363.jpg" alt="Mushrooms in forest" />
        </div> -->
        <div class="header-content">
          <h3>Miej oko na grzyby z</h3>
          <h1 id="fungeye">FungEye</h1>
          <p>
            Odkryj fascynujący świat grzybów dzięki naszej innowacyjnej aplikacji FungEye! Co możesz z nią zrobić?
            <ul>
              <li><b>Przeglądaj grzyby:</b> Eksploruj bogatą bazę danych grzybów i poznawaj nowe gatunki.</li>
              <li><b>Obserwuj pogodę:</b> Sprawdzaj prognozy pogody, aby zaplanować idealny wypad na grzybobranie.</li>
              <li><b>Połącz się z grzybiarzami:</b> Dołącz do społeczności grzybiarzy, dziel się swoimi odkryciami i konkuruj z przyjaciółmi.</li>
              <li><b>Zdobywaj trofea:</b> Zbieraj osiągnięcia i chwal się swoimi sukcesami.</li>
              <li><b>Gatunki grzybów:</b> Korzystaj z zaawansowanego systemu rozpoznawania grzybów, który pomoże Ci zidentyfikować znalezione okazy.</li>
            </ul>
            
          </p>
          <p>Dołącz do nas i zacznij swoją przygodę z grzybobraniem na nowo – z FungEye!</p>
          <router-link to="/recognize" class="btn fungeye-default-button" >Rozpoznaj grzyby</router-link>
        </div>
      </section>
    <section class="main">
      <h2>Dołącz do grona grzybiarzy</h2>
      <h3>Dziel się swoimi zdobyczami z innymi miłośnikami zbierania grzybów.</h3>
      <div class="cards">
        <div class="card left-card">
          <img src="../assets/images/backgrounds/pexels-fabianwiktor-3466363.jpg" alt="Mushroom 1" class="first-image" />
          <div class="card-body">
            <h3 class="card-title">FanGrzybow</h3>
            <p class="card-text">“Z FungEye grzybobranie stało się jeszcze bardziej ekscytujące! Aplikacja ma intuicyjny interfejs, dzięki czemu nawet osoby niezbyt obeznane z technologią poradzą sobie bez problemu. Funkcja obserwacji pogody pozwala mi planować idealne wypady do lasu.”</p>
          </div>
        </div>
        <div class="card middle-card">
          <img  src="../assets/images/backgrounds/basket-with-mushrooms-knife-standing-forest-glade.jpg" alt="Mushroom 2" />
          <div class="card-body">
            <h3 class="card-title">janina_0234</h3>
            <p class="card-text">“FungEye to must-have dla każdego miłośnika grzybobrania! Aplikacja ułatwia zbieranie grzybów dzięki szczegółowej bazie danych i niezawodnemu systemowi rozpoznawania gatunków. Podoba mi się, że mogę przeglądać zdjęcia i opisy grzybów, a także śledzić swoje osiągnięcia. Dodatkowo, możliwość kontaktu z innymi grzybiarzami i wspólne rywalizowanie dodaje smaczku każdej wyprawie.”</p>
          </div>
        </div>
        <div class="card right-card">
          <img src="../assets/images/backgrounds/mushrooms-grown-after-rain-middle-forest.jpg" alt="Mushroom 3" />
          <div class="card-body">
            <h3 class="card-title">MarekLas</h3>
            <p class="card-text">“FungEye znacząco ułatwiło moje wypady na grzyby. Aplikacja jest prosta w obsłudze, a funkcja rozpoznawania gatunków jest niezastąpiona. Dzięki niej czuję się pewniej w lesie. Zdecydowanie polecam wszystkim grzybiarzom, zarówno początkującym, jak i zaawansowanym!”</p>
          </div>
        </div>
      </div>
      <div class="button-container">
        <RouterLink to="/portal" class="btn fungeye-default-button" id="portal-button" >Przejdź do portalu</RouterLink>
      </div>
    </section>
  </div>
</template>

<script>
</script>

<!-- scoped? -->
<style scoped>
.container {
  color: #333;
  padding: 20px;
  /* overflow: hidden; */
  position: relative;
}

.header {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  background-image: url("../assets/images/backgrounds/mushroom.jpg");
  background-size: cover;
  background-position: center;
  color: white;
  width: 99vw; /* Pełna szerokość ekranu */
  padding: 5em;
  margin: 0;
  /* overflow: hidden; */
  top: 0;
  left: 50%;
  transform: translateX(-50%);
}

.header::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 101%;
  background-image: inherit;
  background-size: inherit;
  background-position: inherit;
  filter: blur(2px); /* Dodanie efektu rozmycia */
  box-sizing: border-box; /* Uwzględnienie obramowania w wymiarach elementu */
  z-index: 1;
}

.header-content {
  background: linear-gradient(to top, rgba(0, 0, 0, 0.8), rgba(0, 0, 0, 0.4)); /* Gradient od czarnego do przezroczystego */
  z-index: 2;
  padding: 40px;
  border-radius: 20px;
}

/* można by było dodać klasy/idiki do tekstu (np. .header h1 na header-title albo coś w tym stylu) */
.header h1, .header h2 {
  margin: 0;
}

#fungeye {
  font-family: 'Avenir Next', sans-serif;
}

.header h3 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  margin-top: 20px;
}

.header p {
  margin-top: 20px;
}

.header ul {
  font-size: 20px;
  padding-left: 20px;
}

.main {
  position:relative;
  text-align: center;
  margin-top: 3em;
}

.main h2 {
  font-size: 50px;
  margin-bottom: 10px;
}

.main p1 {
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-weight: 200;
  font-size: 30px;
}

.main p {
  margin-bottom: 10px;
}

.cards {
  display: flex;
  justify-content: space-around;
  margin-top: 50px; /* Dodanie marginesu górnego dla sekcji kart */
  align-items: center; 
  margin-bottom: 50px;
  gap: 2em;
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
  width: 30%; /* Ustawienie szerokości left-card i right-card na 25% */
  height: 550px;
  background: linear-gradient(to left, var(--dark-green), var(--green));
}

.right-card {
  width: 30%; /* Ustawienie szerokości left-card i right-card na 25% */
  height: 550px;
  background: linear-gradient(to left,var(--dark-green), var(--green));
}

.middle-card {
  width: 35%;
  height: 650px; /* Zwiększona wysokość środkowej karty */
  background: linear-gradient(to right, var(--dark-green), var(--green));
  /* margin-top: -50px; */
}

.card img {
  width: 100%;
  height: auto;
  object-fit: cover;
  object-position: center;
}

.left-card img,
.right-card img {
  max-height: 250px;
}

.card-content {
  padding: 15px;
}

.card-content h3 {
  margin-top: 0;
}

.button-container {
  display: flex;
  justify-content: center;
}

@media screen and (max-width: 1200px) {

  .cards {
    flex-direction: column;
    gap: 3em;
  }

  .left-card, .middle-card, .right-card {
    width: 80%;
    height: auto;
  }
}

</style>
