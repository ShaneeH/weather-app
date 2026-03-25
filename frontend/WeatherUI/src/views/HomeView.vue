<script setup lang="ts">
import { onBeforeMount } from 'vue'
import { httpClient } from '../api/httpClient'
import viteLogo from '../assets/vite.svg'
import heroImg from '../assets/hero.png'
import vueLogo from '../assets/vue.svg'
import CitySelector from '../components/CitySelector.vue'
const url: string = "https://localhost:7157/api/weather/cities";

 onBeforeMount(() => {
  localStorage.setItem('HEY', 'D');


  httpClient.get(url).then(data => {
    console.log(data)
  }) 
})

</script>

<template>
  <section id="center">
    <div class="hero">
      <img :src="heroImg" class="base" width="170" height="179" alt="" />
      <img :src="vueLogo" class="framework" alt="Vue logo" />
      <img :src="viteLogo" class="vite" alt="Vite logo" />
    </div>
    <div>
      <h1>Tokyo</h1>
      <CitySelector />
    </div>
  </section>

  <div class="ticks"></div>


  <div class="ticks"></div>
  <section id="spacer"></section>
</template>

