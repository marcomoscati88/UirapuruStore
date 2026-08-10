import { ProductResult } from './product.model';

export interface UpdateProductInput {
  readonly id: number;
  readonly name: string;
  readonly description: string | null;
  readonly price: number | null;
  readonly idCategory: number | null;
  readonly subCategory: string | null;
}

export type UpdateProductResult = ProductResult<null>;
