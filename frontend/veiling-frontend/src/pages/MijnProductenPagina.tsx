import React, { useEffect, useMemo, useState } from 'react';
import { Container, Typography, Paper, Box, Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField, Stack } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { useAuth } from '../context/AuthContext';
import productService, { ProductResponse } from '../services/productService';

const MijnProductenPagina: React.FC = () => {
  const { email } = useAuth();
  const verkoperEmail = useMemo(() => email || '', [email]);

  const [producten, setProducten] = useState<ProductResponse[]>([]);
  const [laden, setLaden] = useState<boolean>(false);
  const [open, setOpen] = useState<boolean>(false);
  const [form, setForm] = useState({ naam: '', beschrijving: '', prijs: '', voorraad: '' });
  const [fouten, setFouten] = useState<{ [k: string]: string }>({});

  const fetchProducten = async () => {
    if (!verkoperEmail) return;
    setLaden(true);
    try {
      const data = await productService.getMijnProducten(verkoperEmail);
      setProducten(data);
    } finally {
      setLaden(false);
    }
  };

  useEffect(() => {
    fetchProducten();
  }, [verkoperEmail]);

  const validate = () => {
    const errs: { [k: string]: string } = {};
    if (!form.naam.trim()) errs.naam = 'Naam is verplicht';
    const prijs = Number(form.prijs);
    if (isNaN(prijs) || prijs < 0) errs.prijs = 'Prijs moet 0 of hoger zijn';
    const voorraad = Number(form.voorraad);
    if (!Number.isInteger(voorraad) || voorraad < 0) errs.voorraad = 'Voorraad moet 0 of hoger zijn';
    setFouten(errs);
    return Object.keys(errs).length === 0;
  };

  const submit = async () => {
    if (!validate() || !verkoperEmail) return;
    await productService.createProduct({
      naam: form.naam.trim(),
      beschrijving: form.beschrijving.trim() || undefined,
      prijs: Number(form.prijs),
      voorraad: Number(form.voorraad),
      verkoperEmail
    });
    setOpen(false);
    setForm({ naam: '', beschrijving: '', prijs: '', voorraad: '' });
    await fetchProducten();
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Mijn Producten
        </Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => setOpen(true)}
        >
          Product Toevoegen
        </Button>
      </Box>

      <Paper elevation={3} sx={{ p: 3, minHeight: '400px' }}>
        {laden ? (
          <Typography>Bezig met laden...</Typography>
        ) : producten.length === 0 ? (
          <>
            <Typography variant="h6" color="text.secondary" align="center" sx={{ mt: 8 }}>
              Je hebt nog geen producten toegevoegd
            </Typography>
            <Typography variant="body1" color="text.secondary" align="center" sx={{ mt: 2 }}>
              Klik op "Product Toevoegen" om je eerste product aan te bieden
            </Typography>
          </>
        ) : (
          <Box
            sx={{
              display: 'grid',
              gridTemplateColumns: { xs: '1fr', md: '1fr 1fr', lg: '1fr 1fr 1fr' },
              gap: 2
            }}
          >
            {producten.map((p) => (
              <Paper key={p.product_id} variant="outlined" sx={{ p: 2 }}>
                <Stack spacing={0.5}>
                  <Typography variant="h6">{p.naam}</Typography>
                  {p.beschrijving && (
                    <Typography variant="body2" color="text.secondary">
                      {p.beschrijving}
                    </Typography>
                  )}
                  <Typography variant="body2">Prijs: € {p.prijs.toFixed(2)}</Typography>
                  <Typography variant="body2">Voorraad: {p.voorraad}</Typography>
                  <Typography variant="caption" color="text.secondary">
                    Aangemaakt op: {new Date(p.aangemaakt_op).toLocaleString()}
                  </Typography>
                </Stack>
              </Paper>
            ))}
          </Box>
        )}
      </Paper>

      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="sm">
        <DialogTitle>Product toevoegen</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1 }}>
            <TextField
              label="Naam"
              value={form.naam}
              onChange={(e) => setForm((f) => ({ ...f, naam: e.target.value }))}
              error={!!fouten.naam}
              helperText={fouten.naam}
              required
              fullWidth
            />
            <TextField
              label="Beschrijving"
              value={form.beschrijving}
              onChange={(e) => setForm((f) => ({ ...f, beschrijving: e.target.value }))}
              multiline
              minRows={3}
              fullWidth
            />
            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
              <TextField
                label="Prijs (€)"
                value={form.prijs}
                onChange={(e) => setForm((f) => ({ ...f, prijs: e.target.value }))}
                error={!!fouten.prijs}
                helperText={fouten.prijs}
                type="number"
                inputProps={{ min: 0, step: '0.01' }}
                required
                fullWidth
              />
              <TextField
                label="Voorraad"
                value={form.voorraad}
                onChange={(e) => setForm((f) => ({ ...f, voorraad: e.target.value }))}
                error={!!fouten.voorraad}
                helperText={fouten.voorraad}
                type="number"
                inputProps={{ min: 0, step: 1 }}
                required
                fullWidth
              />
            </Stack>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)}>Annuleren</Button>
          <Button variant="contained" onClick={submit} disabled={!verkoperEmail}>
            Opslaan
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default MijnProductenPagina;
