const SELECTED_CITY_KEY = "selectedCity"

export function getSelectedCity(): string {
  return localStorage.getItem(SELECTED_CITY_KEY) || "Dublin"
}

export function setSelectedCity(city: string): void {
  localStorage.setItem(SELECTED_CITY_KEY, city)
}