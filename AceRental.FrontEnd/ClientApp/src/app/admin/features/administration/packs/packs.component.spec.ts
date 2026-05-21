import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PacksComponent } from './packs.component';

describe('PacksComponent', () => {
  let component: PacksComponent;
  let fixture: ComponentFixture<PacksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PacksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PacksComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
