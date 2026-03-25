import axios from "axios"
import type { AxiosInstance, AxiosRequestConfig } from "axios"

class HttpClient {
  private client: AxiosInstance

  constructor() {
    this.client = axios.create({
      baseURL: import.meta.env.VITE_API_BASE_URL,
      timeout: 5000,
      headers: {
        "Content-Type": "application/json"
      }
    })
  }

  async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.get<T>(url, config)
    return response.data
  }
}

export const httpClient = new HttpClient()