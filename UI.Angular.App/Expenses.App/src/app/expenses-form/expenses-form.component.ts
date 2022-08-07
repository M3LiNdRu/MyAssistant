import {Component, OnInit} from '@angular/core';
import { CategoriesService } from '../categories.service';
import { Category } from '../category';


@Component({
  selector: 'app-expenses-form',
  templateUrl: './expenses-form.component.html',
  styleUrls: ['./expenses-form.component.scss']
})
export class ExpensesFormComponent implements OnInit {
  
  categories: Category[] = []

  constructor(private categoriesService: CategoriesService) { 
  }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoriesService.getCategories()
    .subscribe(categories => this.categories = categories);
  }
}
