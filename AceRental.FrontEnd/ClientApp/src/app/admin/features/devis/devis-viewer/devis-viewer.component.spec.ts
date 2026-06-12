import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevisViewerComponent } from './devis-viewer.component';

describe('DevisViewerComponent', () => {
  let component: DevisViewerComponent;
  let fixture: ComponentFixture<DevisViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DevisViewerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DevisViewerComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
