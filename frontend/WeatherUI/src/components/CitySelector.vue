<script setup lang="ts">
const props = defineProps<{
  cities: string[]
  modelValue: string
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()

function onCityChange(event: Event) {
  const target = event.target as HTMLSelectElement
  emit('update:modelValue', target.value)
}
</script>

<template>
  <div class="city-selector">
    <label for="city" class="label">Select City</label>

    <div class="select-wrapper">
      <select
        id="city"
        :value="modelValue"
        @change="onCityChange"
        class="select"
      >
        <option disabled value="">Choose a city</option>
        <option v-for="city in cities" :key="city" :value="city">
          {{ city }}
        </option>
      </select>

      <!-- Custom chevron -->
      <svg class="chevron" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <polyline points="6 9 12 15 18 9" />
      </svg>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;600&display=swap');

.city-selector {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.label {
  font-family: 'JetBrains Mono', monospace;
  font-size: 14px;
  font-weight: 600;
  letter-spacing: 0.2em;
  text-transform: uppercase;
  color: #807a5a;
}

.select-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.select {
  width: 100%;
  appearance: none;
  -webkit-appearance: none;
  background: #0d1120;
  color: #e8eaf0;
  border: 1px solid #1e2540;
  border-radius: 10px;
  padding: 11px 42px 11px 16px;
  font-family: 'JetBrains Mono', monospace;
  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.04em;
  cursor: pointer;
  transition: border-color 0.2s, box-shadow 0.2s;
  outline: none;
}

.select:hover {
  border-color: rgba(240, 165, 0, 0.4);
}

.select:focus {
  border-color: #f0a500;
  box-shadow: 0 0 0 3px rgba(240, 165, 0, 0.12);
}

/* Style the dropdown options */
.select option {
  background: #0d1120;
  color: #e8eaf0;
}

.chevron {
  position: absolute;
  right: 14px;
  width: 14px;
  height: 14px;
  color: #f0a500;
  pointer-events: none;
  flex-shrink: 0;
}
</style>