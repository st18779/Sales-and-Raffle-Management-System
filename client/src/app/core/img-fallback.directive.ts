import { Directive, ElementRef, inject } from '@angular/core';

const FALLBACK_SRC = 'assets/no-image.png';

// מציג תמונת ברירת מחדל כשהתמונה המקורית לא נטענת (כתובת שגויה או ריקה)
@Directive({
  selector: 'img[appImgFallback]',
  host: { '(error)': 'useFallback()' },
})
export class ImgFallback {
  private img: HTMLImageElement = inject(ElementRef).nativeElement;

  useFallback() {
    // מונע לולאה אינסופית אם גם תמונת ברירת המחדל חסרה
    if (!this.img.src.endsWith(FALLBACK_SRC)) {
      this.img.src = FALLBACK_SRC;
    }
  }
}
