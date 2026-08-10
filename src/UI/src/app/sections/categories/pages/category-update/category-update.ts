import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs';
import { GetCategoryResult } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-category-update',
  imports: [ReactiveFormsModule],
  templateUrl: './category-update.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './category-update.css',
  ],
})
export class CategoryUpdate implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly activatedRoute = inject(ActivatedRoute);

  readonly idCategory = input<number | null>(null);
  readonly saved = output<void>();

  private readonly selectedCategoryId = signal<number | null>(null);
  private readonly originalCategoryName = signal('');

  protected readonly maxNameLength = 255;
  protected readonly isEditingActive = signal(false);
  protected readonly isLoading = signal(true);
  protected readonly isSaving = signal(false);
  protected readonly isChangingStatus = signal(false);
  protected readonly isEnabled = signal(true);
  protected readonly loadError = signal<string | null>(null);
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

  ngOnInit(): void {
    const idCategory =
      this.idCategory() ?? Number(this.activatedRoute.snapshot.paramMap.get('id'));

    if (!Number.isInteger(idCategory) || idCategory <= 0) {
      this.isLoading.set(false);
      this.loadError.set('La categoria selezionata non è valida.');
      return;
    }

    this.selectedCategoryId.set(idCategory);

    this.categoryService
      .get(idCategory)
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (result) => this.setCategory(result),
        error: () =>
          this.loadError.set(
            'Non è stato possibile caricare la categoria selezionata.',
          ),
      });
  }

  private setCategory(result: GetCategoryResult): void {
    if (!result.isSuccess || result.object === null) {
      this.loadError.set(result.errorMessage);
      return;
    }

    this.originalCategoryName.set(result.object.name);
    this.name.setValue(result.object.name);
    this.isEnabled.set(result.object.isEnabled);
  }

  protected onEdit(): void {
    this.requestError.set(null);
    this.requestSuccess.set(null);
    this.isEditingActive.set(true);
  }

  protected onCancel(): void {
    this.name.setValue(this.originalCategoryName());
    this.name.markAsUntouched();
    this.name.markAsPristine();
    this.requestError.set(null);
    this.isEditingActive.set(false);
  }

  protected onSave(): void {
    if (this.name.invalid) {
      this.name.markAsTouched();
      return;
    }

    const idCategory = this.selectedCategoryId();

    if (idCategory === null) {
      return;
    }

    const name = this.name.value.trim();

    this.isSaving.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    this.categoryService
      .update({ id: idCategory, name })
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(result.errorMessage);
            return;
          }

          this.originalCategoryName.set(name);
          this.name.setValue(name);
          this.name.markAsPristine();
          this.isEditingActive.set(false);
          this.requestSuccess.set('Categoria modificata correttamente.');
          this.saved.emit();
        },
        error: () =>
          this.requestError.set(
            'Non è stato possibile modificare la categoria.',
          ),
      });
  }

  protected onToggleEnabled(): void {
    const idCategory = this.selectedCategoryId();

    if (idCategory === null) {
      return;
    }

    const enable = !this.isEnabled();

    this.isChangingStatus.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    const request = enable
      ? this.categoryService.enable(idCategory)
      : this.categoryService.disable(idCategory);

    request
      .pipe(finalize(() => this.isChangingStatus.set(false)))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(result.errorMessage);
            return;
          }

          this.isEnabled.set(enable);
          this.requestSuccess.set(enable ? 'Categoria abilitata correttamente.' : 'Categoria disabilitata correttamente.');
          this.saved.emit();
        },
        error: () =>
          this.requestError.set(
            'Non è stato possibile modificare lo stato della categoria.',
          ),
      });
  }
}
