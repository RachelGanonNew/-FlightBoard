import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

interface FiltersState {
  status: string;
  destination: string;
}

const initialState: FiltersState = {
  status: '',
  destination: '',
};

const filtersSlice = createSlice({
  name: 'filters',
  initialState,
  reducers: {
    setStatus(state, action: PayloadAction<string>) {
      state.status = action.payload;
    },
    setDestination(state, action: PayloadAction<string>) {
      state.destination = action.payload;
    },
    clearFilters(state) {
      state.status = '';
      state.destination = '';
    },
  },
});

export const { setStatus, setDestination, clearFilters } = filtersSlice.actions;
export default filtersSlice.reducer; 