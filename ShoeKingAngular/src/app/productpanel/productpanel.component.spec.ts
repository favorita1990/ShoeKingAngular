import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductpanelComponent } from './productpanel.component';

describe('ProductpanelComponent', () => {
  let component: ProductpanelComponent;
  let fixture: ComponentFixture<ProductpanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductpanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductpanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
