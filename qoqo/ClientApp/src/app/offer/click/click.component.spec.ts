import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClickComponent } from './click.component';

describe('ClickComponent', () => {
  let component: ClickComponent;
  let fixture: ComponentFixture<ClickComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClickComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClickComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
