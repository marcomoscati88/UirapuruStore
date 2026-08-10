import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateCategoryInput,
  CreateCategoryResult,
} from '../models/create-category.model';
import { CategoryResult, GetCategoryListResult, GetCategoryResult } from '../models/category.model';
import {
  UpdateCategoryInput,
  UpdateCategoryResult,
} from '../models/update-category.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private readonly httpClient = inject(HttpClient);

  get(idCategory: number): Observable<GetCategoryResult> {
    return this.httpClient.get<GetCategoryResult>(`/api/category/get/${idCategory}`);
  }

  list(): Observable<GetCategoryListResult> {
    return this.httpClient.get<GetCategoryListResult>('/api/category/list');
  }

  create(command: CreateCategoryInput): Observable<CreateCategoryResult> {
    return this.httpClient.post<CreateCategoryResult>(
      '/api/category/create',
      command,
    );
  }

  update(command: UpdateCategoryInput): Observable<UpdateCategoryResult> {
    return this.httpClient.post<UpdateCategoryResult>(
      '/api/category/update',
      command,
    );
  }

  disable(id: number): Observable<CategoryResult<null>> {
    return this.httpClient.post<CategoryResult<null>>('/api/category/disable', { id });
  }

  enable(id: number): Observable<CategoryResult<null>> {
    return this.httpClient.post<CategoryResult<null>>('/api/category/enable', { id });
  }
}
