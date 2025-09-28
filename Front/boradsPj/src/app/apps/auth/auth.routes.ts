import { Routes } from '@angular/router';
import { Login } from './login';
import { Error } from './error';
import { Register } from './register';
import { ForgotPassword } from './forgotpassword';
import { LockScreen } from './lockscreen';

export default [
    { path: 'error', component: Error },
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'forgotpassword', component: ForgotPassword },
    { path: 'lockscreen', component: LockScreen }
] as Routes;
