import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, ReplaySubject, map, tap } from 'rxjs';
import { User } from './../_models/user';
import { Router } from '@angular/router';
import { IAuthResponse } from '../_models/IAuthResponse';
import { PresenceService } from './presence.service';


@Injectable({
  providedIn: 'root'
})
export class AuthinticationService {
  private authSubject = new BehaviorSubject<boolean>(false);
  private readonly TOKEN_NAME = 'jwt_token';
  private currentUserSubject = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient, private router: Router, private hub: PresenceService) { this.initializeCurrentUserId() }

  login(credentials: any): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>('https://localhost:7183/Account/login', credentials).pipe(
      map((response: IAuthResponse) => {
        localStorage.setItem(this.TOKEN_NAME, response.token);
        this.authSubject.next(true);
        this.currentUserSubject.next(response.id);
        localStorage.setItem('user_id', response.id);
        this.hub.CreateHubConnection();
        return response;
      })
    );
  }
  register(credentials: any): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>('https://localhost:7183/Account/register', credentials).pipe(
      map((response: IAuthResponse) => {
        localStorage.setItem(this.TOKEN_NAME, response.token);
        this.authSubject.next(true);
        this.currentUserSubject.next(response.id);
        localStorage.setItem('user_id', response.id);
        this.hub.CreateHubConnection();
        return response;
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_NAME);
    localStorage.removeItem('user_id');
    this.hub.StopConnection();
    this.authSubject.next(false);
    this.router.navigate(['/home']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem(this.TOKEN_NAME);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_NAME);
  }
  getCurrentUserId(): Observable<string | null> {
    return this.currentUserSubject.asObservable();
  }
  private initializeCurrentUserId(): void {
    const storedUser = localStorage.getItem('user_id');
    if (storedUser) {
      this.currentUserSubject.next(storedUser);
    }
  }
}
