import { Component, inject } from '@angular/core';
import { Donor } from '../../models/Donor.model';
import { DonorService } from '../../services/donor.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-donor-form',
  imports: [CommonModule,FormsModule],
  templateUrl: './donor-form.html',
  styleUrl: './donor-form.css',
})
export class DonorForm {
  donorId!: string;
  curDonor: Donor = new Donor
  isNew: boolean = true
  showErrorCreate: boolean = false;
  showErrorUpdate: boolean = false;

  private activatedRoute = inject(ActivatedRoute);
  private route = inject(Router);
  private donorService = inject(DonorService);


  ngOnInit(): void {

    this.activatedRoute.params.subscribe(params => {
      if (params['id']) {
        this.donorId = params['id']
        this.isNew = false;
        this.donorService.getDonorById(params['id']).subscribe({
          next: (donor) => this.curDonor = donor,
          error: (err) => console.error("שגיאה בטעינת תורם", err)
        });
      }
    });
  }

  saveDonor() {
    const action = this.isNew 
      ? this.donorService.createDonor(this.curDonor)
      : this.donorService.updateDonor(this.donorId, this.curDonor);

    action.subscribe({
      next: () => this.route.navigate(['/donors']),
      error: () => this.isNew ? this.showErrorCreate = true : this.showErrorUpdate = true
    });
  }

  cancel() {
    this.route.navigate(['/donors']);
  }

}
