import { ProductResult } from './product.model';

export interface CreateProductInput {
  readonly name: string;
  readonly description: string | null;
  readonly price: number | null;
  readonly idCategory: number | null;
  readonly subCategory: string | null;
}

export type CreateProductResult = ProductResult<null>;
