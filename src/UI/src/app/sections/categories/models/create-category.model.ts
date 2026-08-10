import { CategoryResult } from './category.model';

export interface CreateCategoryInput {
  readonly name: string;
}

export type CreateCategoryResult = CategoryResult<null>;
