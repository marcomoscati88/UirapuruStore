import { Component, inject, OnInit, output, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Category } from '../../../categories/models/category.model';
import { CategoryService } from '../../../categories/services/category.service';
import { ProductCategorySelect } from '../../components/product-category-select/product-category-select';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-create',
  imports: [ReactiveFormsModule, ProductCategorySelect],
  templateUrl: './product-create.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './product-create.css',
  ],
})
export class ProductCreate implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly categoryService = inject(CategoryService);

  readonly saved = output<void>();
  readonly savedAndClosed = output<void>();

  protected readonly maxNameLength = 255;
  protected readonly maxDescriptionLength = 500;
  protected readonly categories = signal<readonly Category[]>([]);
  protected readonly isSaving = signal(false);
  protected readonly categoryLoadError = signal<string | null>(null);
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

  protected readonly description = new FormControl('', {
    nonNullable: true,
    validators: [Validators.maxLength(this.maxDescriptionLength)],
  });

  protected readonly price = new FormControl<number | null>(null);
  protected readonly idCategory = new FormControl<number | null>(null);

  protected readonly subCategory = new FormControl('', {
    nonNullable: true,
  });

  ngOnInit(): void {
    this.categoryService.list().subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.categoryLoadError.set(result.errorMessage);
          return;
        }

        this.categories.set(result.object);
      },
      error: () =>
        this.categoryLoadError.set(
          'Non è stato possibile caricare le categorie.',
        ),
    });
  }

  protected onSave(): void {
    this.save(false);
  }

  protected onSaveAndClose(): void {
    this.save(true);
  }

  protected isFormInvalid(): boolean {
    return this.name.invalid || this.description.invalid;
  }

  protected truncatePrice(): void {
    if (this.price.value !== null) {
      this.price.setValue(Math.trunc(this.price.value * 100) / 100);
    }
  }

  private save(closeAfterSave: boolean): void {
    if (this.isFormInvalid()) {
      this.name.markAsTouched();
      this.description.markAsTouched();
      return;
    }

    this.truncatePrice();

    this.isSaving.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    this.productService
      .create({
        name: this.name.value.trim(),
        description: this.description.value.trim() || null,
        price: this.price.value,
        idCategory: this.idCategory.value,
        subCategory: this.subCategory.value.trim() || null,
      })
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(result.errorMessage);
            return;
          }

          this.saved.emit();

          if (closeAfterSave) {
            this.savedAndClosed.emit();
            return;
          }

          this.name.reset();
          this.description.reset();
          this.price.reset();
          this.idCategory.reset();
          this.subCategory.reset();
          this.requestSuccess.set('Prodotto salvato correttamente.');
        },
        error: () =>
          this.requestError.set(
            'Non è stato possibile creare il prodotto.',
          ),
      });
  }
}
