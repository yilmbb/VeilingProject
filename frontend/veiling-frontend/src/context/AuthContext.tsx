import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

interface AuthContextType {
  isLoggedIn: boolean;
  userType: 'Koper' | 'Verkoper' | null;
  email: string | null;
  username: string | null;
  login: (email: string, userType: 'Koper' | 'Verkoper') => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [userType, setUserType] = useState<'Koper' | 'Verkoper' | null>(null);
  const [email, setEmail] = useState<string | null>(null);
  const [username, setUsername] = useState<string | null>(null);

  // Bij laden: check localStorage voor bestaande sessie
  useEffect(() => {
    const storedEmail = localStorage.getItem('auth_email');
    const storedUserType = localStorage.getItem('auth_userType');
    const storedUsername = localStorage.getItem('auth_username');

    if (storedEmail && storedUserType) {
      setIsLoggedIn(true);
      setEmail(storedEmail);
      setUserType(storedUserType as 'Koper' | 'Verkoper');
      setUsername(storedUsername || storedEmail.split('@')[0]);
    }
  }, []);

  const login = (email: string, userType: 'Koper' | 'Verkoper') => {
    const derivedUsername = email.split('@')[0];

    // State updaten
    setIsLoggedIn(true);
    setEmail(email);
    setUserType(userType);
    setUsername(derivedUsername);

    // localStorage updaten voor persistence
    localStorage.setItem('auth_email', email);
    localStorage.setItem('auth_userType', userType);
    localStorage.setItem('auth_username', derivedUsername);
  };

  const logout = () => {
    // State clearen
    setIsLoggedIn(false);
    setEmail(null);
    setUserType(null);
    setUsername(null);

    // localStorage clearen
    localStorage.removeItem('auth_email');
    localStorage.removeItem('auth_userType');
    localStorage.removeItem('auth_username');
  };

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        userType,
        email,
        username,
        login,
        logout
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook om AuthContext te gebruiken
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
