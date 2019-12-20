import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBeachComponent } from './create-beach.component';

describe('CreateBeachComponent', () => {
  let component: CreateBeachComponent;
  let fixture: ComponentFixture<CreateBeachComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateBeachComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateBeachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
