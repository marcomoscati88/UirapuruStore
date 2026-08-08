export interface CreateCategoryInput {
  readonly name: string;
}

export interface CreateCategoryResult {
  readonly isSuccess: boolean;
  readonly errorMessage: string;
}
