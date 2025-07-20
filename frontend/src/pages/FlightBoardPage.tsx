import React, { useEffect, useState } from 'react';
import { Container, Typography, Snackbar, Alert } from '@mui/material';
import FlightTable from '../components/FlightTable';
import AddFlightForm from '../components/AddFlightForm';
import FlightFilters from '../components/FlightFilters';
import { useFlights, useAddFlight, useDeleteFlight } from '../api/flightsApi';
import type { Flight, FlightCreate } from '../api/flightsApi';
import { createFlightHubConnection } from '../api/signalRClient';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { setStatus, setDestination, clearFilters } from '../features/filtersSlice';
import type { RootState } from '../store';

const FlightBoardPage: React.FC = () => {
  const dispatch = useAppDispatch();
  const { status, destination } = useAppSelector((state: RootState) => state.filters);
  const [snackbar, setSnackbar] = useState<{ open: boolean; message: string; severity: 'success' | 'error' }>({ open: false, message: '', severity: 'success' });

  const { data: flights = [], refetch } = useFlights();
  const addFlight = useAddFlight();
  const deleteFlight = useDeleteFlight();

  useEffect(() => {
    const connection = createFlightHubConnection();
    connection.start().then(() => {
      connection.on('FlightAdded', () => refetch());
      connection.on('FlightDeleted', () => refetch());
    });
    return () => { connection.stop(); };
  }, [refetch]);

  const handleAdd = async (flight: FlightCreate) => {
    try {
      await addFlight.mutateAsync(flight);
      setSnackbar({ open: true, message: 'Flight added!', severity: 'success' });
    } catch (e: any) {
      setSnackbar({ open: true, message: e?.response?.data?.[0] || 'Error adding flight', severity: 'error' });
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteFlight.mutateAsync(id);
      setSnackbar({ open: true, message: 'Flight deleted!', severity: 'success' });
    } catch {
      setSnackbar({ open: true, message: 'Error deleting flight', severity: 'error' });
    }
  };

  const handleSearch = () => {
    // Filtering is handled client-side for demo; for large datasets, use useSearchFlights
    // refetch();
  };

  const filteredFlights = flights.filter(f =>
    (!status || f.status === status) &&
    (!destination || f.destination.toLowerCase().includes(destination.toLowerCase()))
  );

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>Flight Board</Typography>
      <AddFlightForm onAdd={handleAdd} loading={addFlight.isPending} />
      <FlightFilters
        status={status}
        destination={destination}
        onStatusChange={s => dispatch(setStatus(s))}
        onDestinationChange={d => dispatch(setDestination(d))}
        onSearch={handleSearch}
        onClear={() => dispatch(clearFilters())}
      />
      <FlightTable flights={filteredFlights} onDelete={handleDelete} />
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity} onClose={() => setSnackbar({ ...snackbar, open: false })}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Container>
  );
};

export default FlightBoardPage; 