import { Component, HostListener, inject, OnInit, signal } from '@angular/core';
import { finalize, forkJoin } from 'rxjs';
import { Category } from '../../../categories/models/category.model';
import { CategoryService } from '../../../categories/services/category.service';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { ProductCreate } from '../product-create/product-create';
import { ProductUpdate } from '../product-update/product-update';

@Component({
  selector: 'app-product-landing',
  imports: [ProductCreate, ProductUpdate],
  templateUrl: './product-landing.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './product-landing.css',
  ],
})
export class ProductLanding implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly categoryService = inject(CategoryService);

  protected readonly isCreatePanelOpen = signal(false);
  protected readonly selectedProductId = signal<number | null>(null);
  protected readonly products = signal<readonly Product[]>([]);
  protected readonly categories = signal<readonly Category[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly loadError = signal<string | null>(null);
  protected readonly statusError = signal<string | null>(null);
  protected readonly changingStatusIds = signal<ReadonlySet<number>>(new Set());

  ngOnInit(): void {
    this.loadProducts();
  }

  protected loadProducts(): void {
    this.isLoading.set(true);
    this.loadError.set(null);

    forkJoin({
      productResult: this.productService.list(),
      categoryResult: this.categoryService.list(),
    })
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: ({ productResult, categoryResult }) => {
          if (!productResult.isSuccess) {
            this.loadError.set(productResult.errorMessage);
            return;
          }

          if (!categoryResult.isSuccess) {
            this.loadError.set(categoryResult.errorMessage);
            return;
          }

          this.products.set(productResult.object);
          this.categories.set(categoryResult.object);
        },
        error: () =>
          this.loadError.set(
            'Non è stato possibile caricare i prodotti.',
          ),
      });
  }

  protected categoryName(idCategory: number | null): string {
    if (idCategory === null) {
      return '—';
    }

    return this.categories().find((category) => category.id === idCategory)?.name ?? '—';
  }

  protected formatPrice(price: number | null): string {
    if (price === null) {
      return '—';
    }

    return new Intl.NumberFormat('it-IT', {
      style: 'currency',
      currency: 'EUR',
    }).format(price);
  }

  protected openCreatePanel(): void {
    this.selectedProductId.set(null);
    this.isCreatePanelOpen.set(true);
  }

  protected closeCreatePanel(): void {
    this.isCreatePanelOpen.set(false);
  }

  protected openUpdatePanel(idProduct: number): void {
    this.isCreatePanelOpen.set(false);
    this.selectedProductId.set(idProduct);
  }

  protected closeUpdatePanel(): void {
    this.selectedProductId.set(null);
  }

  protected isStatusChanging(idProduct: number): boolean {
    return this.changingStatusIds().has(idProduct);
  }

  protected onToggleEnabled(product: Product): void {
    const enable = !product.isEnabled;

    this.statusError.set(null);
    this.changingStatusIds.update(ids => new Set(ids).add(product.id));

    const request = enable
      ? this.productService.enable(product.id)
      : this.productService.disable(product.id);

    request
      .pipe(finalize(() => this.changingStatusIds.update(ids => {
        const updatedIds = new Set(ids);
        updatedIds.delete(product.id);
        return updatedIds;
      })))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.statusError.set(result.errorMessage);
            return;
          }

          this.products.update(products => products.map(item => item.id === product.id ? { ...item, isEnabled: enable } : item));
        },
        error: () => this.statusError.set('Non è stato possibile modificare lo stato del prodotto.'),
      });
  }

  @HostListener('document:keydown.escape')
  protected closePanelWithEscape(): void {
    this.closeCreatePanel();
    this.closeUpdatePanel();
  }
}
