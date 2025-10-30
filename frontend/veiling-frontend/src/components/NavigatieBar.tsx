import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box, Chip, Tooltip } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import PersonIcon from '@mui/icons-material/Person';
import LogoutIcon from '@mui/icons-material/Logout';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

const NavigatieBar: React.FC = () => {
  const navigate = useNavigate();
  const { isLoggedIn, userType, username, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1, cursor: 'pointer' }}
          onClick={() => navigate(isLoggedIn ? (userType === 'Koper' ? '/' : '/mijn-producten') : '/')}>
          Veiling Platform
        </Typography>

        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          {!isLoggedIn ? (
            // NIET INGELOGD: Toon Login en Registreer
            <>
              <Button color="inherit" onClick={() => navigate('/login')}>
                Login
              </Button>
              <Button color="inherit" onClick={() => navigate('/registratie')}>
                Registreer
              </Button>
            </>
          ) : (
            // INGELOGD: Toon navigatie + profiel + uitloggen
            <>
              {/* Navigatie button op basis van rol */}
              <Button color="inherit" onClick={() => navigate(userType === 'Koper' ? '/' : '/mijn-producten')}>
                {userType === 'Koper' ? 'Veiling Overzicht' : 'Mijn Producten'}
              </Button>

              {/* Rol badge (altijd zichtbaar) */}
              <Chip
                icon={<PersonIcon />}
                label={userType}
                color={userType === 'Verkoper' ? 'primary' : 'secondary'}
                size="small"
                sx={{ fontWeight: 'bold' }}
              />

              {/* Klikbare gebruikersnaam -> navigeert naar profiel */}
              <Tooltip title="Bekijk je profiel" arrow>
                <Chip
                  icon={<AccountCircleIcon />}
                  label={username || 'Gebruiker'}
                  onClick={() => navigate('/profiel')}
                  color="default"
                  sx={{
                    cursor: 'pointer',
                    '&:hover': {
                      bgcolor: 'rgba(255, 255, 255, 0.2)',
                    },
                    transition: 'background-color 0.3s'
                  }}
                />
              </Tooltip>

              {/* Uitloggen button */}
              <Button
                color="inherit"
                startIcon={<LogoutIcon />}
                onClick={handleLogout}
              >
                Uitloggen
              </Button>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default NavigatieBar;
