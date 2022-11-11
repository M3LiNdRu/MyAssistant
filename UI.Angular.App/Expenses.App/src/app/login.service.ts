import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private loggedInSource$ = new BehaviorSubject(false);
  loggedIn = this.loggedInSource$.asObservable();
  
  constructor() { 
  }

  login() {
    (window as any).googleLogin = function(response: any) {
      localStorage.setItem('id_token', response.credential);
      window.location.reload();
    }
  }

  logout() {
    localStorage.removeItem("id_token");
    this.loggedInSource$.next(false);
  }

  isUserLoggedIn(): boolean {
    if (localStorage.getItem('id_token') !== null) {
      this.loggedInSource$.next(true);
      return true;
    }

    return false;
  }

  ngOnDestroy() {
    this.loggedInSource$.complete();
  }
}
