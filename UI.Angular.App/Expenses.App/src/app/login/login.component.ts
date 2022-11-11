import { AfterViewInit, Component, ElementRef } from '@angular/core';

import { LoginService } from '../login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {
  
  loggedIn: boolean = false;


  constructor(
    private elementRef: ElementRef, 
    private loginService: LoginService) {
  }

  ngOnInit() {
    this.loggedIn = this.loginService.isUserLoggedIn();
    if (!this.loggedIn) {
      this.loginService.login(); 
      this.loginService.loggedIn.subscribe(value => this.loggedIn = value);
    }
  }

  ngAfterViewInit() {
    const s = document.createElement("script");
    s.type = "text/javascript";
    s.src = "https://accounts.google.com/gsi/client";
    this.elementRef.nativeElement.appendChild(s);
  }

  logout(): void {
    this.loginService.logout();
    this.loggedIn = false;
  }

}
