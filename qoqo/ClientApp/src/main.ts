import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

// to avoid make query request to the dom at each request
let elementCache: HTMLBaseElement;

export function getBaseUrl() {
  elementCache ||= document.getElementsByTagName('base')[0];
  return elementCache?.href;
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
  {
    provide: 'AUTH',
    useFactory: () => {
      return {};
    },
  },
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers)
  .bootstrapModule(AppModule)
  .catch((err) => console.log(err));
