import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CssBaseline, Container } from '@mui/material';
import { AuthProvider } from './context/AuthContext';
import NavigatieBar from './components/NavigatieBar';
import ProtectedRoute from './components/ProtectedRoute';
import RegistratiePagina from './pages/RegistratiePagina';
import LoginPagina from './pages/LoginPagina';
import VeilingOverzichtPagina from './pages/VeilingOverzichtPagina';
import MijnProductenPagina from './pages/MijnProductenPagina';
import ProfielPagina from './pages/ProfielPagina';

function App() {
  return (
    <AuthProvider>
      <Router>
        <CssBaseline />
        <NavigatieBar />
        <Container maxWidth="lg" sx={{ mt: 4 }}>
          <Routes>
            {/* Public routes */}
            <Route path="/login" element={<LoginPagina />} />
            <Route path="/registratie" element={<RegistratiePagina />} />

            {/* Protected route: alleen voor Kopers */}
            <Route
              path="/"
              element={
                <ProtectedRoute allowedUserTypes={['Koper']}>
                  <VeilingOverzichtPagina />
                </ProtectedRoute>
              }
            />

            {/* Protected route: alleen voor Verkopers */}
            <Route
              path="/mijn-producten"
              element={
                <ProtectedRoute allowedUserTypes={['Verkoper']}>
                  <MijnProductenPagina />
                </ProtectedRoute>
              }
            />

            {/* Protected route: profiel pagina (voor beide gebruikerstypes) */}
            <Route
              path="/profiel"
              element={
                <ProtectedRoute allowedUserTypes={['Koper', 'Verkoper']}>
                  <ProfielPagina />
                </ProtectedRoute>
              }
            />
          </Routes>
        </Container>
      </Router>
    </AuthProvider>
  );
}

export default App;
