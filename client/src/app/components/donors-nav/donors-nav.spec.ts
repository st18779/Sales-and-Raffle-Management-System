import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonorsNav } from './donors-nav';

describe('GiftsNav', () => {
  let component: DonorsNav;
  let fixture: ComponentFixture<DonorsNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonorsNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonorsNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
