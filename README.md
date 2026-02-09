# Music Showcase Generator

## Overview

Music Showcase Generator is a single-page web application that simulates a music store showcase by generating deterministic fake song information. The application provides realistic, localized data including song titles, artists, album names, genres, and reviews. Key features include:


**Deterministic Generation**: The same seed value always produces identical data across different devices and dates.



**Localized Content**: Supports multiple regions including English (US), French, and Italian, with data generated independently for each locale.



**Probabilistic Likes**: Allows specifying an average number of likes (0-10), implemented probabilistically for fractional values (e.g., 0.5 likes results in a 50% chance of 1 like).



**Dynamic Media**: Generates procedural album covers with titles and artists rendered on the image, and creates unique WAV audio tracks for every song.



**Dual View Modes**: Includes a Table View with record expansion for details and a Gallery View with infinite scrolling.



## Technology Stack

### Backend


**Framework**: ASP.NET Core (.NET 10.0) 



**Data Generation**: Bogus (for localized fake data) 



**Graphics**: SkiaSharp (for procedural cover art generation) 



**Audio**: Custom PCM WAV generation logic 



### Frontend


**Framework**: React 18 with TypeScript 



**Build Tool**: Vite 



**UI Library**: Ant Design (Antd) 



**Styling**: CSS Modules 



### Deployment

* **Containerization**: Docker
* **Web Server**: Nginx
* **Hosting**: Render

## Local Deployment via Docker

### 1. Backend Setup

Navigate to the `backend/Task5_MusicShowcase/Task5_MusicShowcase` directory.

Build the backend image:

```bash
docker build -t music-backend .

```

Run the backend container:

```bash
docker run -d -p 8080:8080 --name music-api music-backend

```

The API will be accessible at `http://localhost:8080`.

### 2. Frontend Setup

Navigate to the `frontend` directory.

Build the frontend image, passing the local backend URL as a build argument:

```bash
docker build --build-arg VITE_API_URL=http://localhost:8080 -t music-frontend .

```

Run the frontend container:

```bash
docker run -d -p 80:80 --name music-app music-frontend

```

The application will be accessible at `http://localhost`.
