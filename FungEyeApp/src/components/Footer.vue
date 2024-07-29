<template>
  <div class="container">
    <footer
      class="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top"
    >
      <p class="col-md-4 mb-0 text-body-secondary">© 2024 FungEye</p>

      <ul class="navbar-nav col-md-6 justify-content-end">
        <li class="nav-item">
          <RouterLink to="/" class="nav-link">Strona główna</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/recognize" class="nav-link"
            >Rozpoznawanie grzybów</RouterLink
          >
        </li>
        <li class="nav-item">
          <RouterLink to="/portal" class="nav-link">Portal</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/weather" class="nav-link">Pogoda</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink to="/atlas" class="nav-link">Atlas</RouterLink>
        </li>
      </ul>
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

ul {
  flex-direction: row;
}

@media (max-width: 768px) {
  footer {
    flex-direction: column;
    gap: 3em;
  }
  ul {
    flex-direction: column;
    gap: 0.5em;
  }
}

</style>
