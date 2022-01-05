import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  constructor(private matSnackbar: MatSnackBar) {}

  openMessage(message?: string, { action = 'Ok', duration = 3000 } = {}) {
    if (!message) return;
    this.matSnackbar.open(message, action, {
      duration: duration,
    });
  }

  openError(message?: string) {
    if (!message) return;
    this.matSnackbar.open(message, 'Close', {
      duration: 10000,
      panelClass: ['error-snackbar'],
    });
  }
}
