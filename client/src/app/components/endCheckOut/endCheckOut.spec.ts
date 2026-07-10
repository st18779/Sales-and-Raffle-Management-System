import { ComponentFixture, TestBed } from '@angular/core/testing';

import { endCheckOut } from './endCheckOut';

describe('endCheckOut', () => {
  let component: endCheckOut;
  let fixture: ComponentFixture<endCheckOut>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [endCheckOut]
    })
    .compileComponents();

    fixture = TestBed.createComponent(endCheckOut);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
