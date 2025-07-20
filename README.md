# FlightBoard

A full-stack, real-time flight board management system.

## Project Structure

- **FlightBoard.Backend/**: ASP.NET Core Web API backend with Entity Framework Core (SQLite), SignalR for real-time updates, and Clean Architecture.
- **frontend/**: React + TypeScript frontend using Material UI, Redux Toolkit, React Query, and SignalR client.

---

## Backend (FlightBoard.Backend)

### Features
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- SignalR for real-time updates
- Clean Architecture (Domain, Application, Infrastructure, API)

### Setup & Run
1. **Install .NET 9 SDK**
2. Navigate to the backend directory:
   ```sh
   cd FlightBoard.Backend/FlightBoard.API
   ```
3. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
4. **Apply migrations (if needed):**
   ```sh
   dotnet ef database update
   ```
5. **Run the API:**
   ```sh
   dotnet run
   ```
6. The API will be available at `http://localhost:5000` (or as configured).

---

## Frontend (frontend)

### Features
- React + TypeScript
- Material UI
- Redux Toolkit (UI state)
- React Query (server state)
- SignalR client for real-time updates

### Setup & Run
1. **Install Node.js (v18+) and npm**
2. Navigate to the frontend directory:
   ```sh
   cd frontend
   ```
3. **Install dependencies:**
   ```sh
   npm install
   ```
4. **Run the development server:**
   ```sh
   npm run dev
   ```
5. The app will be available at `http://localhost:5173` (or as configured).

---

## Quick Start: Example User Flow

Once both the backend and frontend are running:

1. **Open the frontend app** in your browser at [http://localhost:5173](http://localhost:5173).
2. **Add a new flight** using the "Add Flight" form:
   - Enter a flight number (e.g., `AB123`)
   - Enter a destination (e.g., `London`)
   - Select a departure time (future date/time)
   - Enter a gate (e.g., `A1`)
   - Click "Add Flight"
3. **See the new flight appear** instantly in the flight board table.
4. **Filter flights** using the status and destination filters at the top.
5. **Delete a flight** by clicking the trash/delete icon in the Actions column.
6. **Observe real-time updates:**
   - If you open the app in another browser window or device, adding or deleting flights in one window will update all others in real time (via SignalR).

---

## Project Overview

- **Real-time updates:** Flights are updated live via SignalR.
- **Add, filter, and delete flights** from the UI.
- **Clean, maintainable architecture** for both backend and frontend.

---

## License

MIT (or your preferred license) 