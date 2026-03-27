<script setup lang="ts">
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
  <main class="station">
    <!-- Scan-line overlay -->
    <div class="scanlines" aria-hidden="true"></div>

    <div class="station-inner">

     
      <header class="station-header">
  

        <div class="header-right">
          <CitySelector v-model="selectedCity" :cities="cities" />
        </div>
      </header>

      <!-- ── Loading ────────────────────────────────── -->
      <div v-if="isLoading" class="loading-frame">
        <span class="loading-bar"></span>
        <span class="loading-text">Loading Data...</span>
      </div>

      <!-- ── Main content ───────────────────────────── -->
      <template v-else-if="weatherData">

        <!-- Primary readout -->
        <section class="primary-readout">
          <div class="readout-left">
            <p class="readout-location">
              {{ weatherData.city }}
    
            </p>
            <div class="readout-temp">
              {{ weatherData.weather.temperatureC }}<span class="readout-unit">°C</span>
            </div>
            <div class="readout-condition">
              <img
                :src="'https:' + weatherData.weather.icon"
                class="condition-icon"
                alt=""
              />
              <span>{{ weatherData.weather.condition }}</span>
            </div>
          </div>

          <div class="readout-right">
            <div class="pill-stat">
              <span class="pill-label">LOCAL TIME</span>
              <span class="pill-value">{{ weatherData.timezone.localTime }}</span>
            </div>
            <div class="pill-stat">
              <span class="pill-label">DAYTIME</span>
              <span class="pill-value" :class="weatherData.weather.isDay ? 'accent-yes' : 'accent-no'">
                {{ weatherData.weather.isDay ? 'YES' : 'NO' }}
              </span>
            </div>
          </div>
        </section>

        <!-- Divider rule -->
        <div class="rule">
          <span class="rule-label">ATMOSPHERIC DATA</span>
        </div>

        <!-- Metric grid -->
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

            <!-- Visual bar -->
            <div class="metric-bar-track">
              <div
                class="metric-bar-fill"
                :style="{ width: weatherData.weather.cloud + '%' }"
              ></div>
            </div>
          </div>

          <div class="metric-card wide">
            <span class="metric-label">MOON PHASE</span>
            <span class="metric-value mono">{{ weatherData.astronomy.moonPhase }}</span>
          </div>
        </section>

        <!-- Divider rule -->
        <div class="rule">
          <span class="rule-label">SOLAR & LUNAR EVENTS</span>
        </div>

        <!-- Astronomy row -->
        <section class="astro-row">
          <div class="astro-block">
            <div class="astro-header">
              <svg class="astro-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                <circle cx="12" cy="12" r="4"/><line x1="12" y1="2" x2="12" y2="4"/>
                <line x1="12" y1="20" x2="12" y2="22"/><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/>
                <line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/><line x1="2" y1="12" x2="4" y2="12"/>
                <line x1="20" y1="12" x2="22" y2="12"/><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/>
                <line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
              </svg>
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
              <svg class="astro-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
              </svg>
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

/* Scanlines */
.scanlines {
  pointer-events: none;
  position: fixed;
  inset: 0;
  z-index: 9;
  background: repeating-linear-gradient(
    0deg,
    transparent,
    transparent 3px,
    rgba(0,0,0,0.07) 3px,
    rgba(0,0,0,0.07) 4px
  );
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
  justify-content: space-between;
  gap: 20px;
  flex-wrap: wrap;
  margin-bottom: 40px;
  padding-bottom: 24px;
  border-bottom: 1px solid var(--border);
}

.brand {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 13px;
  font-weight: 700;
  letter-spacing: 0.25em;
  text-transform: uppercase;
  color: var(--text);
}
.brand em {
  font-style: normal;
  color: var(--amber);
}
.brand-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--amber);
  box-shadow: 0 0 8px var(--amber);
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
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
.readout-tz {
  color: var(--amber);
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
  width: 40px;
  height: 40px;
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
  font-size: 10px;
  letter-spacing: 0.15em;
  color: var(--muted);
}
.pill-value {
  font-family: var(--font-mono);
  font-size: 15px;
  font-weight: 600;
  color: var(--text);
}
.accent-yes { color: var(--green); }
.accent-no  { color: var(--muted); }


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

/* ── Metric grid ──────────────────────────────────── */
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
  border-color: rgba(240,165,0,0.35);
  transform: translateY(-2px);
}
.metric-card.wide {
  grid-column: span 2;
}

.metric-label {
  font-family: var(--font-mono);
  font-size: 10px;
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
  font-size: 18px;
  font-weight: 600;
  letter-spacing: 0;
}
.metric-unit {
  font-family: var(--font-mono);
  font-size: 11px;
  color: var(--muted);
  margin-top: -2px;
}

/* Cloud bar */
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

/* ── Astro row ────────────────────────────────────── */
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
  width: 1px;
  background: var(--border);
  flex-shrink: 0;
}

.astro-header {
  display: flex;
  align-items: center;
  gap: 8px;
  font-family: var(--font-mono);
  font-size: 11px;
  letter-spacing: 0.2em;
  color: var(--muted);
  margin-bottom: 16px;
}
.astro-icon {
  width: 16px;
  height: 16px;
  color: var(--amber);
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
  font-size: 10px;
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
  0%   { transform: translateX(-100%); }
  100% { transform: translateX(200%); }
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

</style>