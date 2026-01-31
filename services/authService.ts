import { 
  createUserWithEmailAndPassword, 
  signInWithEmailAndPassword, 
  signOut, 
  sendPasswordResetEmail,
  updateProfile,
  User
} from 'firebase/auth';
import { auth } from '../config/firebase';

const API_BASE_URL = 'http://localhost:5005/api';

export interface DadosUsuario {
  id: number;
  firebaseUid: string;
  email: string;
  nome: string;
  criadoEm: string;
  ultimoLoginEm?: string;
  ativo: boolean;
}

// Registrar novo usuário
export const registerUser = async (email: string, password: string, name: string): Promise<DadosUsuario> => {
  try {
    // 1. Criar usuário no Firebase
    const userCredential = await createUserWithEmailAndPassword(auth, email, password);
    const firebaseUser = userCredential.user;

    // 2. Atualizar perfil do Firebase com o nome
    await updateProfile(firebaseUser, { displayName: name });

    // 3. Criar usuário no backend
    const response = await fetch(`${API_BASE_URL}/usuarios`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        firebaseUid: firebaseUser.uid,
        email: firebaseUser.email,
        nome: name
      })
    });

    if (!response.ok) {
      throw new Error('Erro ao criar usuário no backend');
    }

    const userData: DadosUsuario = await response.json();
    return userData;
  } catch (error: any) {
    console.error('Erro no registro:', error);
    throw new Error(getFirebaseErrorMessage(error.code));
  }
};

// Login de usuário
export const loginUser = async (email: string, password: string): Promise<DadosUsuario> => {
  try {
    // 1. Fazer login no Firebase
    const userCredential = await signInWithEmailAndPassword(auth, email, password);
    const firebaseUser = userCredential.user;

    // 2. Buscar dados do usuário no backend
    const response = await fetch(`${API_BASE_URL}/usuarios/firebase/${firebaseUser.uid}`);
    
    if (!response.ok) {
      throw new Error('Usuário não encontrado no sistema');
    }

    const userData: DadosUsuario = await response.json();

    // 3. Atualizar último login
    await fetch(`${API_BASE_URL}/usuarios/login/${firebaseUser.uid}`, { method: 'POST' });

    return userData;
  } catch (error: any) {
    console.error('Erro no login:', error);
    throw new Error(getFirebaseErrorMessage(error.code));
  }
};

// Logout de usuário
export const logoutUser = async (): Promise<void> => {
  try {
    await signOut(auth);
  } catch (error: any) {
    console.error('Erro no logout:', error);
    throw new Error('Erro ao fazer logout');
  }
};

// Redefinir senha
export const resetPassword = async (email: string): Promise<void> => {
  try {
    await sendPasswordResetEmail(auth, email);
  } catch (error: any) {
    console.error('Erro ao enviar email:', error);
    throw new Error(getFirebaseErrorMessage(error.code));
  }
};

// Obter usuário atual do Firebase
export const getCurrentFirebaseUser = (): User | null => {
  return auth.currentUser;
};

// Tradução de erros do Firebase
const getFirebaseErrorMessage = (errorCode: string): string => {
  const errorMessages: { [key: string]: string } = {
    'auth/email-already-in-use': 'Este email já está em uso',
    'auth/invalid-email': 'Email inválido',
    'auth/operation-not-allowed': 'Operação não permitida',
    'auth/weak-password': 'A senha deve ter pelo menos 6 caracteres',
    'auth/user-disabled': 'Esta conta foi desabilitada',
    'auth/user-not-found': 'Usuário não encontrado',
    'auth/wrong-password': 'Senha incorreta',
    'auth/too-many-requests': 'Muitas tentativas. Tente novamente mais tarde',
    'auth/network-request-failed': 'Erro de conexão. Verifique sua internet',
    'auth/invalid-credential': 'Credenciais inválidas'
  };

  return errorMessages[errorCode] || 'Erro ao autenticar. Tente novamente.';
};
