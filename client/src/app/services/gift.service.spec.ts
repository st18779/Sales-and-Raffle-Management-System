import { TestBed } from '@angular/core/testing';

import { GiftService } from './gift.service';

describe('Gift', () => {
  let service: GiftService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GiftService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
