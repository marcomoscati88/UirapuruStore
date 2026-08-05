import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateCategoryCommand } from '../models/create-category-command.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private readonly httpClient = inject(HttpClient);

  create(command: CreateCategoryCommand): Observable<void> {
    return this.httpClient.post<void>('/api/category/create', command);
  }
}
