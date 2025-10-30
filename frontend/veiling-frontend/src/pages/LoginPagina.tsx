import React, { useState } from 'react';
import {
  Container,
  Paper,
  Box,
  Typography,
  TextField,
  Button,
  FormControlLabel,
  Checkbox,
  Link,
  Alert,
  Divider,
  CircularProgress
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { loginGebruiker, bepaalGebruikerType } from '../services/gebruikerService';

const LoginPagina: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [formData, setFormData] = useState({
    email: '',
    wachtwoord: '',
    onthoudMij: false
  });
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      // Validatie
      if (!formData.email || !formData.wachtwoord) {
        setError('Vul alle velden in');
        setLoading(false);
        return;
      }

      // API call naar backend voor verificatie
      const response = await loginGebruiker(formData.email, formData.wachtwoord);

      // Bepaal userType op basis van response
      const userType = bepaalGebruikerType(response);

      // Login via AuthContext
      login(formData.email, userType);

      setSuccess('Login succesvol! Je wordt doorgestuurd...');

      // Redirect naar juiste pagina op basis van userType
      setTimeout(() => {
        if (userType === 'Koper') {
          navigate('/');
        } else {
          navigate('/mijn-producten');
        }
      }, 1500);
    } catch (err: any) {
      console.error('Login fout:', err);
      setError(err.message || 'Er is een fout opgetreden bij het inloggen');
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: '80vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          py: 4
        }}
      >
        <Paper elevation={6} sx={{ p: 4, width: '100%', maxWidth: 400 }}>
          <Box sx={{ textAlign: 'center', mb: 3 }}>
            <Typography variant="h4" component="h1" gutterBottom fontWeight="bold" color="primary">
              Welkom terug!
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Log in op je account
            </Typography>
          </Box>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          {success && (
            <Alert severity="success" sx={{ mb: 2 }}>
              {success}
            </Alert>
          )}

          <form onSubmit={handleSubmit}>
            <TextField
              fullWidth
              label="Email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleInputChange}
              required
              margin="normal"
              autoComplete="email"
              autoFocus
            />

            <TextField
              fullWidth
              label="Wachtwoord"
              name="wachtwoord"
              type="password"
              value={formData.wachtwoord}
              onChange={handleInputChange}
              required
              margin="normal"
              autoComplete="current-password"
            />

            <FormControlLabel
              control={
                <Checkbox
                  name="onthoudMij"
                  checked={formData.onthoudMij}
                  onChange={handleInputChange}
                  color="primary"
                />
              }
              label="Onthoud mij"
              sx={{ mt: 1, mb: 2 }}
            />

            <Button
              type="submit"
              variant="contained"
              color="primary"
              fullWidth
              size="large"
              sx={{ mt: 1, mb: 2, py: 1.5 }}
              disabled={loading}
            >
              {loading ? (
                <>
                  <CircularProgress size={24} sx={{ mr: 1 }} color="inherit" />
                  Inloggen...
                </>
              ) : (
                'Inloggen'
              )}
            </Button>

            <Divider sx={{ my: 2 }} />

            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="body2" color="text.secondary">
                Nog geen account?{' '}
                <Link
                  component="button"
                  variant="body2"
                  onClick={() => navigate('/registratie')}
                  sx={{
                    cursor: 'pointer',
                    fontWeight: 'bold',
                    textDecoration: 'none',
                    '&:hover': {
                      textDecoration: 'underline'
                    }
                  }}
                >
                  Registreer hier
                </Link>
              </Typography>
            </Box>
          </form>
        </Paper>
      </Box>
    </Container>
  );
};

export default LoginPagina;
