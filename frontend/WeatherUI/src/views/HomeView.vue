<script setup lang="ts">
import CitySelector from '../components/CitySelector.vue'
import { ref, onMounted, watch } from 'vue'
import { getCities, getWeatherByCity } from '../services/weatherService'
import { getSelectedCity, setSelectedCity } from "../services/localStorageService"
import type { CityWeatherData } from '../types/weather'
import { formatTime } from '../utils/time'
import { Sun, Moon, CircleAlert } from 'lucide-vue-next'

const cities = ref<string[]>([])
const selectedCity = ref("")
const weatherData = ref<CityWeatherData | null>(null)
const isLoading = ref(false)
const error = ref<string | null>(null)

async function loadWeather(city: string) {
  if (!city) return

  try {
    isLoading.value = true
    error.value = null
    weatherData.value = await getWeatherByCity(city)
  } catch (err) {
    console.error("Failed to fetch weather:", err)
    error.value = "Failed to load weather data. Please try again."
    weatherData.value = null
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
  } catch (err) {
    console.error("Failed to load cities:", err)
    error.value = "Failed to load city list. Please refresh the page."
  }
})

watch(selectedCity, async (newCity) => {
  if (!newCity) return
  setSelectedCity(newCity)
  await loadWeather(newCity)
})
</script>

<template>
  <main class="station">
    <div class="scanlines" aria-hidden="true"></div>

    <div class="station-inner">
      <header class="station-header">
        <CitySelector v-model="selectedCity" :cities="cities" />
      </header>

      <div v-if="error" class="error-state" role="alert">
        <CircleAlert class="error-icon" />
        <span>{{ error }}</span>
      </div>

      <div v-else-if="isLoading" class="loading-frame">
        <span class="loading-bar"></span>
        <span class="loading-text">Loading Data...</span>
      </div>

      <template v-else-if="weatherData">
        <section class="primary-readout">
          <div class="readout-left">
            <p class="readout-location">
              {{ weatherData.city }}
            </p>

            <div class="readout-temp">
              {{ weatherData.weather.temperatureC }}<span class="readout-unit">°C</span>
            </div>

            <div class="readout-condition">
              <img :src="'https:' + weatherData.weather.icon" :alt="weatherData.weather.condition"
                class="condition-icon" />
              <span>{{ weatherData.weather.condition }}</span>
            </div>
          </div>

          <div class="readout-right">
            <div class="pill-stat">
              <span class="pill-label">LOCAL TIME</span>
              <span class="pill-value">{{ formatTime(weatherData.timezone.localTime) }}</span>
            </div>

            <div class="pill-stat">
              <span class="pill-label">DAYTIME</span>
              <span class="pill-value" :class="weatherData.weather.isDay ? 'accent-yes' : 'accent-no'">
                {{ weatherData.weather.isDay ? 'YES' : 'NO' }}
              </span>
            </div>
          </div>
        </section>

        <div class="rule">
          <span class="rule-label">ATMOSPHERIC DATA</span>
        </div>

        <section class="metric-grid">
          <div class="metric-card">
            <span class="metric-label">WIND</span>
            <span class="metric-value">{{ weatherData.weather.windKph }}</span>
            <span class="metric-unit">kph</span>
          </div>

          <div class="metric-card">
            <span class="metric-label">HUMIDITY</span>
            <span class="metric-value">{{ weatherData.weather.humidity }}</span>
            <span class="metric-unit">%</span>
          </div>

          <div class="metric-card">
            <span class="metric-label">CLOUD COVER</span>
            <span class="metric-value">{{ weatherData.weather.cloud }}</span>
            <span class="metric-unit">%</span>
            <div class="metric-bar-track">
              <div class="metric-bar-fill" :style="{ width: weatherData.weather.cloud + '%' }"></div>
            </div>
          </div>

          <div class="metric-card wide">
            <span class="metric-label">MOON PHASE</span>
            <span class="metric-value mono">{{ weatherData.astronomy.moonPhase }}</span>
          </div>
        </section>

        <div class="rule">
          <span class="rule-label">SOLAR & LUNAR EVENTS</span>
        </div>

        <section class="astro-row">
          <div class="astro-block">
            <div class="astro-header">
              <Sun class="astro-sun" />
              <span>SUN</span>
            </div>

            <div class="astro-times">
              <div class="astro-time-row">
                <span class="astro-event">RISE</span>
                <span class="astro-time">{{ weatherData.astronomy.sunrise }}</span>
              </div>
              <div class="astro-time-row">
                <span class="astro-event">SET</span>
                <span class="astro-time">{{ weatherData.astronomy.sunset }}</span>
              </div>
            </div>
          </div>

          <div class="astro-divider" aria-hidden="true"></div>

          <div class="astro-block">
            <div class="astro-header">
              <Moon class="astro-moon" />
              <span>MOON</span>
            </div>

            <div class="astro-times">
              <div class="astro-time-row">
                <span class="astro-event">RISE</span>
                <span class="astro-time">{{ weatherData.astronomy.moonrise }}</span>
              </div>
              <div class="astro-time-row">
                <span class="astro-event">SET</span>
                <span class="astro-time">{{ weatherData.astronomy.moonset }}</span>
              </div>
            </div>
          </div>
        </section>
      </template>

      <div v-else class="empty-state">
        <span>SELECT A CITY TO BEGIN</span>
      </div>
    </div>
  </main>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;700;800&family=JetBrains+Mono:wght@400;600&display=swap');

.station {
  position: relative;
  min-height: 100vh;
  background: var(--navy);
  color: var(--text);
  font-family: var(--font-display);
  overflow: hidden;
}

.scanlines {
  pointer-events: none;
  position: fixed;
  inset: 0;
  z-index: 9;
  background: repeating-linear-gradient(0deg,
      transparent,
      transparent 3px,
      rgba(0, 0, 0, 0.07) 3px,
      rgba(0, 0, 0, 0.07) 4px);
}

.station-inner {
  position: relative;
  z-index: 1;
  max-width: 900px;
  margin: 0 auto;
  padding: 36px 28px 64px;
}

.station-header {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 20px;
  flex-wrap: wrap;
  margin-bottom: 40px;
  padding-bottom: 24px;
  border-bottom: 1px solid var(--border);
}

.error-state {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 16px 20px;
  border-radius: 12px;
  background: rgba(240, 82, 82, 0.08);
  border: 1px solid rgba(240, 82, 82, 0.3);
  font-family: var(--font-mono);
  font-size: 13px;
  letter-spacing: 0.05em;
  color: #f05252;
  margin-bottom: 24px;
}

.error-icon {
  width: 18px;
  height: 18px;
  flex-shrink: 0;
}

.primary-readout {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 24px;
  flex-wrap: wrap;
  margin-bottom: 40px;
}

.readout-location {
  margin: 0 0 10px;
  font-family: var(--font-mono);
  font-size: 24px;
  font-weight: bold;
  letter-spacing: 0.1em;
  color: var(--muted);
  text-transform: uppercase;
}

.readout-temp {
  font-size: clamp(64px, 12vw, 108px);
  font-weight: 800;
  line-height: 0.9;
  color: var(--text);
  letter-spacing: -0.03em;
}

.readout-unit {
  font-size: 0.35em;
  font-weight: 400;
  vertical-align: super;
  color: var(--amber);
}

.readout-condition {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 16px;
  font-size: 18px;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.condition-icon {
  width: 50px;
  height: 50px;
}

.readout-right {
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding-top: 4px;
  min-width: 200px;
}

.pill-stat {
  background: var(--panel);
  border: 1px solid var(--border);
  border-radius: 10px;
  padding: 14px 18px;
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.pill-label {
  font-family: var(--font-mono);
  font-size: 14px;
  letter-spacing: 0.15em;
  color: var(--muted);
}

.pill-value {
  font-family: var(--font-mono);
  font-size: 18px;
  font-weight: 700;
  color: var(--text);
}

.accent-yes {
  color: var(--green);
}

.accent-no {
  color: var(--muted);
}

.rule {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-bottom: 20px;
}

.rule::before,
.rule::after {
  content: '';
  flex: 1;
  height: 1px;
  background: var(--border);
}

.rule-label {
  font-family: var(--font-mono);
  font-size: 14px;
  letter-spacing: 0.2em;
  color: #807a5a;
  white-space: nowrap;
}

.metric-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: 12px;
  margin-bottom: 36px;
}

.metric-card {
  background: var(--panel);
  border: 1px solid var(--border);
  border-radius: 14px;
  padding: 18px 20px;
  display: flex;
  flex-direction: column;
  gap: 4px;
  transition: border-color 0.2s, transform 0.2s;
}

.metric-card:hover {
  border-color: rgba(240, 165, 0, 0.35);
  transform: translateY(-2px);
}

.metric-card.wide {
  grid-column: span 2;
}

.metric-label {
  font-family: var(--font-mono);
  font-size: 14px;
  letter-spacing: 0.18em;
  color: var(--muted);
}

.metric-value {
  font-size: 32px;
  font-weight: 800;
  color: var(--amber2);
  line-height: 1.1;
  letter-spacing: -0.02em;
}

.metric-value.mono {
  font-family: var(--font-mono);
  font-size: 20px;
  font-weight: 600;
  letter-spacing: 0;
}

.metric-unit {
  font-family: var(--font-mono);
  font-size: 18px;
  color: var(--muted);
  margin-top: -2px;
}

.metric-bar-track {
  margin-top: 10px;
  height: 4px;
  background: var(--border);
  border-radius: 2px;
  overflow: hidden;
}

.metric-bar-fill {
  height: 100%;
  background: linear-gradient(90deg, var(--amber), var(--amber2));
  border-radius: 2px;
  transition: width 0.6s cubic-bezier(0.4, 0, 0.2, 1);
}

.astro-row {
  display: flex;
  gap: 0;
  background: var(--panel);
  border: 1px solid var(--border);
  border-radius: 16px;
  overflow: hidden;
}

.astro-block {
  flex: 1;
  padding: 22px 24px;
}

.astro-divider {
  width: 2px;
  background: var(--border);
  flex-shrink: 0;
}

.astro-header {
  display: flex;
  align-items: center;
  gap: 8px;
  font-family: var(--font-mono);
  font-size: 16px;
  letter-spacing: 0.2em;
  color: var(--muted);
  margin-bottom: 16px;
}

.astro-sun {
  width: 24px;
  height: 24px;
  color: rgb(185, 176, 76);
}

.astro-moon {
  width: 24px;
  height: 24px;
  color: rgb(97, 97, 216);
}

.astro-times {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.astro-time-row {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  gap: 12px;
}

.astro-event {
  font-family: var(--font-mono);
  font-size: 14px;
  letter-spacing: 0.12em;
  color: var(--muted);
}

.astro-time {
  font-family: var(--font-mono);
  font-size: 15px;
  font-weight: 600;
  color: var(--text);
}

.loading-frame {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
  padding: 80px 0;
}

.loading-bar {
  display: block;
  width: 160px;
  height: 2px;
  background: var(--border);
  border-radius: 2px;
  overflow: hidden;
  position: relative;
}

.loading-bar::after {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(90deg, transparent, var(--amber), transparent);
  animation: sweep 1.4s ease-in-out infinite;
}

@keyframes sweep {
  0% {
    transform: translateX(-100%);
  }

  100% {
    transform: translateX(200%);
  }
}

.loading-text {
  font-family: var(--font-mono);
  font-size: 11px;
  letter-spacing: 0.2em;
  color: var(--muted);
}

.empty-state {
  padding: 80px 0;
  text-align: center;
  font-family: var(--font-mono);
  font-size: 12px;
  letter-spacing: 0.2em;
  color: var(--muted);
}

@media (max-width: 640px) {
  .station-inner {
    padding: 24px 18px 48px;
  }

  .readout-right {
    min-width: unset;
    width: 100%;
  }

  .metric-card.wide {
    grid-column: span 1;
  }

  .astro-row {
    flex-direction: column;
  }

  .astro-divider {
    width: auto;
    height: 1px;
  }
}
</style>