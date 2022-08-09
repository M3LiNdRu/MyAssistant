import { AfterViewInit, Component, ElementRef, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {
  
  loggedIn: boolean = false;

  constructor(private elementRef: ElementRef) {
  }

  ngOnInit() {
    this.loggedIn = localStorage.getItem('id_token') != null;
    (window as any).googleLogin = function(response: any) {
      localStorage.setItem('id_token', response.credential);
      this.loggedIn = true;
    } 
  }

  ngAfterViewInit() {
    const s = document.createElement("script");
    s.type = "text/javascript";
    s.src = "https://accounts.google.com/gsi/client";
    this.elementRef.nativeElement.appendChild(s);
  }

  logout(): void {
    localStorage.removeItem("id_token");
    this.loggedIn = false;
  }

}
