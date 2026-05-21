import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SonorisationComponent } from './sonorisation.component';

describe('SonorisationComponent', () => {
  let component: SonorisationComponent;
  let fixture: ComponentFixture<SonorisationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SonorisationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SonorisationComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
