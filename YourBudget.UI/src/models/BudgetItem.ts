export interface BudgetItem {
    id: string,
    name: string,
    order: number,
    debet: number,
    credit: number,
    planned: number,
    сumulative: boolean,
    currentAmount: number
}