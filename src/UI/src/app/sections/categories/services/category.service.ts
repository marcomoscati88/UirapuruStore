import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateCategoryInput,
  CreateCategoryResult,
} from '../models/create-category.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private readonly httpClient = inject(HttpClient);

  create(command: CreateCategoryInput): Observable<CreateCategoryResult> {
    return this.httpClient.post<CreateCategoryResult>(
      '/api/category/create',
      command,
    );
  }
}
