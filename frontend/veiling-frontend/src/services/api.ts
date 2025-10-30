import axios from 'axios';

// Configureer axios met base URL naar backend
const api = axios.create({
  baseURL: 'http://localhost:5094/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor voor logging (optioneel, handig voor debugging)
api.interceptors.request.use(
  (config) => {
    console.log('API Request:', config.method?.toUpperCase(), config.url);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor voor error handling
api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.status, response.config.url);
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.status, error.response?.data);
    return Promise.reject(error);
  }
);

export default api;
