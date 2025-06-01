<template>
  <div class="input-wrap">
    <label v-if="label" class="form-label" :class="color">{{ label }}</label>
    <input class="form-control" :class="color" type="text" :value="modelValue" @input="$emit('update:modelValue', $event.target.value)"
      v-bind="$attrs" />
  </div>
</template>

<script>
export default {
  props: {
    modelValue: {
      type: String
    },
    label: {
      type: String,
      default: "",
    },
    color: {
      type: String,
      default: "white",
    },
  },
}
</script>

<style scoped>
.input-wrap {
  margin-bottom: 1rem;
}

input {
  width: 100%;
  padding: 0.5rem;
  font-size: 1rem;
  border: 1px solid #ccc;
  border-radius: 0.25rem;
}

input::placeholder {
  color: #ccc !important;
}

.white {
  color: white;
}

.black {
  color: var(--black) !important;
}
</style>
