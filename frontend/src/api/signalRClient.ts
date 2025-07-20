import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';

const SIGNALR_URL = 'http://localhost:5000/flightHub'; // Adjust if needed

export function createFlightHubConnection(): HubConnection {
  return new HubConnectionBuilder()
    .withUrl(SIGNALR_URL)
    .withAutomaticReconnect()
    .build();
} 