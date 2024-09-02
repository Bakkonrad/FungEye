<template>
  <div class="container">
    <footer
      class="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top"
    >
      <p class="col-md-4 mb-0 text-body-secondary">© 2024 FungEye</p>

      <p class="col-md-6 warning">
        <b>Uwaga!</b>
        <br /> Wyniki rozpoznawania grzybów uzyskane za pomocą aplikacji FungEye
        mogą nie być w 100% dokładne. Zawsze zalecamy potwierdzenie
        identyfikacji grzybów u eksperta przed ich spożyciem. Twórcy aplikacji
        FungEye nie biorą odpowiedzialności za jakiekolwiek zatrucia czy inne
        szkody wynikające z niewłaściwego rozpoznania grzybów za pomocą
        aplikacji.
      </p>
    </footer>
  </div>
</template>

<script>
export default {
  name: "Footer",
};
</script>

<style scoped>
.border-top {
  border-top: 1px solid var(--dark-green) !important;
}

.container {
  max-width: 97%;
  margin: 0 auto;
}

footer {
  align-items: flex-start !important;
  gap: 2em;
}

.warning {
  font-size: medium;
}

@media (max-width: 768px) {
  footer {
    flex-direction: column;
    gap: 3em;
  }
}
</style>
