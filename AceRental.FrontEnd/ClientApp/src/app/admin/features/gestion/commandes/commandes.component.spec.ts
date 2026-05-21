import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommandesComponent } from './commandes.component';

describe('CommandesComponent', () => {
  let component: CommandesComponent;
  let fixture: ComponentFixture<CommandesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommandesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommandesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
