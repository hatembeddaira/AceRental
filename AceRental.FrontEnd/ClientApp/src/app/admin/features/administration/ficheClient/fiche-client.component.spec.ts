import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FicheClientComponent } from './fiche-client.component';

describe('FicheClientComponent', () => {
  let component: FicheClientComponent;
  let fixture: ComponentFixture<FicheClientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FicheClientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FicheClientComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
