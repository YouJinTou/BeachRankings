import { TestBed } from '@angular/core/testing';

import { BeachesService } from './beaches.service';

describe('BeachesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BeachesService = TestBed.get(BeachesService);
    expect(service).toBeTruthy();
  });
});
