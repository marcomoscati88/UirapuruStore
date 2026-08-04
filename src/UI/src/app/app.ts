import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

interface MenuItem {
  readonly label: string;
  readonly path: string;
  readonly icon: string;
}

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly menuItems: readonly MenuItem[] = [
    { label: 'Dashboard', path: '/dashboard', icon: 'fa-solid fa-house' },
    { label: 'Prodotti', path: '/products', icon: 'fa-solid fa-box-open' },
    { label: 'Categorie', path: '/categories', icon: 'fa-solid fa-layer-group' },
  ];
}
