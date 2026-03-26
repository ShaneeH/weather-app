// types/weather.ts

export interface WeatherDetails {
  temperatureC: number
  condition: string
  windKph: number
  humidity: number
  cloud: number
  isDay: boolean
  icon: string
}

export interface TimezoneDetails {
  timezoneId: string
  localTime: string
  localtimeEpoch: number
}

export interface AstronomyDetails {
  sunrise: string
  sunset: string
  moonrise: string
  moonset: string
  moonPhase: string
}

export interface CityWeatherData {
  city: string
  weather: WeatherDetails
  timezone: TimezoneDetails
  astronomy: AstronomyDetails
}