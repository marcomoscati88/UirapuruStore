import { Component, inject, output, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-category-create',
  imports: [ReactiveFormsModule],
  templateUrl: './category-create.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './category-create.css',
  ],
})
export class CategoryCreate {
  private readonly categoryService = inject(CategoryService);

  readonly saved = output<void>();
  readonly savedAndClosed = output<void>();

  protected readonly maxNameLength = 255;
  protected readonly isSaving = signal(false);
  protected readonly requestError = signal<string | null>(null);
  protected readonly requestSuccess = signal<string | null>(null);

  protected readonly name = new FormControl('', {
    nonNullable: true,
    validators: [
      Validators.required,
      Validators.pattern(/.*\S.*/),
      Validators.maxLength(this.maxNameLength),
    ],
  });

  protected onSave(): void {
    this.save(false);
  }

  protected onSaveAndClose(): void {
    this.save(true);
  }

  private save(closeAfterSave: boolean): void {
    if (this.name.invalid) {
      this.name.markAsTouched();
      return;
    }

    this.isSaving.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    this.categoryService
      .create({ name: this.name.value.trim() })
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(
              result.errorMessage || 'Non è stato possibile creare la categoria.',
            );
            return;
          }

          this.saved.emit();

          if (closeAfterSave) {
            this.savedAndClosed.emit();
            return;
          }

          this.name.reset();
          this.requestSuccess.set('Categoria salvata correttamente.');
        },
        error: () =>
          this.requestError.set(
            'Non è stato possibile creare la categoria. Riprova.',
          ),
      });
  }
}
