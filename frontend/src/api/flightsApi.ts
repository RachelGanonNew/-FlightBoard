import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import axios from 'axios';

export interface Flight {
  id: string;
  flightNumber: string;
  destination: string;
  departureTime: string;
  gate: string;
  status: string;
}

export interface FlightCreate {
  flightNumber: string;
  destination: string;
  departureTime: string;
  gate: string;
}

const API_URL = 'http://localhost:5000/api/flights'; // Adjust if needed

export function useFlights() {
  return useQuery<Flight[]>({
    queryKey: ['flights'],
    queryFn: async () => (await axios.get(API_URL)).data,
  });
}

export function useAddFlight() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (flight: FlightCreate) => (await axios.post(API_URL, flight)).data,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['flights'] }),
  });
}

export function useDeleteFlight() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => (await axios.delete(`${API_URL}/${id}`)).data,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['flights'] }),
  });
}

export function useSearchFlights(filters: { status?: string; destination?: string }) {
  return useQuery<Flight[]>({
    queryKey: ['flights', filters],
    queryFn: async () => {
      const params = new URLSearchParams();
      if (filters.status) params.append('status', filters.status);
      if (filters.destination) params.append('destination', filters.destination);
      return (await axios.get(`${API_URL}/search?${params.toString()}`)).data;
    },
    enabled: !!(filters.status || filters.destination),
  });
} 