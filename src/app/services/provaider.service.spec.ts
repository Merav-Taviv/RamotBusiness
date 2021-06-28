import { TestBed } from '@angular/core/testing';

import { ProvaiderService } from './provaider.service';

describe('ProvaiderService', () => {
  let service: ProvaiderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProvaiderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
