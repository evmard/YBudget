import axios from 'axios';
import { UserCredantional } from '@/models/UserCredantional';
import { TokenInfo } from '@/models/TokenInfo';


export class AuthService {

    private API_URL: string;

    constructor(api_url = 'https://localhost:44301') {
        this.API_URL = api_url;
    }

    Authorize(userCred: UserCredantional): Promise<TokenInfo> {
        const url = this.API_URL + '/api/user/token';
        return new Promise((resolve, reject) => {
            axios.post<TokenInfo>(url, userCred)
                .then((response) => { resolve(response.data) })
                .catch((error) => { reject(error) })
        });
    }  
}