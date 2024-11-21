import axios from 'axios';

const ApiService = {
  async geocodeCity(city) {
    try {
      const url = `https://geocoding-api.open-meteo.com/v1/search?name=${encodeURIComponent(city)}&count=1&language=en&format=json`;
      return await axios.get(url);
    } catch (error) {
      console.error('Geocoding error:', error.response || error);
      throw error;
    }
  },

  async getWeather(latitude, longitude) {
    try {
      const url = `https://api.open-meteo.com/v1/forecast?latitude=${latitude}&longitude=${longitude}&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m&timezone=auto`;
      return await axios.get(url);
    } catch (error) {
      console.error('Weather API error:', error.response || error);
      throw error;
    }
  },
};

export default ApiService;