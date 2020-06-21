import axios from 'axios';
import { DataResult } from '@/models/Result';
import { Budget } from '@/models/Budget';

export class BudgetService {

    private API_URL: string;

    constructor(api_url = 'https://localhost:44301') {
        this.API_URL = api_url;
    }

    GetBudget(): Promise<DataResult<Budget>> {
        var url = this.API_URL + '/api/Budget/Budget';

        return new Promise((resolve, reject) => {
            axios.get<DataResult<Budget>>(url)
                .then((response) => { resolve(response.data) })
                .catch((error) => { reject(error) })
        });
    }

    NewBudget(): Promise<DataResult<Budget>> {
        var url = this.API_URL + '/api/Budget/NewBudget';

        return new Promise((resolve, reject) => {
            axios.get<DataResult<Budget>>(url)
                .then((response) => { resolve(response.data) })
                .catch((error) => { reject(error) })
        });
    }
}