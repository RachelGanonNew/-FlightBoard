import React from 'react';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { Provider } from 'react-redux';
import { store } from './store';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import FlightBoardPage from './pages/FlightBoardPage';

const theme = createTheme();
const queryClient = new QueryClient();

const App: React.FC = () => (
  <ThemeProvider theme={theme}>
    <CssBaseline />
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <FlightBoardPage />
      </QueryClientProvider>
    </Provider>
  </ThemeProvider>
);

export default App;
