<template>
    <div id="app">
        <MainMenu />
        <Home />
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import BootstrapVue from 'bootstrap-vue';  

    import Home from './components/Home.vue';
    import MainMenu from './components/MainMenu.vue';    

    @Component({
        components: {
            Home,
            MainMenu
        }
    })
    export default class App extends Vue {
        mounted() {
            this.$store.dispatch('authModule/LoadUser');
        }
    }
    Vue.use(BootstrapVue);

    import 'bootstrap/dist/css/bootstrap.css';
    import 'bootstrap-vue/dist/bootstrap-vue.css';
</script>

<style>    
</style>
