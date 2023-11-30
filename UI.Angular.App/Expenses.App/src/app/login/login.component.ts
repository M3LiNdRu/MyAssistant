import { OnInit, Component, ElementRef } from '@angular/core';
import { SocialAuthService, GoogleLoginProvider, SocialUser } from "@abacritt/angularx-social-login";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  
  private accessToken = '';
  user: SocialUser = new SocialUser();
  loggedIn: boolean = false;


  constructor(
    private elementRef: ElementRef, 
    private authService: SocialAuthService) {
  }

  ngOnInit() {
    this.authService.authState.subscribe((user) => {
      this.user = user;
      this.loggedIn = (user != null);
      if (this.loggedIn) {
        this.accessToken = user.idToken;
        localStorage.setItem("id_token", this.accessToken);
      }
    });
  }

  signOut(): void {
    this.authService.signOut();
    localStorage.removeItem("id_token");
  }

  getAccessToken(): void {
    this.authService.getAccessToken(GoogleLoginProvider.PROVIDER_ID).then(accessToken => this.accessToken = accessToken);
  }

  refreshToken(): void {
    this.authService.refreshAccessToken(GoogleLoginProvider.PROVIDER_ID);
  }
}
