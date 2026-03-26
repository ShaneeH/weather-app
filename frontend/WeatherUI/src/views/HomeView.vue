<script setup lang="ts">
import heroImg from '../assets/hero.png'
import vueLogo from '../assets/vue.svg'
import CitySelector from '../components/CitySelector.vue'
import.meta.env.CITIES_URL
import { ref, onMounted, watch } from 'vue'
import { getCities } from '../services/weatherService'
import { getSelectedCity, setSelectedCity } from "../services/localStorageService"


const cities = ref<string[]>([])
const selectedCity = ref("")

onMounted(async () => {
  try {
    cities.value = await getCities()

    const savedCity = getSelectedCity()

    if (cities.value.includes(savedCity)) {
      selectedCity.value = savedCity
    } else if (cities.value.length > 0) {
      selectedCity.value = cities.value[0]
    }
  } catch (error) {
    console.error("Failed to fetch cities:", error)
  }
})

watch(selectedCity, (newCity) => {
  if (newCity) {
    setSelectedCity(newCity)
  }
})

</script>

<template>
   
  <section id="center">
    <div class="hero">
      <img :src="heroImg" class="base" width="170" height="179" alt="" />
      <img :src="vueLogo" class="framework" alt="Vue logo" />
 
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

