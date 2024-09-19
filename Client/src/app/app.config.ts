import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorsInterceptor } from './core/interceptors/errors.interceptor';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';
import { InitService } from './core/services/init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from './core/interceptors/auth.interceptor';

/**
 * Initializes the application by invoking the InitService to load initial data.
 * Removes the splash screen once the initialization is complete.
 *
 * @param {InitService} initService - The service responsible for loading initial data.
 * @returns {() => Promise<void>} A function that returns a Promise which resolves after the initialization process.
 */
function initializeApp(initService: InitService) 
{
  return () => lastValueFrom(initService.init()).finally( () => {
    const splash = document.getElementById('initial-splash');
    if(splash) 
      splash.remove();
  })
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([
      errorsInterceptor,
      loadingInterceptor,
      authInterceptor 
    ])),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      multi: true,
      deps: [InitService]
    }
  ]
};
