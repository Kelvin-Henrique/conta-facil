import { initializeApp } from 'firebase/app';
import { getAuth } from 'firebase/auth';

// Firebase Configuration - Conta Fácil
const firebaseConfig = {
  apiKey: "AIzaSyCo18jXyeUfzalEKTfu71RgUplY6mD1Yfw",
  authDomain: "conta-facil-36865.firebaseapp.com",
  projectId: "conta-facil-36865",
  storageBucket: "conta-facil-36865.firebasestorage.app",
  messagingSenderId: "580279894383",
  appId: "1:580279894383:web:59f029dc79be4aa324689a",
  measurementId: "G-PZJ2N5QLJF"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Authentication
export const auth = getAuth(app);

export default app;
