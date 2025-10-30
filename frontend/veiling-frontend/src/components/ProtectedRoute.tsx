import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

interface ProtectedRouteProps {
  children: React.ReactElement;
  allowedUserTypes?: ('Koper' | 'Verkoper')[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  allowedUserTypes
}) => {
  const { isLoggedIn, userType } = useAuth();

  // Check 1: Gebruiker moet ingelogd zijn
  if (!isLoggedIn) {
    return <Navigate to="/login" replace />;
  }

  // Check 2: Als allowedUserTypes is opgegeven, check of userType erin zit
  if (allowedUserTypes && allowedUserTypes.length > 0) {
    if (!userType || !allowedUserTypes.includes(userType)) {
      // Redirect naar juiste pagina op basis van userType
      if (userType === 'Koper') {
        return <Navigate to="/" replace />;
      } else if (userType === 'Verkoper') {
        return <Navigate to="/mijn-producten" replace />;
      }
      return <Navigate to="/login" replace />;
    }
  }

  // Als alle checks slagen, toon de gevraagde pagina
  return children;
};

export default ProtectedRoute;
