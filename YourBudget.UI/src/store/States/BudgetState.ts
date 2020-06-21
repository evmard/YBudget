import { Budget } from '@/models/Budget';

export interface BudgetState {
    budget: Budget | null;
    lastLoad: Date | null;
};