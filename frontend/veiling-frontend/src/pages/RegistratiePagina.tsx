import React, { useState } from 'react';
import {
  Container,
  Paper,
  Box,
  Typography,
  TextField,
  Button,
  RadioGroup,
  FormControlLabel,
  Radio,
  FormControl,
  FormLabel,
  Link,
  Alert,
  CircularProgress
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { registreerGebruiker, bepaalGebruikerType, RegistratieRequest } from '../services/gebruikerService';

const RegistratiePagina: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [gebruikersType, setGebruikersType] = useState<'verkoper' | 'koper'>('verkoper');
  const [formData, setFormData] = useState({
    email: '',
    wachtwoord: '',
    bevestigWachtwoord: '',
    bedrijfsnaam: '',
    kvk: '',
    btwNummer: '',
    telefoon: '',
    adres: '',
    rekeningNummer: ''
  });
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      // Validatie
      if (formData.wachtwoord !== formData.bevestigWachtwoord) {
        setError('Wachtwoorden komen niet overeen');
        setLoading(false);
        return;
      }

      if (formData.wachtwoord.length < 6) {
        setError('Wachtwoord moet minimaal 6 karakters zijn');
        setLoading(false);
        return;
      }

      // Bouw de registratie request met Type indicator
      const registratieRequest: RegistratieRequest = {
        email: formData.email,
        wachtwoord: formData.wachtwoord,
        Type: gebruikersType === 'verkoper' ? 'Verkoper' : 'Koper', // Let op: hoofdletter!
        bedrijfsnaam: formData.bedrijfsnaam,
        kvk: formData.kvk,
        btw_nummer: formData.btwNummer,
        telefoon: formData.telefoon,
        rekening_nummer: formData.rekeningNummer,
        adres: formData.adres // Backend bepaalt of het bedrijfsadres of leveradres is
      };

      // API call naar backend
      const response = await registreerGebruiker(registratieRequest);

      setSuccess('Registratie succesvol! Je wordt ingelogd...');

      // Bepaal userType op basis van response
      const userType = bepaalGebruikerType(response);

      // Automatisch inloggen na registratie
      login(formData.email, userType);

      // Redirect naar juiste pagina
      setTimeout(() => {
        if (userType === 'Koper') {
          navigate('/');
        } else {
          navigate('/mijn-producten');
        }
      }, 1500);
    } catch (err: any) {
      console.error('Registratie fout:', err);
      setError(err.message || 'Er is een fout opgetreden bij het registreren');
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md">
      <Box
        sx={{
          minHeight: '80vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          py: 4
        }}
      >
        <Paper elevation={3} sx={{ p: 4, width: '100%' }}>
          <Typography variant="h4" component="h1" gutterBottom align="center" sx={{ mb: 3 }}>
            Registreer Account
          </Typography>

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
            {/* Basis Gegevens */}
            <Box sx={{ mb: 3 }}>
              <Typography variant="h6" gutterBottom>
                Algemene Gegevens
              </Typography>

              <TextField
                fullWidth
                label="Email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleInputChange}
                required
                margin="normal"
                helperText="Je email wordt gebruikt als gebruikersnaam"
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
                helperText="Minimaal 6 karakters"
              />

              <TextField
                fullWidth
                label="Bevestig Wachtwoord"
                name="bevestigWachtwoord"
                type="password"
                value={formData.bevestigWachtwoord}
                onChange={handleInputChange}
                required
                margin="normal"
              />
            </Box>

            {/* Gebruikerstype Selectie */}
            <Box sx={{ mb: 3 }}>
              <FormControl component="fieldset">
                <FormLabel component="legend">Ik ben een:</FormLabel>
                <RadioGroup
                  row
                  value={gebruikersType}
                  onChange={(e) => setGebruikersType(e.target.value as 'verkoper' | 'koper')}
                >
                  <FormControlLabel value="verkoper" control={<Radio />} label="Verkoper" />
                  <FormControlLabel value="koper" control={<Radio />} label="Koper" />
                </RadioGroup>
              </FormControl>
            </Box>

            {/* Bedrijfsgegevens */}
            <Box sx={{ mb: 3 }}>
              <Typography variant="h6" gutterBottom>
                Bedrijfsgegevens
              </Typography>

              <TextField
                fullWidth
                label="Bedrijfsnaam"
                name="bedrijfsnaam"
                value={formData.bedrijfsnaam}
                onChange={handleInputChange}
                required
                margin="normal"
              />

              <TextField
                fullWidth
                label="KVK Nummer"
                name="kvk"
                value={formData.kvk}
                onChange={handleInputChange}
                required
                margin="normal"
                helperText="8 cijfers"
              />

              <TextField
                fullWidth
                label="BTW Nummer"
                name="btwNummer"
                value={formData.btwNummer}
                onChange={handleInputChange}
                required
                margin="normal"
              />

              <TextField
                fullWidth
                label="Telefoonnummer"
                name="telefoon"
                value={formData.telefoon}
                onChange={handleInputChange}
                required
                margin="normal"
              />

              <TextField
                fullWidth
                label={gebruikersType === 'verkoper' ? 'Bedrijfsadres' : 'Leveradres'}
                name="adres"
                value={formData.adres}
                onChange={handleInputChange}
                required
                margin="normal"
                multiline
                rows={2}
              />

              <TextField
                fullWidth
                label="Rekeningnummer (IBAN)"
                name="rekeningNummer"
                value={formData.rekeningNummer}
                onChange={handleInputChange}
                required
                margin="normal"
                helperText="Bijvoorbeeld: NL01BANK0123456789"
              />
            </Box>

            {/* Submit Button */}
            <Button
              type="submit"
              variant="contained"
              color="primary"
              fullWidth
              size="large"
              sx={{ mt: 2, mb: 2 }}
              disabled={loading}
            >
              {loading ? (
                <>
                  <CircularProgress size={24} sx={{ mr: 1 }} color="inherit" />
                  Registreren...
                </>
              ) : (
                'Registreer'
              )}
            </Button>

            {/* Link naar Login */}
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="body2">
                Heb je al een account?{' '}
                <Link
                  component="button"
                  variant="body2"
                  onClick={() => navigate('/login')}
                  sx={{ cursor: 'pointer' }}
                >
                  Log in
                </Link>
              </Typography>
            </Box>
          </form>
        </Paper>
      </Box>
    </Container>
  );
};

export default RegistratiePagina;
