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
VueJS

### Backend
C#  
.NET Core Web API

### External API
WeatherAPI via RapidAPI

### Running the App
Make sure the .NET API is running before starting the Vue frontend. 
This App is configured that the C# API Is running on Port `https://localhost:7157`.
And the Vue FrontEnd is running at `https://localhost:5173`

