<script setup lang="ts">
import heroImg from '../assets/hero.png'
import vueLogo from '../assets/vue.svg'
import CitySelector from '../components/CitySelector.vue'
import { ref, onMounted, watch } from 'vue'
import { getCities, getWeatherByCity } from '../services/weatherService'
import { getSelectedCity, setSelectedCity } from "../services/localStorageService"
import type { CityWeatherData } from '../types/weather'

const cities = ref<string[]>([])
const selectedCity = ref("")
const weatherData = ref<CityWeatherData | null>(null)
const isLoading = ref(false)

async function loadWeather(city: string) {
  if (!city) return

  try {
    isLoading.value = true
    weatherData.value = await getWeatherByCity(city)
  } catch (error) {
    console.error("Failed to fetch weather:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(async () => {
  try {
    cities.value = await getCities()

    const savedCity = getSelectedCity()

    if (cities.value.includes(savedCity)) {
      selectedCity.value = savedCity
    } else if (cities.value.length > 0) {
      selectedCity.value = cities.value[0]
    }

    await loadWeather(selectedCity.value)
  } catch (error) {
    console.error("Failed to load data:", error)
  }
})

watch(selectedCity, async (newCity) => {
  if (!newCity) return

  setSelectedCity(newCity)
  await loadWeather(newCity)
})
</script>

<template>
  <section id="center">

    <div class="dashboard-shell">
      <div class="dashboard-top">
        <div>
    
          <h2 class="city">{{ selectedCity || "Choose a city" }}</h2>
        </div>

        <div class="selector">
          <CitySelector v-model="selectedCity" :cities="cities" />
        </div>
      </div>

      <div v-if="isLoading" class="loading-state">
        Loading weather...
      </div>

      <template v-else-if="weatherData">
        <div class="hero-card">
          <div class="hero-left">
            <img
              :src="'https:' + weatherData.weather.icon"
              class="weather-icon"
              alt="weather icon"
            />

            <div>
              <div class="temperature">
                {{ weatherData.weather.temperatureC }}°C
              </div>

              <div class="condition">
                {{ weatherData.weather.condition }}
              </div>

              <div class="subline">
                {{ weatherData.city }} • {{ weatherData.timezone.localTime }}
              </div>
            </div>
          </div>

          <div class="hero-right">
            <div class="mini-stat">
              <span class="mini-label">Daytime</span>
              <span class="mini-value">{{ weatherData.weather.isDay ? "Yes" : "No" }}</span>
            </div>

            <div class="mini-stat">
              <span class="mini-label">Timezone</span>
              <span class="mini-value">{{ weatherData.timezone.timezoneId }}</span>
            </div>
          </div>
        </div>

        <div class="weather-grid">
          <div class="weather-card">
            <h3>Wind</h3>
            <p>{{ weatherData.weather.windKph }} kph</p>
          </div>

          <div class="weather-card">
            <h3>Humidity</h3>
            <p>{{ weatherData.weather.humidity }}%</p>
          </div>

          <div class="weather-card">
            <h3>Cloud Cover</h3>
            <p>{{ weatherData.weather.cloud }}%</p>
          </div>

          <div class="weather-card">
            <h3>Local Time</h3>
            <p>{{ weatherData.timezone.localTime }}</p>
          </div>

          <div class="weather-card">
            <h3>Sunrise</h3>
            <p>{{ weatherData.astronomy.sunrise }}</p>
          </div>

          <div class="weather-card">
            <h3>Sunset</h3>
            <p>{{ weatherData.astronomy.sunset }}</p>
          </div>

          <div class="weather-card">
            <h3>Moonrise</h3>
            <p>{{ weatherData.astronomy.moonrise }}</p>
          </div>

          <div class="weather-card">
            <h3>Moonset</h3>
            <p>{{ weatherData.astronomy.moonset }}</p>
          </div>

          <div class="weather-card">
            <h3>Moon Phase</h3>
            <p>{{ weatherData.astronomy.moonPhase }}</p>
          </div>
        </div>
      </template>
    </div>
  </section>

  <div class="ticks"></div>
  <div class="ticks"></div>
  <section id="spacer"></section>
</template>

<style scoped>
#center {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 25px;
  padding: 20px;
}

.dashboard-shell {
  width: 100%;
  max-width: 1000px;
  padding: 28px;
  border-radius: 24px;
  background:
    linear-gradient(145deg, rgba(255, 255, 255, 0.08), rgba(255, 255, 255, 0.03));
  border: 1px solid rgba(255, 255, 255, 0.12);
  box-shadow:
    0 20px 60px rgba(0, 0, 0, 0.25),
    inset 0 1px 0 rgba(255, 255, 255, 0.06);
  backdrop-filter: blur(18px);
}

.dashboard-top {
  display: flex;
  justify-content: space-between;
  align-items: end;
  gap: 20px;
  margin-bottom: 24px;
  flex-wrap: wrap;
}


.city {
  margin: 0;
  font-size: 34px;
  font-weight: 700;
  line-height: 1.1;
}

.selector {
  min-width: 260px;
}

.hero-card {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
  padding: 24px;
  border-radius: 22px;
  margin-bottom: 24px;
  background:
    radial-gradient(circle at top left, rgba(88, 166, 255, 0.18), transparent 40%),
    linear-gradient(145deg, rgba(255, 255, 255, 0.10), rgba(255, 255, 255, 0.04));
  border: 1px solid rgba(255, 255, 255, 0.14);
}

.hero-left {
  display: flex;
  align-items: center;
  gap: 18px;
}

.hero-right {
  display: flex;
  flex-direction: column;
  gap: 12px;
  min-width: 180px;
}

.weather-icon {
  width: 90px;
  height: 90px;
}

.temperature {
  font-size: 56px;
  font-weight: 800;
  line-height: 1;
}

.condition {
  font-size: 20px;
  margin-top: 6px;
  opacity: 0.92;
}

.subline {
  margin-top: 8px;
  font-size: 14px;
  opacity: 0.7;
}

.mini-stat {
  display: flex;
  flex-direction: column;
  gap: 4px;
  padding: 12px 14px;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.06);
}

.mini-label {
  font-size: 12px;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  opacity: 0.65;
}

.mini-value {
  font-size: 14px;
  font-weight: 600;
}

.weather-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 16px;
}

.weather-card {
  padding: 18px;
  border-radius: 18px;
  text-align: left;
  background:
    linear-gradient(145deg, rgba(255, 255, 255, 0.08), rgba(255, 255, 255, 0.03));
  border: 1px solid rgba(255, 255, 255, 0.10);
  transition: transform 0.18s ease, border-color 0.18s ease;
}

.weather-card:hover {
  transform: translateY(-2px);
  border-color: rgba(255, 255, 255, 0.18);
}

.weather-card h3 {
  margin: 0 0 10px;
  font-size: 12px;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  opacity: 0.65;
}

.weather-card p {
  margin: 0;
  font-size: 20px;
  font-weight: 700;
}

.loading-state {
  padding: 30px 0;
  text-align: center;
  font-size: 16px;
  opacity: 0.75;
}

@media (max-width: 700px) {
  .dashboard-shell {
    padding: 20px;
  }

  .hero-card {
    flex-direction: column;
    align-items: flex-start;
  }

  .hero-right {
    width: 100%;
  }

  .temperature {
    font-size: 44px;
  }

  .city {
    font-size: 28px;
  }
}
</style>