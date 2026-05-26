import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FicheProduitComponent } from './equipment.component';

describe('FicheProduitComponent', () => {
  let component: FicheProduitComponent;
  let fixture: ComponentFixture<FicheProduitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FicheProduitComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FicheProduitComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
