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
import { HttpErrorResponse } from '@angular/common/http';

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

  errorMessage = '';

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
    this.errorMessage = this.validate();
    if (this.errorMessage) return;

    const action = this.isNew
      ? this.giftService.createGift(this.curGift)
      : this.giftService.updateGift(this.giftId, this.curGift);

    action.subscribe({
      next: () => this.route.navigate(['/gifts']),
      error: (err) => this.errorMessage = this.serverError(err)
    });
  }

  // בדיקה לפני שליחה - אותם כללים כמו בשרת
  private validate(): string {
    const g = this.curGift;
    if (!g.name?.trim()) return 'יש למלא שם מתנה.';
    if (g.name.length > 20) return 'שם המתנה יכול להכיל עד 20 תווים.';
    if (!Number.isInteger(g.price) || g.price < 1 || g.price > 100) return 'מחיר הכרטיס חייב להיות מספר שלם בין 1 ל-100.';
    if (g.categoryId == null) return 'יש לבחור קטגוריה.';
    if (!g.donorId) return 'יש לבחור תורם.';
    if (!g.imageUrl?.trim()) return 'יש למלא כתובת תמונה.';
    if (!g.description?.trim()) return 'יש למלא תיאור.';
    if (g.description.length > 70) return 'התיאור יכול להכיל עד 70 תווים.';
    return '';
  }

  // הודעה ברורה לפי מה שהשרת החזיר
  private serverError(err: HttpErrorResponse): string {
    const fields: Record<string, string> = {
      Name: 'שם המתנה יכול להכיל עד 20 תווים.',
      Description: 'התיאור חובה ויכול להכיל עד 70 תווים.',
      Price: 'מחיר הכרטיס חייב להיות מספר שלם בין 1 ל-100.',
    };
    const bad = Object.keys(err.error?.errors ?? {}).find(k => fields[k]);
    if (bad) return fields[bad];
    if (err.status === 404) return 'המתנה לא נמצאה - ייתכן שנמחקה.';
    const text = typeof err.error === 'string' ? err.error : err.error?.message;
    if (text?.includes('Category')) return 'הקטגוריה שנבחרה לא קיימת.';
    if (text?.includes('Donor')) return 'התורם שנבחר לא קיים.';
    return this.isNew ? 'שגיאה ביצירת המתנה. נסה שנית.' : 'שגיאה בעדכון הפרטים. נסה שנית.';
  }
}