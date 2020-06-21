<template>
    <div id="app">
        <MainMenu />
        <Home msg="Hello world!" />
    </div>
</template>

<script lang="ts">
    import { Component, Vue} from 'vue-property-decorator';
    import Home from './components/Home.vue';
    import MainMenu from './components/MainMenu.vue';
    import BootstrapVue from 'bootstrap-vue';
    Vue.use(BootstrapVue);

    @Component({
        components: {
            Home,
            MainMenu
        }
    })

    export default class App extends Vue { }
    
    import 'bootstrap/dist/css/bootstrap.css';
    import 'bootstrap-vue/dist/bootstrap-vue.css';
</script>

<style>
</style>
