import { Component, inject } from '@angular/core';
import { DonorService } from '../../services/donor.service';
import { Donor } from '../../models/Donor.model';
import { tap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-donors',
  imports: [CommonModule,FormsModule,RouterLink],
  templateUrl: './donors-list.html',
  styleUrl: './donors-list.css',
})
export class DonorsList {
  donorService: DonorService = inject(DonorService)
  donors!: Donor[]
  showErrorOnDelete: boolean = false

  donorsList$ = this.donorService.getDonorsList().pipe(
      tap(donors => console.log(donors)));

  deleteDonor(donorId: string) {
    this.donorService.deleteDonor(donorId).subscribe({
      next: (res) => {
        console.log('נמחק בהצלחה:', res);
        window.location.reload();
      },
      error: (err) => {
        console.error('שגיאה במחיקה:', err);
        this.showErrorOnDelete = true;
      }
    });
  }

}
