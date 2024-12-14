import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations, provideNoopAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpInterceptorFn } from '@angular/common/http';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { TimeagoModule } from 'ngx-timeago';
import { PresenceService } from './_services/presence.service';

export const apiUrl:string="https://localhost:7183/";
export const hubUrl:string="https://localhost:7183/hubs/";

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    provideAnimations(),
    importProvidersFrom(NgxSpinnerModule.forRoot()),
    importProvidersFrom(TimeagoModule.forRoot()),
    importProvidersFrom(PresenceService),
    provideToastr({
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
    }),
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }, provideNoopAnimations(), provideAnimationsAsync()]
};
