import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthOrderComponent } from './auth-order.component';

describe('AuthOrderComponent', () => {
  let component: AuthOrderComponent;
  let fixture: ComponentFixture<AuthOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthOrderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
