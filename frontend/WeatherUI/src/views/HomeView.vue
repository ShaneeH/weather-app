<script setup lang="ts">
import { httpClient } from '../api/httpClient'
import viteLogo from '../assets/vite.svg'
import heroImg from '../assets/hero.png'
import vueLogo from '../assets/vue.svg'
import CitySelector from '../components/CitySelector.vue'
import { ref, onMounted, watch } from 'vue'


const cities = ref<string[]>([])
const selectedCity = ref('')

onMounted(async () => {
  try {
    cities.value = await httpClient.get<string[]>('https://localhost:7157/api/weather/cities')

    const savedCity = localStorage.getItem('selectedCity')

    if (savedCity && cities.value.includes(savedCity)) {
      selectedCity.value = savedCity
    } else if (cities.value.length > 0) {
      selectedCity.value = cities.value[0]
    }
  } catch (error) {
    console.error('Failed to load cities:', error)
  }
})

watch(selectedCity, (newCity) => {
  if (newCity) {
    localStorage.setItem('selectedCity', newCity)
  }
})

</script>

<template>
   
  <section id="center">
    <div class="hero">
      <img :src="heroImg" class="base" width="170" height="179" alt="" />
      <img :src="vueLogo" class="framework" alt="Vue logo" />
      <img :src="viteLogo" class="vite" alt="Vite logo" />
    </div>
    <h2> {{ selectedCity }}</h2>
    <div>
     <CitySelector v-model="selectedCity" :cities="cities" />
    </div>
  </section>

  <div class="ticks"></div>


  <div class="ticks"></div>
  <section id="spacer"></section>
</template>

