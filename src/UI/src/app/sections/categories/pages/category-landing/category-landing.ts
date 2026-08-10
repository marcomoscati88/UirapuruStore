import { Component, HostListener, inject, OnInit, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { Category, GetCategoryListResult } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';
import { CategoryCreate } from '../category-create/category-create';
import { CategoryUpdate } from '../category-update/category-update';

@Component({
  selector: 'app-category-landing',
  imports: [CategoryCreate, CategoryUpdate],
  templateUrl: './category-landing.html',
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './category-landing.css',
  ],
})
export class CategoryLanding implements OnInit {
  private readonly categoryService = inject(CategoryService);

  protected readonly isCreatePanelOpen = signal(false);
  protected readonly selectedCategoryId = signal<number | null>(null);
  protected readonly categories = signal<readonly Category[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly loadError = signal<string | null>(null);
  protected readonly statusError = signal<string | null>(null);
  protected readonly changingStatusIds = signal<ReadonlySet<number>>(new Set());

  ngOnInit(): void {
    this.loadCategories();
  }

  protected loadCategories(): void {
    this.isLoading.set(true);
    this.loadError.set(null);

    this.categoryService
      .list()
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe({
        next: (result) => this.setCategories(result),
        error: () =>
          this.loadError.set(
            'Non è stato possibile caricare le categorie.',
          ),
      });
  }

  private setCategories(result: GetCategoryListResult): void {
    if (!result.isSuccess) {
      this.loadError.set(result.errorMessage);
      return;
    }

    this.categories.set(result.object);
  }

  protected openCreatePanel(): void {
    this.selectedCategoryId.set(null);
    this.isCreatePanelOpen.set(true);
  }

  protected closeCreatePanel(): void {
    this.isCreatePanelOpen.set(false);
  }

  protected openUpdatePanel(idCategory: number): void {
    this.isCreatePanelOpen.set(false);
    this.selectedCategoryId.set(idCategory);
  }

  protected closeUpdatePanel(): void {
    this.selectedCategoryId.set(null);
  }

  protected isStatusChanging(idCategory: number): boolean {
    return this.changingStatusIds().has(idCategory);
  }

  protected onToggleEnabled(category: Category): void {
    const enable = !category.isEnabled;

    this.statusError.set(null);
    this.changingStatusIds.update(ids => new Set(ids).add(category.id));

    const request = enable
      ? this.categoryService.enable(category.id)
      : this.categoryService.disable(category.id);

    request
      .pipe(finalize(() => this.changingStatusIds.update(ids => {
        const updatedIds = new Set(ids);
        updatedIds.delete(category.id);
        return updatedIds;
      })))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.statusError.set(result.errorMessage);
            return;
          }

          this.categories.update(categories => categories.map(item => item.id === category.id ? { ...item, isEnabled: enable } : item));
        },
        error: () => this.statusError.set('Non è stato possibile modificare lo stato della categoria.'),
      });
  }

  @HostListener('document:keydown.escape')
  protected closePanelWithEscape(): void {
    this.closeCreatePanel();
    this.closeUpdatePanel();
  }
}
