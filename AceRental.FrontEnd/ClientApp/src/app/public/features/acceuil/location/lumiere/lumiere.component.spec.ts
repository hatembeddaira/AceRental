import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LumiereComponent } from './lumiere.component';

describe('LumiereComponent', () => {
  let component: LumiereComponent;
  let fixture: ComponentFixture<LumiereComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LumiereComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LumiereComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
