<template>
  <div class="container">
    <section class="header">
      <div class="header-content">
        <h1>Sprawdź pogodę</h1>
        <p>Zaplanuj swoją wyprawę na grzyby sprawdzając aktualną pogodę w wybranej lokalizacji.</p>
        <div class="search-container">
          <input 
            v-model="city" 
            placeholder="Wpisz nazwę miasta" 
            @keyup.enter="fetchWeather"
            class="search-input" 
          />
          <button @click="fetchWeather" class="btn fungeye-default-button">
            Sprawdź pogodę
          </button>
        </div>
      </div>
    </section>

    <section class="main">
      <div v-if="error" class="error-container">
        <div class="error-message">{{ error }}</div>
      </div>

      <div v-if="loading" class="loading-container">
        <LoadingSpinner></LoadingSpinner>
      </div>

      <div v-if="weather" class="weather-card">
        <h2>Pogoda w miejscowości {{ displayCity }}</h2>
        <div class="weather-info">
          <div class="weather-item">
            <i class="fas fa-temperature-high"></i>
            <p>Temperatura</p>
            <span>{{ weather.current.temperature_2m }}°C</span>
          </div>
          <div class="weather-item">
            <i class="fas fa-tint"></i>
            <p>Wilgotność</p>
            <span>{{ weather.current.relative_humidity_2m }}%</span>
          </div>
          <div class="weather-item">
            <i class="fas fa-wind"></i>
            <p>Prędkość wiatru</p>
            <span>{{ weather.current.wind_speed_10m }} km/h</span>
          </div>
          <div class="weather-item">
            <i class="fas fa-cloud"></i>
            <p>Warunki</p>
            <span>{{ getWeatherCondition(weather.current.weather_code) }}</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import ApiService from '@/services/WeatherService';
import LoadingSpinner from '@/components/LoadingSpinner.vue';

export default {
  name: 'WeatherView',
  components: {
    LoadingSpinner
  },
  data() {
    return {
      city: '',
      displayCity: '',
      weather: null,
      error: null,
      loading: false,
    };
  },
  methods: {
    normalizeCity(cityName) {
      if (cityName.toLowerCase().includes('warszawa')) {
        return 'Warsaw';
      }
      return cityName;
    },

    async fetchWeather() {
      this.error = null;
      this.weather = null;
      this.loading = true;
      
      try {
        const displayCity = this.city; // zachowaj oryginalną nazwę do wyświetlenia
        const searchCity = this.normalizeCity(this.city); // zamień na Warsaw jeśli trzeba
        
        // First, geocode the city
        const geoResponse = await ApiService.geocodeCity(searchCity);
        console.log('Geocoding response:', geoResponse.data);
        
        if (geoResponse.data.results && geoResponse.data.results.length > 0) {
          const { latitude, longitude } = geoResponse.data.results[0];
          console.log(`Coordinates: lat ${latitude}, lon ${longitude}`);
          
          // Then, fetch the weather
          const weatherResponse = await ApiService.getWeather(latitude, longitude);
          console.log('Weather response:', weatherResponse.data);
          
          this.weather = weatherResponse.data;
          this.displayCity = displayCity; // pokazuje oryginalną nazwę (Warszawa)
        } else {
          this.error = "Nie znaleziono miasta. Spróbuj wpisać inną nazwę.";
        }
      } catch (error) {
        console.error('Error details:', error.response || error);
        this.error = "Wystąpił błąd podczas pobierania danych pogodowych.";
      } finally {
        this.loading = false;
      }
    },

    getWeatherCondition(code) {
      const conditions = {
        0: 'Bezchmurnie',
        1: 'Przeważnie bezchmurnie',
        2: 'Częściowe zachmurzenie',
        3: 'Zachmurzenie',
        45: 'Mgła',
        51: 'Lekka mżawka',
        53: 'Umiarkowana mżawka',
        55: 'Gęsta mżawka',
        61: 'Lekki deszcz',
        63: 'Umiarkowany deszcz',
        65: 'Intensywny deszcz',
        71: 'Lekki śnieg',
        73: 'Umiarkowany śnieg',
        75: 'Intensywny śnieg',
        80: 'Lekkie opady deszczu',
        81: 'Umiarkowane opady deszczu',
        82: 'Silne opady deszczu',
        85: 'Lekkie opady śniegu',
        86: 'Silne opady śniegu',
        95: 'Burza z lekkim deszczem',
        96: 'Burza z lekkim gradem',
        99: 'Burza z intensywnym gradem'
      };
      return conditions[code] || 'Nieznane warunki';
    }
  }
};
</script>

<style scoped>
.container {
  color: #333;
  padding: 20px;
  position: relative;
}

.header {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: var(--dark-green);
  background-image: url("../assets/images/backgrounds/weather-background.jpg"); /* You'll need to add this image */
  background-size: cover;
  background-position: center;
  color: white;
  width: 99vw;
  padding: 5em;
  margin: 0;
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
  filter: blur(2px);
  z-index: 1;
}

.header-content {
  background: linear-gradient(to top, rgba(0, 0, 0, 0.8), rgba(0, 0, 0, 0.4));
  z-index: 2;
  padding: 40px;
  border-radius: 20px;
  text-align: center;
}

.search-container {
  display: flex;
  gap: 1em;
  margin-top: 2em;
  justify-content: center;
}

.search-input {
  padding: 10px 20px;
  border-radius: 5px;
  border: none;
  width: 300px;
  font-size: 1.1em;
}

.weather-card {
  background: linear-gradient(to right, var(--dark-green), var(--green));
  border-radius: 20px;
  padding: 2em;
  color: white;
  margin-top: 2em;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.weather-info {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 2em;
  margin-top: 2em;
}

.weather-item {
  text-align: center;
  padding: 1em;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 10px;
}

.weather-item i {
  font-size: 2em;
  margin-bottom: 0.5em;
}

.weather-item p {
  margin: 0.5em 0;
  font-size: 1.1em;
}

.weather-item span {
  font-size: 1.5em;
  font-weight: bold;
}

.error-container {
  text-align: center;
  margin-top: 2em;
}

.error-message {
  background-color: #ff5757;
  color: white;
  padding: 1em;
  border-radius: 5px;
  display: inline-block;
}

@media screen and (max-width: 768px) {
  .header {
    padding: 1em;
  }

  .search-container {
    flex-direction: column;
  }

  .search-input {
    width: 100%;
  }

  .weather-info {
    grid-template-columns: 1fr;
  }
}
</style>