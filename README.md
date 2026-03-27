# WeatherApp

A small full stack web application that displays **weather, timezone, and astronomy information** for a selected city.

The application uses a **VueJS frontend** and a **.NET Core Web API backend** which proxies requests to the WeatherAPI service via RapidAPI.

This project was created as part of a technical assessment.

## Features

- Select a city from a list of three options
- View current weather information
- View timezone and local time
- View astronomy data such as sunrise, sunset, and moon phase
- Backend API aggregates data from multiple external endpoints

## Tech Stack

### Frontend
Vue

### Backend
C#  
.NET Core Web API

### External API
WeatherAPI via RapidAPI

## Running the App
Ensure the .NET API is running before starting the Vue frontend. 

.NET API runs at:

`https://localhost:7157`

Vue frontend runs at:

`https://localhost:5173`

### Run the Frontend :
- `git clone https://github.com/ShaneeH/weather-app.git`
- `cd weather-app`
- `npm install`
- `npm run dev`

### Run the Backend API : 
> Assuming the repo has already been cloned locally

 - Navigate to the WeatherAPI project folder
- `cd WeatherAPI`
- `dotnet restore`
- `dotnet run`

####  RapidAPI Key Configuration

The backend requires a RapidAPI key to access the WeatherAPI service.

For security reasons the API key is not stored in the repository and is instead configured using .NET User Secrets.

##### To configure the key locally run:

- `dotnet user-secrets set "WeatherApi:ApiKey" "YOUR_RAPIDAPI_KEY"`
- `dotnet user-secrets set "WeatherApi:Host" "weatherapi-com.p.rapidapi.com"`

These values are stored in the user secrets store and loaded by the application at runtime.



####  Notes
- The selected city is stored in localStorage so the choice persists across refreshes.
- The backend uses DTOs to simplify and structure responses returned to the frontend.
- External API calls are handled through a service layer to separate concerns.
- The backend aggregates responses from multiple WeatherAPI endpoints (current weather, timezone and astronomy) and returns a single structured response to the frontend.


