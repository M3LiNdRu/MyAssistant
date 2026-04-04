import { Component, OnInit } from '@angular/core';
import { ChartType, Column } from 'angular-google-charts';
import { CurrentSummary } from '../currentSummary';

import { SummariesService } from '../summaries.service';

@Component({
    selector: 'app-main-page',
    templateUrl: './main-page.component.html',
    styleUrls: ['./main-page.component.scss'],
    standalone: false
})
export class MainPageComponent implements OnInit {

  summary: CurrentSummary = {
    month: "",
    year: "",
    totalAmount: 0,
    progressBar: {}
  };

  columns: Column[] = ['Categoria', '%'];
  myData: any[][] = [['Buit', 100]];
  myType: ChartType;

  constructor(private summariesService: SummariesService) 
  {
    this.myType = ChartType.PieChart;
  }

  ngOnInit(): void {
    this.getSummary();
  }

  getSummary(): void {
    this.summariesService.getSummary()
    .subscribe(summary => { 
      this.summary = summary;
      this.populateChart();
    });
  }

  populateChart(): void {
    if (Object.keys(this.summary.progressBar).length > 0) {
      this.columns = Object.keys(this.summary.progressBar);
      this.myData = Object.entries(this.summary.progressBar);
    }
  }

}
