import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';

import { RootState } from '@/models/RootState';

import authModule from './modules/AuthModule';
import budgetModule from './modules/BudgetModule';


Vue.use(Vuex);

const store: StoreOptions<RootState> = {
    modules: {
        authModule,
        budgetModule
    }
};

export default new Vuex.Store<RootState>(store);