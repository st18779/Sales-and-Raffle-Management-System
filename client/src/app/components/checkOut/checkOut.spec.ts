import { ComponentFixture, TestBed } from '@angular/core/testing';

import { checkOut } from './checkOut';

describe('checkOut', () => {
  let component: checkOut;
  let fixture: ComponentFixture<checkOut>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [checkOut]
    })
    .compileComponents();

    fixture = TestBed.createComponent(checkOut);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
