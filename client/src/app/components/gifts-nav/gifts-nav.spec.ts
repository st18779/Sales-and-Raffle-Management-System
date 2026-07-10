import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftsNav } from './gifts-nav';

describe('GiftsNav', () => {
  let component: GiftsNav;
  let fixture: ComponentFixture<GiftsNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftsNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftsNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
