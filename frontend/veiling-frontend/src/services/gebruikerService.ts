import api from './api';
import { Gebruiker, Verkoper, Koper } from '../types/Gebruiker';

// Login request interface
export interface LoginRequest {
  Email: string;
  Wachtwoord: string;
}

// Login response interface (met Type indicator)
export interface LoginResponse {
  gebruiker_id: number;
  email: string;
  wachtwoord: string;
  registreren: string;
  inloggen: string | null;
  username?: string;
  Type: string; // "Koper" of "Verkoper" - komt van backend DTO
  // Verkoper specifieke velden (optioneel)
  bedrijfsnaam?: string;
  kvk?: string;
  btw_nummer?: string;
  telefoon?: string;
  bedrijfsadres?: string;
  leveradres?: string;
  rekening_nummer?: string;
}

// Registratie request interface
export interface RegistratieRequest {
  email: string;
  wachtwoord: string;
  Type: string; // "Koper" of "Verkoper"
  bedrijfsnaam: string;
  kvk: string;
  btw_nummer: string;
  telefoon: string;
  rekening_nummer: string;
  adres: string; // kan bedrijfsadres of leveradres zijn
}

// Update gebruiker request interface
export interface UpdateGebruikerRequest {
  gebruiker_id: number;
  email: string;
  wachtwoord: string;
  bedrijfsnaam: string;
  kvk: string;
  btw_nummer: string;
  telefoon: string;
  rekening_nummer: string;
  bedrijfsadres?: string; // Voor Verkoper
  leveradres?: string; // Voor Koper
}

/**
 * Registreer een nieuwe gebruiker (Verkoper of Koper)
 * @param request - Registratie data met Type indicator
 * @returns Promise met de aangemaakte gebruiker
 */
export const registreerGebruiker = async (request: RegistratieRequest): Promise<LoginResponse> => {
  try {
    const response = await api.post<LoginResponse>('/gebruiker', request);
    return response.data;
  } catch (error: any) {
    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error('Er is een fout opgetreden bij het registreren');
  }
};

/**
 * Login een gebruiker met email en wachtwoord
 * @param email - Email adres
 * @param wachtwoord - Wachtwoord
 * @returns Promise met de gebruiker data
 */
export const loginGebruiker = async (email: string, wachtwoord: string): Promise<LoginResponse> => {
  try {
    const loginRequest: LoginRequest = {
      Email: email,
      Wachtwoord: wachtwoord
    };

    const response = await api.post<LoginResponse>('/gebruiker/login', loginRequest);
    return response.data;
  } catch (error: any) {
    if (error.response?.status === 401) {
      throw new Error('Onjuist email of wachtwoord');
    }
    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error('Er is een fout opgetreden bij het inloggen');
  }
};

/**
 * Haal een gebruiker op via email
 * @param email - Email adres
 * @returns Promise met de gebruiker data
 */
export const getGebruikerByEmail = async (email: string): Promise<LoginResponse> => {
  try {
    const response = await api.get<LoginResponse>(`/gebruiker/email/${encodeURIComponent(email)}`);
    return response.data;
  } catch (error: any) {
    if (error.response?.status === 404) {
      throw new Error('Gebruiker niet gevonden');
    }
    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error('Er is een fout opgetreden bij het ophalen van gebruiker');
  }
};

/**
 * Haal alle gebruikers op
 * @returns Promise met array van gebruikers
 */
export const getAllGebruikers = async (): Promise<Gebruiker[]> => {
  try {
    const response = await api.get<Gebruiker[]>('/gebruiker');
    return response.data;
  } catch (error: any) {
    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error('Er is een fout opgetreden bij het ophalen van gebruikers');
  }
};

/**
 * Update gebruiker gegevens
 * @param gebruikerId - Het ID van de gebruiker
 * @param updateData - De bij te werken data
 * @returns Promise met de bijgewerkte gebruiker
 */
export const updateGebruiker = async (gebruikerId: number, updateData: UpdateGebruikerRequest): Promise<LoginResponse> => {
  try {
    const response = await api.put<LoginResponse>(`/gebruiker/${gebruikerId}`, updateData);
    return response.data;
  } catch (error: any) {
    if (error.response?.data?.message) {
      throw new Error(error.response.data.message);
    }
    throw new Error('Er is een fout opgetreden bij het updaten van gebruiker');
  }
};

/**
 * Bepaal of een gebruiker een Verkoper of Koper is
 * @param gebruiker - De gebruiker response van de API
 * @returns 'Verkoper' of 'Koper'
 */
export const bepaalGebruikerType = (gebruiker: LoginResponse): 'Verkoper' | 'Koper' => {
  // Backend stuurt nu het Type mee in de response DTO
  if (gebruiker.Type === 'Verkoper') {
    return 'Verkoper';
  }
  if (gebruiker.Type === 'Koper') {
    return 'Koper';
  }

  // Fallback voor oude responses (zou niet moeten gebeuren)
  console.warn('Geen Type veld in response, fallback naar bedrijfsadres check');
  if ('bedrijfsadres' in gebruiker && gebruiker.bedrijfsadres) {
    return 'Verkoper';
  }
  return 'Koper';
};
