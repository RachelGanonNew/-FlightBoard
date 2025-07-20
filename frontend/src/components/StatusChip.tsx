import React from 'react';
import { Chip } from '@mui/material';

const statusColor: Record<string, 'default' | 'primary' | 'success' | 'warning' | 'error'> = {
  Scheduled: 'default',
  Boarding: 'primary',
  Departed: 'warning',
  Landed: 'success',
};

const StatusChip: React.FC<{ status: string }> = ({ status }) => (
  <Chip label={status} color={statusColor[status] || 'default'} variant="outlined" />
);

export default StatusChip; 