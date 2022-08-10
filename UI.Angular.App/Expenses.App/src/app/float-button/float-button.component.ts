import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-float-button',
  templateUrl: './float-button.component.html',
  styleUrls: ['./float-button.component.scss']
})
export class FloatButtonComponent implements OnInit {

  private paths: string[] = ['/expense/add','/not-authorized']
  constructor(public router: Router) { }

  ngOnInit(): void {
  }

  isButtonVisible(): boolean {
    return !this.paths.includes(this.router.url)
  }

}
