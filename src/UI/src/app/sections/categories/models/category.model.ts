export interface Category {
  readonly id: number;
  readonly name: string;
  readonly isEnabled: boolean;
}

export interface CategoryResult<TObject> {
  readonly isSuccess: boolean;
  readonly errorMessage: string;
  readonly object: TObject;
}

export type GetCategoryResult = CategoryResult<Category | null>;
export type GetCategoryListResult = CategoryResult<readonly Category[]>;
