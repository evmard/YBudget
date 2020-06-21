<template>
    <div class="Budget">
        <div v-if="isLoaded">
            <div v-if="hasBudget">
                <h1>Balance {{ budget.balance }}</h1>                
            </div>
            <div v-else>
                <h1>You have no budget yet. Create new one </h1><b-button @click="createNew">Create</b-button>
            </div>
        </div>
        <div v-else>
            <h1>Loading...</h1>
        </div>
        <b-alert show variant="danger" v-if="hasError">Error: {{error}}</b-alert>
</div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { Budget } from '@/models/Budget';
    import { mapGetters } from 'vuex';

    @Component({
        computed: {
            ...mapGetters({
                isLoaded: 'budgetModule/IsLoaded',
                hasBudget: 'budgetModule/HasBudget',
                budget: 'budgetModule/Budget'
            })
        }
    })
    export default class BudgetComp extends Vue {

        error: string = '';
        get hasError(): boolean {
            return this.error !== '';
        }

        createNew() {
            this.$store.dispatch('budgetModule/NewBudget')
                .then((msg) => {
                    this.error = msg;
                    if (msg != '')
                        console.error('then ' + msg);
                })
                .catch((err) => {
                    this.error = err;
                    console.error('catch ' + err);
                });     
        }

        mounted() {
            this.$store.dispatch('budgetModule/Load')
                .then((msg) => {
                    if (msg != '')
                        console.error('then ' + msg);
                })
                .catch((err) => {
                    this.error = err;
                    console.error('catch ' + err);
                });     
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
