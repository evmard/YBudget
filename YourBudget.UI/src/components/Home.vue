<template>
    <div class="home">
        <p v-if="isLogined"><BudgetComp></BudgetComp></p>
        <p v-else>Welcome to the \"your budget\". Please sign on to continue... (Sorry, registration not available yet)"</p>        
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import BudgetComp from './Budget.vue';
    import { mapGetters } from 'vuex';

    @Component({
        components: {
            BudgetComp
        },

        computed: {
            ...mapGetters({
                isLogined: 'authModule/IsLogined'
            })
        }
    })
    export default class Home extends Vue {
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
