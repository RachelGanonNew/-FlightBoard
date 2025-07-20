import React from 'react';
import { Box, Button, Grid, MenuItem, TextField } from '@mui/material';

interface Props {
  status: string;
  destination: string;
  onStatusChange: (status: string) => void;
  onDestinationChange: (destination: string) => void;
  onSearch: () => void;
  onClear: () => void;
}

const statuses = ['', 'Scheduled', 'Boarding', 'Departed', 'Landed'];

const FlightFilters: React.FC<Props> = ({ status, destination, onStatusChange, onDestinationChange, onSearch, onClear }) => (
  <Box sx={{ mb: 2 }}>
    <Grid container spacing={2} alignItems="center">
      <Grid>
        <TextField
          select
          label="Status"
          value={status}
          onChange={e => onStatusChange(e.target.value)}
          fullWidth
        >
          {statuses.map(s => (
            <MenuItem key={s} value={s}>{s || 'All'}</MenuItem>
          ))}
        </TextField>
      </Grid>
      <Grid>
        <TextField
          label="Destination"
          value={destination}
          onChange={e => onDestinationChange(e.target.value)}
          fullWidth
        />
      </Grid>
      <Grid>
        <Button variant="contained" color="primary" onClick={onSearch} sx={{ mr: 1 }}>
          Search
        </Button>
        <Button variant="outlined" onClick={onClear}>
          Clear Filters
        </Button>
      </Grid>
    </Grid>
  </Box>
);

export default FlightFilters; 