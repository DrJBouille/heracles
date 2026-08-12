import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ErrorModal } from './shared/components/modal/error-modal/error-modal';
import { ErrorHandlingService } from './core/services/error-handling-service/error-handling-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ErrorModal],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  private errorHandlingService = inject(ErrorHandlingService);

  error = this.errorHandlingService.error;
}
