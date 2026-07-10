import { TestBed } from '@angular/core/testing';

import { User } from '../models/User.model';
import { Donor } from '../models/Donor.model';
import { DonorForm } from '../components/donor-form/donor-form';

describe('Donor', () => {
  let service: Donor;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    //service = TestBed.inject(DonorForm);
  });

  it('should be created', () => {
    //expect(service).toBeTruthy();
  });
});