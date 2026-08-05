import { Component, HostListener, signal } from '@angular/core';
import { CategoryCreate } from '../category-create/category-create';

@Component({
  selector: 'app-category-landing',
  imports: [CategoryCreate],
  templateUrl: './category-landing.html',
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
