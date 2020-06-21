import { BudgetItem } from './BudgetItem';

export interface Budget {
    Id: string,
    balance: number,
    items: BudgetItem[],
    opened: Date,
    needToClose: boolean
}