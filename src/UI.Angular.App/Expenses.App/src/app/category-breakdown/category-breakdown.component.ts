import { Component, OnInit } from '@angular/core';
import { CategoryBreakdown, CategoryRow } from '../category-breakdown';
import { SummariesService } from '../summaries.service';

@Component({
  selector: 'app-category-breakdown',
  templateUrl: './category-breakdown.component.html',
  styleUrls: ['./category-breakdown.component.scss'],
  standalone: false
})
export class CategoryBreakdownComponent implements OnInit {

  breakdown: CategoryBreakdown = { months: [], categories: [], totalAverage: 0 };
  displayedColumns: string[] = [];
  currentMonth = new Date().toISOString().slice(0, 7);

  constructor(private summariesService: SummariesService) {}

  ngOnInit(): void {
    this.summariesService.getCategoryBreakdown().subscribe(data => {
      if (!data) return;
      this.breakdown = data;
      this.displayedColumns = ['category', ...data.months, 'average'];
    });
  }

  getMonthAmount(row: CategoryRow, month: string): number {
    const idx = this.breakdown.months.indexOf(month);
    return idx >= 0 ? row.monthlyAmounts[idx] : 0;
  }

  getMonthTotal(month: string): number {
    const idx = this.breakdown.months.indexOf(month);
    return idx >= 0
      ? this.breakdown.categories.reduce((sum, row) => sum + row.monthlyAmounts[idx], 0)
      : 0;
  }
}
