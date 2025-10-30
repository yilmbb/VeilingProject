import React, { useState, useEffect } from 'react';
import {
  Container,
  Paper,
  Box,
  Typography,
  TextField,
  Button,
  Alert,
  CircularProgress,
  Chip,
  Divider,
  Stack
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getGebruikerByEmail, updateGebruiker, LoginResponse, UpdateGebruikerRequest } from '../services/gebruikerService';
import PersonIcon from '@mui/icons-material/Person';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';

const ProfielPagina: React.FC = () => {
  const navigate = useNavigate();
  const { email, userType, logout } = useAuth();
  const [loading, setLoading] = useState<boolean>(true);
  const [saving, setSaving] = useState<boolean>(false);
  const [editMode, setEditMode] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');
  const [gebruiker, setGebruiker] = useState<LoginResponse | null>(null);
  const [formData, setFormData] = useState({
    bedrijfsnaam: '',
    kvk: '',
    btw_nummer: '',
    telefoon: '',
    adres: '',
    rekening_nummer: ''
  });

  // Laad gebruikersgegevens bij mount
  useEffect(() => {
    const loadGebruiker = async () => {
      if (!email) {
        navigate('/login');
        return;
      }

      try {
        setLoading(true);
        const data = await getGebruikerByEmail(email);
        setGebruiker(data);

        // Vul formData met huidige gegevens
        setFormData({
          bedrijfsnaam: data.bedrijfsnaam || '',
          kvk: data.kvk || '',
          btw_nummer: data.btw_nummer || '',
          telefoon: data.telefoon || '',
          adres: data.bedrijfsadres || data.leveradres || '',
          rekening_nummer: data.rekening_nummer || ''
        });
      } catch (err: any) {
        console.error('Fout bij laden profiel:', err);
        setError(err.message || 'Kan profiel niet laden');
      } finally {
        setLoading(false);
      }
    };

    loadGebruiker();
  }, [email, navigate]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleEdit = () => {
    setEditMode(true);
    setError('');
    setSuccess('');
  };

  const handleCancel = () => {
    setEditMode(false);
    setError('');
    setSuccess('');

    // Reset formData naar originele waarden
    if (gebruiker) {
      setFormData({
        bedrijfsnaam: gebruiker.bedrijfsnaam || '',
        kvk: gebruiker.kvk || '',
        btw_nummer: gebruiker.btw_nummer || '',
        telefoon: gebruiker.telefoon || '',
        adres: gebruiker.bedrijfsadres || gebruiker.leveradres || '',
        rekening_nummer: gebruiker.rekening_nummer || ''
      });
    }
  };

  const handleSave = async () => {
    if (!gebruiker) return;

    setError('');
    setSuccess('');
    setSaving(true);

    try {
      // Bouw update object op basis van gebruikerstype
      const updateData: UpdateGebruikerRequest = {
        gebruiker_id: gebruiker.gebruiker_id,
        email: gebruiker.email,
        wachtwoord: gebruiker.wachtwoord,
        bedrijfsnaam: formData.bedrijfsnaam,
        kvk: formData.kvk,
        btw_nummer: formData.btw_nummer,
        telefoon: formData.telefoon,
        rekening_nummer: formData.rekening_nummer
      };

      // Voeg het juiste adres veld toe
      if (userType === 'Verkoper') {
        updateData.bedrijfsadres = formData.adres;
      } else {
        updateData.leveradres = formData.adres;
      }

      // API call om gebruiker bij te werken
      const updatedGebruiker = await updateGebruiker(gebruiker.gebruiker_id, updateData);

      setSuccess('Gegevens succesvol bijgewerkt!');
      setEditMode(false);

      // Update de gebruiker state met de response
      setGebruiker(updatedGebruiker);
    } catch (err: any) {
      console.error('Fout bij updaten profiel:', err);
      setError(err.message || 'Kan profiel niet bijwerken');
    } finally {
      setSaving(false);
    }
  };

  const handleGoBack = () => {
    if (userType === 'Verkoper') {
      navigate('/mijn-producten');
    } else {
      navigate('/');
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ mt: 4, textAlign: 'center' }}>
        <CircularProgress />
        <Typography sx={{ mt: 2 }}>Profiel laden...</Typography>
      </Container>
    );
  }

  if (!gebruiker) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Alert severity="error">Kan profiel niet laden</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      <Box sx={{ py: 4 }}>
        <Paper elevation={3} sx={{ p: 4 }}>
          {/* Header met terug knop */}
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
            <Button
              startIcon={<ArrowBackIcon />}
              onClick={handleGoBack}
              sx={{ mr: 2 }}
            >
              Terug
            </Button>
            <Typography variant="h4" component="h1" sx={{ flexGrow: 1 }}>
              Mijn Profiel
            </Typography>
            <Chip
              icon={<PersonIcon />}
              label={userType}
              color={userType === 'Verkoper' ? 'primary' : 'secondary'}
              sx={{ fontSize: '1rem', py: 2.5 }}
            />
          </Box>

          <Divider sx={{ mb: 3 }} />

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

          {/* Account Informatie */}
          <Box sx={{ mb: 4 }}>
            <Typography variant="h6" gutterBottom color="primary">
              Account Informatie
            </Typography>
            <Stack spacing={2}>
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Email"
                    value={gebruiker.email}
                    disabled
                    helperText="Email kan niet worden gewijzigd"
                  />
                </Box>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Gebruikersnaam"
                    value={gebruiker.username}
                    disabled
                    helperText="Afgeleid van email"
                  />
                </Box>
              </Box>
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Geregistreerd op"
                    value={new Date(gebruiker.registreren).toLocaleDateString('nl-NL')}
                    disabled
                  />
                </Box>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Laatst ingelogd"
                    value={gebruiker.inloggen ? new Date(gebruiker.inloggen).toLocaleDateString('nl-NL') : 'Nooit'}
                    disabled
                  />
                </Box>
              </Box>
            </Stack>
          </Box>

          <Divider sx={{ mb: 3 }} />

          {/* Bedrijfsgegevens */}
          <Box sx={{ mb: 4 }}>
            <Typography variant="h6" gutterBottom color="primary">
              Bedrijfsgegevens
            </Typography>
            <Stack spacing={2}>
              <TextField
                fullWidth
                label="Bedrijfsnaam"
                name="bedrijfsnaam"
                value={formData.bedrijfsnaam}
                onChange={handleInputChange}
                disabled={!editMode}
                required
              />
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="KVK Nummer"
                    name="kvk"
                    value={formData.kvk}
                    onChange={handleInputChange}
                    disabled={!editMode}
                    required
                    helperText="8 cijfers"
                  />
                </Box>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="BTW Nummer"
                    name="btw_nummer"
                    value={formData.btw_nummer}
                    onChange={handleInputChange}
                    disabled={!editMode}
                    required
                  />
                </Box>
              </Box>
              <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Telefoonnummer"
                    name="telefoon"
                    value={formData.telefoon}
                    onChange={handleInputChange}
                    disabled={!editMode}
                    required
                  />
                </Box>
                <Box sx={{ flex: { xs: '1 1 100%', sm: '1 1 calc(50% - 8px)' } }}>
                  <TextField
                    fullWidth
                    label="Rekeningnummer (IBAN)"
                    name="rekening_nummer"
                    value={formData.rekening_nummer}
                    onChange={handleInputChange}
                    disabled={!editMode}
                    required
                  />
                </Box>
              </Box>
              <TextField
                fullWidth
                label={userType === 'Verkoper' ? 'Bedrijfsadres' : 'Leveradres'}
                name="adres"
                value={formData.adres}
                onChange={handleInputChange}
                disabled={!editMode}
                required
                multiline
                rows={2}
              />
            </Stack>
          </Box>

          {/* Actie knoppen */}
          <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            {!editMode ? (
              <Button
                variant="contained"
                color="primary"
                startIcon={<EditIcon />}
                onClick={handleEdit}
              >
                Bewerk Gegevens
              </Button>
            ) : (
              <>
                <Button
                  variant="outlined"
                  color="error"
                  startIcon={<CancelIcon />}
                  onClick={handleCancel}
                  disabled={saving}
                >
                  Annuleren
                </Button>
                <Button
                  variant="contained"
                  color="success"
                  startIcon={saving ? <CircularProgress size={20} /> : <SaveIcon />}
                  onClick={handleSave}
                  disabled={saving}
                >
                  {saving ? 'Opslaan...' : 'Opslaan'}
                </Button>
              </>
            )}
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default ProfielPagina;
