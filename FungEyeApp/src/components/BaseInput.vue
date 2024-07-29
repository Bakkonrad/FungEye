<template>
  <div class="input-wrap">
    <label v-if="label" class="form-label">{{ label }}</label>
    <input
      class="form-control"
      type="text"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      v-bind="$attrs"
    />
  </div>
</template>

<script setup>
defineProps({
  label: {
    type: [String, Boolean],
    default: false,
  },
  modelValue: {
    type: String,
    default: "",
  },
});
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
</style>
