export interface Product {
  readonly id: number;
  readonly name: string;
  readonly description: string | null;
  readonly price: number | null;
  readonly idCategory: number | null;
  readonly subCategory: string | null;
  readonly isEnabled: boolean;
  readonly imagePath: string | null;
}

export interface ProductResult<TObject> {
  readonly isSuccess: boolean;
  readonly errorMessage: string;
  readonly object: TObject;
}

export type GetProductResult = ProductResult<Product | null>;
export type GetProductListResult = ProductResult<readonly Product[]>;
