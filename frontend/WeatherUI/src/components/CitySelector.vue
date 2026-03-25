<script setup lang="ts">
import { ref } from 'vue'

const cities = ['Dublin', 'London', 'New York']
const selectedCity = ref('')
</script>


<template>
  <div class="city-selector">
    <label for="city" class="label">Select city</label>

    <select id="city" v-model="selectedCity" class="select">
      <option disabled value="">Choose a city</option>
      <option v-for="city in cities" :key="city" :value="city">
        {{ city }}
      </option>
    </select>
  </div>
</template>

<style scoped>
.city-selector {
  width: 280px;
}

.label {
  display: block;
  margin-bottom: 6px;
  font-size: 14px;
  font-weight: 600;
  color: #333;
}

.select {
  width: 100%;
  padding: 10px 12px;
  font-size: 14px;
  font-family: inherit;
  color: #222;
  background-color: #fff;
  border: 1px solid #cfcfcf;
  border-radius: 6px;
  outline: none;
  cursor: pointer;
}

.select:focus {
  border-color: #666;
}

.select:hover {
  border-color: #999;
}
</style>