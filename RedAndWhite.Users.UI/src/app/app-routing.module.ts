import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './views/products/products.component';

const routes: Routes = [
  // { path: '', component: HomeComponent },
  { path: 'productos', component: ProductsComponent },
  // { path: 'producto', component: ProductoComponent },
  // { path: 'producto/:id', component: ProductoComponent },

  // otherwise redirect to home
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
