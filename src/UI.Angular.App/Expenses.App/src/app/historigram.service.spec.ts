import { TestBed } from '@angular/core/testing';

import { HistorigramService } from './historigram.service';

describe('HistorigramService', () => {
  let service: HistorigramService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HistorigramService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
