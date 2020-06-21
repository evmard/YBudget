import { MutationTree, ActionTree, GetterTree, Module } from 'vuex';

import { Budget } from '@/models/Budget';
import { RootState } from '@/models/RootState';

import { BudgetState } from '../States/BudgetState';
import { BudgetService } from '@/api/BudgetService';

const state: BudgetState = {
    budget: null,
    lastLoad: null
};

const getters: GetterTree<BudgetState, RootState> = {
    IsLoaded(state): boolean {
        return state.lastLoad !== null;
    },

    Budget(state): Budget | null {
        return state.budget;
    },

    HasBudget(state): boolean {
        return state.budget !== null;
    }
};

const mutations: MutationTree<BudgetState> = {
    SetBudget(state, budget: Budget) {
        state.budget = budget;        
    },

    SetLoaded(state) {
        state.lastLoad = new Date();
    }
};

const actions: ActionTree<BudgetState, RootState> = {
    Load({ commit }): Promise<string>{

        return new Promise((resolve, reject) => {
            var service = new BudgetService();
            service.GetBudget()
                .then((data) => {
                    if (data.isSuccess) {
                        commit('SetBudget', data.data);
                        resolve('');
                    }
                    else {
                        resolve(data.message);
                    }
                    commit('SetLoaded');
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

    NewBudget({ commit }): Promise<string> {

        return new Promise((resolve, reject) => {
            var service = new BudgetService();
            service.NewBudget()
                .then((data) => {
                    if (data.isSuccess) {
                        commit('SetBudget', data.data);
                        resolve('');
                    }
                    else {
                        resolve(data.message);
                    }
                    commit('SetLoaded');
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
};

const budgetModule: Module<BudgetState, RootState> = {
    state,
    getters,
    mutations,
    actions,
    namespaced: true
};

export default budgetModule