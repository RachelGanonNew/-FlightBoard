import React, { useState } from 'react';
import { Box, Button, TextField, Grid } from '@mui/material';
import type { FlightCreate } from '../api/flightsApi';

interface Props {
  onAdd: (flight: FlightCreate) => void;
  loading?: boolean;
}

const initialForm: FlightCreate = {
  flightNumber: '',
  destination: '',
  departureTime: '',
  gate: '',
};

const AddFlightForm: React.FC<Props> = ({ onAdd, loading }) => {
  const [form, setForm] = useState(initialForm);
  const [errors, setErrors] = useState<{ [k: string]: string }>({});

  const validate = () => {
    const errs: { [k: string]: string } = {};
    if (!form.flightNumber) errs.flightNumber = 'Required';
    if (!form.destination) errs.destination = 'Required';
    if (!form.gate) errs.gate = 'Required';
    if (!form.departureTime) errs.departureTime = 'Required';
    else if (new Date(form.departureTime) <= new Date()) errs.departureTime = 'Must be in the future';
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      onAdd(form);
      setForm(initialForm);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mb: 2 }}>
      <Grid container spacing={2}>
        <Grid>
          <TextField
            label="Flight Number"
            name="flightNumber"
            value={form.flightNumber}
            onChange={handleChange}
            error={!!errors.flightNumber}
            helperText={errors.flightNumber}
            fullWidth
          />
        </Grid>
        <Grid>
          <TextField
            label="Destination"
            name="destination"
            value={form.destination}
            onChange={handleChange}
            error={!!errors.destination}
            helperText={errors.destination}
            fullWidth
          />
        </Grid>
        <Grid>
          <TextField
            label="Gate"
            name="gate"
            value={form.gate}
            onChange={handleChange}
            error={!!errors.gate}
            helperText={errors.gate}
            fullWidth
          />
        </Grid>
        <Grid>
          <TextField
            label="Departure Time"
            name="departureTime"
            type="datetime-local"
            value={form.departureTime}
            onChange={handleChange}
            error={!!errors.departureTime}
            helperText={errors.departureTime}
            fullWidth
            InputLabelProps={{ shrink: true }}
          />
        </Grid>
        <Grid>
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            Add Flight
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};

export default AddFlightForm; 