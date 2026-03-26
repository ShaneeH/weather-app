// Get Cities Service

import { httpClient } from "../api/httpClient"

export async function getCities(): Promise<string[]> {
  return httpClient.get<string[]>("https://localhost:7157/api/weather/cities")
}