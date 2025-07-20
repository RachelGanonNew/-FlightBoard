import React from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, IconButton } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import type { Flight } from '../api/flightsApi';
import StatusChip from './StatusChip';

interface Props {
  flights: Flight[];
  onDelete: (id: string) => void;
}

const FlightTable: React.FC<Props> = ({ flights, onDelete }) => (
  <TableContainer component={Paper}>
    <Table>
      <TableHead>
        <TableRow>
          <TableCell>Flight Number</TableCell>
          <TableCell>Destination</TableCell>
          <TableCell>Departure Time</TableCell>
          <TableCell>Gate</TableCell>
          <TableCell>Status</TableCell>
          <TableCell>Actions</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {flights.map(flight => (
          <TableRow key={flight.id}>
            <TableCell>{flight.flightNumber}</TableCell>
            <TableCell>{flight.destination}</TableCell>
            <TableCell>{new Date(flight.departureTime).toLocaleString()}</TableCell>
            <TableCell>{flight.gate}</TableCell>
            <TableCell><StatusChip status={flight.status} /></TableCell>
            <TableCell>
              <IconButton color="error" aria-label="delete" onClick={() => onDelete(flight.id)}>
                <DeleteIcon />
              </IconButton>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </TableContainer>
);

export default FlightTable; 