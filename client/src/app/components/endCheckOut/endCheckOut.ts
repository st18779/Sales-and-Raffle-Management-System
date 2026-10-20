import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BasketModel } from '../../models/BasketModel.model';
import { ImgFallback } from '../../core/img-fallback.directive';

@Component({
  selector: 'app-endCheckOut',
  imports: [RouterLink, ImgFallback],
  templateUrl: './endCheckOut.html',
  styleUrl: './endCheckOut.css',
})
export class endCheckOut {
  // הסיכום מגיע רק בניווט מדף התשלום; ברענון הדף מוצגת רק הודעת ההצלחה
  order = inject(Router).getCurrentNavigation()?.extras.state as { items: BasketModel[]; total: number } | undefined;
}
