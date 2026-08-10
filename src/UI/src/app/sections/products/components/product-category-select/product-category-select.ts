import { Component, HostListener, input, output, signal } from '@angular/core';
import { Category } from '../../../categories/models/category.model';

@Component({
  selector: 'app-product-category-select',
  templateUrl: './product-category-select.html',
  styleUrl: './product-category-select.css',
})
export class ProductCategorySelect {
  readonly categories = input<readonly Category[]>([]);
  readonly value = input<number | null>(null);
  readonly disabled = input(false);
  readonly valueChange = output<number | null>();

  protected readonly isOpen = signal(false);

  protected selectedLabel(): string {
    if (this.value() === null) {
      return 'Nessuna categoria';
    }

    return this.categories().find((category) => category.id === this.value())?.name ?? 'Nessuna categoria';
  }

  protected toggle(): void {
    if (!this.disabled()) {
      this.isOpen.update((isOpen) => !isOpen);
    }
  }

  protected select(idCategory: number | null): void {
    this.valueChange.emit(idCategory);
    this.isOpen.set(false);
  }

  protected handleKeydown(event: KeyboardEvent): void {
    if (event.key === 'Escape') {
      this.isOpen.set(false);
      return;
    }

    if (event.key === 'Enter' || event.key === ' ' || event.key === 'ArrowDown') {
      event.preventDefault();
      this.toggle();
    }
  }

  @HostListener('document:click')
  protected close(): void {
    this.isOpen.set(false);
  }
}
