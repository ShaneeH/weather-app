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
    <label for="city" class="label">Select city</label>

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
  </div>
</template>