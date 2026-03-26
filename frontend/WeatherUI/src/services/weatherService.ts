import { httpClient } from "../api/httpClient"
import type { CityWeatherData } from "../types/weather"

export async function getCities(): Promise<string[]> {
  return httpClient.get<string[]>("/api/weather/cities")
}

export async function getWeatherByCity(city: string): Promise<CityWeatherData> {
  console.log("MAKING CALL TO THE WEATHER API");  
  return httpClient.get<CityWeatherData>(`/api/weather/${city}`)
}