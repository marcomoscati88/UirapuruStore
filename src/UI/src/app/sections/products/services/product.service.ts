import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateProductInput,
  CreateProductResult,
} from '../models/create-product.model';
import { GetProductListResult, GetProductResult, ProductResult } from '../models/product.model';
import {
  UpdateProductInput,
  UpdateProductResult,
} from '../models/update-product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly httpClient = inject(HttpClient);

  get(idProduct: number): Observable<GetProductResult> {
    return this.httpClient.get<GetProductResult>(`/api/products/get/${idProduct}`);
  }

  list(): Observable<GetProductListResult> {
    return this.httpClient.get<GetProductListResult>('/api/products/list');
  }

  create(command: CreateProductInput): Observable<CreateProductResult> {
    return this.httpClient.post<CreateProductResult>(
      '/api/products/create',
      command,
    );
  }

  update(command: UpdateProductInput): Observable<UpdateProductResult> {
    return this.httpClient.post<UpdateProductResult>(
      '/api/products/update',
      command,
    );
  }

  disable(id: number): Observable<ProductResult<null>> {
    return this.httpClient.post<ProductResult<null>>('/api/products/disable', { id });
  }

  enable(id: number): Observable<ProductResult<null>> {
    return this.httpClient.post<ProductResult<null>>('/api/products/enable', { id });
  }

  updateImage(id: number, image: File): Observable<ProductResult<string | null>> {
    const formData = new FormData();
    formData.append('id', id.toString());
    formData.append('image', image);

    return this.httpClient.post<ProductResult<string | null>>('/api/products/update-image', formData);
  }
}
