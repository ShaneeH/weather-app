<script setup lang="ts">
import { ref } from 'vue'
import viteLogo from '../assets/vite.svg'
import heroImg from '../assets/hero.png'
import vueLogo from '../assets/vue.svg'
import { onBeforeMount } from 'vue'

const count = ref(0)

onBeforeMount(() => 
{
  localStorage.setItem('HEY' , 'D');
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
    </div>
  </section>

  <div class="ticks"></div>


  <div class="ticks"></div>
  <section id="spacer"></section>
</template>
