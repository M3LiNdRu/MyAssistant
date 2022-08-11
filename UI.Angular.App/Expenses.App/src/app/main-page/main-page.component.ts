import { Component, OnInit } from '@angular/core';
import { CurrentSummary } from '../currentSummary';

import { SummariesService } from '../summaries.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {

  summary: CurrentSummary = {
    month: "",
    year: "",
    totalAmount: 0,
    progressBar: {}
  };

  constructor(private summariesService: SummariesService) { }

  ngOnInit(): void {
    this.getSummary();
  }

  getSummary(): void {
    this.summariesService.getSummary()
    .subscribe(summary => this.summary = summary);
  }

}
