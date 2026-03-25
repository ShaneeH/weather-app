// types/weather.ts

export interface CityWeatherData {
  city: string;

  // Weather

  
  temperatureC: number;
  condition: string;
  windKph: number;
  humidity: number;
  cloud: number;
  isDay: boolean;
  icon: string;

  // Timezone
  timezoneId: string;
  localTime: string;
  localtimeEpoch: number;

  // Astronomy
  sunrise: string;
  sunset: string;
  moonrise: string;
  moonset: string;
  moonPhase: string;
}