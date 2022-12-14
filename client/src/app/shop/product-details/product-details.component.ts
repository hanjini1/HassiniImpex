import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem } from 'src/app/shared/models/basket';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct = {} as IProduct;
  quantity: number = 1;
  constructor(
    private shopService: ShopService,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.loadProduct();
  }
  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }
  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
  loadProduct() {
    this.activateRoute.paramMap.subscribe({
      next: (param) => {
        let id = Number(param.get('id'));
        this.shopService.getProduct(id).subscribe({
          next: (prod) => {
            this.product = prod;
            this.bcService.set('@productDetails', prod.name);
          },
          error: (err) => console.log(err),
        });
      },
    });
  }
}
