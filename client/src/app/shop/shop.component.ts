import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: true }) searchTerm: ElementRef =
    {} as ElementRef;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  brandIdSelected: number = 0;
  typeIdSelected: number = 0;
  sortSelected: string = 'name';
  shopParms: ShopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];
  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParms).subscribe({
      next: (res: IPagination | null) => {
        if (res) {
          this.products = res.data;
          this.shopParms.pageIndex = res.pageIndex;
          this.shopParms.pageSize = res.pageSize;
          this.shopParms.count = res.count;
        }
      },
      error: (err) => console.log(err),
    });
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (brands) => (this.brands = [{ id: 0, name: 'All' }, ...brands]),
    });
  }
  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (types) => (this.types = [{ id: 0, name: 'All' }, ...types]),
    });
  }

  onBrandSelected(brandId: number) {
    this.shopParms.brandId = brandId;
    this.shopParms.pageIndex = 1;
    this.getProducts();
  }
  onTypeSelected(typeId: number) {
    this.shopParms.typeId = typeId;
    this.shopParms.pageIndex = 1;
    this.getProducts();
  }
  onSortSelected($event: Event) {
    this.shopParms.sort = ($event.target as HTMLSelectElement).value;

    this.getProducts();
  }

  onPageChanged($event: PageChangedEvent) {
    this.shopParms.pageIndex = $event.page;
    this.getProducts();
  }
  onSearch() {
    this.shopParms.search = this.searchTerm.nativeElement.value;
    this.shopParms.pageIndex = 1;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParms = new ShopParams();
    this.getProducts();
  }
}
