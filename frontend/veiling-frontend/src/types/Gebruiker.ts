// Base interface voor Gebruiker
export interface Gebruiker {
  gebruiker_id?: number;
  wachtwoord: string;
  email: string;
  registreren?: string; // ISO date string
  inloggen?: string | null; // ISO date string
  username?: string; // Computed from email
}

// Interface voor Verkoper (extends Gebruiker)
export interface Verkoper extends Gebruiker {
  bedrijfsnaam: string;
  kvk: string;
  btw_nummer: string;
  telefoon: string;
  bedrijfsadres: string; // Let op: Verkoper heeft bedrijfsadres
  rekening_nummer: string;
}

// Interface voor Koper (extends Gebruiker)
export interface Koper extends Gebruiker {
  bedrijfsnaam: string;
  kvk: string;
  btw_nummer: string;
  telefoon: string;
  leveradres: string; // Let op: Koper heeft leveradres (niet bedrijfsadres!)
  rekening_nummer: string;
}

// Type voor registratie data (zonder gebruiker_id)
export type RegistratieVerkoper = Omit<Verkoper, 'gebruiker_id' | 'registreren' | 'inloggen' | 'username'>;
export type RegistratieKoper = Omit<Koper, 'gebruiker_id' | 'registreren' | 'inloggen' | 'username'>;

// Union type voor registratie
export type RegistratieData = RegistratieVerkoper | RegistratieKoper;

// Type om gebruikerstype te onderscheiden
export type GebruikerType = 'Koper' | 'Verkoper';
