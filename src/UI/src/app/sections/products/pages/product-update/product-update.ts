import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin, finalize } from 'rxjs';
import { Category } from '../../../categories/models/category.model';
import { CategoryService } from '../../../categories/services/category.service';
import { ProductCategorySelect } from '../../components/product-category-select/product-category-select';
import { GetProductResult, Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-update',
  imports: [ReactiveFormsModule, ProductCategorySelect],
  templateUrl: './product-update.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './product-update.css',
  ],
})
export class ProductUpdate implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly categoryService = inject(CategoryService);

  readonly idProduct = input.required<number>();
  readonly saved = output<void>();

  private readonly originalProduct = signal<Product | null>(null);

  protected readonly maxNameLength = 255;
  protected readonly maxDescriptionLength = 500;
  protected readonly categories = signal<readonly Category[]>([]);
  protected readonly isEditingActive = signal(false);
  protected readonly isLoading = signal(true);
  protected readonly isSaving = signal(false);
  protected readonly isChangingStatus = signal(false);
  protected readonly isEnabled = signal(true);
  protected readonly loadError = signal<string | null>(null);
  protected readonly requestError = signal<string | null>(null);
  protected readonly requestSuccess = signal<string | null>(null);
  protected readonly selectedImage = signal<File | null>(null);
  protected readonly imagePath = signal<string | null>(null);

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
    forkJoin({
      productResult: this.productService.get(this.idProduct()),
      categoryResult: this.categoryService.list(),
    })
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: ({ productResult, categoryResult }) => {
          if (!categoryResult.isSuccess) {
            this.loadError.set(categoryResult.errorMessage);
            return;
          }

          this.categories.set(categoryResult.object);
          this.setProduct(productResult);
        },
        error: () =>
          this.loadError.set(
            'Non è stato possibile caricare il prodotto selezionato.',
          ),
      });
  }

  protected isFormInvalid(): boolean {
    return this.description.invalid;
  }

  protected truncatePrice(): void {
    if (this.price.value !== null) {
      this.price.setValue(Math.trunc(this.price.value * 100) / 100);
    }
  }

  protected onEdit(): void {
    this.requestError.set(null);
    this.requestSuccess.set(null);
    this.isEditingActive.set(true);
  }

  protected onCancel(): void {
    const product = this.originalProduct();

    if (product !== null) {
      this.setFormValues(product);
    }

    this.markFormAsPristine();
    this.selectedImage.set(null);
    this.requestError.set(null);
    this.isEditingActive.set(false);
  }

  protected onSave(): void {
    if (this.isFormInvalid()) {
      this.description.markAsTouched();
      return;
    }

    this.truncatePrice();

    const product: Product = {
      id: this.idProduct(),
      name: this.name.value.trim(),
      description: this.description.value.trim() || null,
      price: this.price.value,
      idCategory: this.idCategory.value,
      subCategory: this.subCategory.value.trim() || null,
      isEnabled: this.isEnabled(),
      imagePath: this.imagePath(),
    };

    this.isSaving.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    this.productService
      .update(product)
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(result.errorMessage);
            this.isSaving.set(false);
            return;
          }

          const selectedImage = this.selectedImage();

          if (selectedImage === null) {
            this.completeSave(product, product.imagePath);
            return;
          }

          this.productService
            .updateImage(product.id, selectedImage)
            .subscribe({
              next: (imageResult) => {
                if (!imageResult.isSuccess || imageResult.object === null) {
                  this.requestError.set(imageResult.errorMessage);
                  this.isSaving.set(false);
                  return;
                }

                this.completeSave(product, imageResult.object);
              },
              error: () => {
                this.requestError.set('Il prodotto è stato modificato, ma non è stato possibile caricare l\'immagine.');
                this.isSaving.set(false);
              },
            });
        },
        error: () => {
          this.requestError.set(
            'Non è stato possibile modificare il prodotto.',
          );
          this.isSaving.set(false);
        },
      });
  }

  protected onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const image = input.files?.[0] ?? null;

    this.requestError.set(null);

    if (image === null) {
      this.selectedImage.set(null);
      return;
    }

    if (!['image/jpeg', 'image/png', 'image/webp'].includes(image.type)) {
      this.selectedImage.set(null);
      input.value = '';
      this.requestError.set('Il formato dell\'immagine deve essere JPEG, PNG o WebP.');
      return;
    }

    if (image.size > 5 * 1024 * 1024) {
      this.selectedImage.set(null);
      input.value = '';
      this.requestError.set('L\'immagine non può superare i 5 MB.');
      return;
    }

    this.selectedImage.set(image);
  }

  private setProduct(result: GetProductResult): void {
    if (!result.isSuccess || result.object === null) {
      this.loadError.set(result.errorMessage);
      return;
    }

    this.originalProduct.set(result.object);
    this.setFormValues(result.object);
    this.markFormAsPristine();
  }

  private setFormValues(product: Product): void {
    this.name.setValue(product.name);
    this.description.setValue(product.description ?? '');
    this.price.setValue(product.price);
    this.idCategory.setValue(product.idCategory);
    this.subCategory.setValue(product.subCategory ?? '');
    this.isEnabled.set(product.isEnabled);
    this.imagePath.set(product.imagePath);
  }

  private markFormAsPristine(): void {
    this.name.markAsUntouched();
    this.description.markAsUntouched();
    this.price.markAsUntouched();
    this.idCategory.markAsUntouched();
    this.subCategory.markAsUntouched();
    this.name.markAsPristine();
    this.description.markAsPristine();
    this.price.markAsPristine();
    this.idCategory.markAsPristine();
    this.subCategory.markAsPristine();
  }

  protected onToggleEnabled(): void {
    const enable = !this.isEnabled();

    this.isChangingStatus.set(true);
    this.requestError.set(null);
    this.requestSuccess.set(null);

    const request = enable
      ? this.productService.enable(this.idProduct())
      : this.productService.disable(this.idProduct());

    request
      .pipe(finalize(() => this.isChangingStatus.set(false)))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.requestError.set(result.errorMessage);
            return;
          }

          this.isEnabled.set(enable);

          const product = this.originalProduct();

          if (product !== null) {
            this.originalProduct.set({ ...product, isEnabled: enable });
          }

          this.requestSuccess.set(enable ? 'Prodotto abilitato correttamente.' : 'Prodotto disabilitato correttamente.');
          this.saved.emit();
        },
        error: () =>
          this.requestError.set(
            'Non è stato possibile modificare lo stato del prodotto.',
          ),
      });
  }

  private completeSave(product: Product, imagePath: string | null): void {
    const savedProduct = { ...product, imagePath };

    this.originalProduct.set(savedProduct);
    this.setFormValues(savedProduct);
    this.selectedImage.set(null);
    this.markFormAsPristine();
    this.isSaving.set(false);
    this.isEditingActive.set(false);
    this.requestSuccess.set('Prodotto modificato correttamente.');
    this.saved.emit();
  }
}
