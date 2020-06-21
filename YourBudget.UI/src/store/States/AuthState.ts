import { User } from '@/models/user';

export interface AuthState {
    user: User | null;
    token: string | null;
}