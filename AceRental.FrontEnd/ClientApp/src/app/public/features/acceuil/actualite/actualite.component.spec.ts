import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualiteComponent } from './actualite.component';

describe('ActualiteComponent', () => {
  let component: ActualiteComponent;
  let fixture: ComponentFixture<ActualiteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActualiteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActualiteComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
