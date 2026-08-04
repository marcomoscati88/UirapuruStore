import { Component, HostListener, signal } from '@angular/core';
import { CategoryCreate } from '../category-create/category-create';

@Component({
  selector: 'app-category-landing',
  imports: [CategoryCreate],
  template: `
    <section class="page-heading">
      <div class="heading-row">
        <p class="eyebrow">CATEGORIE</p>
        <button
          type="button"
          aria-controls="category-create-panel"
          [attr.aria-expanded]="isCreatePanelOpen()"
          (click)="openCreatePanel()"
        >
          <i class="fa-solid fa-plus" aria-hidden="true"></i>
          NUOVA CATEGORIA
        </button>
      </div>
    </section>

    @if (isCreatePanelOpen()) {
      <div
        class="panel-backdrop"
        aria-hidden="true"
        (click)="closeCreatePanel()"
      ></div>

      <aside
        id="category-create-panel"
        class="side-panel"
        role="dialog"
        aria-modal="true"
        aria-label="Crea una nuova categoria"
      >
        <button
          class="close-panel"
          type="button"
          aria-label="Chiudi pannello"
          (click)="closeCreatePanel()"
        >
          <i class="fa-solid fa-xmark" aria-hidden="true"></i>
        </button>

        <app-category-create />
      </aside>
    }
  `,
  styleUrls: [
    '../../../../shared/page-heading/page-heading.css',
    './category-landing.css',
  ],
})
export class CategoryLanding {
  protected readonly isCreatePanelOpen = signal(false);

  protected openCreatePanel(): void {
    this.isCreatePanelOpen.set(true);
  }

  protected closeCreatePanel(): void {
    this.isCreatePanelOpen.set(false);
  }

  @HostListener('document:keydown.escape')
  protected closePanelWithEscape(): void {
    this.closeCreatePanel();
  }
}
