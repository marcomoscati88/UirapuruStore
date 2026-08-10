import { CategoryResult } from './category.model';

export interface UpdateCategoryInput {
  readonly id: number;
  readonly name: string;
}

export type UpdateCategoryResult = CategoryResult<null>;
