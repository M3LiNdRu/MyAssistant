import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-display',
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.scss']
})
export class DisplayComponent implements OnInit {
  
  @Input() 
  currentAmount: number = 0;
  
  constructor() { }

  ngOnInit(): void {
  }

}
