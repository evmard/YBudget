<template>
    <b-modal id="LoginModal" centered title="Вход" @ok="onSubmit">
        <b-form @submit.stop.prevent="handleSubmit" v-if="show">
            <b-form-group id="loginGrp"
                          label="Логин:"
                          label-for="loginInput"
                          description="">
                <b-form-input id="loginInput"
                              type="text"
                              v-model="authInfo.Login"
                              required
                              placeholder="Введите имя пользователя" />
            </b-form-group>

            <b-form-group id="passGrp"
                          label="Пароль"
                          label-for="passInput">
                <b-form-input id="passInput"
                              type="password"
                              v-model="authInfo.Password"
                              required
                              placeholder="Введите пароль" />
            </b-form-group>
            <b-form-checkbox id="cbStayLog"
                             v-model="authInfo.stayLoggined"
                             name="cbStayLog"
                             value=true
                             unchecked-value=false>
                Входить автоматически
            </b-form-checkbox>
            <b-alert variant="danger" :show="HasError">
                {{error}}
            </b-alert>
        </b-form>
    </b-modal>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import { UserCredantional } from '@/models/UserCredantional';

    @Component
    export default class Login extends Vue {

        authInfo: UserCredantional = {
            login: '',
            password: '',
            stayLoggined: false
        };

        error: string = '';
        get HasError(): boolean {
            return this.error !== '';
        }

        show: boolean = true;

        onSubmit(e: Event) {
            e.preventDefault();
            this.$store.dispatch('authModule/Login', this.authInfo)
                .then((msg) => {
                    this.error = msg;
                    if (msg === '') {
                        this.ResetForm();
                        this.$root.$emit('bv::hide::modal', 'LoginModal');
                    }
                })
                .catch((err) => { console.error(err) });            
        }

        handleSubmit() {
            console.log('handleSubmit');
        }

        onReset(e: Event) {
            e.preventDefault();
            this.ResetForm();
            /* Trick to reset/clear native browser form validation state */
            this.show = false;
            this.$nextTick(() => {
                this.show = true
            })
        }

        ResetForm() {
            /* Reset our form values */
            this.authInfo.login = ''
            this.authInfo.password = ''
            this.authInfo.stayLoggined = false
        }
    }
</script>

<style scoped>
</style>
