import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewBeachComponent } from './view-beach.component';

describe('ViewBeachComponent', () => {
  let component: ViewBeachComponent;
  let fixture: ComponentFixture<ViewBeachComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewBeachComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewBeachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
