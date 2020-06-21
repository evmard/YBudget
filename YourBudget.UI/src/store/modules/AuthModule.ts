import axios from 'axios';
import { MutationTree, ActionTree, GetterTree, Module } from 'vuex';

import { User } from '@/models/User';
import { RootState } from '@/models/RootState';
import { UserCredantional } from '@/models/UserCredantional';

import { AuthState } from '../States/AuthState';
import { AuthService } from '@/api/AuthService';


const state: AuthState = {
    user: null,
    token: null
};

const getters: GetterTree<AuthState, RootState> = {
    IsLogined(state): boolean {
        return state.user !== null && state.token !== null
    },

    UserName(state): string {
        var name = "";
        if (state.user !== null)
            name = state.user.name;
        return name;
    },

    Token(state): string | null {
        return state.token;
    }
};

const mutations: MutationTree<AuthState> = {
    SetUser(state, user: User) {
        state.user = user;
    },

    SetToken(state, token: string) {
        state.token = token;
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
    },

    ResetUser(state) {
        axios.defaults.headers.common['Authorization'] = '';
        state.token = null;
        state.user = null;
    }
};
const actions: ActionTree<AuthState, RootState> = {
    Login({ commit }, authInfo: UserCredantional): Promise<string>{

        return new Promise((resolve, reject) => {
            var authService = new AuthService();
            authService.Authorize(authInfo)
                .then((data) => {
                    let user: User = {
                        login: authInfo.login,
                        name: data.username
                    }

                    if (authInfo.stayLoggined) {
                        try {
                            localStorage.setItem('UserName', user.name);
                            localStorage.setItem('UserLogin', user.login);
                            localStorage.setItem('UserToken', data.access_token);
                        }
                        catch (e) {
                            console.error(e);
                        }
                    }
                    else {
                        clearUserStorage();
                    }

                    commit('SetToken', data.access_token);
                    commit('SetUser', user);
                    resolve('');
                })
                .catch((error) => {
                    if (error !== undefined && error.response !== undefined && error.response.status === 400) {
                        resolve(error.response.data);
                    }
                    else {
                        reject(error);
                    }
                });
        });
    },

    Logout({ commit }): void {
        commit('ResetUser');
        clearUserStorage();
    },

    LoadUser({ commit }): void {
        try {
            let login = localStorage.getItem('UserLogin');
            if (login === null)
                return;

            let name = localStorage.getItem('UserName');
            if (name === null)
                return;

            var token = localStorage.getItem('UserToken');

            commit('SetToken', token);
            let user: User = {
                login: login,
                name: name
            }
            commit('SetUser', user);
        }
        catch (e) {
            console.error(e);
        }
    }

};

const authModule: Module<AuthState, RootState> = {
    state,
    getters,
    mutations,
    actions,
    namespaced: true
};

export default authModule

function clearUserStorage() {
    try {
        localStorage.removeItem('UserName');
        localStorage.removeItem('UserLogin');
        localStorage.removeItem('UserToken');
    }
    catch (e) {
        console.error(e);
    }
}
