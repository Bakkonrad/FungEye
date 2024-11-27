<script setup>
import { ref } from 'vue';
import { MapboxMap, MapboxGeolocateControl, MapboxNavigationControl, MapboxGeocoder } from '@studiometa/vue-mapbox-gl';
import 'mapbox-gl/dist/mapbox-gl.css';
import '@mapbox/mapbox-gl-geocoder/lib/mapbox-gl-geocoder.css';
const mapCenter = ref([16.930, 52.405]);
</script>

<template>
  <h1 class="page-title">Mapa</h1>
  <div class="map-container">
    <MapboxMap class="map" style="width: 100%; height: 85%;"
      access-token="pk.eyJ1IjoiYWRpYmVrIiwiYSI6ImNtMnN6NHY0eDAzZmoybHM5Z3dxajAydDkifQ.n038VfqEieWLx_sk7Rr14g"
      map-style="mapbox://styles/mapbox/streets-v11" :center="mapCenter" :zoom="12">
      <MapboxGeolocateControl :track-user-location="true" :show-user-location="true" />
      <MapboxNavigationControl position="bottom-right" />
      <MapboxGeocoder placeholder="Szukaj" />
    </MapboxMap>
  </div>
</template>

<style scoped>
.map-container {
  width: 100%;
  height: 100svh;
  margin-bottom: -120px;
  color: var(--black) !important;
}

@media (max-width: 768px) and (orientation: portrait) {
  .map-container {
    margin-bottom: -120px;
  }
}

@media (max-height: 768px) and (orientation: landscape) {
  .map-container {
    margin-bottom: -30px;
  }
}
</style>