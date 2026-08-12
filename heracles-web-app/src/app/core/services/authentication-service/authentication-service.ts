import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private http = inject(HttpClient);

  private readonly token = signal<string | null>(null);

  readonly isAuthenticated = computed(() => this.token() !== null);

  register(request: RegisterRequestDto) {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/register`, request).pipe(
      tap(response => this.token.set(response.token))
    );
  }

  login(request: LoginRequestDto) {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/login`, request).pipe(
      tap(response => this.token.set(response.token))
    );
  }

  logout() {
    this.token.set(null);
  }

  getAccesToken() : string | null {
    return this.token();
  }
}
