import { Component, inject, OnInit } from '@angular/core';
import { GiftService } from '../../services/gift.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from '../../models/Category.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../services/category.service';
import { Observable } from 'rxjs';
import { GiftFormModel } from '../../models/GiftFormModel.model';
import { Donor } from '../../models/Donor.model';
import { DonorService } from '../../services/donor.service';

@Component({
  selector: 'app-gift-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './gift-form.html',
  styleUrl: './gift-form.css',
})
export class GiftForm implements OnInit {
  giftId!: number
  curGift: GiftFormModel = new GiftFormModel();
  isNew: boolean = true;

  categories$!: Observable<Category[]>;
  donors$!: Observable<Donor[]>;

  showErrorCreate: boolean = false;
  showErrorUpdate: boolean = false;

  private activatedRoute = inject(ActivatedRoute);
  private route = inject(Router);
  private giftService = inject(GiftService);
  private categoryService = inject(CategoryService);
  private donorsService = inject(DonorService);

  ngOnInit(): void {
    this.categories$ = this.categoryService.getCategoriesList();
    this.donors$ = this.donorsService.getDonorsList();

    this.activatedRoute.params.subscribe(params => {
      if (params['id']) {
        this.giftId = +params['id'];
        this.isNew = false;
        this.giftService.getGiftById(params['id']).subscribe({
          next: (gift) => {
            this.curGift = {
              name: gift.name,
              description: gift.description,
              price: gift.price,
              categoryId: gift.categoryId,
              donorId: gift.donorId,
              imageUrl: gift.imageUrl
            }},
              error: (err) => console.error("שגיאה בטעינת מתנה", err)
          });
      }
    });
  }

  saveGift() {
    console.log(this.curGift);
    const action = this.isNew
      ? this.giftService.createGift(this.curGift)
      : this.giftService.updateGift(this.giftId, this.curGift);

    action.subscribe({
      next: () => this.route.navigate(['/gifts']),
      error: () => this.isNew ? this.showErrorCreate = true : this.showErrorUpdate = true
    });
  }
}